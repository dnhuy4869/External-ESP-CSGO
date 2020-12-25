using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public class Data
	{
		public Player[] players;
	}

	public class Player
	{
		public int health;
		public int teamId;
		public int dormant;
		public Vector3 position;

		public bool IsDead()
		{
			if (health < 1) return true;
			else return false;
		}

		public bool IsMyTeam(int teamId)
		{
			if (this.teamId == teamId) return true;
			else return false;
		}
	}
}
