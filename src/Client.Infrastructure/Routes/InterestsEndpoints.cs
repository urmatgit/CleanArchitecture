namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class InterestsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/interests/export";

        public static string GetAll = "api/v1/interests";
        public static string Delete = "api/v1/interests";
        public static string Save = "api/v1/interests";
        public static string GetCount = "api/v1/interests/count";
        public static string GetProductImage(int interestId)
        {
            return $"api/v1/interests/image/{interestId}";
        }
    }
}