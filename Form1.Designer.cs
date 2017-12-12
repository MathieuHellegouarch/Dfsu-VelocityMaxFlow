namespace Compute_Direction_Max
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbInput = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbOptionChoice = new System.Windows.Forms.Label();
            this.radioH = new System.Windows.Forms.RadioButton();
            this.radioV = new System.Windows.Forms.RadioButton();
            this.btStart = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lbInput
            // 
            this.lbInput.AutoSize = true;
            this.lbInput.Location = new System.Drawing.Point(28, 35);
            this.lbInput.Name = "lbInput";
            this.lbInput.Size = new System.Drawing.Size(78, 13);
            this.lbInput.TabIndex = 0;
            this.lbInput.Text = "Input result file:";
            // 
            // txtPath
            // 
            this.txtPath.Enabled = false;
            this.txtPath.Location = new System.Drawing.Point(131, 32);
            this.txtPath.Name = "txtPath";
            this.txtPath.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPath.Size = new System.Drawing.Size(418, 20);
            this.txtPath.TabIndex = 1;
            // 
            // btBrowse
            // 
            this.btBrowse.Location = new System.Drawing.Point(565, 31);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btBrowse.Size = new System.Drawing.Size(33, 23);
            this.btBrowse.TabIndex = 2;
            this.btBrowse.Text = "...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lbOptionChoice
            // 
            this.lbOptionChoice.AutoSize = true;
            this.lbOptionChoice.Location = new System.Drawing.Point(28, 91);
            this.lbOptionChoice.Name = "lbOptionChoice";
            this.lbOptionChoice.Size = new System.Drawing.Size(247, 13);
            this.lbOptionChoice.TabIndex = 3;
            this.lbOptionChoice.Text = "Compute the direction of the flow at the moment of:";
            // 
            // radioH
            // 
            this.radioH.AutoSize = true;
            this.radioH.Checked = true;
            this.radioH.Location = new System.Drawing.Point(306, 89);
            this.radioH.Name = "radioH";
            this.radioH.Size = new System.Drawing.Size(98, 17);
            this.radioH.TabIndex = 4;
            this.radioH.TabStop = true;
            this.radioH.Text = "maximum depth";
            this.radioH.UseVisualStyleBackColor = true;
            // 
            // radioV
            // 
            this.radioV.AutoSize = true;
            this.radioV.Location = new System.Drawing.Point(306, 126);
            this.radioV.Name = "radioV";
            this.radioV.Size = new System.Drawing.Size(100, 17);
            this.radioV.TabIndex = 5;
            this.radioV.Text = "maximum speed";
            this.radioV.UseVisualStyleBackColor = true;
            // 
            // btStart
            // 
            this.btStart.Enabled = false;
            this.btStart.Location = new System.Drawing.Point(425, 183);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 6;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(523, 183);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 7;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(31, 183);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(373, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 221);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.radioV);
            this.Controls.Add(this.radioH);
            this.Controls.Add(this.lbOptionChoice);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lbInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Compute flow direction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInput;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lbOptionChoice;
        private System.Windows.Forms.RadioButton radioH;
        private System.Windows.Forms.RadioButton radioV;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

