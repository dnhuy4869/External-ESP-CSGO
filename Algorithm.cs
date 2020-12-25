using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalESPCSGO
{
	public static class Algorithm
	{
		public static bool WorldToScreen(Vector3 entityPosition, float[] viewMatrix, ref Vector2 desirePosition, Vector4 vector4, int windowWidth, int windowHeight)
		{
			desirePosition.x = entityPosition.x * viewMatrix[0] + entityPosition.y * viewMatrix[1] + entityPosition.z * viewMatrix[2] + viewMatrix[3];
			desirePosition.y = entityPosition.x * viewMatrix[4] + entityPosition.y * viewMatrix[5] + entityPosition.z * viewMatrix[6] + viewMatrix[7];
			vector4.w = entityPosition.x * viewMatrix[12] + entityPosition.y * viewMatrix[13] + entityPosition.z * viewMatrix[14] + viewMatrix[15];

			if (vector4.w < 0.001f) return false;

			desirePosition.x /= vector4.w;
			desirePosition.y /= vector4.w;

			desirePosition.x = (windowWidth / 2) * desirePosition.x + (windowWidth / 2);
			desirePosition.y = -(windowHeight / 2) * desirePosition.y + (windowHeight / 2);

			return true;
		}
	}
}
