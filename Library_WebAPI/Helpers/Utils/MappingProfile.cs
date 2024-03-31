using AutoMapper;
using Library_WebAPI.DTOs;
using Library_WebAPI.Models;

namespace Library_WebAPI.Helpers.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TblUser, UserDTO>()
                .ForMember(res => res.id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(res => res.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(res => res.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(res => res.email, opt => opt.MapFrom(src => src.EmailId))
                .ForMember(res => res.contact_no, opt => opt.MapFrom(src => src.ContactNumber))
                .ForMember(res => res.role, opt => opt.MapFrom(src => src.UserRole))
                .ForMember(res => res.user_name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(res => res.password, opt => opt.MapFrom(src => src.Password))
                .ForMember(res => res.isActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(res => res.created_at, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(res => res.created_by, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(res => res.updated_at, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(res => res.updated_by, opt => opt.MapFrom(src => src.UpdatedBy));

            CreateMap<TblBook, BooksDTO>()
                .ForMember(res => res.id, opt => opt.MapFrom(src => src.BookId))
                .ForMember(res => res.title, opt => opt.MapFrom(src => src.Title))
                .ForMember(res => res.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(res => res.books_count, opt => opt.MapFrom(src => src.BookCount))
                .ForMember(res => res.isActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(res => res.created_at, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(res => res.created_by, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(res => res.updated_at, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(res => res.updated_by, opt => opt.MapFrom(src => src.UpdatedBy));
        }
    }
}
