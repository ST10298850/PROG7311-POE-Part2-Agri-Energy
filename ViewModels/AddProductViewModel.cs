using System;
using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class AddProductViewModel
    {
        public List<Product> Products { get; set; }

        public ProductInputModel NewProduct { get; set; } = new ProductInputModel();
    }

    public class ProductInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }
}