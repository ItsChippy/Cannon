using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace Cannon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //cannon "trajectory line" variables

        //the positions that make up the "trajectory line"
        Vector2 cannonLineStartPos;
        Vector2 cannonLineEndPos;

        //color and thickness of the cannon's "trajectory line"
        int cannonLineThickness;
        Color cannonLineColor;

        //input your own angle to determine the "trajectory" line of the cannon
        float[] cannonAngles;

        //cannon variables

        Vector2 cannonStartPos;

        int cannonThickness;
        Color cannonColor;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //input custom angle to update the trajectory of the cannon and it's "laser"
            cannonAngles = new float[] {0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90 };

 
            //sets cannon "trajectory" line in the middle of the game window pointing upwards to the right
            cannonLineStartPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            cannonLineEndPos = new Vector2(cannonLineStartPos.X + 100, cannonLineStartPos.Y - 100);

            //cannon "trajectory" line aesthetics
            cannonLineThickness = 5;
            cannonLineColor = Color.Red;

            cannonStartPos = new Vector2(cannonLineStartPos.X - 50, cannonLineStartPos.Y + 50);
            cannonThickness = 20;
            cannonColor = Color.White;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            //method to draw out the "trajectory" line.
            foreach (var angle in cannonAngles)
            {
                DrawLine(_spriteBatch, cannonLineStartPos, cannonLineEndPos, cannonLineThickness, cannonLineColor, angle);
            }
            DrawLine(_spriteBatch, cannonStartPos, cannonLineStartPos, cannonThickness, cannonColor, 45);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startPos, Vector2 endPos, int thickness, Color color, float angle)
        {
            
            //creates a line as "wide" as the distance between the two position, aka length
            var distance = (int)Vector2.Distance(startPos, endPos);
            var texture = new Texture2D(spriteBatch.GraphicsDevice, distance, thickness);

            //colors line to whatever color the user input
            var data = new Color[distance * thickness];
            for (int i = 0; i < data.Length; i++) 
            {
                data[i] = color;
            }
            texture.SetData(data);


            float rotation;
            
            if (angle > 90)
            {
                //A cannon can't go above 90 degrees, therefore if user puts in higher than 90, it defaults to it's maximum possible degree
                rotation = ConvertDegreesToRadians(-90); //set angle to negative to invert trajectory (Monogame's positive y-axis points downward)
            }
            else if (angle < 0)
            {
                //if user inputs an angle lower than 0 (horizontal plane) it defaults to a straight line because a cannon can't shoot downwards.
                rotation = ConvertDegreesToRadians(0); //set angle to negative to invert trajectory (Monogame's positive y-axis points downward)
            }
            else
            {
                rotation = ConvertDegreesToRadians(-angle); //set angle to negative to invert trajectory (Monogame's positive y-axis points downward)
            }
            var origin = new Vector2(0, thickness / 2);

            spriteBatch.Draw(texture, startPos, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 1f);
        }

        //converts degrees to radians through the normal formula pi through 180
        public static float ConvertDegreesToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }
    }
}
