using GameCave.Data;

namespace GameCave.Services
{
    public interface ICompanyService
    {
        public Task<Company> GetCompanyByIdAsync(int companyId);
        public Task<ICollection<Company>> GetAllCompaniesAsync();
        public Task<Company> CreateAsync(Company company);
        public Task<Company> UpdateAsync(Company company);
        public Task<Company> DeleteAsync(int companyId);
    }
}
