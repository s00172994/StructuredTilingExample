using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBasedPlayer20172018
{
    class CountDown : GameComponent
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
            time = startTime;
            started = false;
            paused = false;
            finished = false;
            Text = "";
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

        public Vector2 Posistion
        {
            get { return position; }
            set { position = value; }
        }


        #endregion

        public override void Update(GameTime gameTime)
        {
            float delataTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (started)
            {
                if (!paused)
                {
                    if (time > 0)
                    {
                        time -= delataTime;
                    }
                    else
                    {
                        finished = true;
                    }
                }

                
            }
            Text = time.ToString();

            base.Update(gameTime);  
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(Font, Text, Posistion, Color.White, 0f, new Vector2(0,0), 0f, SpriteEffects.None, 200f);
        }
    }
}
