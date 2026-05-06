namespace PrinterDEMO
{
    partial class UserControl_USB_Connect
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_ESC = new System.Windows.Forms.TabPage();
            this.button_ESC_Japanese = new System.Windows.Forms.Button();
            this.button_ESC_State = new System.Windows.Forms.Button();
            this.button_ESC_QRcode = new System.Windows.Forms.Button();
            this.button_ESC_Barcode = new System.Windows.Forms.Button();
            this.button_ESC_Image = new System.Windows.Forms.Button();
            this.button_ESC_Text = new System.Windows.Forms.Button();
            this.tabPage_TSPL = new System.Windows.Forms.TabPage();
            this.button_TSPL_Japanese = new System.Windows.Forms.Button();
            this.button_TSPL_Proof = new System.Windows.Forms.Button();
            this.button_TSPL_State = new System.Windows.Forms.Button();
            this.button_TSPL_QRcode = new System.Windows.Forms.Button();
            this.button_TSPL_Barcode = new System.Windows.Forms.Button();
            this.button_TSPL_Image = new System.Windows.Forms.Button();
            this.button_TSPL_Text = new System.Windows.Forms.Button();
            this.tabPage_CPCL = new System.Windows.Forms.TabPage();
            this.button_CPCL_France = new System.Windows.Forms.Button();
            this.button_CPCL_Japanese = new System.Windows.Forms.Button();
            this.button_CPCL_Datamatrix = new System.Windows.Forms.Button();
            this.button_CPCL_Pdf417 = new System.Windows.Forms.Button();
            this.button_CPCL_Proof = new System.Windows.Forms.Button();
            this.button_CPCL_State = new System.Windows.Forms.Button();
            this.button_CPCL_QRcode = new System.Windows.Forms.Button();
            this.button_CPCL_Barcode = new System.Windows.Forms.Button();
            this.button_CPCL_Image = new System.Windows.Forms.Button();
            this.button_CPCL_Text = new System.Windows.Forms.Button();
            this.tb_USB_state = new System.Windows.Forms.TextBox();
            this.button_clean = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ESC_Codepage = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage_ESC.SuspendLayout();
            this.tabPage_TSPL.SuspendLayout();
            this.tabPage_CPCL.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_ESC);
            this.tabControl1.Controls.Add(this.tabPage_TSPL);
            this.tabControl1.Controls.Add(this.tabPage_CPCL);
            this.tabControl1.Location = new System.Drawing.Point(3, 54);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(300, 297);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_ESC
            // 
            this.tabPage_ESC.Controls.Add(this.button_ESC_Codepage);
            this.tabPage_ESC.Controls.Add(this.button_ESC_Japanese);
            this.tabPage_ESC.Controls.Add(this.button_ESC_State);
            this.tabPage_ESC.Controls.Add(this.button_ESC_QRcode);
            this.tabPage_ESC.Controls.Add(this.button_ESC_Barcode);
            this.tabPage_ESC.Controls.Add(this.button_ESC_Image);
            this.tabPage_ESC.Controls.Add(this.button_ESC_Text);
            this.tabPage_ESC.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ESC.Name = "tabPage_ESC";
            this.tabPage_ESC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ESC.Size = new System.Drawing.Size(292, 271);
            this.tabPage_ESC.TabIndex = 0;
            this.tabPage_ESC.Text = "ESC/POS指令";
            this.tabPage_ESC.UseVisualStyleBackColor = true;
            // 
            // button_ESC_Japanese
            // 
            this.button_ESC_Japanese.Location = new System.Drawing.Point(147, 6);
            this.button_ESC_Japanese.Name = "button_ESC_Japanese";
            this.button_ESC_Japanese.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_Japanese.TabIndex = 5;
            this.button_ESC_Japanese.Text = "日文打印";
            this.button_ESC_Japanese.UseVisualStyleBackColor = true;
            this.button_ESC_Japanese.Click += new System.EventHandler(this.button_ESC_Japanese_Click);
            // 
            // button_ESC_State
            // 
            this.button_ESC_State.Location = new System.Drawing.Point(6, 194);
            this.button_ESC_State.Name = "button_ESC_State";
            this.button_ESC_State.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_State.TabIndex = 4;
            this.button_ESC_State.Text = "状态查询";
            this.button_ESC_State.UseVisualStyleBackColor = true;
            this.button_ESC_State.Click += new System.EventHandler(this.button_ESC_State_Click);
            // 
            // button_ESC_QRcode
            // 
            this.button_ESC_QRcode.Location = new System.Drawing.Point(6, 147);
            this.button_ESC_QRcode.Name = "button_ESC_QRcode";
            this.button_ESC_QRcode.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_QRcode.TabIndex = 3;
            this.button_ESC_QRcode.Text = "二维码打印";
            this.button_ESC_QRcode.UseVisualStyleBackColor = true;
            this.button_ESC_QRcode.Click += new System.EventHandler(this.button_ESC_QRcode_Click);
            // 
            // button_ESC_Barcode
            // 
            this.button_ESC_Barcode.Location = new System.Drawing.Point(6, 100);
            this.button_ESC_Barcode.Name = "button_ESC_Barcode";
            this.button_ESC_Barcode.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_Barcode.TabIndex = 2;
            this.button_ESC_Barcode.Text = "条码打印";
            this.button_ESC_Barcode.UseVisualStyleBackColor = true;
            this.button_ESC_Barcode.Click += new System.EventHandler(this.button_ESC_Barcode_Click);
            // 
            // button_ESC_Image
            // 
            this.button_ESC_Image.Location = new System.Drawing.Point(6, 53);
            this.button_ESC_Image.Name = "button_ESC_Image";
            this.button_ESC_Image.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_Image.TabIndex = 1;
            this.button_ESC_Image.Text = "图片打印";
            this.button_ESC_Image.UseVisualStyleBackColor = true;
            this.button_ESC_Image.Click += new System.EventHandler(this.button_ESC_Image_Click);
            // 
            // button_ESC_Text
            // 
            this.button_ESC_Text.Location = new System.Drawing.Point(6, 6);
            this.button_ESC_Text.Name = "button_ESC_Text";
            this.button_ESC_Text.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_Text.TabIndex = 0;
            this.button_ESC_Text.Text = "文本打印";
            this.button_ESC_Text.UseVisualStyleBackColor = true;
            this.button_ESC_Text.Click += new System.EventHandler(this.button_ESC_Text_Click);
            // 
            // tabPage_TSPL
            // 
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_Japanese);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_Proof);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_State);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_QRcode);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_Barcode);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_Image);
            this.tabPage_TSPL.Controls.Add(this.button_TSPL_Text);
            this.tabPage_TSPL.Location = new System.Drawing.Point(4, 22);
            this.tabPage_TSPL.Name = "tabPage_TSPL";
            this.tabPage_TSPL.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TSPL.Size = new System.Drawing.Size(292, 271);
            this.tabPage_TSPL.TabIndex = 1;
            this.tabPage_TSPL.Text = "TSPL指令";
            this.tabPage_TSPL.UseVisualStyleBackColor = true;
            // 
            // button_TSPL_Japanese
            // 
            this.button_TSPL_Japanese.Location = new System.Drawing.Point(147, 6);
            this.button_TSPL_Japanese.Name = "button_TSPL_Japanese";
            this.button_TSPL_Japanese.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_Japanese.TabIndex = 11;
            this.button_TSPL_Japanese.Text = "日文打印";
            this.button_TSPL_Japanese.UseVisualStyleBackColor = true;
            this.button_TSPL_Japanese.Click += new System.EventHandler(this.button_TSPL_Japanese_Click);
            // 
            // button_TSPL_Proof
            // 
            this.button_TSPL_Proof.Location = new System.Drawing.Point(147, 53);
            this.button_TSPL_Proof.Name = "button_TSPL_Proof";
            this.button_TSPL_Proof.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_Proof.TabIndex = 10;
            this.button_TSPL_Proof.Text = "打印快递样张";
            this.button_TSPL_Proof.UseVisualStyleBackColor = true;
            this.button_TSPL_Proof.Click += new System.EventHandler(this.button_TSPL_Proof_Click);
            // 
            // button_TSPL_State
            // 
            this.button_TSPL_State.Location = new System.Drawing.Point(6, 194);
            this.button_TSPL_State.Name = "button_TSPL_State";
            this.button_TSPL_State.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_State.TabIndex = 6;
            this.button_TSPL_State.Text = "状态查询";
            this.button_TSPL_State.UseVisualStyleBackColor = true;
            this.button_TSPL_State.Click += new System.EventHandler(this.button_TSPL_State_Click);
            // 
            // button_TSPL_QRcode
            // 
            this.button_TSPL_QRcode.Location = new System.Drawing.Point(6, 147);
            this.button_TSPL_QRcode.Name = "button_TSPL_QRcode";
            this.button_TSPL_QRcode.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_QRcode.TabIndex = 5;
            this.button_TSPL_QRcode.Text = "二维码打印";
            this.button_TSPL_QRcode.UseVisualStyleBackColor = true;
            this.button_TSPL_QRcode.Click += new System.EventHandler(this.button_TSPL_QRcode_Click);
            // 
            // button_TSPL_Barcode
            // 
            this.button_TSPL_Barcode.Location = new System.Drawing.Point(6, 100);
            this.button_TSPL_Barcode.Name = "button_TSPL_Barcode";
            this.button_TSPL_Barcode.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_Barcode.TabIndex = 4;
            this.button_TSPL_Barcode.Text = "条码打印";
            this.button_TSPL_Barcode.UseVisualStyleBackColor = true;
            this.button_TSPL_Barcode.Click += new System.EventHandler(this.button_TSPL_Barcode_Click);
            // 
            // button_TSPL_Image
            // 
            this.button_TSPL_Image.Location = new System.Drawing.Point(6, 53);
            this.button_TSPL_Image.Name = "button_TSPL_Image";
            this.button_TSPL_Image.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_Image.TabIndex = 3;
            this.button_TSPL_Image.Text = "图片打印";
            this.button_TSPL_Image.UseVisualStyleBackColor = true;
            this.button_TSPL_Image.Click += new System.EventHandler(this.button_TSPL_Image_Click);
            // 
            // button_TSPL_Text
            // 
            this.button_TSPL_Text.Location = new System.Drawing.Point(6, 6);
            this.button_TSPL_Text.Name = "button_TSPL_Text";
            this.button_TSPL_Text.Size = new System.Drawing.Size(135, 41);
            this.button_TSPL_Text.TabIndex = 2;
            this.button_TSPL_Text.Text = "文本打印";
            this.button_TSPL_Text.UseVisualStyleBackColor = true;
            this.button_TSPL_Text.Click += new System.EventHandler(this.button_TSPL_Text_Click);
            // 
            // tabPage_CPCL
            // 
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_France);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Japanese);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Datamatrix);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Pdf417);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Proof);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_State);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_QRcode);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Barcode);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Image);
            this.tabPage_CPCL.Controls.Add(this.button_CPCL_Text);
            this.tabPage_CPCL.Location = new System.Drawing.Point(4, 22);
            this.tabPage_CPCL.Name = "tabPage_CPCL";
            this.tabPage_CPCL.Size = new System.Drawing.Size(292, 271);
            this.tabPage_CPCL.TabIndex = 2;
            this.tabPage_CPCL.Text = "CPCL指令";
            this.tabPage_CPCL.UseVisualStyleBackColor = true;
            // 
            // button_CPCL_France
            // 
            this.button_CPCL_France.Location = new System.Drawing.Point(147, 53);
            this.button_CPCL_France.Name = "button_CPCL_France";
            this.button_CPCL_France.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_France.TabIndex = 13;
            this.button_CPCL_France.Text = "法文打印(UTF-8)";
            this.button_CPCL_France.UseVisualStyleBackColor = true;
            this.button_CPCL_France.Click += new System.EventHandler(this.button_CPCL_France_Click);
            // 
            // button_CPCL_Japanese
            // 
            this.button_CPCL_Japanese.Location = new System.Drawing.Point(147, 6);
            this.button_CPCL_Japanese.Name = "button_CPCL_Japanese";
            this.button_CPCL_Japanese.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Japanese.TabIndex = 12;
            this.button_CPCL_Japanese.Text = "日文打印";
            this.button_CPCL_Japanese.UseVisualStyleBackColor = true;
            this.button_CPCL_Japanese.Click += new System.EventHandler(this.button_CPCL_Japanese_Click);
            // 
            // button_CPCL_Datamatrix
            // 
            this.button_CPCL_Datamatrix.Location = new System.Drawing.Point(147, 194);
            this.button_CPCL_Datamatrix.Name = "button_CPCL_Datamatrix";
            this.button_CPCL_Datamatrix.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Datamatrix.TabIndex = 11;
            this.button_CPCL_Datamatrix.Text = "DATAMATRIX码打印";
            this.button_CPCL_Datamatrix.UseVisualStyleBackColor = true;
            this.button_CPCL_Datamatrix.Click += new System.EventHandler(this.button_CPCL_Datamatrix_Click);
            // 
            // button_CPCL_Pdf417
            // 
            this.button_CPCL_Pdf417.Location = new System.Drawing.Point(147, 147);
            this.button_CPCL_Pdf417.Name = "button_CPCL_Pdf417";
            this.button_CPCL_Pdf417.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Pdf417.TabIndex = 10;
            this.button_CPCL_Pdf417.Text = "PDF417码打印";
            this.button_CPCL_Pdf417.UseVisualStyleBackColor = true;
            this.button_CPCL_Pdf417.Click += new System.EventHandler(this.button_CPCL_Pdf417_Click);
            // 
            // button_CPCL_Proof
            // 
            this.button_CPCL_Proof.Location = new System.Drawing.Point(147, 100);
            this.button_CPCL_Proof.Name = "button_CPCL_Proof";
            this.button_CPCL_Proof.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Proof.TabIndex = 9;
            this.button_CPCL_Proof.Text = "打印快递样张";
            this.button_CPCL_Proof.UseVisualStyleBackColor = true;
            this.button_CPCL_Proof.Click += new System.EventHandler(this.button_CPCL_Proof_Click);
            // 
            // button_CPCL_State
            // 
            this.button_CPCL_State.Location = new System.Drawing.Point(6, 194);
            this.button_CPCL_State.Name = "button_CPCL_State";
            this.button_CPCL_State.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_State.TabIndex = 8;
            this.button_CPCL_State.Text = "状态查询";
            this.button_CPCL_State.UseVisualStyleBackColor = true;
            this.button_CPCL_State.Click += new System.EventHandler(this.button_CPCL_State_Click);
            // 
            // button_CPCL_QRcode
            // 
            this.button_CPCL_QRcode.Location = new System.Drawing.Point(6, 147);
            this.button_CPCL_QRcode.Name = "button_CPCL_QRcode";
            this.button_CPCL_QRcode.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_QRcode.TabIndex = 7;
            this.button_CPCL_QRcode.Text = "二维码打印";
            this.button_CPCL_QRcode.UseVisualStyleBackColor = true;
            this.button_CPCL_QRcode.Click += new System.EventHandler(this.button_CPCL_QRcode_Click);
            // 
            // button_CPCL_Barcode
            // 
            this.button_CPCL_Barcode.Location = new System.Drawing.Point(6, 100);
            this.button_CPCL_Barcode.Name = "button_CPCL_Barcode";
            this.button_CPCL_Barcode.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Barcode.TabIndex = 6;
            this.button_CPCL_Barcode.Text = "条码打印";
            this.button_CPCL_Barcode.UseVisualStyleBackColor = true;
            this.button_CPCL_Barcode.Click += new System.EventHandler(this.button_CPCL_Barcode_Click);
            // 
            // button_CPCL_Image
            // 
            this.button_CPCL_Image.Location = new System.Drawing.Point(6, 53);
            this.button_CPCL_Image.Name = "button_CPCL_Image";
            this.button_CPCL_Image.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Image.TabIndex = 5;
            this.button_CPCL_Image.Text = "图片打印";
            this.button_CPCL_Image.UseVisualStyleBackColor = true;
            this.button_CPCL_Image.Click += new System.EventHandler(this.button_CPCL_Image_Click);
            // 
            // button_CPCL_Text
            // 
            this.button_CPCL_Text.Location = new System.Drawing.Point(6, 6);
            this.button_CPCL_Text.Name = "button_CPCL_Text";
            this.button_CPCL_Text.Size = new System.Drawing.Size(135, 41);
            this.button_CPCL_Text.TabIndex = 4;
            this.button_CPCL_Text.Text = "文本打印";
            this.button_CPCL_Text.UseVisualStyleBackColor = true;
            this.button_CPCL_Text.Click += new System.EventHandler(this.button_CPCL_Text_Click);
            // 
            // tb_USB_state
            // 
            this.tb_USB_state.Location = new System.Drawing.Point(309, 76);
            this.tb_USB_state.Multiline = true;
            this.tb_USB_state.Name = "tb_USB_state";
            this.tb_USB_state.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_USB_state.Size = new System.Drawing.Size(184, 246);
            this.tb_USB_state.TabIndex = 360;
            // 
            // button_clean
            // 
            this.button_clean.Location = new System.Drawing.Point(418, 328);
            this.button_clean.Name = "button_clean";
            this.button_clean.Size = new System.Drawing.Size(75, 23);
            this.button_clean.TabIndex = 361;
            this.button_clean.Text = "清空";
            this.button_clean.UseVisualStyleBackColor = true;
            this.button_clean.Click += new System.EventHandler(this.button_clean_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 362;
            this.label1.Text = "USB返回值：";
            // 
            // button_ESC_Codepage
            // 
            this.button_ESC_Codepage.Location = new System.Drawing.Point(147, 53);
            this.button_ESC_Codepage.Name = "button_ESC_Codepage";
            this.button_ESC_Codepage.Size = new System.Drawing.Size(135, 41);
            this.button_ESC_Codepage.TabIndex = 6;
            this.button_ESC_Codepage.Text = "代码页打印";
            this.button_ESC_Codepage.UseVisualStyleBackColor = true;
            this.button_ESC_Codepage.Click += new System.EventHandler(this.button_ESC_Codepage_Click);
            // 
            // UserControl_USB_Connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_clean);
            this.Controls.Add(this.tb_USB_state);
            this.Controls.Add(this.tabControl1);
            this.Name = "UserControl_USB_Connect";
            this.Size = new System.Drawing.Size(505, 354);
            this.ParentChanged += new System.EventHandler(this.SmarnetDEMO_ParentChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_ESC.ResumeLayout(false);
            this.tabPage_TSPL.ResumeLayout(false);
            this.tabPage_CPCL.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_ESC;
        private System.Windows.Forms.TabPage tabPage_TSPL;
        private System.Windows.Forms.TabPage tabPage_CPCL;
        private System.Windows.Forms.Button button_ESC_Image;
        private System.Windows.Forms.Button button_ESC_Text;
        private System.Windows.Forms.Button button_TSPL_Image;
        private System.Windows.Forms.Button button_TSPL_Text;
        private System.Windows.Forms.Button button_CPCL_Image;
        private System.Windows.Forms.Button button_CPCL_Text;
        private System.Windows.Forms.Button button_ESC_Barcode;
        private System.Windows.Forms.Button button_TSPL_Barcode;
        private System.Windows.Forms.Button button_CPCL_Barcode;
        private System.Windows.Forms.Button button_ESC_QRcode;
        private System.Windows.Forms.Button button_TSPL_QRcode;
        private System.Windows.Forms.Button button_CPCL_QRcode;
        private System.Windows.Forms.TextBox tb_USB_state;
        private System.Windows.Forms.Button button_clean;
        private System.Windows.Forms.Button button_ESC_State;
        private System.Windows.Forms.Button button_TSPL_State;
        private System.Windows.Forms.Button button_CPCL_State;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_CPCL_Proof;
        private System.Windows.Forms.Button button_TSPL_Proof;
        private System.Windows.Forms.Button button_CPCL_Pdf417;
        private System.Windows.Forms.Button button_CPCL_Datamatrix;
        private System.Windows.Forms.Button button_ESC_Japanese;
        private System.Windows.Forms.Button button_CPCL_Japanese;
        private System.Windows.Forms.Button button_TSPL_Japanese;
        private System.Windows.Forms.Button button_CPCL_France;
        private System.Windows.Forms.Button button_ESC_Codepage;
    }
}
