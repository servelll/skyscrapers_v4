using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skyscrapers_v4
{
    public class point {
        public int x;
        public int y;
        public int value;
	    public point(int _x, int _y, int _value) {
		    x = _x;
		    y = _y;
		    value = _value;
	    }
    }
    public class input
    {
        public int size;
        public int[] left_col;
        public int[] right_col;
        public int[] top_row;
        public int[] bottom_row;

        public List <point> started_cells;
        public input(int _s, int[] a, int[] b, int[] c, int[] d)
        {
            this.size = _s;
            this.left_col = a;
            this.right_col = b;
            this.top_row = c;
            this.bottom_row = d;
			started_cells = new List <point> ();
        }
		public void add_started_cell(int x, int y, int value) {
			started_cells.Add(new point(x, y, value));
		}
    }
}


