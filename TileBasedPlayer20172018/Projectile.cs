using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.Engines;
using AnimatedSprite;
using Tiling;

namespace Tiler
{
    public class Projectile : RotatingSprite
    {
        public enum PROJECTILE_STATUS
        {
            Idle,
            Firing,
            Exploding
        }
        private PROJECTILE_STATUS projectileState = PROJECTILE_STATUS.Idle;
        public PROJECTILE_STATUS ProjectileState
        {
            get { return projectileState; }
            set { projectileState = value; }
        }

        private Vector2 Target;
        private Vector2 StartPosition;

        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }

        public float Velocity = 0.25f;
        public Vector2 Direction;

        public Projectile(Game game, Vector2 projectilePosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, Vector2 direction)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Target = Vector2.Zero;
            Direction = direction;
            DrawOrder = 40;
            StartPosition = projectilePosition;
        }

        public override void Update(GameTime gameTime)
        {
            switch (projectileState)
            {
                case PROJECTILE_STATUS.Idle:
                    this.Visible = false;
                    break;
                case PROJECTILE_STATUS.Firing:
                    this.Visible = true;
                    PixelPosition = Vector2.Lerp(PixelPosition, Target, Velocity);
                    this.angleOfRotation = TurnToFace(PixelPosition, Target, angleOfRotation, 1f);
                    if (Vector2.Distance(PixelPosition, Target) < 2)
                    {
                        projectileState = PROJECTILE_STATUS.Exploding;
                    }
                    break;
                case PROJECTILE_STATUS.Exploding:
                    break;
            }
            base.Update(gameTime);
        }

        public void Shoot(Vector2 CrosshairTarget)
        {
            projectileState = PROJECTILE_STATUS.Firing;
            Target = CrosshairTarget;
        }

        public override void Draw(GameTime gameTime)
        {
            if (ProjectileState != Projectile.PROJECTILE_STATUS.Idle)
            {
                base.Draw(gameTime);
            }
        }
    }
}
