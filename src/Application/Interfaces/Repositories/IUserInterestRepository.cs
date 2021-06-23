using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Repositories
{
    public interface IUserInterestRepository
    {
        Task<List<UserInterest>> GetInterestByUser(string userid);
        Task<bool> IsInterestUsed(int interestId);
        Task<UserInterest> GetUserInterest(string userid, int interestid);
    }
}
