using GameCave.Data;
using GameCave.Repositories;

namespace GameCave.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        public CompanyService(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Company> CreateAsync(Company company)
        {
            return await _companyRepository.CreateAsync(company);

        }

        public async Task<Company> DeleteAsync(int companyId)
        {
            return await _companyRepository.DeleteAsync(companyId);
        }

        public async Task<ICollection<Company>> GetAllCompaniesAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int companyId)
        {
            return await _companyRepository.GetByIdAsync(companyId);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            return await _companyRepository.UpdateAsync(company);
        }
    }
}
