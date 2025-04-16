using OfficeOpenXml; //epplus package should be installed - License Cost Details pls visit: https://www.epplussoftware.com/
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogistICSA_CFS_CFSImport : System.Web.UI.Page
{
    //-----------------------------------------------------------LinerTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LinerTableInsertData(List<LinerTableDataList> LinerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Select Count(*) From CFSImportLiner Where MainJobNo = '" + LinerTableDataList[0].MainJobNo + "' And LinerItemNo = '" + LinerTableDataList[0].LinerItemNo + "' " +
                                "And RecordStatus = 'Active' And LinerStatus = 'New';");
            cmd.Connection = cnn;
            cnn.Open();
            int DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
            cnn.Close();

            if (DupCount > 0) { return "Duplicate Entry !!!"; }

            cmd = new SqlCommand("Insert into CFSImportLiner(ComputerName,IPAddress,Location,LinerCHAName,CompanyName,BranchName,MainJobNo,LinerItemNo,LinerImporterName,LinerLinerAgent,LinerBLNo,LinerBLDate,LinerIMDG,LinerWeightKg,LinerPKG,LinerCargoDetails,LinerTSANo,LinerTSADate,UpdatedBy) " +
                                 "Values(@ComputerName,@IPAddress,@Location,@GeneralCHAName,@CompanyName,@BranchName,@MainJobNo,@LinerItemNo,@LinerImporterName,@LinerLinerAgent,@LinerBLNo,@LinerBLDate,@LinerIMDG,@LinerWeightKg,@LinerPKG,@LinerCargoDetails,@LinerTSANo,@LinerTSADate,@UpdatedBy)", cnn);
            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
            cmd.Parameters.AddWithValue("@MainJobNo", LinerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LinerItemNo", LinerTableDataList[0].LinerItemNo.Trim());
            cmd.Parameters.AddWithValue("@LinerImporterName", LinerTableDataList[0].LinerImporterName.Trim());
            cmd.Parameters.AddWithValue("@LinerLinerAgent", LinerTableDataList[0].LinerLinerAgent.Trim());
            cmd.Parameters.AddWithValue("@LinerBLNo", LinerTableDataList[0].LinerBLNo.Trim());
            if (LinerTableDataList[0].LinerBLDate == "" || LinerTableDataList[0].LinerBLDate == null) { cmd.Parameters.AddWithValue("@LinerBLDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@LinerBLDate", Convert.ToDateTime(LinerTableDataList[0].LinerBLDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@LinerIMDG", LinerTableDataList[0].LinerIMDG.Trim());
            cmd.Parameters.AddWithValue("@LinerWeightKg", LinerTableDataList[0].LinerWeightKg.Trim());
            cmd.Parameters.AddWithValue("@LinerPKG", LinerTableDataList[0].LinerPKG.Trim());
            cmd.Parameters.AddWithValue("@LinerCargoDetails", LinerTableDataList[0].LinerCargoDetails.Trim());
            cmd.Parameters.AddWithValue("@LinerTSANo", LinerTableDataList[0].LinerTSANo.Trim());
            cmd.Parameters.AddWithValue("@GeneralCHAName", LinerTableDataList[0].LinerGeneralCHAName.Trim());
            if (LinerTableDataList[0].LinerTSADate == "" || LinerTableDataList[0].LinerTSADate == null) { cmd.Parameters.AddWithValue("@LinerTSADate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@LinerTSADate", Convert.ToDateTime(LinerTableDataList[0].LinerTSADate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportLiner", LinerTableDataList[0].LinerItemNo.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class LinerTableDataList
    {
        public string MainJobNo { get; set; }
        public string LinerItemNo { get; set; }
        public string LinerImporterName { get; set; }
        public string LinerLinerAgent { get; set; }
        public string LinerGeneralCHAName { get; set; }
        public string LinerBLNo { get; set; }
        public string LinerBLDate { get; set; }
        public string LinerIMDG { get; set; }
        public string LinerWeightKg { get; set; }
        public string LinerPKG { get; set; }
        public string LinerCargoDetails { get; set; }
        public string LinerTSANo { get; set; }
        public string LinerTSADate { get; set; }
        public string TariffValues { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------LinerTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LinerTableUpdateData(List<LinerTableDataList> LinerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<LinerTableDataList> DetailedList = new List<LinerTableDataList>();
        LinerTableDataList DetailedListValue = new LinerTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportLiner WHERE MainJobNo = @MainJobNo and LinerItemNo = @LinerItemNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LinerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LinerItemNo", LinerTableDataList[0].LinerItemNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.LinerItemNo = dtrow["LinerItemNo"].ToString();
                DetailedListValue.LinerImporterName = dtrow["LinerImporterName"].ToString();
                DetailedListValue.LinerLinerAgent = dtrow["LinerLinerAgent"].ToString();
                DetailedListValue.LinerBLNo = dtrow["LinerBLNo"].ToString();
                DetailedListValue.LinerBLDate = dtrow["LinerBLDate"].ToString();
                if (dtrow["LinerBLDate"].ToString() != "" || dtrow["LinerBLDate"] != DBNull.Value) { DetailedListValue.LinerBLDate = Convert.ToDateTime(dtrow["LinerBLDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LinerBLDate = ""; }
                DetailedListValue.LinerIMDG = dtrow["LinerIMDG"].ToString();
                DetailedListValue.LinerWeightKg = dtrow["LinerWeightKg"].ToString();
                DetailedListValue.LinerPKG = dtrow["LinerPKG"].ToString();
                DetailedListValue.LinerCargoDetails = dtrow["LinerCargoDetails"].ToString();
                DetailedListValue.LinerTSANo = dtrow["LinerTSANo"].ToString();
                DetailedListValue.LinerTSADate = dtrow["LinerTSADate"].ToString();
                DetailedListValue.LinerGeneralCHAName = dtrow["LinerCHAName"].ToString();
                if (dtrow["LinerTSADate"].ToString() != "" || dtrow["LinerTSADate"] != DBNull.Value) { DetailedListValue.LinerTSADate = Convert.ToDateTime(dtrow["LinerTSADate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LinerTSADate = ""; }
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportLiner set CompanyName = @CompanyName, BranchName = @BranchName, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, LinerCHAName=@GeneralCHAName,LinerItemNo=@LinerItemNo,LinerImporterName=@LinerImporterName,LinerLinerAgent=@LinerLinerAgent,LinerBLNo=@LinerBLNo,LinerBLDate=@LinerBLDate,LinerIMDG=@LinerIMDG,LinerWeightKg=@LinerWeightKg,LinerPKG=@LinerPKG,LinerCargoDetails=@LinerCargoDetails,LinerTSANo=@LinerTSANo,LinerTSADate=@LinerTSADate where LinerItemNo = @LinerItemNo And MainJobNo = @MainJobNo And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["Company"].ToString());
            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["Branch"].ToString());
            cmd.Parameters.AddWithValue("@MainJobNo", LinerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LinerItemNo", LinerTableDataList[0].LinerItemNo.Trim());
            cmd.Parameters.AddWithValue("@LinerImporterName", LinerTableDataList[0].LinerImporterName.Trim());
            cmd.Parameters.AddWithValue("@LinerLinerAgent", LinerTableDataList[0].LinerLinerAgent.Trim());
            cmd.Parameters.AddWithValue("@LinerBLNo", LinerTableDataList[0].LinerBLNo.Trim());
            if (LinerTableDataList[0].LinerBLDate == "" || LinerTableDataList[0].LinerBLDate == null)
            {
                cmd.Parameters.AddWithValue("@LinerBLDate", DBNull.Value);
                LinerTableDataList[0].LinerBLDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LinerBLDate", Convert.ToDateTime(LinerTableDataList[0].LinerBLDate.Trim()).Date);
                LinerTableDataList[0].LinerBLDate = Convert.ToDateTime(LinerTableDataList[0].LinerBLDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LinerIMDG", LinerTableDataList[0].LinerIMDG.Trim());
            cmd.Parameters.AddWithValue("@LinerWeightKg", LinerTableDataList[0].LinerWeightKg.Trim());
            cmd.Parameters.AddWithValue("@LinerPKG", LinerTableDataList[0].LinerPKG.Trim());
            cmd.Parameters.AddWithValue("@LinerCargoDetails", LinerTableDataList[0].LinerCargoDetails.Trim());
            cmd.Parameters.AddWithValue("@LinerTSANo", LinerTableDataList[0].LinerTSANo.Trim());
            cmd.Parameters.AddWithValue("@GeneralCHAName", LinerTableDataList[0].LinerGeneralCHAName.Trim());
            if (LinerTableDataList[0].LinerTSADate == "" || LinerTableDataList[0].LinerTSADate == null)
            {
                cmd.Parameters.AddWithValue("@LinerTSADate", DBNull.Value);
                LinerTableDataList[0].LinerTSADate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LinerTSADate", Convert.ToDateTime(LinerTableDataList[0].LinerTSADate.Trim()).Date);
                LinerTableDataList[0].LinerTSADate = Convert.ToDateTime(LinerTableDataList[0].LinerTSADate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.LinerItemNo != LinerTableDataList[0].LinerItemNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerItemNo", DetailedListValue.LinerItemNo,
                    LinerTableDataList[0].LinerItemNo.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerGeneralCHAName != LinerTableDataList[0].LinerGeneralCHAName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerCHAName", DetailedListValue.LinerGeneralCHAName,
                    LinerTableDataList[0].LinerGeneralCHAName.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerImporterName != LinerTableDataList[0].LinerImporterName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerImporterName", DetailedListValue.LinerImporterName,
                    LinerTableDataList[0].LinerImporterName.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerLinerAgent != LinerTableDataList[0].LinerLinerAgent.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerLinerAgent", DetailedListValue.LinerLinerAgent,
                    LinerTableDataList[0].LinerLinerAgent.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerBLNo != LinerTableDataList[0].LinerBLNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerBLNo", DetailedListValue.LinerBLNo,
                    LinerTableDataList[0].LinerBLNo.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerBLDate != LinerTableDataList[0].LinerBLDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerBLDate", DetailedListValue.LinerBLDate,
                    LinerTableDataList[0].LinerBLDate.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerIMDG != LinerTableDataList[0].LinerIMDG.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerIMDG", DetailedListValue.LinerIMDG,
                    LinerTableDataList[0].LinerIMDG.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerWeightKg != LinerTableDataList[0].LinerWeightKg.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerWeightKg", DetailedListValue.LinerWeightKg,
                    LinerTableDataList[0].LinerWeightKg.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerPKG != LinerTableDataList[0].LinerPKG.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerPKG", DetailedListValue.LinerPKG,
                    LinerTableDataList[0].LinerPKG.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerCargoDetails != LinerTableDataList[0].LinerCargoDetails.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerCargoDetails", DetailedListValue.LinerCargoDetails,
                    LinerTableDataList[0].LinerCargoDetails.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerTSANo != LinerTableDataList[0].LinerTSANo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerTSANo", DetailedListValue.LinerTSANo,
                    LinerTableDataList[0].LinerTSANo.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LinerTSADate != LinerTableDataList[0].LinerTSADate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLiner",
                    LinerTableDataList[0].LinerItemNo.Trim(), "LinerTSADate", DetailedListValue.LinerTSADate,
                    LinerTableDataList[0].LinerTSADate.Trim(), "Modified", "MainJobNo = '" + LinerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LinerItemNo = '" + LinerTableDataList[0].LinerItemNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------LinerTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LinerTableCancelData(List<LinerTableDataList> LinerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportLiner set RecordStatus = 'Cancelled' where LinerItemNo = @LinerItemNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@LinerItemNo", LinerTableDataList[0].LinerItemNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", LinerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportLiner", LinerTableDataList[0].LinerItemNo.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static LinerTableDataList[] LinerTableSearchData(List<LinerTableDataList> LinerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<LinerTableDataList> DetailedList = new List<LinerTableDataList>();
        LinerTableDataList DetailedListValue = new LinerTableDataList();
        try
        {
            cmd = new SqlCommand("select * from CFSImportLiner Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LinerTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();
            if (dt.Rows.Count == 0)
            {
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new LinerTableDataList();
                DetailedListValue.LinerItemNo = dtrow["LinerItemNo"].ToString();
                DetailedListValue.LinerImporterName = dtrow["LinerImporterName"].ToString();
                DetailedListValue.LinerLinerAgent = dtrow["LinerLinerAgent"].ToString();
                DetailedListValue.LinerBLNo = dtrow["LinerBLNo"].ToString();
                DetailedListValue.LinerBLDate = String.Format("{0:yyyy-MM-dd}", dtrow["LinerBLDate"]);
                DetailedListValue.LinerIMDG = dtrow["LinerIMDG"].ToString();
                DetailedListValue.LinerWeightKg = dtrow["LinerWeightKg"].ToString();
                DetailedListValue.LinerPKG = dtrow["LinerPKG"].ToString();
                DetailedListValue.LinerCargoDetails = dtrow["LinerCargoDetails"].ToString();
                DetailedListValue.LinerTSANo = dtrow["LinerTSANo"].ToString();
                DetailedListValue.LinerGeneralCHAName = dtrow["LinerCHAName"].ToString();

                cmd = new SqlCommand(@"select CFSChargeHead from ContractCFS Where ContractNo In (Select ContractNo from [Contract] Where 
                                     CustomerName = '" + dtrow["LinerCHAName"].ToString() + @"' And RecordStatus = 'Active') And RecordStatus = 'Active'", cnn);
                cnn.Open();
                SqlDataReader Sqlrd = cmd.ExecuteReader();
                string Tariffs = "";
                while (Sqlrd.Read())
                {
                    Tariffs += Sqlrd["CFSChargeHead"].ToString() + ",";
                }
                cnn.Close();

                DetailedListValue.TariffValues = Tariffs;
                DetailedListValue.LinerTSADate = String.Format("{0:yyyy-MM-dd}", dtrow["LinerTSADate"]);
                DetailedList.Add(DetailedListValue);
            }
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
        }

        return DetailedList.ToArray();
    }
    //-----------------------------------------------------------ContainerTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ContainerTableInsertData(List<ContainerTableDataList> ContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            foreach (var ContainerTableDataListField in ContainerTableDataList)
            {
                cmd = new SqlCommand("Select Count(*) From CFSImportContainer Where MainJobNo = '" + ContainerTableDataListField.MainJobNo + "' And ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo + "' And ContainerNo = '" + ContainerTableDataListField.ContainerNo + "' " +
                                "And RecordStatus = 'Active';");
                cmd.Connection = cnn;
                cnn.Open();
                int DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                cnn.Close();

                if (DupCount > 0) { return "Duplicate Entry - " + ContainerTableDataListField.ContainerNo; }

                cmd = new SqlCommand("Insert into CFSImportContainer(ReleaseBy,HoldBy,ComputerName,IPAddress,Location,ContainerIMOCode,CompanyName,BranchName,MainJobNo,ContainerItemNo,ContainerNo,ContainerISOCode,ContainerSize,ContainerType,ContainerSealNo,ContainerTareWeight,ContainerWeightKg,ContainerCargoWeightKg,ContainerCargoNature,ContainerNoofPackage,ContainerFCLLCL,ContainerPrimarySecondary,ContainerGroupCode,ContainerUNNo,ContainerScanType,ContainerScanLocation,ContainerDeliveryMode,ContainerHold,ContainerHoldRemarks,ContainerHoldAgency,ContainerHoldDate,ContainerReleaseDate,ContainerReleaseRemarks,ContainerClaimDetails,ContainerClaimAmount,ContainerPaymentDate,ContainerRemarks,ContainerWHLoc,ContainerPriority,UpdatedBy) " +
                                     "Values(@ReleaseBy,@HoldBy,@ComputerName,@IPAddress,@Location,@ContainerIMOCode,@CompanyName,@BranchName,@MainJobNo,@ContainerItemNo,@ContainerNo,@ContainerISOCode,@ContainerSize,@ContainerType,@ContainerSealNo,@ContainerTareWeight,@ContainerWeightKg,@ContainerCargoWeightKg,@ContainerCargoNature,@ContainerNoofPackage,@ContainerFCLLCL,@ContainerPrimarySecondary,@ContainerGroupCode,@ContainerUNNo,@ContainerScanType,@ContainerScanLocation,@ContainerDeliveryMode,@ContainerHold,@ContainerHoldRemarks,@ContainerHoldAgency,@ContainerHoldDate,@ContainerReleaseDate,@ContainerReleaseRemarks,@ContainerClaimDetails,@ContainerClaimAmount,@ContainerPaymentDate,@ContainerRemarks,@ContainerWHLoc,@ContainerPriority,@UpdatedBy)", cnn);

                cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                cmd.Parameters.AddWithValue("@MainJobNo", ContainerTableDataListField.MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerItemNo", ContainerTableDataListField.ContainerItemNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerNo", ContainerTableDataListField.ContainerNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerISOCode", ContainerTableDataListField.ContainerISOCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerSize", ContainerTableDataListField.ContainerSize.Trim());
                cmd.Parameters.AddWithValue("@ContainerType", ContainerTableDataListField.ContainerType.Trim());
                cmd.Parameters.AddWithValue("@ContainerSealNo", ContainerTableDataListField.ContainerSealNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerTareWeight", ContainerTableDataListField.ContainerTareWeight.Trim());
                cmd.Parameters.AddWithValue("@ContainerWeightKg", ContainerTableDataListField.ContainerWeightKg.Trim());
                cmd.Parameters.AddWithValue("@ContainerCargoWeightKg", ContainerTableDataListField.ContainerCargoWeightKg.Trim());
                cmd.Parameters.AddWithValue("@ContainerCargoNature", ContainerTableDataListField.ContainerCargoNature.Trim());
                cmd.Parameters.AddWithValue("@ContainerNoofPackage", ContainerTableDataListField.ContainerNoofPackage.Trim());
                cmd.Parameters.AddWithValue("@ContainerFCLLCL", ContainerTableDataListField.ContainerFCLLCL.Trim());
                cmd.Parameters.AddWithValue("@ContainerPrimarySecondary", ContainerTableDataListField.ContainerPrimarySecondary.Trim());
                cmd.Parameters.AddWithValue("@ContainerGroupCode", ContainerTableDataListField.ContainerGroupCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerIMOCode", ContainerTableDataListField.ContainerIMOCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerUNNo", ContainerTableDataListField.ContainerUNNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerScanType", ContainerTableDataListField.ContainerScanType.Trim());
                cmd.Parameters.AddWithValue("@ContainerScanLocation", ContainerTableDataListField.ContainerScanLocation.Trim());
                cmd.Parameters.AddWithValue("@ContainerDeliveryMode", ContainerTableDataListField.ContainerDeliveryMode.Trim());
                cmd.Parameters.AddWithValue("@ContainerHold", ContainerTableDataListField.ContainerHold.Trim());
                cmd.Parameters.AddWithValue("@ContainerHoldRemarks", ContainerTableDataListField.ContainerHoldRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerHoldAgency", ContainerTableDataListField.ContainerHoldAgency.Trim());
                if (ContainerTableDataListField.ContainerHoldDate == "" || ContainerTableDataListField.ContainerHoldDate == null) { cmd.Parameters.AddWithValue("@ContainerHoldDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@ContainerHoldDate", Convert.ToDateTime(ContainerTableDataListField.ContainerHoldDate.Trim()).Date); }
                if (ContainerTableDataListField.ContainerReleaseDate == "" || ContainerTableDataListField.ContainerReleaseDate == null)
                {
                    cmd.Parameters.AddWithValue("@ContainerReleaseDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReleaseBy", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContainerReleaseDate", Convert.ToDateTime(ContainerTableDataListField.ContainerReleaseDate.Trim()).Date);
                    cmd.Parameters.AddWithValue("@ReleaseBy", HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString());
                }
                cmd.Parameters.AddWithValue("@ContainerReleaseRemarks", ContainerTableDataListField.ContainerReleaseRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerClaimDetails", ContainerTableDataListField.ContainerClaimDetails.Trim());
                cmd.Parameters.AddWithValue("@ContainerClaimAmount", ContainerTableDataListField.ContainerClaimAmount.Trim());
                if (ContainerTableDataListField.ContainerPaymentDate == "" || ContainerTableDataListField.ContainerPaymentDate == null) { cmd.Parameters.AddWithValue("@ContainerPaymentDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@ContainerPaymentDate", Convert.ToDateTime(ContainerTableDataListField.ContainerPaymentDate.Trim()).Date); }
                cmd.Parameters.AddWithValue("@ContainerRemarks", ContainerTableDataListField.ContainerRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerWHLoc", ContainerTableDataListField.ContainerWHLoc.Trim());
                cmd.Parameters.AddWithValue("@ContainerPriority", ContainerTableDataListField.ContainerPriority.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                if (ContainerTableDataListField.ContainerHold.Trim() == "Yes")
                {
                    cmd.Parameters.AddWithValue("@HoldBy", HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString());
                }
                else { cmd.Parameters.AddWithValue("@HoldBy", DBNull.Value); }
                cmd.Parameters.AddWithValue("@ComputerName", "");
                cmd.Parameters.AddWithValue("@IPAddress", "");
                cmd.Parameters.AddWithValue("@Location", "");
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                InsertOrUpdateUmatchedValue("CFSImportContainer", ContainerTableDataListField.ContainerNo.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            }

            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class ContainerTableDataList
    {
        public string MainJobNo { get; set; }
        public string ContainerItemNo { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerIMOCode { get; set; }
        public string ContainerISOCode { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public string ContainerSealNo { get; set; }
        public string ContainerTareWeight { get; set; }
        public string ContainerWeightKg { get; set; }
        public string ContainerCargoWeightKg { get; set; }
        public string ContainerCargoNature { get; set; }
        public string ContainerNoofPackage { get; set; }
        public string ContainerFCLLCL { get; set; }
        public string ContainerPrimarySecondary { get; set; }
        public string ContainerGroupCode { get; set; }
        public string ContainerUNNo { get; set; }
        public string ContainerScanType { get; set; }
        public string ContainerScanLocation { get; set; }
        public string ContainerDeliveryMode { get; set; }
        public string ContainerHold { get; set; }
        public string ContainerHoldRemarks { get; set; }
        public string ContainerHoldAgency { get; set; }
        public string ContainerHoldDate { get; set; }
        public string ContainerReleaseDate { get; set; }
        public string ContainerReleaseRemarks { get; set; }
        public string ContainerClaimDetails { get; set; }
        public string ContainerClaimAmount { get; set; }
        public string ContainerPaymentDate { get; set; }
        public string ContainerRemarks { get; set; }
        public string ContainerWHLoc { get; set; }
        public string ContainerPriority { get; set; }

        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------ContainerTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ContainerTableUpdateData(List<ContainerTableDataList> ContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<ContainerTableDataList> DetailedList = new List<ContainerTableDataList>();
        ContainerTableDataList DetailedListValue = new ContainerTableDataList();
        try
        {
            foreach (var ContainerTableDataListField in ContainerTableDataList)
            {
                cmd = new SqlCommand("SELECT * FROM CFSImportContainer WHERE MainJobNo = @MainJobNo and ContainerItemNo = @ContainerItemNo And ContainerNo = @ContainerNo and RecordStatus='Active'", cnn);
                cmd.Parameters.AddWithValue("@MainJobNo", ContainerTableDataListField.MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerNo", ContainerTableDataListField.ContainerNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerItemNo", ContainerTableDataListField.ContainerItemNo.Trim());
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dtrow in dt.Rows)
                {
                    DetailedListValue.ContainerItemNo = dtrow["ContainerItemNo"].ToString();
                    DetailedListValue.ContainerNo = dtrow["ContainerNo"].ToString();
                    DetailedListValue.ContainerISOCode = dtrow["ContainerISOCode"].ToString();
                    DetailedListValue.ContainerSize = dtrow["ContainerSize"].ToString();
                    DetailedListValue.ContainerType = dtrow["ContainerType"].ToString();
                    DetailedListValue.ContainerSealNo = dtrow["ContainerSealNo"].ToString();
                    DetailedListValue.ContainerTareWeight = dtrow["ContainerTareWeight"].ToString();
                    DetailedListValue.ContainerWeightKg = dtrow["ContainerWeightKg"].ToString();
                    DetailedListValue.ContainerCargoWeightKg = dtrow["ContainerCargoWeightKg"].ToString();
                    DetailedListValue.ContainerCargoNature = dtrow["ContainerCargoNature"].ToString();
                    DetailedListValue.ContainerNoofPackage = dtrow["ContainerNoofPackage"].ToString();
                    DetailedListValue.ContainerFCLLCL = dtrow["ContainerFCLLCL"].ToString();
                    DetailedListValue.ContainerPrimarySecondary = dtrow["ContainerPrimarySecondary"].ToString();
                    DetailedListValue.ContainerGroupCode = dtrow["ContainerGroupCode"].ToString();
                    DetailedListValue.ContainerIMOCode = dtrow["ContainerIMOCode"].ToString();
                    DetailedListValue.ContainerUNNo = dtrow["ContainerUNNo"].ToString();
                    DetailedListValue.ContainerScanType = dtrow["ContainerScanType"].ToString();
                    DetailedListValue.ContainerScanLocation = dtrow["ContainerScanLocation"].ToString();
                    DetailedListValue.ContainerDeliveryMode = dtrow["ContainerDeliveryMode"].ToString();
                    DetailedListValue.ContainerHold = dtrow["ContainerHold"].ToString();
                    DetailedListValue.ContainerHoldRemarks = dtrow["ContainerHoldRemarks"].ToString();
                    DetailedListValue.ContainerHoldAgency = dtrow["ContainerHoldAgency"].ToString();
                    DetailedListValue.ContainerHoldDate = dtrow["ContainerHoldDate"].ToString();
                    if (dtrow["ContainerHoldDate"].ToString() != "" || dtrow["ContainerHoldDate"] != DBNull.Value) { DetailedListValue.ContainerHoldDate = Convert.ToDateTime(dtrow["ContainerHoldDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.ContainerHoldDate = ""; }
                    DetailedListValue.ContainerReleaseDate = dtrow["ContainerReleaseDate"].ToString();
                    if (dtrow["ContainerReleaseDate"].ToString() != "" || dtrow["ContainerReleaseDate"] != DBNull.Value) { DetailedListValue.ContainerReleaseDate = Convert.ToDateTime(dtrow["ContainerReleaseDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.ContainerReleaseDate = ""; }
                    DetailedListValue.ContainerReleaseRemarks = dtrow["ContainerReleaseRemarks"].ToString();
                    DetailedListValue.ContainerClaimDetails = dtrow["ContainerClaimDetails"].ToString();
                    DetailedListValue.ContainerClaimAmount = dtrow["ContainerClaimAmount"].ToString();
                    DetailedListValue.ContainerPaymentDate = dtrow["ContainerPaymentDate"].ToString();
                    if (dtrow["ContainerPaymentDate"].ToString() != "" || dtrow["ContainerPaymentDate"] != DBNull.Value) { DetailedListValue.ContainerPaymentDate = Convert.ToDateTime(dtrow["ContainerPaymentDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.ContainerPaymentDate = ""; }
                    DetailedListValue.ContainerRemarks = dtrow["ContainerRemarks"].ToString();
                    DetailedListValue.ContainerWHLoc = dtrow["ContainerWHLoc"].ToString();
                    DetailedListValue.ContainerPriority = dtrow["ContainerPriority"].ToString();
                }
                cnn.Close();

                cmd = new SqlCommand("Update CFSImportContainer set ReleaseBy = @ReleaseBy, HoldBy = @HoldBy, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, ContainerIMOCode=@ContainerIMOCode,ContainerWeightKg=@ContainerWeightKg,ContainerISOCode=@ContainerISOCode,ContainerSize=@ContainerSize,ContainerType=@ContainerType,ContainerSealNo=@ContainerSealNo,ContainerTareWeight=@ContainerTareWeight,ContainerCargoWeightKg=@ContainerCargoWeightKg,ContainerCargoNature=@ContainerCargoNature,ContainerNoofPackage=@ContainerNoofPackage,ContainerFCLLCL=@ContainerFCLLCL,ContainerPrimarySecondary=@ContainerPrimarySecondary,ContainerGroupCode=@ContainerGroupCode,ContainerUNNo=@ContainerUNNo,ContainerScanType=@ContainerScanType,ContainerScanLocation=@ContainerScanLocation,ContainerDeliveryMode=@ContainerDeliveryMode,ContainerHold=@ContainerHold,ContainerHoldRemarks=@ContainerHoldRemarks,ContainerHoldAgency=@ContainerHoldAgency,ContainerHoldDate=@ContainerHoldDate,ContainerReleaseDate=@ContainerReleaseDate,ContainerReleaseRemarks=@ContainerReleaseRemarks,ContainerClaimDetails=@ContainerClaimDetails,ContainerClaimAmount=@ContainerClaimAmount,ContainerPaymentDate=@ContainerPaymentDate,ContainerRemarks=@ContainerRemarks,ContainerWHLoc=@ContainerWHLoc,ContainerPriority=@ContainerPriority" +
                                     " where ContainerNo=@ContainerNo And ContainerItemNo = @ContainerItemNo And MainJobNo = @MainJobNo And RecordStatus = 'Active'", cnn);
                cmd.Parameters.AddWithValue("@MainJobNo", ContainerTableDataListField.MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerItemNo", ContainerTableDataListField.ContainerItemNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerNo", ContainerTableDataListField.ContainerNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerISOCode", ContainerTableDataListField.ContainerISOCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerSize", ContainerTableDataListField.ContainerSize.Trim());
                cmd.Parameters.AddWithValue("@ContainerType", ContainerTableDataListField.ContainerType.Trim());
                cmd.Parameters.AddWithValue("@ContainerSealNo", ContainerTableDataListField.ContainerSealNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerTareWeight", ContainerTableDataListField.ContainerTareWeight.Trim());
                cmd.Parameters.AddWithValue("@ContainerWeightKg", ContainerTableDataListField.ContainerWeightKg.Trim());
                cmd.Parameters.AddWithValue("@ContainerCargoWeightKg", ContainerTableDataListField.ContainerCargoWeightKg.Trim());
                cmd.Parameters.AddWithValue("@ContainerCargoNature", ContainerTableDataListField.ContainerCargoNature.Trim());
                cmd.Parameters.AddWithValue("@ContainerNoofPackage", ContainerTableDataListField.ContainerNoofPackage.Trim());
                cmd.Parameters.AddWithValue("@ContainerFCLLCL", ContainerTableDataListField.ContainerFCLLCL.Trim());
                cmd.Parameters.AddWithValue("@ContainerPrimarySecondary", ContainerTableDataListField.ContainerPrimarySecondary.Trim());
                cmd.Parameters.AddWithValue("@ContainerGroupCode", ContainerTableDataListField.ContainerGroupCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerIMOCode", ContainerTableDataListField.ContainerIMOCode.Trim());
                cmd.Parameters.AddWithValue("@ContainerUNNo", ContainerTableDataListField.ContainerUNNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerScanType", ContainerTableDataListField.ContainerScanType.Trim());
                cmd.Parameters.AddWithValue("@ContainerScanLocation", ContainerTableDataListField.ContainerScanLocation.Trim());
                cmd.Parameters.AddWithValue("@ContainerDeliveryMode", ContainerTableDataListField.ContainerDeliveryMode.Trim());
                cmd.Parameters.AddWithValue("@ContainerHold", ContainerTableDataListField.ContainerHold.Trim());
                cmd.Parameters.AddWithValue("@ContainerHoldRemarks", ContainerTableDataListField.ContainerHoldRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerHoldAgency", ContainerTableDataListField.ContainerHoldAgency.Trim());
                if (ContainerTableDataListField.ContainerHold.Trim() == "Yes")
                {
                    cmd.Parameters.AddWithValue("@HoldBy", HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString());
                }
                else { cmd.Parameters.AddWithValue("@HoldBy", DBNull.Value); }
                if (ContainerTableDataListField.ContainerHoldDate == "" || ContainerTableDataListField.ContainerHoldDate == null)
                {
                    cmd.Parameters.AddWithValue("@ContainerHoldDate", DBNull.Value);
                    ContainerTableDataListField.ContainerHoldDate = "";
                }
                else
                {
                    cmd.Parameters.AddWithValue("ContainerHoldDate", Convert.ToDateTime(ContainerTableDataListField.ContainerHoldDate.Trim()).Date);
                    ContainerTableDataListField.ContainerHoldDate = Convert.ToDateTime(ContainerTableDataListField.ContainerHoldDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (ContainerTableDataListField.ContainerReleaseDate == "" || ContainerTableDataListField.ContainerReleaseDate == null)
                {
                    cmd.Parameters.AddWithValue("@ContainerReleaseDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReleaseBy", DBNull.Value);
                    ContainerTableDataListField.ContainerReleaseDate = "";
                }
                else
                {
                    cmd.Parameters.AddWithValue("ContainerReleaseDate", Convert.ToDateTime(ContainerTableDataListField.ContainerReleaseDate.Trim()).Date);
                    cmd.Parameters.AddWithValue("@ReleaseBy", HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString());
                    ContainerTableDataListField.ContainerReleaseDate = Convert.ToDateTime(ContainerTableDataListField.ContainerReleaseDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                cmd.Parameters.AddWithValue("@ContainerReleaseRemarks", ContainerTableDataListField.ContainerReleaseRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerClaimDetails", ContainerTableDataListField.ContainerClaimDetails.Trim());
                cmd.Parameters.AddWithValue("@ContainerClaimAmount", ContainerTableDataListField.ContainerClaimAmount.Trim());
                if (ContainerTableDataListField.ContainerPaymentDate == "" || ContainerTableDataListField.ContainerPaymentDate == null)
                {
                    cmd.Parameters.AddWithValue("@ContainerPaymentDate", DBNull.Value);
                    ContainerTableDataListField.ContainerPaymentDate = "";
                }
                else
                {
                    cmd.Parameters.AddWithValue("ContainerPaymentDate", Convert.ToDateTime(ContainerTableDataListField.ContainerPaymentDate.Trim()).Date);
                    ContainerTableDataListField.ContainerPaymentDate = Convert.ToDateTime(ContainerTableDataListField.ContainerPaymentDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                cmd.Parameters.AddWithValue("@ContainerRemarks", ContainerTableDataListField.ContainerRemarks.Trim());
                cmd.Parameters.AddWithValue("@ContainerWHLoc", ContainerTableDataListField.ContainerWHLoc.Trim());
                cmd.Parameters.AddWithValue("@ContainerPriority", ContainerTableDataListField.ContainerPriority.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@ComputerName", "");
                cmd.Parameters.AddWithValue("@IPAddress", "");
                cmd.Parameters.AddWithValue("@Location", "");
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();


                foreach (DataRow dtrow in dt.Rows)
                {
                    if (DetailedListValue.ContainerItemNo != ContainerTableDataListField.ContainerItemNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerItemNo", DetailedListValue.ContainerItemNo,
                        ContainerTableDataListField.ContainerItemNo.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerNo != ContainerTableDataListField.ContainerNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerNo", DetailedListValue.ContainerNo,
                        ContainerTableDataListField.ContainerNo.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerISOCode != ContainerTableDataListField.ContainerISOCode.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerISOCode", DetailedListValue.ContainerISOCode,
                        ContainerTableDataListField.ContainerISOCode.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerSize != ContainerTableDataListField.ContainerSize.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerSize", DetailedListValue.ContainerSize,
                        ContainerTableDataListField.ContainerSize.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerType != ContainerTableDataListField.ContainerType.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerType", DetailedListValue.ContainerType,
                        ContainerTableDataListField.ContainerType.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerSealNo != ContainerTableDataListField.ContainerSealNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerSealNo", DetailedListValue.ContainerSealNo,
                        ContainerTableDataListField.ContainerSealNo.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerTareWeight != ContainerTableDataListField.ContainerTareWeight.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerTareWeight", DetailedListValue.ContainerTareWeight,
                        ContainerTableDataListField.ContainerTareWeight.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerWeightKg != ContainerTableDataListField.ContainerWeightKg.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerWeightKg", DetailedListValue.ContainerWeightKg,
                        ContainerTableDataListField.ContainerWeightKg.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerCargoWeightKg != ContainerTableDataListField.ContainerCargoWeightKg.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerCargoWeightKg", DetailedListValue.ContainerCargoWeightKg,
                        ContainerTableDataListField.ContainerCargoWeightKg.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerCargoNature != ContainerTableDataListField.ContainerCargoNature.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerCargoNature", DetailedListValue.ContainerCargoNature,
                        ContainerTableDataListField.ContainerCargoNature.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerNoofPackage != ContainerTableDataListField.ContainerNoofPackage.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerNoofPackage", DetailedListValue.ContainerNoofPackage,
                        ContainerTableDataListField.ContainerNoofPackage.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerFCLLCL != ContainerTableDataListField.ContainerFCLLCL.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerFCLLCL", DetailedListValue.ContainerFCLLCL,
                        ContainerTableDataListField.ContainerFCLLCL.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerPrimarySecondary != ContainerTableDataListField.ContainerPrimarySecondary.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerPrimarySecondary", DetailedListValue.ContainerPrimarySecondary,
                        ContainerTableDataListField.ContainerPrimarySecondary.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerGroupCode != ContainerTableDataListField.ContainerGroupCode.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerGroupCode", DetailedListValue.ContainerGroupCode,
                        ContainerTableDataListField.ContainerGroupCode.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerIMOCode != ContainerTableDataListField.ContainerIMOCode.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerIMOCode", DetailedListValue.ContainerIMOCode,
                        ContainerTableDataListField.ContainerIMOCode.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerUNNo != ContainerTableDataListField.ContainerUNNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerUNNo", DetailedListValue.ContainerUNNo,
                        ContainerTableDataListField.ContainerUNNo.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerScanType != ContainerTableDataListField.ContainerScanType.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerScanType", DetailedListValue.ContainerScanType,
                        ContainerTableDataListField.ContainerScanType.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerScanLocation != ContainerTableDataListField.ContainerScanLocation.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerScanLocation", DetailedListValue.ContainerScanLocation,
                        ContainerTableDataListField.ContainerScanLocation.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerDeliveryMode != ContainerTableDataListField.ContainerDeliveryMode.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerDeliveryMode", DetailedListValue.ContainerDeliveryMode,
                        ContainerTableDataListField.ContainerDeliveryMode.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerHold != ContainerTableDataListField.ContainerHold.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerHold", DetailedListValue.ContainerHold,
                        ContainerTableDataListField.ContainerHold.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerHoldRemarks != ContainerTableDataListField.ContainerHoldRemarks.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerHoldRemarks", DetailedListValue.ContainerHoldRemarks,
                        ContainerTableDataListField.ContainerHoldRemarks.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerHoldAgency != ContainerTableDataListField.ContainerHoldAgency.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerHoldAgency", DetailedListValue.ContainerHoldAgency,
                        ContainerTableDataListField.ContainerHoldAgency.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerHoldDate != ContainerTableDataListField.ContainerHoldDate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerHoldDate", DetailedListValue.ContainerHoldDate,
                        ContainerTableDataListField.ContainerHoldDate.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerReleaseDate != ContainerTableDataListField.ContainerReleaseDate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerReleaseDate", DetailedListValue.ContainerReleaseDate,
                        ContainerTableDataListField.ContainerReleaseDate.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerReleaseRemarks != ContainerTableDataListField.ContainerReleaseRemarks.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerReleaseRemarks", DetailedListValue.ContainerReleaseRemarks,
                        ContainerTableDataListField.ContainerReleaseRemarks.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerClaimDetails != ContainerTableDataListField.ContainerClaimDetails.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerClaimDetails", DetailedListValue.ContainerClaimDetails,
                        ContainerTableDataListField.ContainerClaimDetails.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerClaimAmount != ContainerTableDataListField.ContainerClaimAmount.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerClaimAmount", DetailedListValue.ContainerClaimAmount,
                        ContainerTableDataListField.ContainerClaimAmount.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerPaymentDate != ContainerTableDataListField.ContainerPaymentDate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerPaymentDate", DetailedListValue.ContainerPaymentDate,
                        ContainerTableDataListField.ContainerPaymentDate.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerRemarks != ContainerTableDataListField.ContainerRemarks.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerRemarks", DetailedListValue.ContainerRemarks,
                        ContainerTableDataListField.ContainerRemarks.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerWHLoc != ContainerTableDataListField.ContainerWHLoc.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerWHLoc", DetailedListValue.ContainerWHLoc,
                        ContainerTableDataListField.ContainerWHLoc.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.ContainerPriority != ContainerTableDataListField.ContainerPriority.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportContainer",
                        ContainerTableDataListField.ContainerItemNo.Trim(), "ContainerPriority", DetailedListValue.ContainerPriority,
                        ContainerTableDataListField.ContainerPriority.Trim(), "Modified", "MainJobNo = '" + ContainerTableDataListField.MainJobNo.Trim() + "' And " +
                        "ContainerItemNo = '" + ContainerTableDataListField.ContainerItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                }
            }

            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------ContainerTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ContainerTableCancelData(List<ContainerTableDataList> ContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Update CFSImportContainer set RecordStatus = 'Cancelled' where ContainerNo = @ContainerNo And ContainerItemNo = @ContainerItemNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@ContainerItemNo", ContainerTableDataList[0].ContainerItemNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", ContainerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerNo", ContainerTableDataList[0].ContainerNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportContainer", ContainerTableDataList[0].ContainerNo.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static ContainerTableDataList[] ContainerTableSearchData(List<ContainerTableDataList> ContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<ContainerTableDataList> DetailedList = new List<ContainerTableDataList>();
        ContainerTableDataList DetailedListValue = new ContainerTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportContainer Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ContainerTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new ContainerTableDataList();
                DetailedListValue.ContainerItemNo = dtrow["ContainerItemNo"].ToString();
                DetailedListValue.ContainerNo = dtrow["ContainerNo"].ToString();
                DetailedListValue.ContainerISOCode = dtrow["ContainerISOCode"].ToString();
                DetailedListValue.ContainerSize = dtrow["ContainerSize"].ToString();
                DetailedListValue.ContainerType = dtrow["ContainerType"].ToString();
                DetailedListValue.ContainerSealNo = dtrow["ContainerSealNo"].ToString();
                DetailedListValue.ContainerTareWeight = dtrow["ContainerTareWeight"].ToString();
                DetailedListValue.ContainerWeightKg = dtrow["ContainerWeightKg"].ToString();
                DetailedListValue.ContainerCargoWeightKg = dtrow["ContainerCargoWeightKg"].ToString();
                DetailedListValue.ContainerCargoNature = dtrow["ContainerCargoNature"].ToString();
                DetailedListValue.ContainerNoofPackage = dtrow["ContainerNoofPackage"].ToString();
                DetailedListValue.ContainerFCLLCL = dtrow["ContainerFCLLCL"].ToString();
                DetailedListValue.ContainerPrimarySecondary = dtrow["ContainerPrimarySecondary"].ToString();
                DetailedListValue.ContainerGroupCode = dtrow["ContainerGroupCode"].ToString();
                DetailedListValue.ContainerIMOCode = dtrow["ContainerIMOCode"].ToString();
                DetailedListValue.ContainerUNNo = dtrow["ContainerUNNo"].ToString();
                DetailedListValue.ContainerScanType = dtrow["ContainerScanType"].ToString();
                DetailedListValue.ContainerScanLocation = dtrow["ContainerScanLocation"].ToString();
                DetailedListValue.ContainerDeliveryMode = dtrow["ContainerDeliveryMode"].ToString();
                DetailedListValue.ContainerHold = dtrow["ContainerHold"].ToString();
                DetailedListValue.ContainerHoldRemarks = dtrow["ContainerHoldRemarks"].ToString();
                DetailedListValue.ContainerHoldAgency = dtrow["ContainerHoldAgency"].ToString();
                DetailedListValue.ContainerHoldDate = String.Format("{0:yyyy-MM-dd}", dtrow["ContainerHoldDate"]);
                DetailedListValue.ContainerReleaseDate = String.Format("{0:yyyy-MM-dd}", dtrow["ContainerReleaseDate"]);
                DetailedListValue.ContainerReleaseRemarks = dtrow["ContainerReleaseRemarks"].ToString();
                DetailedListValue.ContainerClaimDetails = dtrow["ContainerClaimDetails"].ToString();
                DetailedListValue.ContainerClaimAmount = dtrow["ContainerClaimAmount"].ToString();
                DetailedListValue.ContainerPaymentDate = String.Format("{0:yyyy-MM-dd}", dtrow["ContainerPaymentDate"]);
                DetailedListValue.ContainerRemarks = dtrow["ContainerRemarks"].ToString();
                DetailedListValue.ContainerWHLoc = dtrow["ContainerWHLoc"].ToString();
                DetailedListValue.ContainerPriority = dtrow["ContainerPriority"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //----------------------------------------------------------TSA & DRF Data-----------------------------------------------------------------------
    public class OutputList1
    {
        public string ItemNo { get; set; }
        public string LinerCode { get; set; }
        public string TSANumber { get; set; }
        public string LinerJobNo { get; set; }
        public string TSADate { get; set; }
        public string tabname { get; set; }
        public string colsname { get; set; }
        public string RenameValue { get; set; }
        public string ReturnedValue { get; set; }
    }
    public class OutputList2
    {
        public string DRFNo { get; set; }
        public string DRFIssuedDate { get; set; }
        public string TransportName { get; set; }
        public string ContainerNumber { get; set; }
        public string ContSize { get; set; }
        public string ContScantype { get; set; }
        public string ContScanLoc { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string Operator { get; set; }
        public string ColumnValue { get; set; }
        public string TableName { get; set; }
        public string ReturnedValue { get; set; }
    }

    [WebMethod(EnableSession = true)]
    public static Tuple<List<OutputList1>, List<OutputList2>> TSADocumentUploadTableSearchData(string MainIGMNo)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();

        List<OutputList1> DetailedList1 = new List<OutputList1>();
        List<OutputList2> DetailedList2 = new List<OutputList2>();
        OutputList1 DetailedListValue1 = new OutputList1();
        OutputList2 DetailedListValue2 = new OutputList2();

        try
        {
            cmd = new SqlCommand("Select MainJobNo, LinerItemNo, LinerLinerAgent, LinerTSANo, LinerTSADate From CFSImportLiner Where MainJobNo IN (Select MainJobNo From CFSImport Where MainIGMNo = @MainIGMNo And RecordStatus = 'Active') And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@MainIGMNo", MainIGMNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue1.ReturnedValue = "No Data Found!!!";
                DetailedList1.Add(DetailedListValue1);
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue1 = new OutputList1();
                DetailedListValue1.LinerJobNo = dtrow["MainJobNo"].ToString();
                DetailedListValue1.ItemNo = dtrow["LinerItemNo"].ToString();
                DetailedListValue1.LinerCode = dtrow["LinerLinerAgent"].ToString();
                DetailedListValue1.TSANumber = dtrow["LinerTSANo"].ToString();
                DetailedListValue1.TSADate = String.Format("{0:yyyy-MM-dd}", dtrow["LinerTSADate"]);
                DetailedList1.Add(DetailedListValue1);
            }
            cnn.Close();

            cmd = new SqlCommand("Select B.DRFNo, B.DRFIssuedDate, B.TransportName, A.ContainerNo, A.ContainerSize, ContainerScanType, ContainerScanLocation From CFSImportContainer A " +
                                 "Left Join CFSImportDRFScanListUpload B On A.ContainerNo = B.ContainerNo " +
                                 "Where A.MainJobNo IN (Select MainJobNo From CFSImport Where MainIGMNo = @MainIGMNo And RecordStatus = 'Active') And A.RecordStatus = 'Active';", cnn);
            cmd.Parameters.AddWithValue("@MainIGMNo", MainIGMNo.Trim());
            cnn.Open();
            da = new SqlDataAdapter(cmd); dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue2.ReturnedValue = "No Data Found!!!";
                DetailedList2.Add(DetailedListValue2);
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue2 = new OutputList2();
                DetailedListValue2.DRFNo = dtrow["DRFNo"].ToString();
                DetailedListValue2.DRFIssuedDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["DRFIssuedDate"]);
                DetailedListValue2.TransportName = dtrow["TransportName"].ToString();
                DetailedListValue2.ContainerNumber = dtrow["ContainerNo"].ToString();
                DetailedListValue2.ContSize = dtrow["ContainerSize"].ToString();
                DetailedListValue2.ContScantype = dtrow["ContainerScanType"].ToString();
                DetailedListValue2.ContScanLoc = dtrow["ContainerScanLocation"].ToString();
                DetailedList2.Add(DetailedListValue2);
            }
            cnn.Close();
        }
        catch (Exception ex)
        {
            DetailedListValue1.ReturnedValue = ex.Message;
            DetailedList1.Add(DetailedListValue1);
        }

        return Tuple.Create(DetailedList1, DetailedList2);
    }

    //----------------------------------------------------------TSA & DRF Data-----------------------------------------------------------------------
    //-----------------------------------------------------------DocumentUploadTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string DocumentUploadTableInsertData(List<DocumentUploadTableDataList> DocumentUploadTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            foreach (var FileDatasFields in DocumentUploadTableDataList)
            {
                cmd = new SqlCommand("Insert into CFSImportDocumentUpload (JobNo, FileName, ActualFile, FileLink, UpdatedBy) Values (@JobNo, @FileName, @ActualFile, @FileLink, @UpdatedBy)", cnn);
                cmd.Parameters.AddWithValue("@JobNo", FileDatasFields.JobNo.Trim());
                cmd.Parameters.AddWithValue("@FileName", FileDatasFields.DummyFileName.Trim());
                cmd.Parameters.AddWithValue("@ActualFile", FileDatasFields.ActFileName.Trim());
                cmd.Parameters.AddWithValue("@FileLink", FileDatasFields.FileLink.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                InsertOrUpdateUmatchedValue("CFSImportDocumentUpload", FileDatasFields.DummyFileName.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            }
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class DocumentUploadTableDataList
    {
        public string JobNo { get; set; }
        public string ID { get; set; }
        public string DummyFileName { get; set; }
        public string ActFileName { get; set; }
        public string FileLink { get; set; }
        public string ReturnedValue { get; set; }
        public string FilePath { get; set; }
    }
    //-----------------------------------------------------------DocumentUploadTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string DocumentUploadTableCancelData(List<DocumentUploadTableDataList> DocumentUploadTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr), cnnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Select JobNo, ActualFile From CFSImportDocumentUpload Where FileId = @FileId And RecordStatus = 'Active'", cnnn);
            cmd.Parameters.AddWithValue("@FileId", DocumentUploadTableDataList[0].ID.Trim());
            cnnn.Open();
            SqlDataReader sqlrd = cmd.ExecuteReader();
            while (sqlrd.Read())
            {
                //string path = @"C:/Users/LOGISTICSA-04/OneDrive - ICSA I Pvt Ltd/ICSA Project BackUp/LogistICSA/LogistICSA/CFS/CFS Files/Import/" + sqlrd["JobNo"].ToString().Replace("/", "-") + "/" + sqlrd["ActualFile"].ToString() + ""; // Local Path
                string path = @"D:/Production/LogistICSA/CFS/CFS Files/Import/" + sqlrd["JobNo"].ToString().Replace("/", "-") + "/" + sqlrd["ActualFile"].ToString() + ""; //Server Path

                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exist or not
                {
                    file.Delete();

                    cmd = new SqlCommand("Update CFSImportDocumentUpload set RecordStatus = 'Cancelled' where FileId = @FileId And JobNo = @MainJobNo", cnn);
                    cmd.Parameters.AddWithValue("@FileId", DocumentUploadTableDataList[0].ID.Trim());
                    cmd.Parameters.AddWithValue("@MainJobNo", DocumentUploadTableDataList[0].JobNo.Trim());
                    cmd.Parameters.AddWithValue("@UpdatedBy", "");
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    InsertOrUpdateUmatchedValue("CFSImportDocumentUpload", DocumentUploadTableDataList[0].DummyFileName.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
                    cnn.Close();

                    return "Cancelled";
                }
                else
                {
                    return "File does not exist or the path is invalid.";
                }
            }
            cnnn.Close();

            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static DocumentUploadTableDataList[] DocumentUploadTableSearchData(List<DocumentUploadTableDataList> DocumentUploadTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<DocumentUploadTableDataList> DetailedList = new List<DocumentUploadTableDataList>();
        DocumentUploadTableDataList DetailedListValue = new DocumentUploadTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportDocumentUpload Where RecordStatus = 'Active' and JobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", DocumentUploadTableDataList[0].JobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new DocumentUploadTableDataList();
                DetailedListValue.ID = dtrow["FileId"].ToString();
                DetailedListValue.DummyFileName = dtrow["FileName"].ToString();
                DetailedListValue.FileLink = dtrow["FileLink"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class TransportTableDataList
    {
        public string MainJobNo { get; set; }
        public string ContainerNo { get; set; }
        public string TransportVehicleNo { get; set; }
        public string TransportDRFNo { get; set; }
        public string TransportDRFIssuedDate { get; set; }
        public string TransportTransportName { get; set; }
        public string TransportTruckDeployDate { get; set; }
        public string TransportDriverName { get; set; }
        public string TransportDriverMobileNo { get; set; }
        public string TransportTerminalGateIn { get; set; }
        public string TransportTerminalGateOut { get; set; }
        public string TransportScanStatus { get; set; }
        public string TransportCustomsGateOut { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------TransportTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string TransportTableUpdateData(List<TransportTableDataList> TransportTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<TransportTableDataList> DetailedList = new List<TransportTableDataList>();
        TransportTableDataList DetailedListValue = new TransportTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportContainer WHERE MainJobNo = @MainJobNo and ContainerNo = @ContainerNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TransportTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerNo", TransportTableDataList[0].ContainerNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.ContainerNo = dtrow["ContainerNo"].ToString();
                DetailedListValue.TransportVehicleNo = dtrow["TransportVehicleNo"].ToString();
                DetailedListValue.TransportDRFNo = dtrow["TransportDRFNo"].ToString();
                DetailedListValue.TransportDRFIssuedDate = dtrow["TransportDRFIssuedDate"].ToString();
                if (dtrow["TransportDRFIssuedDate"].ToString() != "" || dtrow["TransportDRFIssuedDate"] != DBNull.Value) { DetailedListValue.TransportDRFIssuedDate = Convert.ToDateTime(dtrow["TransportDRFIssuedDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TransportDRFIssuedDate = ""; }
                DetailedListValue.TransportTransportName = dtrow["TransportTransportName"].ToString();
                DetailedListValue.TransportTruckDeployDate = dtrow["TransportTruckDeployDate"].ToString();
                if (dtrow["TransportTruckDeployDate"].ToString() != "" || dtrow["TransportTruckDeployDate"] != DBNull.Value) { DetailedListValue.TransportTruckDeployDate = Convert.ToDateTime(dtrow["TransportTruckDeployDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TransportTruckDeployDate = ""; }
                DetailedListValue.TransportDriverName = dtrow["TransportDriverName"].ToString();
                DetailedListValue.TransportDriverMobileNo = dtrow["TransportDriverMobileNo"].ToString();
                DetailedListValue.TransportTerminalGateIn = dtrow["TransportTerminalGateIn"].ToString();
                if (dtrow["TransportTerminalGateIn"].ToString() != "" || dtrow["TransportTerminalGateIn"] != DBNull.Value) { DetailedListValue.TransportTerminalGateIn = Convert.ToDateTime(dtrow["TransportTerminalGateIn"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TransportTerminalGateIn = ""; }
                DetailedListValue.TransportTerminalGateOut = dtrow["TransportTerminalGateOut"].ToString();
                if (dtrow["TransportTerminalGateOut"].ToString() != "" || dtrow["TransportTerminalGateOut"] != DBNull.Value) { DetailedListValue.TransportTerminalGateOut = Convert.ToDateTime(dtrow["TransportTerminalGateOut"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TransportTerminalGateOut = ""; }
                DetailedListValue.TransportScanStatus = dtrow["TransportScanStatus"].ToString();
                DetailedListValue.TransportCustomsGateOut = dtrow["TransportCustomsGateOut"].ToString();
                if (dtrow["TransportCustomsGateOut"].ToString() != "" || dtrow["TransportCustomsGateOut"] != DBNull.Value) { DetailedListValue.TransportCustomsGateOut = Convert.ToDateTime(dtrow["TransportCustomsGateOut"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TransportCustomsGateOut = ""; }
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportContainer set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, ContainerNo=@ContainerNo,TransportVehicleNo=@TransportVehicleNo,TransportDRFNo=@TransportDRFNo,TransportDRFIssuedDate=@TransportDRFIssuedDate,TransportTransportName=@TransportTransportName,TransportTruckDeployDate=@TransportTruckDeployDate,TransportDriverName=@TransportDriverName,TransportDriverMobileNo=@TransportDriverMobileNo,TransportTerminalGateIn=@TransportTerminalGateIn,TransportTerminalGateOut=@TransportTerminalGateOut,TransportScanStatus=@TransportScanStatus,TransportCustomsGateOut=@TransportCustomsGateOut where ContainerNo = @ContainerNo And MainJobNo = @MainJobNo And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TransportTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerNo", TransportTableDataList[0].ContainerNo.Trim());
            cmd.Parameters.AddWithValue("@TransportVehicleNo", TransportTableDataList[0].TransportVehicleNo.Trim());
            cmd.Parameters.AddWithValue("@TransportDRFNo", TransportTableDataList[0].TransportDRFNo.Trim());
            if (TransportTableDataList[0].TransportDRFIssuedDate == "" || TransportTableDataList[0].TransportDRFIssuedDate == null)
            {
                cmd.Parameters.AddWithValue("@TransportDRFIssuedDate", DBNull.Value);
                TransportTableDataList[0].TransportDRFIssuedDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TransportDRFIssuedDate", Convert.ToDateTime(TransportTableDataList[0].TransportDRFIssuedDate.Trim()));
                TransportTableDataList[0].TransportDRFIssuedDate = Convert.ToDateTime(TransportTableDataList[0].TransportDRFIssuedDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@TransportTransportName", TransportTableDataList[0].TransportTransportName.Trim());
            if (TransportTableDataList[0].TransportTruckDeployDate == "" || TransportTableDataList[0].TransportTruckDeployDate == null)
            {
                cmd.Parameters.AddWithValue("@TransportTruckDeployDate", DBNull.Value);
                TransportTableDataList[0].TransportTruckDeployDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TransportTruckDeployDate", Convert.ToDateTime(TransportTableDataList[0].TransportTruckDeployDate.Trim()));
                TransportTableDataList[0].TransportTruckDeployDate = Convert.ToDateTime(TransportTableDataList[0].TransportTruckDeployDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@TransportDriverName", TransportTableDataList[0].TransportDriverName.Trim());
            cmd.Parameters.AddWithValue("@TransportDriverMobileNo", TransportTableDataList[0].TransportDriverMobileNo.Trim());
            if (TransportTableDataList[0].TransportTerminalGateIn == "" || TransportTableDataList[0].TransportTerminalGateIn == null)
            {
                cmd.Parameters.AddWithValue("@TransportTerminalGateIn", DBNull.Value);
                TransportTableDataList[0].TransportTerminalGateIn = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TransportTerminalGateIn", Convert.ToDateTime(TransportTableDataList[0].TransportTerminalGateIn.Trim()));
                TransportTableDataList[0].TransportTerminalGateIn = Convert.ToDateTime(TransportTableDataList[0].TransportTerminalGateIn).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (TransportTableDataList[0].TransportTerminalGateOut == "" || TransportTableDataList[0].TransportTerminalGateOut == null)
            {
                cmd.Parameters.AddWithValue("@TransportTerminalGateOut", DBNull.Value);
                TransportTableDataList[0].TransportTerminalGateOut = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TransportTerminalGateOut", Convert.ToDateTime(TransportTableDataList[0].TransportTerminalGateOut.Trim()));
                TransportTableDataList[0].TransportTerminalGateOut = Convert.ToDateTime(TransportTableDataList[0].TransportTerminalGateOut).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@TransportScanStatus", TransportTableDataList[0].TransportScanStatus.Trim());
            if (TransportTableDataList[0].TransportCustomsGateOut == "" || TransportTableDataList[0].TransportCustomsGateOut == null)
            {
                cmd.Parameters.AddWithValue("@TransportCustomsGateOut", DBNull.Value);
                TransportTableDataList[0].TransportCustomsGateOut = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TransportCustomsGateOut", Convert.ToDateTime(TransportTableDataList[0].TransportCustomsGateOut.Trim()));
                TransportTableDataList[0].TransportCustomsGateOut = Convert.ToDateTime(TransportTableDataList[0].TransportCustomsGateOut).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.ContainerNo != TransportTableDataList[0].ContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "ContainerNo", DetailedListValue.ContainerNo,
                    TransportTableDataList[0].ContainerNo.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportVehicleNo != TransportTableDataList[0].TransportVehicleNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportVehicleNo", DetailedListValue.TransportVehicleNo,
                    TransportTableDataList[0].TransportVehicleNo.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportDRFNo != TransportTableDataList[0].TransportDRFNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportDRFNo", DetailedListValue.TransportDRFNo,
                    TransportTableDataList[0].TransportDRFNo.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportDRFIssuedDate != TransportTableDataList[0].TransportDRFIssuedDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportDRFIssuedDate", DetailedListValue.TransportDRFIssuedDate,
                    TransportTableDataList[0].TransportDRFIssuedDate.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportTransportName != TransportTableDataList[0].TransportTransportName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportTransportName", DetailedListValue.TransportTransportName,
                    TransportTableDataList[0].TransportTransportName.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportTruckDeployDate != TransportTableDataList[0].TransportTruckDeployDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportTruckDeployDate", DetailedListValue.TransportTruckDeployDate,
                    TransportTableDataList[0].TransportTruckDeployDate.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportDriverName != TransportTableDataList[0].TransportDriverName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportDriverName", DetailedListValue.TransportDriverName,
                    TransportTableDataList[0].TransportDriverName.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportDriverMobileNo != TransportTableDataList[0].TransportDriverMobileNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportDriverMobileNo", DetailedListValue.TransportDriverMobileNo,
                    TransportTableDataList[0].TransportDriverMobileNo.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportTerminalGateIn != TransportTableDataList[0].TransportTerminalGateIn.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportTerminalGateIn", DetailedListValue.TransportTerminalGateIn,
                    TransportTableDataList[0].TransportTerminalGateIn.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportTerminalGateOut != TransportTableDataList[0].TransportTerminalGateOut.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportTerminalGateOut", DetailedListValue.TransportTerminalGateOut,
                    TransportTableDataList[0].TransportTerminalGateOut.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportScanStatus != TransportTableDataList[0].TransportScanStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportScanStatus", DetailedListValue.TransportScanStatus,
                    TransportTableDataList[0].TransportScanStatus.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TransportCustomsGateOut != TransportTableDataList[0].TransportCustomsGateOut.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    TransportTableDataList[0].ContainerNo.Trim(), "TransportCustomsGateOut", DetailedListValue.TransportCustomsGateOut,
                    TransportTableDataList[0].TransportCustomsGateOut.Trim(), "Modified", "MainJobNo = '" + TransportTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + TransportTableDataList[0].ContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static TransportTableDataList[] TransportTableSearchData(List<TransportTableDataList> TransportTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<TransportTableDataList> DetailedList = new List<TransportTableDataList>();
        TransportTableDataList DetailedListValue = new TransportTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(@"select Distinct 
                                A.DRFNo As [TransportDRFNo],
                                Case When C.TransportVehicleNo <> '' And C.TransportVehicleNo Is Not Null Then A.ContainerNo Else '' End As [ContainerNo],
                                C.TransportVehicleNo, C.TransportDRFIssuedDate, C.TransportTransportName, C.TransportTruckDeployDate, C.TransportDriverName, C.TransportDriverMobileNo, 
                                C.TransportTerminalGateIn, C.TransportTerminalGateOut, C.LoadContScanStatus, C.TransportCustomsGateOut 
                                from CFSImportDRFScanListUpload A 
                                Left Join CFSImport B On A.IGMNo = B.MainIGMNo 
                                Left Join CFSImportContainer C On B.RecordStatus = C.RecordStatus And A.ContainerNo = C.ContainerNo 
                                Where C.RecordStatus = 'Active' And B.MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", TransportTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new TransportTableDataList();
                DetailedListValue.ContainerNo = dtrow["ContainerNo"].ToString();
                DetailedListValue.TransportVehicleNo = dtrow["TransportVehicleNo"].ToString();
                DetailedListValue.TransportDRFNo = dtrow["TransportDRFNo"].ToString();
                DetailedListValue.TransportDRFIssuedDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["TransportDRFIssuedDate"]);
                DetailedListValue.TransportTransportName = dtrow["TransportTransportName"].ToString();
                DetailedListValue.TransportTruckDeployDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["TransportTruckDeployDate"]);
                DetailedListValue.TransportDriverName = dtrow["TransportDriverName"].ToString();
                DetailedListValue.TransportDriverMobileNo = dtrow["TransportDriverMobileNo"].ToString();
                DetailedListValue.TransportTerminalGateIn = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["TransportTerminalGateIn"]);
                DetailedListValue.TransportTerminalGateOut = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["TransportTerminalGateOut"]);
                DetailedListValue.TransportScanStatus = dtrow["LoadContScanStatus"].ToString();
                DetailedListValue.TransportCustomsGateOut = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["TransportCustomsGateOut"]);
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------LoadedContainerTableInsertDataStartsHere-----------------------------------------------------------------   
    public class LoadedContainerTableDataList
    {
        public string MainJobNo { get; set; }
        public string LoadContContainerNo { get; set; }
        public string LoadContVehicleNo { get; set; }
        public string LoadContGateInPassNo { get; set; }
        public string LoadContGateInPassDate { get; set; }
        public string LoadContAgentSealNo { get; set; }
        public string LoadContScanStatus { get; set; }
        public string LoadContScanType { get; set; }
        public string LoadContScanLocation { get; set; }
        public string LoadContStatusType { get; set; }
        public string LoadContPluginRequired { get; set; }
        public string LoadContAdditionAgentSealNo { get; set; }
        public string LoadContODC { get; set; }
        public string LoadContODCDimension { get; set; }
        public string LoadContContainerTag { get; set; }
        public string LoadContDamageDetails { get; set; }
        public string LoadContPortorCustomSealNo { get; set; }
        public string LoadContAdditionPortorCustomSealNo { get; set; }
        public string LoadContModeofArrival { get; set; }
        public string LoadContTransportType { get; set; }
        public string LoadContEIRNo { get; set; }
        public string LoadContVehicleType { get; set; }
        public string LoadContContainerHold { get; set; }
        public string LoadContContainerHoldRemarks { get; set; }
        public string LoadContContainerHoldAgency { get; set; }
        public string LoadContContainerHoldDate { get; set; }
        public string LoadContTruckTag { get; set; }
        public string LoadContContainerCondition { get; set; }
        public string LoadContItemNo { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------LoadedContainerTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LoadedContainerTableUpdateData(List<LoadedContainerTableDataList> LoadedContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<LoadedContainerTableDataList> DetailedList = new List<LoadedContainerTableDataList>();
        LoadedContainerTableDataList DetailedListValue = new LoadedContainerTableDataList();
        try
        {
            if (LoadedContainerTableDataList[0].LoadContGateInPassNo == "" && LoadedContainerTableDataList[0].LoadContGateInPassDate == "")
            {
                cmd = new SqlCommand("Select ContainerId From CFSImportContainer Where MainJobNo = @MainJobNo And ContainerItemNo = @ContainerItemNo And ContainerNo = @ContainerNo And RecordStatus = 'Active'", cnn);
                cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerTableDataList[0].MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerItemNo", LoadedContainerTableDataList[0].LoadContItemNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerNo", LoadedContainerTableDataList[0].LoadContContainerNo.Trim());
                cnn.Open();
                SqlDataReader sqlrd = cmd.ExecuteReader();
                while (sqlrd.Read())
                {
                    LoadedContainerTableDataList[0].LoadContGateInPassNo = "LC_GATE_IN_PASS_" + sqlrd["ContainerId"].ToString();
                    LoadedContainerTableDataList[0].LoadContGateInPassDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                cnn.Close();
            }

            cmd = new SqlCommand("SELECT * FROM CFSImportContainer WHERE MainJobNo = @MainJobNo and ContainerItemNo = @ContainerItemNo And ContainerNo = @ContainerNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerItemNo", LoadedContainerTableDataList[0].LoadContItemNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerNo", LoadedContainerTableDataList[0].LoadContContainerNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.LoadContContainerNo = dtrow["ContainerNo"].ToString();
                DetailedListValue.LoadContVehicleNo = dtrow["LoadContVehicleNo"].ToString();
                DetailedListValue.LoadContGateInPassNo = dtrow["LoadContGateInPassNo"].ToString();
                DetailedListValue.LoadContGateInPassDate = dtrow["LoadContGateInPassDate"].ToString();
                if (dtrow["LoadContGateInPassDate"].ToString() != "" || dtrow["LoadContGateInPassDate"] != DBNull.Value) { DetailedListValue.LoadContGateInPassDate = Convert.ToDateTime(dtrow["LoadContGateInPassDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LoadContGateInPassDate = ""; }
                DetailedListValue.LoadContAgentSealNo = dtrow["LoadContAgentSealNo"].ToString();
                DetailedListValue.LoadContScanStatus = dtrow["LoadContScanStatus"].ToString();
                DetailedListValue.LoadContScanType = dtrow["ContainerScanType"].ToString();
                DetailedListValue.LoadContScanLocation = dtrow["ContainerScanLocation"].ToString();
                DetailedListValue.LoadContStatusType = dtrow["LoadContStatusType"].ToString();
                DetailedListValue.LoadContPluginRequired = dtrow["LoadContPluginRequired"].ToString();
                DetailedListValue.LoadContAdditionAgentSealNo = dtrow["LoadContAdditionAgentSealNo"].ToString();
                DetailedListValue.LoadContODC = dtrow["LoadContODC"].ToString();
                DetailedListValue.LoadContODCDimension = dtrow["LoadContODCDimension"].ToString();
                DetailedListValue.LoadContContainerTag = dtrow["LoadContContainerTag"].ToString();
                DetailedListValue.LoadContDamageDetails = dtrow["LoadContDamageDetails"].ToString();
                DetailedListValue.LoadContPortorCustomSealNo = dtrow["LoadContPortorCustomSealNo"].ToString();
                DetailedListValue.LoadContAdditionPortorCustomSealNo = dtrow["LoadContAdditionPortorCustomSealNo"].ToString();
                DetailedListValue.LoadContModeofArrival = dtrow["LoadContModeofArrival"].ToString();
                DetailedListValue.LoadContTransportType = dtrow["LoadContTransportType"].ToString();
                DetailedListValue.LoadContEIRNo = dtrow["LoadContEIRNo"].ToString();
                DetailedListValue.LoadContVehicleType = dtrow["LoadContVehicleType"].ToString();
                DetailedListValue.LoadContContainerHold = dtrow["ContainerHold"].ToString();
                DetailedListValue.LoadContContainerHoldAgency = dtrow["ContainerHoldAgency"].ToString();
                DetailedListValue.LoadContContainerHoldRemarks = dtrow["ContainerHoldRemarks"].ToString();
                if (dtrow["ContainerHoldDate"].ToString() != "" || dtrow["ContainerHoldDate"] != DBNull.Value) { DetailedListValue.LoadContContainerHoldDate = Convert.ToDateTime(dtrow["ContainerHoldDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LoadContContainerHoldDate = ""; }
                DetailedListValue.LoadContTruckTag = dtrow["LoadContTruckTag"].ToString();
                DetailedListValue.LoadContContainerCondition = dtrow["LoadContContainerCondition"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportContainer set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, LoadContVehicleNo=@LoadContVehicleNo,LoadContGateInPassNo=@LoadContGateInPassNo," +
                                 "LoadContGateInPassDate=@LoadContGateInPassDate,LoadContAgentSealNo=@LoadContAgentSealNo,LoadContScanStatus=@LoadContScanStatus," +
                                 "ContainerScanType=@LoadContScanType,ContainerScanLocation=@LoadContScanLocation,LoadContStatusType=@LoadContStatusType," +
                                 "LoadContPluginRequired=@LoadContPluginRequired,LoadContAdditionAgentSealNo=@LoadContAdditionAgentSealNo,LoadContODC=@LoadContODC," +
                                 "LoadContODCDimension=@LoadContODCDimension,LoadContContainerTag=@LoadContContainerTag,LoadContDamageDetails=@LoadContDamageDetails," +
                                 "LoadContPortorCustomSealNo=@LoadContPortorCustomSealNo,LoadContAdditionPortorCustomSealNo=@LoadContAdditionPortorCustomSealNo," +
                                 "LoadContModeofArrival=@LoadContModeofArrival,LoadContTransportType=@LoadContTransportType,LoadContEIRNo=@LoadContEIRNo," +
                                 "LoadContVehicleType=@LoadContVehicleType,LoadContTruckTag=@LoadContTruckTag,LoadContContainerCondition=@LoadContContainerCondition," +
                                 "ContainerHold=@ContainerHold,ContainerHoldRemarks=@ContainerHoldRemarks,ContainerHoldAgency=@ContainerHoldAgency," +
                                 "ContainerHoldDate=@ContainerHoldDate,ContainerStatus=@ContainerStatus " +
                                 "where ContainerNo = @LoadContContainerNo And ContainerItemNo = @ContainerItemNo And MainJobNo = @MainJobNo And RecordStatus = 'Active'", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContContainerNo", LoadedContainerTableDataList[0].LoadContContainerNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerItemNo", LoadedContainerTableDataList[0].LoadContItemNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContVehicleNo", LoadedContainerTableDataList[0].LoadContVehicleNo.Trim());
            if (LoadedContainerTableDataList[0].LoadContGateInPassNo.Trim() != "" && LoadedContainerTableDataList[0].LoadContGateInPassNo.Trim() != null) { cmd.Parameters.AddWithValue("@ContainerStatus", "Gated In"); }
            else { cmd.Parameters.AddWithValue("@ContainerStatus", ""); }
            cmd.Parameters.AddWithValue("@LoadContGateInPassNo", LoadedContainerTableDataList[0].LoadContGateInPassNo.Trim());
            if (LoadedContainerTableDataList[0].LoadContGateInPassDate == "" || LoadedContainerTableDataList[0].LoadContGateInPassDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadContGateInPassDate", DBNull.Value);
                LoadedContainerTableDataList[0].LoadContGateInPassDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadContGateInPassDate", Convert.ToDateTime(LoadedContainerTableDataList[0].LoadContGateInPassDate.Trim()));
                LoadedContainerTableDataList[0].LoadContGateInPassDate = Convert.ToDateTime(LoadedContainerTableDataList[0].LoadContGateInPassDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LoadContAgentSealNo", LoadedContainerTableDataList[0].LoadContAgentSealNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContScanStatus", LoadedContainerTableDataList[0].LoadContScanStatus.Trim());
            cmd.Parameters.AddWithValue("@LoadContScanType", LoadedContainerTableDataList[0].LoadContScanType.Trim());
            cmd.Parameters.AddWithValue("@LoadContScanLocation", LoadedContainerTableDataList[0].LoadContScanLocation.Trim());
            cmd.Parameters.AddWithValue("@LoadContStatusType", LoadedContainerTableDataList[0].LoadContStatusType.Trim());
            cmd.Parameters.AddWithValue("@LoadContPluginRequired", LoadedContainerTableDataList[0].LoadContPluginRequired.Trim());
            cmd.Parameters.AddWithValue("@LoadContAdditionAgentSealNo", LoadedContainerTableDataList[0].LoadContAdditionAgentSealNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContODC", LoadedContainerTableDataList[0].LoadContODC.Trim());
            cmd.Parameters.AddWithValue("@LoadContODCDimension", LoadedContainerTableDataList[0].LoadContODCDimension.Trim());
            cmd.Parameters.AddWithValue("@LoadContContainerTag", LoadedContainerTableDataList[0].LoadContContainerTag.Trim());
            cmd.Parameters.AddWithValue("@LoadContDamageDetails", LoadedContainerTableDataList[0].LoadContDamageDetails.Trim());
            cmd.Parameters.AddWithValue("@LoadContPortorCustomSealNo", LoadedContainerTableDataList[0].LoadContPortorCustomSealNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContAdditionPortorCustomSealNo", LoadedContainerTableDataList[0].LoadContAdditionPortorCustomSealNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContModeofArrival", LoadedContainerTableDataList[0].LoadContModeofArrival.Trim());
            cmd.Parameters.AddWithValue("@LoadContTransportType", LoadedContainerTableDataList[0].LoadContTransportType.Trim());
            cmd.Parameters.AddWithValue("@LoadContEIRNo", LoadedContainerTableDataList[0].LoadContEIRNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContVehicleType", LoadedContainerTableDataList[0].LoadContVehicleType.Trim());

            cmd.Parameters.AddWithValue("@ContainerHold", LoadedContainerTableDataList[0].LoadContContainerHold.Trim());
            cmd.Parameters.AddWithValue("@ContainerHoldRemarks", LoadedContainerTableDataList[0].LoadContContainerHoldRemarks.Trim());
            cmd.Parameters.AddWithValue("@ContainerHoldAgency", LoadedContainerTableDataList[0].LoadContContainerHoldAgency.Trim());

            if (LoadedContainerTableDataList[0].LoadContContainerHoldDate == "" || LoadedContainerTableDataList[0].LoadContContainerHoldDate == null)
            {
                cmd.Parameters.AddWithValue("@ContainerHoldDate", DBNull.Value);
                LoadedContainerTableDataList[0].LoadContContainerHoldDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("ContainerHoldDate", Convert.ToDateTime(LoadedContainerTableDataList[0].LoadContContainerHoldDate.Trim()));
                LoadedContainerTableDataList[0].LoadContContainerHoldDate = Convert.ToDateTime(LoadedContainerTableDataList[0].LoadContContainerHoldDate).ToString("yyyy-MM-dd HH:mm:ss");
            }

            cmd.Parameters.AddWithValue("@LoadContTruckTag", LoadedContainerTableDataList[0].LoadContTruckTag.Trim());
            cmd.Parameters.AddWithValue("@LoadContContainerCondition", LoadedContainerTableDataList[0].LoadContContainerCondition.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.LoadContContainerNo != LoadedContainerTableDataList[0].LoadContContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerNo", DetailedListValue.LoadContContainerNo,
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContVehicleNo != LoadedContainerTableDataList[0].LoadContVehicleNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContVehicleNo", DetailedListValue.LoadContVehicleNo,
                    LoadedContainerTableDataList[0].LoadContVehicleNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContGateInPassNo != LoadedContainerTableDataList[0].LoadContGateInPassNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContGateInPassNo", DetailedListValue.LoadContGateInPassNo,
                    LoadedContainerTableDataList[0].LoadContGateInPassNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContGateInPassDate != LoadedContainerTableDataList[0].LoadContGateInPassDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContGateInPassDate", DetailedListValue.LoadContGateInPassDate,
                    LoadedContainerTableDataList[0].LoadContGateInPassDate.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContAgentSealNo != LoadedContainerTableDataList[0].LoadContAgentSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContAgentSealNo", DetailedListValue.LoadContAgentSealNo,
                    LoadedContainerTableDataList[0].LoadContAgentSealNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContScanStatus != LoadedContainerTableDataList[0].LoadContScanStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContScanStatus", DetailedListValue.LoadContScanStatus,
                    LoadedContainerTableDataList[0].LoadContScanStatus.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContScanType != LoadedContainerTableDataList[0].LoadContScanType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerScanType", DetailedListValue.LoadContScanType,
                    LoadedContainerTableDataList[0].LoadContScanType.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContScanLocation != LoadedContainerTableDataList[0].LoadContScanLocation.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerScanLocation", DetailedListValue.LoadContScanLocation,
                    LoadedContainerTableDataList[0].LoadContScanLocation.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContStatusType != LoadedContainerTableDataList[0].LoadContStatusType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContStatusType", DetailedListValue.LoadContStatusType,
                    LoadedContainerTableDataList[0].LoadContStatusType.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContPluginRequired != LoadedContainerTableDataList[0].LoadContPluginRequired.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContPluginRequired", DetailedListValue.LoadContPluginRequired,
                    LoadedContainerTableDataList[0].LoadContPluginRequired.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContAdditionAgentSealNo != LoadedContainerTableDataList[0].LoadContAdditionAgentSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContAdditionAgentSealNo", DetailedListValue.LoadContAdditionAgentSealNo,
                    LoadedContainerTableDataList[0].LoadContAdditionAgentSealNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContODC != LoadedContainerTableDataList[0].LoadContODC.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContODC", DetailedListValue.LoadContODC,
                    LoadedContainerTableDataList[0].LoadContODC.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContODCDimension != LoadedContainerTableDataList[0].LoadContODCDimension.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContODCDimension", DetailedListValue.LoadContODCDimension,
                    LoadedContainerTableDataList[0].LoadContODCDimension.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContContainerTag != LoadedContainerTableDataList[0].LoadContContainerTag.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContContainerTag", DetailedListValue.LoadContContainerTag,
                    LoadedContainerTableDataList[0].LoadContContainerTag.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContDamageDetails != LoadedContainerTableDataList[0].LoadContDamageDetails.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContDamageDetails", DetailedListValue.LoadContDamageDetails,
                    LoadedContainerTableDataList[0].LoadContDamageDetails.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContPortorCustomSealNo != LoadedContainerTableDataList[0].LoadContPortorCustomSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContPortorCustomSealNo", DetailedListValue.LoadContPortorCustomSealNo,
                    LoadedContainerTableDataList[0].LoadContPortorCustomSealNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContAdditionPortorCustomSealNo != LoadedContainerTableDataList[0].LoadContAdditionPortorCustomSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContAdditionPortorCustomSealNo", DetailedListValue.LoadContAdditionPortorCustomSealNo,
                    LoadedContainerTableDataList[0].LoadContAdditionPortorCustomSealNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContModeofArrival != LoadedContainerTableDataList[0].LoadContModeofArrival.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContModeofArrival", DetailedListValue.LoadContModeofArrival,
                    LoadedContainerTableDataList[0].LoadContModeofArrival.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContTransportType != LoadedContainerTableDataList[0].LoadContTransportType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContTransportType", DetailedListValue.LoadContTransportType,
                    LoadedContainerTableDataList[0].LoadContTransportType.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContEIRNo != LoadedContainerTableDataList[0].LoadContEIRNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContEIRNo", DetailedListValue.LoadContEIRNo,
                    LoadedContainerTableDataList[0].LoadContEIRNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContVehicleType != LoadedContainerTableDataList[0].LoadContVehicleType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContVehicleType", DetailedListValue.LoadContVehicleType,
                    LoadedContainerTableDataList[0].LoadContVehicleType.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }

                if (DetailedListValue.LoadContContainerHold != LoadedContainerTableDataList[0].LoadContContainerHold.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerHold", DetailedListValue.LoadContContainerHold,
                    LoadedContainerTableDataList[0].LoadContContainerHold.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContContainerHoldAgency != LoadedContainerTableDataList[0].LoadContContainerHoldAgency.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerHoldRemarks", DetailedListValue.LoadContContainerHoldAgency,
                    LoadedContainerTableDataList[0].LoadContContainerHoldAgency.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContContainerHoldDate != LoadedContainerTableDataList[0].LoadContContainerHoldDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerHoldAgency", DetailedListValue.LoadContContainerHoldDate,
                    LoadedContainerTableDataList[0].LoadContContainerHoldDate.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContContainerHoldRemarks != LoadedContainerTableDataList[0].LoadContContainerHoldRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "ContainerHoldDate", DetailedListValue.LoadContContainerHoldRemarks,
                    LoadedContainerTableDataList[0].LoadContContainerHoldRemarks.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }

                if (DetailedListValue.LoadContTruckTag != LoadedContainerTableDataList[0].LoadContTruckTag.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContTruckTag", DetailedListValue.LoadContTruckTag,
                    LoadedContainerTableDataList[0].LoadContTruckTag.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadContContainerCondition != LoadedContainerTableDataList[0].LoadContContainerCondition.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportContainer",
                    LoadedContainerTableDataList[0].LoadContContainerNo.Trim(), "LoadContContainerCondition", DetailedListValue.LoadContContainerCondition,
                    LoadedContainerTableDataList[0].LoadContContainerCondition.Trim(), "Modified", "MainJobNo = '" + LoadedContainerTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ContainerNo = '" + LoadedContainerTableDataList[0].LoadContContainerNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }

                //Create Work order for the Empty Truck Out
                List<WorkOrderTableDataList> workOrderDataList = new List<WorkOrderTableDataList>();
                foreach (var item in LoadedContainerTableDataList)
                {
                    SqlCommand cmmd = new SqlCommand("Select WorkOrderWorkOrderNo,WorkOrderWorkOrderDate From CFSImportWorkOrder Where WorkOrderType = 'Empty Truck Out' And MainJobNo = '" + item.MainJobNo.Trim() + "' And WorkOrderTruckNo = '" + item.LoadContVehicleNo.Trim() + "' And RecordStatus = 'Active'", cnn);
                    cnn.Open();
                    SqlDataReader sqlrd = cmmd.ExecuteReader();
                    string WorkOrderNo = "", WorkOrderDate = "";
                    while (sqlrd.Read())
                    {
                        WorkOrderNo = sqlrd["WorkOrderWorkOrderNo"].ToString();
                        WorkOrderDate = sqlrd["WorkOrderWorkOrderDate"].ToString();
                        break;
                    }
                    cnn.Close();

                    workOrderDataList.Add(new WorkOrderTableDataList
                    {
                        MainJobNo = item.MainJobNo.Trim(), //Mapping properties correctly
                        WorkOrderType = "Empty Truck Out",
                        WorkOrderWorkOrderNo = WorkOrderNo,
                        WorkOrderWorkOrderDate = WorkOrderDate,
                        WorkOrderContainerNo = "",
                        WorkOrderStuffingContainerNo = "",
                        WorkOrderTruckNo = item.LoadContVehicleNo.Trim(),
                        WorkOrderEquipmentType = "",
                        WorkOrderEquipment = "",
                        WorkOrderVendor = "",
                        ReturnedValue = ""
                    });
                }
                WorkOrderTableInsertData(workOrderDataList);

            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static LoadedContainerTableDataList[] LoadedContainerTableSearchData(List<LoadedContainerTableDataList> LoadedContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, ContColumn = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<LoadedContainerTableDataList> DetailedList = new List<LoadedContainerTableDataList>();
        LoadedContainerTableDataList DetailedListValue = new LoadedContainerTableDataList();
        try
        {
            if (LoadedContainerTableDataList[0].LoadContContainerNo != "" && LoadedContainerTableDataList[0].LoadContContainerNo != null) { ContColumn = "ContainerNo = @LoadContContainerNo and"; }
            cmd = new SqlCommand("select * from CFSImportContainer Where RecordStatus = 'Active' and ContainerItemNo = @LoadContItemNo and " + ContColumn + " MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContContainerNo", LoadedContainerTableDataList[0].LoadContContainerNo.Trim());
            cmd.Parameters.AddWithValue("@LoadContItemNo", LoadedContainerTableDataList[0].LoadContItemNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new LoadedContainerTableDataList();
                DetailedListValue.LoadContContainerNo = dtrow["ContainerNo"].ToString();
                if (dtrow["LoadContVehicleNo"].ToString() != "" && dtrow["LoadContVehicleNo"].ToString() != null)
                { DetailedListValue.LoadContVehicleNo = dtrow["LoadContVehicleNo"].ToString(); }
                else { DetailedListValue.LoadContVehicleNo = dtrow["TransportVehicleNo"].ToString(); }
                DetailedListValue.LoadContGateInPassNo = dtrow["LoadContGateInPassNo"].ToString();
                DetailedListValue.LoadContGateInPassDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["LoadContGateInPassDate"]);
                DetailedListValue.LoadContAgentSealNo = dtrow["ContainerSealNo"].ToString();
                DetailedListValue.LoadContScanStatus = dtrow["LoadContScanStatus"].ToString();
                DetailedListValue.LoadContScanType = dtrow["ContainerScanType"].ToString();
                DetailedListValue.LoadContScanLocation = dtrow["ContainerScanLocation"].ToString();
                DetailedListValue.LoadContStatusType = dtrow["LoadContStatusType"].ToString();
                DetailedListValue.LoadContPluginRequired = dtrow["LoadContPluginRequired"].ToString();
                DetailedListValue.LoadContAdditionAgentSealNo = dtrow["LoadContAdditionAgentSealNo"].ToString();
                DetailedListValue.LoadContODC = dtrow["LoadContODC"].ToString();
                DetailedListValue.LoadContODCDimension = dtrow["LoadContODCDimension"].ToString();
                DetailedListValue.LoadContContainerTag = dtrow["LoadContContainerTag"].ToString();
                DetailedListValue.LoadContDamageDetails = dtrow["LoadContDamageDetails"].ToString();
                DetailedListValue.LoadContPortorCustomSealNo = dtrow["LoadContPortorCustomSealNo"].ToString();
                DetailedListValue.LoadContAdditionPortorCustomSealNo = dtrow["LoadContAdditionPortorCustomSealNo"].ToString();
                DetailedListValue.LoadContModeofArrival = dtrow["LoadContModeofArrival"].ToString();
                DetailedListValue.LoadContTransportType = dtrow["LoadContTransportType"].ToString();
                DetailedListValue.LoadContEIRNo = dtrow["LoadContEIRNo"].ToString();
                DetailedListValue.LoadContVehicleType = dtrow["LoadContVehicleType"].ToString();
                DetailedListValue.LoadContContainerHold = dtrow["ContainerHold"].ToString();
                DetailedListValue.LoadContContainerHoldRemarks = dtrow["ContainerHoldRemarks"].ToString();
                DetailedListValue.LoadContContainerHoldAgency = dtrow["ContainerHoldAgency"].ToString();
                DetailedListValue.LoadContContainerHoldDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["ContainerHoldDate"]);
                DetailedListValue.LoadContTruckTag = dtrow["LoadContTruckTag"].ToString();
                DetailedListValue.LoadContContainerCondition = dtrow["LoadContContainerCondition"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class EmptyTruckorContainerTableDataList
    {
        public string Category { get; set; }
        public string EmptyTorCGateInPassNo { get; set; }
        public string EmptyTorCGateInPassDate { get; set; }
        public string EmptyTorCGateInMode { get; set; }
        public string EmptyTorCWorkOrderDate { get; set; }
        public string EmptyTorCWorkOrderNo { get; set; }
        public string EmptyTorCCHAName { get; set; }
        public string EmptyTorCTruckNo { get; set; }
        public string EmptyTorCContainerNo { get; set; }
        public string EmptyTorCDriverName { get; set; }
        public string EmptyTorCDriverLicenseNo { get; set; }
        public string EmptyTorCRemarks { get; set; }
        public string EmptyTruckorContainerId { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------EmptyTruckorContainerTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string EmptyTruckorContainerTableUpdateData(List<EmptyTruckorContainerTableDataList> EmptyTruckorContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, Prefix = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<EmptyTruckorContainerTableDataList> DetailedList = new List<EmptyTruckorContainerTableDataList>();
        EmptyTruckorContainerTableDataList DetailedListValue = new EmptyTruckorContainerTableDataList();
        try
        {
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Import Destuff Delivery") { Prefix = "IMD"; }
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Import Loaded Delivery") { Prefix = "IML"; }
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Empty Container Out") { Prefix = "ECO"; }
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Empty Container In") { Prefix = "ECI"; }
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Truck Halting") { Prefix = "TH"; }
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode == "Others") { Prefix = "OT"; }

            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo == "" && EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate == "")
            {
                cmd = new SqlCommand("Select Case When Count(*) = '0' then '1' Else Count(*) + 1 End As [Count] From CFSImportEmptyTruckorContainer", cnn);
                cnn.Open();
                SqlDataReader sqlrd = cmd.ExecuteReader();
                while (sqlrd.Read())
                {
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo = Prefix + "_GATE_IN_PASS_" + sqlrd["Count"].ToString();
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                cnn.Close();
            }

            cmd = new SqlCommand("SELECT * FROM CFSImportEmptyTruckorContainer WHERE EmptyTruckorContainerId = @EmptyTruckorContainerId and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@EmptyTruckorContainerId", EmptyTruckorContainerTableDataList[0].EmptyTruckorContainerId.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.EmptyTorCGateInPassNo = dtrow["EmptyTorCGateInPassNo"].ToString();
                DetailedListValue.EmptyTorCGateInPassDate = dtrow["EmptyTorCGateInPassDate"].ToString();
                if (dtrow["EmptyTorCGateInPassDate"].ToString() != "" || dtrow["EmptyTorCGateInPassDate"] != DBNull.Value) { DetailedListValue.EmptyTorCGateInPassDate = Convert.ToDateTime(dtrow["EmptyTorCGateInPassDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.EmptyTorCGateInPassDate = ""; }
                DetailedListValue.EmptyTorCGateInMode = dtrow["EmptyTorCGateInMode"].ToString();
                DetailedListValue.EmptyTorCCHAName = dtrow["EmptyTorCCHAName"].ToString();
                DetailedListValue.EmptyTorCTruckNo = dtrow["EmptyTorCTruckNo"].ToString();
                DetailedListValue.EmptyTorCContainerNo = dtrow["EmptyTorCContainerNo"].ToString();
                DetailedListValue.EmptyTorCDriverName = dtrow["EmptyTorCDriverName"].ToString();
                DetailedListValue.EmptyTorCDriverLicenseNo = dtrow["EmptyTorCDriverLicenseNo"].ToString();
                DetailedListValue.EmptyTorCRemarks = dtrow["EmptyTorCRemarks"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportEmptyTruckorContainer set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, EmptyTorCGateInPassNo=@EmptyTorCGateInPassNo,EmptyTorCGateInPassDate=@EmptyTorCGateInPassDate,EmptyTorCGateInMode=@EmptyTorCGateInMode,EmptyTorCCHAName=@EmptyTorCCHAName,EmptyTorCTruckNo=@EmptyTorCTruckNo,EmptyTorCContainerNo=@EmptyTorCContainerNo,EmptyTorCDriverName=@EmptyTorCDriverName,EmptyTorCDriverLicenseNo=@EmptyTorCDriverLicenseNo,EmptyTorCRemarks=@EmptyTorCRemarks,EmptyTruckorContainerStatus=@EmptyTruckorContainerStatus " +
                                 "where EmptyTruckorContainerId = @EmptyTruckorContainerId And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@EmptyTruckorContainerId", EmptyTruckorContainerTableDataList[0].EmptyTruckorContainerId.Trim());
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() != "" && EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() != null) { cmd.Parameters.AddWithValue("@EmptyTruckorContainerStatus", "Gated In"); }
            else { cmd.Parameters.AddWithValue("@EmptyTruckorContainerStatus", ""); }
            cmd.Parameters.AddWithValue("@EmptyTorCGateInPassNo", EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim());
            if (EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate == "" || EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate == null)
            {
                cmd.Parameters.AddWithValue("@EmptyTorCGateInPassDate", DBNull.Value);
                EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("EmptyTorCGateInPassDate", Convert.ToDateTime(EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate.Trim()));
                EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate = Convert.ToDateTime(EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@EmptyTorCGateInMode", EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCCHAName", EmptyTruckorContainerTableDataList[0].EmptyTorCCHAName.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCTruckNo", EmptyTruckorContainerTableDataList[0].EmptyTorCTruckNo.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCContainerNo", EmptyTruckorContainerTableDataList[0].EmptyTorCContainerNo.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCDriverName", EmptyTruckorContainerTableDataList[0].EmptyTorCDriverName.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCDriverLicenseNo", EmptyTruckorContainerTableDataList[0].EmptyTorCDriverLicenseNo.Trim());
            cmd.Parameters.AddWithValue("@EmptyTorCRemarks", EmptyTruckorContainerTableDataList[0].EmptyTorCRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.EmptyTorCGateInPassNo != EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCGateInPassNo", DetailedListValue.EmptyTorCGateInPassNo,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCGateInPassDate != EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCGateInPassDate", DetailedListValue.EmptyTorCGateInPassDate,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassDate.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCGateInMode != EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCGateInMode", DetailedListValue.EmptyTorCGateInMode,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInMode.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCCHAName != EmptyTruckorContainerTableDataList[0].EmptyTorCCHAName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCCHAName", DetailedListValue.EmptyTorCCHAName,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCCHAName.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCTruckNo != EmptyTruckorContainerTableDataList[0].EmptyTorCTruckNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCTruckNo", DetailedListValue.EmptyTorCTruckNo,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCTruckNo.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCContainerNo != EmptyTruckorContainerTableDataList[0].EmptyTorCContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCContainerNo", DetailedListValue.EmptyTorCContainerNo,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCContainerNo.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCDriverName != EmptyTruckorContainerTableDataList[0].EmptyTorCDriverName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCDriverName", DetailedListValue.EmptyTorCDriverName,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCDriverName.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCDriverLicenseNo != EmptyTruckorContainerTableDataList[0].EmptyTorCDriverLicenseNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCDriverLicenseNo", DetailedListValue.EmptyTorCDriverLicenseNo,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCDriverLicenseNo.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.EmptyTorCRemarks != EmptyTruckorContainerTableDataList[0].EmptyTorCRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyTruckorContainer",
                    EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim(), "EmptyTorCRemarks", DetailedListValue.EmptyTorCRemarks,
                    EmptyTruckorContainerTableDataList[0].EmptyTorCRemarks.Trim(), "Modified", "EmptyTorCGateInPassNo = '" + EmptyTruckorContainerTableDataList[0].EmptyTorCGateInPassNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static EmptyTruckorContainerTableDataList[] EmptyTruckorContainerTableSearchData(List<EmptyTruckorContainerTableDataList> EmptyTruckorContainerTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, AddCond = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<EmptyTruckorContainerTableDataList> DetailedList = new List<EmptyTruckorContainerTableDataList>();
        EmptyTruckorContainerTableDataList DetailedListValue = new EmptyTruckorContainerTableDataList();
        try
        {
            if (EmptyTruckorContainerTableDataList[0].Category != "") { AddCond = "And EmptyTruckorContainerId = '" + EmptyTruckorContainerTableDataList[0].Category + "'"; }
            else { AddCond = "And EmptyTruckorContainerStatus = 'New'"; }
            cmd = new SqlCommand("Select * From CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' " + AddCond + "", cnn);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new EmptyTruckorContainerTableDataList();
                DetailedListValue.EmptyTorCWorkOrderNo = dtrow["EmptyTorCWorkOrderNo"].ToString();
                DetailedListValue.EmptyTorCWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["EmptyTorCWorkOrderDate"]);
                DetailedListValue.EmptyTorCGateInPassNo = dtrow["EmptyTorCGateInPassNo"].ToString();
                DetailedListValue.EmptyTorCGateInPassDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["EmptyTorCGateInPassDate"]);
                DetailedListValue.EmptyTorCGateInMode = dtrow["EmptyTorCGateInMode"].ToString();
                DetailedListValue.EmptyTorCCHAName = dtrow["EmptyTorCCHAName"].ToString();
                DetailedListValue.EmptyTorCTruckNo = dtrow["EmptyTorCTruckNo"].ToString();
                DetailedListValue.EmptyTorCContainerNo = dtrow["EmptyTorCContainerNo"].ToString();
                DetailedListValue.EmptyTorCDriverName = dtrow["EmptyTorCDriverName"].ToString();
                DetailedListValue.EmptyTorCDriverLicenseNo = dtrow["EmptyTorCDriverLicenseNo"].ToString();
                DetailedListValue.EmptyTorCRemarks = dtrow["EmptyTorCRemarks"].ToString();
                DetailedListValue.EmptyTruckorContainerId = dtrow["EmptyTruckorContainerId"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------WorkOrderTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string WorkOrderTableInsertData(List<WorkOrderTableDataList> WorkOrderTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "", qry1 = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        int DupCount = 0; bool Exists = false;

        DataTable dt = new DataTable();
        List<WorkOrderTableDataList> DetailedList = new List<WorkOrderTableDataList>();
        WorkOrderTableDataList DetailedListValue = new WorkOrderTableDataList();
        try
        {
            foreach (var WorkOrderTableDatas in WorkOrderTableDataList)
            {
                string WorkOrderDate = WorkOrderTableDatas.WorkOrderWorkOrderDate.Replace("T", " "), WorkOrderNo = WorkOrderTableDatas.WorkOrderWorkOrderNo;

                if (WorkOrderTableDatas.WorkOrderType == "Loaded Container Out" || WorkOrderTableDatas.WorkOrderType == "Empty Truck Out")
                {
                    string Column = "EmptyTorCGateInMode = 'Import Loaded Delivery' And"; if (WorkOrderTableDatas.WorkOrderType == "Empty Truck Out") { Column = ""; }
                    cmd = new SqlCommand("Select COUNT(*) From CFSImportContainer Where RecordStatus = 'Active' And ContainerStatus = 'Gated In' And MainJobNo = @MainJobNo And LoadContVehicleNo = @EmptyTorCTruckNo And " +
                                         "LoadContGateInPassNo <> '' And LoadContGateInPassNo Is Not Null", cnn);
                    cmd.Parameters.AddWithValue("@EmptyTorCTruckNo", WorkOrderTableDatas.WorkOrderTruckNo.Trim());
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDatas.MainJobNo.Trim());
                    cnn.Open();
                    DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                    cnn.Close();

                    if (DupCount == 0)
                    {
                        qry = "Select Count(*) from CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' And EmptyTruckorContainerStatus = 'Gated In' And " +
                          "EmptyTorCGateInPassNo <> '' And EmptyTorCGateInPassNo Is Not Null And " + Column + " EmptyTorCTruckNo = @EmptyTorCTruckNo And MainJobNo = @MainJobNo;";
                        Exists = true;
                    }
                }

                if (WorkOrderTableDatas.WorkOrderType == "Empty Container Out")
                {
                    qry = "Select Count(*) from CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' And EmptyTruckorContainerStatus = 'Gated In' And " +
                      "EmptyTorCGateInPassNo <> '' And EmptyTorCGateInPassNo Is Not Null And EmptyTorCGateInMode = 'Empty Container Out' And EmptyTorCTruckNo = @EmptyTorCTruckNo And MainJobNo = @MainJobNo;";
                    Exists = true;
                }

                if (WorkOrderTableDatas.WorkOrderType == "FCL Cargo Out")
                {
                    qry = "Select Count(*) from CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' And EmptyTruckorContainerStatus = 'Gated In' And " +
                      "EmptyTorCGateInPassNo <> '' And EmptyTorCGateInPassNo Is Not Null And EmptyTorCGateInMode = 'Import Destuff Delivery' And EmptyTorCTruckNo = @EmptyTorCTruckNo And MainJobNo = @MainJobNo;";
                    Exists = true;
                }

                if (Exists)
                {
                    cmd = new SqlCommand(qry, cnn);
                    cmd.Parameters.AddWithValue("@EmptyTorCTruckNo", WorkOrderTableDatas.WorkOrderTruckNo.Trim());
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDatas.MainJobNo.Trim());
                    cnn.Open();
                    DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                    cnn.Close();

                    if (DupCount == 0) { return "The truck has not been gated in for this job."; }
                }

                if (WorkOrderNo == "" && WorkOrderDate == "")
                {
                    cmd = new SqlCommand("SELECT Case When Count(*) = '0' then '1' Else Count(*) + 1 End As [WorkOrderNo] FROM CFSImportWorkOrder", cnn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cnn;
                    cnn.Open();
                    SqlDataReader sqlrd = cmd.ExecuteReader();
                    sqlrd.Read();
                    WorkOrderNo = "WORK_ORDER_NO_" + sqlrd["WorkOrderNo"].ToString();
                    WorkOrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    cnn.Close();

                    qry = "Insert into CFSImportWorkOrder(ComputerName,IPAddress,Location,MainJobNo,WorkOrderType,WorkOrderWorkOrderNo,WorkOrderWorkOrderDate,WorkOrderContainerNo,WorkOrderTruckNo,WorkOrderEquipmentType,WorkOrderEquipment,WorkOrderVendor,UpdatedBy) " +
                          "Values(@ComputerName,@IPAddress,@Location,@MainJobNo,@WorkOrderType,@WorkOrderWorkOrderNo,@WorkOrderWorkOrderDate,@WorkOrderContainerNo,@WorkOrderTruckNo,@WorkOrderEquipmentType,@WorkOrderEquipment,@WorkOrderVendor,@UpdatedBy);";

                    cmd = new SqlCommand(qry, cnn);
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDatas.MainJobNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderType", WorkOrderTableDatas.WorkOrderType.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderWorkOrderNo", WorkOrderNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderWorkOrderDate", WorkOrderDate.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderContainerNo", WorkOrderTableDatas.WorkOrderContainerNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderTruckNo", WorkOrderTableDatas.WorkOrderTruckNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderEquipmentType", WorkOrderTableDatas.WorkOrderEquipmentType.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderEquipment", WorkOrderTableDatas.WorkOrderEquipment.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderVendor", WorkOrderTableDatas.WorkOrderVendor.Trim());
                    cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@ComputerName", "");
                    cmd.Parameters.AddWithValue("@IPAddress", "");
                    cmd.Parameters.AddWithValue("@Location", "");
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    InsertOrUpdateUmatchedValue("CFSImportWorkOrder", WorkOrderTableDatas.WorkOrderType.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());

                    if (WorkOrderTableDatas.WorkOrderType == "Empty Truck In" || WorkOrderTableDatas.WorkOrderType == "Empty Container In")
                    {
                        string col = "", value = "";
                        if (WorkOrderTableDatas.WorkOrderType == "Empty Container In") { col = "EmptyTorCGateInMode, "; value = "'Empty Container In', "; }

                        qry1 = "Insert Into CFSImportEmptyTruckorContainer (CompanyName, BranchName, MainJobNo, EmptyTorCWorkOrderNo, " + col + " EmptyTorCWorkOrderDate, " +
                        "EmptyTorCTruckNo, EmptyTorCContainerNo, UpdatedBy, ComputerName, IPAddress, Location) " +
                        "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, " + value + " @WorkOrderdate, @TruckNo, @ContainerNo, @UpdatedBy, @ComputerName, @IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Seal Cutting")
                    {
                        qry1 = "Insert Into CFSImportSealCutting (CompanyName, BranchName, MainJobNo, SealCutWorkOrderNo, SealCutWorkOrderDate, SealCutContainerNo, SealCutVendor, SealCutWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, @Vendor, 'Pending', @UpdatedBy, @ComputerName, @IPAddress, @Location);";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Examination")
                    {
                        qry1 = "Insert Into CFSImportExamination (CompanyName, BranchName, MainJobNo, ExamWorkOrderNo, ExamWorkOrderDate, ExamContainerNo, " +
                               "ExamEquipment, ExamVendor, ExamWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, @Equipment, @Vendor, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Section-49")
                    {
                        qry1 = "Insert Into CFSImportStuffing (StuffStuffingContainerNo, CompanyName, BranchName, MainJobNo, StuffWorkOrderNo, StuffWorkOrderDate, StuffDeStuffingContainerNo, " +
                               "StuffWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@StuffStuffingContainerNo, @CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "De-Stuffing")
                    {
                        qry1 = "Insert Into CFSImportDeStuffing (CompanyName, BranchName, MainJobNo, DeStuffWorkOrderNo, DeStuffWorkOrderDate, DeStuffContainerNo, " +
                               "DeStuffWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Loaded Container Out")
                    {
                        qry1 = "Insert Into CFSImportLoadedContainerOut (CompanyName, BranchName, MainJobNo, LoadOutWorkOrderNo, LoadOutWorkOrderDate, LoadOutContainerNo, " +
                               "LoadOutVehicleNo, LoadOutWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, @TruckNo, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Empty Container Out")
                    {
                        qry1 = "Insert Into CFSImportEmptyContainerOut (MtyOutModeofGateOut, CompanyName, BranchName, MainJobNo, MtyOutWorkOrderNo, MtyOutWorkOrderDate, MtyOutContainerNo, " +
                               "MtyOutVehicleNo, MtyOutWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values ('Empty Container Out', @CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, @TruckNo, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "FCL Cargo Out")
                    {
                        qry1 = "Insert Into CFSImportFCLCargoOut (CompanyName, BranchName, MainJobNo, FCLOutWorkOrderNo, FCLOutWorkOrderDate, FCLOutContainerNo, " +
                               "FCLOutVehicleNo, FCLOutEquipment, FCLOutVendor, FCLOutWorkOrderStatus, UpdatedBy, ComputerName, IPAddress, Location) " +
                               "Values (@CompanyName, @BranchName, @MainJobNo, @WorkOrderNo, @WorkOrderdate, @ContainerNo, @TruckNo, @Equipment, @Vendor, 'Pending', @UpdatedBy, @ComputerName, " +
                               "@IPAddress, @Location)";
                    }
                }
                else
                {
                    if (WorkOrderTableDatas.WorkOrderType == "Empty Truck In" || WorkOrderTableDatas.WorkOrderType == "Empty Container In" || WorkOrderTableDatas.WorkOrderType == "Empty Truck Out")
                    {
                        string colvalue = "";
                        if (WorkOrderTableDatas.WorkOrderType == "Empty Container In") { colvalue = "EmptyTorCGateInMode = 'Empty Container In', "; }

                        qry1 = "Update CFSImportEmptyTruckorContainer Set CompanyName = @CompanyName, " + colvalue + " BranchName = @BranchName, EmptyTorCWorkOrderDate = @WorkOrderdate, EmptyTorCTruckNo = @TruckNo, EmptyTorCContainerNo " +
                        "= @ContainerNo, UpdatedBy = @UpdatedBy, ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location Where MainJobNo = @MainJobNo And EmptyTorCWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Seal Cutting")
                    {
                        qry1 = "Update CFSImportSealCutting Set CompanyName = @CompanyName, BranchName = @BranchName, SealCutWorkOrderDate = @WorkOrderdate, SealCutContainerNo = @ContainerNo, " +
                               "SealCutVendor = @Vendor, SealCutWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And SealCutWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Examination")
                    {
                        qry1 = "Update CFSImportExamination Set CompanyName = @CompanyName, BranchName = @BranchName, ExamWorkOrderDate = @WorkOrderdate, " +
                               "ExamContainerNo = @ContainerNo, ExamEquipment = @Equipment, ExamVendor = @Vendor, ExamWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And ExamWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Section-49")
                    {
                        qry1 = "Update CFSImportStuffing Set StuffStuffingContainerNo = @StuffStuffingContainerNo, CompanyName = @CompanyName, BranchName = @BranchName, StuffWorkOrderDate = @WorkOrderdate, " +
                               "StuffDeStuffingContainerNo = @ContainerNo, StuffWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And StuffWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "De-Stuffing")
                    {
                        qry1 = "Update CFSImportDeStuffing Set CompanyName = @CompanyName, BranchName = @BranchName, DeStuffWorkOrderDate = @WorkOrderdate, " +
                               "DeStuffContainerNo = @ContainerNo, DeStuffWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And DeStuffWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Loaded Container Out")
                    {
                        qry1 = "Update CFSImportLoadedContainerOut Set CompanyName = @CompanyName, BranchName = @BranchName, LoadOutWorkOrderDate = @WorkOrderdate, " +
                               "LoadOutContainerNo = @ContainerNo, LoadOutVehicleNo = @TruckNo, LoadOutWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And LoadOutWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "Empty Container Out")
                    {
                        qry1 = "Update CFSImportEmptyContainerOut Set MtyOutModeofGateOut = 'Empty Container Out', CompanyName = @CompanyName, BranchName = @BranchName, MtyOutWorkOrderDate = @WorkOrderdate, " +
                               "MtyOutContainerNo = @ContainerNo, MtyOutVehicleNo = @TruckNo, MtyOutWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And MtyOutWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }
                    if (WorkOrderTableDatas.WorkOrderType == "FCL Cargo Out")
                    {
                        qry1 = "Update CFSImportFCLCargoOut Set CompanyName = @CompanyName, BranchName = @BranchName, FCLOutWorkOrderDate = @WorkOrderdate, " +
                               "FCLOutContainerNo = @ContainerNo, FCLOutVehicleNo = @TruckNo, FCLOutEquipment = @Equipment, FCLOutVendor = @Vendor, FCLOutWorkOrderStatus = 'Pending', UpdatedBy = @UpdatedBy, " +
                               "ComputerName = @ComputerName, IPAddress = @IPAddress, Location = @Location " +
                               "Where MainJobNo = @MainJobNo And FCLOutWorkOrderNo = @WorkOrderNo And RecordStatus = 'Active'";
                    }

                    cnn = new SqlConnection(constr);
                    cmd = new SqlCommand("SELECT * FROM CFSImportWorkOrder WHERE MainJobNo = @MainJobNo and WorkOrderWorkOrderNo = @WorkOrderWorkOrderNo and RecordStatus='Active'", cnn);
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDataList[0].MainJobNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderWorkOrderNo", WorkOrderTableDataList[0].WorkOrderWorkOrderNo.Trim());
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    foreach (DataRow dtrow in dt.Rows)
                    {
                        DetailedListValue.WorkOrderType = dtrow["WorkOrderType"].ToString();
                        DetailedListValue.WorkOrderWorkOrderNo = dtrow["WorkOrderWorkOrderNo"].ToString();
                        DetailedListValue.WorkOrderWorkOrderDate = dtrow["WorkOrderWorkOrderDate"].ToString();
                        if (dtrow["WorkOrderWorkOrderDate"].ToString() != "" || dtrow["WorkOrderWorkOrderDate"] != DBNull.Value) { DetailedListValue.WorkOrderWorkOrderDate = Convert.ToDateTime(dtrow["WorkOrderWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                        else { DetailedListValue.WorkOrderWorkOrderDate = ""; }
                        DetailedListValue.WorkOrderContainerNo = dtrow["WorkOrderContainerNo"].ToString();
                        DetailedListValue.WorkOrderTruckNo = dtrow["WorkOrderTruckNo"].ToString();
                        DetailedListValue.WorkOrderEquipmentType = dtrow["WorkOrderEquipmentType"].ToString();
                        DetailedListValue.WorkOrderEquipment = dtrow["WorkOrderEquipment"].ToString();
                        DetailedListValue.WorkOrderVendor = dtrow["WorkOrderVendor"].ToString();
                    }
                    cnn.Close();

                    qry = "Update CFSImportWorkOrder Set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, WorkOrderType=WorkOrderType,WorkOrderContainerNo=@WorkOrderContainerNo,WorkOrderTruckNo=@WorkOrderTruckNo," +
                    "WorkOrderEquipmentType=@WorkOrderEquipmentType,WorkOrderEquipment=@WorkOrderEquipment,WorkOrderVendor=@WorkOrderVendor " +
                    "Where MainJobNo=@MainJobNo And RecordStatus = 'Active' And WorkOrderWorkOrderNo=@WorkOrderWorkOrderNo;";

                    cmd = new SqlCommand(qry, cnn);
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDatas.MainJobNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderType", WorkOrderTableDatas.WorkOrderType.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderWorkOrderNo", WorkOrderNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderWorkOrderDate", WorkOrderDate.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderContainerNo", WorkOrderTableDatas.WorkOrderContainerNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderTruckNo", WorkOrderTableDatas.WorkOrderTruckNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderEquipmentType", WorkOrderTableDatas.WorkOrderEquipmentType.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderEquipment", WorkOrderTableDatas.WorkOrderEquipment.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderVendor", WorkOrderTableDatas.WorkOrderVendor.Trim());
                    cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@ComputerName", "");
                    cmd.Parameters.AddWithValue("@IPAddress", "");
                    cmd.Parameters.AddWithValue("@Location", "");
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();

                    foreach (DataRow dtrow in dt.Rows)
                    {
                        if (DetailedListValue.WorkOrderType != WorkOrderTableDataList[0].WorkOrderType.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderType", DetailedListValue.WorkOrderType,
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderWorkOrderNo != WorkOrderTableDataList[0].WorkOrderWorkOrderNo.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderWorkOrderNo", DetailedListValue.WorkOrderWorkOrderNo,
                            WorkOrderTableDataList[0].WorkOrderWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderWorkOrderDate != WorkOrderTableDataList[0].WorkOrderWorkOrderDate.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderWorkOrderDate", DetailedListValue.WorkOrderWorkOrderDate,
                            WorkOrderTableDataList[0].WorkOrderWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderContainerNo != WorkOrderTableDataList[0].WorkOrderContainerNo.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderContainerNo", DetailedListValue.WorkOrderContainerNo,
                            WorkOrderTableDataList[0].WorkOrderContainerNo.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderTruckNo != WorkOrderTableDataList[0].WorkOrderTruckNo.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderTruckNo", DetailedListValue.WorkOrderTruckNo,
                            WorkOrderTableDataList[0].WorkOrderTruckNo.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderEquipmentType != WorkOrderTableDataList[0].WorkOrderEquipmentType.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderEquipmentType", DetailedListValue.WorkOrderEquipmentType,
                            WorkOrderTableDataList[0].WorkOrderEquipmentType.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderEquipment != WorkOrderTableDataList[0].WorkOrderEquipment.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderEquipment", DetailedListValue.WorkOrderEquipment,
                            WorkOrderTableDataList[0].WorkOrderEquipment.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                        if (DetailedListValue.WorkOrderVendor != WorkOrderTableDataList[0].WorkOrderVendor.Trim())
                        {
                            InsertOrUpdateUmatchedValue("CFSImportWorkOrder",
                            WorkOrderTableDataList[0].WorkOrderType.Trim(), "WorkOrderVendor", DetailedListValue.WorkOrderVendor,
                            WorkOrderTableDataList[0].WorkOrderVendor.Trim(), "Modified", "MainJobNo = '" + WorkOrderTableDataList[0].MainJobNo.Trim() + "' And " +
                            "WorkOrderType = '" + WorkOrderTableDataList[0].WorkOrderType.Trim() + "' And RecordStatus = 'Active'",
                            HttpContext.Current.Session["UserName"].ToString());
                        }
                    }
                }

                if (qry1 != "")
                {
                    cmd = new SqlCommand(qry1, cnn);
                    cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                    cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                    cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDatas.MainJobNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo.Trim());
                    cmd.Parameters.AddWithValue("@WorkOrderdate", WorkOrderDate.Trim());
                    cmd.Parameters.AddWithValue("@TruckNo", WorkOrderTableDatas.WorkOrderTruckNo.Trim());
                    cmd.Parameters.AddWithValue("@ContainerNo", WorkOrderTableDatas.WorkOrderContainerNo.Trim());
                    cmd.Parameters.AddWithValue("@StuffStuffingContainerNo", WorkOrderTableDatas.WorkOrderStuffingContainerNo.Trim());
                    cmd.Parameters.AddWithValue("@EquipmentType", WorkOrderTableDatas.WorkOrderEquipmentType.Trim());
                    cmd.Parameters.AddWithValue("@Equipment", WorkOrderTableDatas.WorkOrderEquipment.Trim());
                    cmd.Parameters.AddWithValue("@Vendor", WorkOrderTableDatas.WorkOrderVendor.Trim());
                    cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@ComputerName", "");
                    cmd.Parameters.AddWithValue("@IPAddress", "");
                    cmd.Parameters.AddWithValue("@Location", "");
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }

            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class WorkOrderTableDataList
    {
        public string MainJobNo { get; set; }
        public string WorkOrderType { get; set; }
        public string WorkOrderWorkOrderNo { get; set; }
        public string WorkOrderWorkOrderDate { get; set; }
        public string WorkOrderContainerNo { get; set; }
        public string WorkOrderStuffingContainerNo { get; set; }
        public string WorkOrderTruckNo { get; set; }
        public string WorkOrderEquipmentType { get; set; }
        public string WorkOrderEquipment { get; set; }
        public string WorkOrderVendor { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------WorkOrderTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string WorkOrderTableCancelData(List<WorkOrderTableDataList> WorkOrderTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportWorkOrder set RecordStatus = 'Cancelled' where WorkOrderWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportSealCutting set RecordStatus = 'Cancelled' where SealCutWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportExamination set RecordStatus = 'Cancelled' where ExamWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportStuffing set RecordStatus = 'Cancelled' where StuffWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportDeStuffing set RecordStatus = 'Cancelled' where DeStuffWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportLoadedContainerOut set RecordStatus = 'Cancelled' where LoadOutWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportEmptyContainerOut set RecordStatus = 'Cancelled' where MtyOutWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;" +
                                 "Update CFSImportFCLCargoOut set RecordStatus = 'Cancelled' where FCLOutWorkOrderNo = @WorkOrderWorkOrderNo And MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@WorkOrderWorkOrderNo", WorkOrderTableDataList[0].WorkOrderWorkOrderNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportWorkOrder", WorkOrderTableDataList[0].WorkOrderWorkOrderNo.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static WorkOrderTableDataList[] WorkOrderTableSearchData(List<WorkOrderTableDataList> WorkOrderTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<WorkOrderTableDataList> DetailedList = new List<WorkOrderTableDataList>();
        WorkOrderTableDataList DetailedListValue = new WorkOrderTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportWorkOrder Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", WorkOrderTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new WorkOrderTableDataList();
                DetailedListValue.WorkOrderType = dtrow["WorkOrderType"].ToString();
                DetailedListValue.WorkOrderWorkOrderNo = dtrow["WorkOrderWorkOrderNo"].ToString();
                DetailedListValue.WorkOrderWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["WorkOrderWorkOrderDate"]);
                DetailedListValue.WorkOrderContainerNo = dtrow["WorkOrderContainerNo"].ToString();
                DetailedListValue.WorkOrderTruckNo = dtrow["WorkOrderTruckNo"].ToString();
                DetailedListValue.WorkOrderEquipmentType = dtrow["WorkOrderEquipmentType"].ToString();
                DetailedListValue.WorkOrderEquipment = dtrow["WorkOrderEquipment"].ToString();
                DetailedListValue.WorkOrderVendor = dtrow["WorkOrderVendor"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class SealCuttingTableDataList
    {
        public string MainJobNo { get; set; }
        public string SealCutWorkOrderNo { get; set; }
        public string SealCutWorkOrderDate { get; set; }
        public string SealCutContainerNo { get; set; }
        public string SealCutCFSSealNo { get; set; }
        public string SealCutVendor { get; set; }
        public string SealCutWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------SealCuttingTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string SealCuttingTableUpdateData(List<SealCuttingTableDataList> SealCuttingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<SealCuttingTableDataList> DetailedList = new List<SealCuttingTableDataList>();
        SealCuttingTableDataList DetailedListValue = new SealCuttingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportSealCutting WHERE MainJobNo = @MainJobNo and SealCutWorkOrderNo = @SealCutWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", SealCuttingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@SealCutWorkOrderNo", SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.SealCutWorkOrderNo = dtrow["SealCutWorkOrderNo"].ToString();
                DetailedListValue.SealCutWorkOrderDate = dtrow["SealCutWorkOrderDate"].ToString();
                if (dtrow["SealCutWorkOrderDate"].ToString() != "" || dtrow["SealCutWorkOrderDate"] != DBNull.Value) { DetailedListValue.SealCutWorkOrderDate = Convert.ToDateTime(dtrow["SealCutWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.SealCutWorkOrderDate = ""; }
                DetailedListValue.SealCutContainerNo = dtrow["SealCutContainerNo"].ToString();
                DetailedListValue.SealCutCFSSealNo = dtrow["SealCutCFSSealNo"].ToString();
                DetailedListValue.SealCutVendor = dtrow["SealCutVendor"].ToString();
                DetailedListValue.SealCutWorkOrderStatus = dtrow["SealCutWorkOrderStatus"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportSealCutting set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, SealCutWorkOrderNo=@SealCutWorkOrderNo,SealCutWorkOrderDate=@SealCutWorkOrderDate,SealCutContainerNo=@SealCutContainerNo,SealCutCFSSealNo=@SealCutCFSSealNo,SealCutVendor=@SealCutVendor,SealCutWorkOrderStatus=@SealCutWorkOrderStatus where SealCutWorkOrderNo = @SealCutWorkOrderNo And MainJobNo = @MainJobNo And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", SealCuttingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@SealCutWorkOrderNo", SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim());
            if (SealCuttingTableDataList[0].SealCutWorkOrderDate == "" || SealCuttingTableDataList[0].SealCutWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@SealCutWorkOrderDate", DBNull.Value);
                SealCuttingTableDataList[0].SealCutWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("SealCutWorkOrderDate", Convert.ToDateTime(SealCuttingTableDataList[0].SealCutWorkOrderDate.Trim()));
                SealCuttingTableDataList[0].SealCutWorkOrderDate = Convert.ToDateTime(SealCuttingTableDataList[0].SealCutWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@SealCutContainerNo", SealCuttingTableDataList[0].SealCutContainerNo.Trim());
            cmd.Parameters.AddWithValue("@SealCutCFSSealNo", SealCuttingTableDataList[0].SealCutCFSSealNo.Trim());
            cmd.Parameters.AddWithValue("@SealCutVendor", SealCuttingTableDataList[0].SealCutVendor.Trim());
            cmd.Parameters.AddWithValue("@SealCutWorkOrderStatus", SealCuttingTableDataList[0].SealCutWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.SealCutWorkOrderNo != SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutWorkOrderNo", DetailedListValue.SealCutWorkOrderNo,
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.SealCutWorkOrderDate != SealCuttingTableDataList[0].SealCutWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutWorkOrderDate", DetailedListValue.SealCutWorkOrderDate,
                    SealCuttingTableDataList[0].SealCutWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.SealCutContainerNo != SealCuttingTableDataList[0].SealCutContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutContainerNo", DetailedListValue.SealCutContainerNo,
                    SealCuttingTableDataList[0].SealCutContainerNo.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.SealCutCFSSealNo != SealCuttingTableDataList[0].SealCutCFSSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutCFSSealNo", DetailedListValue.SealCutCFSSealNo,
                    SealCuttingTableDataList[0].SealCutCFSSealNo.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.SealCutVendor != SealCuttingTableDataList[0].SealCutVendor.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutVendor", DetailedListValue.SealCutVendor,
                    SealCuttingTableDataList[0].SealCutVendor.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.SealCutWorkOrderStatus != SealCuttingTableDataList[0].SealCutWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportSealCutting",
                    SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim(), "SealCutWorkOrderStatus", DetailedListValue.SealCutWorkOrderStatus,
                    SealCuttingTableDataList[0].SealCutWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + SealCuttingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "SealCutWorkOrderNo = '" + SealCuttingTableDataList[0].SealCutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static SealCuttingTableDataList[] SealCuttingTableSearchData(List<SealCuttingTableDataList> SealCuttingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<SealCuttingTableDataList> DetailedList = new List<SealCuttingTableDataList>();
        SealCuttingTableDataList DetailedListValue = new SealCuttingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportSealCutting Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", SealCuttingTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new SealCuttingTableDataList();
                DetailedListValue.SealCutWorkOrderNo = dtrow["SealCutWorkOrderNo"].ToString();
                DetailedListValue.SealCutWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["SealCutWorkOrderDate"]);
                DetailedListValue.SealCutContainerNo = dtrow["SealCutContainerNo"].ToString();
                DetailedListValue.SealCutCFSSealNo = dtrow["SealCutCFSSealNo"].ToString();
                DetailedListValue.SealCutVendor = dtrow["SealCutVendor"].ToString();
                DetailedListValue.SealCutWorkOrderStatus = dtrow["SealCutWorkOrderStatus"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class ExaminationTableDataList
    {
        public string MainJobNo { get; set; }
        public string ExamWorkOrderNo { get; set; }
        public string ExamWorkOrderDate { get; set; }
        public string ExamContainerNo { get; set; }
        public string ExamCFSSealNo { get; set; }
        public string ExamScrapLabour { get; set; }
        public string ExamExamedPkgs { get; set; }
        public string ExamExamedPerc { get; set; }
        public string ExamStartDateTime { get; set; }
        public string ExamEndDateTime { get; set; }
        public string ExamEquipment { get; set; }
        public string ExamVendor { get; set; }
        public string ExamWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------ExaminationTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ExaminationTableUpdateData(List<ExaminationTableDataList> ExaminationTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<ExaminationTableDataList> DetailedList = new List<ExaminationTableDataList>();
        ExaminationTableDataList DetailedListValue = new ExaminationTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportExamination WHERE MainJobNo = @MainJobNo and ExamWorkOrderNo = @ExamWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ExaminationTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ExamWorkOrderNo", ExaminationTableDataList[0].ExamWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.ExamWorkOrderNo = dtrow["ExamWorkOrderNo"].ToString();
                DetailedListValue.ExamWorkOrderDate = dtrow["ExamWorkOrderDate"].ToString();
                if (dtrow["ExamWorkOrderDate"].ToString() != "" || dtrow["ExamWorkOrderDate"] != DBNull.Value) { DetailedListValue.ExamWorkOrderDate = Convert.ToDateTime(dtrow["ExamWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.ExamWorkOrderDate = ""; }
                DetailedListValue.ExamContainerNo = dtrow["ExamContainerNo"].ToString();
                DetailedListValue.ExamCFSSealNo = dtrow["ExamCFSSealNo"].ToString();
                DetailedListValue.ExamScrapLabour = dtrow["ExamScrapLabour"].ToString();
                DetailedListValue.ExamExamedPkgs = dtrow["ExamExamedPkgs"].ToString();
                DetailedListValue.ExamExamedPerc = dtrow["ExamExamedPerc"].ToString();
                DetailedListValue.ExamStartDateTime = dtrow["ExamStartDateTime"].ToString();
                if (dtrow["ExamStartDateTime"].ToString() != "" || dtrow["ExamStartDateTime"] != DBNull.Value) { DetailedListValue.ExamStartDateTime = Convert.ToDateTime(dtrow["ExamStartDateTime"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.ExamStartDateTime = ""; }
                DetailedListValue.ExamEndDateTime = dtrow["ExamEndDateTime"].ToString();
                if (dtrow["ExamEndDateTime"].ToString() != "" || dtrow["ExamEndDateTime"] != DBNull.Value) { DetailedListValue.ExamEndDateTime = Convert.ToDateTime(dtrow["ExamEndDateTime"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.ExamEndDateTime = ""; }
                DetailedListValue.ExamEquipment = dtrow["ExamEquipment"].ToString();
                DetailedListValue.ExamVendor = dtrow["ExamVendor"].ToString();
                DetailedListValue.ExamWorkOrderStatus = dtrow["ExamWorkOrderStatus"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportExamination set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, ExamWorkOrderNo=@ExamWorkOrderNo,ExamWorkOrderDate=@ExamWorkOrderDate,ExamContainerNo=@ExamContainerNo,ExamCFSSealNo=@ExamCFSSealNo,ExamScrapLabour=@ExamScrapLabour,ExamExamedPkgs=@ExamExamedPkgs,ExamExamedPerc=@ExamExamedPerc,ExamStartDateTime=@ExamStartDateTime,ExamEndDateTime=@ExamEndDateTime,ExamEquipment=@ExamEquipment,ExamVendor=@ExamVendor,ExamWorkOrderStatus=@ExamWorkOrderStatus where ExamWorkOrderNo = @ExamWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ExaminationTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ExamWorkOrderNo", ExaminationTableDataList[0].ExamWorkOrderNo.Trim());
            if (ExaminationTableDataList[0].ExamWorkOrderDate == "" || ExaminationTableDataList[0].ExamWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@ExamWorkOrderDate", DBNull.Value);
                ExaminationTableDataList[0].ExamWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("ExamWorkOrderDate", Convert.ToDateTime(ExaminationTableDataList[0].ExamWorkOrderDate.Trim()));
                ExaminationTableDataList[0].ExamWorkOrderDate = Convert.ToDateTime(ExaminationTableDataList[0].ExamWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@ExamContainerNo", ExaminationTableDataList[0].ExamContainerNo.Trim());
            cmd.Parameters.AddWithValue("@ExamCFSSealNo", ExaminationTableDataList[0].ExamCFSSealNo.Trim());
            cmd.Parameters.AddWithValue("@ExamScrapLabour", ExaminationTableDataList[0].ExamScrapLabour.Trim());
            cmd.Parameters.AddWithValue("@ExamExamedPkgs", ExaminationTableDataList[0].ExamExamedPkgs.Trim());
            cmd.Parameters.AddWithValue("@ExamExamedPerc", ExaminationTableDataList[0].ExamExamedPerc.Trim());
            if (ExaminationTableDataList[0].ExamStartDateTime == "" || ExaminationTableDataList[0].ExamStartDateTime == null)
            {
                cmd.Parameters.AddWithValue("@ExamStartDateTime", DBNull.Value);
                ExaminationTableDataList[0].ExamStartDateTime = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("ExamStartDateTime", Convert.ToDateTime(ExaminationTableDataList[0].ExamStartDateTime.Trim()));
                ExaminationTableDataList[0].ExamStartDateTime = Convert.ToDateTime(ExaminationTableDataList[0].ExamStartDateTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (ExaminationTableDataList[0].ExamEndDateTime == "" || ExaminationTableDataList[0].ExamEndDateTime == null)
            {
                cmd.Parameters.AddWithValue("@ExamEndDateTime", DBNull.Value);
                ExaminationTableDataList[0].ExamEndDateTime = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("ExamEndDateTime", Convert.ToDateTime(ExaminationTableDataList[0].ExamEndDateTime.Trim()));
                ExaminationTableDataList[0].ExamEndDateTime = Convert.ToDateTime(ExaminationTableDataList[0].ExamEndDateTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@ExamEquipment", ExaminationTableDataList[0].ExamEquipment.Trim());
            cmd.Parameters.AddWithValue("@ExamVendor", ExaminationTableDataList[0].ExamVendor.Trim());
            cmd.Parameters.AddWithValue("@ExamWorkOrderStatus", ExaminationTableDataList[0].ExamWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.ExamWorkOrderNo != ExaminationTableDataList[0].ExamWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamWorkOrderNo", DetailedListValue.ExamWorkOrderNo,
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamWorkOrderDate != ExaminationTableDataList[0].ExamWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamWorkOrderDate", DetailedListValue.ExamWorkOrderDate,
                    ExaminationTableDataList[0].ExamWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamContainerNo != ExaminationTableDataList[0].ExamContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamContainerNo", DetailedListValue.ExamContainerNo,
                    ExaminationTableDataList[0].ExamContainerNo.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamCFSSealNo != ExaminationTableDataList[0].ExamCFSSealNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamCFSSealNo", DetailedListValue.ExamCFSSealNo,
                    ExaminationTableDataList[0].ExamCFSSealNo.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamScrapLabour != ExaminationTableDataList[0].ExamScrapLabour.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamScrapLabour", DetailedListValue.ExamScrapLabour,
                    ExaminationTableDataList[0].ExamScrapLabour.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamExamedPkgs != ExaminationTableDataList[0].ExamExamedPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamExamedPkgs", DetailedListValue.ExamExamedPkgs,
                    ExaminationTableDataList[0].ExamExamedPkgs.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamExamedPerc != ExaminationTableDataList[0].ExamExamedPerc.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamExamedPerc", DetailedListValue.ExamExamedPerc,
                    ExaminationTableDataList[0].ExamExamedPerc.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamStartDateTime != ExaminationTableDataList[0].ExamStartDateTime.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamStartDateTime", DetailedListValue.ExamStartDateTime,
                    ExaminationTableDataList[0].ExamStartDateTime.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamEndDateTime != ExaminationTableDataList[0].ExamEndDateTime.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamEndDateTime", DetailedListValue.ExamEndDateTime,
                    ExaminationTableDataList[0].ExamEndDateTime.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamEquipment != ExaminationTableDataList[0].ExamEquipment.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamEquipment", DetailedListValue.ExamEquipment,
                    ExaminationTableDataList[0].ExamEquipment.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamVendor != ExaminationTableDataList[0].ExamVendor.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamVendor", DetailedListValue.ExamVendor,
                    ExaminationTableDataList[0].ExamVendor.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ExamWorkOrderStatus != ExaminationTableDataList[0].ExamWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportExamination",
                    ExaminationTableDataList[0].ExamWorkOrderNo.Trim(), "ExamWorkOrderStatus", DetailedListValue.ExamWorkOrderStatus,
                    ExaminationTableDataList[0].ExamWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + ExaminationTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ExamWorkOrderNo = '" + ExaminationTableDataList[0].ExamWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static ExaminationTableDataList[] ExaminationTableSearchData(List<ExaminationTableDataList> ExaminationTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<ExaminationTableDataList> DetailedList = new List<ExaminationTableDataList>();
        ExaminationTableDataList DetailedListValue = new ExaminationTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportExamination Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ExaminationTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new ExaminationTableDataList();
                DetailedListValue.ExamWorkOrderNo = dtrow["ExamWorkOrderNo"].ToString();
                DetailedListValue.ExamWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["ExamWorkOrderDate"]);
                DetailedListValue.ExamContainerNo = dtrow["ExamContainerNo"].ToString();
                DetailedListValue.ExamCFSSealNo = dtrow["ExamCFSSealNo"].ToString();
                DetailedListValue.ExamScrapLabour = dtrow["ExamScrapLabour"].ToString();
                DetailedListValue.ExamExamedPkgs = dtrow["ExamExamedPkgs"].ToString();
                DetailedListValue.ExamExamedPerc = dtrow["ExamExamedPerc"].ToString();
                DetailedListValue.ExamStartDateTime = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["ExamStartDateTime"]);
                DetailedListValue.ExamEndDateTime = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["ExamEndDateTime"]);
                DetailedListValue.ExamEquipment = dtrow["ExamEquipment"].ToString();
                DetailedListValue.ExamVendor = dtrow["ExamVendor"].ToString();
                DetailedListValue.ExamWorkOrderStatus = dtrow["ExamWorkOrderStatus"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class StuffingTableDataList
    {
        public string MainJobNo { get; set; }
        public string StuffWorkOrderNo { get; set; }
        public string StuffWorkOrderDate { get; set; }
        public string StuffDeStuffingContainerNo { get; set; }
        public string StuffStuffingContainerNo { get; set; }
        public string StuffDeclaredPkgs { get; set; }
        public string StuffDeclaredWeight { get; set; }
        public string StuffStuffedFrom { get; set; }
        public string StuffStuffedTo { get; set; }
        public string StuffStuffedWeight { get; set; }
        public string StuffStuffedPkgs { get; set; }
        public string StuffRemarks { get; set; }
        public string StuffBalancePkgs { get; set; }
        public string StuffBalanceWeight { get; set; }
        public string StuffWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
        public string ContainerNoofPackage { get; set; }
        public string ContainerCargoWeightKg { get; set; }
    }
    //-----------------------------------------------------------StuffingTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string StuffingTableUpdateData(List<StuffingTableDataList> StuffingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<StuffingTableDataList> DetailedList = new List<StuffingTableDataList>();
        StuffingTableDataList DetailedListValue = new StuffingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportStuffing WHERE MainJobNo = @MainJobNo and StuffWorkOrderNo = @StuffWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", StuffingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@StuffWorkOrderNo", StuffingTableDataList[0].StuffWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.StuffWorkOrderNo = dtrow["StuffWorkOrderNo"].ToString();
                DetailedListValue.StuffWorkOrderDate = dtrow["StuffWorkOrderDate"].ToString();
                if (dtrow["StuffWorkOrderDate"].ToString() != "" || dtrow["StuffWorkOrderDate"] != DBNull.Value) { DetailedListValue.StuffWorkOrderDate = Convert.ToDateTime(dtrow["StuffWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.StuffWorkOrderDate = ""; }
                DetailedListValue.StuffDeStuffingContainerNo = dtrow["StuffDeStuffingContainerNo"].ToString();
                DetailedListValue.StuffStuffingContainerNo = dtrow["StuffStuffingContainerNo"].ToString();
                DetailedListValue.StuffDeclaredPkgs = dtrow["StuffDeclaredPkgs"].ToString();
                DetailedListValue.StuffDeclaredWeight = dtrow["StuffDeclaredWeight"].ToString();
                DetailedListValue.StuffStuffedFrom = dtrow["StuffStuffedFrom"].ToString();
                if (dtrow["StuffStuffedFrom"].ToString() != "" || dtrow["StuffStuffedFrom"] != DBNull.Value) { DetailedListValue.StuffStuffedFrom = Convert.ToDateTime(dtrow["StuffStuffedFrom"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.StuffStuffedFrom = ""; }
                DetailedListValue.StuffStuffedTo = dtrow["StuffStuffedTo"].ToString();
                if (dtrow["StuffStuffedTo"].ToString() != "" || dtrow["StuffStuffedTo"] != DBNull.Value) { DetailedListValue.StuffStuffedTo = Convert.ToDateTime(dtrow["StuffStuffedTo"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.StuffStuffedTo = ""; }
                DetailedListValue.StuffStuffedWeight = dtrow["StuffStuffedWeight"].ToString();
                DetailedListValue.StuffStuffedPkgs = dtrow["StuffStuffedPkgs"].ToString();
                DetailedListValue.StuffRemarks = dtrow["StuffRemarks"].ToString();
                DetailedListValue.StuffWorkOrderStatus = dtrow["StuffWorkOrderStatus"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportStuffing set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, StuffingStatus=@StuffingStatus,StuffWorkOrderNo=@StuffWorkOrderNo,StuffWorkOrderDate=@StuffWorkOrderDate,StuffDeStuffingContainerNo=@StuffDeStuffingContainerNo,StuffStuffingContainerNo=@StuffStuffingContainerNo,StuffDeclaredPkgs=@StuffDeclaredPkgs,StuffDeclaredWeight=@StuffDeclaredWeight,StuffStuffedFrom=@StuffStuffedFrom,StuffStuffedTo=@StuffStuffedTo,StuffStuffedWeight=@StuffStuffedWeight,StuffStuffedPkgs=@StuffStuffedPkgs,StuffRemarks=@StuffRemarks,StuffWorkOrderStatus=@StuffWorkOrderStatus " +
                                 "where StuffWorkOrderNo = @StuffWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", StuffingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@StuffWorkOrderNo", StuffingTableDataList[0].StuffWorkOrderNo.Trim());
            if (StuffingTableDataList[0].StuffWorkOrderDate == "" || StuffingTableDataList[0].StuffWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@StuffWorkOrderDate", DBNull.Value);
                StuffingTableDataList[0].StuffWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("StuffWorkOrderDate", Convert.ToDateTime(StuffingTableDataList[0].StuffWorkOrderDate.Trim()));
                StuffingTableDataList[0].StuffWorkOrderDate = Convert.ToDateTime(StuffingTableDataList[0].StuffWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@StuffDeStuffingContainerNo", StuffingTableDataList[0].StuffDeStuffingContainerNo.Trim());
            cmd.Parameters.AddWithValue("@StuffStuffingContainerNo", StuffingTableDataList[0].StuffStuffingContainerNo.Trim());
            cmd.Parameters.AddWithValue("@StuffDeclaredPkgs", StuffingTableDataList[0].StuffDeclaredPkgs.Trim());
            cmd.Parameters.AddWithValue("@StuffDeclaredWeight", StuffingTableDataList[0].StuffDeclaredWeight.Trim());
            if (StuffingTableDataList[0].StuffStuffedFrom == "" || StuffingTableDataList[0].StuffStuffedFrom == null)
            {
                cmd.Parameters.AddWithValue("@StuffStuffedFrom", DBNull.Value);
                StuffingTableDataList[0].StuffStuffedFrom = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("StuffStuffedFrom", Convert.ToDateTime(StuffingTableDataList[0].StuffStuffedFrom.Trim()));
                StuffingTableDataList[0].StuffStuffedFrom = Convert.ToDateTime(StuffingTableDataList[0].StuffStuffedFrom).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (StuffingTableDataList[0].StuffStuffedTo == "" || StuffingTableDataList[0].StuffStuffedTo == null)
            {
                cmd.Parameters.AddWithValue("@StuffStuffedTo", DBNull.Value);
                StuffingTableDataList[0].StuffStuffedTo = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("StuffStuffedTo", Convert.ToDateTime(StuffingTableDataList[0].StuffStuffedTo.Trim()));
                StuffingTableDataList[0].StuffStuffedTo = Convert.ToDateTime(StuffingTableDataList[0].StuffStuffedTo).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@StuffStuffedWeight", StuffingTableDataList[0].StuffStuffedWeight.Trim());
            cmd.Parameters.AddWithValue("@StuffStuffedPkgs", StuffingTableDataList[0].StuffStuffedPkgs.Trim());
            if (Convert.ToInt32(StuffingTableDataList[0].StuffBalancePkgs.Trim()) == 0 && Convert.ToInt32(StuffingTableDataList[0].StuffBalanceWeight.Trim()) == 0)
            {
                cmd.Parameters.AddWithValue("@StuffingStatus", "Fully Stuffed");

                SqlCommand cmmd = new SqlCommand("Update CFSImportContainer Set IsContainerEmpty = @IsContainerEmpty Where MainJobno = @MainJobno And ContainerNo = @ContainerNo And RecordStatus = 'Active'", cnn);
                cmmd.Parameters.AddWithValue("@IsContainerEmpty", "Yes");
                cmmd.Parameters.AddWithValue("@MainJobno", StuffingTableDataList[0].MainJobNo.Trim());
                cmmd.Parameters.AddWithValue("@ContainerNo", StuffingTableDataList[0].StuffDeStuffingContainerNo.Trim());
                cnn.Open();
                cmmd.ExecuteNonQuery();
                cnn.Close();
            }
            else
            {
                cmd.Parameters.AddWithValue("@StuffingStatus", "Partially Stuffed");
            }
            cmd.Parameters.AddWithValue("@StuffRemarks", StuffingTableDataList[0].StuffRemarks.Trim());
            cmd.Parameters.AddWithValue("@StuffWorkOrderStatus", StuffingTableDataList[0].StuffWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.StuffWorkOrderNo != StuffingTableDataList[0].StuffWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffWorkOrderNo", DetailedListValue.StuffWorkOrderNo,
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffWorkOrderDate != StuffingTableDataList[0].StuffWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffWorkOrderDate", DetailedListValue.StuffWorkOrderDate,
                    StuffingTableDataList[0].StuffWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffDeStuffingContainerNo != StuffingTableDataList[0].StuffDeStuffingContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffDeStuffingContainerNo", DetailedListValue.StuffDeStuffingContainerNo,
                    StuffingTableDataList[0].StuffDeStuffingContainerNo.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffStuffingContainerNo != StuffingTableDataList[0].StuffStuffingContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffStuffingContainerNo", DetailedListValue.StuffStuffingContainerNo,
                    StuffingTableDataList[0].StuffStuffingContainerNo.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffDeclaredPkgs != StuffingTableDataList[0].StuffDeclaredPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffDeclaredPkgs", DetailedListValue.StuffDeclaredPkgs,
                    StuffingTableDataList[0].StuffDeclaredPkgs.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffDeclaredWeight != StuffingTableDataList[0].StuffDeclaredWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffDeclaredWeight", DetailedListValue.StuffDeclaredWeight,
                    StuffingTableDataList[0].StuffDeclaredWeight.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffStuffedFrom != StuffingTableDataList[0].StuffStuffedFrom.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffStuffedFrom", DetailedListValue.StuffStuffedFrom,
                    StuffingTableDataList[0].StuffStuffedFrom.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffStuffedTo != StuffingTableDataList[0].StuffStuffedTo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffStuffedTo", DetailedListValue.StuffStuffedTo,
                    StuffingTableDataList[0].StuffStuffedTo.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffStuffedWeight != StuffingTableDataList[0].StuffStuffedWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffStuffedWeight", DetailedListValue.StuffStuffedWeight,
                    StuffingTableDataList[0].StuffStuffedWeight.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffStuffedPkgs != StuffingTableDataList[0].StuffStuffedPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffStuffedPkgs", DetailedListValue.StuffStuffedPkgs,
                    StuffingTableDataList[0].StuffStuffedPkgs.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffRemarks != StuffingTableDataList[0].StuffRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffRemarks", DetailedListValue.StuffRemarks,
                    StuffingTableDataList[0].StuffRemarks.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.StuffWorkOrderStatus != StuffingTableDataList[0].StuffWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportStuffing",
                    StuffingTableDataList[0].StuffWorkOrderNo.Trim(), "StuffWorkOrderStatus", DetailedListValue.StuffWorkOrderStatus,
                    StuffingTableDataList[0].StuffWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + StuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "StuffWorkOrderNo = '" + StuffingTableDataList[0].StuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static StuffingTableDataList[] StuffingTableSearchData(List<StuffingTableDataList> StuffingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<StuffingTableDataList> DetailedList = new List<StuffingTableDataList>();
        StuffingTableDataList DetailedListValue = new StuffingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select " +
                                 "A.*, B.ContainerCargoWeightKg, B.ContainerNoofPackage " +
                                 "from CFSImportStuffing A " +
                                 "Left Join CFSImportContainer B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.StuffDeStuffingContainerNo = B.ContainerNo " +
                                 "Where A.RecordStatus = 'Active' and A.MainJobNo = @MainJobNo; ", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", StuffingTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new StuffingTableDataList();
                DetailedListValue.StuffWorkOrderNo = dtrow["StuffWorkOrderNo"].ToString();
                DetailedListValue.StuffWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["StuffWorkOrderDate"]);
                DetailedListValue.StuffDeStuffingContainerNo = dtrow["StuffDeStuffingContainerNo"].ToString();
                DetailedListValue.StuffStuffingContainerNo = dtrow["StuffStuffingContainerNo"].ToString();
                DetailedListValue.StuffDeclaredPkgs = dtrow["ContainerNoofPackage"].ToString();
                DetailedListValue.StuffDeclaredWeight = dtrow["ContainerCargoWeightKg"].ToString();
                DetailedListValue.StuffStuffedFrom = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["StuffStuffedFrom"]);
                DetailedListValue.StuffStuffedTo = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["StuffStuffedTo"]);
                DetailedListValue.StuffStuffedWeight = dtrow["StuffStuffedWeight"].ToString();
                DetailedListValue.StuffStuffedPkgs = dtrow["StuffStuffedPkgs"].ToString();
                DetailedListValue.StuffRemarks = dtrow["StuffRemarks"].ToString();
                DetailedListValue.StuffWorkOrderStatus = dtrow["StuffWorkOrderStatus"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class DeStuffingTableDataList
    {
        public string MainJobNo { get; set; }
        public string DeStuffWorkOrderNo { get; set; }
        public string DeStuffWorkOrderDate { get; set; }
        public string DeStuffContainerNo { get; set; }
        public string DeStuffVehicleNo { get; set; }
        public string DeStuffFromDate { get; set; }
        public string DeStuffToDate { get; set; }
        public string DeStuffContainerCondition { get; set; }
        public string DeStuffDeclaredWeight { get; set; }
        public string DeStuffDeclaredPkgs { get; set; }
        public string DeStuffDeStuffedWeight { get; set; }
        public string DeStuffDeStuffedPkgs { get; set; }
        public string DeStuffDestuffMarkNo { get; set; }
        public string DeStuffDestuffLocation { get; set; }
        public string DeStuffAreainSqmt { get; set; }
        public string DeStuffVolume { get; set; }
        public string DeStuffMode { get; set; }
        public string DeStuffShort { get; set; }
        public string DeStuffExcess { get; set; }
        public string DeStuffNoofGrids { get; set; }
        public string DeStuffDelayDueTo { get; set; }
        public string DeStuffDelayRemarks { get; set; }
        public string DeStuffContractor { get; set; }
        public string DeStuffSupervisor { get; set; }
        public string DeStuffMarksNo { get; set; }
        public string DeStuffMovementType { get; set; }
        public string DeStuffRemarks { get; set; }
        public string DeStuffWorkOrderStatus { get; set; }
        public string DeStuffBalancePkgs { get; set; }
        public string DeStuffBalanceWeight { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------DeStuffingTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string DeStuffingTableUpdateData(List<DeStuffingTableDataList> DeStuffingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<DeStuffingTableDataList> DetailedList = new List<DeStuffingTableDataList>();
        DeStuffingTableDataList DetailedListValue = new DeStuffingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportDeStuffing WHERE MainJobNo = @MainJobNo and DeStuffWorkOrderNo = @DeStuffWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", DeStuffingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@DeStuffWorkOrderNo", DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.DeStuffWorkOrderNo = dtrow["DeStuffWorkOrderNo"].ToString();
                DetailedListValue.DeStuffWorkOrderDate = dtrow["DeStuffWorkOrderDate"].ToString();
                if (dtrow["DeStuffWorkOrderDate"].ToString() != "" || dtrow["DeStuffWorkOrderDate"] != DBNull.Value) { DetailedListValue.DeStuffWorkOrderDate = Convert.ToDateTime(dtrow["DeStuffWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.DeStuffWorkOrderDate = ""; }
                DetailedListValue.DeStuffContainerNo = dtrow["DeStuffContainerNo"].ToString();
                //DetailedListValue.DeStuffVehicleNo = dtrow["DeStuffVehicleNo"].ToString();
                DetailedListValue.DeStuffFromDate = dtrow["DeStuffFromDate"].ToString();
                if (dtrow["DeStuffFromDate"].ToString() != "" || dtrow["DeStuffFromDate"] != DBNull.Value) { DetailedListValue.DeStuffFromDate = Convert.ToDateTime(dtrow["DeStuffFromDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.DeStuffFromDate = ""; }
                DetailedListValue.DeStuffToDate = dtrow["DeStuffToDate"].ToString();
                if (dtrow["DeStuffToDate"].ToString() != "" || dtrow["DeStuffToDate"] != DBNull.Value) { DetailedListValue.DeStuffToDate = Convert.ToDateTime(dtrow["DeStuffToDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.DeStuffToDate = ""; }
                DetailedListValue.DeStuffContainerCondition = dtrow["DeStuffContainerCondition"].ToString();
                DetailedListValue.DeStuffDeclaredWeight = dtrow["DeStuffDeclaredWeight"].ToString();
                DetailedListValue.DeStuffDeclaredPkgs = dtrow["DeStuffDeclaredPkgs"].ToString();
                DetailedListValue.DeStuffDeStuffedWeight = dtrow["DeStuffDeStuffedWeight"].ToString();
                DetailedListValue.DeStuffDeStuffedPkgs = dtrow["DeStuffDeStuffedPkgs"].ToString();
                DetailedListValue.DeStuffDestuffMarkNo = dtrow["DeStuffDestuffMarkNo"].ToString();
                DetailedListValue.DeStuffDestuffLocation = dtrow["DeStuffDestuffLocation"].ToString();
                DetailedListValue.DeStuffAreainSqmt = dtrow["DeStuffAreainSqmt"].ToString();
                DetailedListValue.DeStuffVolume = dtrow["DeStuffVolume"].ToString();
                DetailedListValue.DeStuffMode = dtrow["DeStuffMode"].ToString();
                DetailedListValue.DeStuffShort = dtrow["DeStuffShort"].ToString();
                DetailedListValue.DeStuffExcess = dtrow["DeStuffExcess"].ToString();
                DetailedListValue.DeStuffNoofGrids = dtrow["DeStuffNoofGrids"].ToString();
                DetailedListValue.DeStuffDelayDueTo = dtrow["DeStuffDelayDueTo"].ToString();
                DetailedListValue.DeStuffDelayRemarks = dtrow["DeStuffDelayRemarks"].ToString();
                DetailedListValue.DeStuffContractor = dtrow["DeStuffContractor"].ToString();
                DetailedListValue.DeStuffSupervisor = dtrow["DeStuffSupervisor"].ToString();
                DetailedListValue.DeStuffMarksNo = dtrow["DeStuffMarksNo"].ToString();
                DetailedListValue.DeStuffMovementType = dtrow["DeStuffMovementType"].ToString();
                DetailedListValue.DeStuffRemarks = dtrow["DeStuffRemarks"].ToString();
                DetailedListValue.DeStuffWorkOrderStatus = dtrow["DeStuffWorkOrderStatus"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportDeStuffing set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, DeStuffingStatus=@DeStuffingStatus,DeStuffWorkOrderNo=@DeStuffWorkOrderNo,DeStuffWorkOrderDate=@DeStuffWorkOrderDate,DeStuffContainerNo=@DeStuffContainerNo,DeStuffFromDate=@DeStuffFromDate,DeStuffToDate=@DeStuffToDate,DeStuffContainerCondition=@DeStuffContainerCondition,DeStuffDeclaredWeight=@DeStuffDeclaredWeight,DeStuffDeclaredPkgs=@DeStuffDeclaredPkgs,DeStuffDeStuffedWeight=@DeStuffDeStuffedWeight,DeStuffDeStuffedPkgs=@DeStuffDeStuffedPkgs,DeStuffDestuffMarkNo=@DeStuffDestuffMarkNo,DeStuffDestuffLocation=@DeStuffDestuffLocation,DeStuffAreainSqmt=@DeStuffAreainSqmt,DeStuffVolume=@DeStuffVolume,DeStuffMode=@DeStuffMode,DeStuffShort=@DeStuffShort,DeStuffExcess=@DeStuffExcess,DeStuffNoofGrids=@DeStuffNoofGrids,DeStuffDelayDueTo=@DeStuffDelayDueTo,DeStuffDelayRemarks=@DeStuffDelayRemarks,DeStuffContractor=@DeStuffContractor,DeStuffSupervisor=@DeStuffSupervisor,DeStuffMarksNo=@DeStuffMarksNo,DeStuffMovementType=@DeStuffMovementType,DeStuffRemarks=@DeStuffRemarks,DeStuffWorkOrderStatus=@DeStuffWorkOrderStatus " +//DeStuffVehicleNo=@DeStuffVehicleNo,
                                 "where DeStuffWorkOrderNo = @DeStuffWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", DeStuffingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@DeStuffWorkOrderNo", DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim());
            if (DeStuffingTableDataList[0].DeStuffWorkOrderDate == "" || DeStuffingTableDataList[0].DeStuffWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@DeStuffWorkOrderDate", DBNull.Value);
                DeStuffingTableDataList[0].DeStuffWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("DeStuffWorkOrderDate", Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffWorkOrderDate.Trim()));
                DeStuffingTableDataList[0].DeStuffWorkOrderDate = Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@DeStuffContainerNo", DeStuffingTableDataList[0].DeStuffContainerNo.Trim());
            //cmd.Parameters.AddWithValue("@DeStuffVehicleNo", DeStuffingTableDataList[0].DeStuffVehicleNo.Trim());
            if (DeStuffingTableDataList[0].DeStuffFromDate == "" || DeStuffingTableDataList[0].DeStuffFromDate == null)
            {
                cmd.Parameters.AddWithValue("@DeStuffFromDate", DBNull.Value);
                DeStuffingTableDataList[0].DeStuffFromDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("DeStuffFromDate", Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffFromDate.Trim()));
                DeStuffingTableDataList[0].DeStuffFromDate = Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffFromDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (DeStuffingTableDataList[0].DeStuffToDate == "" || DeStuffingTableDataList[0].DeStuffToDate == null)
            {
                cmd.Parameters.AddWithValue("@DeStuffToDate", DBNull.Value);
                DeStuffingTableDataList[0].DeStuffToDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("DeStuffToDate", Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffToDate.Trim()));
                DeStuffingTableDataList[0].DeStuffToDate = Convert.ToDateTime(DeStuffingTableDataList[0].DeStuffToDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@DeStuffContainerCondition", DeStuffingTableDataList[0].DeStuffContainerCondition.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDeclaredWeight", DeStuffingTableDataList[0].DeStuffDeclaredWeight.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDeclaredPkgs", DeStuffingTableDataList[0].DeStuffDeclaredPkgs.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDeStuffedWeight", DeStuffingTableDataList[0].DeStuffDeStuffedWeight.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDeStuffedPkgs", DeStuffingTableDataList[0].DeStuffDeStuffedPkgs.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDestuffMarkNo", DeStuffingTableDataList[0].DeStuffDestuffMarkNo.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDestuffLocation", DeStuffingTableDataList[0].DeStuffDestuffLocation.Trim());
            cmd.Parameters.AddWithValue("@DeStuffAreainSqmt", DeStuffingTableDataList[0].DeStuffAreainSqmt.Trim());
            cmd.Parameters.AddWithValue("@DeStuffVolume", DeStuffingTableDataList[0].DeStuffVolume.Trim());
            cmd.Parameters.AddWithValue("@DeStuffMode", DeStuffingTableDataList[0].DeStuffMode.Trim());
            cmd.Parameters.AddWithValue("@DeStuffShort", DeStuffingTableDataList[0].DeStuffShort.Trim());
            cmd.Parameters.AddWithValue("@DeStuffExcess", DeStuffingTableDataList[0].DeStuffExcess.Trim());
            cmd.Parameters.AddWithValue("@DeStuffNoofGrids", DeStuffingTableDataList[0].DeStuffNoofGrids.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDelayDueTo", DeStuffingTableDataList[0].DeStuffDelayDueTo.Trim());
            cmd.Parameters.AddWithValue("@DeStuffDelayRemarks", DeStuffingTableDataList[0].DeStuffDelayRemarks.Trim());
            cmd.Parameters.AddWithValue("@DeStuffContractor", DeStuffingTableDataList[0].DeStuffContractor.Trim());
            cmd.Parameters.AddWithValue("@DeStuffSupervisor", DeStuffingTableDataList[0].DeStuffSupervisor.Trim());
            cmd.Parameters.AddWithValue("@DeStuffMarksNo", DeStuffingTableDataList[0].DeStuffMarksNo.Trim());
            cmd.Parameters.AddWithValue("@DeStuffMovementType", DeStuffingTableDataList[0].DeStuffMovementType.Trim());
            cmd.Parameters.AddWithValue("@DeStuffRemarks", DeStuffingTableDataList[0].DeStuffRemarks.Trim());
            cmd.Parameters.AddWithValue("@DeStuffWorkOrderStatus", DeStuffingTableDataList[0].DeStuffWorkOrderStatus.Trim());
            if (Convert.ToInt32(DeStuffingTableDataList[0].DeStuffBalancePkgs.Trim()) == 0 && Convert.ToInt32(DeStuffingTableDataList[0].DeStuffBalanceWeight.Trim()) == 0)
            {
                cmd.Parameters.AddWithValue("@DeStuffingStatus", "Fully De-Stuffed");

                SqlCommand cmmd = new SqlCommand("Update CFSImportContainer Set IsContainerEmpty = @IsContainerEmpty Where MainJobno = @MainJobno And ContainerNo = @ContainerNo And RecordStatus = 'Active'", cnn);
                cmmd.Parameters.AddWithValue("@IsContainerEmpty", "Yes");
                cmmd.Parameters.AddWithValue("@MainJobno", DeStuffingTableDataList[0].MainJobNo.Trim());
                cmmd.Parameters.AddWithValue("@ContainerNo", DeStuffingTableDataList[0].DeStuffContainerNo.Trim());
                cnn.Open();
                cmmd.ExecuteNonQuery();
                cnn.Close();
            }
            else { cmd.Parameters.AddWithValue("@DeStuffingStatus", "Partially De-Stuffed"); }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.DeStuffWorkOrderNo != DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffWorkOrderNo", DetailedListValue.DeStuffWorkOrderNo,
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffWorkOrderDate != DeStuffingTableDataList[0].DeStuffWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffWorkOrderDate", DetailedListValue.DeStuffWorkOrderDate,
                    DeStuffingTableDataList[0].DeStuffWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffContainerNo != DeStuffingTableDataList[0].DeStuffContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffContainerNo", DetailedListValue.DeStuffContainerNo,
                    DeStuffingTableDataList[0].DeStuffContainerNo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                //if (DetailedListValue.DeStuffVehicleNo != DeStuffingTableDataList[0].DeStuffVehicleNo.Trim())
                //{
                //    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                //    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffVehicleNo", DetailedListValue.DeStuffVehicleNo,
                //    DeStuffingTableDataList[0].DeStuffVehicleNo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                //    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                //    HttpContext.Current.Session["UserName"].ToString());
                //}
                if (DetailedListValue.DeStuffFromDate != DeStuffingTableDataList[0].DeStuffFromDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffFromDate", DetailedListValue.DeStuffFromDate,
                    DeStuffingTableDataList[0].DeStuffFromDate.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffToDate != DeStuffingTableDataList[0].DeStuffToDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffToDate", DetailedListValue.DeStuffToDate,
                    DeStuffingTableDataList[0].DeStuffToDate.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffContainerCondition != DeStuffingTableDataList[0].DeStuffContainerCondition.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffContainerCondition", DetailedListValue.DeStuffContainerCondition,
                    DeStuffingTableDataList[0].DeStuffContainerCondition.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDeclaredWeight != DeStuffingTableDataList[0].DeStuffDeclaredWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDeclaredWeight", DetailedListValue.DeStuffDeclaredWeight,
                    DeStuffingTableDataList[0].DeStuffDeclaredWeight.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDeclaredPkgs != DeStuffingTableDataList[0].DeStuffDeclaredPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDeclaredPkgs", DetailedListValue.DeStuffDeclaredPkgs,
                    DeStuffingTableDataList[0].DeStuffDeclaredPkgs.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDeStuffedWeight != DeStuffingTableDataList[0].DeStuffDeStuffedWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDeStuffedWeight", DetailedListValue.DeStuffDeStuffedWeight,
                    DeStuffingTableDataList[0].DeStuffDeStuffedWeight.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDeStuffedPkgs != DeStuffingTableDataList[0].DeStuffDeStuffedPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDeStuffedPkgs", DetailedListValue.DeStuffDeStuffedPkgs,
                    DeStuffingTableDataList[0].DeStuffDeStuffedPkgs.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDestuffMarkNo != DeStuffingTableDataList[0].DeStuffDestuffMarkNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDestuffMarkNo", DetailedListValue.DeStuffDestuffMarkNo,
                    DeStuffingTableDataList[0].DeStuffDestuffMarkNo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDestuffLocation != DeStuffingTableDataList[0].DeStuffDestuffLocation.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDestuffLocation", DetailedListValue.DeStuffDestuffLocation,
                    DeStuffingTableDataList[0].DeStuffDestuffLocation.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffAreainSqmt != DeStuffingTableDataList[0].DeStuffAreainSqmt.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffAreainSqmt", DetailedListValue.DeStuffAreainSqmt,
                    DeStuffingTableDataList[0].DeStuffAreainSqmt.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffVolume != DeStuffingTableDataList[0].DeStuffVolume.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffVolume", DetailedListValue.DeStuffVolume,
                    DeStuffingTableDataList[0].DeStuffVolume.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffMode != DeStuffingTableDataList[0].DeStuffMode.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffMode", DetailedListValue.DeStuffMode,
                    DeStuffingTableDataList[0].DeStuffMode.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffShort != DeStuffingTableDataList[0].DeStuffShort.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffShort", DetailedListValue.DeStuffShort,
                    DeStuffingTableDataList[0].DeStuffShort.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffExcess != DeStuffingTableDataList[0].DeStuffExcess.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffExcess", DetailedListValue.DeStuffExcess,
                    DeStuffingTableDataList[0].DeStuffExcess.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffNoofGrids != DeStuffingTableDataList[0].DeStuffNoofGrids.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffNoofGrids", DetailedListValue.DeStuffNoofGrids,
                    DeStuffingTableDataList[0].DeStuffNoofGrids.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDelayDueTo != DeStuffingTableDataList[0].DeStuffDelayDueTo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDelayDueTo", DetailedListValue.DeStuffDelayDueTo,
                    DeStuffingTableDataList[0].DeStuffDelayDueTo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffDelayRemarks != DeStuffingTableDataList[0].DeStuffDelayRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffDelayRemarks", DetailedListValue.DeStuffDelayRemarks,
                    DeStuffingTableDataList[0].DeStuffDelayRemarks.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffContractor != DeStuffingTableDataList[0].DeStuffContractor.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffContractor", DetailedListValue.DeStuffContractor,
                    DeStuffingTableDataList[0].DeStuffContractor.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffSupervisor != DeStuffingTableDataList[0].DeStuffSupervisor.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffSupervisor", DetailedListValue.DeStuffSupervisor,
                    DeStuffingTableDataList[0].DeStuffSupervisor.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffMarksNo != DeStuffingTableDataList[0].DeStuffMarksNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffMarksNo", DetailedListValue.DeStuffMarksNo,
                    DeStuffingTableDataList[0].DeStuffMarksNo.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffMovementType != DeStuffingTableDataList[0].DeStuffMovementType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffMovementType", DetailedListValue.DeStuffMovementType,
                    DeStuffingTableDataList[0].DeStuffMovementType.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffRemarks != DeStuffingTableDataList[0].DeStuffRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffRemarks", DetailedListValue.DeStuffRemarks,
                    DeStuffingTableDataList[0].DeStuffRemarks.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.DeStuffWorkOrderStatus != DeStuffingTableDataList[0].DeStuffWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportDeStuffing",
                    DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim(), "DeStuffWorkOrderStatus", DetailedListValue.DeStuffWorkOrderStatus,
                    DeStuffingTableDataList[0].DeStuffWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + DeStuffingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "DeStuffWorkOrderNo = '" + DeStuffingTableDataList[0].DeStuffWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static DeStuffingTableDataList[] DeStuffingTableSearchData(List<DeStuffingTableDataList> DeStuffingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<DeStuffingTableDataList> DetailedList = new List<DeStuffingTableDataList>();
        DeStuffingTableDataList DetailedListValue = new DeStuffingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select A.*, B.ContainerCargoWeightKg, B.ContainerNoofPackage from CFSImportDeStuffing A " +
                                 "Left Join CFSImportContainer B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.DeStuffContainerNo = B.ContainerNo " +
                                 "Where A.RecordStatus = 'Active' and A.MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", DeStuffingTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new DeStuffingTableDataList();
                DetailedListValue.DeStuffWorkOrderNo = dtrow["DeStuffWorkOrderNo"].ToString();
                DetailedListValue.DeStuffWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["DeStuffWorkOrderDate"]);
                DetailedListValue.DeStuffContainerNo = dtrow["DeStuffContainerNo"].ToString();
                //DetailedListValue.DeStuffVehicleNo = dtrow["DeStuffVehicleNo"].ToString();
                DetailedListValue.DeStuffFromDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["DeStuffFromDate"]);
                DetailedListValue.DeStuffToDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["DeStuffToDate"]);
                DetailedListValue.DeStuffContainerCondition = dtrow["DeStuffContainerCondition"].ToString();
                DetailedListValue.DeStuffDeclaredWeight = dtrow["ContainerCargoWeightKg"].ToString();
                DetailedListValue.DeStuffDeclaredPkgs = dtrow["ContainerNoofPackage"].ToString();
                DetailedListValue.DeStuffDeStuffedWeight = dtrow["DeStuffDeStuffedWeight"].ToString();
                DetailedListValue.DeStuffDeStuffedPkgs = dtrow["DeStuffDeStuffedPkgs"].ToString();
                DetailedListValue.DeStuffDestuffMarkNo = dtrow["DeStuffDestuffMarkNo"].ToString();
                DetailedListValue.DeStuffDestuffLocation = dtrow["DeStuffDestuffLocation"].ToString();
                DetailedListValue.DeStuffAreainSqmt = dtrow["DeStuffAreainSqmt"].ToString();
                DetailedListValue.DeStuffVolume = dtrow["DeStuffVolume"].ToString();
                DetailedListValue.DeStuffMode = dtrow["DeStuffMode"].ToString();
                DetailedListValue.DeStuffShort = dtrow["DeStuffShort"].ToString();
                DetailedListValue.DeStuffExcess = dtrow["DeStuffExcess"].ToString();
                DetailedListValue.DeStuffNoofGrids = dtrow["DeStuffNoofGrids"].ToString();
                DetailedListValue.DeStuffDelayDueTo = dtrow["DeStuffDelayDueTo"].ToString();
                DetailedListValue.DeStuffDelayRemarks = dtrow["DeStuffDelayRemarks"].ToString();
                DetailedListValue.DeStuffContractor = dtrow["DeStuffContractor"].ToString();
                DetailedListValue.DeStuffSupervisor = dtrow["DeStuffSupervisor"].ToString();
                DetailedListValue.DeStuffMarksNo = dtrow["DeStuffMarksNo"].ToString();
                DetailedListValue.DeStuffMovementType = dtrow["DeStuffMovementType"].ToString();
                DetailedListValue.DeStuffRemarks = dtrow["DeStuffRemarks"].ToString();
                DetailedListValue.DeStuffWorkOrderStatus = dtrow["DeStuffWorkOrderStatus"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class LoadedContainerOutTableDataList
    {
        public string MainJobNo { get; set; }
        public string LoadOutWorkOrderNo { get; set; }
        public string LoadOutWorkOrderDate { get; set; }
        public string LoadOutContainerNo { get; set; }
        public string LoadOutVehicleNo { get; set; }
        public string LoadOutCustDutyValue { get; set; }
        public string LoadOutStampDutyValue { get; set; }
        public string LoadOutOpenOrderDate { get; set; }
        public string LoadOutOutofChargeDate { get; set; }
        public string LoadOutCustOutofChargeNo { get; set; }
        public string LoadOutCondition { get; set; }
        public string LoadOutRemarks { get; set; }
        public string LoadOutWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------LoadedContainerOutTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LoadedContainerOutTableUpdateData(List<LoadedContainerOutTableDataList> LoadedContainerOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<LoadedContainerOutTableDataList> DetailedList = new List<LoadedContainerOutTableDataList>();
        LoadedContainerOutTableDataList DetailedListValue = new LoadedContainerOutTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportLoadedContainerOut WHERE MainJobNo = @MainJobNo and LoadOutWorkOrderNo = @LoadOutWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LoadOutWorkOrderNo", LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.LoadOutWorkOrderNo = dtrow["LoadOutWorkOrderNo"].ToString();
                DetailedListValue.LoadOutWorkOrderDate = dtrow["LoadOutWorkOrderDate"].ToString();
                if (dtrow["LoadOutWorkOrderDate"].ToString() != "" || dtrow["LoadOutWorkOrderDate"] != DBNull.Value) { DetailedListValue.LoadOutWorkOrderDate = Convert.ToDateTime(dtrow["LoadOutWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LoadOutWorkOrderDate = ""; }
                DetailedListValue.LoadOutContainerNo = dtrow["LoadOutContainerNo"].ToString();
                DetailedListValue.LoadOutVehicleNo = dtrow["LoadOutVehicleNo"].ToString();
                DetailedListValue.LoadOutCustDutyValue = dtrow["LoadOutCustDutyValue"].ToString();
                DetailedListValue.LoadOutStampDutyValue = dtrow["LoadOutStampDutyValue"].ToString();
                DetailedListValue.LoadOutOpenOrderDate = dtrow["LoadOutOpenOrderDate"].ToString();
                if (dtrow["LoadOutOpenOrderDate"].ToString() != "" || dtrow["LoadOutOpenOrderDate"] != DBNull.Value) { DetailedListValue.LoadOutOpenOrderDate = Convert.ToDateTime(dtrow["LoadOutOpenOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LoadOutOpenOrderDate = ""; }
                DetailedListValue.LoadOutOutofChargeDate = dtrow["LoadOutOutofChargeDate"].ToString();
                if (dtrow["LoadOutOutofChargeDate"].ToString() != "" || dtrow["LoadOutOutofChargeDate"] != DBNull.Value) { DetailedListValue.LoadOutOutofChargeDate = Convert.ToDateTime(dtrow["LoadOutOutofChargeDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.LoadOutOutofChargeDate = ""; }
                DetailedListValue.LoadOutCustOutofChargeNo = dtrow["LoadOutCustOutofChargeNo"].ToString();
                DetailedListValue.LoadOutCondition = dtrow["LoadOutCondition"].ToString();
                DetailedListValue.LoadOutRemarks = dtrow["LoadOutRemarks"].ToString();
                DetailedListValue.LoadOutWorkOrderStatus = dtrow["LoadOutWorkOrderStatus"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportLoadedContainerOut set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, LoadOutWorkOrderNo=@LoadOutWorkOrderNo,LoadOutWorkOrderDate=@LoadOutWorkOrderDate,LoadOutContainerNo=@LoadOutContainerNo,LoadOutVehicleNo=@LoadOutVehicleNo,LoadOutCustDutyValue=@LoadOutCustDutyValue,LoadOutStampDutyValue=@LoadOutStampDutyValue,LoadOutOpenOrderDate=@LoadOutOpenOrderDate,LoadOutOutofChargeDate=@LoadOutOutofChargeDate,LoadOutCustOutofChargeNo=@LoadOutCustOutofChargeNo,LoadOutCondition=@LoadOutCondition,LoadOutRemarks=@LoadOutRemarks,LoadOutWorkOrderStatus=@LoadOutWorkOrderStatus " +
                                 "where LoadOutWorkOrderNo = @LoadOutWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LoadOutWorkOrderNo", LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim());
            if (LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate == "" || LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadOutWorkOrderDate", DBNull.Value);
                LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadOutWorkOrderDate", Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate.Trim()));
                LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate = Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LoadOutContainerNo", LoadedContainerOutTableDataList[0].LoadOutContainerNo.Trim());
            cmd.Parameters.AddWithValue("@LoadOutVehicleNo", LoadedContainerOutTableDataList[0].LoadOutVehicleNo.Trim());
            cmd.Parameters.AddWithValue("@LoadOutCustDutyValue", LoadedContainerOutTableDataList[0].LoadOutCustDutyValue.Trim());
            cmd.Parameters.AddWithValue("@LoadOutStampDutyValue", LoadedContainerOutTableDataList[0].LoadOutStampDutyValue.Trim());
            if (LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate == "" || LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadOutOpenOrderDate", DBNull.Value);
                LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadOutOpenOrderDate", Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate.Trim()).Date);
                LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate = Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate == "" || LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadOutOutofChargeDate", DBNull.Value);
                LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadOutOutofChargeDate", Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate.Trim()).Date);
                LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate = Convert.ToDateTime(LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LoadOutCustOutofChargeNo", LoadedContainerOutTableDataList[0].LoadOutCustOutofChargeNo.Trim());
            cmd.Parameters.AddWithValue("@LoadOutCondition", LoadedContainerOutTableDataList[0].LoadOutCondition.Trim());
            cmd.Parameters.AddWithValue("@LoadOutRemarks", LoadedContainerOutTableDataList[0].LoadOutRemarks.Trim());
            cmd.Parameters.AddWithValue("@LoadOutWorkOrderStatus", LoadedContainerOutTableDataList[0].LoadOutWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.LoadOutWorkOrderNo != LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutWorkOrderNo", DetailedListValue.LoadOutWorkOrderNo,
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutWorkOrderDate != LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutWorkOrderDate", DetailedListValue.LoadOutWorkOrderDate,
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutContainerNo != LoadedContainerOutTableDataList[0].LoadOutContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutContainerNo", DetailedListValue.LoadOutContainerNo,
                    LoadedContainerOutTableDataList[0].LoadOutContainerNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutVehicleNo != LoadedContainerOutTableDataList[0].LoadOutVehicleNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutVehicleNo", DetailedListValue.LoadOutVehicleNo,
                    LoadedContainerOutTableDataList[0].LoadOutVehicleNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutCustDutyValue != LoadedContainerOutTableDataList[0].LoadOutCustDutyValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutCustDutyValue", DetailedListValue.LoadOutCustDutyValue,
                    LoadedContainerOutTableDataList[0].LoadOutCustDutyValue.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutStampDutyValue != LoadedContainerOutTableDataList[0].LoadOutStampDutyValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutStampDutyValue", DetailedListValue.LoadOutStampDutyValue,
                    LoadedContainerOutTableDataList[0].LoadOutStampDutyValue.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutOpenOrderDate != LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutOpenOrderDate", DetailedListValue.LoadOutOpenOrderDate,
                    LoadedContainerOutTableDataList[0].LoadOutOpenOrderDate.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutOutofChargeDate != LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutOutofChargeDate", DetailedListValue.LoadOutOutofChargeDate,
                    LoadedContainerOutTableDataList[0].LoadOutOutofChargeDate.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutCustOutofChargeNo != LoadedContainerOutTableDataList[0].LoadOutCustOutofChargeNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutCustOutofChargeNo", DetailedListValue.LoadOutCustOutofChargeNo,
                    LoadedContainerOutTableDataList[0].LoadOutCustOutofChargeNo.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutCondition != LoadedContainerOutTableDataList[0].LoadOutCondition.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutCondition", DetailedListValue.LoadOutCondition,
                    LoadedContainerOutTableDataList[0].LoadOutCondition.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutRemarks != LoadedContainerOutTableDataList[0].LoadOutRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutRemarks", DetailedListValue.LoadOutRemarks,
                    LoadedContainerOutTableDataList[0].LoadOutRemarks.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.LoadOutWorkOrderStatus != LoadedContainerOutTableDataList[0].LoadOutWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportLoadedContainerOut",
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim(), "LoadOutWorkOrderStatus", DetailedListValue.LoadOutWorkOrderStatus,
                    LoadedContainerOutTableDataList[0].LoadOutWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + LoadedContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "LoadOutWorkOrderNo = '" + LoadedContainerOutTableDataList[0].LoadOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static LoadedContainerOutTableDataList[] LoadedContainerOutTableSearchData(List<LoadedContainerOutTableDataList> LoadedContainerOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<LoadedContainerOutTableDataList> DetailedList = new List<LoadedContainerOutTableDataList>();
        LoadedContainerOutTableDataList DetailedListValue = new LoadedContainerOutTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportLoadedContainerOut Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedContainerOutTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new LoadedContainerOutTableDataList();
                DetailedListValue.LoadOutWorkOrderNo = dtrow["LoadOutWorkOrderNo"].ToString();
                DetailedListValue.LoadOutWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["LoadOutWorkOrderDate"]);
                DetailedListValue.LoadOutContainerNo = dtrow["LoadOutContainerNo"].ToString();
                DetailedListValue.LoadOutVehicleNo = dtrow["LoadOutVehicleNo"].ToString();
                DetailedListValue.LoadOutCustDutyValue = dtrow["LoadOutCustDutyValue"].ToString();
                DetailedListValue.LoadOutStampDutyValue = dtrow["LoadOutStampDutyValue"].ToString();
                DetailedListValue.LoadOutOpenOrderDate = String.Format("{0:yyyy-MM-dd}", dtrow["LoadOutOpenOrderDate"]);
                DetailedListValue.LoadOutOutofChargeDate = String.Format("{0:yyyy-MM-dd}", dtrow["LoadOutOutofChargeDate"]);
                DetailedListValue.LoadOutCustOutofChargeNo = dtrow["LoadOutCustOutofChargeNo"].ToString();
                DetailedListValue.LoadOutCondition = dtrow["LoadOutCondition"].ToString();
                DetailedListValue.LoadOutRemarks = dtrow["LoadOutRemarks"].ToString();
                DetailedListValue.LoadOutWorkOrderStatus = dtrow["LoadOutWorkOrderStatus"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class EmptyContainerOutTableDataList
    {
        public string MainJobNo { get; set; }
        public string MtyOutWorkOrderNo { get; set; }
        public string MtyOutWorkOrderDate { get; set; }
        public string MtyOutContainerNo { get; set; }
        public string MtyOutVehicleNo { get; set; }
        public string MtyOutModeofGateOut { get; set; }
        public string MtyOutCycle { get; set; }
        public string MtyOutDriverName { get; set; }
        public string MtyOutEquipmentCondition { get; set; }
        public string MtyOutContainerTag { get; set; }
        public string MtyOutRemarks { get; set; }
        public string MtyOutWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
        public string MtyMovementBy { get; set; }
    }
    //-----------------------------------------------------------EmptyContainerOutTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string EmptyContainerOutTableUpdateData(List<EmptyContainerOutTableDataList> EmptyContainerOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<EmptyContainerOutTableDataList> DetailedList = new List<EmptyContainerOutTableDataList>();
        EmptyContainerOutTableDataList DetailedListValue = new EmptyContainerOutTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportEmptyContainerOut WHERE MainJobNo = @MainJobNo and MtyOutWorkOrderNo = @MtyOutWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", EmptyContainerOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@MtyOutWorkOrderNo", EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.MtyOutWorkOrderNo = dtrow["MtyOutWorkOrderNo"].ToString();
                DetailedListValue.MtyOutWorkOrderDate = dtrow["MtyOutWorkOrderDate"].ToString();
                if (dtrow["MtyOutWorkOrderDate"].ToString() != "" || dtrow["MtyOutWorkOrderDate"] != DBNull.Value) { DetailedListValue.MtyOutWorkOrderDate = Convert.ToDateTime(dtrow["MtyOutWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.MtyOutWorkOrderDate = ""; }
                DetailedListValue.MtyOutContainerNo = dtrow["MtyOutContainerNo"].ToString();
                DetailedListValue.MtyOutVehicleNo = dtrow["MtyOutVehicleNo"].ToString();
                DetailedListValue.MtyOutModeofGateOut = dtrow["MtyOutModeofGateOut"].ToString();
                DetailedListValue.MtyOutCycle = dtrow["MtyOutCycle"].ToString();
                DetailedListValue.MtyOutDriverName = dtrow["MtyOutDriverName"].ToString();
                DetailedListValue.MtyOutEquipmentCondition = dtrow["MtyOutEquipmentCondition"].ToString();
                DetailedListValue.MtyOutContainerTag = dtrow["MtyOutContainerTag"].ToString();
                DetailedListValue.MtyOutRemarks = dtrow["MtyOutRemarks"].ToString();
                DetailedListValue.MtyOutWorkOrderStatus = dtrow["MtyOutWorkOrderStatus"].ToString();
                DetailedListValue.MtyMovementBy = dtrow["MtyMovementBy"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportEmptyContainerOut set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, MtyMovementBy=@MtyMovementBy,MtyOutWorkOrderNo=@MtyOutWorkOrderNo,MtyOutWorkOrderDate=@MtyOutWorkOrderDate,MtyOutContainerNo=@MtyOutContainerNo,MtyOutVehicleNo=@MtyOutVehicleNo,MtyOutModeofGateOut=@MtyOutModeofGateOut,MtyOutCycle=@MtyOutCycle,MtyOutDriverName=@MtyOutDriverName,MtyOutEquipmentCondition=@MtyOutEquipmentCondition,MtyOutContainerTag=@MtyOutContainerTag,MtyOutRemarks=@MtyOutRemarks,MtyOutWorkOrderStatus=@MtyOutWorkOrderStatus where MtyOutWorkOrderNo = @MtyOutWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", EmptyContainerOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@MtyOutWorkOrderNo", EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim());
            if (EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate == "" || EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@MtyOutWorkOrderDate", DBNull.Value);
                EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("MtyOutWorkOrderDate", Convert.ToDateTime(EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate.Trim()));
                EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate = Convert.ToDateTime(EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@MtyOutContainerNo", EmptyContainerOutTableDataList[0].MtyOutContainerNo.Trim());
            cmd.Parameters.AddWithValue("@MtyOutVehicleNo", EmptyContainerOutTableDataList[0].MtyOutVehicleNo.Trim());
            cmd.Parameters.AddWithValue("@MtyOutModeofGateOut", EmptyContainerOutTableDataList[0].MtyOutModeofGateOut.Trim());
            cmd.Parameters.AddWithValue("@MtyOutCycle", EmptyContainerOutTableDataList[0].MtyOutCycle.Trim());
            cmd.Parameters.AddWithValue("@MtyOutDriverName", EmptyContainerOutTableDataList[0].MtyOutDriverName.Trim());
            cmd.Parameters.AddWithValue("@MtyOutEquipmentCondition", EmptyContainerOutTableDataList[0].MtyOutEquipmentCondition.Trim());
            cmd.Parameters.AddWithValue("@MtyOutContainerTag", EmptyContainerOutTableDataList[0].MtyOutContainerTag.Trim());
            cmd.Parameters.AddWithValue("@MtyOutRemarks", EmptyContainerOutTableDataList[0].MtyOutRemarks.Trim());
            cmd.Parameters.AddWithValue("@MtyOutWorkOrderStatus", EmptyContainerOutTableDataList[0].MtyOutWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@MtyMovementBy", EmptyContainerOutTableDataList[0].MtyMovementBy.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.MtyOutWorkOrderNo != EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutWorkOrderNo", DetailedListValue.MtyOutWorkOrderNo,
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutWorkOrderDate != EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutWorkOrderDate", DetailedListValue.MtyOutWorkOrderDate,
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutContainerNo != EmptyContainerOutTableDataList[0].MtyOutContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutContainerNo", DetailedListValue.MtyOutContainerNo,
                    EmptyContainerOutTableDataList[0].MtyOutContainerNo.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutVehicleNo != EmptyContainerOutTableDataList[0].MtyOutVehicleNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutVehicleNo", DetailedListValue.MtyOutVehicleNo,
                    EmptyContainerOutTableDataList[0].MtyOutVehicleNo.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyMovementBy != EmptyContainerOutTableDataList[0].MtyMovementBy.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyMovementBy", DetailedListValue.MtyMovementBy,
                    EmptyContainerOutTableDataList[0].MtyMovementBy.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutModeofGateOut != EmptyContainerOutTableDataList[0].MtyOutModeofGateOut.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutModeofGateOut", DetailedListValue.MtyOutModeofGateOut,
                    EmptyContainerOutTableDataList[0].MtyOutModeofGateOut.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutCycle != EmptyContainerOutTableDataList[0].MtyOutCycle.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutCycle", DetailedListValue.MtyOutCycle,
                    EmptyContainerOutTableDataList[0].MtyOutCycle.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutDriverName != EmptyContainerOutTableDataList[0].MtyOutDriverName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutDriverName", DetailedListValue.MtyOutDriverName,
                    EmptyContainerOutTableDataList[0].MtyOutDriverName.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutEquipmentCondition != EmptyContainerOutTableDataList[0].MtyOutEquipmentCondition.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutEquipmentCondition", DetailedListValue.MtyOutEquipmentCondition,
                    EmptyContainerOutTableDataList[0].MtyOutEquipmentCondition.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutContainerTag != EmptyContainerOutTableDataList[0].MtyOutContainerTag.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutContainerTag", DetailedListValue.MtyOutContainerTag,
                    EmptyContainerOutTableDataList[0].MtyOutContainerTag.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutRemarks != EmptyContainerOutTableDataList[0].MtyOutRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutRemarks", DetailedListValue.MtyOutRemarks,
                    EmptyContainerOutTableDataList[0].MtyOutRemarks.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MtyOutWorkOrderStatus != EmptyContainerOutTableDataList[0].MtyOutWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportEmptyContainerOut",
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim(), "MtyOutWorkOrderStatus", DetailedListValue.MtyOutWorkOrderStatus,
                    EmptyContainerOutTableDataList[0].MtyOutWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + EmptyContainerOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MtyOutWorkOrderNo = '" + EmptyContainerOutTableDataList[0].MtyOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static EmptyContainerOutTableDataList[] EmptyContainerOutTableSearchData(List<EmptyContainerOutTableDataList> EmptyContainerOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<EmptyContainerOutTableDataList> DetailedList = new List<EmptyContainerOutTableDataList>();
        EmptyContainerOutTableDataList DetailedListValue = new EmptyContainerOutTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select A.*, B.EmptyTorCDriverName from CFSImportEmptyContainerOut A " +
                                "Left Join CFSImportEmptyTruckorContainer B On B.EmptyTorCGateInMode = 'Empty Container Out' And A.MainJobNo = B.MainJobNo And " +
                                "A.MtyOutVehicleNo = B.EmptyTorCTruckNo And A.RecordStatus = B.RecordStatus And B.EmptyTruckorContainerStatus = 'Gated In' " +
                                "Where A.RecordStatus = 'Active' and A.MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", EmptyContainerOutTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new EmptyContainerOutTableDataList();
                DetailedListValue.MtyOutWorkOrderNo = dtrow["MtyOutWorkOrderNo"].ToString();
                DetailedListValue.MtyOutWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["MtyOutWorkOrderDate"]);
                DetailedListValue.MtyOutContainerNo = dtrow["MtyOutContainerNo"].ToString();
                DetailedListValue.MtyOutVehicleNo = dtrow["MtyOutVehicleNo"].ToString();
                DetailedListValue.MtyOutModeofGateOut = dtrow["MtyOutModeofGateOut"].ToString();
                DetailedListValue.MtyOutCycle = dtrow["MtyOutCycle"].ToString();
                if (dtrow["MtyOutDriverName"].ToString() != "" && dtrow["MtyOutDriverName"].ToString() != null) { DetailedListValue.MtyOutDriverName = dtrow["MtyOutDriverName"].ToString(); }
                else { DetailedListValue.MtyOutDriverName = dtrow["EmptyTorCDriverName"].ToString(); }
                DetailedListValue.MtyOutEquipmentCondition = dtrow["MtyOutEquipmentCondition"].ToString();
                DetailedListValue.MtyOutContainerTag = dtrow["MtyOutContainerTag"].ToString();
                DetailedListValue.MtyOutRemarks = dtrow["MtyOutRemarks"].ToString();
                DetailedListValue.MtyOutWorkOrderStatus = dtrow["MtyOutWorkOrderStatus"].ToString(); //Get Driver Name from Empty Truck Gate in, Work Whiel edit in work order
                DetailedListValue.MtyMovementBy = dtrow["MtyMovementBy"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------FCLCargoOutTableInsertDataStartsHere-----------------------------------------------------------------    
    public class FCLCargoOutTableDataList
    {
        public string MainJobNo { get; set; }
        public string FCLOutWorkOrderNo { get; set; }
        public string FCLOutWorkOrderDate { get; set; }
        public string FCLOutContainerNo { get; set; }
        public string FCLOutVehicleNo { get; set; }
        public string FCLOutManifestPackages { get; set; }
        public string FCLOutManifestWeight { get; set; }
        public string FCLOutBalancePackages { get; set; }
        public string FCLOutBalanceWeight { get; set; }
        public string FCLOutDestuffedFrom { get; set; }
        public string FCLOutDestuffedTo { get; set; }
        public string FCLOutDestuffedPkgs { get; set; }
        public string FCLOutDestuffedWeight { get; set; }
        public string FCLOutCustDutyValue { get; set; }
        public string FCLOutStampDutyValue { get; set; }
        public string FCLOutOOCNo { get; set; }
        public string FCLOutOOCDate { get; set; }
        public string FCLOutTallyDetails { get; set; }
        public string FCLOutEquipment { get; set; }
        public string FCLOutVendor { get; set; }
        public string FCLOutRemarks { get; set; }
        public string FCLOutWorkOrderStatus { get; set; }
        public string ReturnedValue { get; set; }
        public string DeStuffWorkOrderNo { get; set; }
    }
    //-----------------------------------------------------------FCLCargoOutTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string FCLCargoOutTableUpdateData(List<FCLCargoOutTableDataList> FCLCargoOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<FCLCargoOutTableDataList> DetailedList = new List<FCLCargoOutTableDataList>();
        FCLCargoOutTableDataList DetailedListValue = new FCLCargoOutTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportFCLCargoOut WHERE MainJobNo = @MainJobNo and FCLOutWorkOrderNo = @FCLOutWorkOrderNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", FCLCargoOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@FCLOutWorkOrderNo", FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.FCLOutWorkOrderNo = dtrow["FCLOutWorkOrderNo"].ToString();
                DetailedListValue.FCLOutWorkOrderDate = dtrow["FCLOutWorkOrderDate"].ToString();
                if (dtrow["FCLOutWorkOrderDate"].ToString() != "" || dtrow["FCLOutWorkOrderDate"] != DBNull.Value) { DetailedListValue.FCLOutWorkOrderDate = Convert.ToDateTime(dtrow["FCLOutWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.FCLOutWorkOrderDate = ""; }
                DetailedListValue.FCLOutContainerNo = dtrow["FCLOutContainerNo"].ToString();
                DetailedListValue.FCLOutVehicleNo = dtrow["FCLOutVehicleNo"].ToString();
                DetailedListValue.FCLOutManifestPackages = dtrow["FCLOutManifestPackages"].ToString();
                DetailedListValue.FCLOutManifestWeight = dtrow["FCLOutManifestWeight"].ToString();
                DetailedListValue.FCLOutBalancePackages = dtrow["FCLOutBalancePackages"].ToString();
                DetailedListValue.FCLOutBalanceWeight = dtrow["FCLOutBalanceWeight"].ToString();
                DetailedListValue.FCLOutDestuffedFrom = dtrow["FCLOutDestuffedFrom"].ToString();
                if (dtrow["FCLOutDestuffedFrom"].ToString() != "" || dtrow["FCLOutDestuffedFrom"] != DBNull.Value) { DetailedListValue.FCLOutDestuffedFrom = Convert.ToDateTime(dtrow["FCLOutDestuffedFrom"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.FCLOutDestuffedFrom = ""; }
                DetailedListValue.FCLOutDestuffedTo = dtrow["FCLOutDestuffedTo"].ToString();
                if (dtrow["FCLOutDestuffedTo"].ToString() != "" || dtrow["FCLOutDestuffedTo"] != DBNull.Value) { DetailedListValue.FCLOutDestuffedTo = Convert.ToDateTime(dtrow["FCLOutDestuffedTo"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.FCLOutDestuffedTo = ""; }
                DetailedListValue.FCLOutDestuffedPkgs = dtrow["FCLOutDestuffedPkgs"].ToString();
                DetailedListValue.FCLOutDestuffedWeight = dtrow["FCLOutDestuffedWeight"].ToString();
                DetailedListValue.FCLOutCustDutyValue = dtrow["FCLOutCustDutyValue"].ToString();
                DetailedListValue.FCLOutStampDutyValue = dtrow["FCLOutStampDutyValue"].ToString();
                DetailedListValue.FCLOutOOCNo = dtrow["FCLOutOOCNo"].ToString();
                DetailedListValue.FCLOutOOCDate = dtrow["FCLOutOOCDate"].ToString();
                if (dtrow["FCLOutOOCDate"].ToString() != "" || dtrow["FCLOutOOCDate"] != DBNull.Value) { DetailedListValue.FCLOutOOCDate = Convert.ToDateTime(dtrow["FCLOutOOCDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.FCLOutOOCDate = ""; }
                DetailedListValue.FCLOutTallyDetails = dtrow["FCLOutTallyDetails"].ToString();
                DetailedListValue.FCLOutEquipment = dtrow["FCLOutEquipment"].ToString();
                DetailedListValue.FCLOutVendor = dtrow["FCLOutVendor"].ToString();
                DetailedListValue.FCLOutRemarks = dtrow["FCLOutRemarks"].ToString();
                DetailedListValue.FCLOutWorkOrderStatus = dtrow["FCLOutWorkOrderStatus"].ToString();
            }
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportFCLCargoOut set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, FCLOutWorkOrderNo=@FCLOutWorkOrderNo,FCLOutWorkOrderDate=@FCLOutWorkOrderDate,FCLOutContainerNo=@FCLOutContainerNo,FCLOutVehicleNo=@FCLOutVehicleNo,FCLOutManifestPackages=@FCLOutManifestPackages,FCLOutManifestWeight=@FCLOutManifestWeight,FCLOutBalancePackages=@FCLOutBalancePackages,FCLOutBalanceWeight=@FCLOutBalanceWeight,FCLOutDestuffedFrom=@FCLOutDestuffedFrom,FCLOutDestuffedTo=@FCLOutDestuffedTo,FCLOutDestuffedPkgs=@FCLOutDestuffedPkgs,FCLOutDestuffedWeight=@FCLOutDestuffedWeight,FCLOutCustDutyValue=@FCLOutCustDutyValue,FCLOutStampDutyValue=@FCLOutStampDutyValue,FCLOutOOCNo=@FCLOutOOCNo,FCLOutOOCDate=@FCLOutOOCDate,FCLOutTallyDetails=@FCLOutTallyDetails,FCLOutEquipment=@FCLOutEquipment,FCLOutVendor=@FCLOutVendor,FCLOutRemarks=@FCLOutRemarks,FCLOutWorkOrderStatus=@FCLOutWorkOrderStatus where FCLOutWorkOrderNo = @FCLOutWorkOrderNo And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", FCLCargoOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@FCLOutWorkOrderNo", FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim());
            if (FCLCargoOutTableDataList[0].FCLOutWorkOrderDate == "" || FCLCargoOutTableDataList[0].FCLOutWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@FCLOutWorkOrderDate", DBNull.Value);
                FCLCargoOutTableDataList[0].FCLOutWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("FCLOutWorkOrderDate", Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutWorkOrderDate.Trim()));
                FCLCargoOutTableDataList[0].FCLOutWorkOrderDate = Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@FCLOutContainerNo", FCLCargoOutTableDataList[0].FCLOutContainerNo.Trim());
            cmd.Parameters.AddWithValue("@FCLOutVehicleNo", FCLCargoOutTableDataList[0].FCLOutVehicleNo.Trim());
            cmd.Parameters.AddWithValue("@FCLOutManifestPackages", FCLCargoOutTableDataList[0].FCLOutManifestPackages.Trim());
            cmd.Parameters.AddWithValue("@FCLOutManifestWeight", FCLCargoOutTableDataList[0].FCLOutManifestWeight.Trim());
            cmd.Parameters.AddWithValue("@FCLOutBalancePackages", FCLCargoOutTableDataList[0].FCLOutBalancePackages.Trim());
            cmd.Parameters.AddWithValue("@FCLOutBalanceWeight", FCLCargoOutTableDataList[0].FCLOutBalanceWeight.Trim());
            if (FCLCargoOutTableDataList[0].FCLOutDestuffedFrom == "" || FCLCargoOutTableDataList[0].FCLOutDestuffedFrom == null)
            {
                cmd.Parameters.AddWithValue("@FCLOutDestuffedFrom", DBNull.Value);
                FCLCargoOutTableDataList[0].FCLOutDestuffedFrom = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("FCLOutDestuffedFrom", Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutDestuffedFrom.Trim()));
                FCLCargoOutTableDataList[0].FCLOutDestuffedFrom = Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutDestuffedFrom).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (FCLCargoOutTableDataList[0].FCLOutDestuffedTo == "" || FCLCargoOutTableDataList[0].FCLOutDestuffedTo == null)
            {
                cmd.Parameters.AddWithValue("@FCLOutDestuffedTo", DBNull.Value);
                FCLCargoOutTableDataList[0].FCLOutDestuffedTo = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("FCLOutDestuffedTo", Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutDestuffedTo.Trim()));
                FCLCargoOutTableDataList[0].FCLOutDestuffedTo = Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutDestuffedTo).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@FCLOutDestuffedPkgs", FCLCargoOutTableDataList[0].FCLOutDestuffedPkgs.Trim());
            cmd.Parameters.AddWithValue("@FCLOutDestuffedWeight", FCLCargoOutTableDataList[0].FCLOutDestuffedWeight.Trim());
            cmd.Parameters.AddWithValue("@FCLOutCustDutyValue", FCLCargoOutTableDataList[0].FCLOutCustDutyValue.Trim());
            cmd.Parameters.AddWithValue("@FCLOutStampDutyValue", FCLCargoOutTableDataList[0].FCLOutStampDutyValue.Trim());
            cmd.Parameters.AddWithValue("@FCLOutOOCNo", FCLCargoOutTableDataList[0].FCLOutOOCNo.Trim());
            if (FCLCargoOutTableDataList[0].FCLOutOOCDate == "" || FCLCargoOutTableDataList[0].FCLOutOOCDate == null)
            {
                cmd.Parameters.AddWithValue("@FCLOutOOCDate", DBNull.Value);
                FCLCargoOutTableDataList[0].FCLOutOOCDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("FCLOutOOCDate", Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutOOCDate.Trim()).Date);
                FCLCargoOutTableDataList[0].FCLOutOOCDate = Convert.ToDateTime(FCLCargoOutTableDataList[0].FCLOutOOCDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@FCLOutTallyDetails", FCLCargoOutTableDataList[0].FCLOutTallyDetails.Trim());
            cmd.Parameters.AddWithValue("@FCLOutEquipment", FCLCargoOutTableDataList[0].FCLOutEquipment.Trim());
            cmd.Parameters.AddWithValue("@FCLOutVendor", FCLCargoOutTableDataList[0].FCLOutVendor.Trim());
            cmd.Parameters.AddWithValue("@FCLOutRemarks", FCLCargoOutTableDataList[0].FCLOutRemarks.Trim());
            cmd.Parameters.AddWithValue("@FCLOutWorkOrderStatus", FCLCargoOutTableDataList[0].FCLOutWorkOrderStatus.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            cmd = new SqlCommand("Update CFSImportDeStuffing Set FCLCargoOut = 'Gated Out' Where MainJobNo = @MainJobNo And DeStuffContainerNo = @ContainerNo And " +
                                 "RecordStatus = 'Active' And DeStuffWorkOrderNo = @WorkOrderNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", FCLCargoOutTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ContainerNo", FCLCargoOutTableDataList[0].FCLOutContainerNo.Trim());
            cmd.Parameters.AddWithValue("@WorkOrderNo", FCLCargoOutTableDataList[0].DeStuffWorkOrderNo.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.FCLOutWorkOrderNo != FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutWorkOrderNo", DetailedListValue.FCLOutWorkOrderNo,
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutWorkOrderDate != FCLCargoOutTableDataList[0].FCLOutWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutWorkOrderDate", DetailedListValue.FCLOutWorkOrderDate,
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutContainerNo != FCLCargoOutTableDataList[0].FCLOutContainerNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutContainerNo", DetailedListValue.FCLOutContainerNo,
                    FCLCargoOutTableDataList[0].FCLOutContainerNo.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutVehicleNo != FCLCargoOutTableDataList[0].FCLOutVehicleNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutVehicleNo", DetailedListValue.FCLOutVehicleNo,
                    FCLCargoOutTableDataList[0].FCLOutVehicleNo.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutManifestPackages != FCLCargoOutTableDataList[0].FCLOutManifestPackages.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutManifestPackages", DetailedListValue.FCLOutManifestPackages,
                    FCLCargoOutTableDataList[0].FCLOutManifestPackages.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutManifestWeight != FCLCargoOutTableDataList[0].FCLOutManifestWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutManifestWeight", DetailedListValue.FCLOutManifestWeight,
                    FCLCargoOutTableDataList[0].FCLOutManifestWeight.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutBalancePackages != FCLCargoOutTableDataList[0].FCLOutBalancePackages.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutBalancePackages", DetailedListValue.FCLOutBalancePackages,
                    FCLCargoOutTableDataList[0].FCLOutBalancePackages.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutBalanceWeight != FCLCargoOutTableDataList[0].FCLOutBalanceWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutBalanceWeight", DetailedListValue.FCLOutBalanceWeight,
                    FCLCargoOutTableDataList[0].FCLOutBalanceWeight.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutDestuffedFrom != FCLCargoOutTableDataList[0].FCLOutDestuffedFrom.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutDestuffedFrom", DetailedListValue.FCLOutDestuffedFrom,
                    FCLCargoOutTableDataList[0].FCLOutDestuffedFrom.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutDestuffedTo != FCLCargoOutTableDataList[0].FCLOutDestuffedTo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutDestuffedTo", DetailedListValue.FCLOutDestuffedTo,
                    FCLCargoOutTableDataList[0].FCLOutDestuffedTo.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutDestuffedPkgs != FCLCargoOutTableDataList[0].FCLOutDestuffedPkgs.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutDestuffedPkgs", DetailedListValue.FCLOutDestuffedPkgs,
                    FCLCargoOutTableDataList[0].FCLOutDestuffedPkgs.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutDestuffedWeight != FCLCargoOutTableDataList[0].FCLOutDestuffedWeight.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutDestuffedWeight", DetailedListValue.FCLOutDestuffedWeight,
                    FCLCargoOutTableDataList[0].FCLOutDestuffedWeight.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutCustDutyValue != FCLCargoOutTableDataList[0].FCLOutCustDutyValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutCustDutyValue", DetailedListValue.FCLOutCustDutyValue,
                    FCLCargoOutTableDataList[0].FCLOutCustDutyValue.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutStampDutyValue != FCLCargoOutTableDataList[0].FCLOutStampDutyValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutStampDutyValue", DetailedListValue.FCLOutStampDutyValue,
                    FCLCargoOutTableDataList[0].FCLOutStampDutyValue.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutOOCNo != FCLCargoOutTableDataList[0].FCLOutOOCNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutOOCNo", DetailedListValue.FCLOutOOCNo,
                    FCLCargoOutTableDataList[0].FCLOutOOCNo.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutOOCDate != FCLCargoOutTableDataList[0].FCLOutOOCDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutOOCDate", DetailedListValue.FCLOutOOCDate,
                    FCLCargoOutTableDataList[0].FCLOutOOCDate.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutTallyDetails != FCLCargoOutTableDataList[0].FCLOutTallyDetails.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutTallyDetails", DetailedListValue.FCLOutTallyDetails,
                    FCLCargoOutTableDataList[0].FCLOutTallyDetails.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutEquipment != FCLCargoOutTableDataList[0].FCLOutEquipment.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutEquipment", DetailedListValue.FCLOutEquipment,
                    FCLCargoOutTableDataList[0].FCLOutEquipment.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutVendor != FCLCargoOutTableDataList[0].FCLOutVendor.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutVendor", DetailedListValue.FCLOutVendor,
                    FCLCargoOutTableDataList[0].FCLOutVendor.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutRemarks != FCLCargoOutTableDataList[0].FCLOutRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutRemarks", DetailedListValue.FCLOutRemarks,
                    FCLCargoOutTableDataList[0].FCLOutRemarks.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.FCLOutWorkOrderStatus != FCLCargoOutTableDataList[0].FCLOutWorkOrderStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportFCLCargoOut",
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim(), "FCLOutWorkOrderStatus", DetailedListValue.FCLOutWorkOrderStatus,
                    FCLCargoOutTableDataList[0].FCLOutWorkOrderStatus.Trim(), "Modified", "MainJobNo = '" + FCLCargoOutTableDataList[0].MainJobNo.Trim() + "' And " +
                    "FCLOutWorkOrderNo = '" + FCLCargoOutTableDataList[0].FCLOutWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static FCLCargoOutTableDataList[] FCLCargoOutTableSearchData(List<FCLCargoOutTableDataList> FCLCargoOutTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<FCLCargoOutTableDataList> DetailedList = new List<FCLCargoOutTableDataList>();
        FCLCargoOutTableDataList DetailedListValue = new FCLCargoOutTableDataList();
        try
        {
            cmd = new SqlCommand(@"Select Distinct A.CompanyName, A.BranchName, A.MainJobNo, A.FCLCargoOutId, A.FCLOutWorkOrderNo, A.FCLOutWorkOrderDate, A.FCLOutContainerNo, 
                                A.FCLOutVehicleNo,
                                A.FCLOutBalancePackages, A.FCLOutBalanceWeight, A.FCLOutCustDutyValue, A.FCLOutStampDutyValue, A.FCLOutOOCNo, A.FCLOutOOCDate, A.FCLOutTallyDetails, 
                                A.FCLOutEquipment, A.FCLOutVendor, A.FCLOutRemarks, A.FCLOutWorkOrderStatus, A.FCLCargoOutStatus, A.RecordStatus, A.UpdatedBy, A.ComputerName, 
                                A.IPAddress, A.Location, A.UpdatedOn,
                                Case When A.FCLOutManifestPackages = '0' Or A.FCLOutManifestPackages = Null Or A.FCLOutManifestPackages = '' Then B.DeStuffDeclaredPkgs Else 
                                A.FCLOutManifestPackages End As [FCLOutManifestPackages],

                                Case When A.FCLOutManifestWeight = '0' Or A.FCLOutManifestWeight = Null Or A.FCLOutManifestWeight = '' Then B.DeStuffDeclaredWeight Else 
                                A.FCLOutManifestWeight End As [FCLOutManifestWeight],

                                Case When A.FCLOutDestuffedFrom Is Not Null And A.FCLOutDestuffedFrom <> '' Then A.FCLOutDestuffedFrom Else 
                                B.DeStuffFromDate End As [FCLOutDestuffedFrom],

                                Case When A.FCLOutDestuffedTo Is Not Null And A.FCLOutDestuffedTo <> '' Then A.FCLOutDestuffedTo Else 
                                B.DeStuffToDate End As [FCLOutDestuffedTo],

                                Case When A.FCLOutDestuffedPkgs = '0' Or A.FCLOutDestuffedPkgs = Null Or A.FCLOutDestuffedPkgs = '' Then B.DeStuffDeStuffedPkgs Else 
                                A.FCLOutDestuffedPkgs End As [FCLOutDestuffedPkgs],

                                Case When A.FCLOutDestuffedWeight = '0' Or A.FCLOutDestuffedWeight = Null Or A.FCLOutDestuffedWeight = '' Then B.DeStuffDeStuffedWeight Else 
                                A.FCLOutDestuffedWeight End As [FCLOutDestuffedWeight],

                                Case When A.FCLOutWorkOrderStatus = 'Pending' Then B.DeStuffWorkOrderNo Else '' End As [DeStuffWorkOrderNo]

                                From CFSImportFCLCargoOut A 
                                Left Join CFSImportDeStuffing B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.FCLOutContainerNo = B.DeStuffContainerNo And 
                                B.DeStuffWorkOrderStatus = 'Completed' And B.FCLCargoOut = 'Not Gated Out' Where A.RecordStatus = 'Active' And A.MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", FCLCargoOutTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new FCLCargoOutTableDataList();
                DetailedListValue.FCLOutWorkOrderNo = dtrow["FCLOutWorkOrderNo"].ToString();
                DetailedListValue.FCLOutWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["FCLOutWorkOrderDate"]);
                DetailedListValue.FCLOutContainerNo = dtrow["FCLOutContainerNo"].ToString();
                DetailedListValue.FCLOutVehicleNo = dtrow["FCLOutVehicleNo"].ToString();
                DetailedListValue.FCLOutBalancePackages = dtrow["FCLOutBalancePackages"].ToString();
                DetailedListValue.FCLOutBalanceWeight = dtrow["FCLOutBalanceWeight"].ToString();
                DetailedListValue.FCLOutManifestPackages = dtrow["FCLOutManifestPackages"].ToString();
                DetailedListValue.FCLOutManifestWeight = dtrow["FCLOutManifestWeight"].ToString();
                DetailedListValue.FCLOutDestuffedFrom = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["FCLOutDestuffedFrom"]);
                DetailedListValue.FCLOutDestuffedTo = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["FCLOutDestuffedTo"]);
                DetailedListValue.FCLOutDestuffedPkgs = dtrow["FCLOutDestuffedPkgs"].ToString();
                DetailedListValue.FCLOutDestuffedWeight = dtrow["FCLOutDestuffedWeight"].ToString();
                DetailedListValue.FCLOutCustDutyValue = dtrow["FCLOutCustDutyValue"].ToString();
                DetailedListValue.FCLOutStampDutyValue = dtrow["FCLOutStampDutyValue"].ToString();
                DetailedListValue.FCLOutOOCNo = dtrow["FCLOutOOCNo"].ToString();
                DetailedListValue.FCLOutOOCDate = String.Format("{0:yyyy-MM-dd}", dtrow["FCLOutOOCDate"]);
                DetailedListValue.FCLOutTallyDetails = dtrow["FCLOutTallyDetails"].ToString();
                DetailedListValue.FCLOutEquipment = dtrow["FCLOutEquipment"].ToString();
                DetailedListValue.FCLOutVendor = dtrow["FCLOutVendor"].ToString();
                DetailedListValue.FCLOutRemarks = dtrow["FCLOutRemarks"].ToString();
                DetailedListValue.FCLOutWorkOrderStatus = dtrow["FCLOutWorkOrderStatus"].ToString();
                DetailedListValue.DeStuffWorkOrderNo = dtrow["DeStuffWorkOrderNo"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------ScopeTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ScopeTableInsertData(List<ScopeTableDataList> ScopeTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Select Count(*) From CFSImportScope Where MainJobNo = @MainJobNo And ScopeName = @ScopeId And BLNo = @BLNo And RecordStatus = 'Active';");
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ScopeId", ScopeTableDataList[0].ScopeId.Trim());
            cmd.Parameters.AddWithValue("@BLNo", ScopeTableDataList[0].BLNo.Trim());
            cmd.Connection = cnn;
            cnn.Open();
            int DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
            cnn.Close();

            if (DupCount > 0) { return "Duplicate Entry !!!"; }

            cmd = new SqlCommand("Insert into CFSImportScope(CompanyName,BranchName,MainJobNo,ScopeWorkOrderNo,ScopeWorkOrderDate,ScopeName,ScopeDescription,ScopeAgreedCostwithCustomer,ScopeActualCost,ScopeStatus,ScopeRemarks,UpdatedBy,BLNo) " +
                                 "Values(@CompanyName,@BranchName,@MainJobNo,@ScopeWorkOrderNo,@ScopeWorkOrderDate,@ScopeId,@ScopeDescription,@ScopeAgreedCostwithCustomer,@ScopeActualCost,@ScopeStatus,@ScopeRemarks,@UpdatedBy,@BLNo)", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());

            if (ScopeTableDataList[0].ScopeWorkOrderNo.Trim() == "" || ScopeTableDataList[0].ScopeWorkOrderNo.Trim() == null)
            {
                SqlCommand cmmd = new SqlCommand("Select Case When Count(*) = 0 then 1 else Count(*) + 1 end as [WOCount] From CFSImportScope Where RecordStatus = 'Active'");
                cmmd.Connection = cnn;
                cnn.Open();
                int WOCount = Convert.ToInt32(cmmd.ExecuteScalar().GetHashCode());
                cnn.Close();

                ScopeTableDataList[0].ScopeWorkOrderNo = "SP_WORK_ORDER_" + WOCount;
            }

            cmd.Parameters.AddWithValue("@ScopeWorkOrderNo", ScopeTableDataList[0].ScopeWorkOrderNo.Trim());
            if (ScopeTableDataList[0].ScopeWorkOrderDate == "" || ScopeTableDataList[0].ScopeWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@ScopeWorkOrderDate", DateTime.Now);
            }
            else { cmd.Parameters.AddWithValue("@ScopeWorkOrderDate", Convert.ToDateTime(ScopeTableDataList[0].ScopeWorkOrderDate.Trim())); }
            cmd.Parameters.AddWithValue("@ScopeId", ScopeTableDataList[0].ScopeId.Trim());
            cmd.Parameters.AddWithValue("@ScopeDescription", ScopeTableDataList[0].ScopeDescription.Trim());
            cmd.Parameters.AddWithValue("@ScopeAgreedCostwithCustomer", ScopeTableDataList[0].ScopeAgreedCostwithCustomer.Trim());
            cmd.Parameters.AddWithValue("@ScopeActualCost", ScopeTableDataList[0].ScopeActualCost.Trim());
            cmd.Parameters.AddWithValue("@ScopeStatus", ScopeTableDataList[0].ScopeStatus.Trim());
            cmd.Parameters.AddWithValue("@ScopeRemarks", ScopeTableDataList[0].ScopeRemarks.Trim());
            cmd.Parameters.AddWithValue("@BLNo", ScopeTableDataList[0].BLNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["Company"].ToString());
            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["Branch"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportScope", ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class ScopeTableDataList
    {
        public string MainJobNo { get; set; }
        public string ScopeWorkOrderNo { get; set; }
        public string ScopeWorkOrderDate { get; set; }
        public string ScopeId { get; set; }
        public string ScopeDescription { get; set; }
        public string ScopeAgreedCostwithCustomer { get; set; }
        public string ScopeActualCost { get; set; }
        public string ScopeStatus { get; set; }
        public string ScopeRemarks { get; set; }
        public string BLNo { get; set; }
        public string ScpId { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------ScopeTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ScopeTableUpdateData(List<ScopeTableDataList> ScopeTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<ScopeTableDataList> DetailedList = new List<ScopeTableDataList>();
        ScopeTableDataList DetailedListValue = new ScopeTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportScope WHERE MainJobNo = @MainJobNo and ScopeWorkOrderNo = @ScopeWorkOrderNo And ScopeId = @ScpId and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ScopeWorkOrderNo", ScopeTableDataList[0].ScopeWorkOrderNo.Trim());
            cmd.Parameters.AddWithValue("@ScpId", ScopeTableDataList[0].ScpId.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.ScopeWorkOrderNo = dtrow["ScopeWorkOrderNo"].ToString();
                DetailedListValue.ScopeWorkOrderDate = dtrow["ScopeWorkOrderDate"].ToString();
                if (dtrow["ScopeWorkOrderDate"].ToString() != "" || dtrow["ScopeWorkOrderDate"] != DBNull.Value) { DetailedListValue.ScopeWorkOrderDate = Convert.ToDateTime(dtrow["ScopeWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.ScopeWorkOrderDate = ""; }
                DetailedListValue.ScopeId = dtrow["ScopeName"].ToString();
                DetailedListValue.BLNo = dtrow["BLNo"].ToString();
                DetailedListValue.ScopeDescription = dtrow["ScopeDescription"].ToString();
                DetailedListValue.ScopeAgreedCostwithCustomer = dtrow["ScopeAgreedCostwithCustomer"].ToString();
                DetailedListValue.ScopeActualCost = dtrow["ScopeActualCost"].ToString();
                DetailedListValue.ScopeStatus = dtrow["ScopeStatus"].ToString();
                DetailedListValue.ScopeRemarks = dtrow["ScopeRemarks"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportScope set CompanyName=@CompanyName,BranchName=@BranchName,BLNo=@BLNo,ScopeWorkOrderNo=@ScopeWorkOrderNo,ScopeWorkOrderDate=@ScopeWorkOrderDate,ScopeName=@ScopeId,ScopeDescription=@ScopeDescription,ScopeAgreedCostwithCustomer=@ScopeAgreedCostwithCustomer,ScopeActualCost=@ScopeActualCost,ScopeStatus=@ScopeStatus,ScopeRemarks=@ScopeRemarks " +
                "where ScopeWorkOrderNo = @ScopeWorkOrderNo And MainJobNo = @MainJobNo And ScopeId = @ScpId And RecordStatus = 'Active';", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ScopeWorkOrderNo", ScopeTableDataList[0].ScopeWorkOrderNo.Trim());
            cmd.Parameters.AddWithValue("@ScpId", ScopeTableDataList[0].ScpId.Trim());
            cmd.Parameters.AddWithValue("@BLNo", ScopeTableDataList[0].BLNo.Trim());
            if (ScopeTableDataList[0].ScopeWorkOrderDate == "" || ScopeTableDataList[0].ScopeWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@ScopeWorkOrderDate", DBNull.Value);
                ScopeTableDataList[0].ScopeWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("ScopeWorkOrderDate", Convert.ToDateTime(ScopeTableDataList[0].ScopeWorkOrderDate.Trim()));
                ScopeTableDataList[0].ScopeWorkOrderDate = Convert.ToDateTime(ScopeTableDataList[0].ScopeWorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@ScopeId", ScopeTableDataList[0].ScopeId.Trim());
            cmd.Parameters.AddWithValue("@ScopeDescription", ScopeTableDataList[0].ScopeDescription.Trim());
            cmd.Parameters.AddWithValue("@ScopeAgreedCostwithCustomer", ScopeTableDataList[0].ScopeAgreedCostwithCustomer.Trim());
            cmd.Parameters.AddWithValue("@ScopeActualCost", ScopeTableDataList[0].ScopeActualCost.Trim());
            cmd.Parameters.AddWithValue("@ScopeStatus", ScopeTableDataList[0].ScopeStatus.Trim());
            cmd.Parameters.AddWithValue("@ScopeRemarks", ScopeTableDataList[0].ScopeRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["Company"].ToString());
            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["Branch"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.ScopeWorkOrderNo != ScopeTableDataList[0].ScopeWorkOrderNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeWorkOrderNo", DetailedListValue.ScopeWorkOrderNo,
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeWorkOrderDate != ScopeTableDataList[0].ScopeWorkOrderDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeWorkOrderDate", DetailedListValue.ScopeWorkOrderDate,
                    ScopeTableDataList[0].ScopeWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeId != ScopeTableDataList[0].ScopeId.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeId", DetailedListValue.ScopeId,
                    ScopeTableDataList[0].ScopeId.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeDescription != ScopeTableDataList[0].ScopeDescription.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeDescription", DetailedListValue.ScopeDescription,
                    ScopeTableDataList[0].ScopeDescription.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.BLNo != ScopeTableDataList[0].BLNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "BLNo", DetailedListValue.BLNo,
                    ScopeTableDataList[0].BLNo.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeAgreedCostwithCustomer != ScopeTableDataList[0].ScopeAgreedCostwithCustomer.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeAgreedCostwithCustomer", DetailedListValue.ScopeAgreedCostwithCustomer,
                    ScopeTableDataList[0].ScopeAgreedCostwithCustomer.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeActualCost != ScopeTableDataList[0].ScopeActualCost.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeActualCost", DetailedListValue.ScopeActualCost,
                    ScopeTableDataList[0].ScopeActualCost.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeStatus != ScopeTableDataList[0].ScopeStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeStatus", DetailedListValue.ScopeStatus,
                    ScopeTableDataList[0].ScopeStatus.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.ScopeRemarks != ScopeTableDataList[0].ScopeRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportScope",
                    ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "ScopeRemarks", DetailedListValue.ScopeRemarks,
                    ScopeTableDataList[0].ScopeRemarks.Trim(), "Modified", "MainJobNo = '" + ScopeTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ScopeWorkOrderNo = '" + ScopeTableDataList[0].ScopeWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------ScopeTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ScopeTableCancelData(List<ScopeTableDataList> ScopeTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportScope set RecordStatus = 'Cancelled' where ScopeWorkOrderNo = @ScopeWorkOrderNo And MainJobNo = @MainJobNo And ScopeId = @ScpId;", cnn);
            cmd.Parameters.AddWithValue("@ScopeWorkOrderNo", ScopeTableDataList[0].ScopeWorkOrderNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ScpId", ScopeTableDataList[0].ScpId.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportScope", ScopeTableDataList[0].ScopeWorkOrderNo.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static ScopeTableDataList[] ScopeTableSearchData(List<ScopeTableDataList> ScopeTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<ScopeTableDataList> DetailedList = new List<ScopeTableDataList>();
        ScopeTableDataList DetailedListValue = new ScopeTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportScope Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", ScopeTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new ScopeTableDataList();
                DetailedListValue.ScpId = dtrow["ScopeId"].ToString();
                DetailedListValue.ScopeWorkOrderNo = dtrow["ScopeWorkOrderNo"].ToString();
                DetailedListValue.ScopeWorkOrderDate = String.Format("{0:yyyy-MM-dd}", dtrow["ScopeWorkOrderDate"]);
                DetailedListValue.ScopeId = dtrow["ScopeName"].ToString();
                DetailedListValue.BLNo = dtrow["BLNo"].ToString();
                DetailedListValue.ScopeDescription = dtrow["ScopeDescription"].ToString();
                DetailedListValue.ScopeAgreedCostwithCustomer = dtrow["ScopeAgreedCostwithCustomer"].ToString();
                DetailedListValue.ScopeActualCost = dtrow["ScopeActualCost"].ToString();
                DetailedListValue.ScopeStatus = dtrow["ScopeStatus"].ToString();
                DetailedListValue.ScopeRemarks = dtrow["ScopeRemarks"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------RevenueTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string RevenueTableInsertData(List<RevenueTableDataList> RevenueTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Insert into CFSImportRevenue(MainJobNo,RevenueChargeHead,RevenueChargeDescription,RevenueCostCenter,RevenueCustomerCurrency,RevenueCompanyCurrency,RevenueExchangeRate,RevenueHSNOrSACCode,RevenueQty,RevenueUOM,RevenueUnitPrice,RevenueGSTPercentage,RevenueGSTAmt,RevenueTotalPrice,RevenueReceivableType,RevenueInvoiceNo,RevenueInvoiceDate,RevenueDueDate)   Values(@MainJobNo,@RevenueChargeHead,@RevenueChargeDescription,@RevenueCostCenter,@RevenueCustomerCurrency,@RevenueCompanyCurrency,@RevenueExchangeRate,@RevenueHSNOrSACCode,@RevenueQty,@RevenueUOM,@RevenueUnitPrice,@RevenueGSTPercentage,@RevenueGSTAmt,@RevenueTotalPrice,@RevenueReceivableType,@RevenueInvoiceNo,@RevenueInvoiceDate,@RevenueDueDate)", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", RevenueTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@RevenueChargeHead", RevenueTableDataList[0].RevenueChargeHead.Trim());
            cmd.Parameters.AddWithValue("@RevenueChargeDescription", RevenueTableDataList[0].RevenueChargeDescription.Trim());
            cmd.Parameters.AddWithValue("@RevenueCostCenter", RevenueTableDataList[0].RevenueCostCenter.Trim());
            cmd.Parameters.AddWithValue("@RevenueCustomerCurrency", RevenueTableDataList[0].RevenueCustomerCurrency.Trim());
            cmd.Parameters.AddWithValue("@RevenueCompanyCurrency", RevenueTableDataList[0].RevenueCompanyCurrency.Trim());
            cmd.Parameters.AddWithValue("@RevenueExchangeRate", RevenueTableDataList[0].RevenueExchangeRate.Trim());
            cmd.Parameters.AddWithValue("@RevenueHSNOrSACCode", RevenueTableDataList[0].RevenueHSNOrSACCode.Trim());
            cmd.Parameters.AddWithValue("@RevenueQty", RevenueTableDataList[0].RevenueQty.Trim());
            cmd.Parameters.AddWithValue("@RevenueUOM", RevenueTableDataList[0].RevenueUOM.Trim());
            cmd.Parameters.AddWithValue("@RevenueUnitPrice", RevenueTableDataList[0].RevenueUnitPrice.Trim());
            cmd.Parameters.AddWithValue("@RevenueGSTPercentage", RevenueTableDataList[0].RevenueGSTPercentage.Trim());
            cmd.Parameters.AddWithValue("@RevenueGSTAmt", RevenueTableDataList[0].RevenueGSTAmt.Trim());
            cmd.Parameters.AddWithValue("@RevenueTotalPrice", RevenueTableDataList[0].RevenueTotalPrice.Trim());
            cmd.Parameters.AddWithValue("@RevenueReceivableType", RevenueTableDataList[0].RevenueReceivableType.Trim());
            cmd.Parameters.AddWithValue("@RevenueInvoiceNo", RevenueTableDataList[0].RevenueInvoiceNo.Trim());
            if (RevenueTableDataList[0].RevenueInvoiceDate == "" || RevenueTableDataList[0].RevenueInvoiceDate == null) { cmd.Parameters.AddWithValue("@RevenueInvoiceDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@RevenueInvoiceDate", Convert.ToDateTime(RevenueTableDataList[0].RevenueInvoiceDate.Trim()).Date); }
            if (RevenueTableDataList[0].RevenueDueDate == "" || RevenueTableDataList[0].RevenueDueDate == null) { cmd.Parameters.AddWithValue("@RevenueDueDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@RevenueDueDate", Convert.ToDateTime(RevenueTableDataList[0].RevenueDueDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportRevenue", RevenueTableDataList[0].RevenueChargeHead.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class RevenueTableDataList
    {
        public string MainJobNo { get; set; }
        public string RevenueChargeHead { get; set; }
        public string RevenueChargeDescription { get; set; }
        public string RevenueCostCenter { get; set; }
        public string RevenueCustomerCurrency { get; set; }
        public string RevenueCompanyCurrency { get; set; }
        public string RevenueExchangeRate { get; set; }
        public string RevenueHSNOrSACCode { get; set; }
        public string RevenueQty { get; set; }
        public string RevenueUOM { get; set; }
        public string RevenueUnitPrice { get; set; }
        public string RevenueGSTPercentage { get; set; }
        public string RevenueGSTAmt { get; set; }
        public string RevenueTotalPrice { get; set; }
        public string RevenueReceivableType { get; set; }
        public string RevenueInvoiceNo { get; set; }
        public string RevenueInvoiceDate { get; set; }
        public string RevenueDueDate { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------RevenueTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string RevenueTableUpdateData(List<RevenueTableDataList> RevenueTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<RevenueTableDataList> DetailedList = new List<RevenueTableDataList>();
        RevenueTableDataList DetailedListValue = new RevenueTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportRevenue WHERE MainJobNo = @MainJobNo and RevenueChargeHead = @RevenueChargeHead and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", RevenueTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@RevenueChargeHead", RevenueTableDataList[0].RevenueChargeHead.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.RevenueChargeHead = dtrow["RevenueChargeHead"].ToString();
                DetailedListValue.RevenueChargeDescription = dtrow["RevenueChargeDescription"].ToString();
                DetailedListValue.RevenueCostCenter = dtrow["RevenueCostCenter"].ToString();
                DetailedListValue.RevenueCustomerCurrency = dtrow["RevenueCustomerCurrency"].ToString();
                DetailedListValue.RevenueCompanyCurrency = dtrow["RevenueCompanyCurrency"].ToString();
                DetailedListValue.RevenueExchangeRate = dtrow["RevenueExchangeRate"].ToString();
                DetailedListValue.RevenueHSNOrSACCode = dtrow["RevenueHSNOrSACCode"].ToString();
                DetailedListValue.RevenueQty = dtrow["RevenueQty"].ToString();
                DetailedListValue.RevenueUOM = dtrow["RevenueUOM"].ToString();
                DetailedListValue.RevenueUnitPrice = dtrow["RevenueUnitPrice"].ToString();
                DetailedListValue.RevenueGSTPercentage = dtrow["RevenueGSTPercentage"].ToString();
                DetailedListValue.RevenueGSTAmt = dtrow["RevenueGSTAmt"].ToString();
                DetailedListValue.RevenueTotalPrice = dtrow["RevenueTotalPrice"].ToString();
                DetailedListValue.RevenueReceivableType = dtrow["RevenueReceivableType"].ToString();
                DetailedListValue.RevenueInvoiceNo = dtrow["RevenueInvoiceNo"].ToString();
                DetailedListValue.RevenueInvoiceDate = dtrow["RevenueInvoiceDate"].ToString();
                if (dtrow["RevenueInvoiceDate"].ToString() != "" || dtrow["RevenueInvoiceDate"] != DBNull.Value) { DetailedListValue.RevenueInvoiceDate = Convert.ToDateTime(dtrow["RevenueInvoiceDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.RevenueInvoiceDate = ""; }
                DetailedListValue.RevenueDueDate = dtrow["RevenueDueDate"].ToString();
                if (dtrow["RevenueDueDate"].ToString() != "" || dtrow["RevenueDueDate"] != DBNull.Value) { DetailedListValue.RevenueDueDate = Convert.ToDateTime(dtrow["RevenueDueDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.RevenueDueDate = ""; }
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportRevenue set RevenueChargeHead=@RevenueChargeHead,RevenueChargeDescription=@RevenueChargeDescription,RevenueCostCenter=@RevenueCostCenter,RevenueCustomerCurrency=@RevenueCustomerCurrency,RevenueCompanyCurrency=@RevenueCompanyCurrency,RevenueExchangeRate=@RevenueExchangeRate,RevenueHSNOrSACCode=@RevenueHSNOrSACCode,RevenueQty=@RevenueQty,RevenueUOM=@RevenueUOM,RevenueUnitPrice=@RevenueUnitPrice,RevenueGSTPercentage=@RevenueGSTPercentage,RevenueGSTAmt=@RevenueGSTAmt,RevenueTotalPrice=@RevenueTotalPrice,RevenueReceivableType=@RevenueReceivableType,RevenueInvoiceNo=@RevenueInvoiceNo,RevenueInvoiceDate=@RevenueInvoiceDate,RevenueDueDate=@RevenueDueDate where RevenueChargeHead = @RevenueChargeHead And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", RevenueTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@RevenueChargeHead", RevenueTableDataList[0].RevenueChargeHead.Trim());
            cmd.Parameters.AddWithValue("@RevenueChargeDescription", RevenueTableDataList[0].RevenueChargeDescription.Trim());
            cmd.Parameters.AddWithValue("@RevenueCostCenter", RevenueTableDataList[0].RevenueCostCenter.Trim());
            cmd.Parameters.AddWithValue("@RevenueCustomerCurrency", RevenueTableDataList[0].RevenueCustomerCurrency.Trim());
            cmd.Parameters.AddWithValue("@RevenueCompanyCurrency", RevenueTableDataList[0].RevenueCompanyCurrency.Trim());
            cmd.Parameters.AddWithValue("@RevenueExchangeRate", RevenueTableDataList[0].RevenueExchangeRate.Trim());
            cmd.Parameters.AddWithValue("@RevenueHSNOrSACCode", RevenueTableDataList[0].RevenueHSNOrSACCode.Trim());
            cmd.Parameters.AddWithValue("@RevenueQty", RevenueTableDataList[0].RevenueQty.Trim());
            cmd.Parameters.AddWithValue("@RevenueUOM", RevenueTableDataList[0].RevenueUOM.Trim());
            cmd.Parameters.AddWithValue("@RevenueUnitPrice", RevenueTableDataList[0].RevenueUnitPrice.Trim());
            cmd.Parameters.AddWithValue("@RevenueGSTPercentage", RevenueTableDataList[0].RevenueGSTPercentage.Trim());
            cmd.Parameters.AddWithValue("@RevenueGSTAmt", RevenueTableDataList[0].RevenueGSTAmt.Trim());
            cmd.Parameters.AddWithValue("@RevenueTotalPrice", RevenueTableDataList[0].RevenueTotalPrice.Trim());
            cmd.Parameters.AddWithValue("@RevenueReceivableType", RevenueTableDataList[0].RevenueReceivableType.Trim());
            cmd.Parameters.AddWithValue("@RevenueInvoiceNo", RevenueTableDataList[0].RevenueInvoiceNo.Trim());
            if (RevenueTableDataList[0].RevenueInvoiceDate == "" || RevenueTableDataList[0].RevenueInvoiceDate == null)
            {
                cmd.Parameters.AddWithValue("@RevenueInvoiceDate", DBNull.Value);
                RevenueTableDataList[0].RevenueInvoiceDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("RevenueInvoiceDate", Convert.ToDateTime(RevenueTableDataList[0].RevenueInvoiceDate.Trim()).Date);
                RevenueTableDataList[0].RevenueInvoiceDate = Convert.ToDateTime(RevenueTableDataList[0].RevenueInvoiceDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (RevenueTableDataList[0].RevenueDueDate == "" || RevenueTableDataList[0].RevenueDueDate == null)
            {
                cmd.Parameters.AddWithValue("@RevenueDueDate", DBNull.Value);
                RevenueTableDataList[0].RevenueDueDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("RevenueDueDate", Convert.ToDateTime(RevenueTableDataList[0].RevenueDueDate.Trim()).Date);
                RevenueTableDataList[0].RevenueDueDate = Convert.ToDateTime(RevenueTableDataList[0].RevenueDueDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.RevenueChargeHead != RevenueTableDataList[0].RevenueChargeHead.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueChargeHead", DetailedListValue.RevenueChargeHead,
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueChargeDescription != RevenueTableDataList[0].RevenueChargeDescription.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueChargeDescription", DetailedListValue.RevenueChargeDescription,
                    RevenueTableDataList[0].RevenueChargeDescription.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueCostCenter != RevenueTableDataList[0].RevenueCostCenter.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueCostCenter", DetailedListValue.RevenueCostCenter,
                    RevenueTableDataList[0].RevenueCostCenter.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueCustomerCurrency != RevenueTableDataList[0].RevenueCustomerCurrency.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueCustomerCurrency", DetailedListValue.RevenueCustomerCurrency,
                    RevenueTableDataList[0].RevenueCustomerCurrency.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueCompanyCurrency != RevenueTableDataList[0].RevenueCompanyCurrency.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueCompanyCurrency", DetailedListValue.RevenueCompanyCurrency,
                    RevenueTableDataList[0].RevenueCompanyCurrency.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueExchangeRate != RevenueTableDataList[0].RevenueExchangeRate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueExchangeRate", DetailedListValue.RevenueExchangeRate,
                    RevenueTableDataList[0].RevenueExchangeRate.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueHSNOrSACCode != RevenueTableDataList[0].RevenueHSNOrSACCode.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueHSNOrSACCode", DetailedListValue.RevenueHSNOrSACCode,
                    RevenueTableDataList[0].RevenueHSNOrSACCode.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueQty != RevenueTableDataList[0].RevenueQty.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueQty", DetailedListValue.RevenueQty,
                    RevenueTableDataList[0].RevenueQty.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueUOM != RevenueTableDataList[0].RevenueUOM.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueUOM", DetailedListValue.RevenueUOM,
                    RevenueTableDataList[0].RevenueUOM.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueUnitPrice != RevenueTableDataList[0].RevenueUnitPrice.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueUnitPrice", DetailedListValue.RevenueUnitPrice,
                    RevenueTableDataList[0].RevenueUnitPrice.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueGSTPercentage != RevenueTableDataList[0].RevenueGSTPercentage.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueGSTPercentage", DetailedListValue.RevenueGSTPercentage,
                    RevenueTableDataList[0].RevenueGSTPercentage.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueGSTAmt != RevenueTableDataList[0].RevenueGSTAmt.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueGSTAmt", DetailedListValue.RevenueGSTAmt,
                    RevenueTableDataList[0].RevenueGSTAmt.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueTotalPrice != RevenueTableDataList[0].RevenueTotalPrice.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueTotalPrice", DetailedListValue.RevenueTotalPrice,
                    RevenueTableDataList[0].RevenueTotalPrice.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueReceivableType != RevenueTableDataList[0].RevenueReceivableType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueReceivableType", DetailedListValue.RevenueReceivableType,
                    RevenueTableDataList[0].RevenueReceivableType.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueInvoiceNo != RevenueTableDataList[0].RevenueInvoiceNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueInvoiceNo", DetailedListValue.RevenueInvoiceNo,
                    RevenueTableDataList[0].RevenueInvoiceNo.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueInvoiceDate != RevenueTableDataList[0].RevenueInvoiceDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueInvoiceDate", DetailedListValue.RevenueInvoiceDate,
                    RevenueTableDataList[0].RevenueInvoiceDate.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.RevenueDueDate != RevenueTableDataList[0].RevenueDueDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportRevenue",
                    RevenueTableDataList[0].RevenueChargeHead.Trim(), "RevenueDueDate", DetailedListValue.RevenueDueDate,
                    RevenueTableDataList[0].RevenueDueDate.Trim(), "Modified", "MainJobNo = '" + RevenueTableDataList[0].MainJobNo.Trim() + "' And " +
                    "RevenueChargeHead = '" + RevenueTableDataList[0].RevenueChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------RevenueTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string RevenueTableCancelData(List<RevenueTableDataList> RevenueTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportRevenue set RecordStatus = 'Cancelled' where RevenueChargeHead = @RevenueChargeHead And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@RevenueChargeHead", RevenueTableDataList[0].RevenueChargeHead.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", RevenueTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportRevenue", RevenueTableDataList[0].RevenueChargeHead.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static RevenueTableDataList[] RevenueTableSearchData(List<RevenueTableDataList> RevenueTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<RevenueTableDataList> DetailedList = new List<RevenueTableDataList>();
        RevenueTableDataList DetailedListValue = new RevenueTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportRevenue Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", RevenueTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new RevenueTableDataList();
                DetailedListValue.RevenueChargeHead = dtrow["RevenueChargeHead"].ToString();
                DetailedListValue.RevenueChargeDescription = dtrow["RevenueChargeDescription"].ToString();
                DetailedListValue.RevenueCostCenter = dtrow["RevenueCostCenter"].ToString();
                DetailedListValue.RevenueCustomerCurrency = dtrow["RevenueCustomerCurrency"].ToString();
                DetailedListValue.RevenueCompanyCurrency = dtrow["RevenueCompanyCurrency"].ToString();
                DetailedListValue.RevenueExchangeRate = dtrow["RevenueExchangeRate"].ToString();
                DetailedListValue.RevenueHSNOrSACCode = dtrow["RevenueHSNOrSACCode"].ToString();
                DetailedListValue.RevenueQty = dtrow["RevenueQty"].ToString();
                DetailedListValue.RevenueUOM = dtrow["RevenueUOM"].ToString();
                DetailedListValue.RevenueUnitPrice = dtrow["RevenueUnitPrice"].ToString();
                DetailedListValue.RevenueGSTPercentage = dtrow["RevenueGSTPercentage"].ToString();
                DetailedListValue.RevenueGSTAmt = dtrow["RevenueGSTAmt"].ToString();
                DetailedListValue.RevenueTotalPrice = dtrow["RevenueTotalPrice"].ToString();
                DetailedListValue.RevenueReceivableType = dtrow["RevenueReceivableType"].ToString();
                DetailedListValue.RevenueInvoiceNo = dtrow["RevenueInvoiceNo"].ToString();
                DetailedListValue.RevenueInvoiceDate = dtrow["RevenueInvoiceDate"].ToString();
                DetailedListValue.RevenueDueDate = dtrow["RevenueDueDate"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------CostTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string CostTableInsertData(List<CostTableDataList> CostTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Insert into CFSImportCost(MainJobNo,CostChargeHead,CostChargeDescription,CostCostCenter,CostPayableCurrency,CostCompanyCurrency,CostExchangeRate,CostHSNOrSACCode,CostQty,CostUOM,CostUnitPrice,CostGSTPercentage,CostGSTAmt,CostTotalPrice,CostPayableType,CostEmployeeName,CostVendorName,CostVendorBranch,CostInvoiceNo,CostInvoiceDate,CostDueDate)   Values(@MainJobNo,@CostChargeHead,@CostChargeDescription,@CostCostCenter,@CostPayableCurrency,@CostCompanyCurrency,@CostExchangeRate,@CostHSNOrSACCode,@CostQty,@CostUOM,@CostUnitPrice,@CostGSTPercentage,@CostGSTAmt,@CostTotalPrice,@CostPayableType,@CostEmployeeName,@CostVendorName,@CostVendorBranch,@CostInvoiceNo,@CostInvoiceDate,@CostDueDate)", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", CostTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@CostChargeHead", CostTableDataList[0].CostChargeHead.Trim());
            cmd.Parameters.AddWithValue("@CostChargeDescription", CostTableDataList[0].CostChargeDescription.Trim());
            cmd.Parameters.AddWithValue("@CostCostCenter", CostTableDataList[0].CostCostCenter.Trim());
            cmd.Parameters.AddWithValue("@CostPayableCurrency", CostTableDataList[0].CostPayableCurrency.Trim());
            cmd.Parameters.AddWithValue("@CostCompanyCurrency", CostTableDataList[0].CostCompanyCurrency.Trim());
            cmd.Parameters.AddWithValue("@CostExchangeRate", CostTableDataList[0].CostExchangeRate.Trim());
            cmd.Parameters.AddWithValue("@CostHSNOrSACCode", CostTableDataList[0].CostHSNOrSACCode.Trim());
            cmd.Parameters.AddWithValue("@CostQty", CostTableDataList[0].CostQty.Trim());
            cmd.Parameters.AddWithValue("@CostUOM", CostTableDataList[0].CostUOM.Trim());
            cmd.Parameters.AddWithValue("@CostUnitPrice", CostTableDataList[0].CostUnitPrice.Trim());
            cmd.Parameters.AddWithValue("@CostGSTPercentage", CostTableDataList[0].CostGSTPercentage.Trim());
            cmd.Parameters.AddWithValue("@CostGSTAmt", CostTableDataList[0].CostGSTAmt.Trim());
            cmd.Parameters.AddWithValue("@CostTotalPrice", CostTableDataList[0].CostTotalPrice.Trim());
            cmd.Parameters.AddWithValue("@CostPayableType", CostTableDataList[0].CostPayableType.Trim());
            cmd.Parameters.AddWithValue("@CostEmployeeName", CostTableDataList[0].CostEmployeeName.Trim());
            cmd.Parameters.AddWithValue("@CostVendorName", CostTableDataList[0].CostVendorName.Trim());
            cmd.Parameters.AddWithValue("@CostVendorBranch", CostTableDataList[0].CostVendorBranch.Trim());
            cmd.Parameters.AddWithValue("@CostInvoiceNo", CostTableDataList[0].CostInvoiceNo.Trim());
            if (CostTableDataList[0].CostInvoiceDate == "" || CostTableDataList[0].CostInvoiceDate == null) { cmd.Parameters.AddWithValue("@CostInvoiceDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@CostInvoiceDate", Convert.ToDateTime(CostTableDataList[0].CostInvoiceDate.Trim()).Date); }
            if (CostTableDataList[0].CostDueDate == "" || CostTableDataList[0].CostDueDate == null) { cmd.Parameters.AddWithValue("@CostDueDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@CostDueDate", Convert.ToDateTime(CostTableDataList[0].CostDueDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportCost", CostTableDataList[0].CostChargeHead.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class CostTableDataList
    {
        public string MainJobNo { get; set; }
        public string CostChargeHead { get; set; }
        public string CostChargeDescription { get; set; }
        public string CostCostCenter { get; set; }
        public string CostPayableCurrency { get; set; }
        public string CostCompanyCurrency { get; set; }
        public string CostExchangeRate { get; set; }
        public string CostHSNOrSACCode { get; set; }
        public string CostQty { get; set; }
        public string CostUOM { get; set; }
        public string CostUnitPrice { get; set; }
        public string CostGSTPercentage { get; set; }
        public string CostGSTAmt { get; set; }
        public string CostTotalPrice { get; set; }
        public string CostPayableType { get; set; }
        public string CostEmployeeName { get; set; }
        public string CostVendorName { get; set; }
        public string CostVendorBranch { get; set; }
        public string CostInvoiceNo { get; set; }
        public string CostInvoiceDate { get; set; }
        public string CostDueDate { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------CostTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string CostTableUpdateData(List<CostTableDataList> CostTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<CostTableDataList> DetailedList = new List<CostTableDataList>();
        CostTableDataList DetailedListValue = new CostTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportCost WHERE MainJobNo = @MainJobNo and CostChargeHead = @CostChargeHead and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", CostTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@CostChargeHead", CostTableDataList[0].CostChargeHead.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.CostChargeHead = dtrow["CostChargeHead"].ToString();
                DetailedListValue.CostChargeDescription = dtrow["CostChargeDescription"].ToString();
                DetailedListValue.CostCostCenter = dtrow["CostCostCenter"].ToString();
                DetailedListValue.CostPayableCurrency = dtrow["CostPayableCurrency"].ToString();
                DetailedListValue.CostCompanyCurrency = dtrow["CostCompanyCurrency"].ToString();
                DetailedListValue.CostExchangeRate = dtrow["CostExchangeRate"].ToString();
                DetailedListValue.CostHSNOrSACCode = dtrow["CostHSNOrSACCode"].ToString();
                DetailedListValue.CostQty = dtrow["CostQty"].ToString();
                DetailedListValue.CostUOM = dtrow["CostUOM"].ToString();
                DetailedListValue.CostUnitPrice = dtrow["CostUnitPrice"].ToString();
                DetailedListValue.CostGSTPercentage = dtrow["CostGSTPercentage"].ToString();
                DetailedListValue.CostGSTAmt = dtrow["CostGSTAmt"].ToString();
                DetailedListValue.CostTotalPrice = dtrow["CostTotalPrice"].ToString();
                DetailedListValue.CostPayableType = dtrow["CostPayableType"].ToString();
                DetailedListValue.CostEmployeeName = dtrow["CostEmployeeName"].ToString();
                DetailedListValue.CostVendorName = dtrow["CostVendorName"].ToString();
                DetailedListValue.CostVendorBranch = dtrow["CostVendorBranch"].ToString();
                DetailedListValue.CostInvoiceNo = dtrow["CostInvoiceNo"].ToString();
                DetailedListValue.CostInvoiceDate = dtrow["CostInvoiceDate"].ToString();
                if (dtrow["CostInvoiceDate"].ToString() != "" || dtrow["CostInvoiceDate"] != DBNull.Value) { DetailedListValue.CostInvoiceDate = Convert.ToDateTime(dtrow["CostInvoiceDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.CostInvoiceDate = ""; }
                DetailedListValue.CostDueDate = dtrow["CostDueDate"].ToString();
                if (dtrow["CostDueDate"].ToString() != "" || dtrow["CostDueDate"] != DBNull.Value) { DetailedListValue.CostDueDate = Convert.ToDateTime(dtrow["CostDueDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.CostDueDate = ""; }
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportCost set CostChargeHead=@CostChargeHead,CostChargeDescription=@CostChargeDescription,CostCostCenter=@CostCostCenter,CostPayableCurrency=@CostPayableCurrency,CostCompanyCurrency=@CostCompanyCurrency,CostExchangeRate=@CostExchangeRate,CostHSNOrSACCode=@CostHSNOrSACCode,CostQty=@CostQty,CostUOM=@CostUOM,CostUnitPrice=@CostUnitPrice,CostGSTPercentage=@CostGSTPercentage,CostGSTAmt=@CostGSTAmt,CostTotalPrice=@CostTotalPrice,CostPayableType=@CostPayableType,CostEmployeeName=@CostEmployeeName,CostVendorName=@CostVendorName,CostVendorBranch=@CostVendorBranch,CostInvoiceNo=@CostInvoiceNo,CostInvoiceDate=@CostInvoiceDate,CostDueDate=@CostDueDate where CostChargeHead = @CostChargeHead And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", CostTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@CostChargeHead", CostTableDataList[0].CostChargeHead.Trim());
            cmd.Parameters.AddWithValue("@CostChargeDescription", CostTableDataList[0].CostChargeDescription.Trim());
            cmd.Parameters.AddWithValue("@CostCostCenter", CostTableDataList[0].CostCostCenter.Trim());
            cmd.Parameters.AddWithValue("@CostPayableCurrency", CostTableDataList[0].CostPayableCurrency.Trim());
            cmd.Parameters.AddWithValue("@CostCompanyCurrency", CostTableDataList[0].CostCompanyCurrency.Trim());
            cmd.Parameters.AddWithValue("@CostExchangeRate", CostTableDataList[0].CostExchangeRate.Trim());
            cmd.Parameters.AddWithValue("@CostHSNOrSACCode", CostTableDataList[0].CostHSNOrSACCode.Trim());
            cmd.Parameters.AddWithValue("@CostQty", CostTableDataList[0].CostQty.Trim());
            cmd.Parameters.AddWithValue("@CostUOM", CostTableDataList[0].CostUOM.Trim());
            cmd.Parameters.AddWithValue("@CostUnitPrice", CostTableDataList[0].CostUnitPrice.Trim());
            cmd.Parameters.AddWithValue("@CostGSTPercentage", CostTableDataList[0].CostGSTPercentage.Trim());
            cmd.Parameters.AddWithValue("@CostGSTAmt", CostTableDataList[0].CostGSTAmt.Trim());
            cmd.Parameters.AddWithValue("@CostTotalPrice", CostTableDataList[0].CostTotalPrice.Trim());
            cmd.Parameters.AddWithValue("@CostPayableType", CostTableDataList[0].CostPayableType.Trim());
            cmd.Parameters.AddWithValue("@CostEmployeeName", CostTableDataList[0].CostEmployeeName.Trim());
            cmd.Parameters.AddWithValue("@CostVendorName", CostTableDataList[0].CostVendorName.Trim());
            cmd.Parameters.AddWithValue("@CostVendorBranch", CostTableDataList[0].CostVendorBranch.Trim());
            cmd.Parameters.AddWithValue("@CostInvoiceNo", CostTableDataList[0].CostInvoiceNo.Trim());
            if (CostTableDataList[0].CostInvoiceDate == "" || CostTableDataList[0].CostInvoiceDate == null)
            {
                cmd.Parameters.AddWithValue("@CostInvoiceDate", DBNull.Value);
                CostTableDataList[0].CostInvoiceDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("CostInvoiceDate", Convert.ToDateTime(CostTableDataList[0].CostInvoiceDate.Trim()).Date);
                CostTableDataList[0].CostInvoiceDate = Convert.ToDateTime(CostTableDataList[0].CostInvoiceDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (CostTableDataList[0].CostDueDate == "" || CostTableDataList[0].CostDueDate == null)
            {
                cmd.Parameters.AddWithValue("@CostDueDate", DBNull.Value);
                CostTableDataList[0].CostDueDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("CostDueDate", Convert.ToDateTime(CostTableDataList[0].CostDueDate.Trim()).Date);
                CostTableDataList[0].CostDueDate = Convert.ToDateTime(CostTableDataList[0].CostDueDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.CostChargeHead != CostTableDataList[0].CostChargeHead.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostChargeHead", DetailedListValue.CostChargeHead,
                    CostTableDataList[0].CostChargeHead.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostChargeDescription != CostTableDataList[0].CostChargeDescription.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostChargeDescription", DetailedListValue.CostChargeDescription,
                    CostTableDataList[0].CostChargeDescription.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostCostCenter != CostTableDataList[0].CostCostCenter.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostCostCenter", DetailedListValue.CostCostCenter,
                    CostTableDataList[0].CostCostCenter.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostPayableCurrency != CostTableDataList[0].CostPayableCurrency.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostPayableCurrency", DetailedListValue.CostPayableCurrency,
                    CostTableDataList[0].CostPayableCurrency.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostCompanyCurrency != CostTableDataList[0].CostCompanyCurrency.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostCompanyCurrency", DetailedListValue.CostCompanyCurrency,
                    CostTableDataList[0].CostCompanyCurrency.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostExchangeRate != CostTableDataList[0].CostExchangeRate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostExchangeRate", DetailedListValue.CostExchangeRate,
                    CostTableDataList[0].CostExchangeRate.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostHSNOrSACCode != CostTableDataList[0].CostHSNOrSACCode.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostHSNOrSACCode", DetailedListValue.CostHSNOrSACCode,
                    CostTableDataList[0].CostHSNOrSACCode.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostQty != CostTableDataList[0].CostQty.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostQty", DetailedListValue.CostQty,
                    CostTableDataList[0].CostQty.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostUOM != CostTableDataList[0].CostUOM.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostUOM", DetailedListValue.CostUOM,
                    CostTableDataList[0].CostUOM.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostUnitPrice != CostTableDataList[0].CostUnitPrice.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostUnitPrice", DetailedListValue.CostUnitPrice,
                    CostTableDataList[0].CostUnitPrice.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostGSTPercentage != CostTableDataList[0].CostGSTPercentage.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostGSTPercentage", DetailedListValue.CostGSTPercentage,
                    CostTableDataList[0].CostGSTPercentage.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostGSTAmt != CostTableDataList[0].CostGSTAmt.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostGSTAmt", DetailedListValue.CostGSTAmt,
                    CostTableDataList[0].CostGSTAmt.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostTotalPrice != CostTableDataList[0].CostTotalPrice.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostTotalPrice", DetailedListValue.CostTotalPrice,
                    CostTableDataList[0].CostTotalPrice.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostPayableType != CostTableDataList[0].CostPayableType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostPayableType", DetailedListValue.CostPayableType,
                    CostTableDataList[0].CostPayableType.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostEmployeeName != CostTableDataList[0].CostEmployeeName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostEmployeeName", DetailedListValue.CostEmployeeName,
                    CostTableDataList[0].CostEmployeeName.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostVendorName != CostTableDataList[0].CostVendorName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostVendorName", DetailedListValue.CostVendorName,
                    CostTableDataList[0].CostVendorName.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostVendorBranch != CostTableDataList[0].CostVendorBranch.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostVendorBranch", DetailedListValue.CostVendorBranch,
                    CostTableDataList[0].CostVendorBranch.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostInvoiceNo != CostTableDataList[0].CostInvoiceNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostInvoiceNo", DetailedListValue.CostInvoiceNo,
                    CostTableDataList[0].CostInvoiceNo.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostInvoiceDate != CostTableDataList[0].CostInvoiceDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostInvoiceDate", DetailedListValue.CostInvoiceDate,
                    CostTableDataList[0].CostInvoiceDate.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.CostDueDate != CostTableDataList[0].CostDueDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportCost",
                    CostTableDataList[0].CostChargeHead.Trim(), "CostDueDate", DetailedListValue.CostDueDate,
                    CostTableDataList[0].CostDueDate.Trim(), "Modified", "MainJobNo = '" + CostTableDataList[0].MainJobNo.Trim() + "' And " +
                    "CostChargeHead = '" + CostTableDataList[0].CostChargeHead.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------CostTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string CostTableCancelData(List<CostTableDataList> CostTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportCost set RecordStatus = 'Cancelled' where CostChargeHead = @CostChargeHead And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@CostChargeHead", CostTableDataList[0].CostChargeHead.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", CostTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportCost", CostTableDataList[0].CostChargeHead.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static CostTableDataList[] CostTableSearchData(List<CostTableDataList> CostTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<CostTableDataList> DetailedList = new List<CostTableDataList>();
        CostTableDataList DetailedListValue = new CostTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportCost Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", CostTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new CostTableDataList();
                DetailedListValue.CostChargeHead = dtrow["CostChargeHead"].ToString();
                DetailedListValue.CostChargeDescription = dtrow["CostChargeDescription"].ToString();
                DetailedListValue.CostCostCenter = dtrow["CostCostCenter"].ToString();
                DetailedListValue.CostPayableCurrency = dtrow["CostPayableCurrency"].ToString();
                DetailedListValue.CostCompanyCurrency = dtrow["CostCompanyCurrency"].ToString();
                DetailedListValue.CostExchangeRate = dtrow["CostExchangeRate"].ToString();
                DetailedListValue.CostHSNOrSACCode = dtrow["CostHSNOrSACCode"].ToString();
                DetailedListValue.CostQty = dtrow["CostQty"].ToString();
                DetailedListValue.CostUOM = dtrow["CostUOM"].ToString();
                DetailedListValue.CostUnitPrice = dtrow["CostUnitPrice"].ToString();
                DetailedListValue.CostGSTPercentage = dtrow["CostGSTPercentage"].ToString();
                DetailedListValue.CostGSTAmt = dtrow["CostGSTAmt"].ToString();
                DetailedListValue.CostTotalPrice = dtrow["CostTotalPrice"].ToString();
                DetailedListValue.CostPayableType = dtrow["CostPayableType"].ToString();
                DetailedListValue.CostEmployeeName = dtrow["CostEmployeeName"].ToString();
                DetailedListValue.CostVendorName = dtrow["CostVendorName"].ToString();
                DetailedListValue.CostVendorBranch = dtrow["CostVendorBranch"].ToString();
                DetailedListValue.CostInvoiceNo = dtrow["CostInvoiceNo"].ToString();
                DetailedListValue.CostInvoiceDate = dtrow["CostInvoiceDate"].ToString();
                DetailedListValue.CostDueDate = dtrow["CostDueDate"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class LoadedTruckTableDataList
    {
        public string MainJobNo { get; set; }
        public string LoadTruckWorkOrderNo { get; set; }
        public string LoadTruckWorkOrderDate { get; set; }
        public string LoadTruckGateOutPassNo { get; set; }
        public string LoadTruckGateOutPassDate { get; set; }
        public string LoadTruckModeofGateOut { get; set; }
        public string LoadTruckContainerNo { get; set; }
        public string LoadTruckTruckNo { get; set; }
        public string LoadTruckPkgsorWeight { get; set; }
        public string LoadTruckRemarks { get; set; }
        public string ReturnedValue { get; set; }
        public string LoadTruckStatus { get; set; }
    }
    //-----------------------------------------------------------LoadedTruckTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string LoadedTruckTableUpdateData(List<LoadedTruckTableDataList> LoadedTruckTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, InsertOrUpdate = "", qry = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<LoadedTruckTableDataList> DetailedList = new List<LoadedTruckTableDataList>();
        LoadedTruckTableDataList DetailedListValue = new LoadedTruckTableDataList();
        try
        {
            if (LoadedTruckTableDataList[0].LoadTruckGateOutPassNo.Trim() == "" && LoadedTruckTableDataList[0].LoadTruckGateOutPassDate.Trim() == "")
            {
                InsertOrUpdate = "Inserted";
                qry = "Insert into CFSImportLoadedTruck(ComputerName,IPAddress,Location,MainJobNo,LoadTruckWorkOrderNo,LoadTruckWorkOrderDate,LoadTruckGateOutPassNo,LoadTruckGateOutPassDate,LoadTruckModeofGateOut,LoadTruckContainerNo,LoadTruckTruckNo,LoadTruckPkgsorWeight,LoadTruckRemarks,UpdatedBy) " +
                      "Values(@ComputerName,@IPAddress,@Location,@MainJobNo,@LoadTruckWorkOrderNo,@LoadTruckWorkOrderDate,@LoadTruckGateOutPassNo,@LoadTruckGateOutPassDate,@LoadTruckModeofGateOut,@LoadTruckContainerNo,@LoadTruckTruckNo,@LoadTruckPkgsorWeight,@LoadTruckRemarks,@UpdatedBy)";

                cmd = new SqlCommand("Select Case When Count(*) = 0 Then '1' Else Count(*) + 1 End As [Count] From CFSImportLoadedTruck", cnn);
                cnn.Open();
                SqlDataReader sqlrd = cmd.ExecuteReader();
                sqlrd.Read();
                LoadedTruckTableDataList[0].LoadTruckGateOutPassNo = "LT_GATE_OUT_PASS_" + sqlrd["Count"].ToString();
                LoadedTruckTableDataList[0].LoadTruckGateOutPassDate = DateTime.Now.ToString();
                cnn.Close();
            }
            else
            {
                InsertOrUpdate = "Updated";
                qry = "Update CFSImportLoadedTruck set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, LoadTruckWorkOrderNo=@LoadTruckWorkOrderNo,LoadTruckWorkOrderDate=@LoadTruckWorkOrderDate," +
                      "LoadTruckGateOutPassNo=@LoadTruckGateOutPassNo,LoadTruckGateOutPassDate=@LoadTruckGateOutPassDate,LoadTruckModeofGateOut=@LoadTruckModeofGateOut," +
                      "LoadTruckContainerNo=@LoadTruckContainerNo,LoadTruckTruckNo=@LoadTruckTruckNo,LoadTruckPkgsorWeight=@LoadTruckPkgsorWeight,LoadTruckRemarks=@LoadTruckRemarks " +
                      "where LoadTruckWorkOrderNo = @LoadTruckWorkOrderNo And MainJobNo = @MainJobNo And LoadTruckGateOutPassNo = @LoadTruckGateOutPassNo;";
            }

            if ((LoadedTruckTableDataList[0].LoadTruckGateOutPassNo != "" && LoadedTruckTableDataList[0].LoadTruckGateOutPassNo != null) &&
                (LoadedTruckTableDataList[0].LoadTruckGateOutPassDate != "" && LoadedTruckTableDataList[0].LoadTruckGateOutPassDate != null))
            {
                cmd = new SqlCommand("Update CFSImport Set MainJobNoStatus = 'Completed' Where MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'", cnn);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            if (InsertOrUpdate == "Updated")
            {
                cmd = new SqlCommand("SELECT * FROM CFSImportLoadedTruck WHERE MainJobNo = @MainJobNo and LoadTruckWorkOrderNo = @LoadTruckWorkOrderNo and RecordStatus='Active' And " +
                                     "LoadTruckGateOutPassNo = @LoadTruckGateOutPassNo;", cnn);
                cmd.Parameters.AddWithValue("@MainJobNo", LoadedTruckTableDataList[0].MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@LoadTruckWorkOrderNo", LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim());
                cmd.Parameters.AddWithValue("@LoadTruckGateOutPassNo", LoadedTruckTableDataList[0].LoadTruckGateOutPassNo.Trim());
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dtrow in dt.Rows)
                {
                    DetailedListValue.LoadTruckWorkOrderNo = dtrow["LoadTruckWorkOrderNo"].ToString();
                    DetailedListValue.LoadTruckWorkOrderDate = dtrow["LoadTruckWorkOrderDate"].ToString();
                    if (dtrow["LoadTruckWorkOrderDate"].ToString() != "" || dtrow["LoadTruckWorkOrderDate"] != DBNull.Value) { DetailedListValue.LoadTruckWorkOrderDate = Convert.ToDateTime(dtrow["LoadTruckWorkOrderDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.LoadTruckWorkOrderDate = ""; }
                    DetailedListValue.LoadTruckGateOutPassNo = dtrow["LoadTruckGateOutPassNo"].ToString();
                    DetailedListValue.LoadTruckGateOutPassDate = dtrow["LoadTruckGateOutPassDate"].ToString();
                    if (dtrow["LoadTruckGateOutPassDate"].ToString() != "" || dtrow["LoadTruckGateOutPassDate"] != DBNull.Value) { DetailedListValue.LoadTruckGateOutPassDate = Convert.ToDateTime(dtrow["LoadTruckGateOutPassDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.LoadTruckGateOutPassDate = ""; }
                    DetailedListValue.LoadTruckModeofGateOut = dtrow["LoadTruckModeofGateOut"].ToString();
                    DetailedListValue.LoadTruckContainerNo = dtrow["LoadTruckContainerNo"].ToString();
                    DetailedListValue.LoadTruckTruckNo = dtrow["LoadTruckTruckNo"].ToString();
                    DetailedListValue.LoadTruckPkgsorWeight = dtrow["LoadTruckPkgsorWeight"].ToString();
                    DetailedListValue.LoadTruckRemarks = dtrow["LoadTruckRemarks"].ToString();
                }
                cnn.Close();
            }

            cmd = new SqlCommand(qry, cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedTruckTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@LoadTruckWorkOrderNo", LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim());
            if (LoadedTruckTableDataList[0].LoadTruckWorkOrderDate == "" || LoadedTruckTableDataList[0].LoadTruckWorkOrderDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadTruckWorkOrderDate", DBNull.Value);
                LoadedTruckTableDataList[0].LoadTruckWorkOrderDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadTruckWorkOrderDate", Convert.ToDateTime(LoadedTruckTableDataList[0].LoadTruckWorkOrderDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                LoadedTruckTableDataList[0].LoadTruckWorkOrderDate = Convert.ToDateTime(LoadedTruckTableDataList[0].LoadTruckWorkOrderDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LoadTruckGateOutPassNo", LoadedTruckTableDataList[0].LoadTruckGateOutPassNo.Trim());
            if (LoadedTruckTableDataList[0].LoadTruckGateOutPassDate == "" || LoadedTruckTableDataList[0].LoadTruckGateOutPassDate == null)
            {
                cmd.Parameters.AddWithValue("@LoadTruckGateOutPassDate", DBNull.Value);
                LoadedTruckTableDataList[0].LoadTruckGateOutPassDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("LoadTruckGateOutPassDate", Convert.ToDateTime(LoadedTruckTableDataList[0].LoadTruckGateOutPassDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                LoadedTruckTableDataList[0].LoadTruckGateOutPassDate = Convert.ToDateTime(LoadedTruckTableDataList[0].LoadTruckGateOutPassDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@LoadTruckModeofGateOut", LoadedTruckTableDataList[0].LoadTruckModeofGateOut.Trim());
            cmd.Parameters.AddWithValue("@LoadTruckContainerNo", LoadedTruckTableDataList[0].LoadTruckContainerNo.Trim());
            cmd.Parameters.AddWithValue("@LoadTruckTruckNo", LoadedTruckTableDataList[0].LoadTruckTruckNo.Trim());
            cmd.Parameters.AddWithValue("@LoadTruckPkgsorWeight", LoadedTruckTableDataList[0].LoadTruckPkgsorWeight.Trim());
            cmd.Parameters.AddWithValue("@LoadTruckRemarks", LoadedTruckTableDataList[0].LoadTruckRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            if (InsertOrUpdate == "Updated")
            {
                foreach (DataRow dtrow in dt.Rows)
                {
                    if (DetailedListValue.LoadTruckWorkOrderNo != LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckWorkOrderNo", DetailedListValue.LoadTruckWorkOrderNo,
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckWorkOrderDate != LoadedTruckTableDataList[0].LoadTruckWorkOrderDate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckWorkOrderDate", DetailedListValue.LoadTruckWorkOrderDate,
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderDate.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckGateOutPassNo != LoadedTruckTableDataList[0].LoadTruckGateOutPassNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckGateOutPassNo", DetailedListValue.LoadTruckGateOutPassNo,
                        LoadedTruckTableDataList[0].LoadTruckGateOutPassNo.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckGateOutPassDate != LoadedTruckTableDataList[0].LoadTruckGateOutPassDate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckGateOutPassDate", DetailedListValue.LoadTruckGateOutPassDate,
                        LoadedTruckTableDataList[0].LoadTruckGateOutPassDate.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckModeofGateOut != LoadedTruckTableDataList[0].LoadTruckModeofGateOut.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckModeofGateOut", DetailedListValue.LoadTruckModeofGateOut,
                        LoadedTruckTableDataList[0].LoadTruckModeofGateOut.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckContainerNo != LoadedTruckTableDataList[0].LoadTruckContainerNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckContainerNo", DetailedListValue.LoadTruckContainerNo,
                        LoadedTruckTableDataList[0].LoadTruckContainerNo.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckTruckNo != LoadedTruckTableDataList[0].LoadTruckTruckNo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckTruckNo", DetailedListValue.LoadTruckTruckNo,
                        LoadedTruckTableDataList[0].LoadTruckTruckNo.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckPkgsorWeight != LoadedTruckTableDataList[0].LoadTruckPkgsorWeight.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckPkgsorWeight", DetailedListValue.LoadTruckPkgsorWeight,
                        LoadedTruckTableDataList[0].LoadTruckPkgsorWeight.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LoadTruckRemarks != LoadedTruckTableDataList[0].LoadTruckRemarks.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLoadedTruck",
                        LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim(), "LoadTruckRemarks", DetailedListValue.LoadTruckRemarks,
                        LoadedTruckTableDataList[0].LoadTruckRemarks.Trim(), "Modified", "MainJobNo = '" + LoadedTruckTableDataList[0].MainJobNo.Trim() + "' And " +
                        "LoadTruckWorkOrderNo = '" + LoadedTruckTableDataList[0].LoadTruckWorkOrderNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                }
            }

            return InsertOrUpdate;
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static LoadedTruckTableDataList[] LoadedTruckTableSearchData(List<LoadedTruckTableDataList> LoadedTruckTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, Top500 = "", JobNo = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<LoadedTruckTableDataList> DetailedList = new List<LoadedTruckTableDataList>();
        LoadedTruckTableDataList DetailedListValue = new LoadedTruckTableDataList();
        try
        {
            if (LoadedTruckTableDataList[0].LoadTruckStatus.Trim() == "Completed") { Top500 = "Top 500"; }
            if (LoadedTruckTableDataList[0].MainJobNo.Trim() != "" && LoadedTruckTableDataList[0].MainJobNo.Trim() != null) { JobNo = "And A.MainJobNo = @MainJobNo"; }

            cmd = new SqlCommand(@"Select " + Top500 + @" * From (Select 
                                A.MainJobNo, A.LoadOutWorkOrderNo As [LoadTruckWorkOrderNo], A.LoadOutWorkOrderDate As [LoadTruckWorkOrderDate], B.LoadTruckGateOutPassNo AS 
                                [LoadTruckGateOutPassNo], B.LoadTruckGateOutPassDate As [LoadTruckGateOutPassDate], 
                                Case When B.LoadTruckModeofGateOut <> '' Or B.LoadTruckModeofGateOut <> Null Then B.LoadTruckModeofGateOut 
                                Else 'Loaded Container Out' End As [LoadTruckModeofGateOut], 
                                A.LoadOutContainerNo As [LoadTruckContainerNo], A.LoadOutVehicleNo As [LoadTruckTruckNo], 
                                Case When B.LoadTruckPkgsorWeight <> '' Or B.LoadTruckPkgsorWeight <> Null Then Cast(B.LoadTruckPkgsorWeight As varchar) 
                                Else '' End As [LoadTruckPkgsorWeight], 
                                B.LoadTruckRemarks As [LoadTruckRemarks], 
                                Case When B.LoadTruckGateOutPassNo <> '' And B.LoadTruckGateOutPassNo Is Not Null Then 'Completed' Else 'Pending' End As [LoadedTruckStatus] 
                                From CFSImportLoadedContainerOut A 
                                Left Join CFSImportLoadedTruck B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.LoadOutContainerNo = B.LoadTruckContainerNo 
                                And A.LoadOutVehicleNo = B.LoadTruckTruckNo 
                                Where A.RecordStatus = 'Active' And A.LoadOutWorkOrderStatus = 'Completed' And (A.LoadOutWorkOrderNo <> '' And A.LoadOutWorkOrderNo Is Not Null) 
                                Union 
                                Select 
                                A.MainJobNo, A.FCLOutWorkOrderNo, A.FCLOutWorkOrderDate, B.LoadTruckGateOutPassNo, B.LoadTruckGateOutPassDate, 
                                Case When B.LoadTruckModeofGateOut <> '' Or B.LoadTruckModeofGateOut <> Null Then B.LoadTruckModeofGateOut 
                                Else 'FCL Cargo Out' End As [Mode of Gate Out],
                                A.FCLOutContainerNo, A.FCLOutVehicleNo, 
                                Case When B.LoadTruckPkgsorWeight <> '' Or B.LoadTruckPkgsorWeight <> Null Then Cast(B.LoadTruckPkgsorWeight As varchar) 
                                Else cast(A.FCLOutDestuffedPkgs As varchar) + ' / ' + cast(A.FCLOutDestuffedWeight As varchar) End As [LoadTruckPkgsorWeight],
                                B.LoadTruckRemarks, 
                                Case When B.LoadTruckGateOutPassNo <> '' And B.LoadTruckGateOutPassNo Is Not Null Then 'Completed' Else 'Pending' End As [LoadedTruckStatus] 
                                From CFSImportFCLCargoOut A 
                                Left Join CFSImportLoadedTruck B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.FCLOutContainerNo = B.LoadTruckContainerNo 
                                And A.FCLOutVehicleNo = B.LoadTruckTruckNo 
                                Where A.RecordStatus = 'Active' And A.FCLOutWorkOrderStatus = 'Completed' And (A.FCLOutWorkOrderNo <> '' And A.FCLOutWorkOrderNo Is Not Null)
                                Union
                                Select 
                                A.MainJobNo, A.WorkOrderWorkOrderNo As [LoadTruckWorkOrderNo], A.WorkOrderWorkOrderDate As [LoadTruckWorkOrderDate], B.LoadTruckGateOutPassNo AS 
                                [LoadTruckGateOutPassNo], B.LoadTruckGateOutPassDate As [LoadTruckGateOutPassDate], 
                                Case When B.LoadTruckModeofGateOut <> '' Or B.LoadTruckModeofGateOut <> Null Then B.LoadTruckModeofGateOut 
                                Else 'Empty Truck Out' End As [LoadTruckModeofGateOut],
                                '' As [LoadTruckContainerNo], A.WorkOrderTruckNo As [LoadTruckTruckNo], 
                                '' As [LoadTruckPkgsorWeight],
                                B.LoadTruckRemarks As [LoadTruckRemarks],
                                Case When B.LoadTruckGateOutPassNo <> '' And B.LoadTruckGateOutPassNo Is Not Null Then 'Completed' Else 'Pending' End As [LoadedTruckStatus]
                                From CFSImportWorkOrder A
                                Left Join CFSImportLoadedTruck B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.WorkOrderTruckNo = B.LoadTruckTruckNo
                                Where A.RecordStatus = 'Active' And (A.WorkOrderWorkOrderNo <> '' And A.WorkOrderWorkOrderNo Is Not Null) And A.WorkOrderType = 'Empty Truck Out'
                                Union
                                Select 
                                A.MainJobNo, A.MtyOutWorkOrderNo As [LoadTruckWorkOrderNo], A.MtyOutWorkOrderDate As [LoadTruckWorkOrderDate], B.LoadTruckGateOutPassNo AS 
                                [LoadTruckGateOutPassNo], B.LoadTruckGateOutPassDate As [LoadTruckGateOutPassDate], 
                                Case When B.LoadTruckModeofGateOut <> '' Or B.LoadTruckModeofGateOut <> Null Then B.LoadTruckModeofGateOut 
                                Else 'Empty Container Out' End As [LoadTruckModeofGateOut],
                                A.MtyOutContainerNo As [LoadTruckContainerNo], A.MtyOutVehicleNo As [LoadTruckTruckNo], 
                                '' As [LoadTruckPkgsorWeight],
                                B.LoadTruckRemarks As [LoadTruckRemarks],
                                Case When B.LoadTruckGateOutPassNo <> '' And B.LoadTruckGateOutPassNo Is Not Null Then 'Completed' Else 'Pending' End As [LoadedTruckStatus]
                                From CFSImportEmptyContainerOut A
                                Left Join CFSImportLoadedTruck B On A.MainJobNo = B.MainJobNo And A.RecordStatus = B.RecordStatus And A.MtyOutContainerNo = B.LoadTruckContainerNo
                                And A.MtyOutVehicleNo = B.LoadTruckTruckNo
                                Where A.RecordStatus = 'Active' And A.MtyOutWorkOrderStatus = 'Completed' And (A.MtyOutWorkOrderNo <> '' And A.MtyOutWorkOrderNo Is Not Null)
                                ) A Where A.LoadedTruckStatus = @LoadTruckStatus " + JobNo + @"", cnn);

            cmd.Parameters.AddWithValue("@LoadTruckStatus", LoadedTruckTableDataList[0].LoadTruckStatus.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", LoadedTruckTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                if (dtrow["LoadTruckWorkOrderNo"].ToString() != "" && dtrow["LoadTruckWorkOrderNo"].ToString() != null)
                {
                    DetailedListValue = new LoadedTruckTableDataList();
                    DetailedListValue.LoadTruckWorkOrderNo = dtrow["LoadTruckWorkOrderNo"].ToString();
                    DetailedListValue.LoadTruckWorkOrderDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["LoadTruckWorkOrderDate"]);
                    DetailedListValue.LoadTruckGateOutPassNo = dtrow["LoadTruckGateOutPassNo"].ToString();
                    DetailedListValue.LoadTruckGateOutPassDate = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["LoadTruckGateOutPassDate"]);
                    DetailedListValue.LoadTruckModeofGateOut = dtrow["LoadTruckModeofGateOut"].ToString();
                    DetailedListValue.LoadTruckContainerNo = dtrow["LoadTruckContainerNo"].ToString();
                    DetailedListValue.LoadTruckTruckNo = dtrow["LoadTruckTruckNo"].ToString();
                    DetailedListValue.LoadTruckPkgsorWeight = dtrow["LoadTruckPkgsorWeight"].ToString();
                    DetailedListValue.LoadTruckRemarks = dtrow["LoadTruckRemarks"].ToString();
                    DetailedListValue.MainJobNo = dtrow["MainJobNo"].ToString();
                    DetailedList.Add(DetailedListValue);
                }
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------TrackingTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string TrackingTableInsertData(List<TrackingTableDataList> TrackingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Insert into CFSImportTracking(MainJobNo,TrackingMileStone,TrackingPlanDate,TrackingActualDate,TrackingRemarks)   Values(@MainJobNo,@TrackingMileStone,@TrackingPlanDate,@TrackingActualDate,@TrackingRemarks)", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TrackingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@TrackingMileStone", TrackingTableDataList[0].TrackingMileStone.Trim());
            if (TrackingTableDataList[0].TrackingPlanDate == "" || TrackingTableDataList[0].TrackingPlanDate == null) { cmd.Parameters.AddWithValue("@TrackingPlanDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@TrackingPlanDate", Convert.ToDateTime(TrackingTableDataList[0].TrackingPlanDate.Trim()).Date); }
            if (TrackingTableDataList[0].TrackingActualDate == "" || TrackingTableDataList[0].TrackingActualDate == null) { cmd.Parameters.AddWithValue("@TrackingActualDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@TrackingActualDate", Convert.ToDateTime(TrackingTableDataList[0].TrackingActualDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@TrackingRemarks", TrackingTableDataList[0].TrackingRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportTracking", TrackingTableDataList[0].TrackingMileStone.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class TrackingTableDataList
    {
        public string MainJobNo { get; set; }
        public string TrackingMileStone { get; set; }
        public string TrackingPlanDate { get; set; }
        public string TrackingActualDate { get; set; }
        public string TrackingRemarks { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------TrackingTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string TrackingTableUpdateData(List<TrackingTableDataList> TrackingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<TrackingTableDataList> DetailedList = new List<TrackingTableDataList>();
        TrackingTableDataList DetailedListValue = new TrackingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportTracking WHERE MainJobNo = @MainJobNo and TrackingMileStone = @TrackingMileStone and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TrackingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@TrackingMileStone", TrackingTableDataList[0].TrackingMileStone.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.TrackingMileStone = dtrow["TrackingMileStone"].ToString();
                DetailedListValue.TrackingPlanDate = dtrow["TrackingPlanDate"].ToString();
                if (dtrow["TrackingPlanDate"].ToString() != "" || dtrow["TrackingPlanDate"] != DBNull.Value) { DetailedListValue.TrackingPlanDate = Convert.ToDateTime(dtrow["TrackingPlanDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TrackingPlanDate = ""; }
                DetailedListValue.TrackingActualDate = dtrow["TrackingActualDate"].ToString();
                if (dtrow["TrackingActualDate"].ToString() != "" || dtrow["TrackingActualDate"] != DBNull.Value) { DetailedListValue.TrackingActualDate = Convert.ToDateTime(dtrow["TrackingActualDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.TrackingActualDate = ""; }
                DetailedListValue.TrackingRemarks = dtrow["TrackingRemarks"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportTracking set TrackingMileStone=@TrackingMileStone,TrackingPlanDate=@TrackingPlanDate,TrackingActualDate=@TrackingActualDate,TrackingRemarks=@TrackingRemarks where TrackingMileStone = @TrackingMileStone And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TrackingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@TrackingMileStone", TrackingTableDataList[0].TrackingMileStone.Trim());
            if (TrackingTableDataList[0].TrackingPlanDate == "" || TrackingTableDataList[0].TrackingPlanDate == null)
            {
                cmd.Parameters.AddWithValue("@TrackingPlanDate", DBNull.Value);
                TrackingTableDataList[0].TrackingPlanDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TrackingPlanDate", Convert.ToDateTime(TrackingTableDataList[0].TrackingPlanDate.Trim()).Date);
                TrackingTableDataList[0].TrackingPlanDate = Convert.ToDateTime(TrackingTableDataList[0].TrackingPlanDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (TrackingTableDataList[0].TrackingActualDate == "" || TrackingTableDataList[0].TrackingActualDate == null)
            {
                cmd.Parameters.AddWithValue("@TrackingActualDate", DBNull.Value);
                TrackingTableDataList[0].TrackingActualDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("TrackingActualDate", Convert.ToDateTime(TrackingTableDataList[0].TrackingActualDate.Trim()).Date);
                TrackingTableDataList[0].TrackingActualDate = Convert.ToDateTime(TrackingTableDataList[0].TrackingActualDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@TrackingRemarks", TrackingTableDataList[0].TrackingRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.TrackingMileStone != TrackingTableDataList[0].TrackingMileStone.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportTracking",
                    TrackingTableDataList[0].TrackingMileStone.Trim(), "TrackingMileStone", DetailedListValue.TrackingMileStone,
                    TrackingTableDataList[0].TrackingMileStone.Trim(), "Modified", "MainJobNo = '" + TrackingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "TrackingMileStone = '" + TrackingTableDataList[0].TrackingMileStone.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TrackingPlanDate != TrackingTableDataList[0].TrackingPlanDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportTracking",
                    TrackingTableDataList[0].TrackingMileStone.Trim(), "TrackingPlanDate", DetailedListValue.TrackingPlanDate,
                    TrackingTableDataList[0].TrackingPlanDate.Trim(), "Modified", "MainJobNo = '" + TrackingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "TrackingMileStone = '" + TrackingTableDataList[0].TrackingMileStone.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TrackingActualDate != TrackingTableDataList[0].TrackingActualDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportTracking",
                    TrackingTableDataList[0].TrackingMileStone.Trim(), "TrackingActualDate", DetailedListValue.TrackingActualDate,
                    TrackingTableDataList[0].TrackingActualDate.Trim(), "Modified", "MainJobNo = '" + TrackingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "TrackingMileStone = '" + TrackingTableDataList[0].TrackingMileStone.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.TrackingRemarks != TrackingTableDataList[0].TrackingRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportTracking",
                    TrackingTableDataList[0].TrackingMileStone.Trim(), "TrackingRemarks", DetailedListValue.TrackingRemarks,
                    TrackingTableDataList[0].TrackingRemarks.Trim(), "Modified", "MainJobNo = '" + TrackingTableDataList[0].MainJobNo.Trim() + "' And " +
                    "TrackingMileStone = '" + TrackingTableDataList[0].TrackingMileStone.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------TrackingTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string TrackingTableCancelData(List<TrackingTableDataList> TrackingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportTracking set RecordStatus = 'Cancelled' where TrackingMileStone = @TrackingMileStone And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@TrackingMileStone", TrackingTableDataList[0].TrackingMileStone.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", TrackingTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportTracking", TrackingTableDataList[0].TrackingMileStone.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static TrackingTableDataList[] TrackingTableSearchData(List<TrackingTableDataList> TrackingTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<TrackingTableDataList> DetailedList = new List<TrackingTableDataList>();
        TrackingTableDataList DetailedListValue = new TrackingTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportTracking Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", TrackingTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new TrackingTableDataList();
                DetailedListValue.TrackingMileStone = dtrow["TrackingMileStone"].ToString();
                DetailedListValue.TrackingPlanDate = dtrow["TrackingPlanDate"].ToString();
                DetailedListValue.TrackingActualDate = dtrow["TrackingActualDate"].ToString();
                DetailedListValue.TrackingRemarks = dtrow["TrackingRemarks"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------NotesTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string NotesTableInsertData(List<NotesTableDataList> NotesTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Insert into CFSImportNotes(MainJobNo,ImportNotes,Reminder,NotesRemarks)   Values(@MainJobNo,@ImportNotes,@Reminder,@NotesRemarks)", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", NotesTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ImportNotes", NotesTableDataList[0].ImportNotes.Trim());
            if (NotesTableDataList[0].Reminder == "" || NotesTableDataList[0].Reminder == null) { cmd.Parameters.AddWithValue("@Reminder", DBNull.Value); } else { cmd.Parameters.AddWithValue("@Reminder", Convert.ToDateTime(NotesTableDataList[0].Reminder.Trim())); }
            cmd.Parameters.AddWithValue("@NotesRemarks", NotesTableDataList[0].NotesRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImportNotes", NotesTableDataList[0].ImportNotes.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class NotesTableDataList
    {
        public string MainJobNo { get; set; }
        public string ImportNotes { get; set; }
        public string Reminder { get; set; }
        public string NotesRemarks { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------NotesTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string NotesTableUpdateData(List<NotesTableDataList> NotesTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        DataTable dt = new DataTable();
        List<NotesTableDataList> DetailedList = new List<NotesTableDataList>();
        NotesTableDataList DetailedListValue = new NotesTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImportNotes WHERE MainJobNo = @MainJobNo and ImportNotes = @ImportNotes and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", NotesTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ImportNotes", NotesTableDataList[0].ImportNotes.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.ImportNotes = dtrow["ImportNotes"].ToString();
                DetailedListValue.Reminder = dtrow["Reminder"].ToString();
                if (dtrow["Reminder"].ToString() != "" || dtrow["Reminder"] != DBNull.Value) { DetailedListValue.Reminder = Convert.ToDateTime(dtrow["Reminder"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.Reminder = ""; }
                DetailedListValue.NotesRemarks = dtrow["NotesRemarks"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportNotes set ImportNotes=@ImportNotes,Reminder=@Reminder,NotesRemarks=@NotesRemarks where ImportNotes = @ImportNotes And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", NotesTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@ImportNotes", NotesTableDataList[0].ImportNotes.Trim());
            if (NotesTableDataList[0].Reminder == "" || NotesTableDataList[0].Reminder == null)
            {
                cmd.Parameters.AddWithValue("@Reminder", DBNull.Value);
                NotesTableDataList[0].Reminder = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("Reminder", Convert.ToDateTime(NotesTableDataList[0].Reminder.Trim()));
                NotesTableDataList[0].Reminder = Convert.ToDateTime(NotesTableDataList[0].Reminder).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@NotesRemarks", NotesTableDataList[0].NotesRemarks.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.ImportNotes != NotesTableDataList[0].ImportNotes.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportNotes",
                    NotesTableDataList[0].ImportNotes.Trim(), "ImportNotes", DetailedListValue.ImportNotes,
                    NotesTableDataList[0].ImportNotes.Trim(), "Modified", "MainJobNo = '" + NotesTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ImportNotes = '" + NotesTableDataList[0].ImportNotes.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.Reminder != NotesTableDataList[0].Reminder.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportNotes",
                    NotesTableDataList[0].ImportNotes.Trim(), "Reminder", DetailedListValue.Reminder,
                    NotesTableDataList[0].Reminder.Trim(), "Modified", "MainJobNo = '" + NotesTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ImportNotes = '" + NotesTableDataList[0].ImportNotes.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.NotesRemarks != NotesTableDataList[0].NotesRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImportNotes",
                    NotesTableDataList[0].ImportNotes.Trim(), "NotesRemarks", DetailedListValue.NotesRemarks,
                    NotesTableDataList[0].NotesRemarks.Trim(), "Modified", "MainJobNo = '" + NotesTableDataList[0].MainJobNo.Trim() + "' And " +
                    "ImportNotes = '" + NotesTableDataList[0].ImportNotes.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------NotesTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string NotesTableCancelData(List<NotesTableDataList> NotesTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImportNotes set RecordStatus = 'Cancelled' where ImportNotes = @ImportNotes And MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@ImportNotes", NotesTableDataList[0].ImportNotes.Trim());
            cmd.Parameters.AddWithValue("@MainJobNo", NotesTableDataList[0].MainJobNo.Trim());
            cmd.Parameters.AddWithValue("@UpdatedBy", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            InsertOrUpdateUmatchedValue("CFSImportNotes", NotesTableDataList[0].ImportNotes.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            cnn.Close();
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static NotesTableDataList[] NotesTableSearchData(List<NotesTableDataList> NotesTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<NotesTableDataList> DetailedList = new List<NotesTableDataList>();
        NotesTableDataList DetailedListValue = new NotesTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportNotes Where RecordStatus = 'Active' and MainJobNo = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", NotesTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new NotesTableDataList();
                DetailedListValue.ImportNotes = dtrow["ImportNotes"].ToString();
                DetailedListValue.Reminder = dtrow["Reminder"].ToString();
                DetailedListValue.NotesRemarks = dtrow["NotesRemarks"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class LogTableDataList
    {
        public string MainJobNo { get; set; }
        public string TableKeyColumn { get; set; }
        //public string TableName { get; set; }
        public string UpdatedBy { get; set; }
        public string Comments { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UpdatedOn { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static LogTableDataList[] LogTableSearchData(List<LogTableDataList> LogTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<LogTableDataList> DetailedList = new List<LogTableDataList>();
        LogTableDataList DetailedListValue = new LogTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select * from CFSImportLog Where RecordStatus = 'Active' And TableKeyColumn = @MainJobNo;", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", LogTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new LogTableDataList();
                DetailedListValue.TableKeyColumn = dtrow["TableKeyColumn"].ToString();
                //DetailedListValue.TableName = dtrow["TableName"].ToString();
                DetailedListValue.UpdatedBy = dtrow["UpdatedBy"].ToString();
                DetailedListValue.Comments = dtrow["Comments"].ToString();
                DetailedListValue.ColumnName = dtrow["ColumnName"].ToString();
                DetailedListValue.OldValue = dtrow["OldValue"].ToString();
                DetailedListValue.NewValue = dtrow["NewValue"].ToString();
                DetailedListValue.UpdatedOn = dtrow["UpdatedOn"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------HTMLTableDupCheckEndsHere-----------------------------------------------------------------
    [WebMethod]
    public static DropDownDetails[] DropDownValueMethod(string columnvalue)
    {
        string Condition = "";
        if (columnvalue != "" && columnvalue != null) { Condition = "MainJobNo = '" + columnvalue + "' And"; }

        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        string qry = " SELECT 'MainIGMNo' As [DropDownColumnName], MainIGMNo AS [DefaultValue] FROM CFSImport Where RecordStatus = 'Active' And MainJobNoStatus = 'New' " +
                     " UNION SELECT 'JobNo' As [DropDownColumnName], MainJobNo AS [DefaultValue] FROM CFSImport Where RecordStatus = 'Active' And MainJobNoStatus = 'New' " +
                     " UNION SELECT 'PortName', PortName FROM PortMaster Where RecordStatus = 'Active' And PortStatus = 'New' And Country = 'India' " +
                     " UNION SELECT DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-Services' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                     " UNION SELECT 'Country', Country FROM PortMaster Where RecordStatus = 'Active' And PortStatus = 'New' And Country <> 'India' " +
                     " UNION SELECT DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-Commodity' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                     " UNION SELECT 'ContainerISOCode', ContainerISOCode FROM CFSImportISOCodeMaster Where RecordStatus = 'Active' " +
                     " UNION SELECT DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-CargoNature' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                     " UNION SELECT DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-Scan Location' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                     " UNION SELECT DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-HoldAgency' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                     " UNION Select DropDownColumnName, DefaultValue From DefaultDropDownValues Where DropDownColumnName = 'Import CFS Charge Head' And RecordStatus = 'Active' " +
                     " UNION SELECT 'TransportVehicleNo', TransportVehicleNo FROM CFSImportContainer Where RecordStatus = 'Active' " +
                     " UNION SELECT 'TransportVehicleNo', LoadContVehicleNo FROM CFSImportContainer Where RecordStatus = 'Active' " +
                     " UNION SELECT 'TransportVehicleNo', EmptyTorCTruckNo FROM CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' " +
                     " UNION SELECT 'ContainerNo', ContainerNo FROM CFSImportContainer Where RecordStatus = 'Active' And ContainerStatus = 'New' " +
                     " UNION SELECT 'TemplateName', TemplateName FROM ICSAColumnTemplates Where RecordStatus = 'Active' And FormName = 'CFSImport.aspx' " +
                     " UNION SELECT 'ContType', ContainerType FROM CFSImportISOCodeMaster Where RecordStatus = 'Active' " +
                     " UNION SELECT 'Equipment Name', EquipmentName FROM CFSEquipmentMaster Where RecordStatus = 'Active' " +
                     " UNION SELECT 'Vendor Name', VendorName FROM CFSEquipmentMaster Where RecordStatus = 'Active' And VendorName <> '' " +
                     " UNION SELECT 'Vendor Name', EntityName FROM EntityType Where Vertical = 'CFS' And EntityType Like '%Vendor%' And EntityType Like '%Survey%' And RecordStatus = 'Active' " +
                     " Union Select 'Vessel Name', MainVesselName From CFSImport Where RecordStatus = 'Active' " +
                     " Union Select 'BLNumber', LinerBLNo From CFSImportLiner Where " + Condition + " RecordStatus = 'Active' " +
                     " Union Select Distinct 'Access Company', CompanyName from UserAccessGeneral Where RecordStatus = 'Active' And Employee = '" + HttpContext.Current.Session["UserName"].ToString() + "' " +
                     " Union Select 'Consoler Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Consoler%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'Line Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Line%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'Customer Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'Forwarder', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Forwarder%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'Importer Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Importer%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'CHA Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%CHA%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                     " Union Select 'Transport Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Vendor%' And EntityType Like '%Transport%' And RecordStatus = 'Active' " +
                     " Union Select 'Liner Code', EntityDesc From [Entity] Where EntityName In " +
                     " (Select EntityName From [EntityType] Where Vertical = 'CFS' And RecordStatus = 'Active') And RecordStatus = 'Active' " +
                     " ORDER BY 1,2;";

        DataTable dt = new DataTable();
        List<DropDownDetails> details = new List<DropDownDetails>();

        using (SqlConnection cnn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(qry, cnn))
            {
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dtrow in dt.Rows)
                {
                    DropDownDetails columns = new DropDownDetails();
                    columns.DropDownColumnName = dtrow["DropDownColumnName"].ToString();
                    columns.DefaultValue = dtrow["DefaultValue"].ToString();
                    details.Add(columns);
                }
                cnn.Close();
            }
        }
        return details.ToArray();
    }
    public class DropDownDetails
    {
        public string DropDownColumnName { get; set; }
        public string DefaultValue { get; set; }
        public string ReturnedValue { get; set; }
        public string ddvalue { get; set; }
        public string ddname { get; set; }
    }
    //-----------------------------------------------------------MainTableInsertDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string MainTableInsertData(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        try
        {
            cmd = new SqlCommand("Select Count(*) From CFSImport Where MainIGMNo = '" + MainTableDataList[0].MainIGMNo + "' And MainItemNo = '" + MainTableDataList[0].MainItemNo + "' " +
                                 "And RecordStatus = 'Active' And MainJobNoStatus = 'New';");
            cmd.Connection = cnn;
            cnn.Open();
            int DupCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
            cnn.Close();

            if (DupCount > 0) { return "Duplicate Entry !!!"; }

            cmd = new SqlCommand("SELECT " +
        "    iif(b.MainJobNo is null,Prefix,JobNoPreFix) as JobNoPreFix, " +
        "    iif(b.MainJobNo is null,NumberSeries,JobNoSeries+1) as JobNoSeries, " +
        "    iif(b.MainJobNo is null,Suffix,FinancialYear) as FinancialYear, " +
        "    case when b.MainJobNo is null then " +
        "    Concat(Prefix,NumberSeries,'/',Suffix) " +
        "    else  " +
        "    Concat(JobNoPreFix,JobNoSeries+1,'/',Suffix)  end as MainJobNo " +
        "    FROM TransactionNumberFormat a left join ( " +
        "    select top 1 MainJobNo, JobNoPreFix,JobNoSeries, FinancialYear from CFSImport order by MainJobNoId Desc ) b " +
        "    on a.Suffix = b.FinancialYear where Module = 'CFS' And FormName = 'Import' And " +
        "    Suffix = substring(datename(YEAR,  DATEADD(M,-3,GETDATE())),3,2) +'-'+ cast((datepart(YEAR,  DATEADD(M,-3,GETDATE())) + 1) %100 as varchar(2)); ", cnn);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string JobNoPreFix = "", JobNoSeries = "", FinancialYear = "", RefNo = "";
            foreach (DataRow dtrow in dt.Rows)
            {
                JobNoPreFix = dtrow["JobNoPreFix"].ToString();
                JobNoSeries = dtrow["JobNoSeries"].ToString();
                FinancialYear = dtrow["FinancialYear"].ToString();
                RefNo = dtrow["MainJobNo"].ToString();
            }
            cnn.Close();
            cmd = new SqlCommand("Insert into CFSImport(ComputerName,IPAddress,Location,CompanyName,BranchName,JobNoPreFix,JobNoSeries,FinancialYear,MainJobNo,MainIGMNo,MainItemNo,MainIGMDate,MainBookingNo,MainBookingDate,MainVesselName,MainPOA,MainVIANo,MainVoyNo,MainJobOwner,MainIGMUpload,GeneralDOA,GeneralServices,GeneralTPNo,GeneralTPDate,GeneralConsoler,GeneralBerthingDate,GeneralDateofDeparture,GeneralCutoffDate,GeneralRotationNo,GeneralGateOpenDate,GeneralScanlistRecvDate,GeneralBOENo,GeneralBOEDate,GeneralLineName,GeneralEnblock,GeneralGCRMSStatus,GeneralPOLCountry,GeneralPOL,GeneralPOLCode,GeneralPOD,GeneralAccountHolder,GeneralSplitItem,GeneralConsoleValidUpto,GeneralDONo,GeneralDODate,GeneralDOValidDate,GeneralSMTP,GeneralCustomChallanNo,GeneralDutyValue,GeneralVolume,GeneralAssessableValue,GeneralCommodity,GeneralDefaultCustomExamPerc,GeneralFromLocation,GeneralPackageType,GeneralForwarder,GeneralNominatedCustomer,GeneralBacktoPort,GeneralBacktoPortRemarks,GeneralTransportation,GeneralGateInType,GeneralShippingLine) " +
                                 "Values(@ComputerName,@IPAddress,@Location,@CompanyName,@BranchName,@JobNoPreFix,@JobNoSeries,@FinancialYear,@MainJobNo,@MainIGMNo,@MainItemNo,@MainIGMDate,@MainBookingNo,@MainBookingDate,@MainVesselName,@MainPOA,@MainVIANo,@MainVoyNo,@MainJobOwner,@MainIGMUpload,@GeneralDOA,@GeneralServices,@GeneralTPNo,@GeneralTPDate,@GeneralConsoler,@GeneralBerthingDate,@GeneralDateofDeparture,@GeneralCutoffDate,@GeneralRotationNo,@GeneralGateOpenDate,@GeneralScanlistRecvDate,@GeneralBOENo,@GeneralBOEDate,@GeneralLineName,@GeneralEnblock,@GeneralGCRMSStatus,@GeneralPOLCountry,@GeneralPOL,@GeneralPOLCode,@GeneralPOD,@GeneralAccountHolder,@GeneralSplitItem,@GeneralConsoleValidUpto,@GeneralDONo,@GeneralDODate,@GeneralDOValidDate,@GeneralSMTP,@GeneralCustomChallanNo,@GeneralDutyValue,@GeneralVolume,@GeneralAssessableValue,@GeneralCommodity,@GeneralDefaultCustomExamPerc,@GeneralFromLocation,@GeneralPackageType,@GeneralForwarder,@GeneralNominatedCustomer,@GeneralBacktoPort,@GeneralBacktoPortRemarks,@GeneralTransportation,@GeneralGateInType,@GeneralShippingLine)", cnn);

            cmd.Parameters.AddWithValue("@CompanyName", MainTableDataList[0].MainCompanyName.Trim());
            cmd.Parameters.AddWithValue("@BranchName", MainTableDataList[0].MainBranchName.Trim());
            cmd.Parameters.AddWithValue("@JobNoPreFix", JobNoPreFix);
            cmd.Parameters.AddWithValue("@JobNoSeries", JobNoSeries);
            cmd.Parameters.AddWithValue("@FinancialYear", FinancialYear);
            cmd.Parameters.AddWithValue("@MainJobNo", RefNo);
            cmd.Parameters.AddWithValue("@MainIGMNo", MainTableDataList[0].MainIGMNo.Trim());
            cmd.Parameters.AddWithValue("@MainItemNo", MainTableDataList[0].MainItemNo.Trim());
            if (MainTableDataList[0].MainIGMDate == "" || MainTableDataList[0].MainIGMDate == null) { cmd.Parameters.AddWithValue("@MainIGMDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@MainIGMDate", Convert.ToDateTime(MainTableDataList[0].MainIGMDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@MainBookingNo", MainTableDataList[0].MainBookingNo.Trim());
            if (MainTableDataList[0].MainBookingDate == "" || MainTableDataList[0].MainBookingDate == null) { cmd.Parameters.AddWithValue("@MainBookingDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@MainBookingDate", Convert.ToDateTime(MainTableDataList[0].MainBookingDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@MainVesselName", MainTableDataList[0].MainVesselName.Trim());
            cmd.Parameters.AddWithValue("@MainPOA", MainTableDataList[0].MainPOA.Trim());
            cmd.Parameters.AddWithValue("@MainVIANo", MainTableDataList[0].MainVIANo.Trim());
            cmd.Parameters.AddWithValue("@MainVoyNo", MainTableDataList[0].MainVoyNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobOwner", MainTableDataList[0].MainJobOwner.Trim());
            cmd.Parameters.AddWithValue("@MainIGMUpload", MainTableDataList[0].MainIGMUpload.Trim());
            if (MainTableDataList[0].GeneralDOA == "" || MainTableDataList[0].GeneralDOA == null) { cmd.Parameters.AddWithValue("@GeneralDOA", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralDOA", Convert.ToDateTime(MainTableDataList[0].GeneralDOA.Trim())); }
            cmd.Parameters.AddWithValue("@GeneralServices", MainTableDataList[0].GeneralServices.Trim());
            cmd.Parameters.AddWithValue("@GeneralTPNo", MainTableDataList[0].GeneralTPNo.Trim());
            if (MainTableDataList[0].GeneralTPDate == "" || MainTableDataList[0].GeneralTPDate == null) { cmd.Parameters.AddWithValue("@GeneralTPDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralTPDate", Convert.ToDateTime(MainTableDataList[0].GeneralTPDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralConsoler", MainTableDataList[0].GeneralConsoler.Trim());
            if (MainTableDataList[0].GeneralBerthingDate == "" || MainTableDataList[0].GeneralBerthingDate == null) { cmd.Parameters.AddWithValue("@GeneralBerthingDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralBerthingDate", Convert.ToDateTime(MainTableDataList[0].GeneralBerthingDate.Trim()).Date); }
            if (MainTableDataList[0].GeneralDateofDeparture == "" || MainTableDataList[0].GeneralDateofDeparture == null) { cmd.Parameters.AddWithValue("@GeneralDateofDeparture", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralDateofDeparture", Convert.ToDateTime(MainTableDataList[0].GeneralDateofDeparture.Trim()).Date); }
            if (MainTableDataList[0].GeneralCutoffDate == "" || MainTableDataList[0].GeneralCutoffDate == null) { cmd.Parameters.AddWithValue("@GeneralCutoffDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralCutoffDate", Convert.ToDateTime(MainTableDataList[0].GeneralCutoffDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralRotationNo", MainTableDataList[0].GeneralRotationNo.Trim());
            if (MainTableDataList[0].GeneralGateOpenDate == "" || MainTableDataList[0].GeneralGateOpenDate == null) { cmd.Parameters.AddWithValue("@GeneralGateOpenDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralGateOpenDate", Convert.ToDateTime(MainTableDataList[0].GeneralGateOpenDate.Trim()).Date); }
            if (MainTableDataList[0].GeneralScanlistRecvDate == "" || MainTableDataList[0].GeneralScanlistRecvDate == null) { cmd.Parameters.AddWithValue("@GeneralScanlistRecvDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralScanlistRecvDate", Convert.ToDateTime(MainTableDataList[0].GeneralScanlistRecvDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralBOENo", MainTableDataList[0].GeneralBOENo.Trim());
            if (MainTableDataList[0].GeneralBOEDate == "" || MainTableDataList[0].GeneralBOEDate == null) { cmd.Parameters.AddWithValue("@GeneralBOEDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralBOEDate", Convert.ToDateTime(MainTableDataList[0].GeneralBOEDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralLineName", MainTableDataList[0].GeneralLineName.Trim());
            cmd.Parameters.AddWithValue("@GeneralEnblock", MainTableDataList[0].GeneralEnblock.Trim());
            cmd.Parameters.AddWithValue("@GeneralGCRMSStatus", MainTableDataList[0].GeneralGCRMSStatus.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOLCountry", MainTableDataList[0].GeneralPOLCountry.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOL", MainTableDataList[0].GeneralPOL.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOLCode", MainTableDataList[0].GeneralPOLCode.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOD", MainTableDataList[0].GeneralPOD.Trim());
            cmd.Parameters.AddWithValue("@GeneralAccountHolder", MainTableDataList[0].GeneralAccountHolder.Trim());
            cmd.Parameters.AddWithValue("@GeneralSplitItem", MainTableDataList[0].GeneralSplitItem.Trim());
            if (MainTableDataList[0].GeneralConsoleValidUpto == "" || MainTableDataList[0].GeneralConsoleValidUpto == null) { cmd.Parameters.AddWithValue("@GeneralConsoleValidUpto", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralConsoleValidUpto", Convert.ToDateTime(MainTableDataList[0].GeneralConsoleValidUpto.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralDONo", MainTableDataList[0].GeneralDONo.Trim());
            if (MainTableDataList[0].GeneralDODate == "" || MainTableDataList[0].GeneralDODate == null) { cmd.Parameters.AddWithValue("@GeneralDODate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralDODate", Convert.ToDateTime(MainTableDataList[0].GeneralDODate.Trim()).Date); }
            if (MainTableDataList[0].GeneralDOValidDate == "" || MainTableDataList[0].GeneralDOValidDate == null) { cmd.Parameters.AddWithValue("@GeneralDOValidDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@GeneralDOValidDate", Convert.ToDateTime(MainTableDataList[0].GeneralDOValidDate.Trim()).Date); }
            cmd.Parameters.AddWithValue("@GeneralSMTP", MainTableDataList[0].GeneralSMTP.Trim());
            cmd.Parameters.AddWithValue("@GeneralCustomChallanNo", MainTableDataList[0].GeneralCustomChallanNo.Trim());
            cmd.Parameters.AddWithValue("@GeneralDutyValue", MainTableDataList[0].GeneralDutyValue.Trim());
            cmd.Parameters.AddWithValue("@GeneralVolume", MainTableDataList[0].GeneralVolume.Trim());
            cmd.Parameters.AddWithValue("@GeneralAssessableValue", MainTableDataList[0].GeneralAssessableValue.Trim());
            cmd.Parameters.AddWithValue("@GeneralCommodity", MainTableDataList[0].GeneralCommodity.Trim());
            cmd.Parameters.AddWithValue("@GeneralDefaultCustomExamPerc", MainTableDataList[0].GeneralDefaultCustomExamPerc.Trim());
            cmd.Parameters.AddWithValue("@GeneralFromLocation", MainTableDataList[0].GeneralFromLocation.Trim());
            cmd.Parameters.AddWithValue("@GeneralPackageType", MainTableDataList[0].GeneralPackageType.Trim());
            cmd.Parameters.AddWithValue("@GeneralForwarder", MainTableDataList[0].GeneralForwarder.Trim());
            cmd.Parameters.AddWithValue("@GeneralNominatedCustomer", MainTableDataList[0].GeneralNominatedCustomer.Trim());
            cmd.Parameters.AddWithValue("@GeneralBacktoPort", MainTableDataList[0].GeneralBacktoPort.Trim());
            cmd.Parameters.AddWithValue("@GeneralBacktoPortRemarks", MainTableDataList[0].GeneralBacktoPortRemarks.Trim());
            cmd.Parameters.AddWithValue("@GeneralTransportation", MainTableDataList[0].GeneralTransportation.Trim());
            cmd.Parameters.AddWithValue("@GeneralGateInType", MainTableDataList[0].GeneralGateInType.Trim());
            cmd.Parameters.AddWithValue("@GeneralShippingLine", MainTableDataList[0].GeneralShippingLine.Trim());
            cmd.Parameters.AddWithValue("@ub", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImport", RefNo, "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class MainTableDataList
    {
        public string MainJobNo { get; set; }
        public string MainJobDate { get; set; }
        public string MainIGMNo { get; set; }
        public string MainItemNo { get; set; }
        public string MainIGMDate { get; set; }
        public string MainBookingNo { get; set; }
        public string MainBookingDate { get; set; }
        public string MainVesselName { get; set; }
        public string MainPOA { get; set; }
        public string MainVIANo { get; set; }
        public string MainVoyNo { get; set; }
        public string MainJobOwner { get; set; }
        public string MainIGMUpload { get; set; }
        public string GeneralDOA { get; set; }
        public string GeneralServices { get; set; }
        public string GeneralTPNo { get; set; }
        public string GeneralTPDate { get; set; }
        public string GeneralConsoler { get; set; }
        public string GeneralBerthingDate { get; set; }
        public string GeneralDateofDeparture { get; set; }
        public string GeneralCutoffDate { get; set; }
        public string GeneralRotationNo { get; set; }
        public string GeneralGateOpenDate { get; set; }
        public string GeneralScanlistRecvDate { get; set; }
        public string GeneralBOENo { get; set; }
        public string GeneralBOEDate { get; set; }
        public string GeneralLineName { get; set; }
        public string GeneralEnblock { get; set; }
        public string GeneralGCRMSStatus { get; set; }
        public string GeneralPOLCountry { get; set; }
        public string GeneralPOL { get; set; }
        public string GeneralPOLCode { get; set; }
        public string GeneralPOD { get; set; }
        public string GeneralAccountHolder { get; set; }
        public string GeneralSplitItem { get; set; }
        public string GeneralConsoleValidUpto { get; set; }
        public string GeneralDONo { get; set; }
        public string GeneralDODate { get; set; }
        public string GeneralDOValidDate { get; set; }
        public string GeneralSMTP { get; set; }
        public string GeneralCustomChallanNo { get; set; }
        public string GeneralDutyValue { get; set; }
        public string GeneralVolume { get; set; }
        public string GeneralAssessableValue { get; set; }
        public string GeneralCommodity { get; set; }
        public string GeneralDefaultCustomExamPerc { get; set; }
        public string GeneralFromLocation { get; set; }
        public string GeneralPackageType { get; set; }
        public string GeneralForwarder { get; set; }
        public string GeneralNominatedCustomer { get; set; }
        public string GeneralBacktoPort { get; set; }
        public string GeneralBacktoPortRemarks { get; set; }
        public string GeneralTransportation { get; set; }
        public string GeneralGateInType { get; set; }
        public string GeneralShippingLine { get; set; }
        public string ReturnedValue { get; set; }
        public string MainPassingType { get; set; }
        public string MainCompanyName { get; set; }
        public string MainBranchName { get; set; }
    }
    //-----------------------------------------------------------MainTableUpdateDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string MainTableUpdateData(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<MainTableDataList> DetailedList = new List<MainTableDataList>();
        MainTableDataList DetailedListValue = new MainTableDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("SELECT * FROM CFSImport WHERE MainJobNo = @MainJobNo and RecordStatus='Active'", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", MainTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.MainJobNo = dtrow["MainJobNo"].ToString();
                DetailedListValue.MainJobDate = dtrow["MainJobDate"].ToString();
                if (dtrow["MainJobDate"].ToString() != "" || dtrow["MainJobDate"] != DBNull.Value) { DetailedListValue.MainJobDate = Convert.ToDateTime(dtrow["MainJobDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.MainJobDate = ""; }
                DetailedListValue.MainIGMNo = dtrow["MainIGMNo"].ToString();
                DetailedListValue.MainItemNo = dtrow["MainItemNo"].ToString();
                DetailedListValue.MainIGMDate = dtrow["MainIGMDate"].ToString();
                if (dtrow["MainIGMDate"].ToString() != "" || dtrow["MainIGMDate"] != DBNull.Value) { DetailedListValue.MainIGMDate = Convert.ToDateTime(dtrow["MainIGMDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.MainIGMDate = ""; }
                DetailedListValue.MainBookingNo = dtrow["MainBookingNo"].ToString();
                DetailedListValue.MainBookingDate = dtrow["MainBookingDate"].ToString();
                if (dtrow["MainBookingDate"].ToString() != "" || dtrow["MainBookingDate"] != DBNull.Value) { DetailedListValue.MainBookingDate = Convert.ToDateTime(dtrow["MainBookingDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.MainBookingDate = ""; }
                DetailedListValue.MainVesselName = dtrow["MainVesselName"].ToString();
                DetailedListValue.MainPOA = dtrow["MainPOA"].ToString();
                DetailedListValue.MainVIANo = dtrow["MainVIANo"].ToString();
                DetailedListValue.MainVoyNo = dtrow["MainVoyNo"].ToString();
                DetailedListValue.MainJobOwner = dtrow["MainJobOwner"].ToString();
                DetailedListValue.MainIGMUpload = dtrow["MainIGMUpload"].ToString();
                DetailedListValue.GeneralDOA = dtrow["GeneralDOA"].ToString();
                if (dtrow["GeneralDOA"].ToString() != "" || dtrow["GeneralDOA"] != DBNull.Value) { DetailedListValue.GeneralDOA = Convert.ToDateTime(dtrow["GeneralDOA"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralDOA = ""; }
                DetailedListValue.GeneralServices = dtrow["GeneralServices"].ToString();
                DetailedListValue.GeneralTPNo = dtrow["GeneralTPNo"].ToString();
                DetailedListValue.GeneralTPDate = dtrow["GeneralTPDate"].ToString();
                if (dtrow["GeneralTPDate"].ToString() != "" || dtrow["GeneralTPDate"] != DBNull.Value) { DetailedListValue.GeneralTPDate = Convert.ToDateTime(dtrow["GeneralTPDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralTPDate = ""; }
                DetailedListValue.GeneralConsoler = dtrow["GeneralConsoler"].ToString();
                DetailedListValue.GeneralBerthingDate = dtrow["GeneralBerthingDate"].ToString();
                if (dtrow["GeneralBerthingDate"].ToString() != "" || dtrow["GeneralBerthingDate"] != DBNull.Value) { DetailedListValue.GeneralBerthingDate = Convert.ToDateTime(dtrow["GeneralBerthingDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralBerthingDate = ""; }
                DetailedListValue.GeneralDateofDeparture = dtrow["GeneralDateofDeparture"].ToString();
                if (dtrow["GeneralDateofDeparture"].ToString() != "" || dtrow["GeneralDateofDeparture"] != DBNull.Value) { DetailedListValue.GeneralDateofDeparture = Convert.ToDateTime(dtrow["GeneralDateofDeparture"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralDateofDeparture = ""; }
                DetailedListValue.GeneralCutoffDate = dtrow["GeneralCutoffDate"].ToString();
                if (dtrow["GeneralCutoffDate"].ToString() != "" || dtrow["GeneralCutoffDate"] != DBNull.Value) { DetailedListValue.GeneralCutoffDate = Convert.ToDateTime(dtrow["GeneralCutoffDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralCutoffDate = ""; }
                DetailedListValue.GeneralRotationNo = dtrow["GeneralRotationNo"].ToString();
                DetailedListValue.GeneralGateOpenDate = dtrow["GeneralGateOpenDate"].ToString();
                if (dtrow["GeneralGateOpenDate"].ToString() != "" || dtrow["GeneralGateOpenDate"] != DBNull.Value) { DetailedListValue.GeneralGateOpenDate = Convert.ToDateTime(dtrow["GeneralGateOpenDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralGateOpenDate = ""; }
                DetailedListValue.GeneralScanlistRecvDate = dtrow["GeneralScanlistRecvDate"].ToString();
                if (dtrow["GeneralScanlistRecvDate"].ToString() != "" || dtrow["GeneralScanlistRecvDate"] != DBNull.Value) { DetailedListValue.GeneralScanlistRecvDate = Convert.ToDateTime(dtrow["GeneralScanlistRecvDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralScanlistRecvDate = ""; }
                DetailedListValue.GeneralBOENo = dtrow["GeneralBOENo"].ToString();
                DetailedListValue.GeneralBOEDate = dtrow["GeneralBOEDate"].ToString();
                if (dtrow["GeneralBOEDate"].ToString() != "" || dtrow["GeneralBOEDate"] != DBNull.Value) { DetailedListValue.GeneralBOEDate = Convert.ToDateTime(dtrow["GeneralBOEDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralBOEDate = ""; }
                DetailedListValue.GeneralLineName = dtrow["GeneralLineName"].ToString();
                DetailedListValue.GeneralEnblock = dtrow["GeneralEnblock"].ToString();
                DetailedListValue.GeneralGCRMSStatus = dtrow["GeneralGCRMSStatus"].ToString();
                DetailedListValue.GeneralPOLCountry = dtrow["GeneralPOLCountry"].ToString();
                DetailedListValue.GeneralPOL = dtrow["GeneralPOL"].ToString();
                DetailedListValue.GeneralPOLCode = dtrow["GeneralPOLCode"].ToString();
                DetailedListValue.GeneralPOD = dtrow["GeneralPOD"].ToString();
                DetailedListValue.GeneralAccountHolder = dtrow["GeneralAccountHolder"].ToString();
                DetailedListValue.GeneralSplitItem = dtrow["GeneralSplitItem"].ToString();
                DetailedListValue.GeneralConsoleValidUpto = dtrow["GeneralConsoleValidUpto"].ToString();
                if (dtrow["GeneralConsoleValidUpto"].ToString() != "" || dtrow["GeneralConsoleValidUpto"] != DBNull.Value) { DetailedListValue.GeneralConsoleValidUpto = Convert.ToDateTime(dtrow["GeneralConsoleValidUpto"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralConsoleValidUpto = ""; }
                DetailedListValue.GeneralDONo = dtrow["GeneralDONo"].ToString();
                DetailedListValue.GeneralDODate = dtrow["GeneralDODate"].ToString();
                if (dtrow["GeneralDODate"].ToString() != "" || dtrow["GeneralDODate"] != DBNull.Value) { DetailedListValue.GeneralDODate = Convert.ToDateTime(dtrow["GeneralDODate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralDODate = ""; }
                DetailedListValue.GeneralDOValidDate = dtrow["GeneralDOValidDate"].ToString();
                if (dtrow["GeneralDOValidDate"].ToString() != "" || dtrow["GeneralDOValidDate"] != DBNull.Value) { DetailedListValue.GeneralDOValidDate = Convert.ToDateTime(dtrow["GeneralDOValidDate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                else { DetailedListValue.GeneralDOValidDate = ""; }
                DetailedListValue.GeneralSMTP = dtrow["GeneralSMTP"].ToString();
                DetailedListValue.GeneralCustomChallanNo = dtrow["GeneralCustomChallanNo"].ToString();
                DetailedListValue.GeneralDutyValue = dtrow["GeneralDutyValue"].ToString();
                DetailedListValue.GeneralVolume = dtrow["GeneralVolume"].ToString();
                DetailedListValue.GeneralAssessableValue = dtrow["GeneralAssessableValue"].ToString();
                DetailedListValue.GeneralCommodity = dtrow["GeneralCommodity"].ToString();
                DetailedListValue.GeneralDefaultCustomExamPerc = dtrow["GeneralDefaultCustomExamPerc"].ToString();
                DetailedListValue.GeneralFromLocation = dtrow["GeneralFromLocation"].ToString();
                DetailedListValue.GeneralPackageType = dtrow["GeneralPackageType"].ToString();
                DetailedListValue.GeneralForwarder = dtrow["GeneralForwarder"].ToString();
                DetailedListValue.GeneralNominatedCustomer = dtrow["GeneralNominatedCustomer"].ToString();
                DetailedListValue.GeneralBacktoPort = dtrow["GeneralBacktoPort"].ToString();
                DetailedListValue.GeneralBacktoPortRemarks = dtrow["GeneralBacktoPortRemarks"].ToString();
                DetailedListValue.GeneralTransportation = dtrow["GeneralTransportation"].ToString();
                DetailedListValue.GeneralGateInType = dtrow["GeneralGateInType"].ToString();
                DetailedListValue.GeneralShippingLine = dtrow["GeneralShippingLine"].ToString();
                DetailedListValue.MainCompanyName = dtrow["CompanyName"].ToString();
                DetailedListValue.MainBranchName = dtrow["BranchName"].ToString();
            }
            cnn.Close();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImport set CompanyName=@CompanyName,BranchName=@BranchName, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, MainJobNo=@MainJobNo,MainJobDate=@MainJobDate,MainIGMNo=@MainIGMNo,MainItemNo=@MainItemNo,MainIGMDate=@MainIGMDate,MainBookingNo=@MainBookingNo,MainBookingDate=@MainBookingDate,MainVesselName=@MainVesselName,MainPOA=@MainPOA,MainVIANo=@MainVIANo,MainVoyNo=@MainVoyNo,MainJobOwner=@MainJobOwner,MainIGMUpload=@MainIGMUpload,GeneralDOA=@GeneralDOA,GeneralServices=@GeneralServices,GeneralTPNo=@GeneralTPNo,GeneralTPDate=@GeneralTPDate,GeneralConsoler=@GeneralConsoler,GeneralBerthingDate=@GeneralBerthingDate,GeneralDateofDeparture=@GeneralDateofDeparture,GeneralCutoffDate=@GeneralCutoffDate,GeneralRotationNo=@GeneralRotationNo,GeneralGateOpenDate=@GeneralGateOpenDate,GeneralScanlistRecvDate=@GeneralScanlistRecvDate,GeneralBOENo=@GeneralBOENo,GeneralBOEDate=@GeneralBOEDate,GeneralLineName=@GeneralLineName,GeneralEnblock=@GeneralEnblock,GeneralGCRMSStatus=@GeneralGCRMSStatus,GeneralPOLCountry=@GeneralPOLCountry,GeneralPOL=@GeneralPOL,GeneralPOLCode=@GeneralPOLCode,GeneralPOD=@GeneralPOD,GeneralAccountHolder=@GeneralAccountHolder,GeneralSplitItem=@GeneralSplitItem,GeneralConsoleValidUpto=@GeneralConsoleValidUpto,GeneralDONo=@GeneralDONo,GeneralDODate=@GeneralDODate,GeneralDOValidDate=@GeneralDOValidDate,GeneralSMTP=@GeneralSMTP,GeneralCustomChallanNo=@GeneralCustomChallanNo,GeneralDutyValue=@GeneralDutyValue,GeneralVolume=@GeneralVolume,GeneralAssessableValue=@GeneralAssessableValue,GeneralCommodity=@GeneralCommodity,GeneralDefaultCustomExamPerc=@GeneralDefaultCustomExamPerc,GeneralFromLocation=@GeneralFromLocation,GeneralPackageType=@GeneralPackageType,GeneralForwarder=@GeneralForwarder,GeneralNominatedCustomer=@GeneralNominatedCustomer,GeneralBacktoPort=@GeneralBacktoPort,GeneralBacktoPortRemarks=@GeneralBacktoPortRemarks,GeneralTransportation=@GeneralTransportation,GeneralGateInType=@GeneralGateInType,GeneralShippingLine=@GeneralShippingLine where MainJobNo = @MainJobNo", cnn);
            cmd.Parameters.AddWithValue("@MainJobNo", MainTableDataList[0].MainJobNo.Trim());
            if (MainTableDataList[0].MainJobDate == "" || MainTableDataList[0].MainJobDate == null)
            {
                cmd.Parameters.AddWithValue("@MainJobDate", DBNull.Value);
                MainTableDataList[0].MainJobDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("MainJobDate", Convert.ToDateTime(MainTableDataList[0].MainJobDate.Trim()).Date);
                MainTableDataList[0].MainJobDate = Convert.ToDateTime(MainTableDataList[0].MainJobDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@MainIGMNo", MainTableDataList[0].MainIGMNo.Trim());
            cmd.Parameters.AddWithValue("@MainItemNo", MainTableDataList[0].MainItemNo.Trim());
            if (MainTableDataList[0].MainIGMDate == "" || MainTableDataList[0].MainIGMDate == null)
            {
                cmd.Parameters.AddWithValue("@MainIGMDate", DBNull.Value);
                MainTableDataList[0].MainIGMDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("MainIGMDate", Convert.ToDateTime(MainTableDataList[0].MainIGMDate.Trim()).Date);
                MainTableDataList[0].MainIGMDate = Convert.ToDateTime(MainTableDataList[0].MainIGMDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@MainBookingNo", MainTableDataList[0].MainBookingNo.Trim());
            if (MainTableDataList[0].MainBookingDate == "" || MainTableDataList[0].MainBookingDate == null)
            {
                cmd.Parameters.AddWithValue("@MainBookingDate", DBNull.Value);
                MainTableDataList[0].MainBookingDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("MainBookingDate", Convert.ToDateTime(MainTableDataList[0].MainBookingDate.Trim()).Date);
                MainTableDataList[0].MainBookingDate = Convert.ToDateTime(MainTableDataList[0].MainBookingDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@MainVesselName", MainTableDataList[0].MainVesselName.Trim());
            cmd.Parameters.AddWithValue("@MainPOA", MainTableDataList[0].MainPOA.Trim());
            cmd.Parameters.AddWithValue("@MainVIANo", MainTableDataList[0].MainVIANo.Trim());
            cmd.Parameters.AddWithValue("@MainVoyNo", MainTableDataList[0].MainVoyNo.Trim());
            cmd.Parameters.AddWithValue("@MainJobOwner", MainTableDataList[0].MainJobOwner.Trim());
            cmd.Parameters.AddWithValue("@MainIGMUpload", MainTableDataList[0].MainIGMUpload.Trim());
            if (MainTableDataList[0].GeneralDOA == "" || MainTableDataList[0].GeneralDOA == null)
            {
                cmd.Parameters.AddWithValue("@GeneralDOA", DBNull.Value);
                MainTableDataList[0].GeneralDOA = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralDOA", Convert.ToDateTime(MainTableDataList[0].GeneralDOA.Trim()));
                MainTableDataList[0].GeneralDOA = Convert.ToDateTime(MainTableDataList[0].GeneralDOA).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralServices", MainTableDataList[0].GeneralServices.Trim());
            cmd.Parameters.AddWithValue("@GeneralTPNo", MainTableDataList[0].GeneralTPNo.Trim());
            if (MainTableDataList[0].GeneralTPDate == "" || MainTableDataList[0].GeneralTPDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralTPDate", DBNull.Value);
                MainTableDataList[0].GeneralTPDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralTPDate", Convert.ToDateTime(MainTableDataList[0].GeneralTPDate.Trim()).Date);
                MainTableDataList[0].GeneralTPDate = Convert.ToDateTime(MainTableDataList[0].GeneralTPDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralConsoler", MainTableDataList[0].GeneralConsoler.Trim());
            if (MainTableDataList[0].GeneralBerthingDate == "" || MainTableDataList[0].GeneralBerthingDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralBerthingDate", DBNull.Value);
                MainTableDataList[0].GeneralBerthingDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralBerthingDate", Convert.ToDateTime(MainTableDataList[0].GeneralBerthingDate.Trim()).Date);
                MainTableDataList[0].GeneralBerthingDate = Convert.ToDateTime(MainTableDataList[0].GeneralBerthingDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (MainTableDataList[0].GeneralDateofDeparture == "" || MainTableDataList[0].GeneralDateofDeparture == null)
            {
                cmd.Parameters.AddWithValue("@GeneralDateofDeparture", DBNull.Value);
                MainTableDataList[0].GeneralDateofDeparture = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralDateofDeparture", Convert.ToDateTime(MainTableDataList[0].GeneralDateofDeparture.Trim()).Date);
                MainTableDataList[0].GeneralDateofDeparture = Convert.ToDateTime(MainTableDataList[0].GeneralDateofDeparture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (MainTableDataList[0].GeneralCutoffDate == "" || MainTableDataList[0].GeneralCutoffDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralCutoffDate", DBNull.Value);
                MainTableDataList[0].GeneralCutoffDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralCutoffDate", Convert.ToDateTime(MainTableDataList[0].GeneralCutoffDate.Trim()).Date);
                MainTableDataList[0].GeneralCutoffDate = Convert.ToDateTime(MainTableDataList[0].GeneralCutoffDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralRotationNo", MainTableDataList[0].GeneralRotationNo.Trim());
            if (MainTableDataList[0].GeneralGateOpenDate == "" || MainTableDataList[0].GeneralGateOpenDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralGateOpenDate", DBNull.Value);
                MainTableDataList[0].GeneralGateOpenDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralGateOpenDate", Convert.ToDateTime(MainTableDataList[0].GeneralGateOpenDate.Trim()).Date);
                MainTableDataList[0].GeneralGateOpenDate = Convert.ToDateTime(MainTableDataList[0].GeneralGateOpenDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (MainTableDataList[0].GeneralScanlistRecvDate == "" || MainTableDataList[0].GeneralScanlistRecvDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralScanlistRecvDate", DBNull.Value);
                MainTableDataList[0].GeneralScanlistRecvDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralScanlistRecvDate", Convert.ToDateTime(MainTableDataList[0].GeneralScanlistRecvDate.Trim()).Date);
                MainTableDataList[0].GeneralScanlistRecvDate = Convert.ToDateTime(MainTableDataList[0].GeneralScanlistRecvDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralBOENo", MainTableDataList[0].GeneralBOENo.Trim());
            if (MainTableDataList[0].GeneralBOEDate == "" || MainTableDataList[0].GeneralBOEDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralBOEDate", DBNull.Value);
                MainTableDataList[0].GeneralBOEDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralBOEDate", Convert.ToDateTime(MainTableDataList[0].GeneralBOEDate.Trim()).Date);
                MainTableDataList[0].GeneralBOEDate = Convert.ToDateTime(MainTableDataList[0].GeneralBOEDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralLineName", MainTableDataList[0].GeneralLineName.Trim());
            cmd.Parameters.AddWithValue("@GeneralEnblock", MainTableDataList[0].GeneralEnblock.Trim());
            cmd.Parameters.AddWithValue("@GeneralGCRMSStatus", MainTableDataList[0].GeneralGCRMSStatus.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOLCountry", MainTableDataList[0].GeneralPOLCountry.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOL", MainTableDataList[0].GeneralPOL.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOLCode", MainTableDataList[0].GeneralPOLCode.Trim());
            cmd.Parameters.AddWithValue("@GeneralPOD", MainTableDataList[0].GeneralPOD.Trim());
            cmd.Parameters.AddWithValue("@GeneralAccountHolder", MainTableDataList[0].GeneralAccountHolder.Trim());
            cmd.Parameters.AddWithValue("@GeneralSplitItem", MainTableDataList[0].GeneralSplitItem.Trim());
            if (MainTableDataList[0].GeneralConsoleValidUpto == "" || MainTableDataList[0].GeneralConsoleValidUpto == null)
            {
                cmd.Parameters.AddWithValue("@GeneralConsoleValidUpto", DBNull.Value);
                MainTableDataList[0].GeneralConsoleValidUpto = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralConsoleValidUpto", Convert.ToDateTime(MainTableDataList[0].GeneralConsoleValidUpto.Trim()).Date);
                MainTableDataList[0].GeneralConsoleValidUpto = Convert.ToDateTime(MainTableDataList[0].GeneralConsoleValidUpto).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralDONo", MainTableDataList[0].GeneralDONo.Trim());
            if (MainTableDataList[0].GeneralDODate == "" || MainTableDataList[0].GeneralDODate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralDODate", DBNull.Value);
                MainTableDataList[0].GeneralDODate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralDODate", Convert.ToDateTime(MainTableDataList[0].GeneralDODate.Trim()).Date);
                MainTableDataList[0].GeneralDODate = Convert.ToDateTime(MainTableDataList[0].GeneralDODate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (MainTableDataList[0].GeneralDOValidDate == "" || MainTableDataList[0].GeneralDOValidDate == null)
            {
                cmd.Parameters.AddWithValue("@GeneralDOValidDate", DBNull.Value);
                MainTableDataList[0].GeneralDOValidDate = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("GeneralDOValidDate", Convert.ToDateTime(MainTableDataList[0].GeneralDOValidDate.Trim()).Date);
                MainTableDataList[0].GeneralDOValidDate = Convert.ToDateTime(MainTableDataList[0].GeneralDOValidDate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            cmd.Parameters.AddWithValue("@GeneralSMTP", MainTableDataList[0].GeneralSMTP.Trim());
            cmd.Parameters.AddWithValue("@GeneralCustomChallanNo", MainTableDataList[0].GeneralCustomChallanNo.Trim());
            cmd.Parameters.AddWithValue("@GeneralDutyValue", MainTableDataList[0].GeneralDutyValue.Trim());
            cmd.Parameters.AddWithValue("@GeneralVolume", MainTableDataList[0].GeneralVolume.Trim());
            cmd.Parameters.AddWithValue("@GeneralAssessableValue", MainTableDataList[0].GeneralAssessableValue.Trim());
            cmd.Parameters.AddWithValue("@GeneralCommodity", MainTableDataList[0].GeneralCommodity.Trim());
            cmd.Parameters.AddWithValue("@GeneralDefaultCustomExamPerc", MainTableDataList[0].GeneralDefaultCustomExamPerc.Trim());
            cmd.Parameters.AddWithValue("@GeneralFromLocation", MainTableDataList[0].GeneralFromLocation.Trim());
            cmd.Parameters.AddWithValue("@GeneralPackageType", MainTableDataList[0].GeneralPackageType.Trim());
            cmd.Parameters.AddWithValue("@GeneralForwarder", MainTableDataList[0].GeneralForwarder.Trim());
            cmd.Parameters.AddWithValue("@GeneralNominatedCustomer", MainTableDataList[0].GeneralNominatedCustomer.Trim());
            cmd.Parameters.AddWithValue("@GeneralBacktoPort", MainTableDataList[0].GeneralBacktoPort.Trim());
            cmd.Parameters.AddWithValue("@GeneralBacktoPortRemarks", MainTableDataList[0].GeneralBacktoPortRemarks.Trim());
            cmd.Parameters.AddWithValue("@GeneralTransportation", MainTableDataList[0].GeneralTransportation.Trim());
            cmd.Parameters.AddWithValue("@GeneralGateInType", MainTableDataList[0].GeneralGateInType.Trim());
            cmd.Parameters.AddWithValue("@GeneralShippingLine", MainTableDataList[0].GeneralShippingLine.Trim());
            cmd.Parameters.AddWithValue("@CompanyName", MainTableDataList[0].MainCompanyName.Trim());
            cmd.Parameters.AddWithValue("@BranchName", MainTableDataList[0].MainBranchName.Trim());
            cmd.Parameters.AddWithValue("@ComputerName", "");
            cmd.Parameters.AddWithValue("@IPAddress", "");
            cmd.Parameters.AddWithValue("@Location", "");
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            foreach (DataRow dtrow in dt.Rows)
            {
                if (DetailedListValue.MainJobNo != MainTableDataList[0].MainJobNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainJobNo", DetailedListValue.MainJobNo,
                    MainTableDataList[0].MainJobNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainJobDate != MainTableDataList[0].MainJobDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainJobDate", DetailedListValue.MainJobDate,
                    MainTableDataList[0].MainJobDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainIGMNo != MainTableDataList[0].MainIGMNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainIGMNo", DetailedListValue.MainIGMNo,
                    MainTableDataList[0].MainIGMNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainItemNo != MainTableDataList[0].MainItemNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainItemNo", DetailedListValue.MainItemNo,
                    MainTableDataList[0].MainItemNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainIGMDate != MainTableDataList[0].MainIGMDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainIGMDate", DetailedListValue.MainIGMDate,
                    MainTableDataList[0].MainIGMDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainBookingNo != MainTableDataList[0].MainBookingNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainBookingNo", DetailedListValue.MainBookingNo,
                    MainTableDataList[0].MainBookingNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainBookingDate != MainTableDataList[0].MainBookingDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainBookingDate", DetailedListValue.MainBookingDate,
                    MainTableDataList[0].MainBookingDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainVesselName != MainTableDataList[0].MainVesselName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainVesselName", DetailedListValue.MainVesselName,
                    MainTableDataList[0].MainVesselName.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainPOA != MainTableDataList[0].MainPOA.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainPOA", DetailedListValue.MainPOA,
                    MainTableDataList[0].MainPOA.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainVIANo != MainTableDataList[0].MainVIANo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainVIANo", DetailedListValue.MainVIANo,
                    MainTableDataList[0].MainVIANo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainVoyNo != MainTableDataList[0].MainVoyNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainVoyNo", DetailedListValue.MainVoyNo,
                    MainTableDataList[0].MainVoyNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainJobOwner != MainTableDataList[0].MainJobOwner.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainJobOwner", DetailedListValue.MainJobOwner,
                    MainTableDataList[0].MainJobOwner.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainIGMUpload != MainTableDataList[0].MainIGMUpload.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].MainJobNo.Trim(), "MainIGMUpload", DetailedListValue.MainIGMUpload,
                    MainTableDataList[0].MainIGMUpload.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDOA != MainTableDataList[0].GeneralDOA.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDOA", DetailedListValue.GeneralDOA,
                    MainTableDataList[0].GeneralDOA.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralServices != MainTableDataList[0].GeneralServices.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralServices", DetailedListValue.GeneralServices,
                    MainTableDataList[0].GeneralServices.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralTPNo != MainTableDataList[0].GeneralTPNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralTPNo", DetailedListValue.GeneralTPNo,
                    MainTableDataList[0].GeneralTPNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralTPDate != MainTableDataList[0].GeneralTPDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralTPDate", DetailedListValue.GeneralTPDate,
                    MainTableDataList[0].GeneralTPDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralConsoler != MainTableDataList[0].GeneralConsoler.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralConsoler", DetailedListValue.GeneralConsoler,
                    MainTableDataList[0].GeneralConsoler.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralBerthingDate != MainTableDataList[0].GeneralBerthingDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralBerthingDate", DetailedListValue.GeneralBerthingDate,
                    MainTableDataList[0].GeneralBerthingDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDateofDeparture != MainTableDataList[0].GeneralDateofDeparture.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDateofDeparture", DetailedListValue.GeneralDateofDeparture,
                    MainTableDataList[0].GeneralDateofDeparture.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralCutoffDate != MainTableDataList[0].GeneralCutoffDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralCutoffDate", DetailedListValue.GeneralCutoffDate,
                    MainTableDataList[0].GeneralCutoffDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralRotationNo != MainTableDataList[0].GeneralRotationNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralRotationNo", DetailedListValue.GeneralRotationNo,
                    MainTableDataList[0].GeneralRotationNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralGateOpenDate != MainTableDataList[0].GeneralGateOpenDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralGateOpenDate", DetailedListValue.GeneralGateOpenDate,
                    MainTableDataList[0].GeneralGateOpenDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralScanlistRecvDate != MainTableDataList[0].GeneralScanlistRecvDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralScanlistRecvDate", DetailedListValue.GeneralScanlistRecvDate,
                    MainTableDataList[0].GeneralScanlistRecvDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralBOENo != MainTableDataList[0].GeneralBOENo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralBOENo", DetailedListValue.GeneralBOENo,
                    MainTableDataList[0].GeneralBOENo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralBOEDate != MainTableDataList[0].GeneralBOEDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralBOEDate", DetailedListValue.GeneralBOEDate,
                    MainTableDataList[0].GeneralBOEDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralLineName != MainTableDataList[0].GeneralLineName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralLineName", DetailedListValue.GeneralLineName,
                    MainTableDataList[0].GeneralLineName.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralEnblock != MainTableDataList[0].GeneralEnblock.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralEnblock", DetailedListValue.GeneralEnblock,
                    MainTableDataList[0].GeneralEnblock.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralGCRMSStatus != MainTableDataList[0].GeneralGCRMSStatus.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralGCRMSStatus", DetailedListValue.GeneralGCRMSStatus,
                    MainTableDataList[0].GeneralGCRMSStatus.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralPOLCountry != MainTableDataList[0].GeneralPOLCountry.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralPOLCountry", DetailedListValue.GeneralPOLCountry,
                    MainTableDataList[0].GeneralPOLCountry.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralPOL != MainTableDataList[0].GeneralPOL.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralPOL", DetailedListValue.GeneralPOL,
                    MainTableDataList[0].GeneralPOL.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralPOLCode != MainTableDataList[0].GeneralPOLCode.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralPOLCode", DetailedListValue.GeneralPOLCode,
                    MainTableDataList[0].GeneralPOLCode.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralPOD != MainTableDataList[0].GeneralPOD.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralPOD", DetailedListValue.GeneralPOD,
                    MainTableDataList[0].GeneralPOD.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralAccountHolder != MainTableDataList[0].GeneralAccountHolder.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralAccountHolder", DetailedListValue.GeneralAccountHolder,
                    MainTableDataList[0].GeneralAccountHolder.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralSplitItem != MainTableDataList[0].GeneralSplitItem.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralSplitItem", DetailedListValue.GeneralSplitItem,
                    MainTableDataList[0].GeneralSplitItem.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralConsoleValidUpto != MainTableDataList[0].GeneralConsoleValidUpto.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralConsoleValidUpto", DetailedListValue.GeneralConsoleValidUpto,
                    MainTableDataList[0].GeneralConsoleValidUpto.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDONo != MainTableDataList[0].GeneralDONo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDONo", DetailedListValue.GeneralDONo,
                    MainTableDataList[0].GeneralDONo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDODate != MainTableDataList[0].GeneralDODate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDODate", DetailedListValue.GeneralDODate,
                    MainTableDataList[0].GeneralDODate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDOValidDate != MainTableDataList[0].GeneralDOValidDate.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDOValidDate", DetailedListValue.GeneralDOValidDate,
                    MainTableDataList[0].GeneralDOValidDate.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralSMTP != MainTableDataList[0].GeneralSMTP.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralSMTP", DetailedListValue.GeneralSMTP,
                    MainTableDataList[0].GeneralSMTP.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralCustomChallanNo != MainTableDataList[0].GeneralCustomChallanNo.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralCustomChallanNo", DetailedListValue.GeneralCustomChallanNo,
                    MainTableDataList[0].GeneralCustomChallanNo.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDutyValue != MainTableDataList[0].GeneralDutyValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDutyValue", DetailedListValue.GeneralDutyValue,
                    MainTableDataList[0].GeneralDutyValue.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralVolume != MainTableDataList[0].GeneralVolume.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralVolume", DetailedListValue.GeneralVolume,
                    MainTableDataList[0].GeneralVolume.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralAssessableValue != MainTableDataList[0].GeneralAssessableValue.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralAssessableValue", DetailedListValue.GeneralAssessableValue,
                    MainTableDataList[0].GeneralAssessableValue.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralCommodity != MainTableDataList[0].GeneralCommodity.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralCommodity", DetailedListValue.GeneralCommodity,
                    MainTableDataList[0].GeneralCommodity.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralDefaultCustomExamPerc != MainTableDataList[0].GeneralDefaultCustomExamPerc.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralDefaultCustomExamPerc", DetailedListValue.GeneralDefaultCustomExamPerc,
                    MainTableDataList[0].GeneralDefaultCustomExamPerc.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralFromLocation != MainTableDataList[0].GeneralFromLocation.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralFromLocation", DetailedListValue.GeneralFromLocation,
                    MainTableDataList[0].GeneralFromLocation.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralPackageType != MainTableDataList[0].GeneralPackageType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralPackageType", DetailedListValue.GeneralPackageType,
                    MainTableDataList[0].GeneralPackageType.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralForwarder != MainTableDataList[0].GeneralForwarder.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralForwarder", DetailedListValue.GeneralForwarder,
                    MainTableDataList[0].GeneralForwarder.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralNominatedCustomer != MainTableDataList[0].GeneralNominatedCustomer.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralNominatedCustomer", DetailedListValue.GeneralNominatedCustomer,
                    MainTableDataList[0].GeneralNominatedCustomer.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralBacktoPort != MainTableDataList[0].GeneralBacktoPort.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralBacktoPort", DetailedListValue.GeneralBacktoPort,
                    MainTableDataList[0].GeneralBacktoPort.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralBacktoPortRemarks != MainTableDataList[0].GeneralBacktoPortRemarks.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralBacktoPortRemarks", DetailedListValue.GeneralBacktoPortRemarks,
                    MainTableDataList[0].GeneralBacktoPortRemarks.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralTransportation != MainTableDataList[0].GeneralTransportation.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralTransportation", DetailedListValue.GeneralTransportation,
                    MainTableDataList[0].GeneralTransportation.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralGateInType != MainTableDataList[0].GeneralGateInType.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralGateInType", DetailedListValue.GeneralGateInType,
                    MainTableDataList[0].GeneralGateInType.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.GeneralShippingLine != MainTableDataList[0].GeneralShippingLine.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "GeneralShippingLine", DetailedListValue.GeneralShippingLine,
                    MainTableDataList[0].GeneralShippingLine.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainCompanyName != MainTableDataList[0].MainCompanyName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "MainCompanyName", DetailedListValue.MainCompanyName,
                    MainTableDataList[0].MainCompanyName.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
                if (DetailedListValue.MainBranchName != MainTableDataList[0].MainBranchName.Trim())
                {
                    InsertOrUpdateUmatchedValue("CFSImport",
                    MainTableDataList[0].GeneralDOA.Trim(), "MainBranchName", DetailedListValue.MainBranchName,
                    MainTableDataList[0].MainBranchName.Trim(), "Modified", "MainJobNo = '" + MainTableDataList[0].MainJobNo.Trim() + "' And " +
                    "GeneralDOA = '" + MainTableDataList[0].GeneralDOA.Trim() + "' And RecordStatus = 'Active'",
                    HttpContext.Current.Session["UserName"].ToString());
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------MainTableCancelDataStartsHere-----------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string MainTableCancelData(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("Update CFSImport set RecordStatus = 'Cancelled' Where MainJobNo = @MainJobNo;" +
                                 "Update CFSImportLiner Set RecordStatus = 'Cancelled' Where MainJobNo = @MainJobNo;" +
                                 "Update CFSImportContainer Set RecordStatus = 'Cancelled' Where MainJobNo = @MainJobNo;", cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", MainTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            InsertOrUpdateUmatchedValue("CFSImport", MainTableDataList[0].MainJobNo.Trim(), "", "", "", "Cancelled", "", HttpContext.Current.Session["UserName"].ToString());
            return "Cancelled";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    //-----------------------------------------------------------SearchDetailsStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static MainTableDataList[] MainTableSearchData(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<MainTableDataList> DetailedList = new List<MainTableDataList>();
        MainTableDataList DetailedListValue = new MainTableDataList();
        try
        {
            cmd = new SqlCommand("select Distinct * from CFSImport Where " + MainTableDataList[0].MainPassingType.Trim() + " = @PassingValue And RecordStatus = 'Active';", cnn);
            cmd.Parameters.AddWithValue("@PassingValue", MainTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new MainTableDataList();
                DetailedListValue.MainJobNo = dtrow["MainJobNo"].ToString();
                DetailedListValue.MainJobDate = String.Format("{0:yyyy-MM-dd}", dtrow["MainJobDate"]);
                DetailedListValue.MainIGMNo = dtrow["MainIGMNo"].ToString();
                DetailedListValue.MainItemNo = dtrow["MainItemNo"].ToString();
                DetailedListValue.MainIGMDate = String.Format("{0:yyyy-MM-dd}", dtrow["MainIGMDate"]);
                DetailedListValue.MainBookingNo = dtrow["MainBookingNo"].ToString();
                DetailedListValue.MainBookingDate = String.Format("{0:yyyy-MM-dd}", dtrow["MainBookingDate"]);
                DetailedListValue.MainVesselName = dtrow["MainVesselName"].ToString();
                DetailedListValue.MainPOA = dtrow["MainPOA"].ToString();
                DetailedListValue.MainVIANo = dtrow["MainVIANo"].ToString();
                DetailedListValue.MainVoyNo = dtrow["MainVoyNo"].ToString();
                DetailedListValue.MainJobOwner = dtrow["MainJobOwner"].ToString();
                DetailedListValue.MainIGMUpload = dtrow["MainIGMUpload"].ToString();
                DetailedListValue.GeneralDOA = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["GeneralDOA"]);
                DetailedListValue.GeneralServices = dtrow["GeneralServices"].ToString();
                DetailedListValue.GeneralTPNo = dtrow["GeneralTPNo"].ToString();
                DetailedListValue.GeneralTPDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralTPDate"]);
                DetailedListValue.GeneralConsoler = dtrow["GeneralConsoler"].ToString();
                DetailedListValue.GeneralBerthingDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralBerthingDate"]);
                DetailedListValue.GeneralDateofDeparture = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralDateofDeparture"]);
                DetailedListValue.GeneralCutoffDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralCutoffDate"]);
                DetailedListValue.GeneralRotationNo = dtrow["GeneralRotationNo"].ToString();
                DetailedListValue.GeneralGateOpenDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralGateOpenDate"]);
                DetailedListValue.GeneralScanlistRecvDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralScanlistRecvDate"]);
                DetailedListValue.GeneralBOENo = dtrow["GeneralBOENo"].ToString();
                DetailedListValue.GeneralBOEDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralBOEDate"]);
                DetailedListValue.GeneralLineName = dtrow["GeneralLineName"].ToString();
                DetailedListValue.GeneralEnblock = dtrow["GeneralEnblock"].ToString();
                DetailedListValue.GeneralGCRMSStatus = dtrow["GeneralGCRMSStatus"].ToString();
                DetailedListValue.GeneralPOLCountry = dtrow["GeneralPOLCountry"].ToString();
                DetailedListValue.GeneralPOL = dtrow["GeneralPOL"].ToString();
                DetailedListValue.GeneralPOLCode = dtrow["GeneralPOLCode"].ToString();
                DetailedListValue.GeneralPOD = dtrow["GeneralPOD"].ToString();
                DetailedListValue.GeneralAccountHolder = dtrow["GeneralAccountHolder"].ToString();
                DetailedListValue.GeneralSplitItem = dtrow["GeneralSplitItem"].ToString();
                DetailedListValue.GeneralConsoleValidUpto = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralConsoleValidUpto"]);
                DetailedListValue.GeneralDONo = dtrow["GeneralDONo"].ToString();
                DetailedListValue.GeneralDODate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralDODate"]);
                DetailedListValue.GeneralDOValidDate = String.Format("{0:yyyy-MM-dd}", dtrow["GeneralDOValidDate"]);
                DetailedListValue.GeneralSMTP = dtrow["GeneralSMTP"].ToString();
                DetailedListValue.GeneralCustomChallanNo = dtrow["GeneralCustomChallanNo"].ToString();
                DetailedListValue.GeneralDutyValue = dtrow["GeneralDutyValue"].ToString();
                DetailedListValue.GeneralVolume = dtrow["GeneralVolume"].ToString();
                DetailedListValue.GeneralAssessableValue = dtrow["GeneralAssessableValue"].ToString();
                DetailedListValue.GeneralCommodity = dtrow["GeneralCommodity"].ToString();
                DetailedListValue.GeneralDefaultCustomExamPerc = dtrow["GeneralDefaultCustomExamPerc"].ToString();
                DetailedListValue.GeneralFromLocation = dtrow["GeneralFromLocation"].ToString();
                DetailedListValue.GeneralPackageType = dtrow["GeneralPackageType"].ToString();
                DetailedListValue.GeneralForwarder = dtrow["GeneralForwarder"].ToString();
                DetailedListValue.GeneralNominatedCustomer = dtrow["GeneralNominatedCustomer"].ToString();
                DetailedListValue.GeneralBacktoPort = dtrow["GeneralBacktoPort"].ToString();
                DetailedListValue.GeneralBacktoPortRemarks = dtrow["GeneralBacktoPortRemarks"].ToString();
                DetailedListValue.GeneralTransportation = dtrow["GeneralTransportation"].ToString();
                DetailedListValue.GeneralGateInType = dtrow["GeneralGateInType"].ToString();
                DetailedListValue.GeneralShippingLine = dtrow["GeneralShippingLine"].ToString();
                DetailedListValue.MainCompanyName = dtrow["CompanyName"].ToString();
                DetailedListValue.MainBranchName = dtrow["BranchName"].ToString();
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------ReportSearchStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static string ReportSearch(List<ReportSearchDataList> ReportSearchDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader rd;
        StringBuilder table = new StringBuilder();
        string sqlqry = "", Condition = "";

        try
        {
            int i = 0;
            int j = 0;
            cnn = new SqlConnection(constr);
            if (ReportSearchDataList[0].ReportJobNo.Trim() != "" && ReportSearchDataList[0].ReportJobNo.Trim() != null) { Condition += "MainJobNo = @ReportJobNo AND "; }
            if ((ReportSearchDataList[0].ReportStartDate.Trim() != "" && ReportSearchDataList[0].ReportEndDate.Trim() != "") && (ReportSearchDataList[0].ReportStartDate.Trim() != null && ReportSearchDataList[0].ReportEndDate.Trim() != null)) { Condition += "MainJobDate BETWEEN @ReportStartDate AND @ReportEndDate And "; }
            if (ReportSearchDataList[0].ReportIGMNo.Trim() != "" && ReportSearchDataList[0].ReportIGMNo.Trim() != null) { Condition += "MainIGMNo = @ReportIGMNo AND "; }
            if (ReportSearchDataList[0].ReportVesselName.Trim() != "" && ReportSearchDataList[0].ReportVesselName.Trim() != null) { Condition += "MainVesselName = @ReportVesselName AND "; }
            if (ReportSearchDataList[0].ReportPortofArrival.Trim() != "" && ReportSearchDataList[0].ReportPortofArrival.Trim() != null) { Condition += "MainPOA = @ReportPortofArrival AND "; }
            if (ReportSearchDataList[0].ReportStatus.Trim() != "" && ReportSearchDataList[0].ReportStatus.Trim() != null) { Condition += "MainJobNoStatus = @ReportStatus AND "; }
            if (ReportSearchDataList[0].ReportCompanyName.Trim() != "" && ReportSearchDataList[0].ReportCompanyName.Trim() != null) { Condition += "CompanyName = @CompanyName AND "; }
            else if (ReportSearchDataList[0].ReportCompanyName.Trim() == "") { Condition += "CompanyName In (Select Distinct CompanyName from UserAccessGeneral Where RecordStatus = 'Active' And Employee = '" + HttpContext.Current.Session["UserName"].ToString() + "') AND "; }
            if (ReportSearchDataList[0].ReportBranchName.Trim() != "" && ReportSearchDataList[0].ReportBranchName.Trim() != null) { Condition += "BranchName = @BranchName AND "; }
            else if (ReportSearchDataList[0].ReportBranchName.Trim() == "") { Condition += "BranchName In (Select Distinct BranchName from UserAccessGeneral Where RecordStatus = 'Active' And Employee = '" + HttpContext.Current.Session["UserName"].ToString() + "') AND "; }

            if (ReportSearchDataList[0].ReportTemplateName.Trim() != "")
            {
                cmd = new SqlCommand("Select Query from ICSAColumnTemplates Where TemplateName = @TemplateName;", cnn);
                cmd.Parameters.AddWithValue("@TemplateName", ReportSearchDataList[0].ReportTemplateName.Trim());
                cnn.Open();
                SqlDataReader sqlrd = cmd.ExecuteReader();
                while (sqlrd.Read())
                {
                    sqlqry = sqlrd["Query"].ToString().Trim(); break;
                }
                cnn.Close();
            }
            else if (ReportSearchDataList[0].ReportTemplateName.Trim() == "" && ReportSearchDataList[0].ReportColumnName.Trim() != "" && ReportSearchDataList[0].ReportColumnTabName.Trim() != "")
            {
                sqlqry = "Select MainJobNo as [Job No], " + ReportSearchDataList[0].ReportColumnName.Trim() +
                    " from " + ReportSearchDataList[0].ReportColumnTabName.Trim() + " where RecordStatus = 'Active' And " + Condition.Substring(0, Condition.Length - 4) + "";
            }
            else
            {
                sqlqry = "Select MainJobNo as [Job No],format(MainJobDate,'dd-MMM-yyyy') as [Job Date], MainIGMNo As [IGM No],MainItemNo As [Item No],MainIGMDate As [IGM Date]," +
                "MainBookingNo As [Booking No],MainBookingDate As [Booking Date],MainVesselName As [Vessel Name],MainPOA As [POA],MainVIANo As [VIA No],MainVoyNo As [Voy No],MainJobOwner As [Job Owner] " +
                " from CFSImport where RecordStatus = 'Active' And " + Condition.Substring(0, Condition.Length - 4) + "";
            }

            cmd = new SqlCommand();
            cnn.Open();
            cmd.CommandText = sqlqry;
            cmd.Parameters.AddWithValue("@ReportJobNo", ReportSearchDataList[0].ReportJobNo.Trim());
            cmd.Parameters.AddWithValue("@ReportStartDate", ReportSearchDataList[0].ReportStartDate.Trim());
            cmd.Parameters.AddWithValue("@ReportEndDate", ReportSearchDataList[0].ReportEndDate.Trim());
            cmd.Parameters.AddWithValue("@ReportIGMNo", ReportSearchDataList[0].ReportIGMNo.Trim());
            cmd.Parameters.AddWithValue("@ReportVesselName", ReportSearchDataList[0].ReportVesselName.Trim());
            cmd.Parameters.AddWithValue("@ReportPortofArrival", ReportSearchDataList[0].ReportPortofArrival.Trim());
            cmd.Parameters.AddWithValue("@ReportStatus", ReportSearchDataList[0].ReportStatus.Trim());
            cmd.Parameters.AddWithValue("@ReportTemplateName", ReportSearchDataList[0].ReportTemplateName.Trim());
            cmd.Parameters.AddWithValue("@CompanyName", ReportSearchDataList[0].ReportCompanyName.Trim());
            cmd.Parameters.AddWithValue("@BranchName", ReportSearchDataList[0].ReportBranchName.Trim());
            cmd.Connection = cnn;
            rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                table.Append("<thead><tr>");
                while (rd.Read())
                {
                    if (j == 0)
                    {
                        for (i = 0; i < rd.FieldCount; i++)
                        {
                            table.Append("<th>" + rd.GetName(i).Replace("_", " ") + "</th>");
                        }
                        table.Append("</tr>");
                        table.Append("</thead><tbody>");
                    }
                    j++;
                    table.Append("<tr>");
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        if (i == 0) { table.Append("<td><a href='../CFS/CFSImport.aspx?MainJobNo=" + rd[i] + "' target ='_blank'>" + rd[i] + "</a></td>"); }
                        else { table.Append("<td>" + rd[i] + "</td>"); }
                    }
                    table.Append("</tr>");
                }

                table.Append("</tbody>");
                cnn.Close();
            }
            else { table.Append("No Record Found"); }

            return table.ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public class ReportSearchDataList
    {
        public string ReportColumnTabName { get; set; }
        public string ReportJobNo { get; set; }
        public string ReportStartDate { get; set; }
        public string ReportEndDate { get; set; }
        public string ReportIGMNo { get; set; }
        public string ReportVesselName { get; set; }
        public string ReportCompanyName { get; set; }
        public string ReportBranchName { get; set; }
        public string ReportPortofArrival { get; set; }
        public string ReportStatus { get; set; }
        public string ReportTemplateName { get; set; }
        public string ReportColumnName { get; set; }
        public string ReturnedValue { get; set; }
    }
    //-----------------------------------------------------------MainTablePreviousDataSearchStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static MainTableDataList[] MainTablePreviousDataSearch(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<MainTableDataList> DetailedList = new List<MainTableDataList>();
        MainTableDataList DetailedListValue = new MainTableDataList();
        string qry = "";
        try
        {
            cnn = new SqlConnection(constr);

            qry = "select MainJobNo from CFSImport Where MainJobNoId = ((Select MainJobNoId from CFSImport Where MainJobNo = @MainJobNo) - 1);";
            cmd = new SqlCommand(qry, cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", MainTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.MainJobNo = dtrow["MainJobNo"].ToString();
            }
            cnn.Close();
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------MainTableNextDataSearchStartsHere-------------------------------------------------------------------
    [WebMethod(EnableSession = true)]
    public static MainTableDataList[] MainTableNextDataSearch(List<MainTableDataList> MainTableDataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<MainTableDataList> DetailedList = new List<MainTableDataList>();
        MainTableDataList DetailedListValue = new MainTableDataList();
        string qry = "";
        try
        {
            cnn = new SqlConnection(constr);

            qry = "select MainJobNo from CFSImport Where MainJobNoId = ((Select MainJobNoId from CFSImport Where MainJobNo = @MainJobNo) + 1);";
            cmd = new SqlCommand(qry, cnn);

            cmd.Parameters.AddWithValue("@MainJobNo", MainTableDataList[0].MainJobNo.Trim());
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue.MainJobNo = dtrow["MainJobNo"].ToString();
            }
            cnn.Close();
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    //-----------------------------------------------------------ExcelExportinMultipleSheets-------------------------------------------------------------------
    string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
    SqlCommand cmd;
    SqlConnection cnn;
    DataTable export = new DataTable();
    StringBuilder table = new StringBuilder();
    protected void ExcelExportMultiple(object sender, EventArgs e)
    {
        string MainJobNo = MainJobNoDummy.Text; // MainJobNo.Text;

        List<string> queries = new List<string>
    {
            "Select MainJobNo As [JobNo], MainJobDate As [JobDate], MainIGMNo As [IGMNo], MainItemNo As [ItemNo], MainIGMDate As [IGMDate], MainBookingNo As [BookingNo], MainBookingDate As [BookingDate], MainVesselName As [VesselName], MainPOA As [POA], MainVIANo As [VIANo], MainVoyNo As [VoyNo], MainJobOwner As [JobOwner], GeneralDOA As [DOA], GeneralServices As [Services], GeneralTPNo As [TPNo], GeneralTPDate As [TPDate], GeneralConsoler As [Consoler], GeneralBerthingDate As [BerthingDate], GeneralDateofDeparture As [DateofDeparture], GeneralCutoffDate As [CutoffDate], GeneralRotationNo As [RotationNo], GeneralGateOpenDate As [GateOpenDate], GeneralScanlistRecvDate As [ScanlistRecvDate], GeneralBOENo As [BOENo], GeneralBOEDate As [BOEDate], GeneralLineName As [LineName], GeneralEnblock As [Enblock], GeneralGCRMSStatus As [GCRMSStatus], GeneralPOLCountry As [POLCountry], GeneralPOL As [POL], GeneralPOLCode As [POLCode], GeneralPOD As [POD], GeneralAccountHolder As [AccountHolder], GeneralSplitItem As [SplitItem], GeneralConsoleValidUpto As [ConsoleValidUpto], GeneralDONo As [DONo], GeneralDODate As [DODate], GeneralDOValidDate As [DOValidDate], GeneralSMTP As [SMTP], GeneralCustomChallanNo As [CustomChallanNo], GeneralDutyValue As [DutyValue], GeneralVolume As [Volume], GeneralAssessableValue As [AssessableValue], GeneralCommodity As [Commodity], GeneralDefaultCustomExamPerc As [DefaultCustomExamPerc], GeneralFromLocation As [FromLocation], GeneralPackageType As [PackageType], GeneralForwarder As [Forwarder], GeneralNominatedCustomer As [NominatedCustomer], GeneralBacktoPort As [BacktoPort], GeneralBacktoPortRemarks As [BacktoPortRemarks], GeneralTransportation As [Transportation], GeneralGateInType As [GateInType], GeneralShippingLine As [ShippingLine] From CFSImport where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], LinerItemNo As [ItemNo], LinerImporterName As [ImporterName], LinerLinerAgent As [LinerAgent], LinerBLNo As [BLNo], LinerBLDate As [BLDate], LinerIMDG As [IMDG], LinerWeightKg As [WeightKg], LinerPKG As [PKG], LinerCargoDetails As [CargoDetails], LinerTSANo As [TSANo], LinerTSADate As [TSADate], LinerCHAName As [CHAName], LinerPANNo As [PANNo] From CFSImportLiner where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], ContainerItemNo As [ItemNo], ContainerNo As [ContainerNo], ContainerISOCode As [ISOCode], ContainerSize As [Size], ContainerType As [Type], ContainerSealNo As [SealNo], ContainerTareWeight As [TareWeight], ContainerCargoWeightKg As [CargoWeightKg], ContainerCargoNature As [CargoNature], ContainerNoofPackage As [NoofPackage], ContainerFCLLCL As [FCLLCL], ContainerPrimarySecondary As [PrimarySecondary], ContainerGroupCode As [GroupCode], ContainerUNNo As [UNNo], ContainerScanType As [ScanType], ContainerScanLocation As [ScanLocation], ContainerDeliveryMode As [DeliveryMode], ContainerHold As [Hold], ContainerHoldRemarks As [HoldRemarks], ContainerHoldAgency As [HoldAgency], ContainerHoldDate As [HoldDate], ContainerReleaseDate As [ReleaseDate], ContainerReleaseRemarks As [ReleaseRemarks], ContainerClaimDetails As [ClaimDetails], ContainerClaimAmount As [ClaimAmount], ContainerPaymentDate As [PaymentDate], ContainerRemarks As [Remarks], ContainerWHLoc As [WHLoc], ContainerPriority As [Priority], ContainerStatus As [ContainerStatus], TransportVehicleNo As [VehicleNo], TransportDRFNo As [DRFNo], TransportDRFIssuedDate As [DRFIssuedDate], TransportTransportName As [TransportName], TransportTruckDeployDate As [TruckDeployDate], TransportDriverName As [DriverName], TransportDriverMobileNo As [DriverMobileNo], TransportTerminalGateIn As [TerminalGateIn], TransportTerminalGateOut As [TerminalGateOut], TransportScanStatus As [ScanStatus], TransportCustomsGateOut As [CustomsGateOut], ContainerWeightKg As [WeightKg], ContainerIMOCode As [IMOCode], LoadContVehicleNo As [VehicleNo], LoadContGateInPassNo As [GateInPassNo], LoadContGateInPassDate As [GateInPassDate], LoadContAgentSealNo As [AgentSealNo], LoadContScanStatus As [ScanStatus], LoadContStatusType As [StatusType], LoadContPluginRequired As [PluginRequired], LoadContAdditionAgentSealNo As [AdditionAgentSealNo], LoadContODC As [ODC], LoadContODCDimension As [ODCDimension], LoadContContainerTag As [ContainerTag], LoadContDamageDetails As [DamageDetails], LoadContPortorCustomSealNo As [PortorCustomSealNo], LoadContAdditionPortorCustomSealNo As [AdditionPortorCustomSealNo], LoadContModeofArrival As [ModeofArrival], LoadContTransportType As [TransportType], LoadContEIRNo As [EIRNo], LoadContVehicleType As [VehicleType], LoadContTruckTag As [TruckTag], LoadContContainerCondition As [ContainerCondition] From CFSImportContainer where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], WorkOrderType As [Type], WorkOrderWorkOrderNo As [WorkOrderNo], WorkOrderWorkOrderDate As [WorkOrderDate], WorkOrderContainerNo As [ContainerNo], WorkOrderTruckNo As [TruckNo], WorkOrderEquipmentType As [EquipmentType], WorkOrderEquipment As [Equipment], WorkOrderVendor As [Vendor] From CFSImportWorkOrder where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], EmptyTorCGateInPassNo As [GateInPassNo], EmptyTorCGateInPassDate As [GateInPassDate], EmptyTorCGateInMode As [GateInMode], EmptyTorCCHAName As [CHAName], EmptyTorCTruckNo As [TruckNo], EmptyTorCContainerNo As [ContainerNo], EmptyTorCDriverName As [DriverName], EmptyTorCDriverLicenseNo As [DriverLicenseNo], EmptyTorCRemarks As [Remarks], EmptyTorCWorkOrderNo As [WorkOrderNo], EmptyTorCWorkOrderDate As [WorkOrderDate] From CFSImportEmptyTruckorContainer where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], SealCutWorkOrderNo As [WorkOrderNo], SealCutWorkOrderDate As [WorkOrderDate], SealCutContainerNo As [ContainerNo], SealCutCFSSealNo As [CFSSealNo], SealCutVendor As [Vendor], SealCutWorkOrderStatus As [WorkOrderStatus] From CFSImportSealCutting where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], ExamWorkOrderNo As [WorkOrderNo], ExamWorkOrderDate As [WorkOrderDate], ExamContainerNo As [ContainerNo], ExamCFSSealNo As [CFSSealNo], ExamScrapLabour As [ScrapLabour], ExamExamedPkgs As [ExamedPkgs], ExamExamedPerc As [ExamedPerc], ExamStartDateTime As [StartDateTime], ExamEndDateTime As [EndDateTime], ExamEquipment As [Equipment], ExamVendor As [Vendor], ExamWorkOrderStatus As [WorkOrderStatus] From CFSImportExamination where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], StuffWorkOrderNo As [WorkOrderNo], StuffWorkOrderDate As [WorkOrderDate], StuffDeStuffingContainerNo As [DeStuffingContainerNo], StuffStuffingContainerNo As [StuffingContainerNo], StuffDeclaredPkgs As [DeclaredPkgs], StuffDeclaredWeight As [DeclaredWeight], StuffStuffedFrom As [StuffedFrom], StuffStuffedTo As [StuffedTo], StuffStuffedWeight As [StuffedWeight], StuffStuffedPkgs As [StuffedPkgs], StuffRemarks As [Remarks], StuffWorkOrderStatus As [WorkOrderStatus], StuffingStatus As [Status] From CFSImportStuffing where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], DeStuffWorkOrderNo As [WorkOrderNo], DeStuffWorkOrderDate As [WorkOrderDate], DeStuffContainerNo As [ContainerNo], DeStuffVehicleNo As [VehicleNo], DeStuffFromDate As [FromDate], DeStuffToDate As [ToDate], DeStuffContainerCondition As [ContainerCondition], DeStuffDeclaredWeight As [DeclaredWeight], DeStuffDeclaredPkgs As [DeclaredPkgs], DeStuffDeStuffedWeight As [DeStuffedWeight], DeStuffDeStuffedPkgs As [DeStuffedPkgs], DeStuffDestuffMarkNo As [DestuffMarkNo], DeStuffDestuffLocation As [DestuffLocation], DeStuffAreainSqmt As [AreainSqmt], DeStuffVolume As [Volume], DeStuffMode As [Mode], DeStuffShort As [Short], DeStuffExcess As [Excess], DeStuffNoofGrids As [NoofGrids], DeStuffDelayDueTo As [DelayDueTo], DeStuffDelayRemarks As [DelayRemarks], DeStuffContractor As [Contractor], DeStuffSupervisor As [Supervisor], DeStuffMarksNo As [MarksNo], DeStuffMovementType As [MovementType], DeStuffRemarks As [Remarks], DeStuffWorkOrderStatus As [WorkOrderStatus], DeStuffingStatus As [Status] From CFSImportDeStuffing where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], LoadOutWorkOrderNo As [WorkOrderNo], LoadOutWorkOrderDate As [WorkOrderDate], LoadOutContainerNo As [ContainerNo], LoadOutVehicleNo As [VehicleNo], LoadOutCustDutyValue As [CustDutyValue], LoadOutStampDutyValue As [StampDutyValue], LoadOutOpenOrderDate As [OpenOrderDate], LoadOutOutofChargeDate As [OutofChargeDate], LoadOutCustOutofChargeNo As [CustOutofChargeNo], LoadOutCondition As [Condition], LoadOutRemarks As [Remarks], LoadOutWorkOrderStatus As [WorkOrderStatus] From CFSImportLoadedContainerOut where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], MtyOutWorkOrderNo As [WorkOrderNo], MtyOutWorkOrderDate As [WorkOrderDate], MtyOutContainerNo As [ContainerNo], MtyOutVehicleNo As [VehicleNo], MtyOutModeofGateOut As [ModeofGateOut], MtyOutCycle As [Cycle], MtyOutDriverName As [DriverName], MtyOutEquipmentCondition As [EquipmentCondition], MtyOutContainerTag As [ContainerTag], MtyOutRemarks As [Remarks], MtyMovementBy As [MovementBy], MtyOutWorkOrderStatus As [WorkOrderStatus] From CFSImportEmptyContainerOut where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], FCLOutWorkOrderNo As [WorkOrderNo], FCLOutWorkOrderDate As [WorkOrderDate], FCLOutContainerNo As [ContainerNo], FCLOutVehicleNo As [VehicleNo], FCLOutManifestPackages As [ManifestPackages], FCLOutManifestWeight As [ManifestWeight], FCLOutBalancePackages As [BalancePackages], FCLOutBalanceWeight As [BalanceWeight], FCLOutDestuffedFrom As [DestuffedFrom], FCLOutDestuffedTo As [DestuffedTo], FCLOutDestuffedPkgs As [DestuffedPkgs], FCLOutDestuffedWeight As [DestuffedWeight], FCLOutCustDutyValue As [CustDutyValue], FCLOutStampDutyValue As [StampDutyValue], FCLOutOOCNo As [OOCNo], FCLOutOOCDate As [OOCDate], FCLOutTallyDetails As [TallyDetails], FCLOutEquipment As [Equipment], FCLOutVendor As [Vendor], FCLOutRemarks As [Remarks], FCLOutWorkOrderStatus As [WorkOrderStatus] From CFSImportFCLCargoOut where MainJobNo='"+MainJobNo+"'",
            "Select MainJobNo As [JobNo], LoadTruckWorkOrderNo As [WorkOrderNo], LoadTruckWorkOrderDate As [WorkOrderDate], LoadTruckGateOutPassNo As [GateOutPassNo], LoadTruckGateOutPassDate As [GateOutPassDate], LoadTruckModeofGateOut As [ModeofGateOut], LoadTruckContainerNo As [ContainerNo], LoadTruckTruckNo As [TruckNo], LoadTruckPkgsorWeight As [PkgsorWeight], LoadTruckRemarks As [Remarks] From CFSImportLoadedTruck where MainJobNo='"+MainJobNo+"'",
    };

        List<string> sheetNames = new List<string>
    {
        "Main Data",
        "Liner Data",
        "Container Data",
        "Work Order Data",
        "Gate In Data",
        "Seal Cutting Data",
        "Examination Data",
        "Section-49 Data",
        "De-Stuffing Data",
        "Load Cont Out Data",
        "Empty Cont Out Data",
        "FCL Cargo Out Data",
        "Gate Out Data",        
        //"CFSImportScope",
        //"CFSImportRevenue",
        //"CFSImportCost",        
        //"CFSImportEmptyTruck",
        //"CFSImportTracking",
        //"CFSImportNotes",        
        // Add more sheet names as needed
     };

        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        // Create a new Excel package
        using (ExcelPackage package = new ExcelPackage())
        {
            for (int i = 0; i < queries.Count; i++)
            {
                DataTable export = new DataTable(); // Initialize a new DataTable for each query

                // Execute the query and fill the DataTable
                using (SqlConnection cnn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(queries[i], cnn))
                    {
                        cmd.CommandTimeout = 500;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        SqlCommandBuilder cb = new SqlCommandBuilder(da);
                        da.Fill(export);
                    }
                }
                // Write data to a sheet
                WriteSheet(export, package, sheetNames[i]);
            }

            // Set response headers for Excel download
            string attachment = "attachment; filename= CFSImport " + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // Set content type for XLSX

            // Write the Excel package to the response stream
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }
    }
    void WriteSheet(DataTable data, ExcelPackage package, string sheetName)
    {
        // Add a worksheet
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

        // Write data to the worksheet
        int row = 1;

        // Write column names
        for (int col = 1; col <= data.Columns.Count; col++)
        {
            worksheet.Cells[row, col].Value = data.Columns[col - 1].ColumnName;
        }
        row++;
        // Write data rows
        foreach (DataRow dr in data.Rows)
        {
            for (int col = 1; col <= data.Columns.Count; col++)
            {
                worksheet.Cells[row, col].Value = dr[col - 1];
            }
            row++;
        }
    }
    public static void InsertOrUpdateUmatchedValue(string TableName, string TableKey, string MisMatchedColumnName, string OldValue, string NewValue, string Comments, string Condition, string UserName)
    {
        string ConStr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        SqlConnection cnn = new SqlConnection(ConStr);

        qry = "INSERT INTO CFSImportLog (TableName, TableKeyColumn, ColumnName, OldValue, NewValue, Comments, Condition,UpdatedBy) " +
                "VALUES (@TableName, @TableKeyColumn , @ColumnName, @OldValue, @NewValue, @Comments,@Condition, @UpdatedBy)";
        SqlCommand cmd = new SqlCommand(qry, cnn);
        cmd.Parameters.AddWithValue("@TableName", TableName); // Table Name
        cmd.Parameters.AddWithValue("@TableKeyColumn", TableKey); // Table Key Column
        cmd.Parameters.AddWithValue("@ColumnName", MisMatchedColumnName); // Column Name
        cmd.Parameters.AddWithValue("@OldValue", (object)OldValue ?? DBNull.Value); // Old Value
        cmd.Parameters.AddWithValue("@NewValue", (object)NewValue ?? DBNull.Value); // Modified Value
        cmd.Parameters.AddWithValue("@Comments", Comments); // Comments
        cmd.Parameters.AddWithValue("@Condition", Condition); // Condition To Update
        cmd.Parameters.AddWithValue("@UpdatedBy", UserName); // Session User Name                      

        cnn.Open();
        cmd.ExecuteNonQuery();
        cnn.Close();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/LogistICSA/SessionExpired.html");
        }

        for (int year = 2024; year <= DateTime.Now.Year; year++)
        {
            YearsListDD.Text += "<option value='" + year + "'>" + year + "</option>";
        }

        for (int i = 1; i <= 12; i++)
        {
            MonthListDD.Text += "<option value='" + i + "'>" + new DateTime(DateTime.Now.Year, i, 1).ToString("MMM") + "</option>";
        }

        ReportCompanyName.Value = Session["COMPANY"].ToString();
        ReportBranchName.Value = Session["BRANCH"].ToString();

        MainCompanyName.Value = Session["COMPANY"].ToString();
        MainBranchName.Value = Session["BRANCH"].ToString();
    }
    //-----------------------------------------------------------UserAccess------------------------------------------------------------------- 
    [WebMethod(EnableSession = true)]
    public static UserAccessDataList[] UserAccessValidation(string Dummy)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();
        List<UserAccessDataList> DetailedList = new List<UserAccessDataList>();
        UserAccessDataList DetailedListValue = new UserAccessDataList();
        try
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand("select AccessRole from UserAccessGeneral Where RecordStatus = 'Active' and CompanyName = @Company and BranchName = @Branch and Employee = @Employee and FormName = @FormName;", cnn);
            cmd.Parameters.AddWithValue("@Company", HttpContext.Current.Session["COMPANY"].ToString());
            cmd.Parameters.AddWithValue("@Branch", HttpContext.Current.Session["BRANCH"].ToString());
            cmd.Parameters.AddWithValue("@Employee", HttpContext.Current.Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@FormName", "Import Container Freight Station");
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue.ReturnedValue = "No Data Found!!!";
                DetailedList.Add(DetailedListValue);
                return DetailedList.ToArray();
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue = new UserAccessDataList();
                DetailedListValue.AccessRole = dtrow["AccessRole"].ToString();
                DetailedListValue.ReturnedValue = "";
                DetailedList.Add(DetailedListValue);
            }
            cnn.Close();
            return DetailedList.ToArray();
        }
        catch (Exception ex)
        {
            DetailedListValue.ReturnedValue = ex.Message;
            DetailedList.Add(DetailedListValue);
            return DetailedList.ToArray();
        }
    }
    public class UserAccessDataList
    {
        public string AccessRole { get; set; }
        public string ReturnedValue { get; set; }
    }

    //-------------------------------Added Manually------------------------------------
    [WebMethod]
    public static DropDownDetails[] LoadDropDownValuesonblur(string TextBoxValue, string IGMNo, string JobNo, string Country)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        List<DropDownDetails> details = new List<DropDownDetails>();

        try
        {
            qry = " SELECT 'Item No' AS [DDNAME] ,[MainItemNo] AS [DDVALUE] FROM CFSImport Where MainIGMNo = '" + TextBoxValue + "' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Container No' AS [DDNAME] ,[ContainerNo] AS [DDVALUE] FROM CFSImportContainer Where MainJobNo = '" + JobNo + "' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Only Gated In Container No' AS [DDNAME] ,[ContainerNo] AS [DDVALUE] FROM CFSImportContainer Where ContainerStatus = 'Gated In' And MainJobNo = '" + JobNo + "' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Only Gated In Container No' AS [DDNAME] ,[EmptyTorCContainerNo] AS [DDVALUE] FROM CFSImportEmptyTruckorContainer Where EmptyTorCGateInMode = 'Empty Container In' And EmptyTruckorContainerStatus = 'Stuffed' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'DRF Date' AS [DDNAME] ,CONVERT(VARCHAR, DRFIssuedDate, 120) AS [DDVALUE] FROM CFSImportDRFScanListUpload Where IGMNo = '" + IGMNo + "' And DRFNo = '" + TextBoxValue + "' " +
            "UNION" +
            " SELECT 'DRF TransportName' AS [DDNAME] ,[TransportName] AS [DDVALUE] FROM CFSImportDRFScanListUpload Where IGMNo = '" + IGMNo + "' And DRFNo = '" + TextBoxValue + "' " +
            "UNION" +
            " SELECT 'Port Name' AS [DDNAME] ,[PORTNAME] AS [DDVALUE] FROM PortMaster WHERE COUNTRY = '" + TextBoxValue + "' AND RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Port Code' AS [DDNAME] ,[PortCode] AS [DDVALUE] FROM PortMaster WHERE PortName = '" + TextBoxValue + "' And COUNTRY = '" + Country + "' AND RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Container Size' AS [DDNAME] ,[ContainerSize] AS [DDVALUE] FROM CFSImportISOCodeMaster WHERE ContainerISOCode = '" + TextBoxValue + "' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Container Type' AS [DDNAME] ,[ContainerType] AS [DDVALUE] FROM CFSImportISOCodeMaster WHERE ContainerISOCode = '" + TextBoxValue + "' And RecordStatus = 'Active' " +
            "UNION" +
            " Select 'Branch Name', BranchName from UserAccessGeneral Where RecordStatus = 'Active' And CompanyName = '" + TextBoxValue + "' And Employee = '" + HttpContext.Current.Session["UserName"].ToString() + "' " +
            "UNION" +
            " SELECT 'Container TareWeight' AS [DDNAME] ,Convert(Varchar(Max), [ContainerTareWeight]) AS [DDVALUE] FROM CFSImportISOCodeMaster WHERE ContainerISOCode = '" + TextBoxValue + "' And RecordStatus = 'Active' " +
            " ORDER BY 1,2";

            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                DropDownDetails columns = new DropDownDetails();
                columns.ddname = dtrow["DDNAME"].ToString();
                if (dtrow["DDNAME"].ToString() == "DRF Date") { columns.ddvalue = String.Format("{0:yyyy-MM-dd HH:mm}", dtrow["DDVALUE"]); }
                else { columns.ddvalue = dtrow["DDVALUE"].ToString(); }
                details.Add(columns);
            }
            cnn.Close();
        }
        catch (Exception Ex)
        {
            DropDownDetails columns = new DropDownDetails();
            columns.ReturnedValue = Ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }
    [WebMethod]
    public static DropDownDetails[] LoadDDWhileSearch(string IGM, string Item, string JobNo, string Country, string PortName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        List<DropDownDetails> details = new List<DropDownDetails>();

        try
        {
            qry = " SELECT 'Item No' AS [DDNAME] ,[MainItemNo] AS [DDVALUE] FROM CFSImport Where MainIGMNo = '" + IGM + "' And RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'DRF No' AS [DDNAME] ,[DRFNo] AS [DDVALUE] FROM CFSImportDRFScanListUpload Where IGMNo = '" + IGM + "' " +
            "UNION" +
            " SELECT 'Container No' AS [DDNAME] ,[ContainerNo] AS [DDVALUE] FROM CFSImportContainer Where MainJobNo = '" + JobNo + "' And RecordStatus = 'Active' " +
            "UNION" +
            " Select 'Empty Container No' , EmptyTorCContainerNo From CFSImportEmptyTruckorContainer Where EmptyTorCGateInMode = 'Empty Container In' And EmptyTruckorContainerStatus = 'Gated In' And" +
            " EmptyTorCGateInPassNo Is Not Null And EmptyTorCGateInPassNo <> '' And RecordStatus = 'Active' And MainJobNo = '" + JobNo + "' " +
            "UNION" +
            " SELECT 'Port Name' AS [DDNAME] ,[PORTNAME] AS [DDVALUE] FROM PortMaster WHERE COUNTRY = '" + Country + "' AND RecordStatus = 'Active' " +
            "UNION" +
            " SELECT 'Port Code' AS [DDNAME] ,[PortCode] AS [DDVALUE] FROM PortMaster WHERE PortName = '" + PortName + "' And COUNTRY = '" + Country + "' AND RecordStatus = 'Active' " +
            " ORDER BY 1,2";

            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dtrow in dt.Rows)
            {
                DropDownDetails columns = new DropDownDetails();
                columns.ddname = dtrow["DDNAME"].ToString();
                columns.ddvalue = dtrow["DDVALUE"].ToString();
                details.Add(columns);
            }
            cnn.Close();
        }
        catch (Exception Ex)
        {
            DropDownDetails columns = new DropDownDetails();
            columns.ReturnedValue = Ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }
    protected void TSAExcelFormat_Click(object sender, EventArgs e)
    {
        try
        {
            //Kindly Add the reference file for the Excel Export Process
            // 1.) Click Solution Explorer and Right on the Solution (or) Project.
            // 2.) Find this (Manage NuGet Packages) and Click on it.
            // 3.) Then Search with this Key Word (EPPlus) in that search box.
            // 4.) Click and Install the latest version (7.0.3);

            DataTable export1 = new DataTable();

            string qry = @"Select A.ContainerNo, A.ContainerSize As [Size], A.ContainerItemNo, A.ContainerTareWeight As [Tare Weight], A.ContainerCargoWeightKg As 
                            [CargoWeight],C.DRFNo As [DRF No], FORMAT(C.DRFIssuedDate, 'dd/MM/yyyy HH:mm') As [DRF Issued Date & Time], C.TransportName AS [Transport Name], 
                            A.ContainerScanType As [Scan Type], A.ContainerScanLocation As [Scan Location], 
                            B.MainVesselName As [Vessel Name], B.MainPOA AS [Terminal Name], B.MainVIANo As [VIA No], B.MainVoyNo As [Voy No] 
                            From CFSImportContainer A
                            Left join CFSImport B On A.MainJobNo = B.MainJobNo And A.ContainerItemNo = B.MainItemNo And A.RecordStatus = B.RecordStatus
							Left Join CFSImportDRFScanListUpload C On A.ContainerNo = C.ContainerNo And B.MainIGMNo = C.IGMNo
                            Where B.MainIGMNo = '" + DupJobNoNumber.Text + "' And A.RecordStatus = 'Active';";

            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(export1);

            if (export1.Rows.Count > 0)
            {
                string attachment = "attachment; filename=IGM - " + DupJobNoNumber.Text + " - " + DateTime.Now.ToString("dd/MMM/yyyy") + ".xlsx";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("DRF Details");

                    // Write headers                                    

                    for (int col = 1; col <= (export1.Columns.Count - 4); col++)
                    {
                        if (col == 1)
                        {
                            worksheet.Cells[1, col].Value = export1.Columns[col + 9].ColumnName;
                            worksheet.Cells[1, col].Style.Font.Bold = true;
                            worksheet.Cells[1, col].Style.Font.Color.SetColor(Color.White);
                            worksheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Set your desired color
                            worksheet.Cells[1, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worksheet.Cells[1, col + 2].Value = export1.Columns[col + 10].ColumnName;
                            worksheet.Cells[1, col + 2].Style.Font.Bold = true;
                            worksheet.Cells[1, col + 2].Style.Font.Color.SetColor(Color.White);
                            worksheet.Cells[1, col + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, col + 2].Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Set your desired color
                            worksheet.Cells[1, col + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[1, col + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worksheet.Cells[1, col + 4].Value = export1.Columns[col + 11].ColumnName;
                            worksheet.Cells[1, col + 4].Style.Font.Bold = true;
                            worksheet.Cells[1, col + 4].Style.Font.Color.SetColor(Color.White);
                            worksheet.Cells[1, col + 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, col + 4].Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Set your desired color
                            worksheet.Cells[1, col + 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[1, col + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worksheet.Cells[1, col + 6].Value = export1.Columns[col + 12].ColumnName;
                            worksheet.Cells[1, col + 6].Style.Font.Bold = true;
                            worksheet.Cells[1, col + 6].Style.Font.Color.SetColor(Color.White);
                            worksheet.Cells[1, col + 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, col + 6].Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Set your desired color
                            worksheet.Cells[1, col + 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[1, col + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        worksheet.Cells[3, col].Value = export1.Columns[col - 1].ColumnName;
                        worksheet.Cells[3, col].Style.Font.Bold = true;
                        worksheet.Cells[3, col].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[3, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, col].Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Set your desired color
                        worksheet.Cells[3, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[3, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        if (col == 7) // Column index for "DRF Issued Date & Time" (1-based)
                        {
                            worksheet.Cells[3, col].AddComment("DateTime Format/shortcut - dd/mm/yyyy HH:mm (ex: 20/05/2024 18:27) / [Ctrl + ;][Space][Ctrl + Shift + ;]", "Admin");
                        }
                    }

                    // Write data

                    worksheet.Cells[1, 2].Value = export1.Rows[0][10];
                    worksheet.Cells[1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[1, 4].Value = export1.Rows[0][11];
                    worksheet.Cells[1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[1, 6].Value = export1.Rows[0][12];
                    worksheet.Cells[1, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[1, 8].Value = export1.Rows[0][13];
                    worksheet.Cells[1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    for (int row = 0; row < export1.Rows.Count; row++)
                    {
                        for (int col = 0; col < (export1.Columns.Count - 4); col++)
                        {
                            worksheet.Cells[row + 4, col + 1].Value = export1.Rows[row][col];
                            worksheet.Cells[row + 4, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[row + 4, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        //Adding dropdown values
                        var ScanStatusDDValues = worksheet.DataValidations.AddListValidation("I" + (row + 4));
                        ScanStatusDDValues.Formula.Values.Add("Full Scan");
                        ScanStatusDDValues.Formula.Values.Add("Drive Through");
                        ScanStatusDDValues.Formula.Values.Add("Mobile Scan");

                        var ScanLocDDValues = worksheet.DataValidations.AddListValidation("J" + (row + 4));
                        ScanLocDDValues.Formula.Values.Add("CCTL");
                        ScanLocDDValues.Formula.Values.Add("CITPL");
                        ScanLocDDValues.Formula.Values.Add("Kattupalli");
                        ScanLocDDValues.Formula.Values.Add("Kamarajar");
                    }

                    cnn = new SqlConnection(constr);
                    cmd = new SqlCommand("Select '8' As [ColValue], 'EntityName' As [Name], EntityName As [Values] From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Vendor%' And EntityType Like '%Transport%' And RecordStatus = 'Active'", cnn);
                    cnn.Open();
                    DataTable dt = new DataTable();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.Close();

                    var uniqueRows = dt.AsEnumerable()
                                        .GroupBy(row => new
                                        {
                                            ColumnValue = Convert.ToInt32(row["ColValue"]),
                                            Name = row.Field<string>("Name")
                                        })
                                        .Select(group => new
                                        {
                                            group.Key.ColumnValue,
                                            group.Key.Name
                                        });

                    // Create a dedicated worksheet for dropdown values            
                    var dropdownSheet = package.Workbook.Worksheets["DDValues"] ?? package.Workbook.Worksheets.Add("DDValues");

                    int currentRow = 1; // Start from the first row in the dropdown sheet

                    foreach (var row in uniqueRows)
                    {
                        // Filter the values for the current row
                        var filteredValues = dt.AsEnumerable()
                            .Where(dtrow => dtrow["Name"].ToString() == row.Name)
                            .Select(dtrow => dtrow["Values"].ToString())
                            .ToList();

                        // Sanitize the row.Name to create a valid named range
                        string sanitizedRowName = "Dropdown_" + Regex.Replace(row.Name, @"[^A-Za-z0-9_\.]", "_");

                        // Add these values to the dropdown sheet
                        string rangeStart = "A" + currentRow;
                        foreach (var value in filteredValues)
                        {
                            dropdownSheet.Cells[currentRow, 1].Value = value; // Add values in column A
                            currentRow++;
                        }

                        string rangeEnd = "A" + (currentRow - 1);
                        package.Workbook.Names.Add(sanitizedRowName, dropdownSheet.Cells[rangeStart + ":" + rangeEnd]);

                        // Add the list validation referencing the named range
                        var validation = worksheet.DataValidations.AddListValidation(worksheet.Cells[4, row.ColumnValue, export1.Rows.Count + 3, row.ColumnValue].Address);
                        validation.AllowBlank = true; // Allow blank entries
                        validation.Formula.ExcelFormula = sanitizedRowName; // Reference the named range
                    }

                    // Optionally hide the dropdown sheet to keep it tidy
                    dropdownSheet.Hidden = eWorkSheetHidden.Hidden;

                    //Set Filter To first Column
                    //worksheet.Cells[1, 1, 1, 12].AutoFilter = true;

                    // Auto-fit columns
                    worksheet.Cells.AutoFitColumns();

                    // Stream the Excel package to the response output
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", "showErrorPopup('There is no containers for this JobNo.', 'red');", true);
            }
        }
        catch (Exception ex)
        { ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", "showErrorPopup('" + ex.Message.Replace("'", "’") + "', 'red');", true); }
    }

    protected void IGMDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = "Select Distinct MainJobNo From CFSImport Where MainIGMNo = '" + MainIGMUploadTb.Text.Trim() + "' And RecordStatus = 'Active'", JobNo = "";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                JobNo += "'" + reader["MainJobNo"].ToString() + "',";
            }
            cnn.Close();

            if (JobNo.Length > 0) { JobNo = JobNo.Substring(0, JobNo.Length - 1); }
            else { JobNo = "''"; }

            // Define your queries for each sheet
            string qry1 = "Select Distinct " +
                                "MainIGMNo As [IGM No], Format(MainIGMDate, 'dd/MM/yyyy') As [IGM Date], MainVesselName As [Vessel Name], MainVIANo As [VIA No], MainVoyNo As " +
                                "[Voy No], MainPOA As [Port of Arraival], GeneralAccountHolder As [Account Holder], MainJobOwner As [Job Owner] " +
                          "From CFSImport Where MainJobNo In (" + JobNo + ") And RecordStatus = 'Active'";

            string qry2 = "Select " +
                              "LinerItemNo As [Item No], LinerImporterName As [Importer Name], LinerCHAName As [CHA Name], LinerLinerAgent As [Liner Agent], LinerBLNo As [BL No], Format " +
                              "(LinerBLDate,'dd/MM/yyyy') As [BL Date], LinerIMDG As [IMDG], LinerWeightKg As [Weight(Kg)], LinerPKG As [Pkg], " +
                              "LinerCargoDetails As [Cargo Details], LinerTSANo As [TSA No], Format(LinerTSADate, 'dd/MM/yyyy') As [TSA Date], LinerPANNo As [PAN No] " +
                          "From CFSImportLiner Where MainJobNo In (" + JobNo + ") And RecordStatus = 'Active'";

            string qry3 = "Select " +
                              "ContainerItemNo As [Item No], ContainerNo As [Container No], ContainerISOCode As [ISO Code], ContainerSize As [Size], ContainerType As " +
                              "[Type], ContainerSealNo As [Seal No], ContainerTareWeight As [Tare Weight], ContainerWeightKg AS [Weight (Kg)], ContainerCargoWeightKg As " +
                              "[Cargo Weight(Kg)], ContainerCargoNature As [Cargo Nature], ContainerNoofPackage As [No of Package], ContainerFCLLCL As [FCL/LCL], " +
                              "ContainerPrimarySecondary As [Primary/Secondary], ContainerGroupCode As [Group Code], ContainerIMOCode AS [IMO Code], ContainerUNNo As " +
                              "[UN No], ContainerScanType AS [Scan Type], ContainerScanLocation AS [Scan Location], ContainerDeliveryMode As [Delivery Mode], " +
                              "ContainerHold As [Hold], ContainerHoldRemarks As [Hold Remarks], ContainerHoldAgency As [Hold Agency], " +
                              "Format(ContainerHoldDate, 'dd/MM/yyyy')As [Hold Date], Format(ContainerReleaseDate, 'dd/MM/yyyy') As [Release Date], " +
                              "ContainerReleaseRemarks As [Release Remarks], ContainerClaimDetails As [Claim Details], ContainerClaimAmount AS [Claim Amount], " +
                              "Format(ContainerPaymentDate, 'dd/MM/yyyy') As [Payment Date], ContainerRemarks AS [Remarks], ContainerWHLoc AS [WH Loc], ContainerPriority " +
                              "As [Priority] " +
                          "From CFSImportContainer Where MainJobNo In (" + JobNo + ") And RecordStatus = 'Active'";

            DataTable sheet1Data = ExecuteQuery(qry1);
            DataTable sheet2Data = ExecuteQuery(qry2);
            DataTable sheet3Data = ExecuteQuery(qry3);

            string attachment = "attachment; filename=IGM Upload Format_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage())
            {
                // Create the sheets
                CreateExcelSheet(package, sheet1Data, "General");
                CreateExcelSheet(package, sheet2Data, "Liner");
                CreateExcelSheet(package, sheet3Data, "Container");

                // Stream the Excel package to the response output
                Response.BinaryWrite(package.GetAsByteArray());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", "showErrorPopup('" + ex.Message.Replace("'", "’") + "', 'red');", true);
        }
    }

    private DataTable ExecuteQuery(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection cnn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query, cnn))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }

    private void CreateExcelSheet(ExcelPackage package, DataTable data, string sheetName)
    {
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        // Write headers
        for (int col = 0; col < data.Columns.Count; col++)
        {
            worksheet.Cells[1, col + 1].Value = data.Columns[col].ColumnName;
            worksheet.Cells[1, col + 1].Style.Font.Bold = true;
            worksheet.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[1, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            if (sheetName == "General")
            {
                if (col == 1) // Column index for "IGM Date"
                {
                    worksheet.Cells[1, col + 1].AddComment("Date Format/shortcut - dd/mm/yyyy / [Ctrl + ;]", "Admin");
                }
                if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5)
                {
                    worksheet.Cells[1, col + 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
                }
            }
            if (sheetName == "Liner")
            {
                if (col == 5 || col == 11) // Column index for "5 - BL Date, 11 - TSA Date"
                {
                    worksheet.Cells[1, col + 1].AddComment("Date Format/shortcut - dd/mm/yyyy / [Ctrl + ;]", "Admin");
                }
                if (col == 0 || col == 1 || col == 3 || col == 4 || col == 7 || col == 8 || col == 9)
                {
                    worksheet.Cells[1, col + 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
                }
            }
            if (sheetName == "Container")
            {
                if (col == 22 || col == 23 || col == 27) // Column index for "22 - Hold Date, 23 - Release Date, 27 - Payment Date"
                {
                    worksheet.Cells[1, col + 1].AddComment("Date Format/shortcut - dd/mm/yyyy / [Ctrl + ;]", "Admin");
                }
                if (col == 0 || col == 1 || col == 2 || col == 5 || col == 7 || col == 10 || col == 11)
                {
                    worksheet.Cells[1, col + 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(Color.Red);
                }
            }
        }

        // Write data
        if (data.Rows.Count > 0)
        {
            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = data.Rows[row][col];
                    worksheet.Cells[row + 2, col + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }
        int startRow = 2; // Data starts from the second row
        int endRow;

        if (data.Rows.Count == 0)
        {
            if (sheetName == "General") { endRow = startRow; }
            else { endRow = startRow + 10 - 1; }

            for (int i = startRow; i <= endRow; i++)
            {
                for (int col = 1; col <= data.Columns.Count; col++)
                {
                    worksheet.Cells[i, col].Value = string.Empty; // Add empty cells for dummy rows
                    worksheet.Cells[i, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }
        else
        {
            endRow = data.Rows.Count + 1; // Adjust for actual data rows
        }

        //Adding Drop Down to Excel Cell
        string qry = "";
        if (sheetName == "General")
        {
            qry = "SELECT Top 4 '6' As [ColValue], 'PortName' As [Name], PortName As [Values] FROM PortMaster Where RecordStatus = 'Active' And PortStatus = 'New' And Country = 'India' " +
                  "Union Select '3', 'Vessel Name', MainVesselName From CFSImport Where RecordStatus = 'Active' " +
                  "Union Select '7', 'Customer Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Customer%' And RecordStatus = 'Active'";
        }
        if (sheetName == "Liner")
        {
            qry = "Select '2' As [ColValue], 'Importer Name' As [Name], EntityName As [Values] From [EntityType] Where Vertical = 'CFS' And EntityType Like '%Importer%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                  " Union Select '3', 'CHA Name', EntityName From [EntityType] Where Vertical = 'CFS' And EntityType Like '%CHA%' And EntityType Like '%Customer%' And RecordStatus = 'Active' " +
                  " Union Select '4', 'Liner Code', EntityDesc From [Entity] Where EntityName In " +
                  " (Select EntityName From [EntityType] Where Vertical = 'CFS' And RecordStatus = 'Active') And RecordStatus = 'Active'";
        }
        if (sheetName == "Container")
        {
            qry = "SELECT '3' As [ColValue], 'ContainerISOCode' As [Name], ContainerISOCode As [Values] FROM CFSImportISOCodeMaster Where RecordStatus = 'Active' " +
                  "Union " +
                  "Select '4', 'Size' AS [Name], '20' As [Values] Union Select '4', 'Size' AS [Name], '40' As [Values] Union Select '4', 'Size' AS [Name], '45' As [Values] " +
                  "Union " +
                  "SELECT '5', 'ContType', ContainerType FROM CFSImportISOCodeMaster Where RecordStatus = 'Active' " +
                  "Union " +
                  "SELECT '22', DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-HoldAgency' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                  "Union " +
                  "SELECT '10', DropDownColumnName, DefaultValue FROM DefaultDropDownValues WHERE DropDownColumnName = 'CFS-CargoNature' And RecordStatus = 'Active' And DropDownColumnNameStatus = 'New' " +
                  "Union " +
                  "Select '12', 'FCLLCL' AS [Name], 'FCL' As [Values] Union Select '12', 'FCLLCL' AS [Name], 'LCL' As [Values] " +
                  "Union " +
                  "Select '13', 'PriSec' AS [Name], 'Primary' As [Values] Union Select '13', 'PriSec' AS [Name], 'Secondary' As [Values] " +
                  "Union " +
                  "Select '14', 'GroupCode' AS [Name], 'PNR' As [Values] Union Select '14', 'GroupCode' AS [Name], 'DPD/DPD' As [Values] Union Select '14', 'GroupCode' AS [Name], 'DPD/CFS' As [Values] Union Select '14', 'GroupCode' AS [Name], 'CFS/CFS' As [Values] " +
                  "Union " +
                  "SELECT Top 4 '18', 'PortName' As [Name], PortName As [Values] FROM PortMaster Where RecordStatus = 'Active' And PortStatus = 'New' And Country = 'India' " +
                  "Union " +
                  "Select '17', 'ScanType' AS [Name], 'Full Scan' As [Values] Union Select '17', 'ScanType' AS [Name], 'Drive Through' As [Values] Union Select '17', 'ScanType' AS [Name], 'Mobile Scan' As [Values] " +
                  "Union " +
                  "Select '19', 'DeliveryMode' AS [Name], 'Loaded Delivery' As [Values] Union Select '19', 'DeliveryMode' AS [Name], 'Destuff Delivery' As [Values] " +
                  "Union " +
                  "Select '20', 'YesorNo' AS [Name], 'Yes' As [Values] Union Select '20', 'YesorNo' AS [Name], 'No' As [Values] " +
                  "Union " +
                  "Select '26', 'YesorNo' AS [Name], 'Yes' As [Values] Union Select '26', 'YesorNo' AS [Name], 'No' As [Values] ";
        }

        if (qry != "")
        {
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();

            var uniqueRows = dt.AsEnumerable()
                                .GroupBy(row => new
                                {
                                    ColumnValue = Convert.ToInt32(row["ColValue"]),
                                    Name = row.Field<string>("Name")
                                })
                                .Select(group => new
                                {
                                    group.Key.ColumnValue,
                                    group.Key.Name
                                });

            // Create a dedicated worksheet for dropdown values            
            var dropdownSheet = package.Workbook.Worksheets["DDValues" + sheetName] ?? package.Workbook.Worksheets.Add("DDValues" + sheetName);

            int currentRow = 1; // Start from the first row in the dropdown sheet

            foreach (var row in uniqueRows)
            {
                // Filter the values for the current row
                var filteredValues = dt.AsEnumerable()
                    .Where(dtrow => dtrow["Name"].ToString() == row.Name)
                    .Select(dtrow => dtrow["Values"].ToString())
                    .ToList();

                // Sanitize the row.Name to create a valid named range
                string sanitizedRowName = "Dropdown_" + Regex.Replace(row.Name, @"[^A-Za-z0-9_\.]", "_");

                // Add these values to the dropdown sheet
                string rangeStart = "A" + currentRow;
                foreach (var value in filteredValues)
                {
                    dropdownSheet.Cells[currentRow, 1].Value = value; // Add values in column A
                    currentRow++;
                }

                string rangeEnd = "A" + (currentRow - 1);
                package.Workbook.Names.Add(sanitizedRowName, dropdownSheet.Cells[rangeStart + ":" + rangeEnd]);

                // Add the list validation referencing the named range
                var validation = worksheet.DataValidations.AddListValidation(worksheet.Cells[startRow, row.ColumnValue, endRow, row.ColumnValue].Address);
                validation.AllowBlank = true; // Allow blank entries
                validation.Formula.ExcelFormula = sanitizedRowName; // Reference the named range
            }

            // Optionally hide the dropdown sheet to keep it tidy
            dropdownSheet.Hidden = eWorkSheetHidden.Hidden;
        }
        //Adding Drop Down to Excel Cell

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();
    }


    [WebMethod(EnableSession = true)]
    public static string DRFandScanListUpload(List<SheetData1List> SheetData1List)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlConnection cnnn = new SqlConnection(constr);
        SqlCommand cmd, cmdd;

        try
        {
            ProcessList(SheetData1List);

            foreach (var LinerTab in SheetData1List)
            {
                qry = "Select Count(*) from CFSImportDRFScanListUpload Where IGMNo = '" + LinerTab.ExcelIGMNo + "' And ContainerNo = '" + LinerTab.ExcelContainerNo + "'";
                cmd = new SqlCommand(qry, cnn);
                cnn.Open();
                int Count = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                cnn.Close();

                if (Count > 0)
                {
                    InsertOrUpdateUmatchedValue("CFSImportDRFScanListUpload", LinerTab.ExcelDRFNo, "", "", "", "Modified", "", HttpContext.Current.Session["UserName"].ToString());

                    qry = "Delete from CFSImportDRFScanListUpload Where IGMNo = '" + LinerTab.ExcelIGMNo + "' And ContainerNo = '" + LinerTab.ExcelContainerNo + "'";
                    cmd = new SqlCommand(qry, cnn);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                qry = "Insert Into CFSImportDRFScanListUpload (IGMNo, ContainerNo, DRFNo, DRFIssuedDate, TransportName, Updatedby) " +
                           "Values (@IGMNo, @ContainerNo, @DRFNo, @DRFIssuedDate, @TransportName, @Updatedby)";
                cmd = new SqlCommand(qry, cnn);
                cmd.Parameters.AddWithValue("@IGMNo", LinerTab.ExcelIGMNo.Trim());
                cmd.Parameters.AddWithValue("@ContainerNo", LinerTab.ExcelContainerNo.Trim());
                cmd.Parameters.AddWithValue("@DRFNo", LinerTab.ExcelDRFNo.Trim());
                if (LinerTab.ExcelDRFIssuedDate == "" || LinerTab.ExcelDRFIssuedDate == null) { cmd.Parameters.AddWithValue("@DRFIssuedDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@DRFIssuedDate", Convert.ToDateTime(LinerTab.ExcelDRFIssuedDate.Trim())); }
                cmd.Parameters.AddWithValue("@TransportName", LinerTab.ExcelTransportName.Trim());
                cmd.Parameters.AddWithValue("@Updatedby", HttpContext.Current.Session["UserName"].ToString());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                qry = "Select MainJobNo, MainItemNo From CFSImport Where MainIGMNo = '" + LinerTab.ExcelIGMNo + "' And RecordStatus = 'Active'";
                cmd = new SqlCommand(qry, cnn);
                cnn.Open();
                SqlDataReader sqlrd = cmd.ExecuteReader();
                while (sqlrd.Read())
                {
                    qry = "Update CFSImportContainer Set CompanyName=@CompanyName, BranchName=@BranchName, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, ContainerScanType=@ScanType, ContainerScanLocation=@ScanLocation" +
                          " Where MainJobNo=@MainJobNo And ContainerItemNo=@ContainerItemNo And ContainerNo=@ContainerNo And RecordStatus = 'Active'";
                    cmdd = new SqlCommand(qry, cnnn);
                    cmdd.Parameters.AddWithValue("@MainJobNo", sqlrd["MainJobNo"].ToString().Trim());
                    cmdd.Parameters.AddWithValue("@ContainerItemNo", sqlrd["MainItemNo"].ToString().Trim());
                    cmdd.Parameters.AddWithValue("@ContainerNo", LinerTab.ExcelContainerNo.Trim());
                    cmdd.Parameters.AddWithValue("@ScanType", LinerTab.ExcelScanType.Trim());
                    cmdd.Parameters.AddWithValue("@ScanLocation", LinerTab.ExcelScanLocation.Trim());
                    cmdd.Parameters.AddWithValue("@Updatedby", HttpContext.Current.Session["UserName"].ToString());
                    cmdd.Parameters.AddWithValue("@ComputerName", "");
                    cmdd.Parameters.AddWithValue("@IPAddress", "");
                    cmdd.Parameters.AddWithValue("@Location", "");
                    cmdd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                    cmdd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                    cnnn.Open();
                    cmdd.ExecuteNonQuery();
                    cnnn.Close();
                }
                cnn.Close();
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "Saved";
    }

    public static void ProcessList<T>(List<T> dataList)
    {
        foreach (var item in dataList)
        {
            foreach (var property in item.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var propertyValue = property.GetValue(item) as string;

                    if (propertyValue == "-" || propertyValue == "undefined" || propertyValue == null || propertyValue == "NaN-NaN-NaN" || propertyValue == "1899-12-30")
                    {
                        property.SetValue(item, "");
                    }
                }
            }
        }
    }

    public class SheetData1List
    {
        public string LinerItemNo { get; set; }
        public string LinerImporterName { get; set; }
        public string LinerPANNumber { get; set; }
        public string LinerCHAName { get; set; }
        public string LinerLinerAgent { get; set; }
        public string LinerBLNumber { get; set; }
        public string LinerBLDate { get; set; }
        public string LinerIMDG { get; set; }
        public string LinerWeightKgs { get; set; }
        public string LinerPackages { get; set; }
        public string LinerCargoDetails { get; set; }
        public string LinerTSANumber { get; set; }
        public string LinerTSADate { get; set; }
        public string ExcelIGMNo { get; set; }
        public string ExcelContainerNo { get; set; }
        public string ExcelScanType { get; set; }
        public string ExcelScanLocation { get; set; }
        public string ExcelDRFNo { get; set; }
        public string ExcelDRFIssuedDate { get; set; }
        public string ExcelTransportName { get; set; }
    }

    [WebMethod]
    public static ColumnList[] getcolumnsfromtable(string TableName, string ColumnstoExclude)
    {
        List<ColumnList> details = new List<ColumnList>();
        try
        {
            string ExceptedColumns = "";
            foreach (var Columns in ColumnstoExclude.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                ExceptedColumns += "'" + Columns.Trim() + "', ";
            }

            string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString; //For Testing - LogistICSAProdTest, Real One is - LogistICSAProd
            string qry = "Select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = '" + TableName + "' And " +
                         "COLUMN_NAME Not In (" + ExceptedColumns + "'UpdatedOn', 'Location', 'IPAddress', " +
                         "'ComputerName', 'UpdatedBy', 'RecordStatus')";

            DataTable dt = new DataTable();

            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(qry, cnn))
                {
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        ColumnList columns = new ColumnList();
                        columns.ColumnName = dtrow["COLUMN_NAME"].ToString().Trim();
                        columns.ErrorValue = "";
                        details.Add(columns);
                    }
                    cnn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ColumnList columns = new ColumnList();
            columns.ColumnName = "";
            columns.ErrorValue = ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }
    [WebMethod]
    public static ColumnList[] GetVendorNameonblur(string Value, string Name)
    {
        List<ColumnList> details = new List<ColumnList>();
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
            SqlConnection cnn = new SqlConnection(constr);

            if (Name == "Equipment")
            {
                qry = "Select VendorName As [Values] From CFSEquipmentMaster Where EquipmentName = '" + Value + "' And RecordStatus = 'Active'";
            }
            if (Name == "Vendor")
            {
                qry = "Select EquipmentName As [Values] From CFSEquipmentMaster Where VendorName = '" + Value + "' And RecordStatus = 'Active'";
            }

            SqlCommand cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlDataReader sqlrd = cmd.ExecuteReader();
            while (sqlrd.Read())
            {
                ColumnList columns = new ColumnList();
                columns.ColumnName = sqlrd["Values"].ToString().Trim();
                columns.ErrorValue = "";
                details.Add(columns);
            }
            cnn.Close();
        }
        catch (Exception ex)
        {
            ColumnList columns = new ColumnList();
            columns.ColumnName = "";
            columns.ErrorValue = ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }
    [WebMethod]
    public static ColumnList[] GetContainerNoonblur(string Type, string Jobno)
    {
        List<ColumnList> details = new List<ColumnList>();
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = ""; //For Testing - LogistICSAProdTest, Real One is - LogistICSAProd            

            if (Type == "Seal Cutting" || Type == "Section-49" || Type == "Examination")
            {
                qry = "Select ContainerNo As [Values] From CFSImportContainer Where RecordStatus = 'Active' And MainJobNo = '" + Jobno + "' And ContainerStatus = 'Gated In';";
            }
            if (Type == "De-Stuffing" || Type == "FCL Cargo Out")
            {
                qry = "Select ContainerNo As [Values] From CFSImportContainer Where RecordStatus = 'Active' And MainJobNo = '" + Jobno + "' And ContainerDeliveryMode = 'Destuff Delivery' And ContainerStatus = 'Gated In';";
            }
            if (Type == "Loaded Container Out")
            {
                qry = @"Select ContainerNo As [Values] From CFSImportContainer Where RecordStatus = 'Active' And MainJobNo = '" + Jobno + "' And ContainerDeliveryMode = 'Loaded Delivery' And IsContainerEmpty = 'No' And ContainerStatus = 'Gated In' " +
                       "Union " +
                       "Select StuffStuffingContainerNo As [Values] From CFSimportstuffing Where RecordStatus = 'Active' And MainJobNo = '" + Jobno + "' And StuffingStatus = 'Fully Stuffed' And StuffWorkOrderStatus = 'Completed';";
            }
            if (Type == "Empty Container Out")
            {
                qry = "Select ContainerNo As [Values] From CFSImportContainer Where RecordStatus = 'Active' And MainJobNo = '" + Jobno + "' And IsContainerEmpty = 'Yes' And ContainerStatus = 'Gated In';";
            }

            DataTable dt = new DataTable();

            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(qry, cnn))
                {
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        ColumnList columns = new ColumnList();
                        columns.ColumnName = dtrow["Values"].ToString().Trim();
                        columns.ErrorValue = "";
                        details.Add(columns);
                    }
                    cnn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ColumnList columns = new ColumnList();
            columns.ColumnName = "";
            columns.ErrorValue = ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }
    public class ColumnList
    {
        public string ColumnName { get; set; }
        public string ColumnValues { get; set; }
        public string DataType { get; set; }
        public string ErrorValue { get; set; }
    }
    [WebMethod]
    public static string TSADataUpdate(List<TSADataList> TSADataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        List<LinerTableDataList> DetailedList = new List<LinerTableDataList>();
        LinerTableDataList DetailedListValue = new LinerTableDataList();
        try
        {
            foreach (var TSADataFieldsList in TSADataList)
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("SELECT * FROM CFSImportLiner WHERE MainJobNo = @MainJobNo and LinerItemNo = @LinerItemNo and LinerLinerAgent = @LinerLinerAgent and RecordStatus='Active'", cnn);
                cmd.Parameters.AddWithValue("@MainJobNo", TSADataFieldsList.MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@LinerItemNo", TSADataFieldsList.MainItemNo.Trim());
                cmd.Parameters.AddWithValue("@LinerLinerAgent", TSADataFieldsList.MainLineName.Trim());
                cnn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dtrow in dt.Rows)
                {
                    DetailedListValue.LinerTSANo = dtrow["LinerTSANo"].ToString();
                    if (dtrow["LinerTSADate"].ToString() != "" || dtrow["LinerTSADate"] != DBNull.Value) { DetailedListValue.LinerTSADate = Convert.ToDateTime(dtrow["LinerTSADate"]).ToString("yyyy-MM-dd HH:mm:ss"); }
                    else { DetailedListValue.LinerTSADate = ""; }
                }
                cnn.Close();

                cmd = new SqlCommand("Update CFSImportLiner Set ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, LinerTSANo = @LinerTSANo , LinerTSADate = @LinerTSADate, UpdatedBy=@UpdatedBy Where MainJobNo=@MainJobNo And LinerItemNo = @LinerItemNo And LinerLinerAgent = @LinerLinerAgent And RecordStatus = 'Active'", cnn);
                cmd.Parameters.AddWithValue("@LinerTSANo", TSADataFieldsList.MainTSANo.ToString());
                if (TSADataFieldsList.MainTSADate != "" && TSADataFieldsList.MainTSADate != null) { cmd.Parameters.AddWithValue("@LinerTSADate", Convert.ToDateTime(TSADataFieldsList.MainTSADate.Trim()).Date); }
                else { cmd.Parameters.AddWithValue("@LinerTSADate", DBNull.Value); }
                cmd.Parameters.AddWithValue("@LinerItemNo", TSADataFieldsList.MainItemNo.Trim());
                cmd.Parameters.AddWithValue("@LinerLinerAgent", TSADataFieldsList.MainLineName.Trim());
                cmd.Parameters.AddWithValue("@MainJobNo", TSADataFieldsList.MainJobNo.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@ComputerName", "");
                cmd.Parameters.AddWithValue("@IPAddress", "");
                cmd.Parameters.AddWithValue("@Location", "");
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                foreach (DataRow dtrow in dt.Rows)
                {
                    if (DetailedListValue.LinerTSANo != TSADataFieldsList.MainTSANo.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLiner",
                        TSADataFieldsList.MainItemNo.Trim(), "LinerTSANo", DetailedListValue.LinerTSANo,
                        TSADataFieldsList.MainTSANo.Trim(), "Modified", "MainJobNo = '" + TSADataFieldsList.MainJobNo.Trim() + "' And " +
                        "LinerItemNo = '" + TSADataFieldsList.MainItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                    if (DetailedListValue.LinerTSADate != TSADataFieldsList.MainTSADate.Trim())
                    {
                        InsertOrUpdateUmatchedValue("CFSImportLiner",
                        TSADataFieldsList.MainItemNo.Trim(), "LinerTSADate", DetailedListValue.LinerTSADate,
                        TSADataFieldsList.MainTSADate.Trim(), "Modified", "MainJobNo = '" + TSADataFieldsList.MainJobNo.Trim() + "' And " +
                        "LinerItemNo = '" + TSADataFieldsList.MainItemNo.Trim() + "' And RecordStatus = 'Active'",
                        HttpContext.Current.Session["UserName"].ToString());
                    }
                }
            }
            return "Updated";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class TSADataList
    {
        public string MainJobNo { get; set; }
        public string MainItemNo { get; set; }
        public string MainLineName { get; set; }
        public string MainTSANo { get; set; }
        public string MainTSADate { get; set; }
    }
    [WebMethod]
    public static string TemplateCreate(List<TemplateData> TemplateData, List<TemplateCondtionData> TemplateCondtionData)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Select Count(*) + 1 From ICSAColumnTemplates");
            cmd.Connection = cnn;
            cnn.Open();
            int RefIdCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
            cnn.Close();

            string QryCondition = ""; int q = 1;
            foreach (var TemplateCondDataField in TemplateCondtionData)
            {
                if (q == 1) { QryCondition += " Where "; }

                if (TemplateCondDataField.DataTypes == "varchar" || TemplateCondDataField.DataTypes == "nvarchar")
                {
                    if (TemplateCondDataField.Operators == "In") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " " + TemplateCondDataField.Operators + " " + "('" + TemplateCondDataField.Values.TrimEnd(',').Replace(",", "','") + "')"; }
                    else if (TemplateCondDataField.Operators == "Like") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " " + TemplateCondDataField.Operators + " " + "'%" + TemplateCondDataField.Values + "%'"; }
                    else { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " " + TemplateCondDataField.Operators + " " + "'" + TemplateCondDataField.Values + "'"; }
                }
                if (TemplateCondDataField.DataTypes == "date" || TemplateCondDataField.DataTypes == "datetime")
                {
                    if (TemplateCondDataField.Values == "Today") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(GETDATE() AS DATE) And CAST(GETDATE() AS DATE)"; }
                    if (TemplateCondDataField.Values == "Yesterday") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(DAY, -1, GETDATE()) AS DATE) And CAST(DATEADD(DAY, -1, GETDATE()) AS DATE)"; }
                    if (TemplateCondDataField.Values == "This Week") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), GETDATE()) AS DATE) And CAST(DATEADD(DAY, 7 - DATEPART(WEEKDAY, GETDATE()), GETDATE()) AS DATE)"; }
                    if (TemplateCondDataField.Values == "Last Week") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(DAY, -6 - DATEPART(WEEKDAY, GETDATE()), GETDATE()) AS DATE) And CAST(DATEADD(DAY, -DATEPART(WEEKDAY, GETDATE()), GETDATE()) AS DATE)"; }
                    if (TemplateCondDataField.Values == "This Month") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(DAY, 1 - DAY(GETDATE()), GETDATE()) AS DATE) And CAST(EOMONTH(GETDATE()) AS DATE)"; }
                    if (TemplateCondDataField.Values == "Last Month") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(MONTH, -1, DATEADD(DAY, 1 - DAY(GETDATE()), GETDATE())) AS DATE) And CAST(EOMONTH(GETDATE(), -1) AS DATE)"; }
                    if (TemplateCondDataField.Values == "This Calendar Year") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()), 0) AS DATE) And CAST(DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()) + 1, 0) - 1 AS DATE)"; }
                    if (TemplateCondDataField.Values == "Last Calendar Year") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CAST(DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()) - 1, 0) AS DATE) And CAST(DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()), 0) - 1 AS DATE)"; }
                    if (TemplateCondDataField.Values == "This Fiscal Year") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CASE WHEN MONTH(GETDATE()) >= 4 THEN CAST(DATEFROMPARTS(YEAR(GETDATE()), 4, 1) AS DATE) ELSE CAST(DATEFROMPARTS(YEAR(GETDATE()) - 1, 4, 1) AS DATE) END And CASE WHEN MONTH(GETDATE()) >= 4 THEN CAST(DATEFROMPARTS(YEAR(GETDATE()) + 1, 3, 31) AS DATE) ELSE CAST(DATEFROMPARTS(YEAR(GETDATE()), 3, 31) AS DATE) END"; }
                    if (TemplateCondDataField.Values == "Last Fiscal Year") { QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " Between " + "CASE WHEN MONTH(GETDATE()) >= 4 THEN CAST(DATEFROMPARTS(YEAR(GETDATE()) - 1, 4, 1) AS DATE) ELSE CAST(DATEFROMPARTS(YEAR(GETDATE()) - 2, 4, 1) AS DATE) END And CASE WHEN MONTH(GETDATE()) >= 4 THEN CAST(DATEFROMPARTS(YEAR(GETDATE()), 3, 31) AS DATE) ELSE CAST(DATEFROMPARTS(YEAR(GETDATE()) - 1, 3, 31) AS DATE) END"; }
                }
                if (TemplateCondDataField.DataTypes == "bigint" || TemplateCondDataField.DataTypes == "float" || TemplateCondDataField.DataTypes == "int" || TemplateCondDataField.DataTypes == "real")
                {
                    QryCondition += TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames + " " + TemplateCondDataField.Operators + " " + TemplateCondDataField.Values;
                }

                if (TemplateCondtionData.Count == q) { QryCondition += ";"; }
                else { QryCondition += " And "; }
                q++;
            }

            foreach (var TemplateDataField in TemplateData)
            {
                if (TemplateDataField.SaveorUpdate == "Save")
                {
                    qry = "Insert Into ICSAColumnTemplates (RefId, CompanyName, BranchName, Module, FormName, TemplateName, Query, UpdatedBy, ComputerName, IPAddress, Location) " +
                          "Values (@RefId, @CompanyName, @BranchName, @Module, @FormName, @TemplateName, @Query, @UpdatedBy, @ComputerName, @IPAddress, @Location)";

                    cmd = new SqlCommand("Select Count(*) From ICSAColumnTemplates Where TemplateName = '" + TemplateDataField.Templatename + "' And RecordStatus = 'Active'");
                    cmd.Connection = cnn;
                    cnn.Open();
                    int TemplateCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                    cnn.Close();

                    if (TemplateCount > 0)
                    {
                        return "Template Name already exists...";
                    }
                }
                if (TemplateDataField.SaveorUpdate == "Update")
                {
                    cmd = new SqlCommand("Select Convert(Int, RefId) As [RefId] From ICSAColumnTemplates Where TemplateName = '" + TemplateDataField.Templatename + "' And RecordStatus = 'Active'");
                    cmd.Connection = cnn;
                    cnn.Open();
                    RefIdCount = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                    cnn.Close();

                    qry = "Update ICSAColumnTemplates set RefId=@RefId, CompanyName=@CompanyName, BranchName=@BranchName, Module=@Module, " +
                          "Query=@Query, UpdatedBy=@UpdatedBy, ComputerName=@ComputerName, IPAddress=@IPAddress, Location=@Location " +
                          "Where FormName=@FormName And TemplateName=@TemplateName And Recordstatus = 'Active';";
                }

                cmd = new SqlCommand(qry, cnn);
                cmd.Parameters.AddWithValue("@RefId", RefIdCount.ToString());
                cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                cmd.Parameters.AddWithValue("@Module", TemplateDataField.ModuleName.Trim());
                cmd.Parameters.AddWithValue("@FormName", TemplateDataField.FormName.Trim());
                cmd.Parameters.AddWithValue("@TemplateName", TemplateDataField.Templatename.Trim());
                cmd.Parameters.AddWithValue("@Query", TemplateDataField.Query.Trim() + " " + QryCondition.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@ComputerName", "");
                cmd.Parameters.AddWithValue("@IPAddress", "");
                cmd.Parameters.AddWithValue("@Location", "");

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                InsertOrUpdateUmatchedValue("ICSAColumnTemplates", TemplateDataField.Templatename.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            }

            foreach (var TemplateCondDataField in TemplateCondtionData)
            {
                if (TemplateCondDataField.SaveorUpdate == "Update")
                {
                    cmd = new SqlCommand("Update ICSAColumnCondition Set RecordStatus = 'Deleted' Where RefId = @RefId;", cnn);
                    cmd.Parameters.AddWithValue("@RefId", RefIdCount.ToString());
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

                cmd = new SqlCommand("Insert Into ICSAColumnCondition (TableName, RefId, CompanyName, BranchName, ColumnName, DataType, Operator, ColumnValue, UpdatedBy, ComputerName, IPAddress, Location) " +
                          "Values (@tablename, @RefId, @CompanyName, @BranchName, @ColumnName, @DataType, @Operator, @ColumnValue, @UpdatedBy, @ComputerName, @IPAddress, @Location)", cnn);
                cmd.Parameters.AddWithValue("@RefId", RefIdCount.ToString());
                cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                cmd.Parameters.AddWithValue("@ColumnName", TemplateCondDataField.ColumnNames.Trim());
                cmd.Parameters.AddWithValue("@DataType", TemplateCondDataField.DataTypes.Trim());
                cmd.Parameters.AddWithValue("@Operator", TemplateCondDataField.Operators.Trim());
                cmd.Parameters.AddWithValue("@ColumnValue", TemplateCondDataField.Values.Trim());
                cmd.Parameters.AddWithValue("@tablename", TemplateCondDataField.TabNames.Trim());
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@ComputerName", "");
                cmd.Parameters.AddWithValue("@IPAddress", "");
                cmd.Parameters.AddWithValue("@Location", "");

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                InsertOrUpdateUmatchedValue("ICSAColumnCondition", TemplateCondDataField.TabNames + "." + TemplateCondDataField.ColumnNames.Trim() + "," + TemplateCondDataField.Operators.Trim() + "," + TemplateCondDataField.Values.Trim(), "", "", "", "Inserted", "", HttpContext.Current.Session["UserName"].ToString());
            }

            return "Saved";
        }
        catch (Exception ex)
        { return ex.Message; }
    }
    public class TemplateData
    {
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public string Templatename { get; set; }
        public string Query { get; set; }
        public string SaveorUpdate { get; set; }
    }
    public class TemplateCondtionData
    {
        public string ColumnNames { get; set; }
        public string SaveorUpdate { get; set; }
        public string DataTypes { get; set; }
        public string Operators { get; set; }
        public string Values { get; set; }
        public string TabNames { get; set; }
    }
    [WebMethod]
    public static ColumnList[] GetDatatype(string evalue, string ColumnTableName)
    {
        List<ColumnList> details = new List<ColumnList>();
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
            string qry = "Select Distinct " + evalue + " As [ColumnValues], " +
                         "(Select DATA_TYPE From INFORMATION_SCHEMA.COLUMNS Where Table_Name = '" + ColumnTableName + "' And COLUMN_NAME = '" + evalue + "') As [Datatype] " +
                         "From " + ColumnTableName + " Where RecordStatus = 'Active';";

            DataTable dt = new DataTable();

            using (SqlConnection cnn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(qry, cnn))
                {
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        ColumnList columns = new ColumnList();
                        if (dtrow["Datatype"].ToString().Trim() == "varchar" || dtrow["Datatype"].ToString().Trim() == "nvarchar") { columns.ColumnValues = dtrow["ColumnValues"].ToString().Trim(); }
                        columns.DataType = dtrow["Datatype"].ToString().Trim();
                        columns.ErrorValue = "";
                        details.Add(columns);
                    }
                    cnn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ColumnList columns = new ColumnList();
            columns.ColumnValues = "";
            columns.DataType = "";
            columns.ErrorValue = ex.Message;
            details.Add(columns);
        }

        return details.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public static Tuple<List<OutputList1>, List<OutputList2>> TemplateSearchData(string TemName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, Refid = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable dt = new DataTable();

        List<OutputList1> DetailedList1 = new List<OutputList1>();
        List<OutputList2> DetailedList2 = new List<OutputList2>();
        OutputList1 DetailedListValue1 = new OutputList1();
        OutputList2 DetailedListValue2 = new OutputList2();

        try
        {
            cmd = new SqlCommand("Select RefId, Query From ICSAColumnTemplates Where TemplateName = @TemplateName And RecordStatus = 'Active' And FormName = @FormName;", cnn);
            cmd.Parameters.AddWithValue("@TemplateName", TemName);
            cmd.Parameters.AddWithValue("@FormName", System.IO.Path.GetFileName(HttpContext.Current.Request.PhysicalPath));
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue1.ReturnedValue = "No Data Found!!!";
                DetailedList1.Add(DetailedListValue1);
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                if (dtrow["Query"].ToString() != "" && dtrow["Query"].ToString() != null)
                {
                    Refid = dtrow["RefId"].ToString();

                    string pattern = @"(?i)select\s+(.+?)\s+from";
                    Match match = Regex.Match(dtrow["Query"].ToString(), pattern);
                    if (match.Success)
                    {
                        foreach (var Column in match.Groups[1].Value.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            DetailedListValue1 = new OutputList1(); int tc = 1;
                            foreach (var TabandColumn in Column.Trim().Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (tc == 1) { DetailedListValue1.tabname = TabandColumn.Trim(); }
                                else
                                {
                                    bool Exists = false; int rc = 1;
                                    if (TabandColumn.Trim().Contains(" As ["))
                                    {
                                        foreach (var RenameColumn in TabandColumn.Trim().Split(new[] { " As [" }, StringSplitOptions.RemoveEmptyEntries))
                                        {
                                            if (rc == 1) { DetailedListValue1.colsname = RenameColumn.Trim(); }
                                            else { DetailedListValue1.RenameValue = RenameColumn.TrimEnd(']').Trim(); }
                                            Exists = true;
                                            rc++;
                                        }
                                    }
                                    if (!Exists) { DetailedListValue1.colsname = TabandColumn.Trim(); DetailedListValue1.RenameValue = ""; }
                                }
                                tc++;
                            }
                            DetailedList1.Add(DetailedListValue1);
                        }
                    }
                }

                break;
            }
            cnn.Close();

            cmd = new SqlCommand("Select ColumnName, DataType, Operator, ColumnValue, TableName From ICSAColumnCondition Where RefId = @RefId And RecordStatus = 'Active'", cnn);
            cmd.Parameters.AddWithValue("@RefId", Refid.Trim());
            cnn.Open();
            da = new SqlDataAdapter(cmd); dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cnn.Close();
                DetailedListValue2.ReturnedValue = "No Data Found!!!";
                DetailedList2.Add(DetailedListValue2);
            }
            foreach (DataRow dtrow in dt.Rows)
            {
                DetailedListValue2 = new OutputList2();
                DetailedListValue2.ColumnName = dtrow["ColumnName"].ToString();
                DetailedListValue2.DataType = dtrow["DataType"].ToString();
                DetailedListValue2.Operator = dtrow["Operator"].ToString();
                DetailedListValue2.ColumnValue = dtrow["ColumnValue"].ToString();
                DetailedListValue2.TableName = dtrow["TableName"].ToString();
                DetailedList2.Add(DetailedListValue2);
            }
            cnn.Close();
        }
        catch (Exception ex)
        {
            DetailedListValue1.ReturnedValue = ex.Message;
            DetailedList1.Add(DetailedListValue1);
        }

        return Tuple.Create(DetailedList1, DetailedList2);
    }

    [WebMethod]
    public static string LoadDataBasedonCont(string evalue)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "", JobNo = "";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd;

            qry = "Select MainJobNo From CFSImportContainer Where ContainerNo = @ContNo And RecordStatus = 'Active' And ContainerStatus = 'New'\r\n";
            cmd = new SqlCommand(qry, cnn);
            cmd.Parameters.AddWithValue("@ContNo", evalue);
            cnn.Open();
            SqlDataReader sqldr = cmd.ExecuteReader();
            while (sqldr.Read())
            {
                JobNo = sqldr["MainJobNo"].ToString(); break;
            }
            cnn.Close();

            return JobNo;
        }
        catch (Exception ex)
        {
            return "ErrorMessage : " + ex.Message;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string ReadingDataFromExcel(List<SheetDataList> SheetDataList, List<SheetData1List> SheetData1List, List<SheetData2List> SheetData2List)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlConnection cnnn = new SqlConnection(constr);
        SqlCommand cmd, cmdd;
        SqlDataReader sqlrd;

        try
        {
            ProcessList(SheetDataList); ProcessList(SheetData1List); ProcessList(SheetData2List);

            foreach (var GeneralTab in SheetDataList)
            {
                if (GeneralTab.IGMNo == "" || GeneralTab.IGMNo == null || GeneralTab.IGMNo == "undefined") { return "Kindly Enter the IGM No on Excel."; }

                foreach (var LinerTab in SheetData1List)
                {
                    if (LinerTab.LinerItemNo == "" || LinerTab.LinerItemNo == null || LinerTab.LinerItemNo == "undefined") { return "Kindly Enter the Liner No on Excel."; }

                    string JobNo = "", Prefix = "", Series = "", Suffix = ""; int Insert = 1;

                    qry = "Select MainJobNo From CFSImport Where MainIGMNo = '" + GeneralTab.IGMNo + "' And MainItemNo = '" + LinerTab.LinerItemNo + "' And RecordStatus = 'Active'";
                    cmd = new SqlCommand(qry, cnn);
                    cnn.Open();
                    sqlrd = null;
                    sqlrd = cmd.ExecuteReader();
                    while (sqlrd.Read())
                    {
                        if (sqlrd["MainJobNo"].ToString() != "" && sqlrd["MainJobNo"].ToString() != null) { JobNo = sqlrd["MainJobNo"].ToString(); Insert = 0; }
                    }
                    cnn.Close();

                    if (Insert == 1)
                    {
                        cmd = new SqlCommand("SELECT " +
                                             "    iif(b.MainJobNo is null,Prefix,JobNoPreFix) as JobNoPreFix, " +
                                             "    iif(b.MainJobNo is null,NumberSeries,JobNoSeries+1) as JobNoSeries, " +
                                             "    iif(b.MainJobNo is null,Suffix,FinancialYear) as FinancialYear, " +
                                             "    case when b.MainJobNo is null then " +
                                             "    Concat(Prefix,NumberSeries,'/',Suffix) " +
                                             "    else  " +
                                             "    Concat(JobNoPreFix,JobNoSeries+1,'/',Suffix)  end as MainJobNo " +
                                             "    FROM TransactionNumberFormat a left join ( " +
                                             "    select top 1 MainJobNo, JobNoPreFix,JobNoSeries, FinancialYear from CFSImport order by MainJobNo Desc ) b " + "    on a.Suffix = b.FinancialYear where Module = 'CFS' And FormName = 'Import' And " +
                                             "    Suffix = substring(datename(YEAR,  DATEADD(M,-3,GETDATE())),3,2) +'-'+ cast((datepart(YEAR,  DATEADD(M,-3,GETDATE())) + 1) %100 as varchar(2)); ", cnn);

                        cnn.Open();
                        sqlrd = null;
                        sqlrd = cmd.ExecuteReader();
                        while (sqlrd.Read())
                        {
                            JobNo = sqlrd["MainJobNo"].ToString();
                            Prefix = sqlrd["JobNoPreFix"].ToString();
                            Series = sqlrd["JobNoSeries"].ToString();
                            Suffix = sqlrd["FinancialYear"].ToString();
                        }
                        cnn.Close();
                    }

                    if (JobNo != "")
                    {
                        if (GeneralTab.IGMNo != "" && GeneralTab.IGMDate != "" && GeneralTab.Vessel != "" && GeneralTab.VIANo != "" && GeneralTab.VoyNo != "" && GeneralTab.POA != "")
                        {
                            //General Tab Insert
                            if (Insert == 1)
                            {
                                qry = "Insert Into CFSImport (CompanyName,BranchName,UpdatedBy,ComputerName,IPAddress,Location,MainJobNo, JobNoPreFix, JobNoSeries, FinancialYear, MainIGMNo, MainItemNo, MainIGMDate, MainVesselName, MainVIANo, MainVoyNo, MainPOA, GeneralPOD, GeneralAccountHolder, MainJobOwner) " +
                                      "Values (@CompanyName,@BranchName,@UpdatedBy,@ComputerName,@IPAddress,@Location,@JobNo, @JobNoPreFix, @JobNoSeries, @FinancialYear, @IGMNo, @ItemNo, @IGMDate, @Vessel, @VIANo, @VoyNo, @POA, @GeneralPortofDischarge, @GeneralAccountHolderName, @JobOwner)";
                            }
                            else
                            {
                                qry = "Update CFSImport Set CompanyName=@CompanyName,BranchName=@BranchName, UpdatedBy = @UpdatedBy, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, MainIGMDate=@IGMDate, MainVesselName=@Vessel, MainVIANo=@VIANo, MainVoyNo=@VoyNo, MainPOA=@POA, GeneralPOD=@GeneralPortofDischarge, " +
                                      "GeneralAccountHolder=@GeneralAccountHolderName, MainJobOwner=@JobOwner Where MainJobNo=@JobNo And MainIGMNo=@IGMNo And MainItemNo=@ItemNo And RecordStatus = 'Active'";
                            }

                            cmd = new SqlCommand(qry, cnn);
                            cmd.Parameters.AddWithValue("@JobNo", JobNo.Trim());
                            if (Insert == 1)
                            {
                                cmd.Parameters.AddWithValue("@JobNoPreFix", Prefix.Trim());
                                cmd.Parameters.AddWithValue("@JobNoSeries", Series.Trim());
                                cmd.Parameters.AddWithValue("@FinancialYear", Suffix.Trim());
                            }
                            cmd.Parameters.AddWithValue("@IGMNo", GeneralTab.IGMNo.Trim());
                            cmd.Parameters.AddWithValue("@ItemNo", LinerTab.LinerItemNo.Trim());
                            if (GeneralTab.IGMDate == "" || GeneralTab.IGMDate == null) { cmd.Parameters.AddWithValue("@IGMDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@IGMDate", Convert.ToDateTime(GeneralTab.IGMDate.Trim()).Date); }
                            cmd.Parameters.AddWithValue("@Vessel", GeneralTab.Vessel.Trim());
                            cmd.Parameters.AddWithValue("@VIANo", GeneralTab.VIANo.Trim());
                            cmd.Parameters.AddWithValue("@VoyNo", GeneralTab.VoyNo.Trim());
                            cmd.Parameters.AddWithValue("@POA", GeneralTab.POA.Trim());
                            cmd.Parameters.AddWithValue("@GeneralPortofDischarge", GeneralTab.GeneralPortofDischarge.Trim());
                            cmd.Parameters.AddWithValue("@GeneralAccountHolderName", GeneralTab.GeneralAccountHolderName.Trim());
                            cmd.Parameters.AddWithValue("@JobOwner", GeneralTab.JobOwner.Trim());
                            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                            cmd.Parameters.AddWithValue("@ComputerName", "");
                            cmd.Parameters.AddWithValue("@IPAddress", "");
                            cmd.Parameters.AddWithValue("@Location", "");
                            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            cnn.Close();
                        }
                        else
                        {
                            return "Kindly Enter all the required fields (*).";
                        }

                        if (LinerTab.LinerItemNo != "" && LinerTab.LinerImporterName != "" && LinerTab.LinerLinerAgent != "" && LinerTab.LinerBLNumber != "" && LinerTab.LinerWeightKgs != "" &&
                            LinerTab.LinerPackages != "")//&& Convert.ToInt32(LinerTab.LinerWeightKgs) != 0 && Convert.ToInt32(LinerTab.LinerPackages) != 0)
                        {

                            qry = @"Select 
                                    A.EntityDesc
                                    From Entity A 
                                    Left Join EntityDocs B On A.EntityName = B.EntityName And A.RecordStatus = B.RecordStatus And B.GeneralDocumentType = 'Pan No'
                                    Left Join EntityType C On A.EntityName = C.EntityName And A.RecordStatus = C.RecordStatus
                                    Where B.GeneralDocumentNo = '" + LinerTab.LinerPANNumber.Trim() + @"' And A.RecordStatus = 'Active' And C.EntityType Like '%Line%';";

                            cmd = new SqlCommand(qry);
                            cmd.Connection = cnn;
                            cnn.Open();
                            SqlDataReader sqldr = cmd.ExecuteReader();
                            while (sqldr.Read())
                            {
                                if (sqldr["EntityDesc"].ToString().Trim().ToUpper() != LinerTab.LinerLinerAgent.Trim().ToUpper()) { LinerTab.LinerLinerAgent = ""; }
                            }
                            cnn.Close();

                            //Liner Tab Insert
                            if (Insert == 1)
                            {
                                qry = "Insert Into CFSImportLiner(CompanyName,BranchName,ComputerName,IPAddress,Location,UpdatedBy,MainJobNo,LinerItemNo,LinerImporterName,LinerCHAName,LinerLinerAgent,LinerBLNo,LinerBLDate,LinerIMDG,LinerWeightKg,LinerPKG,LinerCargoDetails,LinerTSANo,LinerTSADate,LinerPANNo) " +
                                      "Values (@CompanyName,@BranchName,@ComputerName,@IPAddress,@Location,@UpdatedBy,@JobNo,@LinerItemNo,@LinerImporterName,@LinerCHAName,@LinerLinerAgent,@LinerBLNumber,@LinerBLDate,@LinerIMDG,@LinerWeightKgs,@LinerPackages,@LinerCargoDetails,@LinerTSANumber,@LinerTSADate,@LinerPANNo)";
                            }
                            else
                            {
                                qry = "Select Count(*) As [DupCheck] From CFSImportLiner Where MainJobNo = '" + JobNo + "' And LinerItemNo = '" + LinerTab.LinerItemNo + "' And RecordStatus = 'Active'";
                                cmd = new SqlCommand(qry);
                                cmd.Connection = cnn;
                                cnn.Open();
                                int Count = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                                cnn.Close();

                                if (Count > 0)
                                {
                                    qry = "Update CFSImportLiner set CompanyName=@CompanyName,BranchName=@BranchName, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, UpdatedBy = @UpdatedBy, LinerPANNo=@LinerPANNo, LinerCHAName=@LinerCHAName, LinerImporterName=@LinerImporterName,LinerLinerAgent=@LinerLinerAgent,LinerBLNo=@LinerBLNumber,LinerBLDate=@LinerBLDate,LinerIMDG=@LinerIMDG,LinerWeightKg=@LinerWeightKgs,LinerPKG=@LinerPackages,LinerCargoDetails=@LinerCargoDetails,LinerTSANo=@LinerTSANumber,LinerTSADate=@LinerTSADate " +
                                          "where LinerItemNo = @LinerItemNo And MainJobNo = @JobNo And RecordStatus = 'Active'";
                                }
                                else
                                {
                                    qry = "Insert Into CFSImportLiner(CompanyName,BranchName,ComputerName,IPAddress,Location,UpdatedBy,MainJobNo,LinerItemNo,LinerImporterName,LinerCHAName,LinerLinerAgent,LinerBLNo,LinerBLDate,LinerIMDG,LinerWeightKg,LinerPKG,LinerCargoDetails,LinerTSANo,LinerTSADate,LinerPANNo) " +
                                          "Values (@CompanyName,@BranchName,@ComputerName,@IPAddress,@Location,@UpdatedBy,@JobNo,@LinerItemNo,@LinerImporterName,@LinerCHAName,@LinerLinerAgent,@LinerBLNumber,@LinerBLDate,@LinerIMDG,@LinerWeightKgs,@LinerPackages,@LinerCargoDetails,@LinerTSANumber,@LinerTSADate,@LinerPANNo)";
                                }
                            }

                            cmd = new SqlCommand(qry, cnn);
                            cmd.Parameters.AddWithValue("@JobNo", JobNo.Trim());
                            cmd.Parameters.AddWithValue("@LinerPANNo", LinerTab.LinerPANNumber.Trim());
                            cmd.Parameters.AddWithValue("@LinerItemNo", LinerTab.LinerItemNo.Trim());
                            cmd.Parameters.AddWithValue("@LinerImporterName", LinerTab.LinerImporterName.Trim());
                            cmd.Parameters.AddWithValue("@LinerCHAName", LinerTab.LinerCHAName.Trim());
                            cmd.Parameters.AddWithValue("@LinerLinerAgent", LinerTab.LinerLinerAgent.Trim());
                            cmd.Parameters.AddWithValue("@LinerBLNumber", LinerTab.LinerBLNumber.Trim());
                            if (LinerTab.LinerBLDate == "" || LinerTab.LinerBLDate == null) { cmd.Parameters.AddWithValue("@LinerBLDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@LinerBLDate", Convert.ToDateTime(LinerTab.LinerBLDate.Trim()).Date); }
                            cmd.Parameters.AddWithValue("@LinerIMDG", LinerTab.LinerIMDG.Trim());
                            cmd.Parameters.AddWithValue("@LinerWeightKgs", LinerTab.LinerWeightKgs.Trim());
                            cmd.Parameters.AddWithValue("@LinerPackages", LinerTab.LinerPackages.Trim());
                            cmd.Parameters.AddWithValue("@LinerCargoDetails", LinerTab.LinerCargoDetails.Trim());
                            cmd.Parameters.AddWithValue("@LinerTSANumber", LinerTab.LinerTSANumber.Trim());
                            if (LinerTab.LinerTSADate == "" || LinerTab.LinerTSADate == null) { cmd.Parameters.AddWithValue("@LinerTSADate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@LinerTSADate", Convert.ToDateTime(LinerTab.LinerTSADate.Trim()).Date); }
                            cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                            cmd.Parameters.AddWithValue("@ComputerName", "");
                            cmd.Parameters.AddWithValue("@IPAddress", "");
                            cmd.Parameters.AddWithValue("@Location", "");
                            cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                            cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            cnn.Close();
                        }
                        else
                        {
                            return "Kindly Enter all the required fields (*).";
                        }
                    }

                    foreach (var ContainerTab in SheetData2List)
                    {
                        if (ContainerTab.ContainerContNo == "" || ContainerTab.ContainerContNo == null || ContainerTab.ContainerContNo == "undefined") { return "Kindly Enter the Liner & Container No on Excel."; }

                        if (LinerTab.LinerItemNo == ContainerTab.ContainerItemNo)
                        {
                            if (ContainerTab.ContainerItemNo != "" && ContainerTab.ContainerContNo != "" && ContainerTab.ContainerISOCode != "" &&
                                ContainerTab.ContainerSealNo != "" && ContainerTab.ContainerWeightKgs != "" && ContainerTab.ContainerNoofPackage != "" && ContainerTab.ContainerFCLLCL != "")                                // && Convert.ToInt32(ContainerTab.ContainerWeightKgs) != 0 && Convert.ToInt32(ContainerTab.ContainerNoofPackage) != 0)
                            {
                                //Container Tab Insert
                                if (Insert == 1)
                                {
                                    qry = "Insert Into CFSImportContainer (CompanyName,BranchName,ComputerName,IPAddress,Location,UpdatedBy,ContainerCargoNature,ContainerPriority,MainJobNo,ContainerItemNo,ContainerNo,ContainerISOCode,ContainerSize,ContainerType,ContainerSealNo,ContainerTareWeight,ContainerWeightKg,ContainerCargoWeightKg,ContainerNoofPackage,ContainerFCLLCL,ContainerPrimarySecondary,ContainerGroupCode,ContainerIMOCode,ContainerUNNo,ContainerScanType,ContainerScanLocation,ContainerDeliveryMode,ContainerHold,ContainerHoldRemarks,ContainerHoldAgency,ContainerHoldDate,ContainerReleaseDate,ContainerReleaseRemarks,ContainerClaimDetails,ContainerClaimAmount,ContainerPaymentDate,ContainerRemarks,ContainerWHLoc) " +
                                          "Values(@CompanyName,@BranchName,@ComputerName,@IPAddress,@Location,@UpdatedBy,@CargoNature,@Priority,@JobNo,@ContainerItemNo,@ContainerContNo,@ContainerISOCode,@ContainerSize,@ContainerType,@ContainerSealNo,@ContainerTareWeight,@ContainerWeightKg,@ContainerCargoWeightKgs,@ContainerNoofPackage,@ContainerFCLLCL,@ContainerPrimSec,@ContainerGroupCode,@ContainerIMOCode,@ContainerUNNo,@ContainerScanType,@ContainerScanLocation,@ContainerDeliveryMode,@ContainerHold,@ContainerHoldRemarks,@ContainerHoldAgency,@ContainerHoldDate,@ContainerReleaseDate,@ContainerReleaseRemarks,@ContainerClaimDetails,@ContainerClaimAmount,@ContainerPaymentDate,@ContainerRemarks,@ContainerWHLoc)";
                                }
                                else
                                {
                                    qry = "Select Count(*) As [DupCheck] From CFSImportContainer Where ContainerNo = '" + ContainerTab.ContainerContNo + "' And MainJobNo = '" + JobNo + "' And ContainerItemNo = '" + LinerTab.LinerItemNo + "' And RecordStatus = 'Active'";
                                    cmd = new SqlCommand(qry);
                                    cmd.Connection = cnn;
                                    cnn.Open();
                                    int Count = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                                    cnn.Close();

                                    if (Count > 0)
                                    {
                                        qry = "Update CFSImportContainer set HoldBy=@HoldBy,CompanyName=@CompanyName,BranchName=@BranchName, ComputerName = @ComputerName,IPAddress = @IPAddress,Location = @Location, UpdatedBy = @UpdatedBy, ContainerWeightKg=@ContainerWeightKg,ContainerCargoNature=@CargoNature,ContainerPriority=@Priority,ContainerISOCode=@ContainerISOCode,ContainerSize=@ContainerSize,ContainerType=@ContainerType,ContainerSealNo=@ContainerSealNo," +
                                              "ContainerTareWeight=@ContainerTareWeight,ContainerCargoWeightKg=@ContainerCargoWeightKgs,ContainerNoofPackage=@ContainerNoofPackage," +
                                              "ContainerFCLLCL=@ContainerFCLLCL,ContainerPrimarySecondary=@ContainerPrimSec,ContainerGroupCode=@ContainerGroupCode,ContainerIMOCode=@ContainerIMOCode," +
                                              "ContainerUNNo=@ContainerUNNo,ContainerScanType=@ContainerScanType,ContainerScanLocation=@ContainerScanLocation,ContainerDeliveryMode=@ContainerDeliveryMode," +
                                              "ContainerHold=@ContainerHold,ContainerHoldRemarks=@ContainerHoldRemarks,ContainerHoldAgency=@ContainerHoldAgency,ContainerHoldDate=@ContainerHoldDate," +
                                              "ContainerReleaseDate=@ContainerReleaseDate,ContainerReleaseRemarks=@ContainerReleaseRemarks,ContainerClaimDetails=@ContainerClaimDetails," +
                                              "ContainerClaimAmount=@ContainerClaimAmount,ContainerPaymentDate=@ContainerPaymentDate,ContainerRemarks=@ContainerRemarks,ContainerWHLoc=@ContainerWHLoc " +
                                              "where MainJobNo = @JobNo And ContainerItemNo = @ContainerItemNo And ContainerNo = @ContainerContNo And RecordStatus = 'Active'";
                                    }
                                    else
                                    {
                                        qry = "Insert Into CFSImportContainer (HoldBy,CompanyName,BranchName,ComputerName,IPAddress,Location,UpdatedBy,ContainerCargoNature,ContainerPriority,MainJobNo,ContainerItemNo,ContainerNo,ContainerISOCode,ContainerSize,ContainerType,ContainerSealNo,ContainerTareWeight,ContainerWeightKg,ContainerCargoWeightKg,ContainerNoofPackage,ContainerFCLLCL,ContainerPrimarySecondary,ContainerGroupCode,ContainerIMOCode,ContainerUNNo,ContainerScanType,ContainerScanLocation,ContainerDeliveryMode,ContainerHold,ContainerHoldRemarks,ContainerHoldAgency,ContainerHoldDate,ContainerReleaseDate,ContainerReleaseRemarks,ContainerClaimDetails,ContainerClaimAmount,ContainerPaymentDate,ContainerRemarks,ContainerWHLoc) " +
                                              "Values(@HoldBy,@CompanyName,@BranchName,@ComputerName,@IPAddress,@Location,@UpdatedBy,@CargoNature,@Priority,@JobNo,@ContainerItemNo,@ContainerContNo,@ContainerISOCode,@ContainerSize,@ContainerType,@ContainerSealNo,@ContainerTareWeight,@ContainerWeightKg,@ContainerCargoWeightKgs,@ContainerNoofPackage,@ContainerFCLLCL,@ContainerPrimSec,@ContainerGroupCode,@ContainerIMOCode,@ContainerUNNo,@ContainerScanType,@ContainerScanLocation,@ContainerDeliveryMode,@ContainerHold,@ContainerHoldRemarks,@ContainerHoldAgency,@ContainerHoldDate,@ContainerReleaseDate,@ContainerReleaseRemarks,@ContainerClaimDetails,@ContainerClaimAmount,@ContainerPaymentDate,@ContainerRemarks,@ContainerWHLoc)";
                                    }
                                }

                                cmd = new SqlCommand(qry, cnn);
                                cmd.Parameters.AddWithValue("@JobNo", JobNo.Trim());
                                cmd.Parameters.AddWithValue("@CargoNature", ContainerTab.CargoNature.Trim());
                                cmd.Parameters.AddWithValue("@Priority", ContainerTab.ContainerPriority.Trim());
                                //cmd.Parameters.AddWithValue("@IGMNo", GeneralTab.IGMNo.Trim());
                                cmd.Parameters.AddWithValue("@ContainerItemNo", LinerTab.LinerItemNo.Trim());
                                cmd.Parameters.AddWithValue("@ContainerContNo", ContainerTab.ContainerContNo.Trim());
                                cmd.Parameters.AddWithValue("@ContainerISOCode", ContainerTab.ContainerISOCode.Trim());

                                cmdd = new SqlCommand("Select ContainerSize, ContainerType, ContainerTareWeight From CFSImportISOCodeMaster Where ContainerISOCode = '" + ContainerTab.ContainerISOCode.Trim() + "' And RecordStatus = 'Active'", cnnn);
                                cnnn.Open();
                                SqlDataReader sqldrd = cmdd.ExecuteReader();
                                while (sqldrd.Read())
                                {
                                    ContainerTab.ContainerSize = sqldrd["ContainerSize"].ToString();
                                    ContainerTab.ContainerType = sqldrd["ContainerType"].ToString();
                                    ContainerTab.ContainerTareWeight = sqldrd["ContainerTareWeight"].ToString();
                                    break;
                                }
                                cnnn.Close();

                                if (!string.IsNullOrEmpty(ContainerTab.ContainerWeightKgs) &&
                                    !string.IsNullOrEmpty(ContainerTab.ContainerTareWeight))
                                {
                                    float containerWeightKgs;
                                    float containerTareWeight;

                                    if (float.TryParse(ContainerTab.ContainerWeightKgs.Trim(), out containerWeightKgs) &&
                                        float.TryParse(ContainerTab.ContainerTareWeight.Trim(), out containerTareWeight))
                                    {
                                        float cargoWeight = containerWeightKgs - containerTareWeight;
                                        ContainerTab.ContainerCargoWeightKgs = cargoWeight.ToString();
                                    }
                                }

                                cmd.Parameters.AddWithValue("@ContainerSize", ContainerTab.ContainerSize.Trim());
                                cmd.Parameters.AddWithValue("@ContainerType", ContainerTab.ContainerType.Trim());
                                cmd.Parameters.AddWithValue("@ContainerSealNo", ContainerTab.ContainerSealNo.Trim());
                                cmd.Parameters.AddWithValue("@ContainerTareWeight", ContainerTab.ContainerTareWeight.Trim());
                                cmd.Parameters.AddWithValue("@ContainerWeightKg", ContainerTab.ContainerWeightKgs.Trim());
                                cmd.Parameters.AddWithValue("@ContainerCargoWeightKgs", ContainerTab.ContainerCargoWeightKgs.Trim());
                                cmd.Parameters.AddWithValue("@ContainerNoofPackage", ContainerTab.ContainerNoofPackage.Trim());
                                cmd.Parameters.AddWithValue("@ContainerFCLLCL", ContainerTab.ContainerFCLLCL.Trim());
                                cmd.Parameters.AddWithValue("@ContainerPrimSec", ContainerTab.ContainerPrimSec.Trim());
                                cmd.Parameters.AddWithValue("@ContainerGroupCode", ContainerTab.ContainerGroupCode.Trim());
                                cmd.Parameters.AddWithValue("@ContainerIMOCode", ContainerTab.ContainerIMOCode.Trim());
                                cmd.Parameters.AddWithValue("@ContainerUNNo", ContainerTab.ContainerUNNo.Trim());
                                cmd.Parameters.AddWithValue("@ContainerScanType", ContainerTab.ContainerScanType.Trim());
                                cmd.Parameters.AddWithValue("@ContainerScanLocation", ContainerTab.ContainerScanLocation.Trim());
                                cmd.Parameters.AddWithValue("@ContainerDeliveryMode", ContainerTab.ContainerDeliveryMode.Trim());
                                cmd.Parameters.AddWithValue("@ContainerHold", ContainerTab.ContainerHold.Trim());
                                if (ContainerTab.ContainerHold.Trim() == "Yes")
                                {
                                    cmd.Parameters.AddWithValue("@HoldBy", HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString());
                                }
                                else { cmd.Parameters.AddWithValue("@HoldBy", DBNull.Value); }
                                cmd.Parameters.AddWithValue("@ContainerHoldRemarks", ContainerTab.ContainerHoldRemarks.Trim());
                                cmd.Parameters.AddWithValue("@ContainerHoldAgency", ContainerTab.ContainerHoldAgency.Trim());
                                if (ContainerTab.ContainerHoldDate == "" || ContainerTab.ContainerHoldDate == null) { cmd.Parameters.AddWithValue("@ContainerHoldDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@ContainerHoldDate", Convert.ToDateTime(ContainerTab.ContainerHoldDate.Trim()).Date); }
                                if (ContainerTab.ContainerReleaseDate == "" || ContainerTab.ContainerReleaseDate == null) { cmd.Parameters.AddWithValue("@ContainerReleaseDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@ContainerReleaseDate", Convert.ToDateTime(ContainerTab.ContainerReleaseDate.Trim()).Date); }
                                cmd.Parameters.AddWithValue("@ContainerReleaseRemarks", ContainerTab.ContainerReleaseRemarks.Trim());
                                cmd.Parameters.AddWithValue("@ContainerClaimDetails", ContainerTab.ContainerClaimDetails.Trim());
                                cmd.Parameters.AddWithValue("@ContainerClaimAmount", ContainerTab.ContainerClaimAmount.Trim());
                                if (ContainerTab.ContainerPaymentDate == "" || ContainerTab.ContainerPaymentDate == null) { cmd.Parameters.AddWithValue("@ContainerPaymentDate", DBNull.Value); } else { cmd.Parameters.AddWithValue("@ContainerPaymentDate", Convert.ToDateTime(ContainerTab.ContainerPaymentDate.Trim()).Date); }
                                cmd.Parameters.AddWithValue("@ContainerRemarks", ContainerTab.ContainerRemarks.Trim());
                                cmd.Parameters.AddWithValue("@ContainerWHLoc", ContainerTab.ContainerWHLoc.Trim());
                                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                                cmd.Parameters.AddWithValue("@ComputerName", "");
                                cmd.Parameters.AddWithValue("@IPAddress", "");
                                cmd.Parameters.AddWithValue("@Location", "");
                                cmd.Parameters.AddWithValue("@CompanyName", HttpContext.Current.Session["COMPANY"].ToString());
                                cmd.Parameters.AddWithValue("@BranchName", HttpContext.Current.Session["BRANCH"].ToString());
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }
                            else
                            {
                                return "Kindly Enter all the required fields (*).";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "Saved";
    }
    public class SheetDataList
    {
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string Vessel { get; set; }
        public string VIANo { get; set; }
        public string VoyNo { get; set; }
        public string POA { get; set; }
        public string GeneralPortofDischarge { get; set; }
        public string GeneralCHAName { get; set; }
        public string GeneralAccountHolderName { get; set; }
        public string JobOwner { get; set; }
    }
    public class SheetData2List
    {
        public string ContainerPriority { get; set; }
        public string ContainerItemNo { get; set; }
        public string ContainerContNo { get; set; }
        public string CargoNature { get; set; }
        public string ContainerISOCode { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public string ContainerSealNo { get; set; }
        public string ContainerTareWeight { get; set; }
        public string ContainerWeightKgs { get; set; }
        public string ContainerCargoWeightKgs { get; set; }
        public string ContainerNoofPackage { get; set; }
        public string ContainerFCLLCL { get; set; }
        public string ContainerPrimSec { get; set; }
        public string ContainerGroupCode { get; set; }
        public string ContainerIMOCode { get; set; }
        public string ContainerUNNo { get; set; }
        public string ContainerScanType { get; set; }
        public string ContainerScanLocation { get; set; }
        public string ContainerDeliveryMode { get; set; }
        public string ContainerHold { get; set; }
        public string ContainerHoldRemarks { get; set; }
        public string ContainerHoldAgency { get; set; }
        public string ContainerHoldDate { get; set; }
        public string ContainerReleaseDate { get; set; }
        public string ContainerReleaseRemarks { get; set; }
        public string ContainerClaimDetails { get; set; }
        public string ContainerClaimAmount { get; set; }
        public string ContainerPaymentDate { get; set; }
        public string ContainerRemarks { get; set; }
        public string ContainerWHLoc { get; set; }
    }
    [WebMethod(EnableSession = true)]
    public static string[] GettheModalStructure(string JobNoValue, string ElementId)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString, qry = "", ErrorMsg = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        StringBuilder htmlTable = new StringBuilder();
        try
        {
            if (ElementId == "LoadOutViewBtn") { qry = "Select Distinct Container_No As [Container No], BL_No As [BL No], BL_Date As [BL Date], BOE_No As [BOE No], BOE_Date As [BOE Date], Item_Wise_Pkgs As [Total Pkgs], Item_Wise_Weight_Kgs As [Weight Kgs] From CFSImportContainerwisedata Where MainJobNo = '" + JobNoValue + "' And RecordStatus = 'Active';"; }
            if (ElementId == "LoadContInViewBtn") { qry = "Select Distinct Container_No As [Container No], Planned_Transport_Name As [Transporter Name], Cargo_Details As [Cargo Description], Type As [Container Type], Line_Name, Liner_Agent, Importer_Name, Terminal_Gate_Out As [Port Out Date Time], Driver_Name, Size As [Container Size], Cargo_Nature From CFSImportContainerWiseData Where MainJobNo = '" + JobNoValue + "' And RecordStatus = 'Active';"; }
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlDataReader sqldr = cmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                htmlTable.Append("<div class='table-container'><table class='table-bordered'>");
                //Headers
                htmlTable.Append("<thead><tr>");
                for (int i = 0; i < sqldr.FieldCount; i++)
                {
                    htmlTable.Append("<th>").Append(sqldr.GetName(i).Replace("_", " ")).Append("</th>");
                }
                htmlTable.Append("</tr></thead><tbody>");
                //Rows                
                while (sqldr.Read())
                {
                    htmlTable.Append("<tr>");
                    for (int i = 0; i < sqldr.FieldCount; i++)
                    {
                        htmlTable.Append("<td>").Append(sqldr[i].ToString()).Append("</td>");
                    }
                    htmlTable.Append("</tr>");
                }
                htmlTable.Append("</tbody></table></div>");
            }
            else
            {
                htmlTable.Append("<p>No data found</p>");
            }
            htmlTable.Append("<button class='close-btn' type='button' onclick='closeModal()'>Close</button>");
            cnn.Close();
        }
        catch (Exception ex)
        {
            ErrorMsg = ex.Message;
        }
        return new string[] { ErrorMsg, htmlTable.ToString() };
    }
    [WebMethod(EnableSession = true)]
    public static object GetDataStructure(string Year, string Month)
    {
        string BarChartOp = "", PieChartOp = "";
        string Error = "";

        string constr = ConfigurationManager.ConnectionStrings["LogistICSAProd"].ConnectionString;
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader rd;

        try
        {
            string sqlqry = @"
                         --Gated Out Containers Count Mon Wise
                         Begin
                         DECLARE @Year INT = " + Year + @";

                         WITH MonthData AS (
                             SELECT 
                                 MONTH(DATEADD(MONTH, Number - 1, DATEFROMPARTS(@Year, 1, 1))) AS MonthNumber,
                                 DATEADD(MONTH, Number - 1, DATEFROMPARTS(@Year, 1, 1)) AS FromDate,
                                 EOMONTH(DATEADD(MONTH, Number - 1, DATEFROMPARTS(@Year, 1, 1))) AS ToDate
                             FROM (VALUES (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)) AS T(Number)
                         )
                         , MonthlyCounts AS (
                             SELECT 
                                 M.MonthNumber,
                                 COUNT(Distinct YT.Gate_Out_Container_No) AS ContainerCount
                             FROM MonthData M
                             LEFT JOIN CFSImportContainerWiseData YT 
                                 ON YT.RecordStatus = 'Active' 
                                 AND YT.Gate_Out_Pass_No <> '' 
                                 AND YT.Gate_Out_Pass_No IS NOT NULL 
                                 AND YT.Mode_Of_Gate_Out = 'Loaded Container Out' 
                                 AND YT.Gate_Out_Pass_Date BETWEEN M.FromDate AND M.ToDate
                             GROUP BY M.MonthNumber
                         )
                         SELECT STRING_AGG(ContainerCount, ', ') As [Final_Output]
                         FROM MonthlyCounts;
                         End;

                         Begin
                         Declare @StartDate Date, @EndDate Date;
                         Set @StartDate = DATEFROMPARTS(YEAR(GETDATE()), " + Month + @", 1);
                         Set @EndDate = EOMONTH(@StartDate);

                         Select 
                            (Select Cast(Count(Distinct EmptyTorCGateInPassNo) As varchar) From CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' And EmptyTorCGateInPassNo <> '' And 
                            EmptyTorCGateInPassNo Is NoT Null And EmptyTorCGateInPassDate Between @StartDate And @EndDate And EmptyTorCGateInMode <> 'Empty Container In') 
                            + ', ' + 
                            (Select Cast(Count(Distinct EmptyTorCGateInPassNo) As varchar) From CFSImportEmptyTruckorContainer Where RecordStatus = 'Active' And EmptyTorCGateInPassNo <> '' And 
                            EmptyTorCGateInPassNo Is NoT Null And EmptyTorCGateInPassDate Between @StartDate And @EndDate And EmptyTorCGateInMode = 'Empty Container In') 
                            + ', ' + 
                            (Select Cast(Count(Distinct Container_No) As varchar) From CFSImportContainerWiseData Where RecordStatus = 'Active' And Gate_In_Pass_No <> '' And 
                            Gate_In_Pass_No Is NoT Null And Gate_In_Pass_Date Between @StartDate And @EndDate) 
                            + ', ' + 
                            (Select Cast(Count(Distinct Container_No) As varchar) From CFSImportContainerWiseData Where RecordStatus = 'Active' And Seal_Cutting_WO_No <> '' And 
                            Seal_Cutting_WO_No Is Not Null And Seal_Cutting_WO_Date Between @StartDate And @EndDate And Seal_Cutting_WO_Status = 'Completed') 
                            + ', ' + 
                            (Select Cast(Count(Distinct Container_No) As varchar) From CFSImportContainerWiseData Where RecordStatus = 'Active' And Examination_WO_No <> '' And 
                            Examination_WO_No Is Not Null And Examination_WO_Date Between @StartDate And @EndDate And Examination_WO_Status = 'Completed') 
                            + ', ' + 
                            (Select Cast(Count(Distinct Container_No) As varchar) From CFSImportContainerWiseData Where RecordStatus = 'Active' And [Section-49_WO_No] <> '' And 
                            [Section-49_WO_No] Is Not Null And [Section-49_WO_Date] Between @StartDate And @EndDate And [Section-49_WO_Status] = 'Completed') 
                            + ', ' + 
                            (Select Cast(Count(Distinct Container_No) As varchar) From CFSImportContainerWiseData Where RecordStatus = 'Active' And [De-Stuff_WO_No] <> '' And 
                            [De-Stuff_WO_No] Is Not Null And [De-Stuff_WO_Date] Between @StartDate And @EndDate And [De-Stuff_WO_Status] = 'Completed') 
                            + ', ' + 
                            (Select Cast(Count(Distinct LoadTruckGateOutPassNo) As varchar) From CFSImportLoadedTruck Where RecordStatus = 'Active' And LoadTruckGateOutPassNo <> '' And LoadTruckGateOutPassNo Is 
                            Not Null And LoadTruckGateOutPassDate Between @StartDate And @EndDate And LoadTruckModeofGateOut = 'Loaded Container Out') 
                            + ', ' + 
                            (Select Cast(Count(Distinct LoadTruckGateOutPassNo) As varchar) From CFSImportLoadedTruck Where RecordStatus = 'Active' And LoadTruckGateOutPassNo <> '' And LoadTruckGateOutPassNo Is 
                            Not Null And LoadTruckGateOutPassDate Between @StartDate And @EndDate And LoadTruckModeofGateOut = 'FCL Cargo Out') 
                            + ', ' + 
                            (Select Cast(Count(Distinct LoadTruckGateOutPassNo) As varchar) From CFSImportLoadedTruck Where RecordStatus = 'Active' And LoadTruckGateOutPassNo <> '' And LoadTruckGateOutPassNo Is 
                            Not Null And LoadTruckGateOutPassDate Between @StartDate And @EndDate And LoadTruckModeofGateOut = 'Empty Container Out') 
                            + ', ' + 
                            (Select Cast(Count(Distinct LoadTruckGateOutPassNo) As varchar) From CFSImportLoadedTruck Where RecordStatus = 'Active' And LoadTruckGateOutPassNo <> '' And LoadTruckGateOutPassNo Is 
                            Not Null And LoadTruckGateOutPassDate Between @StartDate And @EndDate And LoadTruckModeofGateOut = 'Empty Truck Out');
                         End;";

            cnn = new SqlConnection(constr);
            cmd = new SqlCommand();
            cnn.Open();
            cmd.CommandText = sqlqry;
            cmd.Connection = cnn;
            rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                int resultSetCount = 0;

                do
                {
                    resultSetCount++;

                    while (rd.Read())
                    {
                        if (resultSetCount == 1) { BarChartOp = rd[0].ToString(); }
                        if (resultSetCount == 2) { PieChartOp = rd[0].ToString(); }
                    }
                }
                while (rd.NextResult());
            }

            cnn.Close();
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return new
        {
            ErrorMsg = Error,
            Bar = BarChartOp,
            Pie = PieChartOp
        };
    }
    //-------------------------------Added Manually------------------------------------
}