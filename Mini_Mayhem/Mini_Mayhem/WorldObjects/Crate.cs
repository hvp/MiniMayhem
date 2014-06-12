using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;

namespace Mini_Mayhem
{
    class Crate
    {
         Texture2D tex;
        Vector2 pos;
        Body crateBody;

        public Crate(Texture2D tex, Vector2 pos, World world)
        {
            this.tex = tex;
            this.pos = pos;

            crateBody = BodyFactory.CreateRectangle(world, tex.Width / 64, tex.Height/ 64, 1f, pos / 64);

            crateBody.BodyType = BodyType.Static;
            crateBody.Mass = 5f;
            crateBody.Friction = 0.8f;
            crateBody.CollisionCategories = Category.Cat3;
            crateBody.BodyId = 5;

        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, crateBody.Position * 64, null, Color.White, crateBody.Rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 1f);

        }
    }
}
