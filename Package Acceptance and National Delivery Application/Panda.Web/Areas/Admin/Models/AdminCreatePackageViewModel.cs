namespace Panda.Web.Areas.Admin.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class AdminCreatePackageViewModel
    {
        public string Description { get; set; }

        public double Weigth { get; set; }

        public string ShippingAddress { get; set; }

        public string RecipientId { get; set; }

        public IEnumerable<SelectListItem> AllUsers { get; set; }
    }
}