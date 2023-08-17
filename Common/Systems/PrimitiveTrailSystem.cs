using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace PenumbraMod.Common.Systems
{
    internal class PrimitiveTrailSystem
    {
        public static GraphicsDevice _device = Main.instance.GraphicsDevice;
        public static BasicEffect effect = new BasicEffect(_device);

        public static Vector2 normal;
        public Color color;

        public bool isDissapearing;
        public List<Vector2> oldPositions = new List<Vector2>();
        public Vector2 pos;
        public float width;

        public void Draw(Color color, Vector2 pos, List<Vector2> oldPositions, float width = 3f)
        {
            this.color = color;
            this.pos = pos;
            this.oldPositions = oldPositions;
            this.width = width;

            float colorAlpha = color.A;

            VertexPositionColor left, right;
            left = new VertexPositionColor(new Vector3(pos - normal, 0), color);
            right = new VertexPositionColor(new Vector3(pos + normal, 0), color);

            var vertices = new VertexPositionColor[oldPositions.Count * 2 + 1];
            vertices[0] = left;
            vertices[1] = right;

            for (var c = 1; c < oldPositions.Count; c++)
            {
                var prev = oldPositions[oldPositions.Count - c - 1];
                var curr = oldPositions[oldPositions.Count - c];
                var p = c / (float) oldPositions.Count;
                p = 1 - p;
                normal = Vector2.Normalize(curr - prev).RotatedBy(MathHelper.PiOver2) * width;
                vertices[c * 2] = new VertexPositionColor(new Vector3(curr + p * normal, 0), color);
                vertices[c * 2 + 1] = new VertexPositionColor(new Vector3(curr - p * normal, 0), color);
            }

            effect.VertexColorEnabled = true;
            effect.World = Matrix.CreateTranslation(-new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0));
            effect.View = Main.GameViewMatrix.TransformationMatrix;
            var viewport = Main.instance.GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 1);
            _device.RasterizerState = RasterizerState.CullNone;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                if (vertices.Length >= 3)
                    _device.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length - 4);
            }
        }
    }
}