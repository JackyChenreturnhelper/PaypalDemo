using AutoMapper;
using PaypalDemo.Infrastructure.Helper;
using PaypalDemo.Models;
using PaypalDemo.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.Mapper
{
    public class AddApiTransactionMapper : MapperBase
    {
        private static readonly IMapper addApiTransactionMapper;

        static AddApiTransactionMapper()
        {
            addApiTransactionMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddApiTransactionRequest, ApiTransaction>()
                    .ForMember(des => des.apiId, o => o.MapFrom(src => src.apiId))
                    .ForMember(des => des.headId, o => o.MapFrom(src => src.apiId))
                    .ForMember(des => des.createBy, o => o.MapFrom(src => src.createBy));

                cfg.CreateMap<ApiTransaction, AddApiTransactionResponse>();
                cfg.CreateMap<ApiTransaction, ApiTransaction>();
            }).CreateMapper();
        }

        public ApiTransaction CreateApiTransaction(
            AddApiTransactionRequest addApiTransactionRequest)
        {
            ApiTransaction apiTransaction =
                Create<AddApiTransactionRequest, ApiTransaction>(addApiTransactionMapper,
                    addApiTransactionRequest);
            return apiTransaction;
        }

        public ApiTransaction CreateRefundApiTransaction(ApiTransaction src)
        {
            ApiTransaction apiTransaction =
                Create<ApiTransaction, ApiTransaction>(addApiTransactionMapper, src);

            apiTransaction.transactionType = ApiTransactionType.rsl.GetDescription();
            apiTransaction.amount *= -1;
            apiTransaction.notes = $"refund for cancel:{src.transactionNumber}";
            return apiTransaction;
        }

        public void AssignAddApiTransactionResponse(ApiTransaction src, AddApiTransactionResponse des)
        {
            Assign<ApiTransaction, AddApiTransactionResponse>(addApiTransactionMapper, src, des);
        }
    }
}