using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;

namespace ExternalESPCSGO
{
	public class Graphics
	{
		WindowRenderTarget device;
		SolidColorBrush color;

		public Graphics(OverlayWindow overlayWindow)
		{
			RenderTargetProperties renderTargetProperties = new RenderTargetProperties(RenderTargetType.Default,
				new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
				96.0f,
				96.0f,
				RenderTargetUsage.None,
				FeatureLevel.Level_DEFAULT);

			HwndRenderTargetProperties deviceProperties = new HwndRenderTargetProperties()
			{
				Hwnd = overlayWindow.hWnd.Handle,
				PixelSize = new Size2(overlayWindow.width, overlayWindow.height),
				PresentOptions = PresentOptions.Immediately
			};

			device = new WindowRenderTarget(new Factory(), renderTargetProperties, deviceProperties)
			{
				AntialiasMode = AntialiasMode.PerPrimitive,
				TextAntialiasMode = TextAntialiasMode.Grayscale
			};

			color = new SolidColorBrush(device, new RawColor4(0.05f, 0.78f, 1.00f, 1f));
		}

		public void BeginScene()
		{
			device.BeginDraw();
		}

		public void EndScene()
		{
			device.EndDraw();
		}

		public void Clear()
		{
			device.Clear(new RawColor4(0, 0, 0, 0));
		}

		public void DrawLine(int x1, int y1, int x2, int y2)
		{
			device.DrawLine(new RawVector2(x1, y1), new RawVector2(x2, y2), color, 1f);
		}

		public void DrawText()
		{

		}

		public void Resize(int width, int height)
		{
			device.Resize(new Size2(width, height));
		}
	}
}
