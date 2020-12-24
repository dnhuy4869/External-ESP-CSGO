using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public class Main
	{
		#region ModuleName
		const string windowName = "Counter-Strike: Global Offensive";
		const string className = "Valve001";
		const string processName = "csgo";
		const string moduleName1 = "client.dll";
		const string moduleName2 = "engine.dll";
		#endregion

		IntPtr hWnd = IntPtr.Zero;
		POINT point;
		RECT rect;
		OverlayWindow overlayWindow;
		Graphics graphics;
		Memory memory;
		Data data;

		public void Start()
		{
			Console.ForegroundColor = ConsoleColor.Blue;

		lbReload:
			Console.Clear();

			Console.Write("Loading");

			for (int i = 0; i < 4; i++)
			{
				if (i == 3)
				{
					hWnd = DllImport.FindWindow(className, windowName);

					if (hWnd == IntPtr.Zero)
					{
						Console.Clear();
						Console.WriteLine("Failed to load, make sure that game is opened. Press any key to reload");
						Console.ReadKey(true);

						goto lbReload;
					}

					Utilities.EnableSeDebugPrivilege();

					DllImport.ShowWindow(hWnd, 9);
					DllImport.ClientToScreen(hWnd, out point);
					DllImport.GetClientRect(hWnd, out rect);

					overlayWindow = new OverlayWindow(point, rect);
					graphics = new Graphics(overlayWindow);
					memory = new Memory(processName);
					data = new Data();

					(new Thread(QueryData) { IsBackground = true }).Start();
					(new Thread(Esp) { IsBackground = true }).Start();

					Console.Clear();
					Console.WriteLine("Injected Sucessfully");

					while (true)
					{
						overlayWindow.UpdateWindow(hWnd, point, rect, graphics);
						Thread.Sleep(500);
					}
				}

				Console.Write('.');
				Thread.Sleep(500);
			}

			new ManualResetEvent(false).WaitOne();
		}

		private void QueryData()
		{
			memory.GetBaseAddress(moduleName1, moduleName2);

			while (true)
			{
				Thread.Sleep(10);
			}
		}

		private void Esp()
		{
			while (true)
			{
				graphics.BeginScene();
				graphics.Clear();

				graphics.DrawLine(overlayWindow.width / 2, 0, 200, 200);
				graphics.DrawLine(overlayWindow.width / 2, 0, 400, 400);
				graphics.DrawLine(overlayWindow.width / 2, 0, 300, 300);
				graphics.DrawLine(overlayWindow.width / 2, 0, 700, 700);

				graphics.EndScene();
				Thread.Sleep(10);
			}
		}
	}
}
