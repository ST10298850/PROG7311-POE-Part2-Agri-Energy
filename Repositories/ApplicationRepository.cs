using System;
using System.Collections.Generic;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _context;

    public ApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FarmerApplication>> GetAllAsync() =>
        await _context.FarmerApplications.ToListAsync();

    public async Task<FarmerApplication?> GetByIdAsync(int id) =>
        await _context.FarmerApplications.FindAsync(id);

    public async Task UpdateStatusAsync(int id, string status)
    {
        var app = await _context.FarmerApplications.FindAsync(id);
        if (app != null)
        {
            app.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
