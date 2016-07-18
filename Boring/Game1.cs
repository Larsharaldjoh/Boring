using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Boring
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;


		Texture2D block;
		Texture2D reinhardt;
		Texture2D transparentLogo;
		Vector2 position;
		Vector2 positionBlock;
		KeyboardState keyboardPreviousState;
		Song song;
		List<SoundEffect> soundEffects;
		SoundEffect soundEffect;
		SoundEffectInstance soundEffectInstance;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			positionBlock = new Vector2(0, 0);
			IsMouseVisible = true;
			soundEffects = new List<SoundEffect>();
			//this.IsFixedTimeStep = false;
			//this.graphics.SynchronizeWithVerticalRetrace = false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			MediaPlayer.Volume = 0.2f;

			block = new Texture2D(this.GraphicsDevice, 100, 100);
			Color[] colorData = new Color[100 * 100];
			for (int i = 0; i < 10000; i++ )
			{
				colorData[i] = Color.Red;
			}
			block.SetData<Color>(colorData);

			keyboardPreviousState = Keyboard.GetState();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Roboto");

			reinhardt	= this.Content.Load<Texture2D>("Reinhardt");
			position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
			transparentLogo	= this.Content.Load<Texture2D>("logo");


			this.song = Content.Load<Song>("song1");
			//MediaPlayer.Play(song);
			//MediaPlayer.IsRepeating = true;
			//MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
			soundEffects.Add(Content.Load<SoundEffect>("jump1"));
			soundEffects.Add(Content.Load<SoundEffect>("jump"));
			soundEffect = Content.Load<SoundEffect>("jump1");
			soundEffectInstance = soundEffect.CreateInstance();
			soundEffectInstance.IsLooped = true;
		}

		void MediaPlayer_MediaStateChanged(object sender, System.
										   EventArgs e)
		{
			//MediaPlayer.Volume -= 0.1f;
			MediaPlayer.Play(song);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Unload();

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (IsActive)
			{
				KeyboardState keyboardCurrentState = Keyboard.GetState();
				MouseState mouseCurrentState = Mouse.GetState();

				//position.X = mouseCurrentState.X;
				//position.Y = mouseCurrentState.Y;


				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				foreach (var key in keyboardCurrentState.GetPressedKeys())
					sb.Append("Key: ").Append(key).Append(" pressed ");

				if (sb.Length > 0)
					System.Diagnostics.Debug.WriteLine(sb.ToString());

				if (keyboardCurrentState.IsKeyDown(Keys.Right) || keyboardCurrentState.IsKeyDown(Keys.D))
					position.X += 5;
				if (keyboardCurrentState.IsKeyDown(Keys.Left) || keyboardCurrentState.IsKeyDown(Keys.A))
					position.X -= 5;
				if ((keyboardCurrentState.IsKeyDown(Keys.Up) || keyboardCurrentState.IsKeyDown(Keys.W)) && (!keyboardPreviousState.IsKeyDown(Keys.Up) && !keyboardPreviousState.IsKeyDown(Keys.W)))
				{
					position.Y -= 25;
					float volume = 1.0f;
					float pitch = 0.0f;
					float pan = 0.0f;
					soundEffect.Play(volume, pitch, pan);
					soundEffectInstance.Play();
					this.soundEffects[0].CreateInstance().Play();
					this.soundEffects[1].CreateInstance().Play();
					this.soundEffects[0].Play();
					this.soundEffects[1].Play();
					System.Diagnostics.Debug.WriteLine(soundEffectInstance.State.ToString());

				}
				if (keyboardCurrentState.IsKeyDown(Keys.Down) || keyboardCurrentState.IsKeyDown(Keys.S))
					position.Y += 5;

				keyboardPreviousState = keyboardCurrentState;

				positionBlock.X += PixelsPerSecond(300, gameTime);
				positionBlock.Y += PixelsPerSecond(100, gameTime);
				if (positionBlock.X > this.GraphicsDevice.Viewport.Width)
					positionBlock.X = 0;
				if (positionBlock.Y > this.GraphicsDevice.Viewport.Height)
					positionBlock.Y = 0;

				if (keyboardCurrentState.IsKeyDown(Keys.Escape))
					Exit();

				base.Update(gameTime);
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
			spriteBatch.Draw(block, 
				position: positionBlock,
				layerDepth: 0.7f
				);

			spriteBatch.Draw(reinhardt,
				destinationRectangle: new Rectangle((int)position.X, (int)position.Y, 125, 125),
				origin: new Vector2(reinhardt.Width/2, reinhardt.Height/2),
				rotation: 0f,
				layerDepth: 0.8f,
				effects: SpriteEffects.FlipHorizontally
				);
			spriteBatch.Draw(transparentLogo, 
				position: Vector2.Zero,
				scale: new Vector2(0.1f, 0.1f),
				layerDepth: 0.5f
				);
			spriteBatch.Draw(transparentLogo,
				position: new Vector2(75, 50),
				scale: new Vector2(0.1f, 0.1f),
				layerDepth: 1f
				);
			spriteBatch.End();

			cursorCoordinates();

			base.Draw(gameTime);
		}

		protected override void OnActivated(object sender, System.EventArgs args)
		{
			this.Window.Title = "Game";
			base.OnActivated(sender, args);
		}

		protected override void OnDeactivated(object sender, System.EventArgs args)
		{
			this.Window.Title = "Ded Gaem";
			base.OnDeactivated(sender, args);
		}

		protected float PixelsPerSecond(float pixels, GameTime gameTime)
		{
			return pixels * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		private void cursorCoordinates()
		{
			Point mousePosition = Mouse.GetState().Position;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, "X:" + mousePosition.X.ToString(), new Vector2(20, 15), Color.White);
			spriteBatch.DrawString(font, "Y:" + mousePosition.Y.ToString(), new Vector2(20, 30), Color.White);
			spriteBatch.End();

		}
	}
}
