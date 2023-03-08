using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }

        private void keyisup(object sender, KeyEventArgs e)
        {

        }

        private void mainGameTimer(object sender, EventArgs e)
        {

        }

        private void resetGame()
        {
            txtScore.Text = "SCORE: 0";
            score = 0;

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

        }

    }
}
