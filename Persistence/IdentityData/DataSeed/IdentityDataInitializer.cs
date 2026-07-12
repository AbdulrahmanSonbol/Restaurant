using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Admin", NormalizedName = "ADMIN" });
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new User()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Abdulrahman",
                        LastName = "Sonbol",
                        UserName = "AbdulrahmanSonbol",
                        Email = "AbdulrahmanSonbol@gmail.com",
                        PhoneNumber = "01000000000",
                        EmailConfirmed = true 
                    };

                    var result = await _userManager.CreateAsync(user01, "P@ssw0rd123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user01, "Admin");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogWarning($"User creation failed: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding Identity Database: Message = {ex.Message}");
            }
        }
    }
    
}
