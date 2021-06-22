using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class UserInterestRepository : IUserInterestRepository
    {
        private readonly IRepositoryAsync<UserInterest, int> _repository;

        public UserInterestRepository(IRepositoryAsync<UserInterest, int> repository)
        {
            _repository = repository;
        }
        public async Task<List<UserInterest>> GetInterestByUser(string userid)
        {
            return await _repository.Entities
                .Include(i => i.Interest)
                .Where(i => i.UserId == userid)
                 .AsNoTracking()
                .ToListAsync();

        }

        public async Task<bool> IsInterestUsed(int interestId)
        {
            return await _repository.Entities.AnyAsync(b => b.InterestId==interestId);
        }
    }
}
