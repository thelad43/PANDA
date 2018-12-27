namespace Panda.Services.Models.Admin
{
    using AutoMapper;
    using Panda.Common.Mapping;
    using Panda.Models;
    using System;

    public class AdminPackageServiceModel : IMapFrom<Package>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public string RecipientName { get; set; }

        public void CreateMapping(IMapperConfigurationExpression configuration)
            => configuration.CreateMap<Package, AdminPackageServiceModel>()
                .ForMember(src => src.RecipientName, cfg => cfg.MapFrom(dest => dest.Recipient.UserName));
    }
}