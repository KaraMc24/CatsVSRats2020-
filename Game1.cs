using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace CatsVsRats2020

{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D catTxr, ratTxr, backgroundTxr, particleTxr;
        Point screenSize = new Point(800, 450);
        float spawnCooldown = 2;
        float playTime = 0;

        Sprite backgroundSprite;
        CatSprite catSprite;
        List<RatSprite> missileList = new List<RatSprite>();
        List<ParticleSprite> particleList = new List<ParticleSprite>();
        
        SpriteFont UiFont;
        



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            catTxr = Content.Load<Texture2D>("CatSprite");
            ratTxr = Content.Load<Texture2D>("RatSprite");
            particleTxr = Content.Load<Texture2D>("Particlez");
            backgroundTxr = Content.Load<Texture2D>("GalaxyBackground");
            UiFont = Content.Load<SpriteFont>("SpaceFont");
            
            

            backgroundSprite = new Sprite(backgroundTxr, new Vector2(), new Vector2(1.25f, 1.25f));
            catSprite = new CatSprite(catTxr, new Vector2(screenSize.X / 1, screenSize.Y / 1));

        }

        protected override void Update(GameTime gameTime)
        {
            Random rng = new Random();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (spawnCooldown > 0)
            {
                spawnCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (catSprite.playerLives > 0 && missileList.Count < (Math.Min(playTime, 120f) / 120f) * 8f + 2f)
            {

                missileList.Add(new RatSprite(
                ratTxr,
                new Vector2(screenSize.X, rng.Next(0, screenSize.Y - ratTxr.Height)),
                (Math.Min(playTime, 120f) / 120f) * 20000f + 50f
                ));
                spawnCooldown = (float)(rng.NextDouble() + 0.5);
            }

            if (catSprite.playerLives > 0)
            {
                catSprite.Update(gameTime, screenSize);
                playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }



            foreach (RatSprite missile in missileList)
            {
                missile.Update(gameTime, screenSize);

                if (catSprite.playerLives > 0 && catSprite.IsColliding(missile))
                {
                    for (int i = 0; i < 16; i++)

                        particleList.Add(new ParticleSprite(particleTxr,
                            new Vector2(
                             missile.spritePos.X + (ratTxr.Width / 2) - (particleTxr.Width / 2),
                             missile.spritePos.Y + (ratTxr.Height / 2) - (particleTxr.Height / 2)
                             )
                             ));



                    missile.dead = true;
                    catSprite.playerLives--;
                   
                    if (catSprite.playerLives == 0)
                    {
                        for (int i = 0; i < 32; i++)

                            particleList.Add(new ParticleSprite(particleTxr,
                                new Vector2(
                                 catSprite.spritePos.X + (ratTxr.Width / 2) - (particleTxr.Width / 2),
                                 catSprite.spritePos.Y + (ratTxr.Height / 2) - (particleTxr.Height / 2)
                                 )
                                 ));
                      
                    }
                }
            }

            foreach (ParticleSprite particle in particleList) particle.Update(gameTime, screenSize);

            missileList.RemoveAll(missile => missile.dead);
            particleList.RemoveAll(particle => particle.currentLife <= 0);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            backgroundSprite.Draw(_spriteBatch);
            if (catSprite.playerLives > 0) catSprite.Draw(_spriteBatch);
            foreach (RatSprite missile in missileList) missile.Draw(_spriteBatch);
         

            _spriteBatch.DrawString(
               UiFont,
               "Lives: " + catSprite.playerLives,
               new Vector2(14, 14),
               Color.Black
               );

            _spriteBatch.DrawString(
                UiFont,
                "Lives: " + catSprite.playerLives,
                new Vector2(10, 10),
                Color.White
                );

            _spriteBatch.DrawString(
              UiFont,
              "Time: " + Math.Round(playTime),
              new Vector2(14, 44),
              Color.Black
              );

            _spriteBatch.DrawString(
              UiFont,
              "Time: " + Math.Round(playTime),
              new Vector2(10, 40),
              Color.White
              );


            if (catSprite.playerLives <= 0)
            {
                Vector2 textSize = UiFont.MeasureString("GAME OVER");
                _spriteBatch.DrawString(
                    UiFont,
                    "GAME OVER",
                    new Vector2((screenSize.X / 2) - (textSize.X / 2) + 8, (screenSize.Y / 2) - (textSize.Y / 2) + 8),
                    Color.Black
                     );


                _spriteBatch.DrawString(
                    UiFont,
                    "GAME OVER",
                    new Vector2((screenSize.X / 2) - (textSize.X) / 2, (screenSize.Y / 2) - (textSize.Y / 2)),
                    Color.White
                    );


            }


            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
