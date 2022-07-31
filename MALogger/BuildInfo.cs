using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(MALogger.BuildInfo.Id)]
[assembly: AssemblyProduct(MALogger.BuildInfo.Name)]
[assembly: AssemblyVersion(MALogger.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace MALogger
{
	public static class BuildInfo
	{
		public const string Id = "xyz.ljoonal.cvr.malogger";

		public const string Name = "M.A Logger";

		public const string Version = "0.0.3";
	}
}
