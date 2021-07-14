namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetById
{
    public class GetGameTypeByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //TODO other Fields
    }
}