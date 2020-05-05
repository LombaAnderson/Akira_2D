using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Akira.Sprites;

namespace Akira

{
    public class MainGameLoop : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GenericSprite _sprite1;
        private GenericSprite _sprite2;

        private GenericSprite _movableSprite;
        private MessageSprite _messageSprite;


        public MainGameLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


        }
        protected override void Initialize()
        {
            base.Initialize();
            Globals.CDPerformedWith = UseForCollisionDetection.PerPixel;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            _sprite1 = new GenericSprite(this, "akira1", new Vector2(500, 400));
            _sprite1.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));

            _sprite2 = new GenericSprite(this, "akira2", new Vector2(300, 300));
            _sprite2.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));

            _movableSprite = new GenericSprite(this, "Inimigo1", new Vector2(400, 300));
            _movableSprite.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));
            _movableSprite.IsMovable = true;

            _messageSprite = new MessageSprite(this, "================== COLISÃO DETECTADA ==================");
            _messageSprite.Visible = false;

            Primitives2D.dotTexture = Content.Load<Texture2D>("Dot");
           // DeteccaoColisao2D.AdditionalRenderTargetForCollision = new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 1, _graphics.GraphicsDevice.DisplayMode.Format);

            Components.Add(_sprite1);
            Components.Add(_sprite2);
            Components.Add(_movableSprite);
            Components.Add(_messageSprite);

        }
        protected override void UnloadContent()
        {
            _sprite1.Dispose();
            _sprite2.Dispose();
            _movableSprite.Dispose();
            _messageSprite.Dispose();
            Primitives2D.dotTexture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            _messageSprite.Visible = false;

            if ((_movableSprite.Position.X > _movableSprite.Texture.Width) && (_movableSprite.Position.X < _graphics.PreferredBackBufferWidth - _movableSprite.Texture.Width) &&
                (_movableSprite.Position.Y > _movableSprite.Texture.Height) && (_movableSprite.Position.Y < _graphics.PreferredBackBufferHeight - _movableSprite.Texture.Height))
            {

                #region Collision Detection with Bounding Rectangles
                if (Globals.CDPerformedWith == UseForCollisionDetection.Rectangles)
                {
                    if (DeteccaoColisao2D.BoundingRectangle(_movableSprite.RectanglePoints, _sprite1.RectanglePoints))
                        _messageSprite.Visible = true;
                    if (DeteccaoColisao2D.BoundingRectangle(_movableSprite.RectanglePoints, _sprite2.RectanglePoints))
                        _messageSprite.Visible = true;
                }
                #endregion

                #region Collision Detection with Bounding Circles
                if (Globals.CDPerformedWith == UseForCollisionDetection.Circles)
                {
                    if (DeteccaoColisao2D.BoundingCircle((int)_movableSprite.CircleCenter.X, (int)_movableSprite.CircleCenter.Y,
                                                               _movableSprite.CircleRadius,
                                                               (int)_sprite1.CircleCenter.X, (int)_sprite1.CircleCenter.Y,
                                                               _sprite1.CircleRadius))
                        _messageSprite.Visible = true;
                    if (DeteccaoColisao2D.BoundingCircle((int)_movableSprite.CircleCenter.X, (int)_movableSprite.CircleCenter.Y,
                                                               _movableSprite.CircleRadius,
                                                               (int)_sprite2.CircleCenter.X, (int)_sprite2.CircleCenter.Y,
                                                               _sprite2.CircleRadius))
                        _messageSprite.Visible = true;
                }
                #endregion

                #region Collision Detection with Bounding Triangle
                if (Globals.CDPerformedWith == UseForCollisionDetection.Triangles)
                {
                    if (DeteccaoColisao2D.BoundingTriangles(_movableSprite.TrianglePoints, _sprite1.TrianglePoints))
                        _messageSprite.Visible = true;

                    if (DeteccaoColisao2D.BoundingTriangles(_movableSprite.TrianglePoints, _sprite2.TrianglePoints))
                        _messageSprite.Visible = true;
                }
                #endregion


                #region Collision Detection with PerPixel
                if (Globals.CDPerformedWith == UseForCollisionDetection.PerPixel)
                {
                    if (DeteccaoColisao2D.PerPixelWR(_movableSprite.Texture, _sprite1.Texture, _movableSprite.Position, _sprite1.Position, _movableSprite.TextureReferencePoint, _sprite1.TextureReferencePoint, _movableSprite.RectanglePoints, _sprite1.RectanglePoints, _movableSprite.RotationAngle, _sprite1.RotationAngle, _spriteBatch))
                        _messageSprite.Visible = true;

                    if (DeteccaoColisao2D.PerPixelWR(_movableSprite.Texture, _sprite2.Texture, _movableSprite.Position, _sprite2.Position, _movableSprite.TextureReferencePoint, _sprite2.TextureReferencePoint, _movableSprite.RectanglePoints, _sprite2.RectanglePoints, _movableSprite.RotationAngle, _sprite2.RotationAngle, _spriteBatch))
                        _messageSprite.Visible = true;
                }
                #endregion

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}

  
