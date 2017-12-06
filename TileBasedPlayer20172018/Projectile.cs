using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using AnimatedSprite;
using Tiling;

namespace Tiler
{
    public class Projectile : RotatingSprite
    {
        public bool IsActive = false;

        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }

        public Vector2 Velocity = new Vector2(0, 0);

        private Vector2 MaxVelocity = new Vector2(20f, 20f);

        private Vector2 Acceleration = new Vector2(1f);

        public Vector2 Direction;

        public Projectile(Game game, Vector2 projectilePosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 40;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                //this.angleOfRotation = TurnToFace(this.CentrePos, , this.angleOfRotation, 1f);

                Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

                Velocity = Vector2.Clamp(Velocity, -MaxVelocity, MaxVelocity);
                Velocity += Acceleration;

                this.PixelPosition += (Direction * Velocity);

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsActive)
                base.Draw(gameTime);
        }
    }
}
