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
    class Robot
    {
        Texture2D tex;
        Vector2 pos;


       public Body robotBody;
       Path path;
        int health = 100;
        float time = 0;

        bool direction = true; // true for right 
       public bool isDead = false;
        public Robot(Texture2D tex, Vector2 pos, World world)
       {
           this.tex = tex;
           this.pos = pos;

           robotBody = BodyFactory.CreateRectangle(world, 100f / 64f, 100f / 64f, 1f, pos / 64);
           robotBody.BodyType = BodyType.Dynamic;
           robotBody.FixedRotation = true;
           robotBody.CollisionCategories = Category.Cat3;
           robotBody.CollidesWith = Category.All ^ Category.Cat4;
           robotBody.BodyId = 4;
           robotBody.OnCollision += new OnCollisionEventHandler(OnCollision);
          
           // path
           {
               pos = pos / 64;
               path = new Path();
               path.Add(pos);
               path.Add(new Vector2(pos.X + 6.5f, pos.Y));
               path.Add(new Vector2(pos.X + 6.5f, pos.Y + 0.1f));
               path.Add(new Vector2(pos.X, pos.Y + 0.1f));
               path.Closed = true;

           }
       }
        public void Draw(SpriteBatch batch)
        {
            if (direction)
            {
                batch.Draw(tex, robotBody.Position * 64, null, Color.White, robotBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.None, 0.9f);
            }
            else
            {
                batch.Draw(tex, robotBody.Position * 64, null, Color.White, robotBody.Rotation, new Vector2(50, 50), 1f, SpriteEffects.FlipHorizontally, 0.9f);
     
            }

        }
        public void Update(GameTime gameTime)
        {
            time += 0.005f;

            if (time > 1)
            {
                time = 0;
            }

            PathManager.MoveBodyOnPath(path, robotBody, time, 1f, 1f / 60f);


            if (robotBody.LinearVelocity.X > 0)
            {
                direction = true;
            }
            if (robotBody.LinearVelocity.X < 0)
            {
                direction = false;
            }
            if (health <= 0)
            {
                isDead = true;
            }

        }
        public bool OnCollision(Fixture FixtureA, Fixture FixtureB, Contact contact)
        {
            Body fixA = FixtureA.Body;
            Body fixB = FixtureB.Body;

            if (fixA.BodyId == 4 && fixB.BodyId == 3) // fireball collision
            {
                health -= 50;
            }

            return true;
        }
        
    }
}
