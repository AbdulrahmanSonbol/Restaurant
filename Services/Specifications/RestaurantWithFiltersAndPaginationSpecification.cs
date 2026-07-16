using Contracts.QueryParameters;
using Contracts.Sorting;
using RestaurantEntity = Domain.Entities.RestaurantModule.Restaurant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Specifications
{
    public class RestaurantWithFiltersAndPaginationSpecification
    : BaseSpecifications<RestaurantEntity, int>
    {
        public RestaurantWithFiltersAndPaginationSpecification(RestaurantQueryParams queryParams)
            : base(r => string.IsNullOrWhiteSpace(queryParams.Search)
                || r.Name.Contains(queryParams.Search))
        {
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            switch (queryParams.Sort)
            {
                case RestaurantSortingOptions.NameDesc:
                    AddOrderByDescending(r => r.Name);
                    break;

                case RestaurantSortingOptions.RateDesc:
                    AddOrderByDescending(r => r.AverageRating);
                    break;

                case RestaurantSortingOptions.NameAsc:
                default:
                    AddOrderBy(r => r.Name);
                    break;
            }
        }
    }
}