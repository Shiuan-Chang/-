namespace 記帳系統.Forms
{
    partial class AddForm
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
            this.DateBox = new System.Windows.Forms.DateTimePicker();
            this.Date = new System.Windows.Forms.Label();
            this.AccountName = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.Label();
            this.Detail = new System.Windows.Forms.Label();
            this.AmountBox = new System.Windows.Forms.TextBox();
            this.AccountType = new System.Windows.Forms.Label();
            this.AccountTypeBox = new System.Windows.Forms.ComboBox();
            this.DetailBox = new System.Windows.Forms.TextBox();
            this.PaymentMethod = new System.Windows.Forms.Label();
            this.PaymentBox = new System.Windows.Forms.ComboBox();
            this.AccountNameBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.navBar1 = new 記帳系統.Components.NavBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // DateBox
            // 
            this.DateBox.Location = new System.Drawing.Point(45, 60);
            this.DateBox.Name = "DateBox";
            this.DateBox.Size = new System.Drawing.Size(190, 20);
            this.DateBox.TabIndex = 1;
            this.DateBox.Tag = "DateBox";
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Date.Location = new System.Drawing.Point(41, 18);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(48, 24);
            this.Date.TabIndex = 2;
            this.Date.Tag = "Date";
            this.Date.Text = "日期";
            // 
            // AccountName
            // 
            this.AccountName.AutoSize = true;
            this.AccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountName.Location = new System.Drawing.Point(41, 99);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(86, 24);
            this.AccountName.TabIndex = 3;
            this.AccountName.Tag = "AccountName";
            this.AccountName.Text = "帳目名稱";
            this.AccountName.Click += new System.EventHandler(this.AccountDetail_Click);
            // 
            // Amount
            // 
            this.Amount.AutoSize = true;
            this.Amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Amount.Location = new System.Drawing.Point(41, 436);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(48, 24);
            this.Amount.TabIndex = 4;
            this.Amount.Tag = "Account";
            this.Amount.Text = "金額";
            // 
            // Detail
            // 
            this.Detail.AutoSize = true;
            this.Detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.Location = new System.Drawing.Point(41, 266);
            this.Detail.Name = "Detail";
            this.Detail.Size = new System.Drawing.Size(48, 24);
            this.Detail.TabIndex = 5;
            this.Detail.Tag = "Detail";
            this.Detail.Text = "用途";
            // 
            // AmountBox
            // 
            this.AmountBox.Location = new System.Drawing.Point(45, 475);
            this.AmountBox.Name = "AmountBox";
            this.AmountBox.Size = new System.Drawing.Size(125, 20);
            this.AmountBox.TabIndex = 8;
            this.AmountBox.Tag = "AmountBox";
            // 
            // AccountType
            // 
            this.AccountType.AutoSize = true;
            this.AccountType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountType.Location = new System.Drawing.Point(41, 182);
            this.AccountType.Name = "AccountType";
            this.AccountType.Size = new System.Drawing.Size(86, 24);
            this.AccountType.TabIndex = 10;
            this.AccountType.Tag = "AccountType";
            this.AccountType.Text = "帳目類型";
            // 
            // AccountTypeBox
            // 
            this.AccountTypeBox.FormattingEnabled = true;
            this.AccountTypeBox.Items.AddRange(new object[] {
            "餐費",
            "交通",
            "租金",
            "娛樂",
            "學習",
            "投資"});
            this.AccountTypeBox.Location = new System.Drawing.Point(45, 221);
            this.AccountTypeBox.Name = "AccountTypeBox";
            this.AccountTypeBox.Size = new System.Drawing.Size(121, 21);
            this.AccountTypeBox.TabIndex = 11;
            this.AccountTypeBox.Tag = "AccountTypeBox";
            // 
            // DetailBox
            // 
            this.DetailBox.Location = new System.Drawing.Point(45, 303);
            this.DetailBox.Name = "DetailBox";
            this.DetailBox.Size = new System.Drawing.Size(128, 20);
            this.DetailBox.TabIndex = 12;
            this.DetailBox.Tag = "DetailBox";
            // 
            // PaymentMethod
            // 
            this.PaymentMethod.AutoSize = true;
            this.PaymentMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PaymentMethod.Location = new System.Drawing.Point(41, 350);
            this.PaymentMethod.Name = "PaymentMethod";
            this.PaymentMethod.Size = new System.Drawing.Size(86, 24);
            this.PaymentMethod.TabIndex = 13;
            this.PaymentMethod.Tag = "PaymentMethod";
            this.PaymentMethod.Text = "支付方式";
            // 
            // PaymentBox
            // 
            this.PaymentBox.FormattingEnabled = true;
            this.PaymentBox.Items.AddRange(new object[] {
            "現金",
            "網銀轉帳",
            "信用卡"});
            this.PaymentBox.Location = new System.Drawing.Point(45, 389);
            this.PaymentBox.Name = "PaymentBox";
            this.PaymentBox.Size = new System.Drawing.Size(121, 21);
            this.PaymentBox.TabIndex = 14;
            this.PaymentBox.Tag = "PaymentBox";
            // 
            // AccountNameBox
            // 
            this.AccountNameBox.FormattingEnabled = true;
            this.AccountNameBox.Items.AddRange(new object[] {
            "餐費",
            "交通",
            "租金",
            "娛樂",
            "學習",
            "投資"});
            this.AccountNameBox.Location = new System.Drawing.Point(45, 141);
            this.AccountNameBox.Name = "AccountNameBox";
            this.AccountNameBox.Size = new System.Drawing.Size(121, 21);
            this.AccountNameBox.TabIndex = 15;
            this.AccountNameBox.Tag = "AccountNameBox";
            this.AccountNameBox.SelectedIndexChanged += new System.EventHandler(this.AccountNameBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(666, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 41);
            this.button1.TabIndex = 16;
            this.button1.Text = "送出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(235, 155);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(247, 320);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.UploadFile);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(231, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 24);
            this.label1.TabIndex = 18;
            this.label1.Tag = "AccountName";
            this.label1.Text = "發票/收據";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(519, 155);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(247, 320);
            this.pictureBox2.TabIndex = 19;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.UploadFile);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(666, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 41);
            this.button2.TabIndex = 20;
            this.button2.Text = "讀取";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // navBar1
            // 
            this.navBar1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navBar1.Location = new System.Drawing.Point(106, 533);
            this.navBar1.Name = "navBar1";
            this.navBar1.Size = new System.Drawing.Size(550, 118);
            this.navBar1.TabIndex = 0;
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 663);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AccountNameBox);
            this.Controls.Add(this.PaymentBox);
            this.Controls.Add(this.PaymentMethod);
            this.Controls.Add(this.DetailBox);
            this.Controls.Add(this.AccountTypeBox);
            this.Controls.Add(this.AccountType);
            this.Controls.Add(this.AmountBox);
            this.Controls.Add(this.Detail);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.AccountName);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.DateBox);
            this.Controls.Add(this.navBar1);
            this.Name = "AddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "AddForm";
            this.Text = "增一筆";
            this.Load += new System.EventHandler(this.AddForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.NavBar navBar1;
        private System.Windows.Forms.DateTimePicker DateBox;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.Label AccountName;
        private System.Windows.Forms.Label Amount;
        private System.Windows.Forms.Label Detail;
        private System.Windows.Forms.TextBox AmountBox;
        private System.Windows.Forms.Label AccountType;
        private System.Windows.Forms.ComboBox AccountTypeBox;
        private System.Windows.Forms.TextBox DetailBox;
        private System.Windows.Forms.Label PaymentMethod;
        private System.Windows.Forms.ComboBox PaymentBox;
        private System.Windows.Forms.ComboBox AccountNameBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
    }
}