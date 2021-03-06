﻿namespace Panda.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public List<Package> Packages { get; set; } = new List<Package>();

        public List<Receipt> Receipts { get; set; } = new List<Receipt>();
    }
}