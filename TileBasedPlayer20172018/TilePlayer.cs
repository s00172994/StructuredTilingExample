using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiler
{

    public class TilePlayer
    {
        Texture2D texture;
        Vector2 position;
        int speed;
        Vector2 previousPosition;

        public Rectangle CollisionField
        {
            get
            {
                return new Rectangle(Position.ToPoint(), 
                    new Point(texture.Width, texture.Height));
            }

        }

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public TilePlayer(Texture2D tx,
            Vector2 startPos)
        {
            texture = tx;
            Position = previousPosition = startPos;
            speed = 5;

        }

        public void Collision(Collider c)
        {
            if (CollisionField.Intersects(c.CollisionField))
                Position = previousPosition;
        }

        public void update(GameTime gameTime)
        {
            previousPosition = Position;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.Position += new Vector2(1, 0) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.Position += new Vector2(-1, 0) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.Position += new Vector2(0, -1) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.Position += new Vector2(0, 1) * speed;
            }

        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, CollisionField, Color.White);
        }
    }
}
