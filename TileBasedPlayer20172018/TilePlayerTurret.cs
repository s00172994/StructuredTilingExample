﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.Engines;
using AnimatedSprite;
using Tiling;
using CameraNS;

namespace Tiler
{
    class TilePlayerTurret : RotatingSprite
    {
        private float turnSpeed = 0.04f;
        private const float WIDTH_IN = 11f; // Width in from the left for the sprites origin
        private float angleOfRotationPrev;

        public Projectile Bullet;
        public Vector2 CrosshairPosition;
        public Vector2 Direction;

        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
        }

        private Vector2 originToRotate
        {
            get
            {
                return new Vector2(((FrameWidth / 2) - WIDTH_IN), (FrameHeight / 2));
            }
        }

        public TilePlayerTurret(Game game, Vector2 playerPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, playerPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 70;
            origin = originToRotate;
        }

        public override void Update(GameTime gameTime)
        {
            TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));

            // Props this turret onto the underside tank body if it exists
            if (player != null)
            {
                Track(player.PixelPosition + new Vector2(WIDTH_IN, 0f));
            }

            CrosshairPosition = ((InputEngine.MousePosition) + Camera.CamPos);

            angleOfRotationPrev = this.angleOfRotation;

            this.angleOfRotation = TurnToFace(this.CentrePos - new Vector2(WIDTH_IN, 0f), CrosshairPosition, this.angleOfRotation, turnSpeed);

            Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

            Fire();

            base.Update(gameTime);
        }

        public void Track(Vector2 followPos)
        {
            this.PixelPosition = followPos;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void AddProjectile(Projectile loadedBullet)
        {
            Bullet = loadedBullet;
        }

        public void Fire()
        {
            if (Bullet != null && Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle)
            {
                Bullet.PixelPosition = (this.PixelPosition - new Vector2(WIDTH_IN, 0));
            }

            if (Bullet != null)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed
                    && Bullet.ProjectileState == Projectile.PROJECTILE_STATUS.Idle
                    && this.angleOfRotation != 0 && Math.Round(this.angleOfRotationPrev,2) == Math.Round(this.angleOfRotation,2))
                {
                    // Send this direction to the projectile
                    Bullet.GetDirection(Direction);
                    // Shoot at the specified position
                    Bullet.Shoot(CrosshairPosition - new Vector2(FrameWidth / 2, FrameHeight / 2));
                }
            }
        }
    }
}
