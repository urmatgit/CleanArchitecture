using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
   public class BaseEntity: AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Decription { get; set; }
    }
}
