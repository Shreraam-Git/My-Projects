using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Formula1_ConstructorDetails : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        YearValue.Text = DateTime.Now.Year.ToString();
        Year.Text = DateTime.Now.Year.ToString();

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "GettingConstructorDataFromDB();", true);

        qry = "SELECT DISTINCT ID, 'Driver Name' AS [DDNAME], DRIVER_FIRST_NAME +' '+ DRIVER_LAST_NAME AS [DDVALUE] FROM F1_MASTER_DRIVER WHERE STATUS = 'Active';";

        DataTable dt = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        cnn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        DataStorage.DriverData = dt;
        cnn.Close();
        string DriverName = "";

        foreach (DataRow dtrow in dt.Rows)
        {
            if (dtrow["DDNAME"].ToString() == "Driver Name")
            {
                DriverName += "<option value='" + dtrow["DDVALUE"].ToString() + "'>" + dtrow["DDVALUE"].ToString() + "</option>";
            }
        }
        DriverNamePlaceHolder.Controls.Add(new Literal { Text = DriverName });
    }
    public static class DataStorage
    {
        public static DataTable DriverData { get; set; }
    }
    [WebMethod]
    public static string GettingConstructorsValuesFromAPI(string UserID, string Year, string Race)
    {
        if (Year != "" && UserID != "")
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, url = "";
            SqlConnection cnn = new SqlConnection(constr);

            using (HttpClient client = new HttpClient())
            {
                if (Race != "") { url = "http://ergast.com/api/f1/" + Year + "/" + Race + "/constructors"; }
                else { url = "http://ergast.com/api/f1/" + Year + "/constructors"; }

                client.BaseAddress = new Uri(url);

                try
                {
                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string xmlResponse = response.Content.ReadAsStringAsync().Result;

                        // Check if the content type is XML
                        if (response.Content.Headers.ContentType.MediaType == "application/xml" ||
                            response.Content.Headers.ContentType.MediaType == "text/xml")
                        {
                            XDocument xmlDoc = XDocument.Parse(xmlResponse);
                            XNamespace ns = "http://ergast.com/mrd/1.5";

                            var Constructor = xmlDoc.Descendants(ns + "Constructor");

                            SqlCommand cmd = new SqlCommand("SELECT COUNT(YEAR) AS [MAXCOUNT] FROM F1_MASTER_CONSTRUCTORS WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                            cnn.Open();
                            SqlDataReader sqlreader = null;
                            sqlreader = cmd.ExecuteReader();
                            sqlreader.Read();
                            int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
                            cnn.Close();

                            if (DupCount > 0)
                            {
                                cmd = new SqlCommand("UPDATE F1_MASTER_CONSTRUCTORS SET STATUS = 'Deleted' WHERE YEAR = '" + Year + "'", cnn);
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }

                            foreach (var Values in Constructor)
                            {
                                cmd = new SqlCommand("INSERT INTO F1_MASTER_CONSTRUCTORS (YEAR, CONSTRUCTOR_ID, CONSTRUCTOR_BOI_URL, CONSTRUCTOR_NAME, CONSTRUCTOR_NATIONALITY, UPDATED_BY)" +
                                                     " VALUES (@year, @constructor_id, @constructor_boi_url, @constructor_name, @constructor_nationality, @ub)", cnn);

                                cmd.Parameters.AddWithValue("@year", Year);
                                cmd.Parameters.AddWithValue("@constructor_id", Values.Attribute("constructorId").Value);
                                cmd.Parameters.AddWithValue("@constructor_boi_url", Values.Attribute("url").Value);
                                cmd.Parameters.AddWithValue("@constructor_name", Values.Element(ns + "Name").Value);
                                cmd.Parameters.AddWithValue("@constructor_nationality", Values.Element(ns + "Nationality").Value);
                                cmd.Parameters.AddWithValue("@ub", UserID);
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }
                        }
                        else
                        {
                            return "Response is not XML";
                        }
                    }
                    else
                    {
                        return "Error in API Contact - " + response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    return "Error in Code - " + ex.Message;
                }
            }
        }
        else
        {
            return "Year and User ID is Null";
        }

        return "Success";
    }
    [WebMethod]
    public static string GettingConstructorDataFromDB(string Year)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;

        string qry = "SELECT * FROM F1_MASTER_CONSTRUCTORS WHERE YEAR = '" + Year + "' AND STATUS = 'Active'";
        DataTable DT = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT); int i = 1;

        foreach (DataRow dtrow in DT.Rows)
        {
            string ImageName = dtrow["FILE_NAME"].ToString(), ImageUrl = "../assets/images/Formula1/ConstructorsImage/", Style = "", Style1 = "", ImageTag = "";

            ConditionBuliding.Append("<div class='col-3'>");
            ConditionBuliding.Append("<div class='card'>");
            ConditionBuliding.Append("<div class='card-body'>");

            ConditionBuliding.Append("<div class='ThreeDotdropdown'>");
            ConditionBuliding.Append("<button type='button' class='dropbtn' onmouseover='toggleDropdown(" + '"' + "myDropdown" + i + "" + '"' + ")'>⋮</button>");
            ConditionBuliding.Append("<div class='ThreeDotdropdown-content' id='myDropdown" + i + "'>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "NotDelete" + '"' + ")'><b>Remove Image</b></a><div style='margin:-18px 0 -18px 0;'><hr/></div>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "Delete" + '"' + ")'><b>Delete Team</b></a>");            
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("</div>");

            ConditionBuliding.Append("<div class='d-flex justify-content-center align-items-center flex-column'>");
            ConditionBuliding.Append("<div class='round-div' id='image-container" + i + "'>");
            if (ImageName == "") { Style = "style='display:none;'"; }
            else { Style1 = "style='display:none;"; }
            ConditionBuliding.Append("<img " + Style + " id='uploaded-image" + i + "' src='" + ImageUrl + "" + ImageName + "' alt='Driver Image'>");
            ConditionBuliding.Append("<div " + Style1 + " class='upload-icon' id='upload-icon" + i + "'>Upload Image</div>");
            ConditionBuliding.Append("<input onchange='handleImageUpload(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", event, " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ")' type='file' accept='image/*' id='image-upload" + i + "'>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("<h3 class='mt-3' style='margin-bottom:-10px; text-align:center;'>" + dtrow["CONSTRUCTOR_NAME"].ToString() + "</h3>");

            bool Exists = false;
            if (dtrow["DRIVERS_NAME"].ToString() != "" && dtrow["DRIVERS_NAME"].ToString() != null)
            {
                qry = "SELECT CASE WHEN FILE_NAME = '' OR FILE_NAME IS NULL THEN 'images.png' ELSE FILE_NAME END AS [FILE_NAME], " +
                      "CONCAT_WS('\n', " +
                      "CAST('Name - ' + DRIVER_FIRST_NAME +' '+ DRIVER_LAST_NAME AS VARCHAR), CAST('Number - ' + DRIVER_NUMBER AS VARCHAR), CONCAT('Date of Birth - ', CAST(DRIVER_DOB AS VARCHAR)), CAST('Nationality - ' + DRIVER_NATIONALITY AS VARCHAR)) AS [DETAILS], " +
                      "CASE WHEN DRIVER_BIO_URL = '' OR DRIVER_BIO_URL IS NULL THEN '#' ELSE DRIVER_BIO_URL END AS [DRIVER_BIO_URL] " +
                      "FROM F1_MASTER_DRIVER WHERE ID IN (" + dtrow["DRIVERS_NAME"].ToString() + ") AND STATUS = 'Active'";

                cnn = new SqlConnection(constr);
                cmd = new SqlCommand(qry, cnn);
                cnn.Open();
                SqlDataReader SqlReader = cmd.ExecuteReader();
                while (SqlReader.Read())
                {
                    ImageTag += "<a href='" + SqlReader["DRIVER_BIO_URL"].ToString() + "' target='_blank'><img title='" + SqlReader["DETAILS"].ToString() + "' height='180' title='' width='180' src='../assets/images/Formula1/DriversImages/" + SqlReader["FILE_NAME"].ToString() + "' /></a>&nbsp;&nbsp;";
                    Exists = true;
                }
            }

            if (Exists)
            {
                ConditionBuliding.Append("<div style='margin-bottom:10px;' class='direction-dropdown-default mt-1'>");
                ConditionBuliding.Append("<div class='btn-group dropup me-1 mb-1'>");
                ConditionBuliding.Append("<h4 style='cursor: pointer' class='mt-3' data-bs-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><u>Drivers</u></h4>");
                ConditionBuliding.Append("<div style='height: 200px; width: 400px; padding: 15px 15px 15px 15px; background-color: whitesmoke;' class='dropdown-menu'>");
                ConditionBuliding.Append(ImageTag);
                ConditionBuliding.Append("</div></div></div>");
            }
            else
            {
                ConditionBuliding.Append("<h4 style='color:red; cursor: pointer; margin-bottom:23px;' data-bs-toggle='modal' data-bs-target='#MapDriversSummaryDetailsPopup' onclick='MaptheDriver(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ")' class='mt-3' data-bs-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><u>No Drivers Maped</u></h4>");
            }

            ConditionBuliding.Append("<p class='text-small' style='margin-top: -10px;'><b>Nationality - " + dtrow["CONSTRUCTOR_NATIONALITY"].ToString() + "</b></p>");
            ConditionBuliding.Append("<button type='button' style='margin-top: -10px;' data-bs-toggle='modal' data-bs-target='#ImagePreview' class='custom-btn Animebtn-5' onclick='AppendIframe(" + '"' + "" + dtrow["CONSTRUCTOR_BOI_URL"].ToString() + "" + '"' + ")'><span>Read More &#x27A4;</span></button>");            
            ConditionBuliding.Append("</div></div></div></div>");

            i++;
        }

        return ConditionBuliding.ToString();
    }

    [WebMethod]
    public static string UpdateDriverName(string ActualDriverName, string ImageID, string DeleteCode)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
            SqlConnection cnn;
            SqlCommand cmd;
            SqlDataReader SqlReader;

            qry = "SELECT FILE_NAME FROM F1_MASTER_CONSTRUCTORS WHERE ID = '" + ImageID + "' AND STATUS = 'Active';";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                if (SqlReader["FILE_NAME"].ToString() != "" && SqlReader["FILE_NAME"].ToString() != null)
                {
                    // Local Path
                    string path = @"C:/Users/LOGISTICSA-04/OneDrive - ICSA I Pvt Ltd/ICSA Project BackUp/LogistICSA/LogistICSA/assets/images/Formula1/ConstructorsImage/" + SqlReader["FILE_NAME"].ToString() + "";
                    //string path = @"D:/Formula1/assets/images/Formula1/ConstructorsImage/" + SqlReader["FILE_NAME"].ToString() + ""; // Prod Server Path

                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exist or not
                    {
                        file.Delete();
                    }
                }
            }
            cnn.Close();

            if (DeleteCode == "Delete") { qry = "UPDATE F1_MASTER_CONSTRUCTORS SET STATUS = 'Deleted' WHERE ID = '" + ImageID + "';"; }
            else { qry = "UPDATE F1_MASTER_CONSTRUCTORS SET FILE_NAME = '" + ActualDriverName + "' WHERE ID = '" + ImageID + "' AND STATUS = 'Active';"; }

            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        catch (Exception Ex)
        {
            return Ex.Message;
        }

        return "Updated";
    }

    [WebMethod(EnableSession = true)]
    public static string DriverInsertData(List<DataList> DataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable export = new DataTable();

        try
        {
            foreach (var DriverDataFields in DataList)
            {
                cmd = new SqlCommand("INSERT INTO F1_MASTER_CONSTRUCTORS (YEAR, CONSTRUCTOR_ID, CONSTRUCTOR_BOI_URL, CONSTRUCTOR_NAME, CONSTRUCTOR_NATIONALITY, UPDATED_BY)" +
                                                     " VALUES (@year, @constructor_id, @constructor_boi_url, @constructor_name, @constructor_nationality, @ub)", cnn);

                cmd.Parameters.AddWithValue("@year", DriverDataFields.Year.Trim());
                cmd.Parameters.AddWithValue("@constructor_id", DriverDataFields.DriverCode.Trim());
                cmd.Parameters.AddWithValue("@constructor_boi_url", DriverDataFields.DriverBioUrl.Trim());
                cmd.Parameters.AddWithValue("@constructor_name", DriverDataFields.FirstName.Trim());
                cmd.Parameters.AddWithValue("@constructor_nationality", DriverDataFields.Nationality.Trim());
                cmd.Parameters.AddWithValue("@ub", DriverDataFields.UserName.Trim());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
        catch (Exception ex)
        { return ex.Message; }

        return "Saved";
    }

    public class DataList
    {
        public string Year { get; set; }
        public string DriverCode { get; set; }
        public string DriverBioUrl { get; set; }
        public string FirstName { get; set; }
        public string Nationality { get; set; }
        public string UserName { get; set; }

    }
    [WebMethod(EnableSession = true)]
    public static string UpdateMapDriverSave(string DriversName, string CostructorID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, DriverID = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable export = new DataTable();

        try
        {
            foreach (var DriverName in DriversName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (DataRow dtrow in DataStorage.DriverData.Rows)
                {
                    if (dtrow["DDNAME"].ToString() == "Driver Name")
                    {
                        if (dtrow["DDVALUE"].ToString().Trim() == DriverName.Trim())
                        {
                            DriverID += dtrow["ID"].ToString() + ",";
                        }
                    }
                }
            }

            cmd = new SqlCommand("UPDATE F1_MASTER_CONSTRUCTORS SET DRIVERS_NAME = @DriverName WHERE STATUS = 'Active' AND ID = @ConstructorId;", cnn);

            cmd.Parameters.AddWithValue("@DriverName", DriverID.Remove(DriverID.Length - 1).Trim());
            cmd.Parameters.AddWithValue("@ConstructorId", CostructorID.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        catch (Exception ex)
        { return ex.Message; }

        return "Updated";
    }
}