using Contracts.QueryParameters;
using Contracts.Sorting;
using Domain.Entities.RestaurantModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Specifications
{
    public class RestaurantWithFiltersAndPaginationSpecification : BaseSpecifications<Restaurant, int>
    {
        public RestaurantWithFiltersAndPaginationSpecification(RestaurantQueryParams queryParams)
        : base(r => string.IsNullOrEmpty(queryParams.Search) || r.Name.Contains(queryParams.Search))
        {
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            switch (queryParams.Sort)
            {
                case RestaurantSortingOptions.NameDesc:
                    AddOrderByDescending(r => r.Name);
                    break;

                case RestaurantSortingOptions.RateDesc:
                    AddOrderByDescending(r => r.Rating);
                    break;

                case RestaurantSortingOptions.NameAsc:
                default:
                    AddOrderBy(r => r.Name);
                    break;
            }
        }
    }
}