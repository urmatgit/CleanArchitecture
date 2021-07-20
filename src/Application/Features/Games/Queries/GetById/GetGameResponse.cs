namespace BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetById
{
    public class GetGameByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Archive { get; set; }
        public bool Publish { get; set; }

        public ushort PlayerCount { get; set; }

        public string UserId { get; set; }
        public int InterestId { get; set; }
        public string Interest { get; set; }
        public int GameTypeId { get; set; }
        public string GameType { get; set; }
    }
}