using AutoMapper;
using OutOfOffice.Application.Dto.Employees;
using OutOfOffice.Application.Dto.LeaveRequests;
using OutOfOffice.Application.Dto.Projects;
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
            CreateMap<Employee, EmployeeCreateDto>()
            .ReverseMap();
            CreateMap<Employee, EmployeeUpdateDto>()
            .ReverseMap();

            CreateMap<LeaveRequest, LeaveRequestIndexDto>()
            .ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestCreateDto>()
            .ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestUpdateDto>()
            .ReverseMap();

            CreateMap<Project, ProjectIndexDto>()
            .ReverseMap();
            CreateMap<Project, ProjectCreateDto>()
            .ReverseMap();
            CreateMap<Project, ProjectUpdateDto>()
            .ReverseMap();
        }
    }
}
