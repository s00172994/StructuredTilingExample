﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBasedPlayer20172018
{
    class CountDown : DrawableGameComponent
    {
        private SpriteFont CounterFont;
        private string text;
        private float time;
        private Vector2 position;
        private bool started;
        private bool paused;
        private bool finished;

        public CountDown(Game game, float startTime) : base(game)
        {
            game.Components.Add(this);
            time = startTime;
            started = true;
            paused = false;
            finished = false;
            Text = "";
            DrawOrder = 200;
        }

        #region Properties

        public SpriteFont Font
        {
            get { return CounterFont; }
            set { CounterFont = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool Started
        {
            get { return started; }
            set { started= value; }
        }

        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }

        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds;

            if (started)
            {
                if (!paused)
                {
                    if (time > 0)
                    {
                        time -= deltaTime;
                    }
                    else
                    {
                        finished = true;
                    }
                }
            }

            Text = String.Format("{0}", Convert.ToInt32(time/1000));

            base.Update(gameTime);  
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, Text, Position, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

        }
    }
}
