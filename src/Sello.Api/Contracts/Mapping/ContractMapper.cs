using AutoMapper;
using Sello.Datastore.SQL.Model;

namespace Sello.Api.Contracts.Mapping
{
    public class ContractMapper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductContract>()
                    .ForMember(product => product.Id, memberOptions => memberOptions.MapFrom(databaseEntity => databaseEntity.ExternalId))
                    .ForSourceMember(product => product.Id, memberOptions => memberOptions.Ignore())
                    .ForSourceMember(product => product.Orders, memberOptions => memberOptions.Ignore());
                cfg.CreateMap<ProductContract, Product>()
                    .ForMember(product => product.ExternalId, memberOptions => memberOptions.MapFrom(contract => contract.Id))
                    .ForMember(product => product.Id, memberOptions => memberOptions.Ignore())
                    .ForMember(product => product.Orders, memberOptions => memberOptions.Ignore());
                cfg.CreateMap<OrderContract, Order>()
                    .ForMember(order => order.Id, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.ConfirmationId, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.CustomerId, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.Customer, memberOptions => memberOptions.MapFrom(contract => contract.Customer));
                cfg.CreateMap<CustomerContract, Customer>()
                    .ForMember(customer => customer.Id, memberOptions => memberOptions.Ignore());
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}