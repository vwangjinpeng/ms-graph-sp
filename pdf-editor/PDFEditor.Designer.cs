﻿namespace pdf_editor
{
    partial class PDFEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Edit = new System.Windows.Forms.Button();
            this.button_ptw = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Edit
            // 
            this.button_Edit.Location = new System.Drawing.Point(12, 352);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(154, 86);
            this.button_Edit.TabIndex = 0;
            this.button_Edit.Text = "Edit【Extract PDF】";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // button_ptw
            // 
            this.button_ptw.Location = new System.Drawing.Point(172, 352);
            this.button_ptw.Name = "button_ptw";
            this.button_ptw.Size = new System.Drawing.Size(154, 86);
            this.button_ptw.TabIndex = 1;
            this.button_ptw.Text = "PDF To Word";
            this.button_ptw.UseVisualStyleBackColor = true;
            this.button_ptw.Click += new System.EventHandler(this.button_ptw_Click);
            // 
            // PDFEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_ptw);
            this.Controls.Add(this.button_Edit);
            this.Name = "PDFEditor";
            this.Text = "PDFEditorForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.Button button_ptw;
    }
}

