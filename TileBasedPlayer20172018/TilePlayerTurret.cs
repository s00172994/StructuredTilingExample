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
using CameraNS;

namespace Tiler
{
    class TilePlayerTurret : RotatingSprite
    {
        float turnSpeed = 0.04f;
        const float WIDTH_IN = 11f; // Width in from the left for the sprites origin

        public Vector2 Direction;
        public Vector2 CentrePos;
        private Vector2 originToRotate;

        public TilePlayerTurret(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
            originToRotate = new Vector2(((FrameWidth / 2) - WIDTH_IN), (FrameHeight / 2));
            origin = originToRotate;
        }

        public override void Update(GameTime gameTime)
        {
            TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));

            CentrePos = PixelPosition + new Vector2(FrameWidth / 2, FrameHeight / 2);

            if (player != null)
            {
                Track(player.PixelPosition + new Vector2(WIDTH_IN, 0f));
            }

            this.angleOfRotation = TurnToFace(this.CentrePos, (InputEngine.MousePosition + Camera.CamPos), this.angleOfRotation, turnSpeed);

            Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

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
    }
}
