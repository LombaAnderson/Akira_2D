using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Akira.Sprites
{
    public class GenericSprite: DrawableGameComponent
    {
        private Game _game;

        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private SpriteBatch _spriteBatch;

        private float _rotationRate;
        #region Bounding Triangle
        public Vector2 TrianglePoint1
        {
            get { return Rotations.RotatePoint(_position - TextureReferencePoint, _position, RotationAngle); }
        }
        public Vector2 TriangleOffset1 { get; set; }
        public Vector2 TrianglePoint2
        {
            get { return Rotations.RotatePoint(_position - TextureReferencePoint + TriangleOffset1, _position, RotationAngle); }
        }
        public Vector2 TriangleOffset2 { get; set; }
        public Vector2 TrianglePoint3
        {
            get { return Rotations.RotatePoint(_position - TextureReferencePoint + TriangleOffset2, _position, RotationAngle); }
        }

        public List<Vector2> TrianglePoints
        {
            get
            {
                return new List<Vector2>()
                {
                    TrianglePoint1,TrianglePoint2,TrianglePoint3
                };
            }
        }
        #endregion
        #region Bounding Rectangle
        public Vector2 RectUpperLeftCorner
        {
            get { return new Vector2(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2); }
        }
        public int RectWidth
        {
            get { return _texture.Width; }
        }
        public int RectHeight
        {
            get { return _texture.Height; }
        }

        public List<Vector2> RectanglePoints
        {
            get
            {
                return new List<Vector2>()
                {
                     Rotations.RotatePoint((_position-TextureReferencePoint),_position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X + RectWidth, Y = (_position-TextureReferencePoint).Y }, _position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X + RectWidth, Y = (_position-TextureReferencePoint).Y + RectHeight }, _position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X, Y = (_position-TextureReferencePoint).Y + RectHeight }, _position, RotationAngle)
                };
            }

        }

        #endregion
        #region Bounding Circle
        public Vector2 CircleCenter
        {
            get { return new Vector2(_position.X, _position.Y); }
        }
        public int CircleRadius
        {
            get { return Math.Max(_texture.Width / 2, _texture.Height / 2); }
        }
        #endregion

        private Vector2 _textureReferencePoint;
        public Vector2 TextureReferencePoint
        {
            get { return _textureReferencePoint; }
            set { _textureReferencePoint = value; }
        }

        private float _rotationAngle;
        public float RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }

        public bool IsMovable { get; set; }

        private static int _callers = 0;

        public GenericSprite(Game game, string SpriteTexture, Vector2 InitialPosition)
            : base(game)
        {
            _position = InitialPosition;
            _game = game;
            _texture = _game.Content.Load<Texture2D>(SpriteTexture);
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            IsMovable = false;

            TextureReferencePoint = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Random Num = new Random(_callers++);
            _rotationRate = (float)Num.Next(30, 100) / 100.0f;
        }

        public void AddTriangleOffsets(Vector2 p1, Vector2 p2)
        {
            TriangleOffset1 = p1;
            TriangleOffset2 = p2;
        }

        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
            base.Dispose(disposing);
        }


        public override void Update(GameTime gameTime)
        {
            if (IsMovable)
            {
                MouseState MState = Mouse.GetState();
                Vector2 MouseVector = new Vector2(MState.X, MState.Y);
                _position = MouseVector;
            }

            float TimeElapsedInSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            RotationAngle += TimeElapsedInSeconds * _rotationRate;
            RotationAngle = (RotationAngle) % (2 * (float)Math.PI);

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, null, Color.White, RotationAngle, TextureReferencePoint, 1.0f, SpriteEffects.None, 0f);

            if (Globals.CDPerformedWith == UseForCollisionDetection.Circles)
                Primitives2D.DrawCircle(CircleCenter, CircleRadius, _spriteBatch);
            if (Globals.CDPerformedWith == UseForCollisionDetection.Rectangles)
                Primitives2D.DrawRectangle(RectanglePoints, _spriteBatch);
            if (Globals.CDPerformedWith == UseForCollisionDetection.Triangles)
                Primitives2D.DrawTriangle(TrianglePoints, _spriteBatch);
            base.Draw(gameTime);
        }
    }
}
