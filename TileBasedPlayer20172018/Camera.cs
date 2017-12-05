using AnimatedSprite;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiler;
using Tiling;

namespace CameraNS
{
    class Camera : GameComponent
    {
        static Vector2 _camPos = Vector2.Zero;
        static Vector2 _worldBound;
        public float CameraSpeed = 0.03f;
        public float CameraSpread = 120f;

        public static Matrix CurrentCameraTranslation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(
                    -CamPos,
                    0));
            }
        }
        public static Vector2 CamPos
        {
            get
            {
                return _camPos;
            }

            set
            {
                _camPos = value;
            }
        }

        public Camera(Game game, Vector2 startPos, Vector2 bound) : base(game)
        {
            game.Components.Add(this);
            CamPos = startPos;
            _worldBound = bound;
        }

        public override void Update(GameTime gameTime)
        {
            TilePlayer player = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));

            if (player != null)
            {
                Follow((player.CentrePos + (player.Direction * CameraSpread)), Game.GraphicsDevice.Viewport, CameraSpeed);

                #region Clamp player within bounds
                player.PixelPosition = Vector2.Clamp(player.PixelPosition, Vector2.Zero,
                                                new Vector2(_worldBound.X - player.BoundingRectangle.Width,
                                                            _worldBound.Y - player.BoundingRectangle.Height));
                #endregion
            }

            base.Update(gameTime);
        }

        public static void Follow(Vector2 followPos, Viewport v, float cameraSpeed)
        {
            // Center of Viewport
            Vector2 centerScreen = new Vector2(v.Width / 2, v.Height / 2);
            // Add smoothness
            Vector2 delta = ((followPos - centerScreen) - _camPos); // Distance from following position to camera
            _camPos += Vector2.Multiply(delta, cameraSpeed); // Now move the camera by 3%
            _camPos = Vector2.Clamp(_camPos, Vector2.Zero, _worldBound - new Vector2(v.Width, v.Height));
        }
    }
}
