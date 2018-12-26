namespace Panda.Common.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void CreateMapping(IMapperConfigurationExpression configuration);
    }
}