namespace Panda.Services.Models.Receipt
{
    using AutoMapper;
    using Panda.Common.Mapping;
    using Panda.Models;
    using System;

    public class ReceiptDetailsServiceModel : IMapFrom<Receipt>, IHaveCustomMapping
    {
        public int ReceiptId { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime IssuedOn { get; set; }

        public string RecipientName { get; set; }

        public decimal Fee { get; set; }

        public void CreateMapping(IMapperConfigurationExpression configuration)
            => configuration.CreateMap<Receipt, ReceiptDetailsServiceModel>()
                .ForMember(src => src.ReceiptId, cfg => cfg.MapFrom(dest => dest.Id))
                .ForMember(src => src.RecipientName, cfg => cfg.MapFrom(dest => dest.Recipient.UserName))
                .ForMember(src => src.Weight, cfg => cfg.MapFrom(dest => dest.Package.Weight))
                .ForMember(src => src.Description, cfg => cfg.MapFrom(dest => dest.Package.Description))
                .ForMember(src => src.ShippingAddress, cfg => cfg.MapFrom(dest => dest.Package.ShippingAddress));
    }
}