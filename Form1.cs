using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public partial class Form1 : Form
    {

        readonly int boxWidth = 80, boxMargin = 10;
        readonly int startX = 10, startY = 10; 

        readonly int r2 = 238, g2 = 228, b2 = 218;
        readonly int r4 = 238, g4 = 225, b4 = 201; 
        readonly int r8 = 243, g8 = 178, b8 = 122; 
        readonly int r16 = 0, g16 = 0, b16 = 0; 
        readonly int r32 = 0, g32 = 0, b32 = 0; 
        readonly int r64 = 0, g64 = 0, b64 = 0;

        //r[0] = r2
        readonly int[] r = { 238, 238, 243, 0, 0, 0 };
        readonly int[] g = { 228, 225, 178, 0, 0, 0 };
        readonly int[] b = { 218, 201, 122, 0, 0, 0 };

        bool up = false, down = false, right = false, left = false;
        int sequenceCount = 0;

        List<List<PictureBox>> box_positions = new List<List<PictureBox>>();

        PictureBox[,] currentBoxes = new PictureBox[4, 4];
        int[,] currentNumbers = { { 0, 0, 0, 0 }, 
                                  { 0, 0, 0, 0 },
                                  { 0, 0, 0, 0 },
                                  { 0, 0, 0, 0 } };


        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //private void KeyIsPressed(object sender, KeyPressEventArgs e)
        //{

        //    up = false;
        //    down = false;
        //    right = false;
        //    left = false;

        //    if (e.KeyChar == (char)Keys.Up)
        //        up = true;

        //    if (e.KeyChar == (char)Keys.Down)
        //        down = true;

        //    if (e.KeyChar == (char)Keys.Right)
        //        right = true;

        //    if (e.KeyChar == (char)Keys.Left)
        //        left = true;

        //}

        private void lbl_Score_Click(object sender, EventArgs e)
        {

        }

        private void gameTimer_Tick(object sender, EventArgs e) 
        {

            if(sequenceCount == 0) // vid start
            {
                sequenceCount++;

                Console.WriteLine("start");

                List<PictureBox> list1 = new List<PictureBox>();
                list1.Add(pictureBox1);
                list1.Add(pictureBox2);
                list1.Add(pictureBox3);
                list1.Add(pictureBox4);
                box_positions.Add(list1);

                List<PictureBox> list2 = new List<PictureBox>();
                list2.Add(pictureBox5);
                list2.Add(pictureBox6);
                list2.Add(pictureBox7);
                list2.Add(pictureBox8);
                box_positions.Add(list2);

                List<PictureBox> list3 = new List<PictureBox>();
                list3.Add(pictureBox9);
                list3.Add(pictureBox10);
                list3.Add(pictureBox11);
                list3.Add(pictureBox12);
                box_positions.Add(list3);

                List<PictureBox> list4 = new List<PictureBox>();
                list4.Add(pictureBox13);
                list4.Add(pictureBox14);
                list4.Add(pictureBox15);
                list4.Add(pictureBox16);
                box_positions.Add(list4);

                

                Random rnd = new Random();
                int x_index = rnd.Next(0, 3);
                int y_index = rnd.Next(0, 3);
                NewBox(x_index, y_index);

                int prev_x = x_index;
                int prev_y = y_index;

                x_index = rnd.Next(0, 3);
                y_index = rnd.Next(0, 3);

                if(x_index == prev_x && y_index == prev_y)
                {
                    while(x_index == prev_x && y_index == prev_y)
                    {
                        x_index = rnd.Next(0, 3);
                        y_index = rnd.Next(0, 3);
                    }
                }
                NewBox(x_index, y_index);

                PrintCurrentState();

            }

            if(up)
            {
                //Console.WriteLine("up");

                for(int y = 1; y < 4; y++)
                {
                    for(int x = 0; x < 4; x++)
                    {
                        if(currentNumbers[y, x] > 0)
                        {
                            int stepsUp = 0; 
                            for(int i = 0; i < y; i++) // alla platser direkt över positionen
                            {
                                if (currentNumbers[i, x] == 0)
                                    stepsUp++; 
                            }
                            if(stepsUp > 0)
                            {
                                int positionsUp = boxWidth * stepsUp + boxMargin * stepsUp;
                                currentBoxes[y, x].Location = new Point(currentBoxes[y, x].Location.X, currentBoxes[y, x].Location.Y - positionsUp);
                                currentBoxes[y - stepsUp, x] = currentBoxes[y, x];
                                currentBoxes[y, x] = null; 

                                currentNumbers[y - stepsUp, x] = currentNumbers[y, x];
                                currentNumbers[y, x] = 0;

                                Console.WriteLine("");
                                Console.WriteLine("flytta upp");
                                PrintCurrentState();

                            }
                            
                        }
                    }
                }

                for(int y = 1; y < 4; y++)
                {
                    for(int x = 0; x < 4; x++)
                    {
                        if (currentNumbers[y - 1, x] == currentNumbers[y, x] && currentNumbers[y, x] != 0)
                        {
                            int placeInArray = 0;
                            for (int i = 0; i < r.Length; i++)
                            {
                                if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                    placeInArray = i;
                            }
                            currentBoxes[y, x] = null;
                            currentNumbers[y, x] = 0;

                            currentBoxes[y - 1, x].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
                            currentNumbers[y - 1, x] *= 2;

                            if (y < 3) // flytta allt annat ner ett snäpp 
                            {
                                for (int i = y; i < 3; i++)
                                {
                                    currentNumbers[i, x] = currentNumbers[i + 1, x];
                                    currentBoxes[i, x] = currentBoxes[i + 1, x];
                                }

                                currentNumbers[3, x] = 0;
                                currentBoxes[3, x] = null;

                            }

                            Console.WriteLine("");
                            Console.WriteLine("mergea");
                            PrintCurrentState();
                        }
                    }
                }

            }
            else if(down)
            {
                Console.WriteLine("down");
            }
            else if(right)
            {
                Console.WriteLine("right");
            }
            else if(left)
            {
                Console.WriteLine("left");
            }


        }

        void NewBox(int x_index, int y_index)
        {

            PictureBox pb = new PictureBox();
            pb.Parent = panel1;
            pb.BackColor = System.Drawing.Color.FromArgb(r[0], g[0], b[0]);

            int x = startX + boxWidth * x_index + boxMargin * x_index;
            int y = startY + boxWidth * y_index + boxMargin * y_index;
            pb.SetBounds(x, y, boxWidth, boxWidth);

            pb.BringToFront();
            pb.Visible = true;

            currentNumbers[y_index, x_index] = 2;
            currentBoxes[y_index, x_index] = pb; 

            //Label lbl = new Label();
            //lbl.Parent = panel1;
            //lbl.Text = "2";
            //lbl.BringToFront();
            //lbl.Visible = true; 

            //Console.WriteLine("x_index: " + x_index);
            //Console.WriteLine("y_index: " + y_index);
            //Console.WriteLine("x: " + x);
            //Console.WriteLine("y: " + y);

        }

        
        void PrintCurrentState()
        {
            Console.WriteLine("currentNumbers:");
            Console.WriteLine(currentNumbers[0, 0] + " " + currentNumbers[0, 1] + " " + currentNumbers[0, 2] + " " + currentNumbers[0, 3]);
            Console.WriteLine(currentNumbers[1, 0] + " " + currentNumbers[1, 1] + " " + currentNumbers[1, 2] + " " + currentNumbers[1, 3]);
            Console.WriteLine(currentNumbers[2, 0] + " " + currentNumbers[2, 1] + " " + currentNumbers[2, 2] + " " + currentNumbers[2, 3]);
            Console.WriteLine(currentNumbers[3, 0] + " " + currentNumbers[3, 1] + " " + currentNumbers[3, 2] + " " + currentNumbers[3, 3]);
        }
        
        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if(e.KeyCode == Keys.Up)
            {
                up = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                down = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                right = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = true;
            }

        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {

            if(e.KeyCode == Keys.Up)
            {
                up = false; 
            }
            if (e.KeyCode == Keys.Down)
            {
                down = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                right = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = false;
            }

        }

         

    }
}
