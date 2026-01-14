using Microsoft.AspNetCore.Identity;
using System;

namespace SUE.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PostCode { get; set; }
    }
}