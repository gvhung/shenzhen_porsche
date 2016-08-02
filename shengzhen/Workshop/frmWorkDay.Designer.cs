namespace Workshop
{
    partial class frmWorkDay
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全天ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.半天ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.休假ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWorkerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWorkDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.ColWorkerCode,
            this.Column4,
            this.ColWorkDay,
            this.Column5});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(13, 78);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(618, 282);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全天ToolStripMenuItem,
            this.半天ToolStripMenuItem,
            this.休假ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 70);
            // 
            // 全天ToolStripMenuItem
            // 
            this.全天ToolStripMenuItem.Name = "全天ToolStripMenuItem";
            this.全天ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.全天ToolStripMenuItem.Text = "全天";
            this.全天ToolStripMenuItem.Click += new System.EventHandler(this.全天ToolStripMenuItem_Click);
            // 
            // 半天ToolStripMenuItem
            // 
            this.半天ToolStripMenuItem.Name = "半天ToolStripMenuItem";
            this.半天ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.半天ToolStripMenuItem.Text = "半天";
            this.半天ToolStripMenuItem.Click += new System.EventHandler(this.半天ToolStripMenuItem_Click);
            // 
            // 休假ToolStripMenuItem
            // 
            this.休假ToolStripMenuItem.Name = "休假ToolStripMenuItem";
            this.休假ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.休假ToolStripMenuItem.Text = "休假";
            this.休假ToolStripMenuItem.Click += new System.EventHandler(this.休假ToolStripMenuItem_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(118, 33);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(113, 26);
            this.dateTimePicker1.TabIndex = 15;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "选择日期:";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "WorkerGroup";
            this.Column1.FillWeight = 80F;
            this.Column1.HeaderText = "班组";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Position";
            this.Column2.FillWeight = 70F;
            this.Column2.HeaderText = "职务";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // ColWorkerCode
            // 
            this.ColWorkerCode.DataPropertyName = "WorkerCode";
            this.ColWorkerCode.FillWeight = 70F;
            this.ColWorkerCode.HeaderText = "工号";
            this.ColWorkerCode.Name = "ColWorkerCode";
            this.ColWorkerCode.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "WorkerName";
            this.Column4.FillWeight = 70F;
            this.Column4.HeaderText = "姓名";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // ColWorkDay
            // 
            this.ColWorkDay.DataPropertyName = "WorkDay";
            this.ColWorkDay.HeaderText = "工作时间(小时)";
            this.ColWorkDay.Name = "ColWorkDay";
            this.ColWorkDay.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "HourRate";
            this.Column5.FillWeight = 70F;
            this.Column5.HeaderText = "工时系数";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // frmWorkDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(644, 377);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmWorkDay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工人排班";
            this.Load += new System.EventHandler(this.frmWorkDay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全天ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 半天ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 休假ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWorkerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWorkDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}