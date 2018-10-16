namespace PDSimulation
{
    partial class MainForm
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
            this.goButton = new System.Windows.Forms.Button();
            this.daysTextBox = new System.Windows.Forms.TextBox();
            this.daysLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.simNumberTextbox = new System.Windows.Forms.TextBox();
            this.prelimSurveyTextbox = new System.Windows.Forms.TextBox();
            this.surveyTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataoutPathTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(79, 256);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(129, 31);
            this.goButton.TabIndex = 0;
            this.goButton.Text = "GO";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // daysTextBox
            // 
            this.daysTextBox.Location = new System.Drawing.Point(79, 230);
            this.daysTextBox.Name = "daysTextBox";
            this.daysTextBox.Size = new System.Drawing.Size(129, 20);
            this.daysTextBox.TabIndex = 1;
            // 
            // daysLabel
            // 
            this.daysLabel.AutoSize = true;
            this.daysLabel.Location = new System.Drawing.Point(125, 214);
            this.daysLabel.Name = "daysLabel";
            this.daysLabel.Size = new System.Drawing.Size(34, 13);
            this.daysLabel.TabIndex = 2;
            this.daysLabel.Text = "Days:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of sims";
            // 
            // simNumberTextbox
            // 
            this.simNumberTextbox.Location = new System.Drawing.Point(79, 103);
            this.simNumberTextbox.Name = "simNumberTextbox";
            this.simNumberTextbox.Size = new System.Drawing.Size(129, 20);
            this.simNumberTextbox.TabIndex = 4;
            // 
            // prelimSurveyTextbox
            // 
            this.prelimSurveyTextbox.Location = new System.Drawing.Point(79, 64);
            this.prelimSurveyTextbox.Name = "prelimSurveyTextbox";
            this.prelimSurveyTextbox.Size = new System.Drawing.Size(129, 20);
            this.prelimSurveyTextbox.TabIndex = 5;
            // 
            // surveyTextbox
            // 
            this.surveyTextbox.Location = new System.Drawing.Point(79, 25);
            this.surveyTextbox.Name = "surveyTextbox";
            this.surveyTextbox.Size = new System.Drawing.Size(129, 20);
            this.surveyTextbox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Prelim survey data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Survey data";
            // 
            // dataoutPathTextbox
            // 
            this.dataoutPathTextbox.Location = new System.Drawing.Point(79, 191);
            this.dataoutPathTextbox.Name = "dataoutPathTextbox";
            this.dataoutPathTextbox.Size = new System.Drawing.Size(129, 20);
            this.dataoutPathTextbox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Data output ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 299);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataoutPathTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.surveyTextbox);
            this.Controls.Add(this.prelimSurveyTextbox);
            this.Controls.Add(this.simNumberTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.daysLabel);
            this.Controls.Add(this.daysTextBox);
            this.Controls.Add(this.goButton);
            this.Name = "MainForm";
            this.Text = "PD Simulation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.TextBox daysTextBox;
        private System.Windows.Forms.Label daysLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox simNumberTextbox;
        private System.Windows.Forms.TextBox prelimSurveyTextbox;
        private System.Windows.Forms.TextBox surveyTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dataoutPathTextbox;
        private System.Windows.Forms.Label label4;
    }
}

