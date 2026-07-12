using Contracts.QueryParameters;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Validator
{
    internal class RestaurantQueryParamsValidator : AbstractValidator<RestaurantQueryParams>
    {
        public RestaurantQueryParamsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page index must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .Equal(20)
                .WithMessage("Page size must be strictly 20 restaurants per page as per business rules.");


            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search keyword cannot exceed 100 characters.")
                .Must(search => !ContainsSqlInjectionThreat(search))
                .WithMessage("Security Alert: Potential SQL Injection attempt detected in search query!")
                .When(x => !string.IsNullOrEmpty(x.Search));

            RuleFor(x => x.Sort)
                .IsInEnum()
                .WithMessage("Invalid sorting option. Please select a valid option (Alphabetical or Top Rated).");
        }
        private bool ContainsSqlInjectionThreat(string? input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string[] sqlCheckList = { "--", ";--", ";", "DROP", "ALTER", "SELECT", "INSERT", "DELETE", "UPDATE", "UNION" };

            foreach (var item in sqlCheckList)
            {
                if (input.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false; 
                }
            }
            return true; 
        }
    }
}
