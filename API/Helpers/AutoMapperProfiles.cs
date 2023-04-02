using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url));
            
            // display student role
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(p => p.Role.Name)));
            
             // display student role
            // CreateMap<DepartmentCreationDTO, MemberDto>()
            //    .ForMember(dest1 => dest1.DeptName, opt1 => opt1.MapFrom(src1 =>
            //         src1.DepartmentName.ToString()));
          
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<PetitionDto, Petition>();
        }
    }
}