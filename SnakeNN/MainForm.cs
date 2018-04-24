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
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public MainForm()
        {
            InitializeComponent();

            new SnakeSettings();

            gameTimer.Interval = 1000 / SnakeSettings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            new SnakeSettings();
            Snake.Clear(); // clear the List
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = SnakeSettings.Score.ToString();
            lblGameOver.Visible = false;
            GenerateFood();
        }

        // Generate random food
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / SnakeSettings.Width;
            int maxYPos = pbCanvas.Size.Height / SnakeSettings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (SnakeSettings.GameOver)
            {
                //Check if Enter is pressed
                if (SnakeInput.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (SnakeInput.KeyPressed(Keys.Right) && SnakeSettings.direction != Direction.Left)
                    SnakeSettings.direction = Direction.Right;
                else if (SnakeInput.KeyPressed(Keys.Left) && SnakeSettings.direction != Direction.Right)
                    SnakeSettings.direction = Direction.Left;
                else if (SnakeInput.KeyPressed(Keys.Up) && SnakeSettings.direction != Direction.Down)
                    SnakeSettings.direction = Direction.Up;
                else if (SnakeInput.KeyPressed(Keys.Down) && SnakeSettings.direction != Direction.Up)
                    SnakeSettings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (SnakeSettings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //Get maximum X and Y Pos
                    int maxXPos = pbCanvas.Size.Width / SnakeSettings.Width;
                    int maxYPos = pbCanvas.Size.Height / SnakeSettings.Height;

                    //Detect collission with game borders.
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //Detect collission with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Detect collision with food piece
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
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
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //Update Score
            SnakeSettings.Score += SnakeSettings.Points;
            lblScore.Text = SnakeSettings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            SnakeSettings.GameOver = true;
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!SnakeSettings.GameOver)
            {
                //Set colour of snake

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Draw head
                    else
                        snakeColour = Brushes.Green;    //Rest of body

                    //Draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * SnakeSettings.Width,
                                      Snake[i].Y * SnakeSettings.Height,
                                      SnakeSettings.Width, SnakeSettings.Height));


                    //Draw Food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * SnakeSettings.Width,
                             food.Y * SnakeSettings.Height, SnakeSettings.Width, SnakeSettings.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + SnakeSettings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }
    }
}
