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

        public async Task DeleteCompanyAsync(Guid companyId)
        {
            var existingCompany = await _companyRepository.GetCompanyByIdAsync(companyId, trackChanges: false);
            if (existingCompany == null)
            {
                return;
            }

            _companyRepository.DeleteCompany(existingCompany);
            await _repositoryManager.Save();
        }

        public async Task UpdateCompanyAsync(CompanyForUpdateDto company, Guid companyId)
        {
            var existingCompany = await _companyRepository.GetCompanyByIdAsync(companyId, trackChanges: true);
            _mapper.Map(company, existingCompany);

            await _repositoryManager.Save();
        }

        public async Task PatchCompanyAsync(Guid companyId, JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyId, trackChanges: true);
            var companyToPatch = _mapper.Map<CompanyForUpdateDto>(company);

            patchDoc.ApplyTo(companyToPatch);

            _mapper.Map(companyToPatch, company);

            await _repositoryManager.Save();
        }
    }
}
