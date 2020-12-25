using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public class Memory
	{
		private IntPtr hProcess = IntPtr.Zero;
		private Process gameProcess = default;

		public Memory(string processName)
		{
			gameProcess = Process.GetProcessesByName(processName)[0];
			hProcess = DllImport.OpenProcess(0x0010 | 0x0020, false, gameProcess.Id);
		}

		public void GetBaseAddress(string moduleName1, string moduleName2)
		{
			int countModule = 0;
			IntPtr hProcess = DllImport.CreateToolhelp32Snapshot(0x00000008 | 0x00000010, (uint)gameProcess.Id);

			MODULEENTRY32 moduleEntry32 = new MODULEENTRY32();
			moduleEntry32.dwSize = (uint)Marshal.SizeOf(moduleEntry32);

			if (DllImport.Module32First(hProcess, ref moduleEntry32))
			{
				while (countModule < 2)
				{
					if (moduleEntry32.szModule.Contains(moduleName1))
					{
						BaseAddress.client = (int)moduleEntry32.modBaseAddr;
						countModule++;
					}

					if (moduleEntry32.szModule.Contains(moduleName2))
					{
						BaseAddress.engine = (int)moduleEntry32.modBaseAddr;
						countModule++;
					}

					DllImport.Module32Next(hProcess, ref moduleEntry32);
				}
			}
		}

		int iNumberOfBytesRead = 0;
		uint oldProtect = 0;

		public T ReadMemory<T>(int lpAddress) where T : struct
		{
			byte[] byteArray = new byte[Marshal.SizeOf(typeof(T))];

			DllImport.VirtualProtectEx(hProcess, lpAddress, Marshal.SizeOf(typeof(T)), 0x40, ref oldProtect);
			DllImport.ReadProcessMemory(hProcess, lpAddress, ref byteArray, byteArray.Length, ref iNumberOfBytesRead);
			DllImport.VirtualProtectEx(hProcess, lpAddress, Marshal.SizeOf(typeof(T)), oldProtect, ref oldProtect);

			return ByteArrayToStructure<T>(byteArray);
		}

		public float[] ReadMatrix<T>(int lpAddress, int matrixSize) where T : struct
		{
			byte[] byteArray = new byte[Marshal.SizeOf(typeof(T)) * matrixSize];

			DllImport.VirtualProtectEx(hProcess, lpAddress, Marshal.SizeOf(typeof(T)), 0x40, ref oldProtect);
			DllImport.ReadProcessMemory(hProcess, lpAddress, ref byteArray, byteArray.Length, ref iNumberOfBytesRead);
			DllImport.VirtualProtectEx(hProcess, lpAddress, Marshal.SizeOf(typeof(T)), oldProtect, ref oldProtect);

			return ConvertToFloatArray(byteArray);
		}

		private T ByteArrayToStructure<T>(byte[] bytes)
		{
			GCHandle gcHandle = GCHandle.Alloc((object)bytes, GCHandleType.Pinned);

			try
			{
				return (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
			}
			finally
			{
				gcHandle.Free();
			}
		}

		private static float[] ConvertToFloatArray(byte[] bytes)
		{
			if (bytes.Length % 4 != 0) throw new ArgumentException();

			float[] floats = new float[15];

			for (int i = 0; i < floats.Length; i++) floats[i] = BitConverter.ToSingle(bytes, i * 4);

			return floats;
		}
	}
}
