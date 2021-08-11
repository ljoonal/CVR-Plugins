using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(HopLib.HopLibInfo.GUID)]
[assembly: AssemblyProduct(HopLib.HopLibInfo.Name)]
[assembly: AssemblyVersion(HopLib.HopLibInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace HopLib
{
	/// <summary>Info about the current HopLib.</summary>
	public static class HopLibInfo
	{
		/// <summary>The plugin's constant GUID.</summary>
		public const string GUID = "xyz.ljoonal.cvr.hoplib";

		internal const string Name = "Hop Lib";

		/// <summary>The current version of HopLib.</summary>
		public const string Version = "0.0.1";
	}
}
