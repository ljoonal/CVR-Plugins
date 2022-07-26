using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(MoreFilters.BuildInfo.GUID)]
[assembly: AssemblyProduct(MoreFilters.BuildInfo.Name)]
[assembly: AssemblyVersion(MoreFilters.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace MoreFilters
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvr.morefilters";

		public const string Name = "More filters";

		public const string Version = "0.0.2";
	}
}
