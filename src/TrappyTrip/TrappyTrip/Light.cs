using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TrappyTrip
{
    public class Light
    {
        public Color Color { get; set; }
        public int Range { get; set; }
        public Vector2 Position { get; set; }

        private Entity parentEntity { get; set; }        

        public Light(Color lightColor, int lightRange, Vector2 lightPosition)
        {
            Color = lightColor;
            Range = lightRange;
            Position = lightPosition;
        }

        public Color GetColor(Vector2 position)
        {
            int rangeToLight = (int)(position - this.Position).Length();

            int changeValue = (int)((255 / Range) * rangeToLight);
            if (changeValue > 255) changeValue = 255;

            return new Color(this.Color.R - changeValue, this.Color.G - changeValue, this.Color.B - changeValue);
        }

        public void FollowEntity(Entity entity)
        {
            parentEntity = entity;
        }

        public bool FollowsEntity
        {
            get { return parentEntity != null; }
        }


        public void update(GameTime gameTime)
        {
            if (FollowsEntity)
            {
                Position = parentEntity.Center;
            }
        }
    }
}
