using Application.DTO.Response;
using Application.DTO.Resquest.Identity;
using Application.Interface.Identity;
using Azure.Identity;
using infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repository
{
    public class Account
        (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context) : IAccount
    {
        public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model) 
        {
            var user = await FindUserByEmail(model.Email);
            if (user == null)
                return new ServiceResponse(false, "User already exist");

            var newUser = new ApplicationUser()
            {
                Username = model.Email,
                PasswordHash = model.Password,
                Email = model.Email,
                Name = model.Name
            };
            var result = CheckResult(await userManager.CreateAsync(newUser, model.Password));
            if (!result.Flag)
                return result;
            else
                return await CreateUserAsync(model);
        }

        private async Task<ServiceResponse> CreateUserClaims(CreateUserRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Policy)) return new ServiceResponse(false, "No policy specified");
            Claim[] userClaims = [];
            if (model.Policy.Equals(AdminPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims =
                    [
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("Name", model.Name),
                        new Claim("Create", "true"),
                        new Claim("Update", "true"),
                        new Claim("Delete", "true"),
                        new Claim("Read", "true"),
                        new Claim("ManageUser", "true")
                    ];
            }
            else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase)) 
            {
                userClaims =
                    [
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("Name", model.Name),
                        new Claim("Create", "true"),
                        new Claim("Update", "true"),
                        new Claim("Delete", "true"),
                        new Claim("Read", "true"),
                        new Claim("ManageUser", "true")
                    ];
            }
        }
    }
}
