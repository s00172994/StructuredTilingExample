using CameraNS;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Tiler;
using Tiling;

namespace TileBasedPlayer20172018
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();

        string[] backTileNames = { "dirt", "ground", "metal", "ground2", "ground3" };
        public enum TileType { Dirt, PAVEMENT, Metal, GREENBOX, HOME };

        int tileWidth = 64;
        int tileHeight = 64;
        Song backgroundMusic;

        CountDown timer;

        #region Tile Map
        int[,] tileMap = new int[,]
        {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,1,4,4,4,4,4,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,4,4,4,4,4,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,3,3,3,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,1,1,1,1,1,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0},
        {0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            IsMouseVisible = false;
            IsFixedTimeStep = true;

            Window.Title = "Tile Based Tank Game - 2D Game Programming Assignment";
            Window.AllowAltF4 = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            new Camera(this, Vector2.Zero,
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));

            new InputEngine(this);

            #region Create Player Tank
            TilePlayer tankPlayer = new TilePlayer(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(10, 0, 0),
            }, 64, 64, 0f);

            TilePlayerTurret tankPlayerTurret = new TilePlayerTurret(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(10, 1, 0),
            }, 64, 64, 0f);

            // Add Projectile
            Projectile bullet = new Projectile(this, tankPlayer.CentrePos, new List<TileRef>()
            {
                new TileRef(10, 2, 0),
            }, 64, 64, 0f, tankPlayerTurret.Direction);

            tankPlayerTurret.AddProjectile(bullet);

            Services.AddService(tankPlayer);
            Services.AddService(tankPlayerTurret);
            #endregion

            #region Create Sentry Tanks

            Sentry enemyOne = new Sentry(this, new Vector2(320, 128), new List<TileRef>()
            {
                new TileRef(10, 4, 0),
            }, 64, 64, 0f, "Enemy Tank 1");

            Sentry enemyTwo = new Sentry(this, new Vector2(420, 128), new List<TileRef>()
            {
                new TileRef(10, 4, 0),
            }, 64, 64, 0f, "Enemy Tank 2");

            Services.AddService(enemyOne);

            #region Create Sentry Tank Turrets
            SentryTurret enemyTurretOne = new SentryTurret(this, new Vector2(128, 128), new List<TileRef>()
            {
                new TileRef(10, 5, 0),
            }, 64, 64, 0f, "Enemy Tank 1");

            Services.AddService(enemyTurretOne);

            #endregion

            #endregion

            new Crosshair(this, new Vector2(0, 0), new List<TileRef>()
            {
                new TileRef(10, 3, 0),
            }, 64, 64, 0f);

            SetColliders(TileType.Dirt);
            SetColliders(TileType.Metal);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Add SpriteBatch to services, it can be called anywhere.
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"tiles/tilesheet"));

            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            TileRefs.Add(new TileRef(7, 0, 0)); // Dirt
            TileRefs.Add(new TileRef(4, 1, 1)); // Ground
            TileRefs.Add(new TileRef(1, 2, 2)); // Metal Box 2
            TileRefs.Add(new TileRef(5, 0, 3)); // Ground 2
            TileRefs.Add(new TileRef(5, 1, 4)); // groud 3

            timer = new CountDown(this, 100.0f);
            timer.Font = Content.Load<SpriteFont>("count");
            timer.Posistion = new Vector2(this.Window.ClientBounds.Width / 2 - timer.Font.MeasureString(timer.Text).X / 2, 0);
            Components.Add(timer);
            backgroundMusic = Content.Load<Song>("MoreEx");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);

            // This code is used to find tiles of a specific type
            List<Tile> tileFound = SimpleTileLayer.GetNamedTiles(backTileNames[(int)TileType.GREENBOX]);
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"tiles/collider"),
                            x, y
                            ));
                    }
                }
            } 

           
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            timer.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
