
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
    class Ground
    {
        Texture2D tex;
        Vector2 pos;
        Body groundBody;

        public Ground(Texture2D tex, Vector2 pos, World world)
        {
            this.tex = tex;
            this.pos = pos;

            groundBody = BodyFactory.CreateRectangle(world, 512 / 64, 1, 1f, pos / 64);
            groundBody.BodyType = BodyType.Static;
            groundBody.Friction = 1f;
            groundBody.CollisionCategories = Category.Cat3;

        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, pos, null, Color.White, 0f, new Vector2(256, 32), 1f, SpriteEffects.None, 1f);

        }

    }
}
