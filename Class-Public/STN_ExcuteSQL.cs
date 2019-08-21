using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SinoTunnel
{
    /// <summary>
    /// SQL 指令程式庫
    /// </summary>
    public class ExcuteSQL
    {
        // 連結網路資料庫
        string strConn = "Data Source=pmis03;Initial Catalog=SinoExcavationDB;Persist Security Info=True;User ID=mrtdesign;Password=sino@tech";

        #region Get Data
        public DataTable GetDataBySQL(string qstr)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        //讀取資料範例1 - 依Guid編號
        public DataTable GetByUID(string dataName, string UID)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                //string qstr = string.Format("SELECT * FROM {0} WHERE UID=@UID", dataName);
                string qstr = $"SELECT * FROM {dataName} WHERE UID = @UID";
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                da.SelectCommand.Parameters.AddWithValue("UID", UID);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetBySection(string dataName, string UID, string strOrder)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"SELECT * FROM {dataName} WHERE Section = @UID";
                if (strOrder != "") qstr += " " + strOrder;
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                da.SelectCommand.Parameters.AddWithValue("UID", UID);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetByProject(string dataName, string UID)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"SELECT * FROM {dataName} WHERE Project = @UID";
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                da.SelectCommand.Parameters.AddWithValue("UID", UID);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetProject()
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"SELECT * FROM STN_Project";
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetByKeyword(string dataName, string dataItem, string UID)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"SELECT * FROM {dataName} WHERE {dataItem} = @UID";
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                da.SelectCommand.Parameters.AddWithValue("UID", UID);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public string GetActiveSecion(string UserName)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"SELECT * FROM STN_SectionActive WHERE UserName = @UserName";
                SqlDataAdapter da = new SqlDataAdapter(qstr, dbConn);
                da.SelectCommand.Parameters.AddWithValue("UserName", UserName);
                DataTable dt = new DataTable();
                dbConn.Open();
                da.Fill(dt);
                string str = dt.Rows[0]["Section"].ToString();
                return str;
            }
        }
        #endregion

        #region Insert Data
        public string InsertData(string tableName, string InsertSTR)
        {
            string UID = Guid.NewGuid().ToString("D");
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO {tableName} {InsertSTR}";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
            return UID;
        }

        public void InsertActiveSection(string userName, object section)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO STN_SectionActive (UserName,Section) VALUES (@userName,@section)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("section", section);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string InsertData(string tableName, string ItemID, object ItemValue, string ItemID2, object ItemValue2)
        {
            string UID = Guid.NewGuid().ToString("D");
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO {tableName} (UID,{ItemID},{ItemID2}) VALUES (@UID,@ItemValue,@ItemValue2)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", UID);
                cmd.Parameters.AddWithValue("ItemValue", ItemValue);
                cmd.Parameters.AddWithValue("ItemValue2", ItemValue2);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
            return UID;
        }

        public string InsertData(string tableName, string ItemID, object ItemValue)
        {
            string UID = Guid.NewGuid().ToString("D");
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO {tableName} (UID,{ItemID}) VALUES (@UID,@ItemValue)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", UID);
                cmd.Parameters.AddWithValue("ItemValue", ItemValue);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
            return UID;
        }

        public string InsertSAPData(string tableName, List<string> inputUID, object sectionUID, List<Tuple<int, string, int>> sectionProp, List<Tuple<double,double, double,double,double, double>> frameResult)
        {
            string outputUID = "";
            for (int i = 0; i < sectionProp.Count; i++)
            {
                //
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO {tableName} (UID,Section,Member,LoadType,Joint,Axial,ShearY,ShearZ,Torsion,MomentY,MomentZ,CreateUser) VALUES (@UID,@Section,@Member,@LoadType,@Joint,@Axial,@ShearY,@ShearZ,@Torsion,@MomentY,@MomentZ,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);
                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("Member", sectionProp[i].Item1);
                    cmd.Parameters.AddWithValue("LoadType", sectionProp[i].Item2);
                    cmd.Parameters.AddWithValue("Joint", sectionProp[i].Item3);
                    cmd.Parameters.AddWithValue("Axial", frameResult[i].Item1);
                    cmd.Parameters.AddWithValue("ShearY", frameResult[i].Item2);
                    cmd.Parameters.AddWithValue("ShearZ", frameResult[i].Item3);
                    cmd.Parameters.AddWithValue("Torsion", frameResult[i].Item4);
                    cmd.Parameters.AddWithValue("MomentY", frameResult[i].Item5);
                    cmd.Parameters.AddWithValue("MomentZ", frameResult[i].Item6);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();                    
                }
            }            
            return outputUID;
        }            

        public string InsertSAPData(string tableName, List<string> inputUID, object sectionUID, List<Tuple<string, string, string>> sectionProp, List<Tuple<double, double, double, double, double, double>> frameResult)
        {
            string outputUID = "";
            for (int i = 0; i < sectionProp.Count; i++)
            {
                //
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO {tableName} (UID,Section,Member,LoadType,Joint,Axial,ShearY,ShearZ,Torsion,MomentY,MomentZ,CreateUser) VALUES (@UID,@Section,@Member,@LoadType,@Joint,@Axial,@ShearY,@ShearZ,@Torsion,@MomentY,@MomentZ,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);
                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("Member", sectionProp[i].Item1);
                    cmd.Parameters.AddWithValue("LoadType", sectionProp[i].Item2);
                    cmd.Parameters.AddWithValue("Joint", sectionProp[i].Item3);
                    cmd.Parameters.AddWithValue("Axial", frameResult[i].Item1);
                    cmd.Parameters.AddWithValue("ShearY", frameResult[i].Item2);
                    cmd.Parameters.AddWithValue("ShearZ", frameResult[i].Item3);
                    cmd.Parameters.AddWithValue("Torsion", frameResult[i].Item4);
                    cmd.Parameters.AddWithValue("MomentY", frameResult[i].Item5);
                    cmd.Parameters.AddWithValue("MomentZ", frameResult[i].Item6);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return outputUID;
        }

        public string InsertSAPData(string tableName, List<string> inputUID, object sectionUID, List<Tuple<string, string, string>> sectionProp, int times, List<Tuple<double, double, double, double, double, double>> frameResult)
        {
            string outputUID = "";
            for (int i = 0; i < sectionProp.Count; i++)
            {
                //
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO {tableName} (UID,Section,Member,LoadType,Joint,Times,Axial,ShearY,ShearZ,Torsion,MomentY,MomentZ,CreateUser) VALUES (@UID,@Section,@Member,@LoadType,@Joint,@Times,@Axial,@ShearY,@ShearZ,@Torsion,@MomentY,@MomentZ,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);
                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("Member", sectionProp[i].Item1);
                    cmd.Parameters.AddWithValue("LoadType", sectionProp[i].Item2);
                    cmd.Parameters.AddWithValue("Times", times);
                    cmd.Parameters.AddWithValue("Joint", sectionProp[i].Item3);
                    cmd.Parameters.AddWithValue("Axial", frameResult[i].Item1);
                    cmd.Parameters.AddWithValue("ShearY", frameResult[i].Item2);
                    cmd.Parameters.AddWithValue("ShearZ", frameResult[i].Item3);
                    cmd.Parameters.AddWithValue("Torsion", frameResult[i].Item4);
                    cmd.Parameters.AddWithValue("MomentY", frameResult[i].Item5);
                    cmd.Parameters.AddWithValue("MomentZ", frameResult[i].Item6);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return outputUID;
        }

        //[MaterialName, MaerialType, UnitWeight, YoungsModulus, PoissonRatio, CTE(熱膨脹係數), FC(抗壓強度)]
        public void InsertMaterialData(List<string> inputUID, object projectUID, List<Tuple<string, string, double, double, double, double, double>> materialProp)
        {
            for(int i = 0; i < inputUID.Count; i++)
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO STN_SegmentMaterial (UID,Project,MaterialName,MaterialType,UnitWeight,YoungModulus,PoissonRatio,CTE,Fc,CreateUser) VALUES (@UID,@Project,@MaterialName,@MaterialType,@UnitWeight,@YoungModulus,@PoissonRatio,@CTE,@Fc,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);

                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Project", projectUID);
                    cmd.Parameters.AddWithValue("MaterialName", materialProp[i].Item1);
                    cmd.Parameters.AddWithValue("MaterialType", materialProp[i].Item2);
                    cmd.Parameters.AddWithValue("UnitWeight", materialProp[i].Item3);
                    cmd.Parameters.AddWithValue("YoungModulus", materialProp[i].Item4);
                    cmd.Parameters.AddWithValue("PoissonRatio", materialProp[i].Item5);
                    cmd.Parameters.AddWithValue("CTE", materialProp[i].Item6);
                    cmd.Parameters.AddWithValue("Fc", materialProp[i].Item7);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }            
        }

        //[Name, Material, Shape, Width, Height(Diameter)]
        public void InsertFrameData(List<string> inputUID, object sectionUID, List<Tuple<string, string, string, double, double>> frameProp)
        {          
            for(int i = 0; i < inputUID.Count; i++)
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO STN_Segment (UID,Section,Name,Material,Shape,Width,Height,CreateUser) VALUES (@UID,@Section,@Name,@Material,@Shape,@Width,@Height,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);

                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("Name", frameProp[i].Item1);
                    cmd.Parameters.AddWithValue("Material", frameProp[i].Item2);
                    cmd.Parameters.AddWithValue("Shape", frameProp[i].Item3);
                    cmd.Parameters.AddWithValue("Width", frameProp[i].Item4);
                    cmd.Parameters.AddWithValue("Height", frameProp[i].Item5);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }                            
        }

        public void InsertSoilLinkData(List<string> inputUID, object sectionUID, List<Tuple<string, string, double>> soilLinkProp)
        {
            for(int i = 0; i < inputUID.Count; i++)
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO STN_SegmentSoilLink (UID,Section,Name,Type,YoungsModulus,CreateUser) VALUES (@UID,@Section,@Name,@Type,@YoungsModulus,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);

                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("Name", soilLinkProp[i].Item1);
                    cmd.Parameters.AddWithValue("Type", soilLinkProp[i].Item2);
                    cmd.Parameters.AddWithValue("YoungsModulus", soilLinkProp[i].Item3);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertSGCalDepthData(List<string> inputUID, object sectionUID, List<Tuple<string, int, string, string, string, string>> DepthData)
        {
            for(int i = 0; i < inputUID.Count;i++)
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = "";
                    if(DepthData[i].Item6 == "")
                        qstr = $"INSERT INTO STN_SGCalDepth (UID,Section,LoadType,TimesOfLoad,Member,Joint,DataUID," +
                            $"CreateUser) VALUES (@UID,@Section,@LoadType,@TimesOfLoad,@Member,@Joint,@DataUID," +
                            $"@CreateUser)";
                    else
                        qstr = $"INSERT INTO STN_SGCalDepth (UID,Section,LoadType,TimesOfLoad,Member,Joint,DataUID," +
                            $"SegmentUID,CreateUser) VALUES (@UID,@Section,@LoadType,@TimesOfLoad,@Member,@Joint," +
                            $"@DataUID,@SegmentUID,@CreateUser)";

                    SqlCommand cmd = new SqlCommand(qstr, dbConn);

                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("LoadType", DepthData[i].Item1);
                    cmd.Parameters.AddWithValue("TimesOfLoad", DepthData[i].Item2);
                    cmd.Parameters.AddWithValue("Member", DepthData[i].Item3);
                    cmd.Parameters.AddWithValue("Joint", DepthData[i].Item4);
                    cmd.Parameters.AddWithValue("DataUID", DepthData[i].Item5);
                    if (DepthData[i].Item6 != "") cmd.Parameters.AddWithValue("SegmentUID", DepthData[i].Item6);
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertSGCalDepthData(List<string> inputUID, object sectionUID, List<Tuple<string, int, string, string, string, double>> DepthData)
        {
            for (int i = 0; i < inputUID.Count; i++)
            {
                using (SqlConnection dbConn = new SqlConnection(strConn))
                {
                    string qstr = $"INSERT INTO STN_SGCalDepth (UID,Section,LoadType,TimesOfLoad,Member,Joint, DataUID, " +
                        $"ContactDepth,CreateUser) VALUES (@UID,@Section,@LoadType,@TimesOfLoad,@Member,@Joint,@DataUID," +
                        $"@ContactDepth,@CreateUser)";
                    SqlCommand cmd = new SqlCommand(qstr, dbConn);

                    cmd.Parameters.AddWithValue("UID", inputUID[i]);
                    cmd.Parameters.AddWithValue("Section", sectionUID);
                    cmd.Parameters.AddWithValue("LoadType", DepthData[i].Item1);
                    cmd.Parameters.AddWithValue("TimesOfLoad", DepthData[i].Item2);
                    cmd.Parameters.AddWithValue("Member", DepthData[i].Item3);
                    cmd.Parameters.AddWithValue("Joint", DepthData[i].Item4);
                    cmd.Parameters.AddWithValue("DataUID", DepthData[i].Item5);
                    cmd.Parameters.AddWithValue("ContactDepth", DepthData[i].Item6);                    
                    cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertFrameMaterial(object sectionUID, string depth, string matType, string loadType, string times)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO STN_FrameMaterial (Section,Depth,MaterialType,LoadType,Times,CreateUser)" +
                    $" VALUES (@sectionUID,@depth,@matType,@loadType,@times,@CreateUser)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("sectionUID", sectionUID);
                cmd.Parameters.AddWithValue("depth", depth);
                cmd.Parameters.AddWithValue("matType", matType);
                cmd.Parameters.AddWithValue("loadType", loadType);
                cmd.Parameters.AddWithValue("times", times);
                cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertFrameMaterial(object sectionUID, string depth, string matType, string loadType, string times, double beff)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"INSERT INTO STN_FrameMaterial (Section,Depth,MaterialType,LoadType,Times,beff,CreateUser)" +
                    $" VALUES (@sectionUID,@depth,@matType,@loadType,@times,@beff,@CreateUser)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("sectionUID", sectionUID);
                cmd.Parameters.AddWithValue("depth", depth);
                cmd.Parameters.AddWithValue("matType", matType);
                cmd.Parameters.AddWithValue("loadType", loadType);
                cmd.Parameters.AddWithValue("times", times);
                cmd.Parameters.AddWithValue("beff", beff);
                cmd.Parameters.AddWithValue("CreateUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Delete Data
        public void DeleteData(string tableName, string str)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE {tableName} WHERE @str";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("str", str);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDataBySectionUID(string tableName, string sectionUID)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE {tableName} WHERE (Section=@UID)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", sectionUID);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDataBySectionUIDAndTimes(string tableName, string sectionUID, int times)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE {tableName} WHERE (Section=@UID) AND (Times=@Times)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", sectionUID);
                cmd.Parameters.AddWithValue("Times", times);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDataBySectionUIDAndLoadType(string tableName, string sectionUID, string loadType)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE {tableName} WHERE (Section=@UID) AND (LoadType=@loadType)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", sectionUID);
                cmd.Parameters.AddWithValue("loadType", loadType);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDataBySectionUIDAndTimesAndLoadType(string tableName, string sectionUID, int times, string loadType)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE {tableName} WHERE (Section=@UID) AND (Times=@Times) AND (LoadType=@LoadType)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("UID", sectionUID);
                cmd.Parameters.AddWithValue("Times", times);
                cmd.Parameters.AddWithValue("LoadType", loadType);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCalDepth(string sectionUID, string loadType, int times)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"DELETE STN_SGCalDepth WHERE (Section=@SectionUID) AND (LoadType=@LoadType) AND (TimesOfLoad=@TimesOfLoad)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("SectionUID", sectionUID);
                cmd.Parameters.AddWithValue("LoadType", loadType);
                cmd.Parameters.AddWithValue("TimesOfLoad", times);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        public void UpdateData(string tableName, string refID, string refValue, string updateID, object updateValue)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"UPDATE {tableName} SET {updateID} = @updateValue WHERE {refID} = @refValue";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("updateValue", updateValue);
                cmd.Parameters.AddWithValue("refValue", refValue);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateData(string tableName, string refID, string refValue, string refID2, string refValue2,
            string updateID, object updateValue)
        {
            using (SqlConnection dbConn = new SqlConnection(strConn))
            {
                string qstr = $"UPDATE {tableName} SET {updateID} = @updateValue WHERE ({refID} = @refValue) " +
                    $"AND ({refID2} = @refValue2)";
                SqlCommand cmd = new SqlCommand(qstr, dbConn);
                cmd.Parameters.AddWithValue("updateValue", updateValue);
                cmd.Parameters.AddWithValue("refValue", refValue);
                cmd.Parameters.AddWithValue("refValue2", refValue2);
                dbConn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
