using System;
using System.Collections.Generic;
using AutoMapper;
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

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            var companies = _companyRepository.GetAllCompanies(trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public CompanyDto GetCompanyById(Guid companyId, bool trackChanges)
        {
            var company = _companyRepository.GetCompanyById(companyId, trackChanges);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _companyRepository.CreateCompany(companyEntity);
            _repositoryManager.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        public IEnumerable<CompanyDto> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            var companies = _companyRepository.GetCompaniesByIds(companyIds, trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public void DeleteCompany(Guid companyId)
        {
            var existingCompany = _companyRepository.GetCompanyById(companyId, trackChanges: false);
            if (existingCompany == null)
            {
                return;
            }

            _companyRepository.DeleteCompany(existingCompany);
            _repositoryManager.Save();
        }

        public void UpdateCompany(CompanyForUpdateDto company, Guid companyId)
        {
            var existingCompany = _companyRepository.GetCompanyById(companyId, trackChanges: true);
            _mapper.Map(company, existingCompany);

            _repositoryManager.Save();
        }
    }
}
