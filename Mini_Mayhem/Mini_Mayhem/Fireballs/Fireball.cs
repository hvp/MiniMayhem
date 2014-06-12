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
    
    class Fireball
    {
        Texture2D tex; // 384 X 64
        Vector2 pos;
        float rot = 0f;
        Vector2 velo;
        
        int currentFrame = 0;
        float elapsedTime = 0;
        
       public bool hasFried = false;
       public bool hasHit = false;

 
       public Body fireballBody;

        bool direction; // true for right
        public Fireball(Texture2D tex, Vector2 pos, World world, bool direction)
        {
            this.tex = tex;
            this.pos = pos;
            fireballBody = BodyFactory.CreateRectangle(world, 0.5f, 0.5f, 1f,pos / 64);
            fireballBody.BodyType = BodyType.Dynamic;
            fireballBody.IsBullet = true;
            fireballBody.FixedRotation = true;
            this.direction = direction;
            fireballBody.CollisionCategories = Category.Cat2;
            fireballBody.CollidesWith = Category.All ^ Category.Cat1;
            fireballBody.BodyId = 3;
            fireballBody.IgnoreGravity = true;
            fireballBody.OnCollision += new OnCollisionEventHandler(OnCollision);
            if (!direction)
            {
                fireballBody.Rotation = -3.14f;
            }


        }

        public Fireball(Texture2D tex, Vector2 velocity, Vector2 pos, float rotation, World world)
        {
            this.tex = tex;
            this.pos = pos;
            this.rot = rotation;
            this.velo = velocity;
         
            fireballBody = BodyFactory.CreateRectangle(world, 0.5f, 0.5f, 1f, pos / 64);
            fireballBody.IsBullet = true;
            fireballBody.Rotation = rot;
            fireballBody.FixedRotation = true;
            fireballBody.BodyType = BodyType.Dynamic;
            fireballBody.BodyId = 3;
            fireballBody.CollisionCategories = Category.Cat2;
            fireballBody.CollidesWith = Category.All ^ Category.Cat1;
            fireballBody.IgnoreGravity = true;
            fireballBody.OnCollision += new OnCollisionEventHandler(OnCollision);

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, fireballBody.Position * 64, new Rectangle(currentFrame, 0, 64, 64), Color.White, fireballBody.Rotation, new Vector2(32, 32), 1f, SpriteEffects.None, 0.9f);
           // batch.Draw(tex, new Vector2(100,100), new Rectangle(currentFrame, 0, 64, 64), Color.White, fireballBody.Rotation, new Vector2(32, 32), 1f, SpriteEffects.None, 0.9f);
     

        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > 100)
            {
                if (currentFrame > 320 - 64)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame += 64;
                }
                elapsedTime = 0;
            }

            velo.Normalize();
            fireballBody.LinearVelocity = velo * 10;
        }
        public bool OnCollision(Fixture FixtureA, Fixture FixtureB, Contact contact)
        {
            Body fixA = FixtureA.Body;
            Body fixB = FixtureB.Body;

            hasHit = true;


            return true;
        }


    }
}
