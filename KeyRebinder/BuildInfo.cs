using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(KeyRebinder.BuildInfo.GUID)]
[assembly: AssemblyProduct(KeyRebinder.BuildInfo.Name)]
[assembly: AssemblyVersion(KeyRebinder.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace KeyRebinder
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvr.keyrebinder";

		public const string Name = "Key Rebinder";

		public const string Version = "1.8.1";
	}
}
