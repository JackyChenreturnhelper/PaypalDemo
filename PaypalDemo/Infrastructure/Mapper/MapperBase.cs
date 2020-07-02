using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.Mapper
{
    public abstract class MapperBase
    {
        public virtual void Assign<TSource, TDestination>(IMapper mapper, TSource src, TDestination des)
        {
            mapper.Map<TSource, TDestination>(src, des);
        }

        public virtual TDestination Create<TSource, TDestination>(IMapper mapper, TSource src)
        {
            return mapper.Map<TSource, TDestination>(src);
        }
    }
}