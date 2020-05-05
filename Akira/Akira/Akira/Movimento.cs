using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Akira
{
    class Movimento
    {
        private int x;
        private int y;
        private int imageWidth;
        private int imageHeight;
        private int janelaWidth;
        private int janelaHeight;
        private int velocidade;
        private int sentido;  // 0= direita - esquerda, 1= esquerda- direita, 2= de cima- para baixo, 3= de baixo- para cima. 
        private Rectangle retangulo;
        private Texture2D textura;


        public void Initialize(Texture2D textura, int x, int y, int imageWidth, int imageHeigth,
              int janelaWidth, int janelaHeight, int velocidade, int sentido)
        {
            this.textura = textura;
            this.imageHeight = imageHeigth;
            this.imageWidth = imageWidth;
            this.janelaHeight = janelaHeight;
            this.janelaWidth = janelaWidth;
            this.velocidade = velocidade;
            this.sentido = sentido;
            this.x = x;
            this.y = y;

            retangulo = new Rectangle(x, y, imageWidth, imageHeigth);
        }

        public void Update()
        {

            if (sentido == 0)
            {
                x += velocidade;
                if (x == janelaWidth) x = 0;
            }
            else if (sentido == 1)
            {
                x -= velocidade;
                if (x == -imageWidth) x = janelaWidth - imageWidth;
            }
            else if (sentido == 2)
            {
                y += velocidade;
                if (y == janelaHeight) y = 0;
            }
            else
            {
                y -= velocidade;
                if (y == -imageHeight) y = janelaHeight - imageHeight;
            }
            retangulo.X = x;
            retangulo.Y = y;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, retangulo, Color.White);
        }
    }
}

