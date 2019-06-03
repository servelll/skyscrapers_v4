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
    public partial class Form1 : Form
    {
		const int max_value = 9;
		public table main = new table(max_value);
        TextBox[,] mas;
        TextBox[,] mas_sum;
        Panel[,] mas_Panel;
        Label[, ,] mas_Label;
        int count = 0;

		public Methods meth = new Methods();
        public Examples exmpls = new Examples();
        public Form1()
        {
			CallBackMy.callbackEventHandler = new CallBackMy.callbackEvent(this.log_updating);
            CallBackMy.callbackEventHandler2 = new CallBackMy.callbackEvent2(this.block_log_updating);
            InitializeComponent();
			numericUpDown1.ValueChanged += new EventHandler(numericUpDown_ValueChanged);
			exmpls.Owner = this;
			meth.Owner = this;

			//объявление
			mas = new TextBox[max_value, max_value];
			mas_sum = new TextBox[4, max_value];
			mas_Panel = new Panel[max_value, max_value];
			mas_Label = new Label[max_value, max_value, max_value];

			//создание и рендер начальной таблицы-базы максимального размера
			tableLayoutPanel1.Visible = false;
			#region Заполнение массивов графических элементов
			//шапка
			for (int column = 0; column < max_value; column++)
			{
				mas_sum[0, column] = new TextBox();
				tableLayoutPanel1.Controls.Add(mas_sum[0, column], 1 + column, 0);
			}

			//тело
			for (int row = 0; row < max_value; row++)
			{
				//левая сумма
				mas_sum[2, row] = new TextBox();
				tableLayoutPanel1.Controls.Add(mas_sum[2, row], 0, 1 + row);

				//тело
				for (int column = 0; column < max_value; column++)
				{
					//панель
					mas_Panel[column, row] = new Panel();
					tableLayoutPanel1.Controls.Add(mas_Panel[column, row], 1 + column, 1 + row);

					//текстбоксы
					mas[column, row] = new TextBox();
					mas_Panel[column, row].Controls.Add(mas[column, row]);

					//кандидаты
					for (int k = 0; k < max_value; k++)
					{
						int i = (int)(k / 3);
						int j = k % 3;
						mas_Label[column, row, k] = new Label();
						mas_Label[column, row, k].Text = Convert.ToString(k + 1);
						mas_Label[column, row, k].Location = new Point(4 + (10 * (j)), 4 + (12 * (i)));
						mas_Panel[column, row].Controls.Add(mas_Label[column, row, k]);
					}
				}

				//правая сумма
				mas_sum[3, row] = new TextBox();
				tableLayoutPanel1.Controls.Add(mas_sum[3, row], max_value + 2, 1 + row);
			}

			//хвост
			for (int column = 0; column < max_value; column++)
			{
				mas_sum[1, column] = new TextBox();
				tableLayoutPanel1.Controls.Add(mas_sum[1, column], 1 + column, 1 + max_value);
			}
			#endregion
			creating_table(max_value, 1);
			#region Общие свойства для всех полей
			for (int i = 0; i < main.n; i++)
			{
				for (int s = 0; s < 4; s++)
				{
					TextBox ts = mas_sum[s, i];
					ts.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					ts.MaxLength = 1;
					ts.Width = 43;
					ts.Height = 45;
					ts.TextAlign = HorizontalAlignment.Center;
					ts.TextChanged += new EventHandler(sum_textBox_TextChanged);
					ts.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
					ts.Tag = Convert.ToString(s);
				}
				for (int j = 0; j < main.n; j++)
				{
					TextBox t = mas[i, j];
					t.MaxLength = 1;
					t.Width = 43;
					t.Height = 45;
					t.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					t.TextAlign = HorizontalAlignment.Center;
					t.TextChanged += new EventHandler(textBox_TextChanged);
					t.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
					t.MouseLeave += new EventHandler(textBox_Leave);
					t.Location = new System.Drawing.Point(0, 0);
					t.Visible = false;

					Panel tp = mas_Panel[i, j];
					tp.Size = new Size(43, 45);
					tp.MouseEnter += new EventHandler(panel_Enter);
					tp.MouseLeave += new EventHandler(panel_Leave);
					tp.Click += new EventHandler(panel_Click);
					tp.Location = new System.Drawing.Point(0, 0);

					for (int k = 0; k < main.n; k++)
					{
						Label tl = mas_Label[i, j, k];
						tl.DoubleClick += new EventHandler(label_Click);
						tl.MouseEnter += new EventHandler(label_Enter);
						tl.MouseLeave += new EventHandler(label_Leave);
						tl.TextAlign = ContentAlignment.MiddleCenter;
						tl.Size = new Size(13, 13);
					}
				}
			}
			#endregion
			tableLayoutPanel1.Visible = true;
        }
        private void Form1_Load(object sender, EventArgs e){}
		public void creating_table(int value, int last_value)
		{
			main.n = value;
            count = 0;
            for (int i = 0; i < meth.checkedListBox1.Items.Count; i++)
            {
                if (i >= 0 && i <= 2)
                {
                    meth.checkedListBox1.SetItemChecked(i, true);
                }
                else
                {
                    meth.checkedListBox1.SetItemChecked(i, false);
                }
            }
			//в случае не первого запуска - переделывание видимости лишних 
			//	или новых необходимых элементов

			//изменение старых объектов
			for (int i = Math.Min(value, last_value); i < Math.Max(value, last_value); i++)
			{
				//квадратик
				for (int j = Math.Min(value, last_value); j < Math.Max(value, last_value); j++)
				{
					if (value > last_value)
					{
						mas_Panel[i, j].Visible = true;
						mas[i, j].Enabled = true;
						mas_Panel[i, j].Enabled = true;
					}
					else
					{
						mas[i, j].Visible = false;
						mas_Panel[i, j].Visible = false;
						mas[i, j].Enabled = false;
						mas_Panel[i, j].Enabled = false;
					}
				}
				//прямоугольнички
				for (int j = 0; j < Math.Min(value, last_value); j++)
				{
					if (value > last_value)
					{
						mas_Panel[i, j].Visible = true;
						mas[i, j].Enabled = true;
						mas_Panel[i, j].Enabled = true;
						mas_Panel[j, i].Visible = true;
						mas[j, i].Enabled = true;
						mas_Panel[j, i].Enabled = true;
					}
					else
					{
						mas[i, j].Visible = false;
						mas_Panel[i, j].Visible = false;
						mas[i, j].Enabled = false;
						mas_Panel[i, j].Enabled = false;
						mas[j, i].Visible = false;
						mas_Panel[j, i].Visible = false;
						mas[j, i].Enabled = false;
						mas_Panel[j, i].Enabled = false;
					}
				}

				for (int j = 0; j < 4; j++)
				{
					if (value > last_value)
					{
						mas_sum[j, i].Visible = true;
						mas_sum[j, i].Enabled = true;
					}
					else
					{
						mas_sum[j, i].Visible = false;
						mas_sum[j, i].Enabled = false;

					}
				}
			}
			//для кандидатов
			for (int i = 0; i < Math.Max(value, last_value); i++)
			{
				for (int j = 0; j < Math.Max(value, last_value); j++)
				{
					for (int k = Math.Min(value, last_value); k < Math.Max(value, last_value); k++)
					{
						if (value > last_value)
						{
							mas_Label[i, j, k].Visible = true;
							mas_Label[i, j, k].Enabled = true;
						}
						else
						{
							mas_Label[i, j, k].Visible = false;
							mas_Label[i, j, k].Enabled = false;
						}
					}
				}
			}

			tableLayoutPanel1.ColumnCount = value + 2;
			tableLayoutPanel1.RowCount = value + 2;
		}

        //левое меню
        public void numericUpDown_ValueChanged(object sender, EventArgs e)
	        //проверка на потери
		{
			#region Проверка полей на возможную потерю данных перезаписью
			//if (backuped_table.n != 0)
			//{
			//	if (main.n > backuped_table.n)
			//	{
			//		MessageBox.Show("Произойдет переписывание меньшей таблицы в текущую большую");
			//	}
			//	else
			//	{
			//		bool flag_any_one_data = false;
			//		for (int i = main.n; i < backuped_table.n; i++) {
			//			for (int j = main.n; j < backuped_table.n; j++) {
			//				if (backuped_table.matrix[i, j] > 0)
			//				{
			//					flag_any_one_data = true;
			//					break;
			//				}
			//			}
			//			if (flag_any_one_data)
			//			{
			//				break;
			//			}
			//			for (int j = 0; j < 4; j++)
			//			{
			//				if (backuped_table.sum[j, i] > 0)
			//				{
			//					flag_any_one_data = true;
			//					break;
			//				}
			//			}
			//			if (flag_any_one_data)
			//			{
			//				break;
			//			}
			//		}
			//		string common = "Произойдет переписывание большей таблицы в текущую меньшую.";
			//		if (flag_any_one_data) {
			//			string s = "Внимание! Данные с краю не будут отображаться в этой таблице";
			//			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			//			DialogResult result;
			//			result = MessageBox.Show(common + "\nПродолжить?", s, buttons);
			//			if (result == DialogResult.No)
			//			{
			//				return;
			//			}
			//		} 
			//		else 
			//		{
			//			MessageBox.Show(common + "\nМогли бы быть потеряны данные, но терять нечего");
			//		}
					
			//	}
			//}
			#endregion

			//Создание новой таблицы
			tableLayoutPanel1.Visible = false;
			creating_table(Convert.ToInt32(((NumericUpDown)sender).Value), main.n);

			tableLayoutPanel1.Visible = true;
		}
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //TODO проверка условия на заполнение всех ячеек сумм и окно потверждения о дано
                //В случае с пустотами - 0
                foreach (TextBox te in mas_sum)
                {
                    te.Enabled = false;
                }
                foreach (TextBox te in mas)
                {
                    if (te.Text.Length != 0)
                    {
                        te.Enabled = false;
                    }
                }
			}
			else
			{
				foreach (TextBox te in mas_sum)
				{
					te.Enabled = true;
				}
				foreach (TextBox te in mas)
				{
					te.Enabled = true;
				}
			}
        }
        
        //обновления
        public void print_debug()
        {
            label2.Text = "";
            label2.Text += "n="; label2.Text += main.n;
            label2.Text += "\n";
            for (int j = 0; j < main.n; j++)
            {
                for (int i = 0; i < main.n; i++)
                {

                    label2.Text += Convert.ToString(main.matrix[i, j]);
                }
                label2.Text += "\n";
            }
            label2.Text += "\n";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < main.n; j++)
                {
                    label2.Text += Convert.ToString(main.sum[i, j]);
                }
                label2.Text += "\n";
            }
        }
        public void update()
        {
            numericUpDown1.Value = main.n;
            for (int i = 0; i < main.n; i++)
            {
                for (int j = 0; j < main.n; j++)
                {
                    //матрица
                    if (main.matrix[i, j] > 0)
                    {
                        mas[i, j].Visible = true;
                        mas[i, j].Text = Convert.ToString(main.matrix[i, j]);
                    }
                    else
                    {
                        mas[i, j].Visible = false;
                        mas[i, j].Text = "";
                    }
                    //кандидаты
                    for (int k = 0; k < main.n; k++)
                    {
                        if (main.candidates[i, j, k])
                        {
                            mas_Label[i, j, k].ForeColor = Control.DefaultForeColor;
                        }
                        else
                        {
                            mas_Label[i, j, k].ForeColor = Control.DefaultBackColor;
                        }
                    }
                }
            }
            //суммы
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < main.n; j++)
                {
                    if (main.sum[i, j] > 0)
                    {
                        mas_sum[i, j].Text = Convert.ToString(main.sum[i, j]);
                    }
                    else
                    {
                        mas_sum[i, j].Text = "";
                    }
                }
            }
        }
        private void update_check_status()
        {
            label3.Visible = true;
            try
            {
                main.Check();
                label3.Text = "Все нормально";
                foreach (TextBox te in mas)
                {
                    te.BackColor = Color.White;
                }
                foreach (TextBox te in mas_sum)
                {
                    te.BackColor = Color.White;
                }
            }
            catch (MyException_row_or_col e)
            {
                label3.Text = "Ошибка заполнения: " + e.Message;
                if ((int)e.dir/2 == 0)
                {
                    for (int i = 0; i < main.n; i++)
                    {
                        mas[e.number, i].BackColor = Color.Red;
                    }
                }
                else
                {
                    for (int i = 0; i < main.n; i++)
                    {
                        mas[i, e.number].BackColor = Color.Red;
                    }
                }
            }
            catch (MyException_row_or_col_and_sum e)
            {
                label3.Text = "Ошибка заполнения: " + e.Message;
                if ((int)e.dir / 2 == 0)
                {
                    for (int i = 0; i < main.n; i++)
                    {
                        mas[e.number, i].BackColor = Color.Red;
                    }
                }
                else
                {
                    for (int i = 0; i < main.n; i++)
                    {
                        mas[i, e.number].BackColor = Color.Red;
                    }
                }
                mas_sum[e.dir, e.number].BackColor = Color.Blue;
            }
            catch (MyException_cell e)
            {
                label3.Text = "Ошибка заполнения: " + e.Message;
                mas[e.i, e.j].BackColor = Color.Red;
            }
        }

		//ячейки с текстбоксами и панелями с кандидатами
        private void sum_textBox_TextChanged(object sender, EventArgs e)
        {
            TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(((TextBox)sender).Controls.Owner);

            //перевод абсолютных координат таблицы в относительные для массива сумм
            int dir = -1, number = -1;
            if (Convert.ToInt32(((TextBox)sender).Tag) == 0)
            {
                dir = 0;
                number = pos.Column - 1;
            }
            if (Convert.ToInt32(((TextBox)sender).Tag) == 1)
            {
                dir = 1;
                number = pos.Column - 1;

            }
            if (Convert.ToInt32(((TextBox)sender).Tag) == 2)
            {
                dir = 2;
                number = pos.Row - 1;
            }
            if (Convert.ToInt32(((TextBox)sender).Tag) == 3)
            {
                dir = 3;
                number = pos.Row - 1;
            }

            try
            {
                main.sum[dir, number] = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch (System.FormatException ee)
            {
                main.sum[dir, number] = 0;
            }

            update_check_status();
            print_debug();
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(((TextBox)sender).Parent);
            //проверка на значение в matrix
            //занос в матрицу и печать отладки      
            try
            {
                main.set_cell(pos.Column - 1, pos.Row - 1, Convert.ToInt32(((TextBox)sender).Text));
                update();
            }
            catch (FormatException ee)
            {
                main.matrix[pos.Column - 1, pos.Row - 1] = 0;
                ((TextBox)sender).Visible = false;
            }

            update_check_status();
            print_debug();
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
        private void textBox_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Visible = false;
            }

        }
        private void label_Click(object sender, EventArgs e)
        {
			TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(((Label)sender).Parent);
			if (((Label)sender).ForeColor == Control.DefaultForeColor)
            {
				main.candidates[pos.Column - 1, pos.Row - 1, Convert.ToInt32(((Label)sender).Text) - 1] = false;
				((Label)sender).ForeColor = Control.DefaultBackColor;
            }
            else
            {
				main.candidates[pos.Column - 1, pos.Row - 1, Convert.ToInt32(((Label)sender).Text) - 1] = true;
				((Label)sender).ForeColor = Control.DefaultForeColor;
            }

        }
        private void label_Enter(object sender, EventArgs e)
        {
			((Label)sender).Parent.BackColor = Color.Yellow;
			foreach (Label te in ((Label)sender).Parent.Controls.OfType<Label>())
            {
                te.BackColor = Color.Yellow;
            }
			((Label)sender).BackColor = Color.Coral;
        }
        private void label_Leave(object sender, EventArgs e)
        {
			((Label)sender).BackColor = Control.DefaultBackColor;
			foreach (Label te in ((Label)sender).Parent.Controls.OfType<Label>())
            {
                te.BackColor = Control.DefaultBackColor;
            }
			((Label)sender).Parent.BackColor = Control.DefaultBackColor;
        }
        private void panel_Enter(object sender, EventArgs e)
        {
			((Panel)sender).BackColor = Color.Yellow;
			foreach (Label te in ((Panel)sender).Controls.OfType<Label>())
            {
                te.BackColor = Color.Yellow;
            }
        }
        private void panel_Leave(object sender, EventArgs e)
        {
			((Panel)sender).BackColor = Control.DefaultBackColor;
			foreach (Label te in ((Panel)sender).Controls.OfType<Label>())
            {
                te.BackColor = Control.DefaultBackColor;
            }
        }
        private void panel_Click(object sender, EventArgs e)
        {
			TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition((Panel)sender);
			//проверка на моментальное заполнение единиц
			int last_value = -1;
			int count = 0;
			for (int i = 0; i < main.n; i++)
			{
				if (main.candidates[pos.Column - 1, pos.Row - 1, i] == true)
				{
					count++;
					if (count > 1)
					{
						last_value = -1;
						break;
					}
					last_value = i + 1;
				}
			}
			tableLayoutPanel1.GetControlFromPosition(pos.Column, pos.Row).Controls.OfType<TextBox>().First().Visible = true;
			if (last_value > -1 && count == 1)
			{
				tableLayoutPanel1.GetControlFromPosition(pos.Column, pos.Row).Controls.OfType<TextBox>().First().Text = Convert.ToString(last_value);
				main.matrix[pos.Column, pos.Row] = last_value;
			}
			else
			{
				tableLayoutPanel1.GetControlFromPosition(pos.Column, pos.Row).Controls.OfType<TextBox>().First().Focus();
			}
        }

		//Кнопки
		private void button1_Click(object sender, EventArgs e)
		{
			exmpls.ShowDialog(this);
		}
		private void button2_Click(object sender, EventArgs e)
		{
			tableLayoutPanel1.Visible = false;
			main.clear();
			main.clear_sum();
			checkBox1.Checked = false;
			checkBox1.Enabled = true;
			numericUpDown1.Enabled = true;
			update();
			tableLayoutPanel1.Visible = true;
		}
		private void button3_Click(object sender, EventArgs e)
		{
			meth.ShowDialog(this);
			meth.checkedListBox1.Focus();
		}
		private void button4_Click(object sender, EventArgs e)
		{
            set_blocked_state(false);
			richTextBox1.Text = DateTime.Now.ToString() + "\n";
			CallBackMy.callbackEventHandler("Начало выполнения методов\n");
			for (int i = 0; i < meth.checkedListBox1.Items.Count; i++)
			{
                if (count == 2 && i >= 3)
                {
                    CallBackMy.callbackEventHandler("Разблокируем метод №" + i + "\n");
                    meth.checkedListBox1.SetItemChecked(i, true);
                }
                if (meth.checkedListBox1.GetItemChecked(i))
				{
					CallBackMy.callbackEventHandler("Выполняем метод №" + i + "\n");
					update();
					main.do_method(i);
				}
                if (count == 0 && (i == 0 || i == 2))
                {
                    CallBackMy.callbackEventHandler("Блокируем для выполнения метод №" + i + "\n");
                    meth.checkedListBox1.SetItemChecked(i, false);
                }
			}
			CallBackMy.callbackEventHandler("Конец выполнения методов\n");
			update();
			print_debug();
            count++;
            ((Button)sender).Text = "Го " + count;
            set_blocked_state(true);
		}

		//Обработка логов
		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
		private void log_updating(string param)
		{
			richTextBox1.Text += param;
		}
        private void block_log_updating(string param, bool clear=false)
        {
            if (clear) richTextBox2.Text = param; 
            else richTextBox2.Text += param;
        }

		
        //Глобальная блокировка несвоевременных действий
        private void set_blocked_state(bool state)
        {
            button1.Enabled = state;
            button2.Enabled = state;
            button3.Enabled = state;
            button4.Enabled = state;
        }

    }
}
