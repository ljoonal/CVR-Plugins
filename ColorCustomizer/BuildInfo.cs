using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(ColorCustomizer.BuildInfo.GUID)]
[assembly: AssemblyProduct(ColorCustomizer.BuildInfo.Name)]
[assembly: AssemblyVersion(ColorCustomizer.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]


namespace ColorCustomizer
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvr.colorcustomizer";

		public const string Name = "Color Customizer";

		public const string Version = "0.3.1";
	}
}
