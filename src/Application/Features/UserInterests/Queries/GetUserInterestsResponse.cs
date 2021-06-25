using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries
{
    public class GetUserInterestsResponse
    {
        public string UserId { get; set; }

        public int InterestId { get; set; }
        public virtual string Interest { get; set; }
        public string PictureDataUrl { get; set; }
        public byte Level { get; set; }
    }
}
