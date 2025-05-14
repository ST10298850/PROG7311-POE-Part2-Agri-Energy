using AgriEnergyConnect.Repositories;
using AgriEnergyConnect.Services;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFarmService, FarmService>();

var app = builder.Build();

// Seed roles/users with a proper scope
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await SeedDatabase(services);
        logger.LogInformation("Data seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
        throw;
    }
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

async Task SeedDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        // Seed Roles
        foreach (var role in SeedData.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Seed Admin User
        var admin = await userManager.FindByEmailAsync(SeedData.AdminUser.Email);
        if (admin == null)
        {
            admin = new User
            {
                UserName = SeedData.AdminUser.Email,
                Email = SeedData.AdminUser.Email,
                Role = SeedData.AdminUser.Role
            };
            var result = await userManager.CreateAsync(admin, SeedData.AdminUser.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, SeedData.AdminUser.Role);
            }
            else
            {
                logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Seed Farmers and Farms
        foreach (var (Email, Password, FarmName, Location, FarmType) in SeedData.Farmers)
        {
            var farmer = await userManager.FindByEmailAsync(Email);
            if (farmer == null)
            {
                farmer = new User
                {
                    UserName = Email,
                    Email = Email,
                    Role = "Farmer"
                };
                var result = await userManager.CreateAsync(farmer, Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmer, "Farmer");
                }
                else
                {
                    logger.LogError("Failed to create farmer user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    continue;
                }
            }

            var farm = await context.Farms.FirstOrDefaultAsync(f => f.FarmName == FarmName && f.UserID == farmer.Id);
            if (farm == null)
            {
                farm = new Farm
                {
                    FarmName = FarmName,
                    Location = Location,
                    FarmType = FarmType,
                    UserID = farmer.Id
                };
                context.Farms.Add(farm);
            }
            else
            {
                farm.Location = Location;
                farm.FarmType = FarmType;
            }
        }

        // Seed Products
        foreach (var (Name, Description, Category, FarmName, UserEmail) in SeedData.Products)
        {
            var user = await userManager.FindByEmailAsync(UserEmail);
            if (user == null)
            {
                logger.LogWarning("User not found for product: {ProductName}", Name);
                continue;
            }

            var farm = await context.Farms.FirstOrDefaultAsync(f => f.FarmName == FarmName && f.UserID == user.Id);
            if (farm == null)
            {
                logger.LogWarning("Farm not found for product: {ProductName}", Name);
                continue;
            }

            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == Name && p.FarmID == farm.FarmID);
            if (product == null)
            {
                product = new Product
                {
                    Name = Name,
                    Description = Description,
                    Category = Category,
                    ProductionDate = DateTime.Now,
                    FarmID = farm.FarmID,
                    UserId = user.Id
                };
                context.Products.Add(product);
            }
            else
            {
                product.Description = Description;
                product.Category = Category;
                product.ProductionDate = DateTime.Now;
            }
        }

        // Seed FarmerApplications
        foreach (var (FarmName, Location, FarmType, FullName, Email, Phone, Status) in SeedData.FarmerApplications)
        {
            var application = await context.FarmerApplications.FirstOrDefaultAsync(a => a.Email == Email);
            if (application == null)
            {
                application = new FarmerApplication
                {
                    FarmName = FarmName,
                    Location = Location,
                    FarmType = FarmType,
                    FullName = FullName,
                    Email = Email,
                    Phone = Phone,
                    Status = Status,
                    SubmissionDate = DateTime.Now
                };
                context.FarmerApplications.Add(application);
            }
            else
            {
                application.FarmName = FarmName;
                application.Location = Location;
                application.FarmType = FarmType;
                application.FullName = FullName;
                application.Phone = Phone;
                application.Status = Status;
            }
        }

        // Seed UserDetails
        foreach (var (Email, FullName, Phone, Address) in SeedData.UserDetails)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var userDetail = await context.UserDetails.FirstOrDefaultAsync(ud => ud.UserID == user.Id);
                if (userDetail == null)
                {
                    userDetail = new UserDetail
                    {
                        UserID = user.Id,
                        FullName = FullName,
                        Phone = Phone,
                        Address = Address
                    };
                    context.UserDetails.Add(userDetail);
                }
                else
                {
                    userDetail.FullName = FullName;
                    userDetail.Phone = Phone;
                    userDetail.Address = Address;
                }
            }
            else
            {
                logger.LogWarning("User not found for UserDetails: {Email}", Email);
            }
        }

        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
        throw;
    }
}

//Use this to swap to my own database
//"ConnectionStrings": {
//  "DefaultConnection": "Server=localhost,1433;Database=AgriEnergyConnectDB;User Id=sa;Password=AgriStrong123!;TrustServerCertificate=True;"
// },