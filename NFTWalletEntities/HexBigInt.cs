
namespace NFTWalletEntities
{
	public class HexBitIntValue
	{
		public bool IsPowerOfTwo { get; set; }
		public bool IsZero { get; set; }
		public bool IsOne { get; set; }
		public bool IsEven { get; set; }
		public int Sign { get; set; }
	}

    public class HexBigInt
    {
		public string HexValue { get; set; }
		public HexBitIntValue Value { get; set; }

    }
}
