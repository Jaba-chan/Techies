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
        static int indents = 20;
        static int top_border = 60;
        static int face_size_x = 60;
        static int face_size_y = Convert.ToInt32(face_size_x * 1);
        static int marked_cell = 0;
        Image face_u = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/face_unpress.png");
        Image face_d = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/face_press.png");
        Image face = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/face_crige1.png");
        Image cell = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/cell.png");
        Image m_r = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/M_R.png");
        Image m_g = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/M_G.png");
        Image flag_image = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/flag.png");
        Image[] Pictures_array = new Image[9];
        static int bomb_amount = 2;
        static bool Start_Game = false;
        static int net_height = 4;
        static int net_weight = 4;
        static int b_size = 30;
        Point[] black_list = new Point[bomb_amount+1];
        Point[] NET = new Point[net_weight * net_height];
        int[,] weight_point = new int[net_weight , net_height];
        Button[,] buttoms_array = new Button[net_weight , net_height];
        bool [,] is_cell_opened = new bool[net_weight , net_height];
        bool[,] flags = new bool[net_weight , net_height];
        void Restart_Game(object sender, MouseEventArgs e)
        {
            Application.Restart();
        }

        void Restart_Buttons(int size_x, int size_y)
        {
            Button button = new Button();
            button.Size = new Size(size_x, size_y);
            button.Location = new Point((net_weight*b_size + indents* 2 - size_x) / 2, indents/2);
            button.FlatStyle = FlatStyle.Flat;
            button.BackgroundImageLayout = ImageLayout.Stretch;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button.MouseClick += Restart_Game;
            button.BackgroundImage = face_u;
            this.Controls.Add(button);
        }
        void Lose(Button mine)
        {
            for (int i = 1; i <= bomb_amount; i++)
            {
                int x_mime = black_list[i].X;
                int y_mine = black_list[i].Y;
                buttoms_array[x_mime, y_mine].BackgroundImage = m_g; 
            }
            mine.BackgroundImage = m_r;
            MessageBox.Show("You Lose!");
        }
        void Win()
        {
            MessageBox.Show("You Win!");
            Debug.WriteLine(marked_cell);
        }
        void Open_Cell(int X, int Y, int previous_weight)
        {
            if (weight_point[X, Y] < 0)
            { Lose(buttoms_array[X, Y]); return; }
            else
            {
                if (is_cell_opened[X, Y] == false && previous_weight != 0)
                {
                    buttoms_array[X, Y].BackgroundImage = Pictures_array[weight_point[X, Y]];
                    buttoms_array[X, Y].Enabled = false;
                    is_cell_opened[X, Y] = true;
                    previous_weight = weight_point[X, Y];
                    marked_cell += 1;
                    Debug.WriteLine(marked_cell);
                    return;
                }
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
                            buttoms_array[X + i, Y + j].BackgroundImage = Pictures_array[weight_point[X + i, Y + j]];
                            previous_weight = weight_point[X + i, Y + j];
                            Open_Cell(X + i, Y + j, previous_weight);
                            buttoms_array[X + i, Y + j].Enabled = false;
                            marked_cell += 1;
                            Debug.WriteLine(marked_cell);
                        }
                        else if (is_cell_opened[X + i, Y + j] == false && previous_weight == 0)
                        {
                            is_cell_opened[X + i, Y + j] = true;
                            buttoms_array[X + i, Y + j].BackgroundImage = Pictures_array[weight_point[X + i, Y + j]];
                            previous_weight = weight_point[X, Y];
                            buttoms_array[X +i, Y + j].Enabled = false;
                            marked_cell += 1;
                            Debug.WriteLine(marked_cell);
                        }   
                    }
                    catch { }
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
                weight_point[X, Y] = -100;
                for (int j = - 1; j <= 1; j++)
                {for (int z = -1; z <= 1; z++)
                    {try {weight_point[X + j, Y + z] += 1;}
                        catch{}
                    }
                }
            }
        }

        void button_Clicked(object sender, MouseEventArgs e)
        {
            Button trigered_button = (Button)sender; 
            string[] cliked_button_str = trigered_button.Name.Split(' ');
            int X = Int32.Parse(cliked_button_str[0]);
            int Y = Int32.Parse(cliked_button_str[1]);

            if (e.Button == MouseButtons.Right)
            {
                if (flags[X, Y] == false) {trigered_button.BackgroundImage = flag_image;}
                if (flags[X, Y] == true) { trigered_button.BackgroundImage = cell;}
                flags[X, Y] = !flags[X, Y];

                return;
            }

            if (Start_Game == false)
            {
                Mine(trigered_button);
                Start_Game = true;
            }
            if (flags[X, Y] != true)
            {
                int click_weight = weight_point[X, Y];
                Open_Cell(X, Y, click_weight);
                trigered_button.Enabled = false;
            }
            if (marked_cell == (net_height * net_weight) - (bomb_amount))
            {
                Win();
            }
        }
        void Make_Button(int x, int y, string i)
        {
            Button button = new Button();
            button.Name = i.ToString();
            button.BackgroundImage = cell;
            button.BackgroundImageLayout = ImageLayout.Stretch;
            button.Size = new Size(b_size, b_size);
            buttoms_array[x, y] = button;
            button.Location = new Point(x*b_size + indents, y*b_size + indents + top_border);
            button.MouseDown += button_Clicked;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(255, 150, 150, 150);
            this.Controls.Add(button);
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int m_n = 0; m_n <= 8; m_n++) 
            {Pictures_array[m_n] = Image.FromFile("C:/Users/kuzak/source/repos/Techies/Techies/Properties/"+ m_n.ToString()+ "_m.png");}
            this.ClientSize = new Size(net_weight*b_size + indents*2, net_height*b_size + indents*2 + top_border);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 249, 249, 249);
            this.MaximizeBox = false;
            for (int i = 0 ; i <net_weight;  i++)
            {
                for (int j = 0 ; j < net_height; j++)
                {
                    flags[i, j] = false;
                    Restart_Buttons(face_size_x, face_size_y);
                    Make_Button(i, j, i.ToString() + " " + j.ToString());
                    is_cell_opened[i, j] = false;
                    weight_point[i, j] = 0;
                    NET[net_weight * j + i] = new Point(i, j);
                }
            }
        }
    }
}
