using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class UserInterestsEndPoints
    {
        public static string GetCheckedAll = "api/v1/userinterests/GetCheckedAll";
        public static string GetAll = "api/v1/userinterests";
        public static string Delete = "api/v1/userinterests";
        public static string Save = "api/v1/userinterests";
        public static string GetCount = "api/v1/userinterests/count";
    }
}
