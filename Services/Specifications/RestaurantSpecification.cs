using Contracts.QueryParameters;
using Contracts.Sorting;
using Domain.Entities.RestaurantModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Specifications
{
    public class RestaurantSpecification : BaseSpecifications<Restaurant, int>
    {
        public RestaurantSpecification(RestaurantQueryParams queryParams)
            : base(x =>
                string.IsNullOrEmpty(queryParams.Search) ||
                x.Name.ToLower().Contains(queryParams.Search.ToLower()) ||
                x.Category.ToLower().Contains(queryParams.Search.ToLower())
            )
        {
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            if (queryParams.Sort == RestaurantSortingOptions.RateDesc)
            {
                AddOrderByDescending(x => x.Rating);
            }
            else
            {
                AddOrderBy(x => x.Name);
            }
        }
    }
}
