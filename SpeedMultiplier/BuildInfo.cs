using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(SpeedMultiplier.BuildInfo.GUID)]
[assembly: AssemblyProduct(SpeedMultiplier.BuildInfo.Name)]
[assembly: AssemblyVersion(SpeedMultiplier.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace SpeedMultiplier
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvr.speedmultiplier";

		public const string Name = "Speed Multiplier";

		public const string Version = "0.2.0";
	}
}
