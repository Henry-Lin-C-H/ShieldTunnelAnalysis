using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using SinoAuth;

namespace SinoTunnel
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {            
            ExcuteSQL oExcuteSQL = new ExcuteSQL();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            appGlobal.IdentityUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            appGlobal.UserID = appGlobal.IdentityUser.Replace(@"SECLTD\", "");

            Employee emp = new Employee();
            DataTable dtEmp = emp.GetEmp(appGlobal.UserID);

            appGlobal.IsMembership = false;

            if(dtEmp.Rows.Count > 0)
            {
                appGlobal.UserEmail = dtEmp.Rows[0]["email"].ToString();
                DataTable dtable = oExcuteSQL.GetDataBySQL(string.Format("SELECT u.*,p.PropertyValuesString FROM aspnet_Users u LEFT OUTER JOIN aspnet_Profile p ON p.UserId=u.UserId WHERE (u.UserName='{0}')", appGlobal.UserEmail));

                if (dtable.Rows.Count > 0)
                {
                    appGlobal.IsMembership = true;
                    appGlobal.UserName = dtable.Rows[0]["PropertyValuesString"].ToString();
                }
            }

            if (appGlobal.IsMembership)
            {
                DataTable dt = oExcuteSQL.GetDataBySQL("SELECT * FROM STN_APPVersion WHERE (Name = 'SinoTunnel') ORDER BY CreateDate DESC");
                string version = dt.Rows[0]["Version"].ToString();

                if (version == Application.ProductVersion)
                {
                    Application.Run(new Form1());
                }
                else if (appGlobal.IdentityUser == @"SECLTD\6989")
                {
                    MessageBox.Show("並非最新版本:" + version + "，請注意使用");
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("請使用最新版：" + version);
                }
            }
            else MessageBox.Show("尚未申請帳號！");

            
            
        }
    }
}
