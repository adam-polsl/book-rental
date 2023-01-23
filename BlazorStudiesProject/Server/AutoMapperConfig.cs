using AutoMapper;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared.Dtos;
using BlazorStudiesProject.Shared.Dtos.Book;

namespace BlazorStudiesProject.Server
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<BookEntity, BookDto>()
                .ForMember(x => x.Content, o => o.MapFrom(e => e.Content.Text))
                .ForMember(x => x.Price, o => o.MapFrom(e => e.AdditionalInfo.Price))
                .ForMember(x => x.Description, o => o.MapFrom(e => e.AdditionalInfo.Description));

            CreateMap<AdditionalInfoEntity, AdditionalInfoDto>();
            CreateMap<ApplicationUser, UserAdminViewDto>().ForMember(x => x.UserId, o => o.MapFrom(u => u.Id));
            CreateMap<BookEntity, DisplayBookDto>();
            CreateMap<BookDto, BookEntity>().ForMember(x => x.Content, o => o.Ignore());
            CreateMap<ActivePurchaseEntity, ArchivePurchaseEntity>().ForMember(x => x.Id, o => o.Ignore());

            CreateMap<BookUserInfoEntity, BookUserInfoDto>();
            CreateMap<ContentEntity, ContentDto>();
            CreateMap<CodeEntity, CreateCodeDto>();
            CreateMap<CreateCodeDto, CodeEntity>().ForMember(x => x.MoneyPaid, o => o.MapFrom(c => c.MoneyPaid));

        }
    }
}
