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
    class Projectile : RotatingSprite
    {
        public enum PROJECTILE_STATE { STILL, FIRING, EXPLODING };
        PROJECTILE_STATE projectileState = PROJECTILE_STATE.STILL;

        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0) };
        //TileRef currentFrame;

        protected float RocketVelocity = 4.0f;
        Vector2 TexCenter;
        Vector2 Target;

        float ExplosionTimer = 0;
        float ExplosionVisibleLimit = 1000;

        Vector2 StartPosition;

        public PROJECTILE_STATE ProjectileState
        {
            get { return projectileState; }
            set { projectileState = value; }
        }


        /*

        public TilePlayer(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        */
        public Projectile(Game game, Vector2 projectilePosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
            : base(game, projectilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Target = Vector2.Zero;
            TexCenter = new Vector2(texture.Width / 2, texture.Height / 2);
            //explosion = rocketExplosion;
            //explosion.position -= textureCenter;
            //explosion.Visible = false;
            StartPosition = projectilePosition;
            ProjectileState = PROJECTILE_STATE.STILL;

        }
        public override void Update(GameTime gametime)
        {
            switch (projectileState)
            {
                case PROJECTILE_STATE.STILL:
                    this.Visible = false;
                    explosion.Visible = false;
                    break;
                // Using Lerp here could use target - pos and normalise for direction and then apply
                // Velocity
                case PROJECTILE_STATE.FIRING:
                    this.Visible = true;
                    position = Vector2.Lerp(position, Target, 0.02f * RocketVelocity);
                    // rotate towards the Target
                    this.angleOfRotation = TurnToFace(position,
                                            Target, angleOfRotation, 1f);
                    if (Vector2.Distance(position, Target) < 2)
                        projectileState = PROJECTILE_STATE.EXPLODING;
                    break;
                case PROJECTILE_STATE.EXPLODING:
                    explosion.position = Target;
                    explosion.Visible = true;
                    break;
            }
            // if the explosion is visible then just play the animation and count the timer
            if (explosion.Visible)
            {
                explosion.Update(gametime);
                ExplosionTimer += gametime.ElapsedGameTime.Milliseconds;
            }
            // if the timer goes off the explosion is finished
            if (ExplosionTimer > ExplosionVisibleLimit)
            {
                explosion.Visible = false;
                ExplosionTimer = 0;
                projectileState = PROJECTILE_STATE.STILL;
            }

            base.Update(gametime);
        }
        public void fire(Vector2 SiteTarget)
        {
            projectileState = PROJECTILE_STATE.FIRING;
            Target = SiteTarget;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.Begin();
            //spriteBatch.Draw(spriteImage, position, SourceRectangle,Color.White);
            //spriteBatch.End();
            if (explosion.Visible)
                explosion.Draw(spriteBatch);
        }
    }
}
