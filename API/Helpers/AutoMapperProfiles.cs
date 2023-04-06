using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DepartmentDto, Department>().ReverseMap();
            CreateMap<Department, DepartmentCreationDTO>();
            CreateMap<DepartmentDisplayDto, Department>();
            CreateMap<Department, DepartmentDisplayDto>();
            CreateMap<DepartmentDto, DepartmentCreationDTO>();
            CreateMap<IEnumerable<DepartmentDisplayDto>, IEnumerable<Department>>();
CreateMap<IEnumerable<Department>, IEnumerable<DepartmentDisplayDto>>();





            CreateMap<Department, DepartmentNameDto>();

           // CreateMap<MemberDto, FeedbackDto>().ReverseMap();

            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url));
            
            // display student role
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(p => p.Role.Name)));
                        
            CreateMap<AppUser, useremailDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));






            // CreateMap<AppUser, MemberDto>()
            //     .ForMember(dest => dest.Fback, opt => opt.MapFrom(src =>
            //         src.Feedbacks.Select(p => p.Content)));
            // CreateMap<AppUser, MemberDto>()
            //     .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks.Select(fb => fb)));
            // CreateMap<AppUser, MemberDto>()
            //     .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src =>
            //         src.Feedbacks.Where(f => f.SenderId == src.Id)
            //             .Select(f => f.Content)))
            //      .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
            //         src.UserRoles.Select(p => p.Role.Name)))
            //     .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks.Select(fb => fb.Content)));





            
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));

                        
            //  CreateMap<FeedbackCreateDto, Feedback>()
            //     .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
            //     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            //     .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
            
          //  CreateMap<AppUser, MemberDto>();
          //  CreateMap<Department, DepartmentDto>();
           // CreateMap<UserDto, AppUser>();
            CreateMap<AppUser, UserDto>();

            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));




            



        
            
             // display student role
            // CreateMap<DepartmentCreationDTO, MemberDto>()
            //    .ForMember(dest1 => dest1.DeptName, opt1 => opt1.MapFrom(src1 =>
            //         src1.DepartmentName.ToString()));
          
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<PetitionDto, Petition>();
            CreateMap<FeedbackCreateDto, Feedback>();
            CreateMap<FeedbackDto, Feedback>().ReverseMap();
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
            CreateMap<FeedbackDto, AppUser>().ReverseMap();
            CreateMap<MemberDto, FeedbackDto>().ReverseMap();
            CreateMap<String , FeedbackDto>().ReverseMap();
              CreateMap<String , Feedback>().ReverseMap();
        }
    }
}