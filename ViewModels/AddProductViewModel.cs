using System;
using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    /// <summary>
    /// View model for adding a new product and displaying existing products.
    /// </summary>
    public class AddProductViewModel
    {
        /// <summary>
        /// Gets or sets the list of existing products.
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the list of available product categories.
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the input model for a new product.
        /// </summary>
        public ProductInputModel NewProduct { get; set; } = new ProductInputModel();
    }

    /// <summary>
    /// Input model for creating a new product.
    /// </summary>
    public class ProductInputModel
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        [Required]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the production date of the product.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }
}