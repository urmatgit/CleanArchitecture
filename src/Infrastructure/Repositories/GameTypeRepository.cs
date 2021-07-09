using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class GameTypeRepository : IGameTypeRepository
    {
        private readonly IRepositoryAsync<GameType, int> _repository;

        public GameTypeRepository(IRepositoryAsync<GameType, int> repository)
        {
            _repository = repository;
        }
    }
}