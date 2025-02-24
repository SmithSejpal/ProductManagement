using AutoMapper;
using ProductManagement.API.ApiModels.ActionResult;
using ProductManagement.Core;
using ProductManagement.Core.Entities;
using ProductManagement.Core.ServicesResult;

namespace ProductManagement.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Result, ActionResult>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.Success))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<ProductTransactionServiceResult, ProductTransactionActionResult>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
