using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(SpeedMultiplier.BuildInfo.Id)]
[assembly: AssemblyProduct(SpeedMultiplier.BuildInfo.Name)]
[assembly: AssemblyVersion(SpeedMultiplier.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace SpeedMultiplier
{
	public static class BuildInfo
	{
		public const string Id = "xyz.ljoonal.cvr.speedmultiplier";

		public const string Name = "Speed Multiplier";

		public const string Version = "0.3.0";
	}
}
