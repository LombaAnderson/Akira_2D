using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Akira
{
    class Akira
    {
        private Texture2D walkLeft;
        private Texture2D walkRight;
        private Texture2D attackLeft;
        private Texture2D attackRight;
        private Texture2D shurikenLeft;
        private Texture2D shurikenRight;
        private SoundEffect espadaSom;
        private List<Shuriken1> lancements;
        private int frameCountWalk;
        private int frameWidthWalk;
        private int frameHeightWalk;
        private float scaleWalk;
        private int frameCountAttack;
        private int frameWidthAttack;
        private int frameHeightAttack;
        private int frameTimeAttack;
        private float scaleAttack;
        private int x;
        private int y;
        private int currentFrameWalk;
        private int currentFrameAttack;
        private Rectangle sourceRect = new Rectangle();
        private Rectangle destinationRect = new Rectangle();
        private Direcao direcao;
        private int speed;
        private bool espada;
        private int elapsedTime;
        private int shurikens; //revisar
        private int widthScreen;

        internal List<Shuriken1> Lancements  //revisar
        {
            get { return lancements; }
            set { lancements = value; }
        }

        public int X //revisar
        {
            get { return x; }
            set { x = value; }
        }

        internal Direcao Direcao
        {
            get { return Direcao; }
            set { Direcao = value; }
        }
       
        public void Initialize(
        Texture2D walkLeft, Texture2D walkRight, 
        Texture2D attackLeft, Texture2D attackRight, 
        Texture2D shurikenLeft, Texture2D shurikenRight, //revisar
        SoundEffect espadaSom, 
        int frameCountWalk,int frameWidthWalk, int frameHeightWalk,
        float scaleWalk, int frameCountAttack, int frameWidthAttack, int frameHeightAttack, int frameTimeAttack, float scaleAttack, 
        int x, int y, int shurikens, int widthScreen)
        {

            this.walkLeft = walkLeft;
            this.walkRight = walkRight;
            this.attackLeft = attackLeft;
            this.attackRight = attackRight;
            this.shurikenLeft = shurikenLeft;
            this.shurikenRight = shurikenRight;
            this.frameCountWalk = frameCountWalk;
            this.scaleWalk = scaleWalk;
            this.scaleAttack = scaleAttack;
            this.frameWidthWalk = frameWidthWalk;
            this.frameHeightWalk = frameHeightWalk;
            this.frameWidthAttack = frameWidthAttack;
            this.frameHeightAttack = frameHeightAttack;
            this.frameTimeAttack = frameTimeAttack;
            this.frameCountAttack = frameCountAttack;
            this.espadaSom = espadaSom;
            this.x = x;
            this.y = y;
            this.shurikens = shurikens;
            this.currentFrameWalk = 0;
            this.currentFrameAttack = 0;
            this.direcao = Direcao.LeftRight;
            this.speed = 3;
            this.espada = false;
            this.lancements = new List<Shuriken1>();
            this.widthScreen = widthScreen;
        }

        public void Update(GameTime gameTime)
        {
            if (!espada)
            {
                sourceRect = new Rectangle(currentFrameWalk * frameWidthWalk, 0, frameWidthWalk, frameHeightWalk);
                destinationRect = new Rectangle(
                      x - (int)(frameWidthWalk * scaleWalk) / 2,
                      y - (int)(frameHeightWalk * scaleWalk) / 2,
                         (int)(frameWidthWalk * scaleWalk),
                         (int)(frameHeightWalk * scaleWalk));
            }
            else
            {
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsedTime > frameTimeAttack)
                {
                    currentFrameAttack++;
                    if (currentFrameAttack == frameCountAttack)
                    {
                        currentFrameAttack = 0;
                        espada = false;
                    }

                    elapsedTime = 0;
                }

                sourceRect = new Rectangle(currentFrameAttack * frameWidthAttack,
                    0, frameWidthAttack, frameHeightAttack);

                destinationRect = new Rectangle(
                   x - (int)(frameWidthAttack * scaleAttack) / 2,
                   y - (int)(frameHeightAttack * scaleAttack) / 2,
                   (int)(frameWidthAttack * scaleAttack),
                   (int)(frameHeightAttack * scaleAttack));
            }
            UpdateShurikens(gameTime);
        }

        private void UpdateShurikens(GameTime gameTime)
        {
            for( int i =0; i < lancements.Count; i++)
            {
                if (lancements[i].Direcao == Direcao.LeftRight && lancements[i].X > widthScreen)
                {
                    lancements.RemoveAt(i);
                }
                else if (lancements[i].Direcao == Direcao.RightLeft && lancements[i].X < 0)
                 {
                    lancements.RemoveAt(i);
                 }
                else
                {
                    lancements[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!espada)
            {
                if (direcao == Direcao.LeftRight)
                {
                    spriteBatch.Draw(walkRight, destinationRect, sourceRect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(walkLeft, destinationRect, sourceRect, Color.White);
                }
            }
            else
            {
                if (direcao == Direcao.RightLeft)
                {
                    spriteBatch.Draw(attackRight, destinationRect, sourceRect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(attackLeft, destinationRect, sourceRect, Color.White);
                }
            }
            DrawShurikens(spriteBatch);

        }

        private void DrawShurikens(SpriteBatch spriteBatch)
        {
            foreach(Shuriken1 lancement in lancements)
            {
                lancement.Draw(spriteBatch);
            }
        }

        internal void MoveLeft()
        {
            if (espada) return;
            direcao = Direcao.RightLeft;
            x -= speed;
            currentFrameWalk--;
            if (currentFrameWalk <= 0) currentFrameWalk = frameCountWalk;

        }

        internal void MoveRight()
        {
            if (espada) return;
            direcao = Direcao.LeftRight;
            x += speed;
            if (x % 5 == 0) currentFrameWalk++;
            currentFrameWalk++;
            if (currentFrameWalk >= frameCountWalk) currentFrameWalk = 0;
        }

        internal void Espada()
        {
            espadaSom.Play();
            espada = true;
        }

        internal void Shuriken()
        {
            if (shurikens >= 0) return;
            Shuriken1 lancement = new Shuriken1();
            lancement.Initialize(shurikenLeft, shurikenRight, direcao, x, y -15);
            lancements.Add(lancement);
            shurikens--;
        }

    }
}
