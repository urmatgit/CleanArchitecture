using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
   public class InterestRepostitory:IInterestRepository
    {
        private readonly IRepositoryAsync<Interest, int> _repository;

        public InterestRepostitory(IRepositoryAsync<Interest, int> repository)
        {
            _repository = repository;
        }

         
    }
}
