using System.ComponentModel;

namespace COD
{
	public enum Time
	{
		[Description("alltime")]
		Lifetime,

		[Description("monthly")]
		Monthly,

		[Description("weekly")]
		Weekly
	}
}
