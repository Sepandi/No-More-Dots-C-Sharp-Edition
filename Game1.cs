using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    //--Menu
    Texture2D menu;
    bool menuActive = true;

    //--Background
    Texture2D background;

    //-Font
    SpriteFont font;
    SpriteFont smallerFont;

    //--Player
    Texture2D player1;
    Texture2D player2;
    Texture2D player3;
    Texture2D player4;
    Vector2 playerPos = new Vector2(400, 300);
    Vector2 PLAYERPOS = new Vector2(400, 300);
    int Score = 0;
    bool playerCanMove = true;
    Vector2 scorePos;
    bool AutoBot = false;
    
    //--Bit
    static Random rnd = new Random();
    Texture2D bit;
    static int xRnd = rnd.Next(21,747);
    static int yRnd = rnd.Next(65, 516);
    static Vector2 bitPos = new Vector2(xRnd , yRnd);
    

    //--Animation-Timer
    float playerAnimTimer = 1;
    const float TIMER = 1;
    float timer = 30;
    const float GAMETIMER = 30;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        //--Window-Setting
        Window.Title = ("No More Dots - C# Edition");
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        background = Content.Load<Texture2D>("background");
        player1 = Content.Load<Texture2D>("Player/player1");
        player2 = Content.Load<Texture2D>("Player/player2");
        player3 = Content.Load<Texture2D>("Player/player3");
        player4 = Content.Load<Texture2D>("Player/player4");
        bit = Content.Load<Texture2D>("bit");
        font = Content.Load<SpriteFont>("font");
        menu = Content.Load<Texture2D>("menu");
        smallerFont = Content.Load<SpriteFont>("smallerfont");
    }

    protected override void Update(GameTime gameTime)
    {
        float gameElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (menuActive == false)
        {
            
            timer -= gameElapsed;
            if (timer <= 0)
            {
                playerCanMove = false;
            }

            //--Set-Score-Position
            if (Score < 10)
            {
                scorePos = new Vector2(740, 27);
            }
            else if (Score >= 10)
            {
                scorePos = new Vector2(730, 27);
            }

            //- Player Animation Timer
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerAnimTimer -= elapsed;
            if (playerAnimTimer < 0)
            {
                playerAnimTimer = TIMER;
            }


            //--Border
            if (playerPos.X <= 21)
            {
                playerPos.X = 21;
            }
            if (playerPos.X >= 747)
            {
                playerPos.X = 747;
            }
            if (playerPos.Y <= 65)
            {
                playerPos.Y = 65;
            }
            if (playerPos.Y >= 516)
            {
                playerPos.Y = 516;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                menuActive = true;
                timer = GAMETIMER;
                Score = 0;
            }
            if (playerCanMove == true)
            {
                //--Key-Binds
                    
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    playerPos.Y -= 4;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    playerPos.Y += 4;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    playerPos.X += 4;
                }
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    playerPos.X -= 4;
                }
            }
            
            //--Check-For-Colide-of-Player-And-Bit
            if (new Rectangle((int)playerPos.X,(int)playerPos.Y,player1.Bounds.Width,player1.Bounds.Height).Intersects(new Rectangle((int)bitPos.X,(int)bitPos.Y,bit.Bounds.Width,bit.Bounds.Height)))
            {
                xRnd = rnd.Next(23, 747);
                yRnd = rnd.Next(67, 510);
                bitPos = new Vector2(xRnd, yRnd);
                Score += 1;
            }
        }else
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                menuActive = false;
                playerCanMove = true;
                AutoBot = false;
                playerPos = PLAYERPOS;
                xRnd = rnd.Next(23, 747);
                yRnd = rnd.Next(67, 510);
                bitPos = new Vector2(xRnd, yRnd);

            }
        }
        if (timer <= 0)
        {
            timer = 0;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.P)&& Keyboard.GetState().IsKeyDown(Keys.O) && Keyboard.GetState().IsKeyDown(Keys.I))
        {
            AutoBot = true;
        }
        if (AutoBot == true)
        {
            Vector2 dir = bitPos - playerPos;
            dir.Normalize();
            playerPos += dir*10;

        }
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(153,229,80));
        _spriteBatch.Begin();
        if (menuActive == false)
        {
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(menu, new Vector2(10, 10), Color.White);
            //--Player-Animation-------------------------------------------------
            if (playerAnimTimer >= 0.75 & playerAnimTimer <= 1)
            {
                _spriteBatch.Draw(player1, playerPos, Color.White);
            }
            if (playerAnimTimer >= 0.5 & playerAnimTimer < 0.75)
            {
                _spriteBatch.Draw(player2, playerPos, Color.White);
            }
            if (playerAnimTimer >= 0.25 & playerAnimTimer <= 0.5)
            {
                _spriteBatch.Draw(player3, playerPos, Color.White);
            }
            if (playerAnimTimer >= 0 & playerAnimTimer <= 0.25)
            {
                _spriteBatch.Draw(player4, playerPos, Color.White);
            }

            //--Show-Score
            _spriteBatch.DrawString(font, Score.ToString(), scorePos , Color.White);

            //--Show-timer-in-game
            _spriteBatch.DrawString(smallerFont, "Time Left = " +Math.Round(timer).ToString(), new Vector2(30, 560), Color.White);

            //--Enemy
            _spriteBatch.Draw(bit, bitPos, Color.White);
            void gameOver()
            {
                _spriteBatch.DrawString(smallerFont, "GAME OVER . Your Score = "+ Score.ToString(), new Vector2(190, 100), Color.White);
            }
            if (timer <= 0)
            {

                gameOver();
            }
        }
        else
        {
            _spriteBatch.DrawString(font, "No More Dots", new Vector2(200, 27), Color.White);
            _spriteBatch.DrawString(smallerFont, "Press Enter to Start", new Vector2(240, 400), new Color(106, 190, 48));
            _spriteBatch.DrawString(smallerFont, "By Sepano Darbandi(Sepandi)", new Vector2(370, 580), new Color(106, 190, 48));
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

