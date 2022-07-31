using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(HopLib.HopLibInfo.Id)]
[assembly: AssemblyProduct(HopLib.HopLibInfo.Name)]
[assembly: AssemblyVersion(HopLib.HopLibInfo.Version)]
[assembly: AssemblyCompany("cvr.ljoonal.xyz")]

namespace HopLib
{
	/// <summary>Info about the current HopLib.</summary>
	public static class HopLibInfo
	{
		/// <summary>The plugin's constant GUID.</summary>
		public const string Id = "xyz.ljoonal.cvr.hoplib";

		internal const string Name = "Hop Lib";

		/// <summary>The current version of HopLib.</summary>
		public const string Version = "0.1.0";
	}
}
