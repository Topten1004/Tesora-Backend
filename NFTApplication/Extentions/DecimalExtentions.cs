// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
namespace NFTApplication.Extentions
{
    /// <summary>
    /// Decimal Extensions
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Truncate to specified number of decimal places
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static decimal TruncateEx(this decimal value, int decimalPlaces)
        {
            if (decimalPlaces < 0)
                throw new ArgumentException("decimalPlaces must be greater than or equal to 0.");

            var modifier = Convert.ToDecimal(0.5 / Math.Pow(10, decimalPlaces));

            return Math.Round(value >= 0 ? value - modifier : value + modifier, decimalPlaces);
        }
    }
}
