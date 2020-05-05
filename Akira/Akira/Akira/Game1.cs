using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Akira
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Random rnd { get; private set; }

        List<Texture2D> backgrounds;
        // List<Texture2D> segmentos;

        Akira akira;
        Animacao inimigo;
        


        int currentBackground;
        int height;
        int width;
        KeyboardState previousKeyboard;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            width = 900;
            height = 514;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            // IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            Components.Clear();

            currentBackground = 0;
            backgrounds = new List<Texture2D>();
            // segmentos = new List<Texture2D>();

            akira = new Akira();
            inimigo = new Animacao();



            rnd = new Random();

            base.Initialize();

        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 1; i <= 4; i++)     //<=1 é o numero de backgrounds baixados
            {
                backgrounds.Add(Content.Load<Texture2D>("Backgrounds/b" + i.ToString()));
                //segmentos.Add(Content.Load<Texture2D>("Backgrounds/s" + i.ToString()));
            }


            /*akira_ = Content.Load<Texture2D>("Akira_/akira2");
            akira_ = Content.Load<Texture2D>("Akira_/akira1");
            akira_ = Content.Load<Texture2D>("Akira_/akira_ataque");
            akira_ = Content.Load<Texture2D>("Akira_/akira_ataque_esq");
            inimigo_ = Content.Load<Texture2D>("Inimigos/Inimigo1");*/
     


              akira.Initialize(
                 Content.Load<Texture2D>("Akira_/akira1"),
                 Content.Load<Texture2D>("Akira_/akira_ataque"),
                 Content.Load<Texture2D>("Akira_/akira_ataque_esq"),
                 Content.Load<Texture2D>("Akira_/Shuriken_LeftRight"),
                 Content.Load<Texture2D>("Akira_/Shuriken_RightLeft"),
                 Content.Load<SoundEffect>("Sons/Espada"),
                 6, 106, 106, 1f,
                 4, 173, 127, 120, 1f,
                 120, height - 80, 70, width);

            Texture2D InimigoTexture = Content.Load<Texture2D>("Inimigos/Inimigo1");
            inimigo.Initialize(InimigoTexture, new Vector2(600, 432), 98, 108, 5, 90, Color.White, 1f, true);

            /*Texture2D InimigoTexture1 = Content.Load<Texture2D>("Inimigos/Inimigo1");
            inimigo02.Initialize(InimigoTexture1, new Vector2(50, 432), 98, 108, 5, 90, Color.White, 1f, true);*/

        }

        protected override void UnloadContent()
        {

        }

        private void Animate(GameTime gameTime)
        {
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
               Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();



            // MouseState mouse = Mouse.GetState();

            KeyboardState keyboard = Keyboard.GetState();
            previousKeyboard = keyboard;

            if (keyboard.IsKeyDown(Keys.Left)) akira.MoveLeft();

            if (keyboard.IsKeyDown(Keys.Right)) akira.MoveRight();

            if (keyboard.IsKeyDown(Keys.Z) && !previousKeyboard.IsKeyDown(Keys.Z)) akira.Espada();
            if (keyboard.IsKeyDown(Keys.A) && !previousKeyboard.IsKeyDown(Keys.A)) akira.Shuriken();



            akira.Update(gameTime);
            inimigo.Update(gameTime);

     



            checkWorld(gameTime);

            base.Update(gameTime);


        }

        private void checkWorld(GameTime gameTime)
        {
            if (currentBackground == 0 && akira.X < 0)
            {
                akira.X = 0;
            }
            if (currentBackground == 0 && akira.X > width)
            {
                akira.X = 0;
                currentBackground++;
                akira.Lancements.Clear();
            }

            if (currentBackground == 1 && akira.X < 0)
            {
                akira.X = 0;
            }
            if (currentBackground == 1 && akira.X > width)
            {
                akira.X = 0;
                currentBackground++;
                akira.Lancements.Clear();
            }

            if (currentBackground == 2 && akira.X < 0)
            {
                akira.X = 0;
            }
            if (currentBackground == 2 && akira.X > width)
            {
                akira.X = 0;
                currentBackground++;
                akira.Lancements.Clear();
            }

    }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgrounds[currentBackground], new Vector2(0, 0), Color.White);

            akira.Draw(spriteBatch);
            inimigo.Draw(spriteBatch);
            // inimigo02.Draw(spriteBatch);

            //spriteBatch.Draw(segmentos[currentBackground], new Vector2(0, 0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

}
    