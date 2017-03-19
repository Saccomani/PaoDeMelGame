using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace myGame
{
	public class ScalingViewportAdapter : ViewportAdapter
	{
		GraphicsDevice _graphicsDevice;
		public ScalingViewportAdapter(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
			: base(graphicsDevice)
		{
			_virtualWidth = virtualWidth;
			_virtualHeight = virtualHeight;
			_graphicsDevice = graphicsDevice;
		}

		private readonly int _virtualWidth;
		private readonly int _virtualHeight;
		public override int ViewportWidth
		{
			get
			{
				return _virtualWidth;
			}
		}

		public override int ViewportHeight
		{
			get
			{
				return _virtualHeight;
			}
		}

		public override int VirtualWidth
		{
			get
			{
				return _virtualWidth;
			}
		}

		public override int VirtualHeight
		{
			get
			{
				return _virtualHeight;
			}
		}

		public override Matrix GetScaleMatrix()
		{
			var scaleX = (float)_graphicsDevice.Viewport.Width / VirtualWidth;
			var scaleY = (float)_graphicsDevice.Viewport.Height / VirtualHeight;
			return Matrix.CreateScale(scaleX, scaleY, 1.0f);
		}
	}
}
