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

namespace Tiler
{
    class SentryTurret : RotatingSprite
    {
        float collisionRadius = 300;
        float turnSpeed = 0.02f;
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
            DrawOrder = 20;
            origin = trueOrigin;
        }

        public override void Update(GameTime gameTime)
        {
            TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));
            Sentry sentry = (Sentry)Game.Services.GetService(typeof(Sentry));

            // Props this turret onto the appropiate tank body
            if (this.Name == sentry.Name && sentry != null)
            {
                AddSelfToBody(sentry.PixelPosition + new Vector2(WIDTH_IN, 0f));
            }

            angleOfRotationPrev = this.angleOfRotation;

            // Face the player when player is within radius
            Face(player);
            // Shoot at the player
            Detect(player, gameTime);

            Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

            base.Update(gameTime);
        }

        public bool IsInRadius(TilePlayer player)
        {
            float distance = Math.Abs(Vector2.Distance(this.CentrePos, player.CentrePos));

            if (distance <= collisionRadius)
                return true;
            else
                return false;
        }

        private void Face(TilePlayer player)
        {
            if (IsInRadius(player))
            {
                this.angleOfRotation = TurnToFace(this.CentrePos, player.CentrePos, this.angleOfRotation, turnSpeed);
            }
        }

        public void Detect(TilePlayer player, GameTime gameTime)
        {
           
        }

        //public void Reload()
        //{
        //    if (Bullet != null && Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle)
        //    {
        //        Bullet.PixelPosition = (this.PixelPosition - new Vector2(WIDTH_IN, 0));
        //    }
        //    if (Bullet != null)
        //    {
        //        if (Mouse.GetState().LeftButton == ButtonState.Pressed
        //            && Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle
        //            && this.angleOfRotation != 0 && Math.Round(this.angleOfRotationPrev, 2) == Math.Round(this.angleOfRotation, 2))
        //        {
        //            Bullet.Shoot(CrosshairPosition - new Vector2(FrameWidth / 2, FrameHeight / 2));
        //        }
        //    }
        //}

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
