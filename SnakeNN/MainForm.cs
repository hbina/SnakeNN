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
        private SelfBrain selfBrain;
        private WorldController worldController;

        public MainForm(int gameSizeX, int gameSizeY, int gameSpeed, int gamePoint)
        {
            InitializeComponent();
            worldController = new WorldController(gameSizeX, gameSizeY, gameSpeed, gamePoint);
            gameTimer.Interval = 1000 / worldController.GetGameSpeed();
            gameTimer.Tick += Render;
            gameTimer.Start();
            selfBrain = new SelfBrain();
            StartGame();
        }

        private void StartGame()
        {
            worldController.Clear();
            lblScore.Text = worldController.GetCurrentScore().ToString();
            lblGameOver.Visible = false;
        }

        private void Render(object sender, EventArgs e)
        {
            if (worldController.GetIsGameOver())
            {
                StartGame();
            }
            else
            {
#if player
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
#else 
                selfBrain.feed(worldController.Self.X, worldController.Self.Y, worldController.Target.X, worldController.Target.Y);
                worldController.Self.Direction = selfBrain.decide();
#endif
            }

            worldController.UpdateWorld();
            InputController.ClearTable();
            pbCanvas.Invalidate();

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            InputController.ChangeState(e.KeyCode, true);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // Update all texts
            lblScore.Text = worldController.GetCurrentScore().ToString();

            int ellipseSizeX = (pbCanvas.Width / worldController.GameGridX);
            int ellipseSizeY = (pbCanvas.Height / worldController.GameGridY);
            if (!worldController.GetIsGameOver())
            {
                for (int iterateY = 0; iterateY < worldController.GetWorldGrid().GetLength(0); iterateY++)
                {
                    for (int iterateX = 0; iterateX < worldController.GetWorldGrid().GetLength(1); iterateX++)
                    {
                        canvas.FillEllipse(worldController.GetColorOfObject(worldController.GetWorldGrid().GetStateAt(iterateX, iterateY)),
                            new Rectangle(iterateX * ellipseSizeX, iterateY * ellipseSizeY,
                                          ellipseSizeX, ellipseSizeY));
                    }
                }
            }
            else
            {
                string gameOverText = "Game over \n" + worldController.GetCurrentScore();
                //lblGameOver.Text = gameOverText;
                //lblGameOver.Visible = snakeSettings.isGameOver;
            }
        }

        private void TEXT_SCORE_Click(object sender, EventArgs e)
        {

        }
    }
}