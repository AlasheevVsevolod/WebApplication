using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync(trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId, bool trackChanges)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<EmployeeDto> GetEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employee = await _employeeRepository.GetEmployeeForCompanyAsync(companyId, employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesForCompanyAsync(Guid companyId, bool trackChanges)
        {
            var employees = await _employeeRepository.GetAllEmployeesForCompanyAsync(companyId, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreationDto employee, Guid companyId)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);
            employeeEntity.CompanyId = companyId;

            _employeeRepository.CreateEmployee(employeeEntity);
            await _repositoryManager.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByIdsAsync(IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            var employees = await _employeeRepository.GetEmployeesByIdsAsync(employeeIds, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, trackChanges: false);
            if (existingEmployee == null)
            {
                return;
            }

            _employeeRepository.DeleteEmployee(existingEmployee);
            await _repositoryManager.Save();
        }

        public async Task UpdateEmployeeAsync(EmployeeForUpdateDto employee, Guid employeeId)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, trackChanges: true);
            _mapper.Map(employee, existingEmployee);

            await _repositoryManager.Save();
        }

        public async Task PatchEmployeeAsync(Guid employeeId, JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, trackChanges: true);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

            patchDoc.ApplyTo(employeeToPatch);

            _mapper.Map(employeeToPatch, employee);

            await _repositoryManager.Save();
        }
    }
}
