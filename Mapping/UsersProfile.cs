﻿using AutoMapper;
using RETOAPI.DTOs;
using RETOAPI.Models;
using RETOAPI.Services;
using System.Security.Cryptography;
using System.Text;


namespace RETOAPI.Mapping
{
    public class UsersProfile:Profile
    {
        private readonly ServiceCredentials _serviceCredentials;
        public UsersProfile(ServiceCredentials serviceCredentials) { 
            _serviceCredentials = serviceCredentials;
        }
        public UsersProfile() {
            
            //CreateMap<Users, UserList>();
            CreateMap<Users, UserList>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRols.FirstOrDefault().Rols.RolName));
            CreateMap<UserCreate, Users>();
            CreateMap<UserUpdate,Users>();
        }
        
    }
}
