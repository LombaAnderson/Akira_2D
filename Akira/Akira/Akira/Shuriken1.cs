using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Akira
{
    class Shuriken1
    {
        private Texture2D texturaLeft;
        private Texture2D texturaRight;
        private Direcao direcao;
        private int x;
        private int y;
        private Rectangle retangulo;
        private int speed;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        internal Direcao Direcao
        {
            get { return Direcao; }
            set { Direcao = value; }
        }

        public void Initialize(Texture2D texturaLeft,Texture2D texturaRight, Direcao direcao, int x, int y)
        {
            this.texturaLeft = texturaLeft;
            this.texturaRight = texturaRight;
            this.direcao = direcao;
            this.x = x;
            this.y = y;
            this.retangulo = new Rectangle(x, y, 114,114);
            this.speed = 3;
        }

        public void Update(GameTime gameTime)
        {
              if(direcao== Direcao.LeftRight)
            {
                x += speed;
            }
              else
            {
                x -= speed;
            }
            retangulo.X = x;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (direcao == Direcao.LeftRight)
            {
                spriteBatch.Draw(texturaRight, retangulo, Color.White);

            }
            else
            {
                spriteBatch.Draw(texturaLeft, retangulo, Color.White);
            }
        }
    }
}
