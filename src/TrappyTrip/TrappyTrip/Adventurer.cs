using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace TrappyTrip
{
    class Adventurer : Entity
    {
        public TouchCollection oldState { get; set; }
        public TouchCollection newState { get; set; }

        public bool IsJumping { get; set; }

        public Adventurer(Game1 game, int x, int y) : base(game, x, y)            
        {
            IsAnimated = true;
            //DrawBoundingBox = true;

            Width = 33;
            Height = 58;

            BoundingOffsetX = 30;
            BoundingOffsetY = 35;

            AddAnimation(new Animation("idle", game.Content.Load<Texture2D>("player_idle"), 96));

            AddAnimation(new Animation("run", game.Content.Load<Texture2D>("player_run"), 96));
                                                         
            CurrentAnimation = "idle";
        }

        public void Update(GameTime gameTime)
        {
            newState = TouchPanel.GetState();            
            bool moveRight = false;
            bool moveLeft = false;
            bool jump = false;
            foreach (TouchLocation tl in newState)
            {
                if (tl.State == TouchLocationState.Moved)
                {
                    if (tl.Position.X >= 400)
                    {
                        moveRight = true;
                    }
                    else
                    {
                        //moveLeft = true;
                        jump = true;
                    }                    
                }
            }

            if (CollidesWithMap(GetBoundingBox().X, GetBoundingBox().Bottom + 1))
            {
                if (jump)
                {
                    Velocity.Y -= 20;
                }
            }
            else
            {
                Velocity.Y += 1;
            }

                        
            if (moveRight)
            {
                Direction = EntityDirection.Right;
                CurrentAnimation = "run";

                Velocity.X = 5;
            }

            if (moveLeft)
            {
                Direction = EntityDirection.Left;
                CurrentAnimation = "run";

                Velocity.X = -5;
            }

            if(!moveRight && !moveLeft)
            {
                Velocity.X = 0;
                CurrentAnimation = "idle";
            }

            oldState = newState;
            base.Update(gameTime);
        }


        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
