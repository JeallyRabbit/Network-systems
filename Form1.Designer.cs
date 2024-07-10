namespace Network_Systems
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            label2 = new Label();
            textBoxStart1 = new TextBox();
            textBoxStart2 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            textBoxEnd2 = new TextBox();
            textBoxEnd1 = new TextBox();
            btn_start = new Button();
            progressBar1 = new ProgressBar();
            buttonStop = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            textBoxLogs = new RichTextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 24);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 0;
            label1.Text = "Starting address";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(203, 24);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 1;
            label2.Text = "Ending address";
            // 
            // textBoxStart1
            // 
            textBoxStart1.Location = new Point(81, 42);
            textBoxStart1.Name = "textBoxStart1";
            textBoxStart1.Size = new Size(50, 23);
            textBoxStart1.TabIndex = 2;
            textBoxStart1.TextChanged += textBoxStart1_TextChanged;
            // 
            // textBoxStart2
            // 
            textBoxStart2.Location = new Point(137, 42);
            textBoxStart2.Name = "textBoxStart2";
            textBoxStart2.Size = new Size(50, 23);
            textBoxStart2.TabIndex = 3;
            textBoxStart2.TextChanged += textBoxStart2_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 45);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 4;
            label3.Text = "192.168.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(203, 50);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 7;
            label4.Text = "192.168.";
            // 
            // textBoxEnd2
            // 
            textBoxEnd2.Location = new Point(314, 47);
            textBoxEnd2.Name = "textBoxEnd2";
            textBoxEnd2.Size = new Size(50, 23);
            textBoxEnd2.TabIndex = 6;
            textBoxEnd2.TextChanged += textBoxEnd2_TextChanged;
            // 
            // textBoxEnd1
            // 
            textBoxEnd1.Location = new Point(258, 47);
            textBoxEnd1.Name = "textBoxEnd1";
            textBoxEnd1.Size = new Size(50, 23);
            textBoxEnd1.TabIndex = 5;
            textBoxEnd1.TextChanged += textBoxEnd1_TextChanged;
            // 
            // btn_start
            // 
            btn_start.Location = new Point(26, 71);
            btn_start.Name = "btn_start";
            btn_start.Size = new Size(75, 23);
            btn_start.TabIndex = 8;
            btn_start.Text = "Start Scan";
            btn_start.UseVisualStyleBackColor = true;
            btn_start.Click += btn_start_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(26, 110);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(338, 23);
            progressBar1.TabIndex = 9;
            progressBar1.Click += progressBar1_Click;
            // 
            // buttonStop
            // 
            buttonStop.Location = new Point(26, 157);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(75, 23);
            buttonStop.TabIndex = 14;
            buttonStop.Text = "Stop scanning";
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // textBoxLogs
            // 
            textBoxLogs.BackColor = SystemColors.MenuText;
            textBoxLogs.ForeColor = Color.Green;
            textBoxLogs.Location = new Point(26, 186);
            textBoxLogs.Name = "textBoxLogs";
            textBoxLogs.Size = new Size(338, 97);
            textBoxLogs.TabIndex = 18;
            textBoxLogs.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(393, 295);
            Controls.Add(textBoxLogs);
            Controls.Add(buttonStop);
            Controls.Add(progressBar1);
            Controls.Add(btn_start);
            Controls.Add(label4);
            Controls.Add(textBoxEnd2);
            Controls.Add(textBoxEnd1);
            Controls.Add(label3);
            Controls.Add(textBoxStart2);
            Controls.Add(textBoxStart1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Network Systems";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textBoxStart1;
        private TextBox textBoxStart2;
        private Label label3;
        private Label label4;
        private TextBox textBoxEnd2;
        private TextBox textBoxEnd1;
        private Button btn_start;
        private ProgressBar progressBar1;
        private Label labelUsername;
        private Label labelPassword;
        private TextBox textBoxUserName;
        private TextBox textBoxPassword;
        private Button buttonStop;
        private ContextMenuStrip contextMenuStrip1;
        private RichTextBox textBoxLogs;
    }
}