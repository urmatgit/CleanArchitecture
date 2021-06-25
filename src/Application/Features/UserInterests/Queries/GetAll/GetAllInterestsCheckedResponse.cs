using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll
{
   public class GetAllInterestsCheckedResponse: GetAllInterestsResponse
    {
        public bool Checked { get; set; }
        public int UserInterestId { get; set; }
        //public GetAllInterestsCheckedResponse(bool check ,int? userinterestid)
        //{
        //    Checked= check;
        //    UserInterestId = userinterestid??0;
        //}
    }
}
