namespace POSdllDemo
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl_PortType = new System.Windows.Forms.TabControl();
            this.tabPage_USB = new System.Windows.Forms.TabPage();
            this.tabPage_COM = new System.Windows.Forms.TabPage();
            this.tabControl_PortType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_PortType
            // 
            this.tabControl_PortType.Controls.Add(this.tabPage_USB);
            this.tabControl_PortType.Controls.Add(this.tabPage_COM);
            this.tabControl_PortType.Location = new System.Drawing.Point(3, 3);
            this.tabControl_PortType.Name = "tabControl_PortType";
            this.tabControl_PortType.SelectedIndex = 0;
            this.tabControl_PortType.Size = new System.Drawing.Size(513, 380);
            this.tabControl_PortType.TabIndex = 5;
            // 
            // tabPage_USB
            // 
            this.tabPage_USB.Location = new System.Drawing.Point(4, 22);
            this.tabPage_USB.Name = "tabPage_USB";
            this.tabPage_USB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_USB.Size = new System.Drawing.Size(505, 354);
            this.tabPage_USB.TabIndex = 0;
            this.tabPage_USB.Text = "USB口打印";
            this.tabPage_USB.UseVisualStyleBackColor = true;
            // 
            // tabPage_COM
            // 
            this.tabPage_COM.Location = new System.Drawing.Point(4, 22);
            this.tabPage_COM.Name = "tabPage_COM";
            this.tabPage_COM.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_COM.Size = new System.Drawing.Size(505, 354);
            this.tabPage_COM.TabIndex = 1;
            this.tabPage_COM.Text = "串口打印";
            this.tabPage_COM.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 388);
            this.Controls.Add(this.tabControl_PortType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrinterDEMO";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl_PortType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_PortType;
        private System.Windows.Forms.TabPage tabPage_USB;
        private System.Windows.Forms.TabPage tabPage_COM;
    }
}

