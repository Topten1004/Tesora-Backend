
using FluentValidation;
using IdentityServer4.Extensions;
using Nethereum.Util;
using NFTApplication.Models.MyWallet;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// AddItemRequest Validator
    /// </summary>
    public class SendCoinsRequestValidator : AbstractValidator<SendCoinsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SendCoinsRequestValidator()
        {
            RuleFor(x => x.ToAddress).NotEmpty().WithMessage("Must include the to address");
            RuleFor(x => x.AmountEth).GreaterThan(0.00m).WithMessage("The amount must be greater than zero");

            RuleFor(x => x.ToAddress).Must(HasValidAddress).WithMessage("The address must be an valid address");
        }

        private bool HasValidAddress(string? address)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(address))
            {
                var util = new AddressUtil();

                isValid = util.IsValidEthereumAddressHexFormat(address);
            }

            return isValid;
        }
    }
}
