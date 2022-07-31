using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(SkipIntro.BuildInfo.Id)]
[assembly: AssemblyProduct(SkipIntro.BuildInfo.Name)]
[assembly: AssemblyVersion(SkipIntro.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace SkipIntro
{
	public static class BuildInfo
	{
		public const string Id = "xyz.ljoonal.cvr.skipintro";

		public const string Name = "Skip intro";

		public const string Version = "0.0.3";
	}
}
