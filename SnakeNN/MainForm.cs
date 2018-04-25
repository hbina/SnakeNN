using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeNN
{
    public partial class MainForm : Form
    {
        private SnakeSettings snakeSettings;
        public MainForm(int gameSizeX, int gameSizeY, int gameSpeed, int gamePoint)
        {
            InitializeComponent();
            snakeSettings = new SnakeSettings(gameSizeX, gameSizeY, gameSpeed, gamePoint);
            gameTimer.Interval = 1000 / snakeSettings.speed;
            gameTimer.Tick += UpdateScreen; //wtf is this?
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            snakeSettings.Clear();
            Circle head = new Circle();
            head.X = snakeSettings.width / 2;
            head.Y = snakeSettings.height / 2;
            snakeSettings.Snake.Add(head);

            lblScore.Text = snakeSettings.score.ToString();
            lblGameOver.Visible = false;
            GenerateFood();
        }

        // Generate random food
        private void GenerateFood()
        {
            Random random = new Random();
            snakeSettings.food = new Circle();
            snakeSettings.food.X = random.Next(0, snakeSettings.width);
            snakeSettings.food.Y = random.Next(0, snakeSettings.height);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (snakeSettings.gameOver)
            {
                //Check if Enter is pressed
                if (SnakeInput.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (SnakeInput.KeyPressed(Keys.Right) && snakeSettings.direction != Direction.Left)
                    snakeSettings.direction = Direction.Right;
                else if (SnakeInput.KeyPressed(Keys.Left) && snakeSettings.direction != Direction.Right)
                    snakeSettings.direction = Direction.Left;
                else if (SnakeInput.KeyPressed(Keys.Up) && snakeSettings.direction != Direction.Down)
                    snakeSettings.direction = Direction.Up;
                else if (SnakeInput.KeyPressed(Keys.Down) && snakeSettings.direction != Direction.Up)
                    snakeSettings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void MovePlayer()
        {
            for (int i = snakeSettings.Snake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (snakeSettings.direction)
                    {
                        case Direction.Right:
                            snakeSettings.Snake[i].X++;
                            break;
                        case Direction.Left:
                            snakeSettings.Snake[i].X--;
                            break;
                        case Direction.Up:
                            snakeSettings.Snake[i].Y--;
                            break;
                        case Direction.Down:
                            snakeSettings.Snake[i].Y++;
                            break;
                    }


                    //Get maximum X and Y Pos
                    int maxXPos = snakeSettings.width;
                    int maxYPos = snakeSettings.height;

                    //Detect collission with game borders.
                    if (snakeSettings.Snake[i].X < 0 || snakeSettings.Snake[i].Y < 0
                        || snakeSettings.Snake[i].X >= maxXPos || snakeSettings.Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //Detect collission with body
                    for (int j = 1; j < snakeSettings.Snake.Count; j++)
                    {
                        if (snakeSettings.Snake[i].X == snakeSettings.Snake[j].X &&
                           snakeSettings.Snake[i].Y == snakeSettings.Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Detect collision with food piece
                    if (snakeSettings.Snake[0].X == snakeSettings.food.X && snakeSettings.Snake[0].Y == snakeSettings.food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Move body
                    snakeSettings.Snake[i].X = snakeSettings.Snake[i - 1].X;
                    snakeSettings.Snake[i].Y = snakeSettings.Snake[i - 1].Y;
                    int hello;
                }
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            SnakeInput.ChangeState(e.KeyCode, true);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            SnakeInput.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //Add circle to body
            Circle circle = new Circle
            {
                X = snakeSettings.Snake[snakeSettings.Snake.Count - 1].X,
                Y = snakeSettings.Snake[snakeSettings.Snake.Count - 1].Y
            };
            snakeSettings.Snake.Add(circle);

            //Update Score
            snakeSettings.score += snakeSettings.points;
            lblScore.Text = snakeSettings.score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Console.WriteLine("snek ded :(");
            snakeSettings.gameOver = true;
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            int ellipseSizeX = (pbCanvas.Width / snakeSettings.width);
            int ellipseSizeY = (pbCanvas.Height / snakeSettings.height);
            if (!snakeSettings.gameOver)
            {
                //Set colour of snakeSettings.Snake

                //Draw snakeSettings.Snake
                for (int i = 0; i < snakeSettings.Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Draw head
                    else
                        snakeColour = Brushes.Green;    //Rest of body

                    //Draw Snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(snakeSettings.Snake[i].X * ellipseSizeX,
                                      snakeSettings.Snake[i].Y * ellipseSizeY,
                                      ellipseSizeX, ellipseSizeY));
                }
                //Draw Food
                canvas.FillEllipse(Brushes.Red,
                  new Rectangle(snakeSettings.food.X * ellipseSizeX,
                     snakeSettings.food.Y * ellipseSizeY, ellipseSizeX, ellipseSizeY));
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + snakeSettings.score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                //lblGameOver.Visible = true;
            }
        }
    }
}
