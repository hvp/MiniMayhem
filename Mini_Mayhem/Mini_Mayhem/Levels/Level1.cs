#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameStateManagement;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;
#endregion

namespace Mini_Mayhem
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class Level1 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;


        Texture2D mountainsArt;
        Texture2D treesArt; 
        

        InputAction pauseAction;

        bool levelEdit = false;

    

        List<Ground> Grounds = new List<Ground>();
        List<Crate> Crates = new List<Crate>();
        List<Star> Stars = new List<Star>();
        List<Robot> Enemies = new List<Robot>();


        
        String scoreText = "Score: ";
        float pauseAlpha;


        Cam2d cam2D;
        World world;

        Player MC;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Level1()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            world = new World(new Vector2(0, 10));
         
            // 

            // define and action 
            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>

        public override void Activate(bool instancePreserved)
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            cam2D = new Cam2d(ScreenManager.GraphicsDevice);
            Vector2 Center = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2);
            
            gameFont = content.Load<SpriteFont>("gamefont");

            MC = new Player(content.Load<Texture2D>("PlayerAnimation/CycleStrip"), content.Load<Texture2D>("PlayerAnimation/_mcIdleV2"), new Vector2(0, 0), world, content);
            
            //level one 
            {
                mountainsArt = content.Load<Texture2D>("BGArt/_Mountains");
                treesArt = content.Load<Texture2D>("BGArt/_trees");
                Grounds.Add(new Ground(content.Load<Texture2D>("Art/_Ground"), Center + new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 32), world));
                Grounds.Add(new Ground(content.Load<Texture2D>("Art/_Ground"), Center + new Vector2(512, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 32), world));
                Grounds.Add(new Ground(content.Load<Texture2D>("Art/_Ground"), Center + new Vector2(-512, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 32), world));
                Grounds.Add(new Ground(content.Load<Texture2D>("Art/_Ground"), Center + new Vector2(1024, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 32), world));
                Crates.Add(new Crate(content.Load<Texture2D>("Art/_box"), Center + new Vector2(-500, 200), world));

                Grounds.Add(new Ground(content.Load<Texture2D>("Art/_Ground"), Center, world));

                Crates.Add(new Crate(content.Load<Texture2D>("Art/_box"), Center + new Vector2(0, 200), world));


                Crates.Add(new Crate(content.Load<Texture2D>("Art/_box"), Center + new Vector2(500, 200), world));


                Stars.Add(new Star(content.Load<Texture2D>("Art/_star"), Center + new Vector2(0, 130), world));
                Stars.Add(new Star(content.Load<Texture2D>("Art/_star"), Center + new Vector2(500, 130), world));
                Stars.Add(new Star(content.Load<Texture2D>("Art/_star"), Center + new Vector2(-500, 130), world));
                Stars.Add(new Star(content.Load<Texture2D>("Art/_star"), Center + new Vector2(0, -60), world));

                Enemies.Add(new Robot(content.Load<Texture2D>("Enemies/robot1"),Center + new Vector2(-210,-60),world)); 
               
            }
            
            cam2D.TrackingBody = MC.playerBody;
            cam2D.EnablePositionTracking = true;
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {


                MC.Update(gameTime);

                if (MC.isDead)
                {
                    LoadingScreen.Load(ScreenManager, true, 0,
                                new Level1());

                }



                cam2D.Update(gameTime);

                //Robots
                {
                    foreach (Robot robot in Enemies)
                    {
                        robot.Update(gameTime);
                        if (robot.isDead)
                        {
                            world.RemoveBody(robot.robotBody);

                        }

                    }

                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        if (Enemies[i].isDead)
                        {
                            MC.score += 2;
                            Enemies.RemoveAt(i);
                        }
                    }


                }
                //Stars
                {

                    foreach (Star star in Stars)
                    {
                        if (star.wasCollected)
                        {
                            world.RemoveBody(star.starBody);
                        }

                    }

                    for (int i = 0; i < Stars.Count; i++)
                    {
                        if (Stars[i].wasCollected)
                        {
                            Stars.RemoveAt(i);
                            MC.score++;
                        }
                    }

                }

                //Level 1 Script
                {
                    if(MC.score >= 5)
                    {
                        LoadingScreen.Load(ScreenManager,true,0,new Level2());

                    }
                } 
                
                cam2D.MaxRotation = 0.001f;
                cam2D.MinRotation = -0.001f;

                cam2D.MaxPosition = new Vector2(((MC.playerBody.Position.X) * 64 + 1), ((MC.playerBody.Position.Y) * 64) + 1);
                cam2D.MinPosition = new Vector2(((MC.playerBody.Position.X) * 64) + 2, ((MC.playerBody.Position.Y) * 64) + 1);
                cam2D.Update(gameTime);

                world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                }
               

                
            }
        


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        
        public override void HandleInput(GameTime gameTime,InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            MouseState mouseState = Mouse.GetState();

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];
            MC.Input(input, gameTime);
            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 boxPos = (cam2D.Position / 64) + new Vector2(mouseState.X,mouseState.Y);
                    Crates.Add(new Crate(content.Load<Texture2D>("Art/_box"),boxPos,world));

                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    
                    cam2D.MoveCamera(new Vector2(-0.5f, 0));
                }
               
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    
                    cam2D.MoveCamera(new Vector2(0.5f, 0));
                }
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    cam2D.MoveCamera(new Vector2(0, -0.5f));
                    
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    cam2D.MoveCamera(new Vector2(0, 0.5f));
                    
                }
              

             
                
              
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(mountainsArt, new Vector2(-130, -40), Color.White);
            spriteBatch.Draw(treesArt,new Vector2(0,400),Color.White);
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            cam2D.View);
            
            MC.Draw(spriteBatch);

            
            foreach (Ground ground in Grounds)
            {
                ground.Draw(spriteBatch);

            }
            foreach (Crate crate in Crates)
            {
                crate.Draw(spriteBatch);
            }
            foreach (Star star in Stars)
            {
                star.Draw(spriteBatch);
            }
            foreach (Robot robot in Enemies)
            {

                robot.Draw(spriteBatch); 
            }

            
            spriteBatch.End();
            spriteBatch.Begin();
            
            spriteBatch.DrawString(gameFont, scoreText, new Vector2(10, 10), Color.Black);

            Vector2 scorePos = gameFont.MeasureString(scoreText);
            spriteBatch.DrawString(gameFont, MC.score.ToString(), new Vector2(10 + scorePos.X, 10), Color.Black);
            spriteBatch.End();
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
