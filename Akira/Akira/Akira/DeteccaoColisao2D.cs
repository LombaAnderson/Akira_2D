using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Akira
{
    public static class DeteccaoColisao2D
    {
        public static RenderTarget2D AdditionalRenderTargetForCollision { get; set; }

        public static bool BoundingRectangle(List<Vector2> Rect1, List<Vector2> Rect2)
        {
            List<Vector2> Triangle11;
            List<Vector2> Triangle12;
            _createTrianglesFromRectangle(Rect1, out Triangle11, out Triangle12);
            foreach (Vector2 point in Rect2)
            {
                if (_isPointInsideTriangle(Triangle11, point)) return true;
                if (_isPointInsideTriangle(Triangle12, point)) return true;
            }
            List<Vector2> Triangle21;
            List<Vector2> Triangle22;
            _createTrianglesFromRectangle(Rect1, out Triangle21, out Triangle22);
            foreach (Vector2 point in Rect1)
            {
                if (_isPointInsideTriangle(Triangle21, point)) return true;
                if (_isPointInsideTriangle(Triangle22, point)) return true;
            }
            return false;
        }
        public static bool BoundingCircle(int x1, int y1, int radius1, int x2, int y2, int radius2)
        {
            Vector2 V1 = new Vector2(x1, y1);
            Vector2 V2 = new Vector2(x2, y2);

            Vector2 Distance = V1 - V2;

            if (Distance.Length() < radius1 + radius2)
                return true;

            return false;
        }

        public static bool BoundingTriangles(List<Vector2> p1, List<Vector2> p2)
        {
            for (int i = 0; i < 3; i++)
                if (_isPointInsideTriangle(p1, p2[i])) return true;

            for (int i = 0; i < 3; i++)
                if (_isPointInsideTriangle(p2, p1[i])) return true;
            return false;
        }

        public static bool PerPixelWR(Texture2D Texture1, Texture2D Texture2, Vector2 Pos1, Vector2 Pos2, Vector2 Orig1, Vector2 Orig2, List<Vector2> Rect1, List<Vector2> Rect2,
                                      float Theta1, float Theta2, SpriteBatch spriteBatch)
        {

            if (!BoundingRectangle(Rect1, Rect2)) return false;
            Color[] TextureData1 = _akiraeinimigos(spriteBatch, Texture1, Pos1, Orig1, Theta1, Rect1);
            Color[] TextureData2 = _akiraeinimigos(spriteBatch, Texture2, Pos2, Orig2, Theta2, Rect2);

            Rectangle Rectangle1 = _getBoundingRectangleOfRotatedRectangle(Rect1);
            Rectangle Rectangle2 = _getBoundingRectangleOfRotatedRectangle(Rect2);

            int top = Math.Max(Rectangle1.Top, Rectangle2.Top);
            int bottom = Math.Min(Rectangle1.Bottom, Rectangle2.Bottom);
            int left = Math.Max(Rectangle1.Left, Rectangle2.Left);
            int right = Math.Min(Rectangle1.Right, Rectangle2.Right);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color colorA = TextureData1[(x - Rectangle1.Left) + (y - Rectangle1.Top) * Rectangle1.Width];
                    Color colorB = TextureData2[(x - Rectangle2.Left) + (y - Rectangle2.Top) * Rectangle2.Width];
                    if (colorA != new Color(0, 0, 0, 0) && colorB != new Color(0, 0, 0, 0)) return true;
                }

            return false;
        }

        private static Color[] _akiraeinimigos(SpriteBatch spriteBatch, Texture2D texture1, Vector2 pos1, Vector2 orig1, float theta1, List<Vector2> rect1)
        {
            throw new NotImplementedException();
        }
        #region Private memebers
        private static bool _isPointInsideTriangle(List<Vector2> TrianglePoints, Vector2 p)
        {

            Vector2 e0 = p - TrianglePoints[0];
            Vector2 e1 = TrianglePoints[1] - TrianglePoints[0];
            Vector2 e2 = TrianglePoints[2] - TrianglePoints[0];

            float u, v = 0;
            if (e1.X == 0)
            {
                if (e2.X == 0) return false;
                u = e0.X / e2.X;
                if (u < 0 || u > 1) return false;
                if (e1.Y == 0) return false;
                v = (e0.Y - e2.Y * u) / e1.Y;
                if (v < 0) return false;
            }
            else
            {
                float d = e2.Y * e1.X - e2.X * e1.Y;
                if (d == 0) return false;
                u = (e0.Y * e1.X - e0.X * e1.Y) / d;
                if (u < 0 || u > 1) return false;
                v = (e0.X - e2.X * u) / e1.X;
                if (v < 0) return false;
                if ((u + v) > 1) return false;
            }

            return true;
        }
        private static Rectangle _getBoundingRectangleOfRotatedRectangle(List<Vector2> RectanglePoints)
        {
            Vector2 BoundingRectangleStart = new Vector2()
            {
                X = _getMinimumOf(RectanglePoints[0].X, RectanglePoints[1].X, RectanglePoints[2].X, RectanglePoints[3].X),
                Y = _getMinimumOf(RectanglePoints[0].Y, RectanglePoints[1].Y, RectanglePoints[2].Y, RectanglePoints[3].Y)
            };

            int BoundingRectangleWidth = -(int)BoundingRectangleStart.X + _getMaximumOf(RectanglePoints[0].X, RectanglePoints[1].X, RectanglePoints[2].X, RectanglePoints[3].X);
            int BoundingRectangleHeight = -(int)BoundingRectangleStart.Y + _getMaximumOf(RectanglePoints[0].Y, RectanglePoints[1].Y, RectanglePoints[2].Y, RectanglePoints[3].Y);

            return new Rectangle((int)BoundingRectangleStart.X, (int)BoundingRectangleStart.Y, BoundingRectangleWidth, BoundingRectangleHeight);

        }
        private static int _getMinimumOf(float a1, float a2, float a3, float a4)
        {
            return (int)MathHelper.Min(MathHelper.Min(MathHelper.Min(a1, a2), a3), a4);
        }
        private static int _getMaximumOf(float a1, float a2, float a3, float a4)
        {
            return (int)MathHelper.Max(MathHelper.Max(MathHelper.Max(a1, a2), a3), a4);
        }
       // private static Color[] _akiraeinimigos(SpriteBatch spriteBatch, Texture2D textureToDraw, Vector2 Position, Vector2 Origin, float Theta, List<Vector2> RectanglePoints)
        //{
            /*spriteBatch.GraphicsDevice.SetRenderTarget(0, AdditionalRenderTargetForCollision);
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(textureToDraw, Position, null, Color.White, Theta, Origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(2, 0);
            Texture2D SceneTexture = AdditionalRenderTargetForCollision.GetTexture();
            Rectangle BoundingRectangle = _getBoundingRectangleOfRotatedRectangle(RectanglePoints);
            int PixelSize = BoundingRectangle.Width * BoundingRectangle.Height;
            Color[] TextureData = new Color[PixelSize];
            SceneTexture.GetData(0, BoundingRectangle, TextureData, 0, PixelSize);

            return TextureData;*/
        //}
        private static void _createTrianglesFromRectangle(List<Vector2> RectPoints, out List<Vector2> Triangle1, out List<Vector2> Triangle2)
        {
            Triangle1 = new List<Vector2>()
            {
                new Vector2(RectPoints[0].X,RectPoints[0].Y),
                new Vector2(RectPoints[1].X,RectPoints[1].Y),
                new Vector2(RectPoints[3].X,RectPoints[3].Y),
            };
            Triangle2 = new List<Vector2>()
            {
                new Vector2(RectPoints[1].X,RectPoints[1].Y),
                new Vector2(RectPoints[2].X,RectPoints[2].Y),
                new Vector2(RectPoints[3].X,RectPoints[3].Y),
            };
        }
        #endregion
    }
}
