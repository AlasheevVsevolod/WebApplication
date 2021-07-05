using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication.Models;
using WebApplication.Repository.Interface;
using WebApplication.Services.Interface;

namespace WebApplication.Services.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _employeeRepository = _repositoryManager.Employee;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges)
        {
            var employees = _employeeRepository.GetAllEmployees(trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public EmployeeDto GetEmployeeById(Guid employeeId, bool trackChanges)
        {
            var employee = _employeeRepository.GetEmployeeById(employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public EmployeeDto GetEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employee = _employeeRepository.GetEmployeeForCompany(companyId, employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public IEnumerable<EmployeeDto> GetAllEmployeesForCompany(Guid companyId, bool trackChanges)
        {
            var employees = _employeeRepository.GetAllEmployeesForCompany(companyId, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public EmployeeDto CreateEmployee(EmployeeForCreationDto employee, Guid companyId)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);
            employeeEntity.CompanyId = companyId;

            _employeeRepository.CreateEmployee(employeeEntity);
            _repositoryManager.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public IEnumerable<EmployeeDto> GetEmployeesByIds(IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            var employees = _employeeRepository.GetEmployeesByIds(employeeIds, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public void DeleteEmployee(Guid employeeId)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(employeeId, trackChanges: false);
            if (existingEmployee == null)
            {
                return;
            }

            _employeeRepository.DeleteEmployee(existingEmployee);
            _repositoryManager.Save();
        }

        public void UpdateEmployee(EmployeeForUpdateDto employee, Guid employeeId)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(employeeId, trackChanges: true);
            _mapper.Map(employee, existingEmployee);

            _repositoryManager.Save();
        }

        public void PatchEmployee(Guid employeeId, JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            var employee = _employeeRepository.GetEmployeeById(employeeId, trackChanges: true);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

            patchDoc.ApplyTo(employeeToPatch);

            _mapper.Map(employeeToPatch, employee);

            _repositoryManager.Save();
        }
    }
}
