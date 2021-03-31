using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Collections.Generic;

namespace FlappyBirdTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
 
    public partial class MainWindow : Window
    {
        
        int speed = 10;
        int movespeed = 5;
        bool goUp;
        int score = 0;
        DispatcherTimer timer;
        int highScore;
        MediaPlayer background = new MediaPlayer();
        System.Media.SoundPlayer chirp = new System.Media.SoundPlayer(Environment.CurrentDirectory + @"\sounds\chirp.wav");

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            //timer.Start();
            timer.Tick += _timer_Tick;
            highScore = BitConverter.ToInt32(File.ReadAllBytes("scores.txt"));
            background.MediaEnded += startAgain;
            background.Open(new Uri(Environment.CurrentDirectory + @"\sounds\backgroundMusic.wav"));
            background.Play();
            chirp.LoadAsync();
            //chirp.Open(new Uri(Environment.CurrentDirectory + @"\sounds\chirp.wav"));

           

        }

        void startAgain(object sender, EventArgs e)
        {
            background.Position = TimeSpan.Zero;
            background.Play();
        }



        void _timer_Tick(object sender, EventArgs e)
        {


            label.Content = "Score:" + score;

           

            if (Canvas.GetLeft(cloud1) < -cloud1.Width)
                Canvas.SetLeft(cloud1, Application.Current.MainWindow.Width + cloud1.Width);
            else
                Canvas.SetLeft(cloud1, Canvas.GetLeft(cloud1) - movespeed);

            if (Canvas.GetLeft(cloud2) < -cloud2.Width)
                Canvas.SetLeft(cloud2, Application.Current.MainWindow.Width + cloud2.Width);
            else
                Canvas.SetLeft(cloud2, Canvas.GetLeft(cloud2) - movespeed);





            if (Canvas.GetLeft(top1) < -top1.Width)
            {
                Canvas.SetLeft(top1, Application.Current.MainWindow.Width + top1.Width);
                score++;
                if (score % 5 == 0)
                {
                    movespeed++;
                }
            }
            else
                Canvas.SetLeft(top1, Canvas.GetLeft(top1) - movespeed);

            if (Canvas.GetLeft(bottom1) < -bottom1.Width)
                Canvas.SetLeft(bottom1, Application.Current.MainWindow.Width + bottom1.Width);
            else
                Canvas.SetLeft(bottom1, Canvas.GetLeft(bottom1) - movespeed);



            if (Canvas.GetLeft(top2) < -top2.Width)
            {
                Canvas.SetLeft(top2, Application.Current.MainWindow.Width + top2.Width);
                score++;
                if (score % 5 == 0)
                {
                    movespeed++;
                }
            }
            else
                Canvas.SetLeft(top2, Canvas.GetLeft(top2) - movespeed);

            if (Canvas.GetLeft(bottom2) < -bottom2.Width)
                Canvas.SetLeft(bottom2, Application.Current.MainWindow.Width + bottom2.Width);
            else
                Canvas.SetLeft(bottom2, Canvas.GetLeft(bottom2) - movespeed);





            if (goUp && Canvas.GetTop(rec) > 0)
            {
                Canvas.SetTop(rec, Canvas.GetTop(rec) - speed);
                
                
            }
                



            Canvas.SetTop(rec, Canvas.GetTop(rec) + 5);
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if(x.Tag != null)
                {
                    var platformId = (string)x.Tag;
                    if(platformId == "platform")
                    {
                        Rect player = new Rect(Canvas.GetLeft(rec), Canvas.GetTop(rec), rec.Width, rec.Height);

                        Rect platform = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                        if (player.IntersectsWith(platform))
                        {
                            movespeed = 0;
                            Canvas.SetTop(rec, Canvas.GetTop(rec)+2);
                            timer.Stop();
                            //MessageBox.Show("Game Over");
                            label3.Content = "Press Space to restart!";
                            if(score>highScore)
                            {
                                highScore = score;
                                File.WriteAllBytes("scores.txt",BitConverter.GetBytes(highScore));
                            }
                            label4.Content = "High Score:" + highScore;

                        }
                      //  else
                        //    dropspeed = 5;
                    }
                    
                }
            }



        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                goUp = true;
                rec.RenderTransform = new RotateTransform(-20, rec.Width / 2, rec.Height / 2);
                chirp.Play();
            }

            if (e.Key == Key.Space)
                restart();


        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                goUp = false;
                rec.RenderTransform = new RotateTransform(0, rec.Width / 2, rec.Height / 2);
                chirp.Stop();
            }

        }



        private void restart()
        {
            Canvas.SetLeft(cloud1, 200);
            Canvas.SetTop(cloud1, 90);


            Canvas.SetLeft(cloud2, 45);
            Canvas.SetTop(cloud2, 240);

            Canvas.SetLeft(rec, 10);
            Canvas.SetTop(rec, 50);

            Canvas.SetLeft(ground, 0);
            Canvas.SetTop(ground, 394);

            Canvas.SetLeft(grass, 0);
            Canvas.SetTop(grass, 384);

            Canvas.SetLeft(top1, 252);
            Canvas.SetTop(top1, 0);

            Canvas.SetLeft(bottom1, 252);
            Canvas.SetTop(bottom1, 284);

            Canvas.SetLeft(top2, 528);
            Canvas.SetTop(top2, 0);

            Canvas.SetLeft(bottom2, 528);
            Canvas.SetTop(bottom2, 240);
            score = 0;
            goUp = false;
            movespeed = 5;
            speed = 10;
            background.Play();
            Thread.Sleep(500);
            label2.Content = "";
            label3.Content = "";
            label4.Content = "";
            timer.Start();

        }


    }
}
