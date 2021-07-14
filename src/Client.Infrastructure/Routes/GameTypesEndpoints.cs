namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class GameTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/gametypes/export";

        public static string GetAll = "api/v1/gametypes";
        public static string Delete = "api/v1/gametypes";
        public static string Save = "api/v1/gametypes";
        public static string GetCount = "api/v1/gametypes/count";
    }
}