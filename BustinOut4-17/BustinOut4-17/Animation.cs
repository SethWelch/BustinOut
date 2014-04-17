using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// More generalized animating class that takes in a spritesheet
// from the player or enemy object and does the animating



namespace BustinOutMegaMan
{
    public class Animation
    {
        public int spriteFrameCounter;

        int numOfFrames;
        int currentFrame;

        Vector2 position;
        Vector2 origin;
        Texture2D Image;
        Rectangle sourceRect;
        SpriteEffects flipped;
        int direction;

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D animationImage
        {
            set { Image = value; }
        }

        public int spriteWidth
        {
            get { return Image.Height; }
            // should be Image.Width / numOfFrames; } for generalization
            //but currently megaman sprite sheet is not even.
        }

        public int spriteHeight
        {
            get { return Image.Height; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Rectangle bounds
        {
            get { return sourceRect; }
        }

        public void Initialize(Vector2 position, int frames)
        {
            this.position = position;
            this.numOfFrames = frames;

        }

        public void Update(GameTime gameTime)
        {
            if (direction == 1)
                flipped = SpriteEffects.FlipHorizontally;
            else
                flipped = SpriteEffects.None;

            origin = new Vector2(spriteWidth, spriteHeight);
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, position, sourceRect, Color.White, 0f, origin, 1.0f, flipped, 0);
        }
    }
}
