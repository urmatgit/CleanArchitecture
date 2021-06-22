using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Catalog
{
  public class Interest : AuditableEntity<int>
    {
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string PictureDataUrl { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual IList<UserInterest> UserInterests { get; set; }
    }
}
