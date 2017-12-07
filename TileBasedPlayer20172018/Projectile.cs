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

        public int Velocity = 50;
        public Vector2 Direction;
        private const float LIFE_SPAN = 2f; // Explosion life in seconds
        private float timer = 0f;

        public Projectile(Game game, Vector2 projectilePosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, Vector2 direction)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Target = Vector2.Zero;
            Direction = direction;
            DrawOrder = 50;
            StartPosition = projectilePosition;
        }

        public override void Update(GameTime gameTime)
        {
            switch (projectileState)
            {
                case PROJECTILE_STATUS.Idle:
                    this.Visible = true;
                    break;

                case PROJECTILE_STATUS.Firing:
                    this.Visible = true;
                    this.angleOfRotation = TurnToFace(PixelPosition, Target, angleOfRotation, 10f);

                    this.PixelPosition += (Direction * Velocity);

                    #region Collision Checking
                    //if (this.PixelPosition == )
                    // Check to see if it's in the wrong place or (SIMPLY) out of map bounds
                    // THEN explode
                    // So that our bullet returns! =)

                    // Reference Sentry
                    SentryTurret otherSentry = (SentryTurret)Game.Services.GetService(typeof(SentryTurret));
                    // Check collision with Sentry
                    if (collisionDetect(otherSentry))
                    {
                        projectileState = PROJECTILE_STATUS.Exploding;
                    }
                    #endregion

                    //// Old smooth point to point projectile movement, doesn't move beyond target
                    //PixelPosition = Vector2.Lerp(PixelPosition, Target, Velocity);

                    //if (Vector2.Distance(PixelPosition, Target) < Velocity * 2)
                    //{
                    //    projectileState = PROJECTILE_STATUS.Exploding;
                    //}
                    break;

                case PROJECTILE_STATUS.Exploding:
                    this.Visible = false;
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timer > LIFE_SPAN)
                    {
                        // Reload Projectile
                        projectileState = PROJECTILE_STATUS.Idle;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        public void GetDirection(Vector2 TilePlayerTurretDirection)
        {
            Direction = TilePlayerTurretDirection;
        }

        public void Shoot(Vector2 TargetDirection)
        {
            projectileState = PROJECTILE_STATUS.Firing;
            Target = TargetDirection;
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
