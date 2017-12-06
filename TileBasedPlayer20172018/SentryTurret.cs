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

        public SentryTurret(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, string nameIn)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Name = nameIn;
            DrawOrder = 4;
            origin = trueOrigin;
        }

        public override void Update(GameTime gameTime)
        {
            angleOfRotationPrev = angleOfRotation;

            TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));
            Sentry sentry = (Sentry)Game.Services.GetService(typeof(Sentry));

            // Props this turret onto the appropiate tank body
            if (this.Name == sentry.Name)
            {
                AddSelfToBody(sentry.PixelPosition + new Vector2(WIDTH_IN, 0f));
            }

            // Face the player when player is within radius
            Face(player);
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
            //if (MyProjectile != null && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
            //    MyProjectile.position = this.CentrePos;

            //if (MyProjectile != null)
            //{
            //    if (IsInRadius(player) && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL
            //        && angleOfRotation != 0 && angleOfRotationPrev == angleOfRotation
            //        && player.PlayerState != TilePlayer.PLAYERSTATUS.DEAD)
            //    {
            //        MyProjectile.fire(player.position);
            //    }
            //}

            //if (MyProjectile != null)
            //    MyProjectile.Update(gameTime);
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
