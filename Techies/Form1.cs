﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Techies
{
    
    public partial class Form1 : Form
    {
        static int bomb_amount = 99;
        static bool Start_Game = false;
        static int net_height = 16;
        static int net_weight = 30;
        static int b_size = 20;
        Point[] black_list = new Point[bomb_amount+1];
        Point[] NET = new Point[net_weight * net_height];
        int[,] weight_point = new int[net_weight , net_height];
        static Button[,] buttoms_array = new Button[net_weight , net_height];
        bool [,] is_cell_opened = new bool[net_weight , net_height];


        void Open_Cell(int X, int Y, int previous_weight)
        {
            if (is_cell_opened[X, Y] == false && previous_weight != 0)
            {
                buttoms_array[X, Y].BackColor = Color.FromArgb(255,255,255,255);
                is_cell_opened[X, Y] = true;
                previous_weight = weight_point[X, Y];
                return;
            }
            for (int i = - 1; i <= 1; i++)
            {
                for (int j = - 1; j <=1; j++)
                {
                    try
                    {

                        if (is_cell_opened[X + i, Y + j] == false && weight_point[X + i, Y + j] == 0)
                        {
                            is_cell_opened[X + i, Y + j] = true;
                            buttoms_array[X + i, Y + j].BackColor = Color.FromArgb(255, 255, 255, 255);
                            previous_weight = weight_point[X + i, Y + j];
                            Open_Cell(X + i, Y + j, previous_weight);
                        }
                        else if (is_cell_opened[X + i, Y + j] == false && previous_weight == 0)
                        {
                            is_cell_opened[X + i, Y + j] = true;
                            buttoms_array[X + i, Y + j].BackColor = Color.FromArgb(255, 255, 255, 255);
                            previous_weight = weight_point[X, Y];
                        }   
                    }
                    catch { }
                }
            }
        }

        void Show_Weight()
        {
            for (int i = 0; i < net_weight; i++)
            {
                for (int j = 0; j < net_height; j++)
                {
                    if (!black_list.Contains<Point>(new Point(i, j)))
                        {
                        buttoms_array[i,j].Text = weight_point[i, j].ToString();
                        }
                    else if (black_list[0].X == i && black_list[0].Y == j)
                        {
                        buttoms_array[i, j].Text = weight_point[i, j].ToString();
                        }
                }
            }
        }
        void Mine(Button trigered)
        {
            string[] first_click_str = trigered.Name.Split(' ');
            int first_click_X = Int32.Parse(first_click_str[0]);
            int first_click_Y = Int32.Parse(first_click_str[1]);
            black_list[0] = new Point(first_click_X, first_click_Y);
            Random rnd = new Random();
            for (int i = 1; i < bomb_amount + 1; i++)
            {   
                var segment = NET.Except<Point>(black_list).ToArray<Point>();
                int new_mine_location = rnd.Next(0, segment.Length);
                black_list[i] = segment[new_mine_location];
                int X = black_list[i].X;
                int Y = black_list[i].Y;
                buttoms_array[X, Y].BackColor = Color.Red;
                for (int j = - 1; j <= 1; j++)
                {for (int z = -1; z <= 1; z++)
                    {try {weight_point[X + j, Y + z] += 1;}
                        catch{}
                    }
                }

            }
           

        }
        void button_Clicked(object sender, EventArgs e)
        {
            Button trigered_button = (Button)sender;
            string[] cliked_button_str = trigered_button.Name.Split(' ');
            int X = Int32.Parse(cliked_button_str[0]);
            int Y = Int32.Parse(cliked_button_str[1]);
            if (Start_Game == false)
            {
                Mine(trigered_button);
                Show_Weight();
                Start_Game = true;
            }
            int click_weight = weight_point[X, Y]; 
            Open_Cell(X, Y, click_weight);
        }
        void Make_Button(int x, int y, string i)
        {
            Button button = new Button();
            button.Name = i.ToString();
            button.Size = new Size(b_size, b_size);
            buttoms_array[x, y] = button;
            button.Location = new Point(x*b_size, y*b_size);
            button.Click += new System.EventHandler(this.button_Clicked);
            button.BackColor = Color.FromArgb(255, 200, 200, 200);
            this.Controls.Add(button);
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(net_weight*b_size, net_height*b_size);
            for (int i = 0 ; i <net_weight;  i++)
            {
                for (int j = 0 ; j < net_height; j++)
                {
                    Make_Button(i, j, i.ToString() + " " + j.ToString());
                    is_cell_opened[i, j] = false;
                    weight_point[i, j] = 0;
                    NET[net_weight * j + i] = new Point(i, j);
                    
                }
            }
        }
    }
}
