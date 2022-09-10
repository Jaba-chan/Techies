using System;
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
        static bool Start_Game = false;
        static int net_height = 16;
        static int net_weight = 30;
        static int b_size = 20;
        int[] black_list = new int[100];
        int[] weight_point = new int[net_height * net_weight];
        int[] NET = new int[net_weight * net_height];
        static Button[] buttoms_array = new Button[net_weight * net_height];
        void Create_Weight()
        {
            for (int i = 0; i < buttoms_array.Length; i++)
                if (!black_list.Contains(i))
                {
                    buttoms_array[i].Text = weight_point[i].ToString();
                }
        }
        void Mine()
        {
            int[] right_side =  new int[net_height];
            for (int j = 0; j < net_height; j++)
                {
                    right_side[j] = net_weight - 1 + net_weight * j;
                }
            Random rnd = new Random();
            for (int i = 1; i < 100; i++)
            {
                var free_for_mine = NET.Except(black_list).ToArray();
                black_list[i] = free_for_mine[rnd.Next(0, free_for_mine.Length - 1)];
                int index_w = black_list[i];
                
                
                if (index_w % net_weight == 0 && index_w != 0 && index_w != (net_height - 1) * net_weight)
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w + net_weight + 1] += 1;
                    weight_point[index_w - net_weight - 1] += 1;
                }
                else if (right_side.Contains(index_w) && index_w != (net_weight - 1) && index_w != net_height * net_weight - 1)
                {
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w - net_weight + 1] += 1;
                    weight_point[index_w + net_weight - 1] += 1;
                }
                else if (index_w < net_weight - 1 && index_w > 0)
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w + net_weight - 1] += 1;
                    weight_point[index_w + net_weight + 1] += 1;
                }
                else if (index_w < net_weight * net_height - 1 && index_w > net_weight * (net_height - 1))
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w - net_weight - 1] += 1;
                    weight_point[index_w - net_weight + 1] += 1;
                }
                else if (index_w == 0)
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w + net_weight + 1] += 1;
                }
                else if (index_w == net_weight - 1)
                {
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w + net_weight - 1] += 1;
                }
                else if (index_w == (net_height - 1) * net_weight)
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w - net_weight - 1] += 1;
                }
                else if (index_w == (net_height * net_weight - 1))
                {
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w - net_weight + 1] += 1;
                }
                else 
                {
                    weight_point[index_w + 1] += 1;
                    weight_point[index_w - 1] += 1;
                    weight_point[index_w - net_weight] += 1;
                    weight_point[index_w + net_weight] += 1;
                    weight_point[index_w - net_weight + 1] += 1;
                    weight_point[index_w - net_weight - 1] += 1;
                    weight_point[index_w + net_weight + 1] += 1;
                    weight_point[index_w + net_weight - 1] += 1;
                    
                }
            }
            for (int i = 1; i < 100; i++)
            {
                buttoms_array[black_list[i]].BackColor = Color.Red;
            }
            Debug.WriteLine(black_list.Length);
        }
        
        void button_Clicked(object sender, EventArgs e)
        {
            Button trigered_button = (Button)sender;
            Debug.WriteLine(trigered_button.Name);
            if (Start_Game == false)
            {   
                black_list[0] =Int32.Parse(trigered_button.Name);
                Start_Game = true;
                Mine();
                Create_Weight();
            }

        }
        void Make_Button(int x, int y, int i)
        {
            Button button = new Button();
            button.Name = i.ToString();
            button.Size = new Size(b_size, b_size);
            buttoms_array[i] = button;
            button.Location = new Point(x, y);
            button.Click += new System.EventHandler(this.button_Clicked);
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
                    Make_Button(i*b_size, j*b_size, j*net_weight + i);
                    NET[j * net_weight + i] = j * net_weight + i;
                }
            }
        }
    }
}
