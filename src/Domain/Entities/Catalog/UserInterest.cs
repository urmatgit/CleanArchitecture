using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Catalog
{
    public class UserInterest : AuditableEntity<int>
    {
        public string UserId { get; set; }
        
        public int InterestId { get; set; }
        public virtual Interest Interest { get; set; }
        public byte Level { get; set; }
    }
}
