using AutoMapper;

namespace SmartShoppingAssistant.BusinessLogic.AutoMapperProfiles
{
    // Mapping from business logic to data access layer, might need to rework this
    public class PaginationQueryProfile : Profile
    {
        public PaginationQueryProfile()
        {
            CreateMap<DTOs.QueryDTOs.PaginationQueryDTO, DataAccess.Repository.Parameters.PaginationParameters>();
        }
    }
}
