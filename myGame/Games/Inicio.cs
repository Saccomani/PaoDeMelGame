using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace myGame
{
	enum GameState
	{
		MainMenu,
		Gameplay,
		EndOfGame,
	}

	public class Inicio:Game
	{

		GameState _state;
        GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Sprite _jeff;
		Sprite _paoDeMel;
		Sprite _background;
		Sprite _botaoIniciar;
		Vector2 position;
		Vector2 _posicaoPaoDeMel;
		SpriteFont font;
		protected Song song;
		protected Song songItemColetado;
		int score = 0;
		int _vidas = 6;
		bool playerDied;
		bool pushedStartGameButton = false;
		bool pushedMainMenuButton;
		bool pushedRestartLevelButton;
		ScalingViewportAdapter _viewportAdapter;
		public Inicio()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

			_posicaoPaoDeMel.X = 854;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);      
			_background = new Sprite("background.png", new Rectangle(0, 0, 854, 480), this.GraphicsDevice);
			_botaoIniciar=new Sprite("btnStartGame.png", new Rectangle(0, 0, 854, 480), this.GraphicsDevice);
			_jeff = new Sprite("jeff.jpeg", new Rectangle(40, 20, 100, 100), this.GraphicsDevice);
			_paoDeMel = new Sprite("paodemel.png", new Rectangle(400, 20, 50, 50), this.GraphicsDevice);
			font = Content.Load<SpriteFont>("Score");

			song = Content.Load<Song>("game");

			songItemColetado = Content.Load<Song>("item");
			MediaPlayer.Play(song);


			_viewportAdapter = new ScalingViewportAdapter(this.GraphicsDevice, 854, 480);
		}

		private void getNextRandon()
		{
			Random rnd = new Random();
			_posicaoPaoDeMel.Y =rnd.Next(1, 370);
		}

		void UpdateMainMenu(GameTime deltaTime)
		{

			pushedStartGameButton = touchSelect(_botaoIniciar._posicaoInicial);

			_botaoIniciar.Update(deltaTime);

			if (pushedStartGameButton)
				_state = GameState.Gameplay;
		}
		bool touchSelect(Rectangle target)
		{
			var touchCollection = TouchPanel.GetState();
			foreach (TouchLocation tl in touchCollection)
			{
				if (tl.State == TouchLocationState.Pressed && 
				tl.Position.X  > target.Left &&
				tl.Position.X  > target.Right && 
				tl.Position.Y  > target.Top && 
				tl.Position.Y  > target.Bottom)  
    		 {
					return true;
				}
			}
			return false;
		}

		void UpdateGameplay(GameTime gameTime)
		{

			if (_vidas == 0)
			{
				playerDied = true;
				_state = GameState.EndOfGame;
			}

			var speed =  10 * (gameTime.TotalGameTime.Minutes + 1);

			_posicaoPaoDeMel.X -= speed;


			var touchCollection = TouchPanel.GetState();

			foreach (var touch in touchCollection)
			{
				var touchLocation = (int)touch.Position.Y - 100;
				if (touchLocation >= 1 && touchLocation <= 380)
				{
					position.Y = touchLocation;
				}
			
			}

			var isCollided = CollidesWith();

			if (_posicaoPaoDeMel.X <= 0)
			{
				if (!isCollided)
					_vidas -= 1;
				

				_posicaoPaoDeMel.X = 854;
				getNextRandon();
			}


			if (isCollided)
			{
				MediaPlayer.Play(songItemColetado);
				score += 1;
				_posicaoPaoDeMel.X = 854;
				getNextRandon();
			}


			if (playerDied)
				_state = GameState.EndOfGame;
		}

		protected override  void Update(GameTime gameTime)
		{

			base.Update(gameTime);

			switch (_state)
			{
				case GameState.MainMenu:
					UpdateMainMenu(gameTime);
					break;
				case GameState.Gameplay:
					UpdateGameplay(gameTime);
					break;
				case GameState.EndOfGame:
					UpdateEndOfGame(gameTime);
					break;
			}
		}

		protected override void Draw(GameTime deltaTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			base.Draw(deltaTime);

			switch (_state)
			{
				case GameState.MainMenu:
					DrawMainMenu(deltaTime);
					break;
				case GameState.Gameplay:
					DrawGameplay(deltaTime);
					break;
				case GameState.EndOfGame:
					DrawEndOfGame(deltaTime);
					break;
			}
		}

		void DrawMainMenu(GameTime deltaTime)
		{
			var ma = _viewportAdapter.GetScaleMatrix();

			spriteBatch.Begin(transformMatrix: ma);
			_botaoIniciar.Draw(spriteBatch, new Rectangle(250, 150, 300, 200));
			spriteBatch.End();

	
		}

		void DrawGameplay(GameTime deltaTime)
		{

			var ma = _viewportAdapter.GetScaleMatrix();

			spriteBatch.Begin(transformMatrix: ma);


			_background.Draw(spriteBatch, new Rectangle(0, 0, 854, 480));

			_jeff.Draw(spriteBatch, new Rectangle(40, (int)position.Y, 100, 100));

			_paoDeMel.Draw(spriteBatch, new Rectangle((int)_posicaoPaoDeMel.X,
													  (int)_posicaoPaoDeMel.Y, 50, 50));
			spriteBatch.DrawString(font, "Pontos: " + score + " Vidas: " + _vidas + " Tempo: " + deltaTime.ElapsedGameTime.ToString() , new Vector2(240, 20), Color.Black);

			spriteBatch.End();
		}

		void DrawEndOfGame(GameTime deltaTime)
		{
			var ma = _viewportAdapter.GetScaleMatrix();

			spriteBatch.Begin(transformMatrix: ma);
			spriteBatch.DrawString(font, "FIM DO GAME SEU MERDA! ", new Vector2(10, 200), Color.Black);
			_botaoIniciar.Draw(spriteBatch, new Rectangle(250, 150, 300, 200));
			spriteBatch.End();

			playerDied = true;
		}
		public bool CollidesWith()
		{
			return _jeff._posicaoInicial.Intersects(_paoDeMel._posicaoInicial);
		}

		void UpdateEndOfGame(GameTime deltaTime)
		{

			pushedMainMenuButton = touchSelect(_botaoIniciar._posicaoInicial);

			_botaoIniciar.Update(deltaTime);

			if (pushedMainMenuButton) {
				_vidas = 6;
				playerDied = false;
				_state = GameState.Gameplay;
			}

		}
		protected override void UnloadContent()
		{
			Content.Unload();
		}
	}
}
