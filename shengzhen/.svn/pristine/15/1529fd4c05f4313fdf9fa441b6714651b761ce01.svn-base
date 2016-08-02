using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Workshop
{
    public partial class frmSysSet : Form
    {
        public frmSysSet()
        {
            InitializeComponent();
        }
        private SqlDataAdapter Myda = new SqlDataAdapter();
        private DataTable DtPlan;
        string sqlstring = string.Empty;
        private bool updateTreeNode = false;
        private int rowindex = -1;
        private int colindex = -1;
        bool IsEdit = false;
        TreeNode SelectNode;
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (txtUserID.Text == string.Empty || txtUserName.Text == string.Empty)
            {
                MessageBox.Show("用户名或用户密码不能为空！");
                return;
            }
            sqlstring = "Select Count(*) from SysUser where UserID='"+ txtUserID.Text +"'";
            if (int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString()) > 0)
            {
                MessageBox.Show(txtUserID.Text + "已经存在！");
                return;
            }
            sqlstring = "Insert into SysUser(UserID,UserName,Pwd)values('"+ txtUserID.Text +"','"+ txtUserName.Text +"','"+ txtPwd.Text +"')";
            if (!txtUserID.Enabled)
            {
                sqlstring = "Update SysUser Set UserName='"+ txtUserName.Text +"',Pwd='"+ txtPwd.Text +"' Where UserID='"+ txtUserID.Text +"'";
            }
            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
            {
                MessageBox.Show("新增用户成功！");
                TreeNode tn = new TreeNode();
                tn.Text = txtUserName.Text + "(" + txtUserID.Text + ")";
                tn.Tag = txtUserID.Text;
                treeView1.Nodes[0].Nodes.Add(tn);
                txtPwd.Text = string.Empty;
                txtUserID.Text = string.Empty;
                txtUserName.Text = string.Empty;
            }
        }
        private void LoadTree()
        {
            sqlstring = "Select * from SysUser";
            treeView1.Nodes[0].Nodes.Clear();
            DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
            foreach (DataRow dr in Dt.Rows)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr["UserName"].ToString() + "(" + dr["UserID"].ToString() + ")";
                tn.Tag = dr["UserID"].ToString();
                treeView1.Nodes[0].Nodes.Add(tn);
            }
            treeView1.Nodes[0].Expand();


            sqlstring = "Select * from SysFunction";
            treeView2.Nodes[0].Nodes.Clear();
            Dt = SQLDbHelper.Query(sqlstring).Tables[0];
            foreach (DataRow dr in Dt.Select("FunParentID=-1"))
            {
                TreeNode tn = new TreeNode();
                tn.Text = dr["FunName"].ToString();
                tn.Tag = dr["ID"].ToString();
                foreach (DataRow drc in Dt.Select("FunParentID=" + int.Parse(dr["ID"].ToString())))
                {
                    TreeNode tnc = new TreeNode();
                    tnc.Text = drc["FunName"].ToString();
                    tnc.Tag = drc["ID"].ToString();
                    tn.Nodes.Add(tnc);
                }
                treeView2.Nodes[0].Nodes.Add(tn);
            }
            treeView2.Nodes[0].Expand();
        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count == 0)
            {
                sqlstring = "Delete from SysUser where UserID='" + treeView1.SelectedNode.Tag.ToString() + "'";
                sqlstring += ";Delete from SysPower Where UserID='" + treeView1.SelectedNode.Tag.ToString() + "'";
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    treeView1.Nodes[0].Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void treeView2_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (updateTreeNode) return; // 如果在刷新中
            updateTreeNode = true;
            try
            {
                TreeNodeChild(e.Node, e.Node.Checked); // 同步子节点
                TreeNodeCheck(e.Node, e.Node.Checked);
            }
            finally
            {
                updateTreeNode = false;
            }
        }
        private void TreeNodeCheck(TreeNode ATreeNode, bool AChecked)
        {
            if (ATreeNode == null) return;
            ATreeNode.Checked = AChecked;
            if (AChecked) // 如果选中本节点这就是选中全部上级节点
            {
                TreeNodeCheck(ATreeNode.Parent, AChecked);
            }
            else
            {
                if (ATreeNode.Parent != null && ATreeNode.Parent.Checked)
                {
                    foreach(TreeNode vTreeNode in ATreeNode.Parent.Nodes)
                        if (vTreeNode.Checked) return;
                    TreeNodeCheck(ATreeNode.Parent, AChecked); // 判断是否所有的兄弟节点Checked都为false
                }
            }
        }
        private void TreeNodeChild(TreeNode ATreeNode, bool AChecked) // 同步子节点
        {
            if (ATreeNode == null) return;
            ATreeNode.Checked = AChecked;
            foreach (TreeNode vTreeNode in ATreeNode.Nodes)
                TreeNodeChild(vTreeNode, AChecked);
        }

        private void frmSysSet_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            LoadTree();
            LoadGridView1();
            if (!ClsBLL.IsPower("用户管理"))
            {
                btnUserSet.Enabled = false;
            }
            if (!ClsBLL.IsPower("词典定义"))
            {
                btnSysDictionary.Enabled = false;
            }
            if (!ClsBLL.IsPower("参数设置"))
            {
                btnSet.Enabled = false;
            }
            if (!ClsBLL.IsPower("新增用户"))
            {
                btnSaveUser.Enabled = false;
            }
            if (!ClsBLL.IsPower("删除用户"))
            {
                删除用户ToolStripMenuItem.Enabled = false;
            }
            if (!ClsBLL.IsPower("新增工人"))
            {
                btnNewWorker.Enabled = false;
            }
            if (!ClsBLL.IsPower("工作排班"))
            {
                btnWorkerSche.Enabled = false;
                dataGridView1.ReadOnly = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsEdit)
                {
                    ClsBLL.SaveItem(treeView3);
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message + Err.StackTrace.Substring(Err.StackTrace.IndexOf("行号")));
            }
        }

        private void LoadSysDictionary()
        {
            try
            {
                treeView3.Nodes[0].Nodes.Clear();
                DataTable dt = SQLDbHelper.Query("Select * from SysDictionary").Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode[] tns = treeView3.Nodes.Find(dt.Rows[i][0].ToString(), true);
                    if (tns.Length > 0)
                    {
                        TreeNode tn = tns[0];
                        tn.Nodes.Add(dt.Rows[i][1].ToString());
                    }
                    else
                    {
                        TreeNode tn = new TreeNode();
                        tn.Text = dt.Rows[i][0].ToString();
                        tn.Name = dt.Rows[i][0].ToString();
                        tn.Nodes.Add(dt.Rows[i][1].ToString());
                        treeView3.Nodes[0].Nodes.Add(tn);
                    }
                }
                treeView3.Nodes[0].Expand();

            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectNode != null && SelectNode.Level < 2)
                {
                    TreeNode Tn = new TreeNode();
                    Tn.Text = "新项目";
                    SelectNode.Nodes.Add(Tn);

                    SelectNode.Expand();
                    Tn.EndEdit(true);
                    Tn.BeginEdit();
                    IsEdit = true;
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectNode != null)
                {
                    //添加权限处理
                    if (SelectNode.Text != "全部")
                    {
                        if (SelectNode.Nodes.Count == 0)
                        {
                            if (SelectNode.Level != 0)
                            {
                                treeView3.Nodes.Remove(SelectNode);
                                IsEdit = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("该信息下面有其他信息，\n请删除相关信息在删除该信息");
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void btnWorkerSche_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void btnUserSet_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void btnSysDictionary_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            LoadSysDictionary();
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            LoadSysSet();
        }
        private void LoadSysSet()
        {
            DataTable DtSet = SQLDbHelper.Query("Select * from Setting").Tables[0];
            foreach (Control ct in tabPage3.Controls)
            {
                if (ct.Name.StartsWith("txtSet"))
                {
                    DataRow[] Drs = DtSet.Select("KeyWord='" + ct.Name + "'");
                    if (Drs.Length > 0)
                    {
                        ct.Text = Drs[0]["KeyWord_Value"].ToString();
                    }
                }
            }
        }
        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                SelectNode = e.Node;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlstring = string.Empty;
            string wherestr = "'ComeType'";
            sqlstring = "Insert Into Setting(KeyWord,KeyWord_Value) Values('ComeType','" + Convert.ToInt16(chkComeType.Checked).ToString() + "')";
            foreach (Control ct in tabPage3.Controls)
            {
                if (ct.Name.StartsWith("txtSet"))
                {
                    wherestr += ",'" + ct.Name + "'";
                    string val = ct.Text;
                    if (val == string.Empty) val = "0";
                    sqlstring += ";Insert Into Setting(KeyWord,KeyWord_Value) Values('" + ct.Name + "','" + val + "')";
                }
            }
            if (sqlstring.Length > 1)
            {
                sqlstring = "Delete from Setting where KeyWord in (" + wherestr + ");" + sqlstring;
                try
                {
                    SQLDbHelper.ExecuteSql(sqlstring);
                    MessageBox.Show("保存成功！");
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }

        private void btnSavePower_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag != null)
            {
                string userid = treeView1.SelectedNode.Tag.ToString();
                string sqlstring = "Delete from SysPower where UserID='" + userid + "'";
                bool chilrencheck = false;
                for (int i = 0; i < treeView2.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < treeView2.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        if (treeView2.Nodes[0].Nodes[i].Nodes[j].Checked)
                        {
                            sqlstring += ";Insert Into SysPower(UserID,FunID)values('" + userid + "'," + int.Parse(treeView2.Nodes[0].Nodes[i].Nodes[j].Tag.ToString()) + ")";
                            chilrencheck = true;
                        }
                    }
                    if (chilrencheck)
                    {
                        sqlstring += ";Insert Into SysPower(UserID,FunID)values('" + userid + "'," + int.Parse(treeView2.Nodes[0].Nodes[i].Tag.ToString()) + ")";
                    }
                    chilrencheck = false;
                }
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    MessageBox.Show("保存成功！");
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Tag != null)
            {
                string userid = treeView1.SelectedNode.Tag.ToString();
                lblPower.Text = treeView1.SelectedNode.Text + "->权限列表";
                try
                {
                    DataTable Dt = SQLDbHelper.Query("Select * from SysPower Where UserID='" + userid + "' Order by FunID").Tables[0];
                    for (int j = 0; j < treeView2.Nodes[0].Nodes.Count; j++)
                    {
                        for (int i = 0; i < treeView2.Nodes[0].Nodes[j].Nodes.Count; i++)
                        {
                            DataRow[] Drs = Dt.Select("FunID=" + int.Parse(treeView2.Nodes[0].Nodes[j].Nodes[i].Tag.ToString()));
                            if (Drs.Length > 0)
                            {
                                treeView2.Nodes[0].Nodes[j].Nodes[i].Checked = true;
                            }
                            else
                            {
                                treeView2.Nodes[0].Nodes[j].Nodes[i].Checked = false;
                            }
                        }
                    }
                    sqlstring = "Select * from SysUser Where UserID='" + userid + "'";
                    Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                    if (Dt.Rows.Count > 0)
                    {
                        txtUserID.Text = Dt.Rows[0]["UserID"].ToString();
                        txtUserName.Text = Dt.Rows[0]["UserName"].ToString();
                        txtPwd.Text = Dt.Rows[0]["Pwd"].ToString();
                        txtUserID.Enabled = false;
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmUpdate fu = new frmUpdate();
            fu.Show();
        }

        private void nUDMonth_ValueChanged(object sender, EventArgs e)
        {
            labYearMonth.Text = nUDYear.Value.ToString() + "年" + nUDMonth.Value.ToString() + "月";

            string sqlstring = "Select * from WorkerPlan where Wyear=" + nUDYear.Value + " and Wmonth=" + nUDMonth.Value;
            SqlCommand mycmd = SQLDbHelper.connection.CreateCommand();
            mycmd.CommandText = sqlstring;
            Myda.SelectCommand = mycmd;
            DtPlan = new DataTable("WorkerPlan");
            Myda.Fill(DtPlan);
            DtPlan = SQLDbHelper.Query(sqlstring).Tables[0];
            LoadDate();
        }
        private void LoadDate()
        {
            DateTime DateT = new DateTime(int.Parse(nUDYear.Value.ToString()), int.Parse(nUDMonth.Value.ToString()), 1);
            TimeSpan ts = DateT.AddMonths(1).Subtract(DateT);
            if (ts.Days == 30)
            {
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = false;
            }
            else
            {
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = true;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string workercode = dataGridView1.Rows[i].Cells[0].Tag.ToString();
                DataRow[] Drs = DtPlan.Select("WorkerCode='" + workercode + "'");
                for (int j = 0; j < Drs.Length; j++)
                {
                    int day = int.Parse(Drs[j]["Wday"].ToString());
                    DataGridViewCellStyle dgvct1 = new DataGridViewCellStyle();
                    dgvct1.ForeColor = Color.White;
                    dgvct1.NullValue = "0";
                    dgvct1.Format = "N0";
                    dataGridView1.Rows[i].Cells[day].Style = dgvct1;
                    dataGridView1.Rows[i].Cells[day].Value = Drs[j]["WorkDay"].ToString();
                    if (Drs[j]["WorkState"].ToString() != string.Empty)
                    {
                        dataGridView1.Rows[i].Cells[day].Value = Drs[j]["WorkState"].ToString();
                        DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                        dgvct.ForeColor = Color.Red;
                        dataGridView1.Rows[i].Cells[day].Style = dgvct;
                    }
                    if (int.Parse(Drs[j]["WorkDay"].ToString()) < 8)
                    {
                        DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                        dgvct.ForeColor = Color.Red;
                        dataGridView1.Rows[i].Cells[day].Style = dgvct;
                    }
                }
            }
        }
        private void LoadGridView1()
        {
            for (int i = 1; i < 32; i++)
            {
                DataGridViewTextBoxColumn dgcbc = new DataGridViewTextBoxColumn();
                dgcbc.HeaderText = i.ToString();
                dgcbc.Tag = i;
                dgcbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns.Add(dgcbc);
            }
            string sqlstring = "Select WorkerName,WorkerCode from Worker order by WorkerGroup desc";
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    dataGridView1.Rows.Add(Dt.Rows.Count);
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = Dt.Rows[i]["WorkerName"].ToString();
                        dataGridView1.Rows[i].Cells[0].Tag = Dt.Rows[i]["WorkerCode"].ToString();
                        dataGridView1.Rows[i].Height = (dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / Dt.Rows.Count;
                    }
                    nUDYear.Value = DateTime.Today.Year;
                    nUDMonth.Value = DateTime.Today.Month;
                    dataGridView1.ClearSelection();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    int val = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                catch
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                }
                string workercode = dataGridView1.Rows[e.RowIndex].Cells[0].Tag.ToString();
                string day = dataGridView1.Columns[e.ColumnIndex].Tag.ToString();
                string sqlstring = "Update WorkerPlan Set WorkState='',IsWork=1,WorkDay=" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) + " where WorkerCode='" + workercode + "' And Wyear=" + nUDYear.Value + " and Wmonth=" + nUDMonth.Value + " And Wday=" + day;
                sqlstring += ";Update WorkerPlan Set WorkState='',IsWork=0 where WorkDay=0 And WorkerCode='" + workercode + "' And Wyear=" + nUDYear.Value + " and Wmonth=" + nUDMonth.Value + " And Wday=" + day;

                SQLDbHelper.ExecuteSql(sqlstring);
                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 8)
                {
                    DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                    dgvct.ForeColor = Color.Red;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = dgvct;
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void btnNewWorker_Click(object sender, EventArgs e)
        {
            frmNewWorker fnw = new frmNewWorker();
            fnw.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowindex = e.RowIndex;
            colindex = e.ColumnIndex;
        }
        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //rowindex = -1;
            //colindex = -1;
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == string.Empty) return;
            if (rowindex > -1 && colindex > 0)
            {
                try
                {
                    string workercode = dataGridView1.Rows[rowindex].Cells[0].Tag.ToString();
                    string day = dataGridView1.Columns[colindex].Tag.ToString();
                    string sqlstring = string.Empty;
                    if (comboBox1.Text.StartsWith("8"))
                    {
                        sqlstring = "Update WorkerPlan Set IsWork=1,WorkState='" + comboBox1.Text + "',WorkDay=8 where WorkerCode='" + workercode + "' And Wyear=" + nUDYear.Value + " and Wmonth=" + nUDMonth.Value + " And Wday=" + day;
                    }
                    else
                    {
                        sqlstring = "Update WorkerPlan Set IsWork=0,WorkState='" + comboBox1.Text + "',WorkDay=0 where WorkerCode='" + workercode + "' And Wyear=" + nUDYear.Value + " and Wmonth=" + nUDMonth.Value + " And Wday=" + day;
                    }
                    SQLDbHelper.ExecuteSql(sqlstring);
                    dataGridView1.Rows[rowindex].Cells[colindex].Value = comboBox1.Text;
                    if (!comboBox1.Text.StartsWith("8"))
                    {
                        DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                        dgvct.ForeColor = Color.Red;
                        dataGridView1.Rows[rowindex].Cells[colindex].Style = dgvct;
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }

        private void 新增用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtUserID.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPwd.Text = string.Empty;
            txtUserID.Enabled = true;
            txtUserName.Focus();
        }
    }
}