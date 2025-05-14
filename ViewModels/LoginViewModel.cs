using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.ViewModels;

/// <summary>
/// View model for the login form.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <remarks>
    /// This property is required and must be a valid email address.
    /// </remarks>
    [Required, EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <remarks>
    /// This property is required and is treated as a password field.
    /// </remarks>
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
}