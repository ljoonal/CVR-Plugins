using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(RotateIt.BuildInfo.Id)]
[assembly: AssemblyProduct(RotateIt.BuildInfo.Name)]
[assembly: AssemblyVersion(RotateIt.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace RotateIt
{
	public static class BuildInfo
	{
		public const string Id = "xyz.ljoonal.cvr.rotateit";

		public const string Name = "Rotate It";

		public const string Version = "0.3.2";
	}
}
