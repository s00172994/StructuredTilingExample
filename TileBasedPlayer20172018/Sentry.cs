using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tiling;
using AnimatedSprite;

namespace Tiler
{
    class Sentry : RotatingSprite
    {
        public Vector2 Direction;
        public Vector2 PreviousPosition;
        public Vector2 CentrePos;

        public Sentry(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 3;
        }

        public override void Update(GameTime gameTime)
        {
            CentrePos = PixelPosition + new Vector2((FrameWidth / 2), (FrameHeight / 2));

            PreviousPosition = PixelPosition;

            Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

            base.Update(gameTime);
        }

    }
}
