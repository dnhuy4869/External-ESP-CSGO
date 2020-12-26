using ExternalESPCSGO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalESPCSGO
{
	public class OverlayWindow
	{
		private Form _hWnd;
		private int _left;
		private int _top;
		private int _width;
		private int _height;

		public Form hWnd { get => _hWnd; private set => _hWnd = value; }
		public int left { get => _left; private set => _left = value; }
		public int top { get => _top; private set => _top = value; }
		public int width { get => _width; private set => _width = value; }
		public int height { get => _height; private set => _height = value; }

		public OverlayWindow(POINT lpPoint, RECT lpRect)
		{
			_hWnd = new Form
			{
				Name = Utilities.GenerateRandomAsciiString(8, 16),
				Text = Utilities.GenerateRandomAsciiString(8, 16),
				MinimizeBox = false,
				MaximizeBox = false,
				FormBorderStyle = FormBorderStyle.None,
				TopMost = true,
				Width = lpRect.right,
				Height = lpRect.bottom,
				Left = lpPoint.x,
				Top = lpPoint.y,
				StartPosition = FormStartPosition.Manual,
			};

			_hWnd.Load += (sender, args) =>
			{
				uint exStyle = DllImport.GetWindowLong(_hWnd.Handle, -20);
				exStyle = 0x00000008 | 0x00000020 | 0x00080000 | 0x08000000;

				DllImport.SetWindowLong(_hWnd.Handle, -20, (IntPtr)exStyle);
				DllImport.SetLayeredWindowAttributes(_hWnd.Handle, 0, 0, 0x00000002);

				Utilities.ExtendFrameIntoClientArea(_hWnd.Handle);
			};

			left = lpPoint.x;
			top = lpPoint.y;
			width = lpRect.right;
			height = lpRect.bottom;

			_hWnd.Show();
		}

		public void UpdateWindow(IntPtr hWnd, POINT lpPoint, RECT lpRect, Graphics graphics)
		{
			if (DllImport.IsIconic(hWnd) == 0)
			{
				lpPoint = default;

				DllImport.ClientToScreen(hWnd, out lpPoint);
				DllImport.GetClientRect(hWnd, out lpRect);

				if ((_left != lpPoint.x) || (_top != lpPoint.y))
				{
					left = lpPoint.x;
					top = lpPoint.y;
					_hWnd.Left = lpPoint.x;
					_hWnd.Top = lpPoint.y;

					Utilities.ExtendFrameIntoClientArea(_hWnd.Handle);
				}

				if ((_width != lpRect.right) || (_height != lpRect.bottom))
				{
					width = lpRect.right;
					height = lpRect.bottom;
					_hWnd.Width = lpRect.right;
					_hWnd.Height = lpRect.bottom;

					graphics.Resize(_width, _height);

					Utilities.ExtendFrameIntoClientArea(_hWnd.Handle);
				}
			}
		}
	}
}
