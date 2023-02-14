using AutoMapper;
using Vueling.Otd.Application.CurrencyPair.DTOs;
using Vueling.Otd.Application.Transaction.DTOs;
using CurrencyPairModels = Vueling.Otd.Domain.CurrencyPair.Models;
using TransactionModels = Vueling.Otd.Domain.Transaction.Models;

namespace Vueling.Otd.Application
{
    public class ConfigureMapping : Profile
    {
        public ConfigureMapping()
        {
            // Currency pair
            CreateMap<CurrencyPairDTO, CurrencyPairModels::CurrencyPair>().ReverseMap();
            CreateMap<CurrencyPairListDTO, CurrencyPairModels::CurrencyPairList>().ReverseMap();

            // Transaction
            CreateMap<TransactionDTO, TransactionModels::Transaction>().ReverseMap();
            CreateMap<TransactionListDTO, TransactionModels::TransactionList>().ReverseMap();
        }
    }
}