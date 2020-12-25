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
		POINT point = default;
		RECT rect = default;
		OverlayWindow overlayWindow = default;
		Graphics graphics = default;
		Memory memory = default;
		Data data = default;
		Thread espThread = null;

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

					Console.Clear();
					Console.WriteLine("Injected sucessfully");

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

		int myTeamId = 0;
		int localPlayer = 0;
		int clientState = 0;
		int entityCount = 0;
		int entityObject = 0;

		private void QueryData()
		{
			memory.GetBaseAddress(moduleName1, moduleName2);

			while (true)
			{
				localPlayer = memory.ReadMemory<int>(BaseAddress.client + Offsets.dwLocalPlayer);
				myTeamId = memory.ReadMemory<int>(localPlayer + Offsets.m_iTeamNum);
				clientState = memory.ReadMemory<int>(BaseAddress.engine + Offsets.dwClientState);
				entityCount = memory.ReadMemory<int>(clientState + Offsets.dwClientState_MaxPlayer);

				List<Player> playerList = new List<Player>();

				for (int i = 0; i < entityCount; ++i)
				{
					entityObject = memory.ReadMemory<int>(BaseAddress.client + Offsets.dwEntityList + i * 0x10);

					Player player = new Player()
					{
						health = memory.ReadMemory<int>(entityObject + Offsets.m_iHealth),
						teamId = memory.ReadMemory<int>(entityObject + Offsets.m_iTeamNum),
						dormant = memory.ReadMemory<int>(entityObject + Offsets.m_bDormant),
						position = memory.ReadMemory<Vector3>(entityObject + Offsets.m_vecOrigin)
					};

					if (player.IsMyTeam(player.teamId) || player.IsDead() || player.dormant == 1)
						continue;

					playerList.Add(player);
				}

				data.players = playerList.ToArray();

				if (espThread == null)
				{
					(espThread = new Thread(Esp) { IsBackground = true }).Start();
				}	

				Thread.Sleep(10);
			}
		}

		private void Esp()
		{
			while (true)
			{
				graphics.BeginScene();
				graphics.Clear();

				foreach (Player player in data.players)
				{
					graphics.DrawLine(overlayWindow.width / 2f, overlayWindow.height / 2f, player.position.x, player.position.y);
				}	

				graphics.EndScene();
				Thread.Sleep(10);
			}
		}
	}
}
