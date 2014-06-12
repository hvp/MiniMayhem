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

namespace Mini_Mayhem
{
    class Player
    {
        ContentManager content;
       public int score = 0;
        Texture2D playerWalkTex;
        Texture2D playerIdleTex;
        Vector2 pos;
      public  Body playerBody;

      public List<Fireball> Fireballs = new List<Fireball>(); 
        KeyboardState oldKeyState;
        GamePadState oldGamePadState;

        bool playerDirection = true;

        public bool isTouching = false;

        int currentFrame = 0;
        int elapsedTime = 0;

        public bool isDead = false;

        World world;

        public Player(Texture2D wTex, Texture2D iTex, Vector2 pos, World world, ContentManager content)
        {
            playerIdleTex = iTex;
            playerWalkTex = wTex;
            this.pos = pos;
            this.world = world;

            this.content = content;

            playerBody = BodyFactory.CreateRectangle(world, 100f / 64f, 100f / 64f, 1f, pos / 64);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Friction = 1f;

            playerBody.CollisionCategories = Category.Cat1; 
            
           // playerBody.FixedRotation = true;
            playerBody.Mass = 2f;
            playerBody.BodyId = 1;
            playerBody.OnCollision += new OnCollisionEventHandler(OnCollision);
            playerBody.OnSeparation += new OnSeparationEventHandler(OnSeparation);

        }
     
        public void Update(GameTime gameTime)
        {
            playerBody.Rotation = 0f;

            if (playerBody.Position.Y * 64 > 2000)
            {
                isDead = true;
            }
          
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > 100)
            {
                    if (currentFrame == 700)
                    {
                     currentFrame = 0;
                    }
                    else
                    {
    
                        currentFrame += 100;
                    } 
                elapsedTime = 0; 
            }

            foreach (Fireball fireball in Fireballs)
            {

                fireball.Update(gameTime);
                if (fireball.hasHit)
                {
                    world.RemoveBody(fireball.fireballBody);
                }
            }

            for (int i = 0; i < Fireballs.Count; i++)
            {
                if (Fireballs[i].hasHit)
                {
                    Fireballs.RemoveAt(i);

                }

            }
        }
        public void Draw(SpriteBatch batch)
        {
            foreach (Fireball fireball in Fireballs)
            {

                fireball.Draw(batch);
            }
            if (playerBody.LinearVelocity.X > 0)
            {
                playerDirection = true;
                batch.Draw(playerWalkTex, playerBody.Position * 64, new Rectangle(0, currentFrame, 100, 100), Color.White, playerBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.None, 0.9f);
            }
            else if (playerBody.LinearVelocity.X < 0)
            {
                playerDirection = false;
                batch.Draw(playerWalkTex, playerBody.Position * 64, new Rectangle(0, currentFrame, 100, 100), Color.White, playerBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.FlipHorizontally, 0.9f);

            }
            else
            {
                if (playerDirection)
                {

                    batch.Draw(playerIdleTex, playerBody.Position * 64, new Rectangle(0, currentFrame, 100, 100), Color.White, playerBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.None, 0.9f);
                }
                else
                {

                    batch.Draw(playerIdleTex, playerBody.Position * 64, new Rectangle(0, currentFrame, 100, 100), Color.White, playerBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.FlipHorizontally, 0.9f);
                
                }
            }
        }
        public void Input(InputState input, GameTime gameTime)
        {
            KeyboardState keyboardState = input.CurrentKeyboardStates[0];
            GamePadState gpState = input.CurrentGamePadStates[0];

            if (gpState.IsConnected)
            {
                float x = gpState.ThumbSticks.Left.X;
                float y = gpState.ThumbSticks.Left.Y;
                playerBody.ApplyLinearImpulse(new Vector2(x, -y));
                playerBody.LinearDamping = 1f;

                if (gpState.Triggers.Right > 0.5f && oldGamePadState.Triggers.Right == 0f)
                {
                  //  Fireballs.Add(new Fireball(content.Load<Texture2D>("Fireballs/fireball"), playerBody.Position * 64, world, playerDirection));
                    
                      Vector2 velo = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                      velo = new Vector2(velo.X, -velo.Y);
                     // float rot = VectorToAngle(gpState.ThumbSticks.Right);
                      float rot = VectorToAngle(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right) - 1.57f;
                      Console.WriteLine(rot);
                    Fireballs.Add(new Fireball(content.Load<Texture2D>("Fireballs/fireball"),velo,playerBody.Position * 64, rot,world));
                   
                }

                oldGamePadState = gpState;
            }
            else
            {

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (playerBody.LinearVelocity.X > -5)
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(-5, 0));
                    }

                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (playerBody.LinearVelocity.X < 5)
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(5, 0));
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Up) && isTouching == true) // is touching is a little buggy 
                {
                    if (playerBody.LinearVelocity.Y > -5)
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(0, -7));
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
                {

                    Fireballs.Add(new Fireball(content.Load<Texture2D>("Fireballs/fireball"), playerBody.Position * 64, world, playerDirection));
                }


                oldKeyState = keyboardState;
            }
        }

        public bool OnCollision(Fixture FixtureA, Fixture FixtureB, Contact contact)
        {
            Body fixA = FixtureA.Body;
            Body fixB = FixtureB.Body;


               isTouching = true;

               if (fixA.BodyId == 1 && fixB.BodyId == 4) // enemy collision
               {
                   isDead = true;

               }

            return true;
        }
        private void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            Body body1 = fixtureA.Body;
            Body body2 = fixtureB.Body;
             isTouching = false;
            



        }

       float VectorToAngle(Vector2 vector)
        {
         //   return (float)Math.Atan2(vector.X, -vector.Y); point up when shooting to the left 
         //   return (float)Math.Atan2(vector.X, vector.Y); point up when shooting to the left 
            return (float)Math.Atan2(vector.X, vector.Y);
        }
    }


   

}
