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
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _companyRepository = _repositoryManager.Company;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            var companies = await _companyRepository.GetAllCompaniesAsync(trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid companyId, bool trackChanges)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyId, trackChanges);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public CompanyDto GetCompanyById(Company existingCompany)
        {
            var companyDto = _mapper.Map<CompanyDto>(existingCompany);

            return companyDto;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _companyRepository.CreateCompany(companyEntity);
            await _repositoryManager.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            var companies = await _companyRepository.GetCompaniesByIdsAsync(companyIds, trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public async Task DeleteCompanyAsync(Company existingCompany)
        {
            _companyRepository.DeleteCompany(existingCompany);
            await _repositoryManager.Save();
        }

        public async Task UpdateCompanyAsync(CompanyForUpdateDto company, Company existingCompany)
        {
            _mapper.Map(company, existingCompany);

            await _repositoryManager.Save();
        }

        public async Task PatchCompanyAsync(Company existingCompany, JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            var companyToPatch = _mapper.Map<CompanyForUpdateDto>(existingCompany);

            patchDoc.ApplyTo(companyToPatch);

            _mapper.Map(companyToPatch, existingCompany);

            await _repositoryManager.Save();
        }
    }
}
