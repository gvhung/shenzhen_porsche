namespace Workshop
{
    partial class frmCarStateBoad
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCarStateBoad));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.labDateTime = new System.Windows.Forms.Label();
            this.txtCarNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMonth = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.pn0 = new System.Windows.Forms.Panel();
            this.pic0Up = new System.Windows.Forms.PictureBox();
            this.pn1 = new System.Windows.Forms.Panel();
            this.pic1Down = new System.Windows.Forms.PictureBox();
            this.pic1Up = new System.Windows.Forms.PictureBox();
            this.pn2 = new System.Windows.Forms.Panel();
            this.pic2Down = new System.Windows.Forms.PictureBox();
            this.pic2Up = new System.Windows.Forms.PictureBox();
            this.pn3 = new System.Windows.Forms.Panel();
            this.pic3Down = new System.Windows.Forms.PictureBox();
            this.pic3Up = new System.Windows.Forms.PictureBox();
            this.pn4 = new System.Windows.Forms.Panel();
            this.pic4Down = new System.Windows.Forms.PictureBox();
            this.pic4Up = new System.Windows.Forms.PictureBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.追加项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改状态ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pic0Down = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cmbReceiver = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.返修ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pn0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic0Up)).BeginInit();
            this.pn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic1Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1Up)).BeginInit();
            this.pn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic2Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2Up)).BeginInit();
            this.pn3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic3Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3Up)).BeginInit();
            this.pn4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic4Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4Up)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic0Down)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Black;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 30;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(1, 130);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Transparent;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.Size = new System.Drawing.Size(1164, 246);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridView1_Paint);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "等待";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "维修进行中";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "中断";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "完工";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "洗车";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.ContextMenuStrip = this.contextMenuStrip1;
            this.panel1.Controls.Add(this.cmbReceiver);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labDateTime);
            this.panel1.Controls.Add(this.txtCarNo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnMonth);
            this.panel1.Controls.Add(this.btnToday);
            this.panel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(1, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1163, 48);
            this.panel1.TabIndex = 13;
            // 
            // labDateTime
            // 
            this.labDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labDateTime.AutoSize = true;
            this.labDateTime.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDateTime.ForeColor = System.Drawing.Color.White;
            this.labDateTime.Location = new System.Drawing.Point(989, 15);
            this.labDateTime.Name = "labDateTime";
            this.labDateTime.Size = new System.Drawing.Size(164, 21);
            this.labDateTime.TabIndex = 25;
            this.labDateTime.Text = "2009年05月21日";
            // 
            // txtCarNo
            // 
            this.txtCarNo.Location = new System.Drawing.Point(920, 12);
            this.txtCarNo.Name = "txtCarNo";
            this.txtCarNo.Size = new System.Drawing.Size(66, 26);
            this.txtCarNo.TabIndex = 24;
            this.txtCarNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCarNo_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(825, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 23;
            this.label3.Text = "查找车牌号:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRefresh.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(228, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 35);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(543, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(114, 24);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(447, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "中断原因:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExit.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(318, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 35);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMonth
            // 
            this.btnMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMonth.Font = new System.Drawing.Font("幼圆", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMonth.ForeColor = System.Drawing.Color.White;
            this.btnMonth.Location = new System.Drawing.Point(122, 5);
            this.btnMonth.Name = "btnMonth";
            this.btnMonth.Size = new System.Drawing.Size(100, 35);
            this.btnMonth.TabIndex = 10;
            this.btnMonth.Text = "车间运作";
            this.btnMonth.UseVisualStyleBackColor = false;
            this.btnMonth.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnToday.Font = new System.Drawing.Font("幼圆", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToday.ForeColor = System.Drawing.Color.White;
            this.btnToday.Location = new System.Drawing.Point(14, 5);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(101, 35);
            this.btnToday.TabIndex = 8;
            this.btnToday.Text = "预约管理";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // pn0
            // 
            this.pn0.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pn0.Controls.Add(this.pic0Up);
            this.pn0.Location = new System.Drawing.Point(32, 378);
            this.pn0.Name = "pn0";
            this.pn0.Size = new System.Drawing.Size(141, 28);
            this.pn0.TabIndex = 14;
            // 
            // pic0Up
            // 
            this.pic0Up.Image = ((System.Drawing.Image)(resources.GetObject("pic0Up.Image")));
            this.pic0Up.Location = new System.Drawing.Point(23, 0);
            this.pic0Up.Name = "pic0Up";
            this.pic0Up.Size = new System.Drawing.Size(37, 32);
            this.pic0Up.TabIndex = 0;
            this.pic0Up.TabStop = false;
            this.pic0Up.Click += new System.EventHandler(this.pic0Up_Click);
            this.pic0Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic0Up_MouseDown);
            this.pic0Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pic0Up_MouseUp);
            // 
            // pn1
            // 
            this.pn1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pn1.Controls.Add(this.pic1Down);
            this.pn1.Controls.Add(this.pic1Up);
            this.pn1.Location = new System.Drawing.Point(262, 378);
            this.pn1.Name = "pn1";
            this.pn1.Size = new System.Drawing.Size(141, 28);
            this.pn1.TabIndex = 15;
            // 
            // pic1Down
            // 
            this.pic1Down.Image = global::Workshop.Properties.Resources.down1;
            this.pic1Down.Location = new System.Drawing.Point(74, 1);
            this.pic1Down.Name = "pic1Down";
            this.pic1Down.Size = new System.Drawing.Size(35, 32);
            this.pic1Down.TabIndex = 1;
            this.pic1Down.TabStop = false;
            this.pic1Down.Click += new System.EventHandler(this.pic0Down_Click);
            // 
            // pic1Up
            // 
            this.pic1Up.Image = global::Workshop.Properties.Resources.up1;
            this.pic1Up.Location = new System.Drawing.Point(23, 0);
            this.pic1Up.Name = "pic1Up";
            this.pic1Up.Size = new System.Drawing.Size(37, 32);
            this.pic1Up.TabIndex = 0;
            this.pic1Up.TabStop = false;
            this.pic1Up.Click += new System.EventHandler(this.pic0Up_Click);
            // 
            // pn2
            // 
            this.pn2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pn2.Controls.Add(this.pic2Down);
            this.pn2.Controls.Add(this.pic2Up);
            this.pn2.Location = new System.Drawing.Point(487, 378);
            this.pn2.Name = "pn2";
            this.pn2.Size = new System.Drawing.Size(141, 28);
            this.pn2.TabIndex = 13;
            // 
            // pic2Down
            // 
            this.pic2Down.Image = global::Workshop.Properties.Resources.down1;
            this.pic2Down.Location = new System.Drawing.Point(74, 1);
            this.pic2Down.Name = "pic2Down";
            this.pic2Down.Size = new System.Drawing.Size(35, 32);
            this.pic2Down.TabIndex = 1;
            this.pic2Down.TabStop = false;
            this.pic2Down.Click += new System.EventHandler(this.pic0Down_Click);
            // 
            // pic2Up
            // 
            this.pic2Up.Image = global::Workshop.Properties.Resources.up1;
            this.pic2Up.Location = new System.Drawing.Point(23, 0);
            this.pic2Up.Name = "pic2Up";
            this.pic2Up.Size = new System.Drawing.Size(37, 32);
            this.pic2Up.TabIndex = 0;
            this.pic2Up.TabStop = false;
            this.pic2Up.Click += new System.EventHandler(this.pic0Up_Click);
            // 
            // pn3
            // 
            this.pn3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pn3.Controls.Add(this.pic3Down);
            this.pn3.Controls.Add(this.pic3Up);
            this.pn3.Location = new System.Drawing.Point(737, 378);
            this.pn3.Name = "pn3";
            this.pn3.Size = new System.Drawing.Size(141, 28);
            this.pn3.TabIndex = 13;
            // 
            // pic3Down
            // 
            this.pic3Down.Image = global::Workshop.Properties.Resources.down1;
            this.pic3Down.Location = new System.Drawing.Point(74, 1);
            this.pic3Down.Name = "pic3Down";
            this.pic3Down.Size = new System.Drawing.Size(35, 32);
            this.pic3Down.TabIndex = 1;
            this.pic3Down.TabStop = false;
            this.pic3Down.Click += new System.EventHandler(this.pic0Down_Click);
            // 
            // pic3Up
            // 
            this.pic3Up.Image = global::Workshop.Properties.Resources.up1;
            this.pic3Up.Location = new System.Drawing.Point(23, 1);
            this.pic3Up.Name = "pic3Up";
            this.pic3Up.Size = new System.Drawing.Size(37, 32);
            this.pic3Up.TabIndex = 0;
            this.pic3Up.TabStop = false;
            this.pic3Up.Click += new System.EventHandler(this.pic0Up_Click);
            // 
            // pn4
            // 
            this.pn4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pn4.Controls.Add(this.pic4Down);
            this.pn4.Controls.Add(this.pic4Up);
            this.pn4.Location = new System.Drawing.Point(958, 378);
            this.pn4.Name = "pn4";
            this.pn4.Size = new System.Drawing.Size(141, 28);
            this.pn4.TabIndex = 13;
            // 
            // pic4Down
            // 
            this.pic4Down.Image = global::Workshop.Properties.Resources.down1;
            this.pic4Down.Location = new System.Drawing.Point(74, 1);
            this.pic4Down.Name = "pic4Down";
            this.pic4Down.Size = new System.Drawing.Size(35, 32);
            this.pic4Down.TabIndex = 1;
            this.pic4Down.TabStop = false;
            this.pic4Down.Click += new System.EventHandler(this.pic0Down_Click);
            // 
            // pic4Up
            // 
            this.pic4Up.Image = global::Workshop.Properties.Resources.up1;
            this.pic4Up.Location = new System.Drawing.Point(23, 1);
            this.pic4Up.Name = "pic4Up";
            this.pic4Up.Size = new System.Drawing.Size(37, 32);
            this.pic4Up.TabIndex = 0;
            this.pic4Up.TabStop = false;
            this.pic4Up.Click += new System.EventHandler(this.pic0Up_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 200;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 200;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.追加项目ToolStripMenuItem,
            this.返修ToolStripMenuItem,
            this.修改状态ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 92);
            // 
            // 追加项目ToolStripMenuItem
            // 
            this.追加项目ToolStripMenuItem.Image = global::Workshop.Properties.Resources._15_01_;
            this.追加项目ToolStripMenuItem.Name = "追加项目ToolStripMenuItem";
            this.追加项目ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.追加项目ToolStripMenuItem.Text = "追加项目";
            this.追加项目ToolStripMenuItem.Click += new System.EventHandler(this.追加项目ToolStripMenuItem_Click);
            // 
            // 修改状态ToolStripMenuItem
            // 
            this.修改状态ToolStripMenuItem.Image = global::Workshop.Properties.Resources._00_40_;
            this.修改状态ToolStripMenuItem.Name = "修改状态ToolStripMenuItem";
            this.修改状态ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.修改状态ToolStripMenuItem.Text = "修改状态";
            this.修改状态ToolStripMenuItem.Click += new System.EventHandler(this.修改状态ToolStripMenuItem_Click);
            // 
            // pic0Down
            // 
            this.pic0Down.Image = ((System.Drawing.Image)(resources.GetObject("pic0Down.Image")));
            this.pic0Down.InitialImage = ((System.Drawing.Image)(resources.GetObject("pic0Down.InitialImage")));
            this.pic0Down.Location = new System.Drawing.Point(74, 378);
            this.pic0Down.Name = "pic0Down";
            this.pic0Down.Size = new System.Drawing.Size(35, 28);
            this.pic0Down.TabIndex = 1;
            this.pic0Down.TabStop = false;
            this.pic0Down.Click += new System.EventHandler(this.pic0Down_Click);
            this.pic0Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic0Down_MouseDown);
            this.pic0Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pic0Down_MouseUp);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Location = new System.Drawing.Point(1, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1164, 78);
            this.panel4.TabIndex = 12;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = global::Workshop.Properties.Resources._16_01_;
            this.pictureBox2.Location = new System.Drawing.Point(1144, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(17, 16);
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // cmbReceiver
            // 
            this.cmbReceiver.FormattingEnabled = true;
            this.cmbReceiver.Location = new System.Drawing.Point(741, 13);
            this.cmbReceiver.Name = "cmbReceiver";
            this.cmbReceiver.Size = new System.Drawing.Size(85, 24);
            this.cmbReceiver.TabIndex = 27;
            this.cmbReceiver.TextChanged += new System.EventHandler(this.cmbReceiver_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(659, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 26;
            this.label2.Text = "接车人:";
            // 
            // 返修ToolStripMenuItem
            // 
            this.返修ToolStripMenuItem.Image = global::Workshop.Properties.Resources._00_34_;
            this.返修ToolStripMenuItem.Name = "返修ToolStripMenuItem";
            this.返修ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.返修ToolStripMenuItem.Text = "返修";
            this.返修ToolStripMenuItem.Click += new System.EventHandler(this.返修ToolStripMenuItem_Click);
            // 
            // frmCarStateBoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1167, 407);
            this.Controls.Add(this.pic0Down);
            this.Controls.Add(this.pn4);
            this.Controls.Add(this.pn3);
            this.Controls.Add(this.pn2);
            this.Controls.Add(this.pn1);
            this.Controls.Add(this.pn0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCarStateBoad";
            this.Text = "车辆状态";
            this.Load += new System.EventHandler(this.frmCarStateBoad_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pn0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic0Up)).EndInit();
            this.pn1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic1Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1Up)).EndInit();
            this.pn2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic2Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2Up)).EndInit();
            this.pn3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic3Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3Up)).EndInit();
            this.pn4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic4Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4Up)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic0Down)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Panel pn0;
        private System.Windows.Forms.PictureBox pic0Down;
        private System.Windows.Forms.PictureBox pic0Up;
        private System.Windows.Forms.Panel pn1;
        private System.Windows.Forms.PictureBox pic1Down;
        private System.Windows.Forms.PictureBox pic1Up;
        private System.Windows.Forms.Panel pn2;
        private System.Windows.Forms.PictureBox pic2Down;
        private System.Windows.Forms.PictureBox pic2Up;
        private System.Windows.Forms.Panel pn3;
        private System.Windows.Forms.PictureBox pic3Down;
        private System.Windows.Forms.PictureBox pic3Up;
        private System.Windows.Forms.Panel pn4;
        private System.Windows.Forms.PictureBox pic4Down;
        private System.Windows.Forms.PictureBox pic4Up;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 追加项目ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改状态ToolStripMenuItem;
        private System.Windows.Forms.TextBox txtCarNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labDateTime;
        private System.Windows.Forms.ComboBox cmbReceiver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem 返修ToolStripMenuItem;
    }
}