﻿// form1.designer.cs
namespace CompressionApp
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
            this.compressButton = new System.Windows.Forms.Button();
            this.decompressButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // compressButton
            // 
            this.compressButton.Location = new System.Drawing.Point(53, 56);
            this.compressButton.Name = "compressButton";
            this.compressButton.Size = new System.Drawing.Size(168, 52);
            this.compressButton.TabIndex = 0;
            this.compressButton.Text = "Compress Files";
            this.compressButton.UseVisualStyleBackColor = true;
            this.compressButton.Click += new System.EventHandler(this.compressButton_Click);
            // 
            // decompressButton
            // 
            this.decompressButton.Location = new System.Drawing.Point(53, 156);
            this.decompressButton.Name = "decompressButton";
            this.decompressButton.Size = new System.Drawing.Size(168, 52);
            this.decompressButton.TabIndex = 1;
            this.decompressButton.Text = "Decompress Files";
            this.decompressButton.UseVisualStyleBackColor = true;
            this.decompressButton.Click += new System.EventHandler(this.decompressButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.decompressButton);
            this.Controls.Add(this.compressButton);
            this.Name = "Form1";
            this.Text = "File Compression App";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button compressButton;
        private System.Windows.Forms.Button decompressButton;
    }
}
