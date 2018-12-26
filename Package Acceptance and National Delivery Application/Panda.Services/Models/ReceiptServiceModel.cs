namespace Panda.Services.Models
{
    using System;

    public class ReceiptServiceModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public string RecipientName { get; set; }
    }
}