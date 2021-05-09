using AutoMapper;
using WebApplication.Models;

namespace WebApplication.Infrastructure.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                    o => o.MapFrom(c => $"{c.Address}, {c.Country}"));
        }
    }
}