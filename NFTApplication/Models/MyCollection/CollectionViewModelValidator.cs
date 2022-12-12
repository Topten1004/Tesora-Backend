
using FluentValidation;
using NFTDatabaseService;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// AddItemRequest Validator
    /// </summary>
    public class CollectionViewModelValidator : AbstractValidator<CollectionViewModel>
    {
        private readonly INFTDatabaseService _db;
        private readonly bool _update;

        /// <summary>
        /// Constructor
        /// </summary>
        public CollectionViewModelValidator(INFTDatabaseService db, bool update)
        {
            _db = db;
            _update = update;

            RuleFor(x => x.Name).NotEmpty().WithMessage("The collection needs a name");
            RuleFor(x => x.Description).NotEmpty().WithMessage("The collection needs a description");
            RuleFor(x => x.Banner).NotNull().WithMessage("The collection needs a banner");
            RuleFor(x => x.CollectionImage).NotNull().WithMessage("The collection needs an image");

            RuleFor(x => x.Name).Must(HaveUniqueName).WithMessage("The collection name must be unique");
            RuleFor(x => x.CollectionId).Must(CollectionIdNotNull).WithMessage("To update collection, collection id needs to be supplied");

            RuleFor(x => x.Royalties).GreaterThanOrEqualTo(0.00m).LessThanOrEqualTo(100.00m).WithMessage("The royality must be between 0.00 and 100.00");
        }

        private bool HaveUniqueName(string name)
        {
            bool validName = false;

            if (!string.IsNullOrEmpty(name))
            {
                if (!_update)
                    validName = !_db.GetCollectionNameExists(name).Result;
                else
                    validName = true;
            }

            return validName;
        }

        private bool CollectionIdNotNull(int? collectionId)
        {
            bool validId = true;

            if (_update)
                validId = collectionId.HasValue;

            return validId;
        }

    }
}
