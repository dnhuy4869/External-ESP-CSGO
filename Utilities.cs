using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public static class Utilities
	{
		public static void ExtendFrameIntoClientArea(IntPtr hWnd)
		{
			MARGINS margins = new MARGINS
			{
				cxLeftWidth = -1,
				cxRightWidth = -1,
				cyBottomHeight = -1,
				cyTopHeight = -1
			};

			DllImport.DwmExtendFrameIntoClientArea(hWnd, ref margins);
		}

		private static readonly Random random = new Random();

		public static string GenerateRandomAsciiString(int minLenght, int maxLength)
		{
			int lenght = random.Next(minLenght, maxLength);

			char[] chars = new char[lenght];

			for (int i = 0; i < chars.Length; i++)
			{
				chars[i] = (char)random.Next(97, 123);
			}

			return new string(chars);
		}

		public static void EnableSeDebugPrivilege()
		{
			IntPtr tokenHandle = IntPtr.Zero;
			LUID luid = new LUID();

			DllImport.OpenProcessToken(Process.GetCurrentProcess().Handle, 0x0020 | 0x00000008, ref tokenHandle);
			DllImport.LookupPrivilegeValue(null, "SeDebugPrivilege", ref luid);

			TOKEN_PRIVILEGES tokenPrivileges = new TOKEN_PRIVILEGES();

			tokenPrivileges.PrivilegeCount = 1;
			tokenPrivileges.Privileges = new LUID_AND_ATTRIBUTES[1];
			tokenPrivileges.Privileges[0].Luid = luid;
			tokenPrivileges.Privileges[0].Attributes = 0x00000002;

			DllImport.AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);

			DllImport.CloseHandle(tokenHandle);
		}
	}
}
