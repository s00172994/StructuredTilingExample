﻿using System;
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
        public Vector2 CentrePos
        {
            get
            {
                return PixelPosition + new Vector2((FrameWidth / 2), (FrameHeight / 2));
            }
        }
        public string Name;

        public Sentry(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, string nameIn)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Name = nameIn;
            DrawOrder = 3;
        }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = PixelPosition;

            Direction = new Vector2((float)Math.Cos(this.angleOfRotation), (float)Math.Sin(this.angleOfRotation));

            base.Update(gameTime);
        }

    }
}
