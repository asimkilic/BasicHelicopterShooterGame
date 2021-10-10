using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelicopterShooter
{
    public partial class frmGame : Form
    {
        bool goUp, goDown, shot, gameOver;

        int score = 0;
        int speed = 8;
        int ufoSpeed = 10;

        Random rnd = new Random();

        int playerSpeed = 7;
        int index = 0;  //?

        public frmGame()
        {
            InitializeComponent();


        }
        private void frmGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Space && shot == false)
            {
                MakeBullet();
                shot = true;
            }
        }

        private void frmGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (shot == true)
            {
                shot = false;
            }
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            lblScore.Text = "Score: " + score;
            if (goUp == true && pbPlayer.Top > 0) pbPlayer.Top -= playerSpeed;
            if (goDown == true && pbPlayer.Top + pbPlayer.Height < this.ClientSize.Height) pbPlayer.Top += playerSpeed;
            pbUfo.Left -= ufoSpeed;
            if (pbUfo.Left + pbUfo.Width < 0) ChangeUFO();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "pillar")
                {
                    x.Left -= speed;
                    if (x.Left < -200) x.Left = 1000;
                    if (pbPlayer.Bounds.IntersectsWith(x.Bounds)) GameOver(); // bullet intersect ile beraber
                }

                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Left += 25;
                    if (x.Left > 900) RemoveBullet(((PictureBox)x));
                    if (pbUfo.Bounds.IntersectsWith(x.Bounds)) //x is bullet
                    {
                        RemoveBullet(((PictureBox)x));
                        score++;
                        ChangeUFO();
                    }
                }
            }
            if (pbPlayer.Bounds.IntersectsWith(pbUfo.Bounds)) GameOver();
            if (score > 10)
            {
                speed = 12;
                ufoSpeed = 18;
            }
        }
        private void RestartGame()
        {
            goUp = false;
            goDown = false;
            shot = false;
            gameOver = false;
            score = 0;
            speed = 8;
            ufoSpeed = 10;
            lblScore.Text = "Score: " + score;
            ChangeUFO();

            pbPlayer.Location = new Point(39, 119);
            pbPillar1.Location = new Point(338, -6);
            pbPillar2.Location = new Point(495, 246);

            gameTimer.Start();
        }
        private void GameOver()
        {
            gameTimer.Stop();
            lblScore.Text = "Score: " + score + " Game over, press enter to retry!";
            gameOver = true;
        }
        private void RemoveBullet(PictureBox bullet)
        {
            this.Controls.Remove(bullet);
            bullet.Dispose();
        }
        private void MakeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.BackColor = Color.Maroon;
            bullet.Height = 5;
            bullet.Width = 10;

            bullet.Left = pbPlayer.Left + pbPlayer.Width;
            bullet.Top = pbPlayer.Top + pbPlayer.Height / 2;

            bullet.Tag = "bullet";
            this.Controls.Add(bullet);
        }
        private void ChangeUFO()
        {
            if (index > 3)
            {
                index = 1;
            }
            else
            {
                index++;
            }
            switch (index)
            {
                case 1:
                    pbUfo.Image = Properties.Resources.alien1;
                    break;
                case 2:
                    pbUfo.Image = Properties.Resources.alien2;
                    break;
                case 3:
                    pbUfo.Image = Properties.Resources.alien3;
                    break;

            }
            pbUfo.Left = 1000;
            pbUfo.Top = rnd.Next(20, this.ClientSize.Height - pbUfo.Height);
        }


    }
}
