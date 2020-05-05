using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Akira
{
    class Deslocamentos
    {
        private int x1;
        private int x2;
        private int y1;
        private int y2;
        private int imageWidth;
        private int imageHeight;
        private int janelaWidth;
        private int janelaHeight;
        private int velocidade;
        private int sentido;

        private Rectangle rec1;
        private Rectangle rec2;
        private Texture2D textura;

        public void Initialize(Texture2D textura, int x, int y, int imageWidth, int imageHeight,
            int janelaWidth, int janelaHeight, int velocidade, int sentido)
        {
            this.textura = textura;
            this.imageHeight = imageHeight;
            this.imageWidth = imageWidth;
            this.janelaHeight = janelaHeight;
            this.janelaWidth = janelaWidth;
            this.velocidade = velocidade;
            this.sentido = sentido;
            this.x1 = x;
            this.y1 = y;

            if (sentido == 0)
            {
                rec1 = new Rectangle(x1, y1, imageWidth, imageHeight);
                x2 = x1 + janelaWidth;
                rec2 = new Rectangle(x2, y1, imageHeight, imageHeight);
            }
            else if (sentido == 1)
            {
                rec1 = new Rectangle(x1, y1, imageWidth, imageHeight);
                x2 = x1 - janelaWidth;
                rec2 = new Rectangle(x2, y1, imageHeight, imageHeight);

            }

        }

        public void Update()
        {
            if (sentido == 0)
            {
                x1 -= velocidade;
                x2 -= velocidade;
            }
            else if (sentido == 1)
            {
                x1 += velocidade;
                x2 += velocidade;

            }
            else if (sentido == 2)
            {
                y1 -= velocidade;
                y2 -= velocidade;
            }
            else
            {
                y1 += velocidade;
                y2 += velocidade;
            }

            if (sentido == 0 || sentido == 1)
            {
                rec1 = new Rectangle(x1, y1, imageWidth, imageHeight);
                rec2 = new Rectangle(x2, y1, imageWidth, imageHeight);
            }
            else
            {
                rec1 = new Rectangle(x1, y1, imageWidth, imageHeight);
                rec2 = new Rectangle(x1, y2, imageWidth, imageHeight);
            }

            if (sentido == 0)
            {
                if (rec1.X == -janelaWidth)
                {
                    x1 = 0;
                }

                if (rec2.X == 0)
                {
                    x2 = janelaWidth;
                }
            }
            else if (sentido == 1)
            {
                if (rec1.X == janelaWidth)
                {
                    x1 = 0;
                }
                if (rec2.X == 0)
                {
                    x2 = -janelaWidth;
                }
            }

            else if (sentido == 2)
            {
                if(rec1.Y == -janelaHeight)
                {
                    y1 = 0;
                }
                if(rec2.Y == 0)
                {
                    y2 = janelaHeight;
                }
            }
            else
            {
                if (rec1.Y == janelaHeight)
                {
                    y1 = 0;
                }
                if(rec2.Y == 0)
                {
                    y2 = -janelaHeight;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, rec1, Color.White);
            spriteBatch.Draw(textura, rec2, Color.White);
        }
    }
}