using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace myGame
{
	public class Sprite
	{
		public Rectangle _posicaoInicial;
		public Texture2D characterSheetTexture;

		public Sprite(string FileName, Rectangle position, GraphicsDevice graphicsDevice)
		{
			_posicaoInicial = position;

			if (characterSheetTexture == null)
			{
				using (var stream = TitleContainer.OpenStream("Content/" + FileName))
				{
					characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
				}
			}
		}
		public void Update(GameTime GameTime)
		{

		}
		public void Draw(SpriteBatch spriteBatch, Rectangle NovaPosicao)
		{
			_posicaoInicial = NovaPosicao;
			spriteBatch.Draw(characterSheetTexture, NovaPosicao, Color.White);
		}
	}
}
