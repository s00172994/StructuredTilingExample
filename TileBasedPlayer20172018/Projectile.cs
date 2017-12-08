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
        private Vector2 PreviousPosition;

        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }

        public int Velocity = 10;
        public Vector2 Direction;
        private float lifeSpan = 2f; // Default explosion life in seconds
        private float timer = 0;

        private string Parent;

        private bool isPastTarget = false;
        private bool gotDirection = false;

        public Projectile(Game game, string ParentName, Vector2 projectilePosition, List<TileRef> sheetRefs, 
            int frameWidth, int frameHeight, float layerDepth, Vector2 direction)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Parent = ParentName;
            Target = Vector2.Zero;
            Direction = direction;
            DrawOrder = 50;
            StartPosition = projectilePosition;
        }

        #region LifeSpan Overload Method
        public Projectile(Game game, string ParentName, Vector2 projectilePosition, List<TileRef> sheetRefs,
            int frameWidth, int frameHeight, float layerDepth, Vector2 direction, float lifeSpanIn)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Parent = ParentName;
            Target = Vector2.Zero;
            Direction = direction;
            DrawOrder = 50;
            lifeSpan = lifeSpanIn;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = PixelPosition;

            switch (projectileState)
            {
                case PROJECTILE_STATUS.Idle:
                    this.Visible = true;
                    break;

                case PROJECTILE_STATUS.Firing:
                    this.Visible = true;
                    this.gotDirection = false;
                    this.PixelPosition += (Direction * Velocity);

                    FaceThis(gameTime);

                    #region Collision Checking

                    // Projectile is out of tile map bounds
                    if (this.PixelPosition.X < 0 || this.PixelPosition.Y < 0
                        || this.PixelPosition.X > CameraNS.Camera._worldBound.X
                        || this.PixelPosition.Y > CameraNS.Camera._worldBound.Y)
                    {
                        projectileState = PROJECTILE_STATUS.Exploding;
                    }

                    // Ensure the sentry doesn't shoot itself !
                    if (Parent != "PLAYER")
                    {

                    }
                    else
                    {
                        // Reference Collision Objects
                        SentryTurret otherSentry = (SentryTurret)Game.Services.GetService(typeof(SentryTurret));

                        // Check collision with Sentry
                        if (collisionDetect(otherSentry))
                        {
                            projectileState = PROJECTILE_STATUS.Exploding;
                        }
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

                    if (timer > lifeSpan)
                    {
                        timer = 0f;
                        // Reload Projectile
                        projectileState = PROJECTILE_STATUS.Idle;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        public void GetDirection(Vector2 TurretDirection)
        {
            if (!gotDirection)
            {
                Direction = TurretDirection;
                gotDirection = true;
            }
        }

        public void Shoot(Vector2 TargetDirection)
        {
            projectileState = PROJECTILE_STATUS.Firing;
            Target = TargetDirection;
            isPastTarget = false;
        }

        public void FaceThis(GameTime gameTime)
        {

            if (Parent.ToUpper() == "PLAYER" && Vector2.Distance(this.PixelPosition, Target) > 2
                && !isPastTarget)
            {
                this.angleOfRotation = TurnToFace(PixelPosition, Target, angleOfRotation, 10f);
                isPastTarget = true;
            }
            else if (!isPastTarget)
            {
                this.angleOfRotation = TurnToFace(CentrePos, Target, angleOfRotation, 10f);
                isPastTarget = true;
            }
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
