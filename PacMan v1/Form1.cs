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

        int score, playerSpeed;

        List<Ghost> ghosts = new List<Ghost>{
            new Ghost("Blinky", 435, 305, 1, 2, false),
            new Ghost("Pinky", 435, 340, 2, 2, false),
            new Ghost("Inky", 435, 375, 3, 2, false)
            //new Ghost("Clyde", 0, 0, 4, 1, false)
        };
        
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
            if (goRight)
            {
                pacman.Left += playerSpeed;
                pacman.Image = Properties.Resources.right;
            }
            if (goUp)
            {
                pacman.Top -= playerSpeed;
                pacman.Image = Properties.Resources.Up;
            }
            if (goDown)
            {
                pacman.Top += playerSpeed;
                pacman.Image = Properties.Resources.down;
            }

            if (pacman.Left < -10)
            {
                pacman.Left = 760;
            }
            if (pacman.Left > 760)
            {
                pacman.Left = -10;
            }
            if (pacman.Top < -10)
            {
                pacman.Top = 760;
            }
            if (pacman.Top > 980)
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

                        if (Blinky.Bounds.IntersectsWith(x.Bounds)) { MoveGhost(Blinky, 0, -1); }
                        if (Pinky.Bounds.IntersectsWith(x.Bounds)) { MoveGhost(Pinky, 1, -1); }
                        if (Inky.Bounds.IntersectsWith(x.Bounds)) { MoveGhost(Inky, 2, -1); }
                    }
                    // ends game if pacman touches ghosts
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
            MoveGhost(Blinky, 0, ghosts[0].Direction);
            MoveGhost(Pinky, 1, ghosts[1].Direction);
            MoveGhost(Inky, 2, ghosts[2].Direction);

        }

        public void MoveGhost(PictureBox ghost, int ghostsInt, int direction)
        {
            if (direction == -1)
            {
                while (direction != ghosts[ghostsInt].Direction)
                {
                    // Generate a random direction
                    Random random = new Random();
                    direction = random.Next(4);
                }
                ghosts[ghostsInt].Direction = direction;
            }

            // Compute the new position based on the direction and speed of the ghost
            int dx = 0, dy = 0;
            switch (direction)
            {
                case 0: dy = -ghosts[ghostsInt].Speed; break; // up
                case 1: dy = ghosts[ghostsInt].Speed; break; // down
                case 2: dx = -ghosts[ghostsInt].Speed; break; // left
                case 3: dx = ghosts[ghostsInt].Speed; break; // right
            }
            int newTop = ghost.Top + dx;
            int newLeft = ghost.Left + dy;

            // Check if the new position is within the boundaries of the game
            if (newTop >= 0 && newTop + ghost.Height <= 760 &&
                newLeft >= 0 && newLeft + ghost.Width <= 980)
            {
                // Move the ghost to the new position
                ghost.Top = newTop;
                ghost.Left = newLeft;
            }
            else
            {
                // Choose a new random direction
                MoveGhost(ghost, ghostsInt, -1);
            }
        }

        private void resetGame()
        {
            
            Blinky.Left = ghosts[0].Left;
            Blinky.Top = ghosts[0].Top;
            Pinky.Left = ghosts[1].Left;
            Pinky.Top = ghosts[1].Top;
            Inky.Left = ghosts[2].Left;
            Inky.Top = ghosts[2].Top;

            // reset & hide labels
            txtGameOverLabel.Visible = false;
            gameOverLabel.Visible = false;
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
            txtGameOverLabel.Text = message + Environment.NewLine + "press ENTER to play again";
            txtGameOverLabel.Visible = true;
            gameOverLabel.Visible = true;
        }

    }

    public class Ghost
    {
        public string name;
        public int top;
        public int left;
        public int speed;
        public int direction;
        public bool frightened;

        public Ghost(string name, int top, int left, int speed, int direction, bool frightened)
        {
            this.name = name;
            this.top = top;
            this.left = left;
            this.speed = speed;
            this.direction = direction;
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
        public int Top
        {
            get { return top; }
            set { top = value; }
        }
        public int Left
        {
            get { return left; }
            set { left = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public bool Frightened
        {
            get { return frightened; }
            set { frightened = value; }
        }
    }

    
}
