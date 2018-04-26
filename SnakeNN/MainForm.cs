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
            gameTimer.Interval = 1000 / snakeSettings.gameSpeed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            snakeSettings.Clear();
            lblScore.Text = snakeSettings.currentScore.ToString();
            lblGameOver.Visible = false;
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (snakeSettings.isGameOver)
            {
                if (SnakeInput.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (SnakeInput.KeyPressed(Keys.Right) && snakeSettings.headDirection != SnakeDirection.LEFT)
                    snakeSettings.headDirection = SnakeDirection.RIGHT;
                else if (SnakeInput.KeyPressed(Keys.Left) && snakeSettings.headDirection != SnakeDirection.RIGHT)
                    snakeSettings.headDirection = SnakeDirection.LEFT;
                else if (SnakeInput.KeyPressed(Keys.Up) && snakeSettings.headDirection != SnakeDirection.DOWN)
                    snakeSettings.headDirection = SnakeDirection.UP;
                else if (SnakeInput.KeyPressed(Keys.Down) && snakeSettings.headDirection != SnakeDirection.UP)
                    snakeSettings.headDirection = SnakeDirection.DOWN;
                else
                    snakeSettings.headDirection = SnakeDirection.NULL;

                snakeSettings.UpdateWorld();
                SnakeInput.ClearTable();
            }

            pbCanvas.Invalidate();

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            SnakeInput.ChangeState(e.KeyCode, true);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // Update all texts
            lblScore.Text = snakeSettings.currentScore.ToString();

            int ellipseSizeX = (pbCanvas.Width / snakeSettings.gameGridX);
            int ellipseSizeY = (pbCanvas.Height / snakeSettings.gameGridY);
            if (!snakeSettings.isGameOver)
            {
                for (int iterateY = 0; iterateY < snakeSettings.snakeCartesian.GetLength(0); iterateY++)
                {
                    for (int iterateX = 0; iterateX < snakeSettings.snakeCartesian.GetLength(1); iterateX++)
                    {
                        canvas.FillEllipse(snakeSettings.GetColorOfObject(snakeSettings.snakeCartesian.GetWorld()[iterateX, iterateY]),
                            new Rectangle(iterateX * ellipseSizeX, iterateY * ellipseSizeY,
                                          ellipseSizeX, ellipseSizeY));
                    }
                }
            }
            else
            {
                string gameOverText = "Game over \n" + snakeSettings.currentScore;
                //lblGameOver.Text = gameOverText;
                lblGameOver.Visible = snakeSettings.isGameOver;
            }
        }
    }
}