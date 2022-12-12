
using FluentValidation;
using NFTDatabaseService;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// AddItemRequest Validator
    /// </summary>
    public class ItemViewModelValidator : AbstractValidator<ItemViewModel>
    {
        private readonly INFTDatabaseService _db;

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemViewModelValidator(INFTDatabaseService db)
        {
            _db = db;

            RuleFor(addItemRequest => addItemRequest.Name).NotEmpty().WithMessage("The item must have a name");
            RuleFor(addItemRequest => addItemRequest.Media).NotNull().WithMessage("Item must contain an media type");
            RuleFor(addItemRequest => addItemRequest.CollectionId).NotEmpty().WithMessage("The item has to be assigned to a collection");
            RuleFor(addItemRequest => addItemRequest.CategoryId).Must(HaveValidCategory).WithMessage("The item has an invalid category");
        }

        private bool HaveValidCategory(int? categoryId)
        {
            bool validCategory = true;

            if (categoryId.HasValue)
                validCategory = _db.GetCategoryExists(categoryId.Value).Result;

            return validCategory;
        }
    }
}
