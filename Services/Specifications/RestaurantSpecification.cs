using Contracts.QueryParameters;
using Contracts.Sorting;
using RestaurantEntity = Domain.Entities.RestaurantModule.Restaurant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Specifications
{
    public class RestaurantSpecification : BaseSpecifications<RestaurantEntity, int>
    {
        public RestaurantSpecification(RestaurantQueryParams queryParams)
      : base(x =>
          string.IsNullOrWhiteSpace(queryParams.Search) ||
          x.Name.ToLower().Contains(queryParams.Search.ToLower()))
        {
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            if (queryParams.Sort == RestaurantSortingOptions.RateDesc)
            {
                AddOrderByDescending(x => x.AverageRating);
            }
            else
            {
                AddOrderBy(x => x.Name);
            }
        }
    }
}
