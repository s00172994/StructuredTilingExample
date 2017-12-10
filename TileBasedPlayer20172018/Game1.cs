using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

using Tiler;
using Tiling;
using CameraNS;
using Engine.Engines;
using Screens;
using Helpers;
using Microsoft.Xna.Framework.Media;

namespace TileBasedPlayer20172018
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera CurrentCamera;
        SplashScreen MainScreen;

        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> Colliders = new List<Collider>();

        string[] backTileNames = { "dirt", "ground", "metal", "ground2", "ground3", "ground4", "ground5", "ground7", "metal2", "metal3", "metal4" };
        public enum TileType { DIRT, GROUND, METAL, GROUND2, GROUND3, GROUND4,
                               GROUND5 };
        int tileWidth = 64;
        int tileHeight = 64;

        #region Tile Map
        int[,] tileMap = new int[,]
        {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,1,1,1,1,1,5,1,1,1,1,5,5,1,1,1,5,1,0,0,1,1,5,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,1,1,5,1,1,6,1,1,1,6,6,1,1,1,1,1,1,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0},
        {0,7,6,6,1,1,0,0,0,0,0,7,1,1,1,5,1,1,1,1,1,1,5,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,7,1,1,1,1,1,5,1,1,1,1,1,5,1,1,1,1,1,1,1,5,1,1,1,1,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,7,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,7,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,8,9,9,9,9,9,9,9,9,2,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,1,1,5,1,1,1,1,1,1,2,8,9,9,9,9,9,9,9,9,2,2,1,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,1,4,4,4,4,4,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,5,1,1,1,1,1,1,1,5,1,1,1,2,8,9,9,9,9,9,9,9,9,2,2,1,1,1,2,8,9,9,10,2,1,1,1,1,5,1,1,1,1,4,4,4,4,4,1,1,5,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,5,1,1,1,1,1,1,1,2,8,9,9,9,9,9,9,9,9,2,1,1,1,1,2,8,9,9,10,2,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,5,1,1,1,1,5,1,1,1,1,1,1,2,8,9,9,9,9,9,9,9,9,2,1,1,1,1,2,8,9,9,10,2,1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,2,2,2,1,1,1,0,0},
        {0,0,7,6,6,1,1,6,1,6,1,1,1,1,1,5,2,8,9,9,9,9,9,9,9,9,2,1,1,5,1,2,8,9,9,10,2,1,1,1,1,5,1,1,1,1,1,1,1,1,5,1,2,2,2,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,5,2,8,9,9,9,9,9,9,9,10,2,1,1,1,1,2,8,9,9,10,2,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,8,9,9,9,9,9,9,9,10,2,1,1,1,1,2,8,9,9,10,2,1,1,5,1,1,2,2,2,2,1,1,5,1,1,1,1,1,1,1,1,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,8,9,9,9,9,9,9,9,10,2,1,1,1,1,2,2,2,2,1,1,1,1,1,2,2,2,2,2,2,1,1,1,1,6,6,1,1,6,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,8,9,9,9,9,9,9,9,10,2,5,1,1,5,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,5,1,1,1,0,0,1,1,5,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,5,1,1,1,1,2,2,2,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,5,1,5,1,5,1,1,1,1,1,1,5,1,1,5,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,5,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,1,0,0,0,0,1,6,1,6,1,1,5,1,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,5,1,1,1,2,2,2,2,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,0,0},
        {0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,0,0},
        {0,0,1,1,0,0,0,0,0,0,0,0,0,0,5,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,5,1,1,5,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,1,1,1,1,5,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,0,0,0,0,0,1,1,1,5,2,2,2,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
        {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,1,1,1,1,2,2,2,1,1,1,1,1,1,1,5,1,1,1,0,0},
        {0,0,1,6,1,1,1,1,1,6,1,1,1,1,6,1,1,6,1,1,6,1,1,1,1,1,6,1,1,6,1,1,1,1,1,1,1,1,0,0,0,0,0,2,2,2,2,1,1,1,1,1,1,6,6,1,1,1,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            IsMouseVisible = false;
            IsFixedTimeStep = true;

            Window.Title = "Tile Based Tank Game - 2D Game Programming Assignment";
            Window.AllowAltF4 = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            CurrentCamera = new Camera(this, Vector2.Zero,
                new Vector2((tileMap.GetLength(1) * tileWidth), 
                            (tileMap.GetLength(0) * tileHeight)));
            Services.AddService(CurrentCamera);

            new InputEngine(this);

            #region Create Player Tank
            TilePlayer tankPlayer = new TilePlayer(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(10, 0, 0),
            }, 64, 64, 0f);

            TilePlayerTurret tankPlayerTurret = new TilePlayerTurret(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(10, 1, 0),
            }, 64, 64, 0f,
            Content.Load<SoundEffect>("audio/PlayerTankShoot"),
            Content.Load<SoundEffect>("audio/PlayerTankReload"));

            // Add Tank Projectile
            Projectile bullet = new Projectile(this, "PLAYER", tankPlayerTurret.CentrePos, new List<TileRef>()
            {
                new TileRef(10, 2, 0),
            }, 64, 64, 0f, tankPlayerTurret.Direction,
            Content.Load<SoundEffect>("audio/TankShoot"));

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

            Projectile enemyBulletOne = new Projectile(this, "SENTRY", tankPlayerTurret.CentrePos, new List<TileRef>()
            {
                new TileRef(10, 2, 0),
            }, 64, 64, 0f, enemyTurretOne.Direction, Content.Load<SoundEffect>("audio/SentryTankShootAlt"), 3f);

            enemyTurretOne.AddProjectile(enemyBulletOne);
            Services.AddService(enemyTurretOne);

            #endregion

            #endregion

            new Crosshair(this, new Vector2(0, 0), new List<TileRef>()
            {
                new TileRef(10, 3, 0),
            }, 64, 64, 0f);

            SetColliders(TileType.DIRT);
            SetColliders(TileType.METAL);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Helper.graphicsDevice = GraphicsDevice;

            // Add SpriteBatch to services, it can be called anywhere.
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"tiles/tilesheet"));

            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            TileRefs.Add(new TileRef(7, 0, 0)); // Dirt
            TileRefs.Add(new TileRef(4, 1, 1)); // Ground
            TileRefs.Add(new TileRef(3, 2, 2)); // Metal Box
            TileRefs.Add(new TileRef(5, 0, 3)); // Ground 2
            TileRefs.Add(new TileRef(5, 1, 4)); // groud 3
            TileRefs.Add(new TileRef(7, 2, 5)); // ground 4
            TileRefs.Add(new TileRef(7, 3, 6)); // ground 5
            TileRefs.Add(new TileRef(6, 2, 7)); // ground 4
            TileRefs.Add(new TileRef(1, 1, 8)); // metal 2
            TileRefs.Add(new TileRef(2, 1, 9)); // metal 3
            TileRefs.Add(new TileRef(3, 1, 10)); // metal 4

            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);

            // This code is used to find tiles of a specific type
            //List<Tile> tileFound = SimpleTileLayer.GetNamedTiles(backTileNames[(int)TileType.GREENBOX]);

            #region Splash Screen
            MainScreen = new SplashScreen(this, Vector2.Zero, 120000.00f,
                            Content.Load<Texture2D>("background/MainMenu"),
                            Content.Load<Texture2D>("background/Pause"),
                            Content.Load<Texture2D>("background/Lose"),
                            Content.Load<Texture2D>("background/Win"),
                            Content.Load<Song>("audio/MainMenu"),
                            Content.Load<Song>("audio/Play"),
                            Content.Load<Song>("audio/Pause"),
                            Content.Load<Song>("audio/GameOver"),
                            Content.Load<Song>("audio/Win"),
                            Keys.P, 
                            Keys.Enter,
                            Content.Load<SpriteFont>("fonts/font"),
                            Content.Load<SoundEffect>("audio/BlinkPlay"),
                            Content.Load<SoundEffect>("audio/BlinkPause"));
            #endregion
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (this.IsActive)
                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            base.Draw(gameTime);
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        Colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"tiles/collider"),
                            x, y
                            ));
                    }
                }
            }
        }
    }
}
