namespace NFTApplication.Models
{
    /// <summary>
    /// Drop Down List
    /// </summary>
    public class DropDownList
    {
        /// <summary>
        /// Displayed Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Id of entry
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Selected option
        /// </summary>
        public bool Selected { get; set; }
    }
}
