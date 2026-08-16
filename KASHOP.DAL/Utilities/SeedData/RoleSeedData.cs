using KASHOP.DAL.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Utilities.SeedData
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task SeedRole()
        {
            var roles = new List<string>
            {
                RoleConstant.Admin,
                RoleConstant.User
            };
            if (!await _roleManager.Roles.AnyAsync()) {
                foreach (var role in roles)
                {
                   await _roleManager.CreateAsync(new IdentityRole(role));
                    
                }
            }
        }
    }
}
