﻿using AutoMapper;
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
                    .ForSourceMember(product => product.Id, memberCfg => memberCfg.Ignore())
                    .ForSourceMember(product => product.Orders, memberCfg => memberCfg.Ignore());
                cfg.CreateMap<ProductContract, Product>()
                    .ForMember(product => product.Id, memberOptions => memberOptions.Ignore())
                    .ForMember(product => product.Orders, memberOptions => memberOptions.Ignore());
                cfg.CreateMap<OrderContract, Order>()
                    .ForMember(order => order.Id, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.ConfirmationId, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.CustomerId, memberOptions => memberOptions.Ignore())
                    .ForMember(order => order.Products, memberOptions => memberOptions.MapFrom(contract => contract.Products))
                    .ForMember(order => order.Customer, memberOptions => memberOptions.MapFrom(contract => contract.Customer));
                cfg.CreateMap<CustomerContract, Customer>()
                    .ForMember(customer => customer.Id, memberOptions => memberOptions.Ignore());
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}