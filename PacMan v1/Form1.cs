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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace PacMan_v1
{
    public partial class Form1 : Form
    {

        bool goUp, goDown, goLeft, goRight, isGameOver;

        int score, playerSpeed, redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;

        public Form1()
        {
            InitializeComponent();

            resetGame();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) {goUp = true;}
            if (e.KeyCode == Keys.S) {goDown = true;}
            if (e.KeyCode == Keys.A) {goLeft = true;}
            if (e.KeyCode == Keys.D) {goRight = true;}

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
            } else if (goRight)
            {
                pacman.Left += playerSpeed;
                pacman.Image = Properties.Resources.right;
            } else if (goUp)
            {
                pacman.Top -= playerSpeed;
                pacman.Image = Properties.Resources.Up;
            } else if (goDown)
            {
                pacman.Top += playerSpeed;
                pacman.Image = Properties.Resources.down;
            }
            


            if(pacman.Left < -10)
            {
                pacman.Left = 680;
            }
            if(pacman.Left  > 680)
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
                    if ((string)x.Tag == "coin" && x.Visible)
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            score += 1;
                            x.Visible = false;
                        }
                    }
                    
                    if ((string)x.Tag == "wall")
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            pacman.Left = prevLeft;
                            pacman.Top = prevTop;
                        }

                        if (pinkGhost.Bounds.IntersectsWith(x.Bounds))
                        {
                            pinkGhostX = -pinkGhostX;
                        }
                    }

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


            // ghost movement
            redGhost.Left += redGhostSpeed;
            if (redGhost.Bounds.IntersectsWith(wall1.Bounds) || redGhost.Bounds.IntersectsWith(wall2.Bounds))
            {
                redGhostSpeed = -redGhostSpeed;
            }

            yellowGhost.Left += yellowGhostSpeed;
            if (yellowGhost.Bounds.IntersectsWith(wall3.Bounds) || yellowGhost.Bounds.IntersectsWith(wall4.Bounds))
            {
                yellowGhostSpeed = -yellowGhostSpeed;
            }

            pinkGhost.Left -= pinkGhostX;
            pinkGhost.Top -= pinkGhostY;
            
            if (pinkGhost.Top < 0 || pinkGhost.Top > 520)
            {
                pinkGhostY = -pinkGhostY;
            }

            if (pinkGhost.Right < 0 || pinkGhost.Right > 620)
            {
                pinkGhostX = -pinkGhostX;
            }



            if (score == 27)
            {
                // game won
                gameOver("You Win!");
            }

        }

        private void resetGame()
        {
            txtGameOver.Visible = false;
            label3.Visible = false;
            txtScore.Text = "0";
            score = 0;

            playerSpeed = 8;
            redGhostSpeed = 5; 
            yellowGhostSpeed = 5; 
            pinkGhostX = 5; 
            pinkGhostY = 5;

            isGameOver = false;

            pacman.Left = 29;
            pacman.Top = 366;
            redGhost.Left = 205;
            redGhost.Top = 64;
            yellowGhost.Left = 459;
            yellowGhost.Top = 450;
            pinkGhost.Left = 495;
            pinkGhost.Top = 203;

            foreach(Control x in this.Controls)
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
}
