using HexagonHeroes.Client.Resources;
using HexagonHeroes.Client.States.Game;
using HexagonHeroes.Client.States.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexagonHeroes.Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("spriteFont");
            Textures.LoadTextures(Content);
            Sounds.LoadSounds(Content);
            MenuState.Activate();
        }

        protected override void Update(GameTime gameTime)
        {
            MouseManager.Update();

            GameState.Update();
            MenuState.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            GameState.Draw(_spriteBatch);
            MenuState.Draw(_spriteBatch, _spriteFont);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
