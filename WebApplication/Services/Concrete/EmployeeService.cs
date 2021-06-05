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
            var employee = _repository.Employee.GetEmployeeForCompany(companyId, employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public IEnumerable<EmployeeDto> GetAllEmployeesForCompany(Guid companyId, bool trackChanges)
        {
            var employees = _repository.Employee.GetAllEmployeesForCompany(companyId, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public EmployeeDto CreateEmployee(EmployeeForCreationDto employee, Guid companyId)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);
            employeeEntity.CompanyId = companyId;

            _repository.Employee.CreateEmployee(employeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public IEnumerable<EmployeeDto> GetEmployeesByIds(IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            var employees = _repository.Employee.GetEmployeesByIds(employeeIds, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }
    }
}
