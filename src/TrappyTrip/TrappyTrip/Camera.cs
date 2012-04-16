using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrappyTrip
{
    public class Camera
    {
        public Vector2 position { get; set; }
        private float zoom;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation { get; set; }

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
        
        public Entity ParentEntity;

        public Camera(int width, int height)
        {
            ViewportHeight = height;
            ViewportWidth = width;
        }

        public void Update(GameTime gameTime)
        {
            if (ParentEntity != null)
            {
                position = new Vector2(ParentEntity.X, ParentEntity.Y);
            }
        }

        public void FollowEntity(Entity entity)
        {
            ParentEntity = entity;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        }
    }
}
