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
		private Process gameProcess;

		public Memory(string processName)
		{
			gameProcess = Process.GetProcessesByName(processName)[0];
			hProcess = DllImport.OpenProcess(0x1F0FFF, false, gameProcess.Id);
		}

		public void GetBaseAddress(string moduleName1, string moduleName2)
		{
			foreach (ProcessModule module in gameProcess.Modules)
			{
				if (module.ModuleName.Contains(moduleName1))
				{
					BaseAddress.client = (int)module.BaseAddress;
				}

				if (module.ModuleName.Contains(moduleName2))
				{
					BaseAddress.engine = (int)module.BaseAddress;
				}
			}
		}

		public T ReadMemory<T>(int lpAddress) where T : struct
		{
			int numOfByte = 0;

			byte[] numArray = new byte[Marshal.SizeOf(typeof(T))];

			DllImport.ReadProcessMemory(hProcess, lpAddress, numArray, numArray.Length, numOfByte);

			return ByteArrayToStructure<T>(numArray);
		}

		private T ByteArrayToStructure<T>(byte[] lpBytes)
		{
			GCHandle gcHandle = GCHandle.Alloc((object)lpBytes, GCHandleType.Pinned);

			try
			{
				return (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
			}
			finally
			{
				gcHandle.Free();
			}
		}
	}
}
