﻿using System;
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
	public partial class Methods : Form
	{
		public Methods()
		{
			InitializeComponent();
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				checkedListBox1.SetItemChecked(i, true);
			}
		}

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
	}
}
