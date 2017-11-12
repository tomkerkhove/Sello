using AutoMapper;

namespace Sello.Api.Contracts.Mapping
{
    public class ContractMapper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Sello.Datastore.SQL.Model.Product, Sello.Domain.Model.Product>();
                cfg.CreateMap<Sello.Domain.Model.Product, Sello.Api.Contracts.ProductContract>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}