﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebApplication.Infrastructure.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();
        }
    }
}