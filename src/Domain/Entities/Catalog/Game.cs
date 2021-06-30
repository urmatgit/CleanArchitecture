using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Catalog
{
    public class Game : BaseEntity
    {
        public bool Archive { get; set; }
        public bool Publish { get; set; }
        
        public ushort PlayerCount { get; set; }

        public string UserId { get; set; }
        public int InterestId { get; set; }
        public virtual Interest Interest { get; set; }
        public int GameTypeId { get; set; }
        public virtual GameType GameType { get; set; }

    }
}
