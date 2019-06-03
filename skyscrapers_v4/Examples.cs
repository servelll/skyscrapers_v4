using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skyscrapers_v4
{
    public partial class Examples : Form
    {
        public Examples()
        {
            InitializeComponent();
        }
        private void fill(input input)
        {
			Form c = this.Owner;
			{
				if (c is Form1)
				{
					((Form1)c).tableLayoutPanel1.Visible = false;
					((Form1)c).checkBox1.Checked = false;
					((Form1)c).creating_table(input.size, ((Form1)c).main.n);
					((Form1)c).main.fill(input);
					((Form1)c).update();
					((Form1)c).print_debug();
					((Form1)c).checkBox1.Checked = true;
					((Form1)c).checkBox1.Enabled = false;
					((Form1)c).numericUpDown1.Enabled = false;
					((Form1)c).tableLayoutPanel1.Visible = true;
				}
			}
        }
		private void button1_Click(object sender, EventArgs e)
		{
			int size = 5;
			//решается 3x ариадной
			int[] left_col = { 2, 2, 4, 1, 3 };
			int[] right_col = { 2, 2, 2, 4, 1 };
			int[] top_row = { 3, 1, 2, 2, 2 };
			int[] bottom_row = { 2, 3, 4, 2, 1 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button2_Click(object sender, EventArgs e)
		{
			int size = 5;
			//решается без ариадны
			int[] left_col = { 1, 2, 2, 3, 3 };
			int[] right_col = { 3, 2, 3, 3, 1 };
			int[] top_row = { 1, 2, 2, 2, 3 };
			int[] bottom_row = { 4, 2, 2, 3, 1 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button3_Click(object sender, EventArgs e)
		{
			int size = 5;
			//решается без min_stairs и без ариадны
			int[] left_col = { 4, 3, 2, 1, 2 };
			int[] right_col = { 1, 3, 2, 3, 2 };
			int[] top_row = { 4, 4, 2, 3, 1 };
			int[] bottom_row = { 2, 1, 4, 2, 2 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button4_Click(object sender, EventArgs e)
		{
			int size = 6;
			//решается полностью реализованной ариадной
			int[] left_col = { 3, 1, 2, 2, 5, 3 };
			int[] right_col = { 1, 3, 2, 4, 2, 3 };
			int[] top_row = { 2, 2, 3, 3, 3, 1 };
			int[] bottom_row = { 4, 2, 4, 1, 2, 3 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button5_Click(object sender, EventArgs e)
		{
			int size = 6;
			//решается множественной ариадной до 2го уровня
			int[] left_col = { 1, 4, 2, 3, 2, 2 };
			int[] right_col = { 5, 1, 4, 2, 2, 2 };
			int[] top_row = { 1, 2, 3, 4, 3, 2 };
			int[] bottom_row = { 2, 4, 1, 2, 3, 3 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button6_Click(object sender, EventArgs e)
		{
			int size = 7;
			
			int[] left_col = { 5, 2, 3, 3, 3, 1, 2 };
			int[] right_col = { 1, 2, 2, 3, 3, 4, 3 };
			int[] top_row = { 4, 3, 2, 4, 2, 2, 1 };
			int[] bottom_row = { 2, 1, 4, 2, 3, 2, 4 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button7_Click(object sender, EventArgs e)
		{
			int size = 5;
			//первый пример с неполными данными
			int[] left_col = { 3, 2, 3, 0, 2 };
			int[] right_col = { 1, 3, 0, 3, 2 };
			int[] top_row = { 0, 2, 3, 4, 1 };
			int[] bottom_row = { 2, 0, 3, 0, 0 };

			fill(new input(size, left_col, right_col, top_row, bottom_row));
		}
		private void button8_Click(object sender, EventArgs e)
		{
			int size = 7;
			//второй мощный пример с неполными данными и с начальными значениями
			int[] left_col = { 1, 3, 0, 4, 3, 0, 2 };
			int[] right_col = { 3, 2, 3, 2, 2, 1, 3 };
			int[] top_row = { 1, 0, 2, 3, 4, 2, 4 };
			int[] bottom_row = { 3, 0, 0, 4, 2, 3, 2 };
			input inp_obj = new input(size, left_col, right_col, top_row, bottom_row);

			inp_obj.add_started_cell(0, 1, 1);
			inp_obj.add_started_cell(0, 3, 3);
			inp_obj.add_started_cell(2, 1, 3);
			inp_obj.add_started_cell(2, 3, 7);
			inp_obj.add_started_cell(3, 0, 2);
			inp_obj.add_started_cell(5, 1, 5);
			inp_obj.add_started_cell(5, 5, 4);
			inp_obj.add_started_cell(6, 5, 1);

			fill(inp_obj);
		}
    }
}
