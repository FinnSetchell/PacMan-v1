using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan_v1.Ghost;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace PacMan_v1
{
    public partial class Form1 : Form
    {
        bool goUp, goDown, goLeft, goRight, isGameOver;

        int score, playerSpeed, ghostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;

        public Form1()
        {
            InitializeComponent();

            resetGame();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { goUp = true; }
            if (e.KeyCode == Keys.S) { goDown = true; }
            if (e.KeyCode == Keys.A) { goLeft = true; }
            if (e.KeyCode == Keys.D) { goRight = true; }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { goUp = false; }
            if (e.KeyCode == Keys.S) { goDown = false; }
            if (e.KeyCode == Keys.A) { goLeft = false; }
            if (e.KeyCode == Keys.D) { goRight = false; }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                resetGame();
            }
        }

        private void mainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = score.ToString();
            int prevLeft = pacman.Left;
            int prevTop = pacman.Top;

            if (goLeft)
            {
                pacman.Left -= playerSpeed;
                pacman.Image = Properties.Resources.left;
            }
            else if (goRight)
            {
                pacman.Left += playerSpeed;
                pacman.Image = Properties.Resources.right;
            }
            else if (goUp)
            {
                pacman.Top -= playerSpeed;
                pacman.Image = Properties.Resources.Up;
            }
            else if (goDown)
            {
                pacman.Top += playerSpeed;
                pacman.Image = Properties.Resources.down;
            }

            if (pacman.Left < -10)
            {
                pacman.Left = 680;
            }
            if (pacman.Left > 680)
            {
                pacman.Left = -10;
            }
            if (pacman.Top < -10)
            {
                pacman.Top = 550;
            }
            if (pacman.Top > 550)
            {
                pacman.Top = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    // collects coin and adds to score
                    if ((string)x.Tag == "coin" && x.Visible)
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            score += 1;
                            x.Visible = false;
                        }
                    }
                    // stops moving through walls
                    if ((string)x.Tag == "wall")
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            pacman.Left = prevLeft;
                            pacman.Top = prevTop;
                        }

                        if (Inky.Bounds.IntersectsWith(x.Bounds))
                        {
                            pinkGhostX = -pinkGhostX;
                        }
                    }
                    // ends game if pacman touchy ghosts
                    if ((string)x.Tag == "ghost")
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            // game lost
                            gameOver("Lmao, you suck!");
                        }
                    }
                }
            }
            // checks if game won
            if (score == 27)
            {
                // game won
                gameOver("You Win!");
            }



            //**  ghost movement  **//
            Blinky.Left += redGhostSpeed;
            if (Blinky.Bounds.IntersectsWith(wall1.Bounds) || Blinky.Bounds.IntersectsWith(wall2.Bounds))
            {
                redGhostSpeed = -redGhostSpeed;
            }

            Pinky.Left += yellowGhostSpeed;
            if (Pinky.Bounds.IntersectsWith(wall3.Bounds) || Pinky.Bounds.IntersectsWith(wall4.Bounds))
            {
                yellowGhostSpeed = -yellowGhostSpeed;
            }

            Inky.Left -= pinkGhostX;
            Inky.Top -= pinkGhostY;

            if (Inky.Top < 0 || Inky.Top > 520)
            {
                pinkGhostY = -pinkGhostY;
            }

            if (Inky.Right < 0 || Inky.Right > 620)
            {
                pinkGhostX = -pinkGhostX;
            }
            
        }

        private void resetGame()
        {
            // Create an array of ghosts
            List<Ghost> ghosts = new List<Ghost>();
            ghosts.Add(new Ghost("Blinky", 289, 411, 1));
            ghosts.Add(new Ghost("Pinky", 327, 411, 2));
            ghosts.Add(new Ghost("Inky", 365, 411, 3));
            //ghosts.Add(new Ghost("Clyde", 0, 0, 4));

            // reset & hide labels
            txtGameOver.Visible = false;
            label3.Visible = false;
            txtScore.Text = "0";
            score = 0;

            // resets positions
            pacman.Left = 1;
            pacman.Top = 402;

            playerSpeed = 8;
            isGameOver = false;

            // makes all elements visible
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    x.Visible = true;
                }
            }

            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();

            txtScore.Text = score.ToString();
            txtGameOver.Text = message + Environment.NewLine + "press ENTER to play again";
            txtGameOver.Visible = true;
            label3.Visible = true;
        }

    }

    public class Ghost
    {
        public string name;
        public int x;
        public int y;
        public int speed;
        public bool frightened;

        public Ghost(string name, int x, int y, int speed)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.speed = speed;
            this.frightened = false;
        }

        public enum GhostName
        {
            Blinky,
            Pinky,
            Inky,
            Clyde
        }
        public enum GhostSpeed
        {
            Slow = 1,
            Medium = 2,
            Fast = 3,
            VeryFast = 4
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public bool Frightened
        {
            get { return frightened; }
            set { frightened = value; }
        }
        public void Move(int dx, int dy)
        {
            x += dx;
            y += dy;
        }
    }

    
}
