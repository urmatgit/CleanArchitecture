namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class GamesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/games/export";

        public static string GetAll = "api/v1/games";
        public static string Delete = "api/v1/games";
        public static string Save = "api/v1/games";
        public static string GetCount = "api/v1/games/count";
    }
}