using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;

namespace ExternalESPCSGO
{
	public static class DllImport
	{
		[DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("User32.dll", EntryPoint = "ShowWindow")]
		public static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("User32.dll", EntryPoint = "ClientToScreen")]
		public static extern IntPtr ClientToScreen(IntPtr hWnd, out POINT lpPoint);

		[DllImport("User32.dll", EntryPoint = "GetClientRect")]
		public static extern IntPtr GetClientRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("Dwmapi.dll", EntryPoint = "DwmExtendFrameIntoClientArea")]
		public static extern IntPtr DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("Advapi32.dll", EntryPoint = "OpenProcessToken"), SuppressUnmanagedCodeSecurityAttribute]
		public static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

		[DllImport("Advapi32.dll", EntryPoint = "LookupPrivilegeValue")]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

		[DllImport("Advapi32.dll", EntryPoint = "AdjustTokenPrivileges")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
		   [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
		   ref TOKEN_PRIVILEGES NewState,
		   UInt32 BufferLengthInBytes,
		   IntPtr PreviousState,
		   IntPtr ReturnLength);

		[DllImport("Kernel32.dll", EntryPoint = "CloseHandle"), SuppressUnmanagedCodeSecurityAttribute]
		public static extern bool CloseHandle(IntPtr hWnd);

		[DllImport("User32.dll", EntryPoint = "GetWindowLongA")]
		public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("User32.dll", EntryPoint = "SetWindowLongA")]
		public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("User32.dll", EntryPoint = "SetLayeredWindowAttributes")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, uint dwFlags);

		[DllImport("User32.dll", EntryPoint = "IsIconic")]
		public static extern int IsIconic(IntPtr hWnd);

		[DllImport("Kernel32.dll", EntryPoint = "OpenProcess")]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("Kernel32.dll", EntryPoint = "ReadProcessMemory")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, ref byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

		[DllImport("Kernel32.dll", EntryPoint = "VirtualProtectEx")]
		public static extern bool VirtualProtectEx(IntPtr hProcess, long lpBaseAddress, int dwSize, uint flNewProtect, ref uint lpflOldProtect);

		[DllImport("Kernel32.dll", EntryPoint = "CreateToolhelp32Snapshot")]
		public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processId);

		[DllImport("Kernel32.dll", EntryPoint = "Module32First")]
		public static extern bool Module32First(IntPtr hProcess, ref MODULEENTRY32 moduleEntry32);

		[DllImport("Kernel32.dll", EntryPoint = "Module32Next")]
		public static extern bool Module32Next(IntPtr hProcess, ref MODULEENTRY32 moduleEntry32);
	}
}
