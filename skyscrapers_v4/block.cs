using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skyscrapers_v4
{
    class block_cell
    {
        public int value;
        public int cell_s_coord;
        public ArrayList candidates;

        public block_cell(int value, int cell_s_coord, ArrayList candidates)
        {
            this.value = value;
            this.cell_s_coord = cell_s_coord;
            this.candidates = new ArrayList();
            for (int i = 0; i < candidates.Count; i++)
            {
                if ((bool)candidates[i]) this.candidates.Add(i + 1);
            }
        }
    }
	class block
	{
		public int dir;
        public int number;
        public int sum;
        public ArrayList cells_mas;
        public bool fully_isolated;
        public bool is_cell_have_a_candidate(int m, int cand)
		{
            if (((block_cell)cells_mas[m]).value > 0)
            {
                return false;
            }
            for (int k = 0; k < ((block_cell)cells_mas[m]).candidates.Count; k++) {
			    if ((int)((block_cell)cells_mas[m]).candidates[k] == cand) {
				    return true;
			    }
		    }
		    return false;
		}
        public int max_cand()
		{
			int max = 0;
            foreach (block_cell i in cells_mas)
			{
                if (i.value == 0)
                {
                    int temp = (int)i.candidates[i.candidates.Count - 1];
                    if (temp > max) max = temp;
                }
			}
			return max;
		}
        public int min_cand()
		{
            int min = 9;
            foreach (block_cell i in cells_mas)
			{
                if (i.value == 0)
                {
                    int temp = (int)i.candidates[0];
                    if (temp < min) min = temp;
                }
			}
			return min;
		}
        public int get_value_s_pos(int v)
		{
			for (int k = 0; k < cells_mas.Count; k++)
			{
				if (((block_cell)cells_mas[k]).value == v)
				{
					return k;
				}
			}
			return -1;
		}
        public bool is_all_empty(int until_pos)
		{
			if (until_pos == -1)
			{
				return false;
			}

            for (int k = 0; k < until_pos; k++)
			{
				if (((block_cell)cells_mas[k]).value != 0)
				{
					return false;
				}
			}
			return true;
		}
        public int calc_real_sum()
		{
			int last = 0, count = 0;
            foreach (block_cell i in cells_mas)
			{
				if (i.value > last)
				{
					count++; last = i.value;
				}
			}
			return count;
		}
        public int calc_empty_places_until_noempty()
		{
			int empty_places_count = 0;
            foreach (block_cell i in cells_mas)
			{
				if (i.value == 0) empty_places_count++; else return empty_places_count;
			}
            return empty_places_count;
		}

		public block(int dir, int number, int sum, block_cell[] cells, bool fully_isolated)
		{
			this.dir = dir;
			this.number = number;
			this.sum = sum;
            cells_mas = new ArrayList();
            foreach (block_cell i in cells)
            {
                this.cells_mas.Add(i);
            }
			this.fully_isolated = fully_isolated;
		}

		public void print() {
			CallBackMy.callbackEventHandler2("| " + fully_isolated + " " + dir + " " + number + ", ячейки: ");
            foreach (block_cell i in cells_mas)
            {
				CallBackMy.callbackEventHandler2(Convert.ToString(i.cell_s_coord));
			}
			CallBackMy.callbackEventHandler2("\n" + sum + " | ");
			foreach (block_cell i in cells_mas)
            {
				CallBackMy.callbackEventHandler2(" " + i.value);
			}
			CallBackMy.callbackEventHandler2("\n");
            foreach (block_cell i in cells_mas)
			{
				for (int j = 0; j < i.candidates.Count; j++)
				{
					CallBackMy.callbackEventHandler2(Convert.ToString(i.candidates[j]));
				}
				CallBackMy.callbackEventHandler2(" ");
			}
			CallBackMy.callbackEventHandler2("\n");
		}
	}
}
