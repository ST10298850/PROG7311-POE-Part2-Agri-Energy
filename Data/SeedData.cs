using AgriEnergyConnect.Models;
using System;
using System.Collections.Generic;

namespace AgriEnergyConnect.Data
{
    public static class SeedData
    {
        public static readonly string[] Roles = { "Admin", "Employee", "Farmer" };

        public static readonly (string Email, string Password, string Role) AdminUser =
            ("admin@example.com", "Admin123$", "Employee");

        public static readonly List<(string Email, string Password, string FarmName, string Location, string FarmType)> Farmers = new()
        {
            ("sipho@farm.co.za", "Farmer123$", "Green Valley", "Free State", "Livestock"),
            ("maria@sunrise.com", "Farmer123$", "Sunrise Crops", "Mpumalanga", "Crop"),
            ("jabu@hilltop.co.za", "Farmer123$", "Hilltop Farm", "KwaZulu-Natal", "Dairy"),
            ("lerato@orchard.org", "Farmer123$", "Lerato Orchard", "Limpopo", "Fruit")
        };
        public static readonly List<(string Name, string Description, string Category, string FarmName, string UserEmail)> Products = new()
        {
            ("Beef Pack", "Grass-fed beef", "Meat", "Green Valley", "sipho@farm.co.za"),
            ("Maize Bag", "White maize, 50kg", "Grain", "Sunrise Crops", "maria@sunrise.com"),
            ("Cheese Wheel", "Aged cheddar", "Dairy", "Hilltop Farm", "jabu@hilltop.co.za"),
            ("Oranges Box", "Fresh navel oranges", "Fruit", "Lerato Orchard", "lerato@orchard.org")
        };

        public static readonly List<(string FarmName, string Location, string FarmType, string FullName, string Email, string Phone, string Status)> FarmerApplications = new()
        {
            ("Sunshine Farm", "Limpopo", "Mixed", "John Doe", "john@sunshinefarming.com", "0123456789", "Pending"),
            ("Green Fields", "Western Cape", "Crop", "Jane Smith", "jane@greenfields.com", "0987654321", "Pending"),
            ("Ocean Breeze", "Eastern Cape", "Fishery", "David Fisher", "david@oceanbreeze.net", "0214441122", "Pending"),
            ("Mountain View", "Northern Cape", "Livestock", "Nomsa Dube", "nomsa@mountainview.africa", "0712345678", "Pending")
        };

        public static readonly List<(string Email, string FullName, string Phone, string Address)> UserDetails = new()
        {
            ("sipho@farm.co.za", "Sipho Nkosi", "0111234567", "123 Farm Road, Free State"),
            ("maria@sunrise.com", "Maria van der Merwe", "0219876543", "456 Sunrise Avenue, Mpumalanga"),
            ("admin@example.com", "Admin User", "0300000000", "789 Admin Street, Gauteng"),
            ("jabu@hilltop.co.za", "Jabulani Mthembu", "0332223344", "321 Hilltop Lane, KwaZulu-Natal"),
            ("lerato@orchard.org", "Lerato Mokoena", "0156781234", "101 Orchard Way, Limpopo")
        };
    }
}