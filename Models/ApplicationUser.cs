using Diplom.Controllers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Diplom.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfilePicture { get; set; }
        
        public ICollection<Menu> Menus { get; set; }

        public ICollection<Vault> Vaults { get; set; }
    }
}
