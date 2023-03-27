namespace Point_Blank_Launcher
{
    partial class Launcher
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">verdade se for necessário descartar os recursos gerenciados; caso contrário, falso.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte do Designer - não modifique
        /// o conteúdo deste método com o editor de Code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.label2 = new System.Windows.Forms.Label();
            this.closeBut = new System.Windows.Forms.PictureBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.Start = new System.Windows.Forms.Button();
            this.Verif = new System.Windows.Forms.Button();
            this.Button_Update = new System.Windows.Forms.Button();
            this.TotalBar = new System.Windows.Forms.PictureBox();
            this.ArchiveBar = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.fileName = new System.Windows.Forms.Label();
            this.minimBut = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.closeBut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArchiveBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimBut)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(180, 614);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "File:";
            // 
            // closeBut
            // 
            this.closeBut.BackColor = System.Drawing.Color.White;
            this.closeBut.BackgroundImage = global::Point_Blank_Launcher.Properties.Resources.CloseZz1;
            this.closeBut.Location = new System.Drawing.Point(899, 11);
            this.closeBut.Margin = new System.Windows.Forms.Padding(2);
            this.closeBut.Name = "closeBut";
            this.closeBut.Size = new System.Drawing.Size(20, 20);
            this.closeBut.TabIndex = 4;
            this.closeBut.TabStop = false;
            this.closeBut.Click += new System.EventHandler(this.pictureBox1_Click);
            this.closeBut.MouseEnter += new System.EventHandler(this.pictureBox1_MouseMove);
            this.closeBut.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(589, 123);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(13, 13);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(310, 153);
            this.webBrowser1.TabIndex = 5;
            this.webBrowser1.Url = new System.Uri("http://newlauncher.rootpb.com/launcherupdate/announce", System.UriKind.Absolute);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.Transparent;
            this.Start.BackgroundImage = global::Point_Blank_Launcher.Properties.Resources.START_NORMAL;
            this.Start.FlatAppearance.BorderSize = 0;
            this.Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Start.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Start.ForeColor = System.Drawing.Color.Red;
            this.Start.Location = new System.Drawing.Point(745, 599);
            this.Start.Margin = new System.Windows.Forms.Padding(2);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(162, 73);
            this.Start.TabIndex = 7;
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            this.Start.MouseLeave += new System.EventHandler(this.Start_MouseLeave);
            this.Start.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Start_MouseMove);
            // 
            // Verif
            // 
            this.Verif.BackColor = System.Drawing.Color.Transparent;
            this.Verif.BackgroundImage = global::Point_Blank_Launcher.Properties.Resources.CHECK_NORMAL;
            this.Verif.FlatAppearance.BorderSize = 0;
            this.Verif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Verif.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Verif.ForeColor = System.Drawing.Color.Blue;
            this.Verif.Location = new System.Drawing.Point(578, 599);
            this.Verif.Margin = new System.Windows.Forms.Padding(2);
            this.Verif.Name = "Verif";
            this.Verif.Size = new System.Drawing.Size(162, 73);
            this.Verif.TabIndex = 8;
            this.Verif.UseVisualStyleBackColor = false;
            this.Verif.Click += new System.EventHandler(this.Verif_Click);
            this.Verif.MouseLeave += new System.EventHandler(this.Verif_MouseLeave);
            this.Verif.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Verif_MouseMove);
            // 
            // Button_Update
            // 
            this.Button_Update.BackColor = System.Drawing.Color.Transparent;
            this.Button_Update.BackgroundImage = global::Point_Blank_Launcher.Properties.Resources.UPDATE_NORMAL;
            this.Button_Update.Enabled = false;
            this.Button_Update.FlatAppearance.BorderSize = 0;
            this.Button_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Update.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Button_Update.ForeColor = System.Drawing.Color.Red;
            this.Button_Update.Location = new System.Drawing.Point(745, 599);
            this.Button_Update.Margin = new System.Windows.Forms.Padding(2);
            this.Button_Update.Name = "Button_Update";
            this.Button_Update.Size = new System.Drawing.Size(162, 73);
            this.Button_Update.TabIndex = 9;
            this.Button_Update.UseVisualStyleBackColor = true;
            this.Button_Update.Click += new System.EventHandler(this.Button_Update_Click);
            this.Button_Update.MouseLeave += new System.EventHandler(this.Button_Update_MouseLeave);
            this.Button_Update.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button_Update_MouseMove);
            // 
            // TotalBar
            // 
            this.TotalBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.TotalBar.Location = new System.Drawing.Point(0, 698);
            this.TotalBar.Name = "TotalBar";
            this.TotalBar.Size = new System.Drawing.Size(930, 16);
            this.TotalBar.TabIndex = 10;
            this.TotalBar.TabStop = false;
            // 
            // ArchiveBar
            // 
            this.ArchiveBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ArchiveBar.Location = new System.Drawing.Point(183, 662);
            this.ArchiveBar.Name = "ArchiveBar";
            this.ArchiveBar.Size = new System.Drawing.Size(389, 10);
            this.ArchiveBar.TabIndex = 11;
            this.ArchiveBar.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Location = new System.Drawing.Point(183, 662);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(389, 10);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.pictureBox3.Location = new System.Drawing.Point(0, 698);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(930, 16);
            this.pictureBox3.TabIndex = 13;
            this.pictureBox3.TabStop = false;
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.BackColor = System.Drawing.Color.Transparent;
            this.fileName.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold);
            this.fileName.ForeColor = System.Drawing.Color.White;
            this.fileName.Location = new System.Drawing.Point(223, 614);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(56, 19);
            this.fileName.TabIndex = 14;
            this.fileName.Text = "File.zip";
            this.fileName.Visible = false;
            // 
            // minimBut
            // 
            this.minimBut.BackgroundImage = global::Point_Blank_Launcher.Properties.Resources.miniZZ;
            this.minimBut.Location = new System.Drawing.Point(874, 11);
            this.minimBut.Name = "minimBut";
            this.minimBut.Size = new System.Drawing.Size(20, 20);
            this.minimBut.TabIndex = 15;
            this.minimBut.TabStop = false;
            this.minimBut.Click += new System.EventHandler(this.pictureBox4_Click);
            this.minimBut.MouseLeave += new System.EventHandler(this.pictureBox4_MouseLeave);
            this.minimBut.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseMove);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(4F, 7F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(930, 714);
            this.Controls.Add(this.minimBut);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.ArchiveBar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.Verif);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.closeBut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TotalBar);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Button_Update);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PBLauncher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.form_Closing);
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.launcher_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.launcher_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.closeBut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArchiveBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimBut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox closeBut;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button Verif;
        private System.Windows.Forms.Button Button_Update;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.PictureBox TotalBar;
        private System.Windows.Forms.PictureBox ArchiveBar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.PictureBox minimBut;
    }
}