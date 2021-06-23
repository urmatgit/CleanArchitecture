using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Configurations
{
    public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
    {
        public void Configure(EntityTypeBuilder<UserInterest> builder)
        {
            builder.Property(ui => ui.Id).UseIdentityColumn(1, 1);
            builder.HasKey(tp => new {tp.Id,  tp.UserId, tp.InterestId });
            builder.HasOne(i => i.Interest).WithMany(i => i.UserInterests).HasForeignKey(ui => ui.InterestId);
            

        }
    }
}
