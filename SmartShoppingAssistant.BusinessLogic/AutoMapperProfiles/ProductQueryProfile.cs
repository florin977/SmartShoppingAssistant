using AutoMapper;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    // Mapping from business logic to data access layer, might need to rework this
    public class ProductQueryProfile : Profile
    {
        public ProductQueryProfile() 
        {
            CreateMap<DTOs.QueryDTOs.ProductQueryDTO, DataAccess.Repository.Parameters.ProductQueryParameters>();
        }
    }
}
