using AutoMapper;
using OutOfOffice.Application.Dto.Employees;
using OutOfOffice.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeIndexDto>()
            .ReverseMap();
        }
        
    }
}
