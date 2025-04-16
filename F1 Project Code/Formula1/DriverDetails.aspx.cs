using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Services;
using System.Xml.Linq;

public partial class Formula1_DriverDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        YearValue.Text = DateTime.Now.Year.ToString();
        Year.Text = DateTime.Now.Year.ToString();

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "GettingDriverDataFromDB();", true);
    }
    [WebMethod]
    public static string GettingDriverValuesFromAPI(string UserID, string Year, string Race)
    {
        if (Year != "" && UserID != "")
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, url = "";
            SqlConnection cnn = new SqlConnection(constr);

            using (HttpClient client = new HttpClient())
            {
                if (Race != "") { url = "http://ergast.com/api/f1/" + Year + "/" + Race + "/drivers"; }
                else { url = "http://ergast.com/api/f1/" + Year + "/drivers"; }

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

                            var drivers = xmlDoc.Descendants(ns + "Driver");                            

                            SqlCommand cmd = new SqlCommand("SELECT COUNT(YEAR) AS [MAXCOUNT] FROM F1_MASTER_DRIVER WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                            cnn.Open();
                            SqlDataReader sqlreader = null;
                            sqlreader = cmd.ExecuteReader();
                            sqlreader.Read();
                            int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
                            cnn.Close();

                            if (DupCount > 0)
                            {
                                cmd = new SqlCommand("UPDATE F1_MASTER_DRIVER SET STATUS = 'Deleted' WHERE YEAR = '" + Year + "'", cnn);
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }

                            foreach (var driver in drivers)
                            {
                                string DateofBirth = driver.Element(ns + "DateOfBirth").Value;

                                cmd = new SqlCommand("INSERT INTO F1_MASTER_DRIVER (YEAR, DRIVER_CODE, DRIVER_BIO_URL, DRIVER_NUMBER, DRIVER_FIRST_NAME, DRIVER_LAST_NAME, DRIVER_DOB, DRIVER_NATIONALITY, UPDATED_BY)" +
                                                                " VALUES (@year, @code, @BioUrl, @DriverNumber, @Firstname, @Lastname, @DateofBirth, @Nationality, @ub)", cnn);

                                cmd.Parameters.AddWithValue("@year", Year);
                                cmd.Parameters.AddWithValue("@code", driver.Attribute("code").Value);
                                cmd.Parameters.AddWithValue("@BioUrl", driver.Attribute("url").Value);
                                cmd.Parameters.AddWithValue("@DriverNumber", driver.Element(ns + "PermanentNumber").Value);
                                cmd.Parameters.AddWithValue("@Firstname", driver.Element(ns + "GivenName").Value);
                                cmd.Parameters.AddWithValue("@Lastname", driver.Element(ns + "FamilyName").Value);
                                if (DateofBirth == "" || DateofBirth == null)
                                {
                                    cmd.Parameters.AddWithValue("@DateofBirth", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@DateofBirth", Convert.ToDateTime(DateofBirth).Date);
                                }
                                cmd.Parameters.AddWithValue("@Nationality", driver.Element(ns + "Nationality").Value);
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
    public static string GettingDriverDataFromDB(string Year)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;

        string qry = "SELECT * FROM F1_MASTER_DRIVER WHERE YEAR = '" + Year + "' AND STATUS = 'Active'";
        DataTable DT = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT); int i = 1;

        foreach (DataRow dtrow in DT.Rows)
        {
            string ImageName = dtrow["FILE_NAME"].ToString(), ImageUrl = "../assets/images/Formula1/DriversImages/", Style = "", Style1 = "";

            ConditionBuliding.Append("<div class='col-3'>");
            ConditionBuliding.Append("<div class='card'>");
            ConditionBuliding.Append("<div class='card-body'>");

            ConditionBuliding.Append("<div class='ThreeDotdropdown'>");
            ConditionBuliding.Append("<button type='button' class='dropbtn' onmouseover='toggleDropdown(" + '"' + "myDropdown" + i + "" + '"' + ")'>⋮</button>");
            ConditionBuliding.Append("<div class='ThreeDotdropdown-content' id='myDropdown" + i + "'>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "NotDelete" + '"' + ")'><b>Remove Image</b></a><div style='margin:-18px 0 -18px 0;'><hr/></div>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "Delete" + '"' + ")'><b>Delete Driver</b></a>");
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
            ConditionBuliding.Append("<h3 class='mt-3' style='text-align:center;'>" + dtrow["DRIVER_FIRST_NAME"].ToString() + " " + dtrow["DRIVER_LAST_NAME"].ToString() + "</h3>");
            ConditionBuliding.Append("<h3 class='mt-0'>" + dtrow["DRIVER_NUMBER"].ToString() + "</h3>");
            ConditionBuliding.Append("<p class='text-small'><b>Date of Birth - " + Convert.ToDateTime(dtrow["DRIVER_DOB"].ToString()).ToString("dd/MM/yyyy") + "</b></p>");
            ConditionBuliding.Append("<p class='text-small' style='margin-top: -10px;'><b>Nationality - " + dtrow["DRIVER_NATIONALITY"].ToString() + "</b></p>");            
            ConditionBuliding.Append("<button type='button' style='margin-top: -10px;' data-bs-toggle='modal' data-bs-target='#ImagePreview' class='custom-btn Animebtn-5' onclick='AppendIframe(" + '"' + "" + dtrow["DRIVER_BIO_URL"].ToString() + "" + '"' + ")'><span>Read More &#x27A4;</span></button>");
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

            qry = "SELECT FILE_NAME FROM F1_MASTER_DRIVER WHERE ID = '" + ImageID + "' AND STATUS = 'Active';";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                if (SqlReader["FILE_NAME"].ToString() != "" && SqlReader["FILE_NAME"].ToString() != null)
                {
                    // Local Path
                    string path = @"C:/Users/LOGISTICSA-04/OneDrive - ICSA I Pvt Ltd/ICSA Project BackUp/LogistICSA/LogistICSA/assets/images/Formula1/DriversImages/" + SqlReader["FILE_NAME"].ToString() + "";
                    //string path = @"D:/Formula1/assets/images/Formula1/DriversImages/" + SqlReader["FILE_NAME"].ToString() + ""; // Prod Server Path

                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exist or not
                    {
                        file.Delete();
                    }
                }
            }
            cnn.Close();

            if (DeleteCode == "Delete") { qry = "UPDATE F1_MASTER_DRIVER SET STATUS = 'Deleted' WHERE ID = '" + ImageID + "';"; }
            else { qry = "UPDATE F1_MASTER_DRIVER SET FILE_NAME = '" + ActualDriverName + "' WHERE ID = '" + ImageID + "' AND STATUS = 'Active';"; }

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
                cmd = new SqlCommand("INSERT INTO F1_MASTER_DRIVER(YEAR,DRIVER_CODE,DRIVER_BIO_URL,DRIVER_NUMBER,DRIVER_FIRST_NAME,DRIVER_LAST_NAME,DRIVER_DOB,DRIVER_NATIONALITY,	UPDATED_BY)" +
                 " VALUES (@year,@drivercode,@driverurl,@driverno,@fname,@lname,@dob,@nation,@ub)", cnn);

                cmd.Parameters.AddWithValue("@year", DriverDataFields.Year.Trim());
                cmd.Parameters.AddWithValue("@drivercode", DriverDataFields.DriverCode.Trim());
                cmd.Parameters.AddWithValue("@driverurl", DriverDataFields.DriverBioUrl.Trim());
                cmd.Parameters.AddWithValue("@driverno", DriverDataFields.DriverNo.Trim());
                cmd.Parameters.AddWithValue("@fname", DriverDataFields.FirstName.Trim());
                cmd.Parameters.AddWithValue("@lname", DriverDataFields.LastName.Trim());
                if (DriverDataFields.DOB == "") { cmd.Parameters.AddWithValue("@dob", DBNull.Value); }
                else { cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(DriverDataFields.DOB.Trim()).Date); }
                cmd.Parameters.AddWithValue("@nation", DriverDataFields.Nationality.Trim());
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
        public string DriverNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Nationality { get; set; }
        public string UserName { get; set; }

    }
}