using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public static class BaseAddress
	{
		public static int client;
		public static int engine;
	}

	public static class Offsets
	{
		public const int dwViewMatrix = 0x4D914D4;
		public const int dwLocalPlayer = 0xD882BC;
		public const int dwEntityList = 0x4D9FBD4;
		public const int dwClientState = 0x58EFE4;
		public const int dwClientState_MaxPlayer = 0x388;
		public const int m_dwBoneMatrix = 0x26A8;
		public const int m_iHealth = 0x100;
		public const int m_iTeamNum = 0xF4;
		public const int m_bDormant = 0xED;
		public const int m_vecOrigin = 0x138;
	}
}
