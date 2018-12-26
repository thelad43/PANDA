namespace Panda.Services.Models.Package
{
    using AutoMapper;
    using Panda.Common.Mapping;
    using Panda.Models;
    using Panda.Models.Enums;
    using System;

    public class PackageDetailsServiceModel : IMapFrom<Package>, IHaveCustomMapping
    {
        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime EstimatedDeliveryTime { get; set; }

        public string RecipientName { get; set; }

        public void CreateMapping(IMapperConfigurationExpression configuration)
            => configuration.CreateMap<Package, PackageDetailsServiceModel>()
                .ForMember(src => src.RecipientName, cfg => cfg.MapFrom(dest => dest.Recipient.UserName));
    }
}