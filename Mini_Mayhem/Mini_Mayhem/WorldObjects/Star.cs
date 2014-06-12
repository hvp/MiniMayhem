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
    class Star
    {
        Texture2D tex;
        Vector2 pos;

      public  Body starBody;
       public bool wasCollected = false;

        public Star(Texture2D tex, Vector2 pos, World world)
        {
            this.tex = tex;
            this.pos = pos;

            starBody = BodyFactory.CreateRectangle(world, 30f / 64f, 30f / 64f, 1f, pos / 64);
            starBody.BodyType = BodyType.Dynamic;
            starBody.BodyId = 2;
            starBody.OnCollision += new OnCollisionEventHandler(OnCollision);
            starBody.CollisionCategories = Category.Cat4;
            starBody.CollidesWith = Category.All ^ Category.Cat2;
           // starBody.OnSeparation += new OnSeparationEventHandler(OnSeparation);



        }



        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, starBody.Position * 64, null, Color.White, starBody.Rotation, new Vector2(15, 15), 1f, SpriteEffects.None, 1f);


        }

        public bool OnCollision(Fixture FixtureA, Fixture FixtureB, Contact contact)
        {
            Body fixA = FixtureA.Body;
            Body fixB = FixtureB.Body;

            if (fixA.BodyId == 2 && fixB.BodyId == 1)
            {
               wasCollected  = true;
            }

            return true;
        }
        
    }
}
