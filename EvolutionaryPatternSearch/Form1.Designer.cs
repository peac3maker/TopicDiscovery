﻿namespace EvolutionaryPatternSearch
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbRes = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dtmStart = new System.Windows.Forms.DateTimePicker();
            this.dtmEnd = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(134, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Topics";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Folder";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(133, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Click += new System.EventHandler(this.textBox2_Click);
            // 
            // tbRes
            // 
            this.tbRes.Location = new System.Drawing.Point(16, 71);
            this.tbRes.Multiline = true;
            this.tbRes.Name = "tbRes";
            this.tbRes.Size = new System.Drawing.Size(855, 269);
            this.tbRes.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "get1MinuteOfTwitterFeeds";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtmStart
            // 
            this.dtmStart.CustomFormat = "dd.MM.yyyy hh:mm:ss";
            this.dtmStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmStart.Location = new System.Drawing.Point(471, 12);
            this.dtmStart.Name = "dtmStart";
            this.dtmStart.Size = new System.Drawing.Size(200, 20);
            this.dtmStart.TabIndex = 6;
            // 
            // dtmEnd
            // 
            this.dtmEnd.CustomFormat = "dd.MM.yyyy hh:mm:ss";
            this.dtmEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmEnd.Location = new System.Drawing.Point(471, 39);
            this.dtmEnd.Name = "dtmEnd";
            this.dtmEnd.Size = new System.Drawing.Size(200, 20);
            this.dtmEnd.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 352);
            this.Controls.Add(this.dtmEnd);
            this.Controls.Add(this.dtmStart);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbRes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox tbRes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dtmStart;
        private System.Windows.Forms.DateTimePicker dtmEnd;
    }
}

