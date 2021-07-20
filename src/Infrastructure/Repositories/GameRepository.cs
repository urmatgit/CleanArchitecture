using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IRepositoryAsync<Game, int> _repository;

        public GameRepository(IRepositoryAsync<Game, int> repository)
        {
            _repository = repository;
        }
    }
}