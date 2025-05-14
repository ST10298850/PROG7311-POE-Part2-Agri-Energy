using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    /// <summary>
    /// View model for the farmer application form.
    /// </summary>
    public class FarmerApplicationViewModel
    {
        /// <summary>
        /// Gets or sets the name of the farm.
        /// </summary>
        [Required]
        public string FarmName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location of the farm.
        /// </summary>
        [Required]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of farm (e.g., crop, livestock, mixed).
        /// </summary>
        [Required]
        public string FarmType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the farmer applicant.
        /// </summary>
        [Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the farmer applicant.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number of the farmer applicant.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the applicant has accepted the terms and conditions.
        /// </summary>
        public bool TermsAccepted { get; set; }
    }
}