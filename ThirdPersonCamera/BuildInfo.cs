using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(ThirdPersonCamera.BuildInfo.GUID)]
[assembly: AssemblyProduct(ThirdPersonCamera.BuildInfo.Name)]
[assembly: AssemblyVersion(ThirdPersonCamera.BuildInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace ThirdPersonCamera
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvr.thirdpersoncamera";

		public const string Name = "Third person camera";

		public const string Version = "2.2.0";
	}
}
