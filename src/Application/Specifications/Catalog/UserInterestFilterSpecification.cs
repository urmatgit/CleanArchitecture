using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Specifications
{
    public class UserInterestFilterSpecification : HeroSpecification<UserInterest>
    {
        public UserInterestFilterSpecification(string searchString)
        {
            Includes.Add(i => i.Interest);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>p.Interest!=null &&  p.Interest.Name.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
