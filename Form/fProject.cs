using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
//using STN_SQL;

namespace SinoTunnel
{
    public partial class fProject : Form
    {
       // STN_SQL.STN_data datasearch = new STN_SQL.STN_data();

        ExcuteSQL dataSearch = new ExcuteSQL();

        public Form fParent = null;

        public fProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fProject_Load(object sender, EventArgs e)
        {
            /*dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;*/
            DataGridViewColumn col;

            col = new DataGridViewTextBoxColumn();
            col.Name = "ProjectID";
            col.DataPropertyName = "ProjectID";
            col.HeaderText = "標案編號";
            col.ReadOnly = true;
            col.Frozen = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "ProjectName";
            col.DataPropertyName = "ProjectName";            
            col.HeaderText = "標案名稱";
            col.ReadOnly = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "Region";
            col.DataPropertyName = "Region";
            col.HeaderText = "地區";
            col.ReadOnly = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "SectionNum";
            col.DataPropertyName = "SectionNum";
            col.HeaderText = "斷面數量";
            col.ReadOnly = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "EmpName";
            col.DataPropertyName = "EmpName";
            col.HeaderText = "建立者";
            col.ReadOnly = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "CreateDate";
            col.DataPropertyName = "CreateDate";
            col.HeaderText = "建立日期";
            col.ReadOnly = true;
            dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "UID";
            col.DataPropertyName = "UID";
            col.Visible = false;
            dataGridView1.Columns.Add(col);

            ListLoadData();
        }

        public void ListLoadData()
        {
            DataTable list = dataSearch.GetDataBySQL(string.Format("SELECT a.*,b.SectionNum FROM [STN_Project] a LEFT OUTER JOIN (SELECT Project,Count(*) AS SectionNum FROM STN_Section GROUP BY Project) b ON b.Project=a.UID WHERE (a.IsDelete=0) AND (a.CreateUser='{0}' OR (a.UID IN (SELECT DISTINCT Project FROM STN_ProjectUser WHERE UserID='{0}')))  ORDER BY a.CreateDate DESC", appGlobal.UserEmail));

            list.Columns.Remove("Description");
            list.Columns.Remove("Stage");
            list.Columns.Remove("IsDelete");
            

            list.Columns.Add("EmpName", typeof(string));
            //list.Columns.Add("SectionNum", typeof(int));

            DataTable section = dataSearch.GetDataBySQL("SELECT * FROM STN_Section");
            for(int i = 0; i < list.Rows.Count; i++)
            {
                string createUser = list.Rows[i]["CreateUser"].ToString();
                DataTable dt = dataSearch.GetDataBySQL($"SELECT u.*,p.PropertyValuesString FROM aspnet_Users u LEFT OUTER JOIN aspnet_Profile p ON p.UserId=u.UserId WHERE (u.UserName='{createUser}')");
                list.Rows[i]["EmpName"] = dt.Rows[0]["PropertyValuesString"].ToString();

                int k = 0;
                for(int j = 0; j < section.Rows.Count; j++)
                {
                    if (list.Rows[i]["UID"].ToString() == section.Rows[j]["Project"].ToString()) k++;
                }
                list.Rows[i]["SectionNum"] = k;
            }
            list.Columns.Remove("CreateUser");
            list.Columns.Remove("OutSectionA");
            list.Columns.Remove("OutSectionB");
            list.Columns.Remove("OutSectionBS");
            list.Columns.Remove("OutSectionC");
            list.Columns.Remove("OutSectionD");
            list.Columns.Remove("OutSectionE");
            list.Columns.Remove("TemplateForBIM");
            list.Columns.Remove("DesignBy");
            list.Columns.Remove("CheckBy");

            dataGridView1.DataSource = list;
        }

        private void ProjectSelect_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("請選擇標案");
                return;
            }

            appGlobal.projectUID = dataGridView1.SelectedCells[0].OwningRow.Cells["UID"].Value.ToString();
            appGlobal.projectTitle = $"{dataGridView1.SelectedCells[0].OwningRow.Cells["ProjectID"].Value} {dataGridView1.SelectedCells[0].OwningRow.Cells["ProjectName"].Value}";

            if(fParent is Form1)
            {
                ((Form1)fParent).ShowSectionList();
                //((Form1)fParent).UpdateData();
            }

            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            ProjectSelect_Click(null, null);
        }
    }
}
