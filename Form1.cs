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

        /*
        readonly int r2 = 238, g2 = 228, b2 = 218;
        readonly int r4 = 238, g4 = 225, b4 = 201; 
        readonly int r8 = 243, g8 = 178, b8 = 122; 
        */

        //r[0] = r2
        // 2-8192
        readonly int[] r = { 238, 238, 243, 246, 247, 247, 237, 237, 237, 237, 237, 60, 60 }; 
        readonly int[] g = { 228, 225, 178, 150, 124, 95, 208, 204, 200, 197, 194, 58, 58 };
        readonly int[] b = { 218, 201, 122, 100, 95, 59, 115, 98, 80, 63, 46, 50, 50 };

        bool up = false, down = false, right = false, left = false;
        bool prevUp = false, prevDown = false, prevRight = false, prevLeft = false;
        int sequenceCount = 0;

        List<List<PictureBox>> box_positions = new List<List<PictureBox>>();

        PictureBox[,] currentBoxes = new PictureBox[4, 4];
        int[,] currentNumbers = { { 0, 0, 0, 0 }, 
                                  { 0, 0, 0, 0 },
                                  { 0, 0, 0, 0 },
                                  { 0, 0, 0, 0 } };
        Label[,] numbers = new Label[4, 4];


        public Form1()
        {
            InitializeComponent();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
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

            if (e.KeyCode == Keys.Up)
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

       
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // this function is taken from the following link
        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.previewkeydown?view=windowsdesktop-6.0
        private void btn_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Right:
                case Keys.Left: 
                    e.IsInputKey = true;
                    break;
            }
        }

        private void label3_Click(object sender, EventArgs e)
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

            bool newPress = false; 

            if(sequenceCount == 0) // vid start
            {

                btn_NewGame.Focus();

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

                InstantiateBoxes();

                Random rnd = new Random();
                int x_index = rnd.Next(0, 3);
                int y_index = rnd.Next(0, 3);
                NewBox(y_index, x_index);

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
                NewBox(y_index, x_index);

                PrintCurrentState();

            }

            if(up && !prevUp)
            {
                //Console.WriteLine("up");

                newPress = true; 

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

                                int placeInArray = 0; 
                                for (int i = 0; i < r.Length; i++)
                                {
                                    if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                        placeInArray = i;
                                }

                                currentBoxes[y - stepsUp, x].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]); 
                                currentBoxes[y - stepsUp, x].Visible = true; 
                                currentBoxes[y, x].Visible = false; 

                                int n = currentNumbers[y, x];
                                currentNumbers[y - stepsUp, x] = n;
                                currentNumbers[y, x] = 0;
                                
                                Console.WriteLine("");
                                Console.WriteLine("flytta upp");
                                //PrintCurrentState();

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
                                if ((int)Math.Pow(2, i + 1) == currentNumbers[y, x])
                                    placeInArray = i + 1;
                            }

                            currentBoxes[y, x].Visible = false;
                            currentBoxes[y - 1, x].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
                            currentBoxes[y - 1, x].Visible = true; 
                            
                            int n = currentNumbers[y, x];
                            currentNumbers[y - 1, x] = n * 2;
                            currentNumbers[y, x] = 0;

                            UpdateNumbers();
                            
                            if (y < 3) // flytta allt annat upp ett snäpp 
                            {
                                for (int i = y; i < 3; i++)
                                {
                                    currentNumbers[i, x] = currentNumbers[i + 1, x];
                                    currentBoxes[i, x].Visible = currentBoxes[i + 1, x].Visible;
                                    currentBoxes[i, x].BackColor = currentBoxes[i + 1, x].BackColor;
                                }

                                currentNumbers[3, x] = 0;
                                currentBoxes[3, x].Visible = false;

                            } 
                            
                            try
                            {
                                int score = int.Parse(lbl_Score.Text);
                                score += currentNumbers[y - 1, x];
                                lbl_Score.Text = score + "";

                                int bestScore = int.Parse(lbl_BestScore.Text);
                                if(score > bestScore)
                                {
                                    lbl_BestScore.Text = score + "";
                                }
                            } catch(NullReferenceException) { Console.WriteLine("NullReferenceException int score"); }

                            Console.WriteLine("");
                            Console.WriteLine("mergea");
                            //PrintCurrentState();
                        }
                    }
                }
                
            }
            else if(down && !prevDown)
            {
                //Console.WriteLine("down");

                newPress = true; 

                for(int y = 2; y >= 0; y--)
                {
                    for(int x = 0; x < 4; x++)
                    {
                        if(currentNumbers[y, x] > 0)
                        {
                            int stepsDown = 0; 
                            for(int i = y + 1; i < 4; i++) // alla platser direkt under positionen
                            {
                                if (currentNumbers[i, x] == 0)
                                    stepsDown++; 
                            }
                            if(stepsDown > 0)
                            {

                                int placeInArray = 0; 
                                for (int i = 0; i < r.Length; i++)
                                {
                                    if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                        placeInArray = i;
                                }

                                currentBoxes[y + stepsDown, x].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]); 
                                currentBoxes[y + stepsDown, x].Visible = true; 
                                currentBoxes[y, x].Visible = false; 

                                int n = currentNumbers[y, x];
                                currentNumbers[y + stepsDown, x] = n;
                                currentNumbers[y, x] = 0;

                                Console.WriteLine("");
                                Console.WriteLine("flytta ned");
                                //PrintCurrentState();

                            }
                            
                        }
                    }
                }
                
                for(int y = 2; y >= 0; y--)
                {
                    for(int x = 0; x < 4; x++)
                    {
                        if (currentNumbers[y + 1, x] == currentNumbers[y, x] && currentNumbers[y, x] != 0)
                        {
                            int placeInArray = 0;
                            for (int i = 0; i < r.Length; i++)
                            {
                                if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                    placeInArray = i + 1;
                            }

                            currentBoxes[y, x].Visible = false;
                            currentBoxes[y + 1, x].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
                            currentBoxes[y + 1, x].Visible = true;

                            int n = currentNumbers[y, x]; 
                            currentNumbers[y + 1, x] = n * 2;
                            currentNumbers[y, x] = 0;

                            if (y > 1) // flytta allt annat ned ett snäpp 
                            {
                                for (int i = 1; i < y; i++)
                                {
                                    currentNumbers[i, x] = currentNumbers[i - 1, x];
                                    currentBoxes[i, x] = currentBoxes[i - 1, x];
                                }

                                currentNumbers[0, x] = 0;
                                currentBoxes[0, x].Visible = false;

                            } 

                            try
                            {
                                int score = int.Parse(lbl_Score.Text);
                                score += currentNumbers[y + 1, x];
                                lbl_Score.Text = score + "";

                                int bestScore = int.Parse(lbl_BestScore.Text);
                                if (score > bestScore)
                                {
                                    lbl_BestScore.Text = score + "";
                                }
                            } catch(NullReferenceException) { Console.WriteLine("NullReferenceException int score"); }

                            Console.WriteLine("");
                            Console.WriteLine("mergea");
                            PrintCurrentState();
                        }
                    }
                } 
                
            }
            else if(right && !prevRight)
            {
                //Console.WriteLine("right");

                newPress = true; 

                for(int y = 0; y < 4; y++)
                {
                    for(int x = 2; x >= 0; x--)
                    {
                        if(currentNumbers[y, x] > 0)
                        {
                            int stepsRight = 0; 
                            for(int i = x + 1; i < 4; i++) // alla platser direkt höger om positionen
                            {
                                if (currentNumbers[y, i] == 0)
                                    stepsRight++; 
                            }
                            if(stepsRight > 0)
                            {

                                int placeInArray = 0; 
                                for (int i = 0; i < r.Length; i++)
                                {
                                    if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                        placeInArray = i;
                                }

                                currentBoxes[y, x + stepsRight].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]); 
                                currentBoxes[y, x + stepsRight].Visible = true; 
                                currentBoxes[y, x].Visible = false; 

                                int n = currentNumbers[y, x];
                                currentNumbers[y, x + stepsRight] = n;
                                currentNumbers[y, x] = 0;

                                Console.WriteLine("");
                                Console.WriteLine("flytta till höger");
                                //PrintCurrentState();

                            }
                            
                        }
                    }
                }
                
                for(int y = 0; y < 4; y++)
                {
                    for(int x = 2; x >= 0; x--)
                    {
                        if (currentNumbers[y, x + 1] == currentNumbers[y, x] && currentNumbers[y, x] != 0)
                        {
                            int placeInArray = 0;
                            for (int i = 0; i < r.Length; i++)
                            {
                                if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                    placeInArray = i + 1;
                            }

                            currentBoxes[y, x].Visible = false;
                            currentBoxes[y, x + 1].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
                            currentBoxes[y, x + 1].Visible = true;

                            int n = currentNumbers[y, x];
                            currentNumbers[y, x + 1] = n * 2;
                            currentNumbers[y, x] = 0;

                            if (x > 1) // flytta allt annat ett snäpp till höger  
                            {
                                for (int i = 1; i < x; i++)
                                {
                                    currentNumbers[y, i] = currentNumbers[y, i - 1];
                                    currentBoxes[y, i] = currentBoxes[y, i - 1];
                                }

                                currentNumbers[y, 0] = 0;
                                currentBoxes[y, 0].Visible = false;

                            } 

                            try
                            {
                                int score = int.Parse(lbl_Score.Text);
                                score += currentNumbers[y, x + 1];
                                lbl_Score.Text = score + "";

                                int bestScore = int.Parse(lbl_BestScore.Text);
                                if (score > bestScore)
                                {
                                    lbl_BestScore.Text = score + "";
                                }
                            } catch(NullReferenceException) { Console.WriteLine("NullReferenceException int score"); }

                            Console.WriteLine("");
                            Console.WriteLine("mergea");
                            PrintCurrentState();
                        }
                    }
                } 
                

            }
            else if(left && !prevLeft)
            {
                //Console.WriteLine("left");

                newPress = true; 

                for(int y = 0; y < 4; y++)
                {
                    for(int x = 1; x < 4; x++)
                    {
                        if(currentNumbers[y, x] > 0)
                        {
                            int stepsLeft = 0; 
                            for(int i = 0; i < x; i++) // alla platser direkt vänster om positionen
                            {
                                if (currentNumbers[y, i] == 0)
                                    stepsLeft++; 
                            }
                            if(stepsLeft > 0)
                            {

                                int placeInArray = 0; 
                                for (int i = 0; i < r.Length; i++)
                                {
                                    if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                        placeInArray = i;
                                }

                                currentBoxes[y, x - stepsLeft].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]); 
                                currentBoxes[y, x - stepsLeft].Visible = true; 
                                currentBoxes[y, x].Visible = false; 

                                int n = currentNumbers[y, x];
                                currentNumbers[y, x - stepsLeft] = n;
                                currentNumbers[y, x] = 0;

                                Console.WriteLine("");
                                Console.WriteLine("flytta till vänster");
                                //PrintCurrentState();

                            }
                            
                        }
                    }
                    
                }
                
                for(int y = 0; y < 4; y++)
                {
                    for(int x = 1; x < 4; x++)
                    {
                        if (currentNumbers[y, x - 1] == currentNumbers[y, x] && currentNumbers[y, x] != 0)
                        {
                            int placeInArray = 0;
                            for (int i = 0; i < r.Length; i++)
                            {
                                if (Math.Pow(2, i + 1) == currentNumbers[y, x])
                                    placeInArray = i + 1;
                            }

                            currentBoxes[y, x].Visible = false;
                            currentBoxes[y, x - 1].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
                            currentBoxes[y, x - 1].Visible = true;

                            int n = currentNumbers[y, x];
                            currentNumbers[y, x - 1] = n * 2;
                            currentNumbers[y, x] = 0;

                            if (x < 3) // flytta allt annat ett snäpp till vänster  
                            {
                                for (int i = x; i < 3; i++)
                                {
                                    currentNumbers[y, i] = currentNumbers[y, i + 1];
                                    currentBoxes[y, i] = currentBoxes[y, i + 1];
                                }

                                currentNumbers[y, 3] = 0;
                                currentBoxes[y, 3].Visible = false;

                            } 

                            try
                            {
                                int score = int.Parse(lbl_Score.Text);
                                score += currentNumbers[y, x - 1];
                                lbl_Score.Text = score + "";

                                int bestScore = int.Parse(lbl_BestScore.Text);
                                if (score > bestScore)
                                {
                                    lbl_BestScore.Text = score + "";
                                }
                            } catch(NullReferenceException) { Console.WriteLine("NullReferenceException int score"); }

                            Console.WriteLine("");
                            Console.WriteLine("mergea");
                            PrintCurrentState();
                        }
                    }
                } 
                
                
            }
            
            if(newPress) //spawna ny box vid varje move 
            {
                UpdateNumbers();
                PrintCurrentState();

                bool boardHasEmptySpot = false; 
                for(int y = 0; y < 4; y++)
                {
                    for(int x = 0; x < 4; x++)
                    {
                        if(currentNumbers[y, x] == 0)
                        {
                            boardHasEmptySpot = true; 
                            break; 
                        }
                    }
                }
                
                if(boardHasEmptySpot)
                {

                    int y_index = 0, x_index = 0; 

                    Random rnd = new Random();
                    y_index = rnd.Next(0, 4);
                    x_index = rnd.Next(0, 4);

                    int k = 0;
                    int safetyMargin = 10000000; //in case the while-loop never stops 

                    if (currentBoxes[y_index, x_index].Visible)
                    {
                        
                        while(currentBoxes[y_index, x_index].Visible && k < safetyMargin)
                        {
                            y_index = rnd.Next(0, 4);
                            x_index = rnd.Next(0, 4);
                            k++; 
                        }

                        Console.WriteLine("k: " + k);
                        
                    }

                    if(k < safetyMargin)
                    {
                        Console.WriteLine("y: " + y_index + ", x: " + x_index);
                        NewBox(y_index, x_index);
                    }

                }

                Console.WriteLine("");
                PrintCurrentState();

            }

            prevUp = up; 
            prevDown = down; 
            prevRight = right; 
            prevLeft = left; 

        }

        void InstantiateBoxes()
        {

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Parent = panel1;
                    pb.BackColor = System.Drawing.Color.FromArgb(r[0], g[0], b[0]);

                    int x = startX + boxWidth * j + boxMargin * j;
                    int y = startY + boxWidth * i + boxMargin * i;
                    pb.SetBounds(x, y, boxWidth, boxWidth);

                    pb.BringToFront();
                    pb.Visible = false;

                    currentBoxes[i, j] = pb; 

                    int margin = 22; 
                    Label lbl = new Label();
                    lbl.Text = "2";
                    lbl.Parent = pb;
                    lbl.SetBounds(boxWidth/2 - margin, boxWidth/2 - margin, 50, 50);
                    lbl.ForeColor = label1.ForeColor; 
                    lbl.Font = new Font("Arial", 30, FontStyle.Bold); 
                    lbl.Visible = false; 

                    numbers[i, j] = lbl; 
                }
            }

        }

        void NewBox(int y_index, int x_index)
        {
            Console.WriteLine("y: " + y_index + ", x: " + x_index);
            /*
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
            */

            int placeInArray = 0; 

            Random rnd = new Random();
            float randomFloat = (float)rnd.NextDouble();

            if(randomFloat <= 0.1f) 
                placeInArray = 1;
            Console.WriteLine("placeInArray: " + placeInArray);
            currentBoxes[y_index, x_index].BackColor = System.Drawing.Color.FromArgb(r[placeInArray], g[placeInArray], b[placeInArray]);
            currentNumbers[y_index, x_index] = (int)Math.Pow(2, placeInArray + 1);
            currentBoxes[y_index, x_index].Visible = true; 

            numbers[y_index, x_index].Text = (int)Math.Pow(2, placeInArray + 1) + "";
            numbers[y_index, x_index].Visible = true; 

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

        void UpdateNumbers()
        {

            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 4; x++)
                {
                    numbers[y, x].Text = currentNumbers[y, x] + "";

                    if(currentNumbers[y, x] >= 8)
                    {
                        numbers[y, x].ForeColor = System.Drawing.Color.FromArgb(r[0], g[0], b[0]);
                    }
                    else
                    {
                        numbers[y, x].ForeColor = label1.ForeColor; 
                    }

                    if(currentNumbers[y, x] >= 10)
                    {
                        numbers[y, x].Font = new Font("Arial", 25, FontStyle.Bold);
                    }
                    else
                    {
                        numbers[y, x].Font = new Font("Arial", 30, FontStyle.Bold);
                    }

                    numbers[y, x].Visible = currentBoxes[y, x].Visible; 
                }
            }

        }
        
        void PrintCurrentState()
        {
            Console.WriteLine("currentNumbers:");
            Console.WriteLine(currentNumbers[0, 0] + " " + currentNumbers[0, 1] + " " + currentNumbers[0, 2] + " " + currentNumbers[0, 3]);
            Console.WriteLine(currentNumbers[1, 0] + " " + currentNumbers[1, 1] + " " + currentNumbers[1, 2] + " " + currentNumbers[1, 3]);
            Console.WriteLine(currentNumbers[2, 0] + " " + currentNumbers[2, 1] + " " + currentNumbers[2, 2] + " " + currentNumbers[2, 3]);
            Console.WriteLine(currentNumbers[3, 0] + " " + currentNumbers[3, 1] + " " + currentNumbers[3, 2] + " " + currentNumbers[3, 3]);
        }
        
        
        private void btn_NewGame_Click(object sender, EventArgs e)
        {
            Console.WriteLine("New Game");
            ResetGame();

        }

        void ResetGame()
        {
            for(int y = 0; y < 4; y++)
            {
                for(int x = 0; x < 4; x++)
                {
                    currentBoxes[y, x].Visible = false;
                    currentNumbers[y, x] = 0;
                    numbers[y, x].Visible = false; 
                }
            }

            lbl_Score.Text = "0";

            int y_index = 0, x_index = 0; 
            Random rnd = new Random();
            y_index = rnd.Next(0, 4);
            x_index = rnd.Next(0, 4);

            NewBox(y_index, x_index);

            int prev_x = x_index;
            int prev_y = y_index;

            x_index = rnd.Next(0, 3);
            y_index = rnd.Next(0, 3);

            if (x_index == prev_x && y_index == prev_y)
            {
                while (x_index == prev_x && y_index == prev_y)
                {
                    x_index = rnd.Next(0, 3);
                    y_index = rnd.Next(0, 3);
                }
            }
            NewBox(y_index, x_index);

            PrintCurrentState();

        }

    }
}
