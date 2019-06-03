using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skyscrapers_v4
{
    public class MyException_cell : Exception
    {
        public int i;
        public int j;
        public MyException_cell(string message) : base(message) { }
        public MyException_cell(string message, int i, int j) : base(message) { this.i = i; this.j = j; }
    }
    public class MyException_row_or_col : Exception
    {
        public int dir;
        public int number = 0;
        public MyException_row_or_col(string message) : base(message) { }
        public MyException_row_or_col(string message, int i, int dir) : base(message) { this.number = i; this.dir = dir; }
    }
    public class MyException_row_or_col_and_sum : Exception 
    { 
        public int dir;
        public int number = 0;
        public MyException_row_or_col_and_sum(string message) : base(message) { }
        public MyException_row_or_col_and_sum(string message, int i, int dir) : base(message) { this.number = i; this.dir = dir; }
    }
	public class table
	{
		bool g_d = true; //global_debug
		public int n;
		public int[,] matrix;
		public int[,] sum;
		public bool[, ,] candidates;
		public int entry_level;
        ArrayList blocks_table;

		//служебное
		public table(int size)
		{
			this.entry_level = 0;
			this.n = size;
			this.matrix = new int[n, n];
			this.sum = new int[4, n];
			this.candidates = new bool[n, n, n];
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					for (int k = 0; k < n; k++)
					{
						candidates[i, j, k] = true;
					}
				}
			}
            blocks_table = new ArrayList();
		}
		public void fill(input inp)
		{
			clear();
			this.n = inp.size;
			for (int k = 0; k < matrix.GetLength(1); k++)
			{
				if (k >= n)
				{
					sum[0, k] = 0;
					sum[1, k] = 0;
					sum[2, k] = 0;
					sum[3, k] = 0;
				}
				else
				{
					sum[0, k] = inp.top_row[k];
					sum[1, k] = inp.bottom_row[k];
					sum[2, k] = inp.left_col[k];
					sum[3, k] = inp.right_col[k];
				}
			}
			try
			{
				foreach (point k in inp.started_cells)
				{
					set_cell(k);
				}
			}
			catch (NullReferenceException e)
			{

			}
		}
		public void clear()
		{
			//очистка ВСЕХ ячеек и кандидатов на всякий от мусора
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] = 0;
					for (int k = 0; k < candidates.GetLength(2); k++)
					{
						candidates[i, j, k] = true;
					}
				}

			}
		}
		public void clear_sum()
		{
			for (int k = 0; k < matrix.GetLength(1); k++)
			{
				sum[0, k] = 0;
				sum[1, k] = 0;
				sum[2, k] = 0;
				sum[3, k] = 0;
			}
		}

        //преобразование координат
        System.Drawing.Point convert_to_point_coord(int dir, int number_in_a, int number_of) {
            //преобразование координат
			int X = -1;
			int Y = -1;
			switch (dir)  
			{
				case 0:
                    X = number_of;
					Y = number_in_a;
					break;

				case 1:
                    X = number_of;
					Y = n - number_in_a - 1;
					break;

				case 2:
					X = number_in_a;
                    Y = number_of;
					break;

				case 3:
                    X = n - number_in_a - 1;
                    Y = number_of;
					break;
			}
            return new System.Drawing.Point(X, Y);
        }
        int reverce_coord(int dir, int coord)
        {
            switch (dir)
            {
                case 1:
                case 3:
                    return (n - coord - 1);
            }
            return coord;
        }

		//оболочки
		int calc_real_sum(int dir, int k)
		{
			int last = 0, count = 0;
            switch (dir)
            {
                case 0:
                    for (int i = 0; i < n; i++)
                    {
                        if (matrix[k, i] > last)
                        {
                            count++; last = matrix[k, i];
                        }
                    }
                    return count;
                case 1:
                    for (int i = n - 1; i > -1; i--)
                    {
                        if (matrix[k, i] > last)
                        {
                            count++; last = matrix[k, i];
                        }
                    }
                    return count;
                case 2:
                    for (int j = 0; j < n; j++)
                    {
                        if (matrix[j, k] > last)
                        {
                            count++; last = matrix[j, k];
                        }
                    }
                    return count;
                case 3:
                    for (int j = n - 1; j > -1; j--)
                    {
                        if (matrix[j, k] > last)
                        {
                            count++; last = matrix[j, k];
                        }
                    }
                    return count;
            }
			return 0;
		}
		void set_cell(point p)
		{
			this.matrix[p.x, p.y] = p.value;
			for (int k = 0; k < n; k++)
			{
				if (k != p.value - 1)
				{
					candidates[p.x, p.y, k] = false;
				}
			}
			//очищение кандидатов для соседних с поставленными числами
			for (int k = 0; k < n; k++)
			{
				if (k != p.x)
				{
					candidates[k, p.y, p.value - 1] = false;
				}
				if (k != p.y)
				{
					candidates[p.x, k, p.value - 1] = false;
				}
			}
		}
		public void set_cell(int x, int y, int v)
		{
			set_cell(new point(x, y, v));
		}
		public void do_method(int i)
		{
            bool l_d = true;
			switch (i)
			{
				case 0:
					begin();
                    check_set_single();
					break;

				case 1:
                    check_set_single();
					break;

				case 2:
					clear_cand_hard();
                    check_set_single();
					break;

                case 3:
                    creating_blocks();
                    optimization_blocks();
                    if (l_d && g_d) print_blocks();
                    break;

                case 4:
                    sum_two();
                    check_set_single();
                    break;

                case 5:
                    max_stairs();
                    check_set_single();
                    break;

                case 6:
                    min_stairs();
                    check_set_single();
                    break;

                case 7:
                    free_stairs();
                    check_set_single();
                    break;

				case 8:
					Ariadna_for_cells();
					check_set_single();
					break;
				
				default:
				break;
			}
        }

        //печать
        void print_blocks()
        {
            CallBackMy.callbackEventHandler2("Блоки (" + blocks_table.Count + ")\n", true);
            for (int i = 0; i < blocks_table.Count; i++)
            {
                CallBackMy.callbackEventHandler2("Блок " + i + "  ");
                ((block)blocks_table[i]).print();
                CallBackMy.callbackEventHandler2("\n");
            }
            CallBackMy.callbackEventHandler2("Конец распечатки блоков \n");
        }
		void print()
		{
			//шапка
			CallBackMy.callbackEventHandler("    ");
			for (int i = 0; i < n; i++)
			{
				CallBackMy.callbackEventHandler(sum[0, i] + " ");
			}
			CallBackMy.callbackEventHandler("\n");
			//середина
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (j == 0)
					{
						CallBackMy.callbackEventHandler(sum[2, i] + "  ");
					}
					CallBackMy.callbackEventHandler(matrix[i, j] + " ");
					if (j == n - 1)
					{
						CallBackMy.callbackEventHandler(" " + sum[3, i] + "\n");
					}
				}
			}
			//хвост
			CallBackMy.callbackEventHandler("    ");
			for (int i = 0; i < n; i++)
			{
				CallBackMy.callbackEventHandler(sum[1, i] + " ");
			}
			CallBackMy.callbackEventHandler("\n");
		}
		void print_cand()
		{
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					String str = "";
					for (int k = 0; k < n; k++)
					{
						if (candidates[i, j, k])
						{
							str += Convert.ToString(k + 1);
						}
					}
					CallBackMy.callbackEventHandler(str.PadRight(n, '_') + ' ');
				}
				CallBackMy.callbackEventHandler("\n");
			}
		}


        //поисковое
        int get_value_s_pos(int v, int dir, int number_of)
        {
            switch(dir)
            {
                case 0:
                case 1:
                    for (int k = 0; k < this.n; k++)
                    {
                        if (matrix[number_of, k] == v)
                        {
                            return k;
                        }
                    }
                    break;

                case 2:
                case 3:
                    for (int k = 0; k < this.n; k++)
                    {
                        if (matrix[k, number_of] == v)
                        {
                            return k;
                        }
                    }
                    break;
            }
            return -1;
        }

        //для работы с блоками
        public void erase_cell_s_cand_for_block(int _block_number, int _number_in_a_block, int caand)
        {
            int real_coord = ((block_cell)((block)blocks_table[_block_number]).cells_mas[_number_in_a_block]).cell_s_coord;
            int real_number = ((block)blocks_table[_block_number]).number;
            System.Drawing.Point t = new System.Drawing.Point();
            switch (((block)blocks_table[_block_number]).dir)
            {
                case 0:
                case 1:
                    t = new System.Drawing.Point(real_number, real_coord);
                    break;
                case 2:
                case 3:
                    t = new System.Drawing.Point(real_coord, real_number);
                    break;
            }
            
            for (int i = 0; i < ((block_cell)(((block)blocks_table[_block_number]).cells_mas)[_number_in_a_block]).candidates.Count; i++)
            {
                if ((int)((block_cell)(((block)blocks_table[_block_number]).cells_mas)[_number_in_a_block]).candidates[i] == caand)
                {
                    ((block_cell)(((block)blocks_table[_block_number]).cells_mas)[_number_in_a_block]).candidates.RemoveAt(i);
                }
            }

            candidates[t.X, t.Y, caand - 1] = false;
        }
        bool is_cell_in_block_have_a_candidate(int _block_number, int _number_in_a_block, int caand)
        {
            for (int i = 0; i < ((block_cell)(((block)blocks_table[_block_number]).cells_mas)[_number_in_a_block]).candidates.Count; i++)
            {
                if ((int)((block_cell)(((block)blocks_table[_block_number]).cells_mas)[_number_in_a_block]).candidates[i] == caand)
                {
                    return true;
                }
            }

            return false;
        }
        void creating_blocks() {
            blocks_table.Clear();
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    int size_s_pos = get_value_s_pos(n, j, i);

                    switch (j)
                    {
                        case 0:
                        case 2:
                            if (size_s_pos > 0 && sum[j, i] > 1)
                            {
                                block_cell[] cells = new block_cell[size_s_pos + 1];
                                for (int k = 0; k <= size_s_pos; k++)
                                {
                                    System.Drawing.Point t = convert_to_point_coord(j, k, i);
                                    ArrayList temp = new ArrayList();

                                    for (int m = 0; m < this.n; m++)
                                    {
                                        temp.Add(candidates[t.X, t.Y, m]);
                                    }

                                    cells[k] = new block_cell(matrix[t.X, t.Y], k, temp);
                                }
                                blocks_table.Add(new block(j, i, sum[j, i], cells, is_isolated(j, i, size_s_pos)));
                            }
                            break;


                        case 1:
                        case 3:
                            if (size_s_pos >= 0 && size_s_pos < n - 1 && sum[j, i] > 1)
                            {
                                block_cell[] cells = new block_cell[n - size_s_pos];
                                for (int k = 0; k <= reverce_coord(j, size_s_pos); k++)
                                {
                                    System.Drawing.Point t = convert_to_point_coord(j, k, i);
                                    ArrayList temp = new ArrayList();

                                    for (int m = 0; m < this.n; m++)
                                    {
                                        temp.Add(candidates[t.X, t.Y, m]);
                                    }


                                    cells[k] = new block_cell(matrix[t.X, t.Y], reverce_coord(j, k), temp);
                                }
                                blocks_table.Add(new block(j, i, sum[j, i], cells, is_isolated(j, i, 0, size_s_pos)));
                            }
                            break;
                    }
                }
            }
        }
        void optimization_blocks()
        {
            //нужно дописать внутри исключение полностью закрытых, бесполезных для анализа блоков
            bool l_d = false;
            //упрощение блоков
            for (int i = 0; i < blocks_table.Count; i++)
            {
                ArrayList pos_for_deleting = new ArrayList();
                //подсчет вектора переходов
                int last = 0;
                ArrayList change_positions = new ArrayList();
                for (int j = 0; j < ((block)blocks_table[i]).cells_mas.Count; j++)
                {
                    int temp = ((block_cell)(((block)blocks_table[i]).cells_mas)[j]).value;
                    if (temp > last)
                    {
                        last = temp;
                        change_positions.Add(j);
                    }
                }

                int count = 0;
                //перебираем содержимое вектора переходов
                for (int j = 0; j < change_positions.Count - 1; j++)
                {
                    bool flag_delete = false;
                    //перебираем ячейки от начала до очередного элемента вектора переходов
                    for (int k = 0; k < (int)change_positions[j + 1]; k++)
                    {
                        //перебираем кандидаты от последнего кандидата до size -- т.е. ищем ячейки, способные закрыть отрезок стенкой
                        for (int candd = ((block_cell)(((block)blocks_table[i]).cells_mas)[(int)change_positions[j]]).value; candd < n; candd++)
                        {
                            if (((block)blocks_table[i]).is_cell_have_a_candidate(k, candd))
                            {
                               flag_delete = true;
                            }
                        }
                    }
                    //метим ячейки, которые нужно удалить
                    if (!flag_delete)
                    {
                        count++;
                        if (g_d && l_d) CallBackMy.callbackEventHandler("Удаляем в блоке " + i + ": ");
                        for (int k = (int)change_positions[j]; k < (int)change_positions[j + 1]; k++)
                        {
                            if (g_d && l_d) CallBackMy.callbackEventHandler(" " + k);
                            pos_for_deleting.Add(k);

                        }
                        if (g_d && l_d) CallBackMy.callbackEventHandler("\n");
                    }
                }

                //дописать что-то, что исключит 3 | 4 0 0 0 0 6

                //если мы вообще что-нибудь отметили для удаления
                if (pos_for_deleting.Count > 0)
                {
                    if (g_d && l_d) CallBackMy.callbackEventHandler("\nДо удаления, блок " + i + ": ");
                    if (g_d && l_d) ((block)blocks_table[i]).print();
                    //удаляем в обратном порядке отмеченное в pos_for_deleting и снижение суммы на count
                    for (int j = pos_for_deleting.Count - 1; j >= 0; j--)
                    {
                        ((block)blocks_table[i]).cells_mas.RemoveAt((int)pos_for_deleting[j]);
                    }
                    ((block)blocks_table[i]).sum -= count;
                    if (g_d && l_d) CallBackMy.callbackEventHandler("После удаления, блок " + i + ": ");
                    if (g_d && l_d) ((block)blocks_table[i]).print();
                    if (g_d && l_d) CallBackMy.callbackEventHandler("________________________________\n");
                }
            }
            for (int i = blocks_table.Count - 1; i >= 0; i--)
            {
                //проверяем, может это блок вообще больше не нужен, если так - то удаляем нахер!
                if (((block)blocks_table[i]).cells_mas.Count <= 2)
                {
                    if (g_d && l_d) CallBackMy.callbackEventHandler("Совсем удаляем блок " + i + " за бесполезностью\n");
                    blocks_table.RemoveAt(i);
                }
            }
        }

		//проверки
		public void Check()
		{
			//проверка на лишние символы
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (matrix[i, j] > n)
					{
						throw new MyException_cell("В ячейке (" + i + ", " + j + ") цифра, которой не может быть в матрице такой размерности", i, j);
					}
				}
			}
			//проверка на повторяемость в строках и столбцах
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					for (int k = 0; k < n; k++)
					{
						if ((matrix[i, k] == matrix[i, j]) && (k != j) && (matrix[i, j]) > 0)
						{
							throw new MyException_row_or_col("В столбце " + i + " есть повторяющийся символ", i, 0);
						}

						if ((matrix[k, j] == matrix[i, j]) && (k != i) && (matrix[i, j]) > 0)
						{
							throw new MyException_row_or_col("В строке " + j + " есть повторяющийся символ", j, 2);
						}
					}
				}
			}
			//проверка суммы для полных заполненных строк, сделать также для закрытых ячеек -> может, реализовать другой calc сложнее?
			for (int iter = 0; iter < n; iter++)
			{
				//проверка на сумму, для всех сторон
				for (int dir = 0; dir < 4; dir++)
				{
					//проверка на сумму, меньше
					if (calc_real_sum(dir, iter) < sum[dir, iter] && check_for_void(dir, iter) && sum[dir, iter] != 0)
					{
                        throw new MyException_row_or_col_and_sum("Реальная сумма коллекции " + iter + " (dir=" + dir + ") меньше заданной", iter, dir);
					}
					//проверка на соответствие сумм, больше
					if (calc_real_sum(dir, iter) > sum[dir, iter] && check_for_void(dir, iter) && sum[dir, iter] != 0)
					{
                        throw new MyException_row_or_col_and_sum("Реальная сумма коллекции " + iter + " (dir=" + dir + ") больше заданной", iter, dir);
					}
				}
			}
		}
		bool check_for_void(int dir, int number_of)
		{
			//проверка на пустоту
			switch (dir) {
                case 0:
                case 1:
				    for (int k = 0; k < n; k++)
				    {
					    if (matrix[number_of, k] == 0)
					    {
						    return false;
					    }
				    }
				    return true;
                case 2:
                case 3:
				    for (int k = 0; k < n; k++)
				    {
					    if (matrix[k, number_of] == 0)
					    {
						    return false;
					    }
				    }
				    return true;
			}
            return false;
		}
        bool is_isolated(int dir, int number_of, int _first = 0, int _last = -1)
        {
            for (int k = _first; k <= Math.Max(_last, n - 1); k++)
            {
                System.Drawing.Point temp = convert_to_point_coord(dir, k, number_of);
                if (matrix[temp.X, temp.Y] == 0)
                {
                    return false;
                }
            }
            return true;
        }
		bool is_full()
		{
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (matrix[i, j] == 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		//методы - заполнение и исключение без блоков
        void begin()
        {
            bool l_d = false;
            //заполнение самых больших возле единичек
            if (g_d && l_d) CallBackMy.callbackEventHandler("Метод №0: begin()\n");
            for (int j = 0; j < 4; j++)
            {
                for (int iter = 0; iter < n; iter++)
                {
                    if (sum[j, iter] == 1)
                    {
                        int x = -1;
                        int y = -1;
                        switch (j)
                        {
                            case 0:
                                x = iter;
                                y = 0;
                                break;

                            case 1:
                                x = iter;
                                y = n - 1;
                                break;

                            case 2:
                                x = 0;
                                y = iter;
                                break;

                            case 3:
                                x = n - 1;
                                y = iter;
                                break;
                        }
                        if (matrix[x, y] == 0)
                        {
                            if (g_d && l_d) CallBackMy.callbackEventHandler("Заполняем ячейку (" + x + ", " + y + ") единственным возможным значением " + n + "\n");
                            set_cell(x, y, n);
                        }
                        continue;
                    }
                }
            }
        }
        void check_set_single()
        {
            bool l_d = false;
            if (g_d && l_d) CallBackMy.callbackEventHandler("Метод №1: check_set_single\n");
            //кандидат - одиночка
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        int count = 0;
                        int last_pos = -1;
                        for (int k = 0; k < n; k++)
                        {
                            if (candidates[i, j, k])
                            {
                                count++;
                                if (count > 1)
                                {
                                    last_pos = -1;
                                    break;
                                }
                                last_pos = k;
                            }
                        }
                        if (count == 1)
                        {
                            if (g_d && l_d) CallBackMy.callbackEventHandler("Заполняем ячейку-одиночку (" + i + ", " + j + ") значением " + candidates[i, j, last_pos] + "\n");
                            set_cell(i, j, last_pos + 1);
                        }
                    }
                }
            }

            //единственный кандидат в строке\столбце
            for (int k = 0; k < n; k++)
            {
                for (int iter = 0; iter < n; iter++)
                {
                    //для строк
                    int count = 0;
                    int last_pos = -1;
                    for (int i = 0; i < n; i++)
                    {
                        if (candidates[i, iter, k])
                        {
                            count++;
                            if (count > 1)
                            {
                                last_pos = -1;
                                break;
                            }
                            last_pos = i;
                        }
                    }
                    if (count == 1 && matrix[last_pos, iter] == 0)
                    {
                        if (g_d && l_d) CallBackMy.callbackEventHandler("Заполняем ячейку (" + last_pos + ", " + iter + ") значением " + (k + 1) + ", т.к. это единственный кандидат в столбце" + "\n");
                        set_cell(last_pos, iter, k + 1);
                    }
                    //для столбцов
                    count = 0;
                    last_pos = -1;
                    for (int j = 0; j < n; j++)
                    {
                        if (candidates[iter, j, k])
                        {
                            count++;
                            if (count > 1)
                            {
                                last_pos = -1;
                                break;
                            }
                            last_pos = j;
                        }
                    }
                    if (count == 1 && matrix[iter, last_pos] == 0)
                    {
                        if (g_d && l_d) CallBackMy.callbackEventHandler("Заполняем ячейку (" + iter + ", " + last_pos + ") значением " + (k + 1) + ", т.к. это единственный кандидат в строке" + "\n");
                        set_cell(iter, last_pos, k + 1);
                    }
                }
            }
        }
        void clear_cand_hard()
        {
            bool l_d = false;
            //для пустых полей
            //удаление крайних максимальных кандидатов
            for (int j = 0; j < 4; j++)
            {
                for (int iter = 0; iter < n; iter++)
                {
                    if (sum[j, iter] >= 2)
                    {
                        for (int number = 0; number < sum[j, iter] - 1; number++)
                        {
                            System.Drawing.Point temp = convert_to_point_coord(j, number, iter);
                            if (g_d && l_d) CallBackMy.callbackEventHandler("clear_cand_hard (" + j + "): удаляем у ячейки (" + temp.X + ", " + temp.Y + ") кандидаты ");
                            for (int m = 0; m < sum[j, iter] - number - 1; m++)
                            {
                                if (g_d && l_d) CallBackMy.callbackEventHandler(n - m + " ");
                                candidates[temp.X, temp.Y, n - m - 1] = false;
                            }
                            if (g_d && l_d) CallBackMy.callbackEventHandler("\n");
                        }
                    }
                }
            }
        }

        //методы - с блоками
        void sum_two()
        {
            bool l_d = true;
            //можно ли доразвить до большего хз, но довольно хорошо сформулироно
            //проверка суммы-2, наличие size, и m>=2 пустых клеток до него:
            // 1) исключение m-1, m-2, ... кандидатов в первой клетке 
            // 2) верхний зубец стенки - убирание кандидатов максимума от n до края, исключая сам край
            // 3) нижний зубец стенки - убирание минимума у нулевого
            for (int i = 0; i < blocks_table.Count; i++)
            {
                if (((block)blocks_table[i]).sum == 2 
                    && ((block)blocks_table[i]).is_all_empty(((block)blocks_table[i]).get_value_s_pos(n))
                    && ((block)blocks_table[i]).get_value_s_pos(n) > 1
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[0])).value == 0
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[1])).value == 0)
                {
                    //1
                    bool flag_1 = false;
                    for (int k = 1; ((block)blocks_table[i]).get_value_s_pos(n) - k > 0; k++)
                    {
                        int temp = ((block)blocks_table[i]).get_value_s_pos(n) - k;
                        if (is_cell_in_block_have_a_candidate(i, 0, temp))
                        {
                            flag_1 = true;
                            break;
                        }
                    }
                    if (flag_1)
                    {
                        if (g_d && l_d) CallBackMy.callbackEventHandler("sum_two_1 >: удаляем в блоке " + i + " у ячейки " + 0 + " при сумме-2 и наличии " + n + " лишние кандидаты: ");
                        for (int k = 1; ((block)blocks_table[i]).get_value_s_pos(n) - k > 0; k++)
                        {
                            int temp = ((block)blocks_table[i]).get_value_s_pos(n) - k;
                            if (g_d && l_d) CallBackMy.callbackEventHandler(temp + " ");
                            erase_cell_s_cand_for_block(i, 0, temp);
                        }
                        if (g_d && l_d) CallBackMy.callbackEventHandler("\n");
                    }

                    //2
                    bool flag_2 = false;
                    for (int j = ((block)blocks_table[i]).get_value_s_pos(n) - 1; j > 0; j--)
                    {
                        int temp = ((block)blocks_table[i]).max_cand();
                        if (is_cell_in_block_have_a_candidate(i, j, temp))
                        {
                            flag_2 = true;
                            break;
                        }
                    }
                    if (flag_2)
                    {
                        for (int j = ((block)blocks_table[i]).get_value_s_pos(n) - 1; j > 0; j--)
                        {
                            int temp = ((block)blocks_table[i]).max_cand();
                            if (g_d && l_d) CallBackMy.callbackEventHandler("sum_two_2 >: удаляем в блоке " + i + " у ячейки " + j + " кандидат " + temp + " при сумме-2 и наличии " + n + " в блоке\n");
                            erase_cell_s_cand_for_block(i, j, temp);
                        }
                    }

                    //3
                    bool flag_3 = false;
                    if (((block)blocks_table[i]).is_cell_have_a_candidate(0, ((block)blocks_table[i]).min_cand()))
                    {
                        int temp = ((block)blocks_table[i]).min_cand();
                        if (is_cell_in_block_have_a_candidate(i, 0, temp))
                        {
                            flag_3 = true;
                            break;
                        }
                    }
                    if (flag_3)
                    {
                        if (((block)blocks_table[i]).is_cell_have_a_candidate(0, ((block)blocks_table[i]).min_cand()))
                        {
                            int temp = ((block)blocks_table[i]).min_cand();

                            if (g_d && l_d) CallBackMy.callbackEventHandler("sum_two_3 >: удаляем в блоке " + i + " у ячейки " + 0 + " минимальный кандидат " + temp + " в начале блока\n");
                            erase_cell_s_cand_for_block(i, 0, temp);
                        }
                    }
                }
            }
        }
        void max_stairs()
        {
            bool l_d = true;
            //исключение кандидатов лесенкой, начиная с ближайшего к первому ненулевому
            for (int i = 0; i < blocks_table.Count; i++)
            {
                if (((block)blocks_table[i]).sum == ((block)blocks_table[i]).calc_real_sum() + ((block)blocks_table[i]).calc_empty_places_until_noempty()
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[0])).value == 0
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[1])).value == 0
                    && ((block)blocks_table[i]).is_all_empty(((block)blocks_table[i]).cells_mas.Count - 1)
                    )
                {

                    int max_cand = n;
                    for (int j = ((block)blocks_table[i]).calc_empty_places_until_noempty() - 1; j >= 0; j--)
                    {
                        //в каждой ячейке блока: сначала находим следующий первый за максимумом кандидат
                        for (int k = ((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates.Count - 1; k >= 0; k--)
                        {
                            int temp = (int)((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates[k];
                            if (temp < max_cand)
                            {
                                max_cand = temp;
                                break;
                            }
                        }
                        //проверка на необходимость пробега
                        bool flag = false;
                        for (int k = 0; k < ((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates.Count; k++)
                        {
                            int temp = (int)((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates[k];
                            if (temp > max_cand)
                            {
                                if (is_cell_in_block_have_a_candidate(i, j, temp))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        //удаляем в строке все кандидаты, большие максимума, проход обратный обратному - т.е. нормальный
                        if (flag)
                        {
                            if (g_d && l_d) CallBackMy.callbackEventHandler("max_stairs >: исключаем кандидаты блока " + i + " лесенкой\n");
                            for (int k = 0; k < ((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates.Count; k++)
                            {
                                int temp = (int)((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates[k];
                                if (temp > max_cand)
                                {
                                    if (g_d && l_d) CallBackMy.callbackEventHandler(" (" + i + ", " + j + "): " + temp + "\n");
                                    erase_cell_s_cand_for_block(i, j, temp);
                                }
                            }
                        }
                    }
                }
            }
        }
        void min_stairs()
        {
            bool l_d = true;
            //проверка условия, два пустых места сначала, наличие в конце чего-нибудь - убирание кандидатов лесенкой, начиная со второго
            for (int i = 0; i < blocks_table.Count; i++)
            {
                if (((block)blocks_table[i]).sum == ((block)blocks_table[i]).calc_real_sum() + ((block)blocks_table[i]).calc_empty_places_until_noempty()
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[0])).value == 0
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[1])).value == 0
                    && ((block)blocks_table[i]).is_all_empty(((block)blocks_table[i]).cells_mas.Count - 1)
                    )
                {

                    int min_cand = 0;
                    for (int j = 0; j < ((block)blocks_table[i]).cells_mas.Count; j++)
                    {
                        //в каждой ячейке блока: сначала находим следующий первый за минимумом кандидат
                        for (int k = 0; k < ((block_cell)((block)blocks_table[i]).cells_mas[j]).candidates.Count; k++)
                        {
                            int temp = (int)((block_cell)(((block)blocks_table[i]).cells_mas[j])).candidates[k];
                            if (temp > min_cand)
                            {
                                min_cand = temp;
                                break;
                            }
                        }
                        //проверка на необходимость пробега
                        bool flag = false;
                        for (int k = ((block_cell)((block)blocks_table[i]).cells_mas[j]).candidates.Count - 1; k >= 0; k--)
                        {
                            int temp = (int)((block_cell)((block)blocks_table[i]).cells_mas[j]).candidates[k];
                            if (temp < min_cand)
                            {
                                if (is_cell_in_block_have_a_candidate(i, j, temp))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        //удаляем в строке все кандидаты, меньше минимума, проход обратный
                        if (flag)
                        {
                            if (g_d && l_d) CallBackMy.callbackEventHandler("min_stairs >: исключаем кандидаты блока " + i + " лесенкой\n");
                            for (int k = ((block_cell)((block)blocks_table[i]).cells_mas[j]).candidates.Count - 1; k >= 0; k--)
                            {
                                int temp = (int)((block_cell)((block)blocks_table[i]).cells_mas[j]).candidates[k];
                                if (temp < min_cand)
                                {
                                    if (g_d && l_d) CallBackMy.callbackEventHandler(" (" + i + ", " + j + "): " + temp + "\n");
                                    if (i == 1 && j == 1 && temp == 1)
                                    {
                                        int a = 0;
                                    }
                                    erase_cell_s_cand_for_block(i, j, temp);
                                }
                            }
                        }
                    }
                }
            }
        }
        void free_stairs()
        {
            //реализация clear_cand_hard для блоков
            bool l_d = true;
            for (int i = 0; i < blocks_table.Count; i++)
            {
                if (((block)blocks_table[i]).sum > 2
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[0])).value == 0
                    && ((block_cell)(((block)blocks_table[i]).cells_mas[1])).value == 0
                    && ((block)blocks_table[i]).is_all_empty(((block)blocks_table[i]).cells_mas.Count - 1)
                    && ((block)blocks_table[i]).fully_isolated
                    )
                {

                    int max = ((block)blocks_table[i]).max_cand();

                    if (((block)blocks_table[i]).sum > 2)
                    {
                        for (int n = 0; n < ((block)blocks_table[i]).sum - 2; n++)
                        {
                            //проверка на удаление чего-либо
                            bool flag = false;
                            for (int m = 0; m < ((block)blocks_table[i]).sum - n - 2; m++)
                            {
                                if (is_cell_in_block_have_a_candidate(i, n, max - m))
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (flag) {
                                if (g_d && l_d) CallBackMy.callbackEventHandler("free_stairs >: удаляем у ячейки (" + i + ", " + n + ") кандидаты ");
                                for (int m = 0; m < ((block)blocks_table[i]).sum - n - 2; m++)
                                {
                                    if (g_d && l_d) CallBackMy.callbackEventHandler(max - m + " ");
                                    erase_cell_s_cand_for_block(i, n, max - m);
                                }
                                if (g_d && l_d) CallBackMy.callbackEventHandler("\n");
                            }
                        }
                    }
                }
            }
        }

		//Ариадна-1
		void Ariadna_for_cells()
		{
			bool l_d = true;
			//для числа кандидатов в ячейках
			for (int k = 2; k < n - 2; k++)
			{
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						int cout_of_true_candidates = 0;
						ArrayList pos_of_true_candidates = new ArrayList();
						for (int s = 0; s < n; s++)
						{
							if (candidates[i, j, s])
							{
								cout_of_true_candidates++;
								pos_of_true_candidates.Add(s);
							}
						}
						if (g_d && l_d)
						{
							CallBackMy.callbackEventHandler("Сложность " + k + "; уровень " + entry_level + ": Рассматриваем ячейку (" + i + ", " + j + ")");
						}
						if (cout_of_true_candidates == k)
						{
							if (g_d && l_d)
							{
								CallBackMy.callbackEventHandler(": углубляемся\n");
							}
							ArrayList mas = new ArrayList();
							for (int r = 0; r < k; r++)
							{
								mas.Add(this);
							}
							for (int r = 0; r < mas.Count; r++)
							{
								((table)mas[r]).entry_level++;
								if (Ariadna((table)mas[r], new point(i, j, (int)pos_of_true_candidates[r] + 1)))
								{
									return;
								}
							}
						}
						else if (g_d && l_d)
						{
							CallBackMy.callbackEventHandler(": пропускаем\n");
						}
						
					}
				}
			}
		}

		bool Ariadna(table t, point p)
		{
			bool l_d = false;
			if (g_d && l_d)
			{
				CallBackMy.callbackEventHandler("Рассматриваем таблицку\n");
				t.print(); t.print_cand();
				CallBackMy.callbackEventHandler("\n");
			}
			t.set_cell(p);

			int k = 0;
			for (k = 0; (k < 5) && !t.is_full(); k++) {
				try {
					t.Check();
					//методы работы для Ариадны
					t.creating_blocks(); t.optimization_blocks(); t.Check();
					t.sum_two(); t.check_set_single(); t.Check();
					t.max_stairs(); t.check_set_single(); t.Check();
					t.min_stairs(); t.check_set_single(); t.Check();
					t.free_stairs(); t.check_set_single(); t.Check();
					//t->print_blocks();
					//сама Ариадна
					//if (t.entry_level < 2 && k >= 4)
					//{
					//	t.Ariadna_s_thread(); t.check_set_single();
					//}
				}
				catch (Exception e)
				{
					CallBackMy.callbackEventHandler("Уровень " + t.entry_level + ": Ариадна отсекла кандидат " + p.value + " у ячейки (" + p.x + ", " + p.y + ")\n");
					this.candidates[p.x, p.y, p.value - 1] = false;
					return true;
				}
			}
			if (g_d && l_d)
			{
				CallBackMy.callbackEventHandler("После преобразований\n");
				t.print(); t.print_cand(); 
				CallBackMy.callbackEventHandler("\n");
			}
			return false;
		}
	}
}
