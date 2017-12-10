using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.Engines;
using AnimatedSprite;
using Tiling;
using Helpers;

namespace Tiler
{
    class SentryTurret : RotatingSprite
    {
        float collisionRadius = 300;
        float turnSpeed = 0.015f;
        const float WIDTH_IN = 11f; // Width in from the left for the sprites origin
        float angleOfRotationPrev;

        public string Name;

        public Projectile Bullet;
        public Vector2 Direction;

        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }
        private Vector2 trueOrigin
        {
            get
            {
                return new Vector2(((FrameWidth / 2) - WIDTH_IN), (FrameHeight / 2));
            }
        }

        public SentryTurret(Game game, Vector2 sentryPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, string nameIn)
                : base(game, sentryPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Name = nameIn;
            DrawOrder = 60;
            origin = trueOrigin;
        }

        public override void Update(GameTime gameTime)
        {
            if (Helper.CurrentGameStatus == GameStatus.PLAYING)
            {
                TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));
                Sentry sentry = (Sentry)Game.Services.GetService(typeof(Sentry));

                //Props this turret onto the appropiate tank body
                if (this.Name == sentry.Name && sentry != null)
                {
                    AddSelfToBody(sentry.PixelPosition + new Vector2(WIDTH_IN, 0f));
                }

                angleOfRotationPrev = this.angleOfRotation;

                this.Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

                Bullet.GetDirection(this.Direction);

                //Face and shoot at the player when player is within radius
                Detect(player);

                base.Update(gameTime);
            }
        }

        public void AddProjectile(Projectile projectileIn)
        {
            Bullet = projectileIn;
        }

        public bool IsInRadius(TilePlayer player)
        {
            float distance = Math.Abs(Vector2.Distance(this.CentrePos, player.CentrePos));

            if (distance <= collisionRadius)
                return true;
            else
                return false;
        }

        private void Detect(TilePlayer player)
        {
            if (IsInRadius(player))
            {
                this.angleOfRotation = TurnToFace(this.CentrePos, player.CentrePos, this.angleOfRotation, turnSpeed);

                // Shoot at the player
                FireAt(player);
            }
        }

        public void FireAt(TilePlayer player)
        {
            if (Bullet != null && Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle)
            {
                Bullet.PixelPosition = (this.PixelPosition - new Vector2(WIDTH_IN, 0));
            }

            if (Bullet != null)
            {
                if (Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle
                    && this.angleOfRotation != 0 && Math.Round(this.angleOfRotationPrev, 2) == Math.Round(this.angleOfRotation, 2))
                {
                    // Send this direction to the projectile
                    Bullet.GetDirection(Direction);
                    // Shoot at the specified position
                    Bullet.Shoot(player.CentrePos);
                }
            }
        }

        public void AddSelfToBody(Vector2 followPos)
        {
            this.PixelPosition = followPos;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
