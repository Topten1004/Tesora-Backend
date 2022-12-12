using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Item View Mode Parameters
    /// </summary>
    public class ItemViewModelParams
    {
        /// <summary>
        /// Collection Id
        /// </summary>
        [BindRequired]
        public int CollectionId { get; set; }

        /// <summary>
        /// Item Id
        /// </summary>
        public int? ItemId { get; set; }
    }
}
