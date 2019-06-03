namespace skyscrapers_v4
{
	partial class Methods
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Items.AddRange(new object[] {
            "0. Начальный (максимумы возле сумм-единиц)",
            "1. Кандидаты-одиночки (по ячейкам, строкам и столбцам)",
            "2. Ближайшая к краю лесенка кандидатов",
            "Собрать и распечатать блоки",
            "4. [Блоки] - для блоков суммы-2 - 3 метода",
            "5. [Блоки] - максимальная лесенка",
            "6. [Блоки] - минимальная лесенка",
            "7. [Блоки] - свободная лесенка",
            "8. Ариадна"});
			this.checkedListBox1.Location = new System.Drawing.Point(12, 12);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(313, 259);
			this.checkedListBox1.TabIndex = 0;
			this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
			// 
			// Methods
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(333, 279);
			this.Controls.Add(this.checkedListBox1);
			this.Name = "Methods";
			this.Text = "Methods";
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.CheckedListBox checkedListBox1;
	}
}