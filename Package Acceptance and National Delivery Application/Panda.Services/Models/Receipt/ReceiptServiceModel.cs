namespace Panda.Services.Models.Receipt
{
    using AutoMapper;
    using Panda.Common.Mapping;
    using Panda.Models;
    using System;

    public class ReceiptServiceModel : IMapFrom<Receipt>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public string RecipientName { get; set; }

        public void CreateMapping(IMapperConfigurationExpression configuration)
            => configuration.CreateMap<Receipt, ReceiptServiceModel>()
                .ForMember(src => src.RecipientName, cfg => cfg.MapFrom(dest => dest.Recipient.UserName));
    }
}