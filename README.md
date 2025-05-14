# AgriEnergyConnect

AgriEnergyConnect is a role-based ASP.NET Core MVC web platform designed to connect South African farmers with green energy solution providers. It allows farmers to apply for inclusion, and employees (e.g., from sustainability or agri-tech companies) to manage these applications and maintain a catalog of green energy products.

---

## Project Purpose

The platform bridges the gap between agriculture and renewable energy. It streamlines farmer onboarding and allows employees to track and manage sustainable energy solutions. The system uses role-based access control and entity management to maintain security and efficiency.

---

## Key Features

### Authentication & Authorization

- **Role-based access**: Two roles - `Farmer` and `Employee`
- **Login functionality** for both roles
- **Only Employees can create and manage accounts**

### Public Application Form

- Accessible without login
- Allows farmers to apply by submitting:
  - Farm Name
  - Location
  - Type of Farm
  - Farmer Name
  - Contact Info

### Employee Dashboard

- View **all pending farmer applications**
- **Approve or reject** applications
- **Manage products** (CRUD):
  - Add green energy products (e.g., solar panels, water systems)
  - Edit/Delete entries
- **Filter products** by:
  - Category
  - Date range
  - Submitted by (UserID)

### Farmer Dashboard (post-approval)

- View available green energy products
- Browse by category
- Cannot modify system content (read-only access)

---

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap, HTML, CSS, JavaScript
- ASP.NET Identity for Authentication
- LINQ for filtering and querying

---

## Getting Started

### Prerequisites

- **Visual Studio 2022** or newer
- **.NET 8 SDK**
- **SQL Server LocalDB**

---

## Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone
   ```

https://github.com/ST10298850/PROG7311-POE-Part2-Agri-Energy.git

cd AgriEnergyConnect

````

1. Use Get-Packages and intall;
   ```
   Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.16
   ```
2. Then use Update-Database in Package Console Manager
3. Dotnet Run

2. **Open the solution in Visual Studio.**

3. **Set up the database connection in `appsettings.json`:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AgriEnergyConnectDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
````

4. **Open the Package Manager Console** (in Visual Studio):

   - Select `Tools > NuGet Package Manager > Package Manager Console`
   - Then run the following command to install the EF Core tools:
     ```powershell
     Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.16
     ```

5. **Run database migration:**

   ```powershell
   Update-Database
   ```

6. **Start the application (F5 or Ctrl+F5).**

---

## Seeded Employee Account (Default Login)

An Employee account is seeded automatically with the following credentials:

- **Email:** `employee@agrienergy.com`
- **Password:** `Admin@123`

Use this account to log in as an Employee and begin managing the system.

---

## Project Structure

```
AgriEnergyConnect/
│
├── Controllers/          # Handles logic for Employees, Farmers, Account
├── Models/               # Entity classes (ApplicationForm, Product, etc.)
├── Views/                # Razor Views (structured by controller)
├── Data/                 # Database context and seeding logic
├── Services/             # (Optional) Business logic abstraction
├── wwwroot/              # Static files (CSS, JS, Images)
├── appsettings.json      # Configurations
└── Program.cs            # App entry point and middleware setup
```
