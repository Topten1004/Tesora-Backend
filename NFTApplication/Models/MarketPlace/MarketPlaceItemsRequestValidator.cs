// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using FluentValidation;


namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// AddItemRequest Validator
    /// </summary>
    public class MarketPlaceItemsRequestValidator : AbstractValidator<MarketPlaceItemsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketPlaceItemsRequestValidator()
        {
            var conditions = new List<string>() { "USD", "EUR", "ETH" };
            RuleFor(x => x.DisplayCurrency)
              .Must(x => conditions.Contains(x))
              .WithMessage("Valid DisplayCurrency is:" + string.Join(",", conditions));
        }

    }
}
