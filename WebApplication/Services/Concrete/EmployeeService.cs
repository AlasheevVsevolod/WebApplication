using System;
using System.Collections.Generic;
using AutoMapper;
using WebApplication.Models;
using WebApplication.Repository.Interface;
using WebApplication.Services.Interface;

namespace WebApplication.Services.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges)
        {
            var employees = _repository.Employee.GetAllEmployees(trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public EmployeeDto GetEmployeeById(Guid employeeId, bool trackChanges)
        {
            var employee = _repository.Employee.GetEmployeeById(employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public EmployeeDto GetEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employee = _repository.Employee.GetEmployeeForCompany(companyId, employeeId, trackChanges: false);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public IEnumerable<EmployeeDto> GetAllEmployeesForCompany(Guid companyId, bool trackChanges)
        {
            var employees = _repository.Employee.GetAllEmployeesForCompany(companyId, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }
    }
}
