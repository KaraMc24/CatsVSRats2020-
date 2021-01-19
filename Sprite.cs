using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CatsVsRats2020
{
    public class Sprite
    {
        public Texture2D spriteTexture;
        public Vector2 spritePos;
        public Vector2 spriteScale;

        public Sprite(Texture2D newTxr, Vector2 newPos, Vector2 newScale)
        {
            spriteTexture = newTxr;
            spritePos = newPos;
            spriteScale = newScale;

            

        }

        public Sprite(Texture2D newTxr, Vector2 newPos)
        {
        }

        public virtual void Update(GameTime gameTime, Point screenSize) { }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                spriteTexture,
                new Rectangle((int)spritePos.X,
                (int)spritePos.Y,
                (int)((float)spriteTexture.Width * spriteScale.X),
                (int)((float)spriteTexture.Height * spriteScale.Y)
                ),
                Color.White
                );

        }

        public bool IsColliding(Sprite otherSprite)
        {
            Rectangle thisRect = new Rectangle(
                (int)spritePos.X,
                (int)spritePos.Y,
                (int)((float)spriteTexture.Width * spriteScale.X),
                (int)((float)spriteTexture.Height * spriteScale.Y)
                );
            Rectangle otherRect = new Rectangle(
                (int)otherSprite.spritePos.X,
                (int)otherSprite.spritePos.Y,
                (int)((float)otherSprite.spriteTexture.Width * otherSprite.spriteScale.X),
                (int)((float)otherSprite.spriteTexture.Height * otherSprite.spriteScale.Y)
                );

            return thisRect.Intersects(otherRect);



        }

    }
}
