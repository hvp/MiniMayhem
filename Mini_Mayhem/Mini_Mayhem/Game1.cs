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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;
        ScreenFactory screenFactory;

        


      
      

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
           
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
          

            // Create the screen factory and add it to the Services
            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);


            // On Windows and Xbox we just add the initial screens
            AddInitialScreens();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);



            screenManager.AddScreen(new MainMenuScreen(), null);

        }

       
      
     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);
        }
    }
}
