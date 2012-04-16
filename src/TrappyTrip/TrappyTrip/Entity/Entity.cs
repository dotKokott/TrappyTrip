using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TrappyTrip
{
    public class Entity
    {
        public Game1 Game;

        public Dictionary<String, Animation> Animations = new Dictionary<string, Animation>();
        public string CurrentAnimation;

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsCollidable { get; set; }
        public int BoundingOffsetX { get; set; }
        public int BoundingOffsetY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
                
        public bool IsAnimated { get; set; }
        public EntityDirection Direction { get; set; }

        public bool DrawBoundingBox { get; set; }

        public Texture2D Sprite { get; set; }

        public Vector2 Velocity = new Vector2(0, 0);

        public Entity(Game1 game, int x, int y)
        {
            Game = game;
            X = x;
            Y = y;

            BoundingOffsetX = 0;
            BoundingOffsetY = 0;
            IsCollidable = true;
            DrawBoundingBox = false;
        }

        public List<Vector2> GetCollisionPoints(Rectangle boundingBox)
        {            
            List<Vector2> positions = new List<Vector2>();

            positions.Add(new Vector2(boundingBox.Left, boundingBox.Top));
            positions.Add(new Vector2(boundingBox.Left, boundingBox.Bottom));
            positions.Add(new Vector2(boundingBox.Right, boundingBox.Top));
            positions.Add(new Vector2(boundingBox.Right, boundingBox.Bottom));


            return positions;
        }

        public bool CollidesWithMap(Rectangle boundingBox)
        {
            foreach (Vector2 v in GetCollisionPoints(boundingBox))
            {
                if(CollidesWithMap((int)v.X, (int)v.Y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CollidesWithMap(int worldX, int worldY)
        {
            Tile currentTile = Game.tileMap.GetTileByCoordinates(worldX, worldY);
            if (!currentTile.IsPassable())
            {
                return new Rectangle(worldX, worldY, 1,1).Intersects(currentTile.GetRectangle());
            }

            return false;
        }

        public void AddAnimation(Animation animation)
        {
            if(!Animations.ContainsKey(animation.Name))
                Animations.Add(animation.Name, animation);
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle(X + BoundingOffsetX, Y + BoundingOffsetY, Width, Height);
        }

        public Vector2 Center
        {
            get { return new Vector2(GetBoundingBox().Center.X, GetBoundingBox().Center.Y); }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsCollidable)
            {
                for (int i = 0; i < Math.Abs(Velocity.X); i++)
                {
                    Rectangle nextHorizontalBoundingBox = new Rectangle((int)(GetBoundingBox().X + Math.Sign(Velocity.X)), (int)(GetBoundingBox().Y), GetBoundingBox().Width, GetBoundingBox().Height);

                    if (!CollidesWithMap(nextHorizontalBoundingBox))
                    {
                        X += Math.Sign(Velocity.X);
                    }
                    else
                    {
                        Velocity.X = 0;
                        break;
                    }

                }
                
                for (int i = 0; i < Math.Abs(Velocity.Y); i++)
                {
                    Rectangle nextVerticalBoundingBox = new Rectangle((int)(GetBoundingBox().X), (int)(GetBoundingBox().Y + Math.Sign(Velocity.Y)), GetBoundingBox().Width, GetBoundingBox().Height);

                    if (!CollidesWithMap(nextVerticalBoundingBox))
                    {
                        Y += Math.Sign(Velocity.Y);
                    }
                    else
                    {
                        Velocity.Y = 0;
                        break;
                    }                   
                }

            }
            else
            {
                X += (int)Velocity.X;
                Y += (int)Velocity.Y;
            }
                       
            if(Animations.ContainsKey(CurrentAnimation))
            {
                Animations[CurrentAnimation].update(gameTime);
            }
        }

        public virtual SpriteEffects GetSpriteEffect()
        {
            switch (Direction)
            {
                case EntityDirection.Right:
                    return SpriteEffects.None;
                    break;
                case EntityDirection.Left:
                    return SpriteEffects.FlipHorizontally;
                    break;
                default:
                    return SpriteEffects.None;
                    break;
            }
        }

        public virtual void Draw(ExtendedSpriteBatch spriteBatch)
        {
            if (IsAnimated)
            {
                if (Animations.ContainsKey(CurrentAnimation))
                {
                    spriteBatch.Draw(Animations[CurrentAnimation].SpriteSheet, new Rectangle(X, Y, Animations[CurrentAnimation].FrameWidth, Animations[CurrentAnimation].FrameHeight), Animations[CurrentAnimation].FrameRectangle, Color.White, 0.0f, new Vector2(0, 0), GetSpriteEffect(), 0);
                }
            }
            else
            {
                spriteBatch.Draw(Sprite, new Rectangle(X, Y, Sprite.Width, Sprite.Height), Color.White);
            }

            if (DrawBoundingBox)
            {
                spriteBatch.DrawRectangle(GetBoundingBox(), Color.Red);
            }
        }
    }
}
