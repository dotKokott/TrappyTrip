using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrappyTrip
{
    public class Animation
    {
        private Texture2D spriteSheet;

        private int frameWidth;
        private int frameHeight;
        
        private float frameTime;
        private float frameTimeElapsed;

        private string name;

        private bool loopAnimation;
        private bool finishedPlaying;

        private int currentFrame;

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public int FrameCount
        {
            get { return (int)spriteSheet.Width / frameWidth; }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                if (value <= FrameCount - 1) currentFrame = value;
            }
        }

        public Rectangle FrameRectangle
        {
            get { return new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool LoopAnimation
        {
            get { return loopAnimation; }
            set { loopAnimation = value; }
        }

        public Animation(string animationName, Texture2D spriteSheet, int frameWidth)
        {
            this.name = animationName;
            this.spriteSheet = spriteSheet;
            this.frameWidth = frameWidth;
            this.frameHeight = spriteSheet.Height;

            this.frameTime = 0.05f;
            this.frameTimeElapsed = 0;

            this.loopAnimation = true;

            this.currentFrame = 0;
        }

        public void Play(bool loop)
        {
            loopAnimation = loop;
            currentFrame = 0;
            finishedPlaying = false;
        }

        public void update(GameTime gameTime)
        {
            frameTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeElapsed >= frameTime && !finishedPlaying)
            {
                if (currentFrame == FrameCount - 1)
                {
                    if (!loopAnimation)
                    {
                        finishedPlaying = true;
                        return;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
                else
                {
                    currentFrame++;
                }
                                               
                frameTimeElapsed = 0;
            }
        }
    }
}
