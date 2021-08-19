
namespace ScrollParatext
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboBooks = new System.Windows.Forms.ComboBox();
            this.cboChapters = new System.Windows.Forms.ComboBox();
            this.cboVerses = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVerseText = new System.Windows.Forms.Label();
            this.radA = new System.Windows.Forms.RadioButton();
            this.radB = new System.Windows.Forms.RadioButton();
            this.radC = new System.Windows.Forms.RadioButton();
            this.radD = new System.Windows.Forms.RadioButton();
            this.radE = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboBooks
            // 
            this.cboBooks.FormattingEnabled = true;
            this.cboBooks.Location = new System.Drawing.Point(12, 95);
            this.cboBooks.Name = "cboBooks";
            this.cboBooks.Size = new System.Drawing.Size(121, 23);
            this.cboBooks.TabIndex = 0;
            this.cboBooks.SelectedIndexChanged += new System.EventHandler(this.cboBooks_SelectedIndexChanged);
            // 
            // cboChapters
            // 
            this.cboChapters.FormattingEnabled = true;
            this.cboChapters.Location = new System.Drawing.Point(153, 95);
            this.cboChapters.Name = "cboChapters";
            this.cboChapters.Size = new System.Drawing.Size(121, 23);
            this.cboChapters.TabIndex = 1;
            this.cboChapters.SelectedIndexChanged += new System.EventHandler(this.cboChapters_SelectedIndexChanged);
            // 
            // cboVerses
            // 
            this.cboVerses.FormattingEnabled = true;
            this.cboVerses.Location = new System.Drawing.Point(292, 95);
            this.cboVerses.Name = "cboVerses";
            this.cboVerses.Size = new System.Drawing.Size(121, 23);
            this.cboVerses.TabIndex = 2;
            this.cboVerses.SelectedIndexChanged += new System.EventHandler(this.cboVerses_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Book\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chapter\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Verse";
            // 
            // lblVerseText
            // 
            this.lblVerseText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVerseText.Location = new System.Drawing.Point(12, 140);
            this.lblVerseText.Name = "lblVerseText";
            this.lblVerseText.Size = new System.Drawing.Size(537, 193);
            this.lblVerseText.TabIndex = 7;
            // 
            // radA
            // 
            this.radA.AutoSize = true;
            this.radA.Checked = true;
            this.radA.Location = new System.Drawing.Point(12, 27);
            this.radA.Name = "radA";
            this.radA.Size = new System.Drawing.Size(33, 19);
            this.radA.TabIndex = 8;
            this.radA.TabStop = true;
            this.radA.Text = "A";
            this.radA.UseVisualStyleBackColor = true;
            // 
            // radB
            // 
            this.radB.AutoSize = true;
            this.radB.Location = new System.Drawing.Point(60, 27);
            this.radB.Name = "radB";
            this.radB.Size = new System.Drawing.Size(32, 19);
            this.radB.TabIndex = 9;
            this.radB.Text = "B";
            this.radB.UseVisualStyleBackColor = true;
            // 
            // radC
            // 
            this.radC.AutoSize = true;
            this.radC.Location = new System.Drawing.Point(108, 27);
            this.radC.Name = "radC";
            this.radC.Size = new System.Drawing.Size(33, 19);
            this.radC.TabIndex = 10;
            this.radC.Text = "C";
            this.radC.UseVisualStyleBackColor = true;
            // 
            // radD
            // 
            this.radD.AutoSize = true;
            this.radD.Location = new System.Drawing.Point(156, 27);
            this.radD.Name = "radD";
            this.radD.Size = new System.Drawing.Size(33, 19);
            this.radD.TabIndex = 11;
            this.radD.Text = "D";
            this.radD.UseVisualStyleBackColor = true;
            // 
            // radE
            // 
            this.radE.AutoSize = true;
            this.radE.Location = new System.Drawing.Point(204, 27);
            this.radE.Name = "radE";
            this.radE.Size = new System.Drawing.Size(31, 19);
            this.radE.TabIndex = 12;
            this.radE.Text = "E";
            this.radE.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(224, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Scroll Group (Paratext) / Link Set (Logos) ";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 342);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radE);
            this.Controls.Add(this.radD);
            this.Controls.Add(this.radC);
            this.Controls.Add(this.radB);
            this.Controls.Add(this.radA);
            this.Controls.Add(this.lblVerseText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboVerses);
            this.Controls.Add(this.cboChapters);
            this.Controls.Add(this.cboBooks);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBooks;
        private System.Windows.Forms.ComboBox cboChapters;
        private System.Windows.Forms.ComboBox cboVerses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVerseText;
        private System.Windows.Forms.RadioButton radA;
        private System.Windows.Forms.RadioButton radB;
        private System.Windows.Forms.RadioButton radC;
        private System.Windows.Forms.RadioButton radD;
        private System.Windows.Forms.RadioButton radE;
        private System.Windows.Forms.Label label5;
    }
}

