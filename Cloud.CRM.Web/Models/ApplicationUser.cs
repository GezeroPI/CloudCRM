using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.CRM.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public ApplicationUser(string userName, string email) : base(userName)
        {
            Email = email;
        }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Language")]
        public string Language { get; set; }
        public Company Company { get; set; }
        public ApplicationUser Collaborator { get; set; }

        //overrides
        [Required]
        [UIHint("password")]
        [Display(Name = "Password")]
        public override string PasswordHash { get; set; }
        
        [UIHint("email")]
        [Display(Name = "Email")]
        public override string Email { get; set; }
        [UIHint("phone")]
        [Display(Name = "Phone")]
        public override string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Username")]
        public override string UserName { get; set; }
        [Display(Name = "Two Factor Authentication")]
        [UIHint("checkbox")]
        public override bool TwoFactorEnabled { get; set; }
    }
}
