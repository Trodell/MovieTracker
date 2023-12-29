namespace MovieTracker
{
    partial class UserMovieList
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
            this.moviesGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.moviesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // moviesGrid
            // 
            this.moviesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.moviesGrid.Location = new System.Drawing.Point(166, 209);
            this.moviesGrid.Name = "moviesGrid";
            this.moviesGrid.Size = new System.Drawing.Size(440, 150);
            this.moviesGrid.TabIndex = 0;
            // 
            // UserMovieList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.moviesGrid);
            this.Name = "UserMovieList";
            this.Text = "UserMovieList";
            this.Load += new System.EventHandler(this.UserMovieList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.moviesGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView moviesGrid;
    }
}