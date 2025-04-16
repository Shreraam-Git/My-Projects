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
public partial class Formula1_CircuitsDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        YearValue.Text = DateTime.Now.Year.ToString();
        Year.Text = DateTime.Now.Year.ToString();

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "GettingCircuitsDataFromDB();", true);       
    }    
    [WebMethod]
    public static string GettingCircuitsValuesFromAPI(string UserID, string Year, string Race)
    {
        if (Year != "" && UserID != "")
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, url = "";
            SqlConnection cnn = new SqlConnection(constr);

            using (HttpClient client = new HttpClient())
            {
                if (Race != "") { url = "http://ergast.com/api/f1/" + Year + "/" + Race + "/circuits"; }
                else { url = "http://ergast.com/api/f1/" + Year + "/circuits"; }

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

                            var Circuits = xmlDoc.Descendants(ns + "Circuit");

                            SqlCommand cmd = new SqlCommand("SELECT COUNT(YEAR) AS [MAXCOUNT] FROM F1_MASTER_CIRCUITS WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                            cnn.Open();
                            SqlDataReader sqlreader = null;
                            sqlreader = cmd.ExecuteReader();
                            sqlreader.Read();
                            int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
                            cnn.Close();

                            if (DupCount > 0)
                            {
                                cmd = new SqlCommand("UPDATE F1_MASTER_CIRCUITS SET STATUS = 'Deleted' WHERE YEAR = '" + Year + "'", cnn);
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }

                            foreach (var Circuit in Circuits)
                            {
                                cmd = new SqlCommand("INSERT INTO F1_MASTER_CIRCUITS (YEAR, CIRCUIT_ID, CIRCUIT_BOI_URL, CIRCUIT_NAME, CIRCUIT_LATITUDE, CIRCUIT_LONGITUDE, CIRCUIT_LOCALITY, CIRCUIT_COUNTRY, UPDATED_BY)" +
                                                                " VALUES (@year, @circuit_id, @circuit_boi_url, @circuit_name, @circuit_latitude, @circuit_longitude, @circuit_locality, @circuit_country, @ub)", cnn);

                                cmd.Parameters.AddWithValue("@year", Year);
                                cmd.Parameters.AddWithValue("@circuit_id", Circuit.Attribute("circuitId").Value);
                                cmd.Parameters.AddWithValue("@circuit_boi_url", Circuit.Attribute("url").Value);
                                cmd.Parameters.AddWithValue("@circuit_name", Circuit.Element(ns + "CircuitName").Value);

                                var locationElement = Circuit.Element(ns + "Location");

                                cmd.Parameters.AddWithValue("@circuit_latitude", locationElement.Attribute("lat").Value);
                                cmd.Parameters.AddWithValue("@circuit_longitude", locationElement.Attribute("long").Value);
                                cmd.Parameters.AddWithValue("@circuit_locality", locationElement.Element(ns + "Locality").Value);
                                cmd.Parameters.AddWithValue("@circuit_country", locationElement.Element(ns + "Country").Value);
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
    public static string GettingCircuitDataFromDB(string Year)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;

        string qry = "SELECT * FROM F1_MASTER_CIRCUITS WHERE YEAR = '" + Year + "' AND STATUS = 'Active'";
        DataTable DT = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT); int i = 1;

        foreach (DataRow dtrow in DT.Rows)
        {
            string ImageName = dtrow["CIRCUIT_FILE_NAME"].ToString(), ImageUrl = "../assets/images/Formula1/CircuitsImages/", Style = "", Style1 = "", MouseOverFunc = "";

            ConditionBuliding.Append("<div class='col-3'>");
            ConditionBuliding.Append("<div class='card'>");
            ConditionBuliding.Append("<div class='card-body'>");

            ConditionBuliding.Append("<div class='ThreeDotdropdown'>");
            ConditionBuliding.Append("<button type='button' class='dropbtn' onmouseover='toggleDropdown(" + '"' + "myDropdown" + i + "" + '"' + ")'>⋮</button>");
            ConditionBuliding.Append("<div class='ThreeDotdropdown-content' id='myDropdown" + i + "'>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "image-container" + i + "" + '"' + ", " + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "NotDelete" + '"' + ")'><b>Remove Image</b></a><div style='margin:-18px 0 -18px 0;'><hr/></div>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles('', " + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ", " + '"' + "Delete" + '"' + ")'><b>Delete Circuit</b></a>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("</div>");

            ConditionBuliding.Append("<div class='d-flex justify-content-center align-items-center flex-column'>");
            if (ImageName == "") { Style = "style='display:none;'"; }
            else { Style1 = "style='display:none;"; MouseOverFunc = "oncontextmenu='onmouseoverFunc(event, this)' title='Right click to Zoom' onmouseout='onmouseoutFunc(this)'"; }
            ConditionBuliding.Append("<div " + MouseOverFunc + " class='round-div' id='image-container" + i + "'>");
            ConditionBuliding.Append("<img " + Style + " id='uploaded-image" + i + "' src='" + ImageUrl + "" + ImageName + "' alt='Circuit Image'>");
            ConditionBuliding.Append("<div " + Style1 + " class='upload-icon' id='upload-icon" + i + "'>Upload Image</div>");
            ConditionBuliding.Append("<input onchange='handleImageUpload(" + '"' + "image-container" + i + "" + '"' + "," + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", event, " + '"' + "uploaded-image" + i + "" + '"' + ", " + '"' + "upload-icon" + i + "" + '"' + ")' type='file' accept='image/*' id='image-upload" + i + "'>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("<h3 class='mt-3' style='text-align:center;'>" + dtrow["CIRCUIT_NAME"].ToString() + "</h3>");
            ConditionBuliding.Append("<a href='https://maps.google.com/?q=" + dtrow["CIRCUIT_LATITUDE"].ToString() + "," + dtrow["CIRCUIT_LONGITUDE"].ToString() + "' target='_blank'><h4 style='color:blue;' class='mt-0'>Location - <i class='bi bi-geo-alt-fill'></i></h4></a>");
            ConditionBuliding.Append("<p class='text-small'><b>Locality - " + dtrow["CIRCUIT_LOCALITY"].ToString() + "</b></p>");
            ConditionBuliding.Append("<p class='text-small' style='margin-top: -10px;'><b>Country - " + dtrow["CIRCUIT_COUNTRY"].ToString() + "</b></p>");
            ConditionBuliding.Append("<button type='button' style='margin-top: -10px;' data-bs-toggle='modal' data-bs-target='#ImagePreview' class='custom-btn Animebtn-5' onclick='AppendIframe(" + '"' + "" + dtrow["CIRCUIT_BOI_URL"].ToString() + "" + '"' + ")'><span>Read More &#x27A4;</span></button>");            
            ConditionBuliding.Append("</div></div></div></div>");

            i++;
        }

        return ConditionBuliding.ToString();
    }

    [WebMethod]
    public static string UpdateCircuitsName(string ActualDriverName, string ImageID, string DeleteCode)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
            SqlConnection cnn;
            SqlCommand cmd;
            SqlDataReader SqlReader;

            qry = "SELECT CIRCUIT_FILE_NAME FROM F1_MASTER_CIRCUITS WHERE ID = '" + ImageID + "' AND STATUS = 'Active';";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                if (SqlReader["CIRCUIT_FILE_NAME"].ToString() != "" && SqlReader["CIRCUIT_FILE_NAME"].ToString() != null)
                {
                    // Local Path
                    string path = @"C:/Users/LOGISTICSA-04/OneDrive - ICSA I Pvt Ltd/ICSA Project BackUp/LogistICSA/LogistICSA/assets/images/Formula1/CircuitsImages/" + SqlReader["CIRCUIT_FILE_NAME"].ToString() + "";
                    //string path = @"D:/Formula1/assets/images/Formula1/CircuitsImages/" + SqlReader["CIRCUIT_FILE_NAME"].ToString() + ""; // Prod Server Path

                    FileInfo file = new FileInfo(path);
                    if (file.Exists)//check file exist or not
                    {
                        file.Delete();
                    }
                }
            }
            cnn.Close();

            if (DeleteCode == "Delete") { qry = "UPDATE F1_MASTER_CIRCUITS SET STATUS = 'Deleted' WHERE ID = '" + ImageID + "';"; }
            else { qry = "UPDATE F1_MASTER_CIRCUITS SET CIRCUIT_FILE_NAME = '" + ActualDriverName + "' WHERE ID = '" + ImageID + "' AND STATUS = 'Active';"; }

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
    public static string CircuitsInsertData(List<DataList> DataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable export = new DataTable();

        try
        {
            foreach (var DriverDataFields in DataList)
            {
                cmd = new SqlCommand("INSERT INTO F1_MASTER_CIRCUITS (YEAR, CIRCUIT_ID, CIRCUIT_BOI_URL, CIRCUIT_NAME, CIRCUIT_LATITUDE, CIRCUIT_LONGITUDE, CIRCUIT_LOCALITY, CIRCUIT_COUNTRY, UPDATED_BY)" +
                                     " VALUES (@year, @circuit_id, @circuit_boi_url, @circuit_name, @circuit_latitude, @circuit_longitude, @circuit_locality, @circuit_country, @ub)", cnn);

                cmd.Parameters.AddWithValue("@year", DriverDataFields.Year.Trim());
                cmd.Parameters.AddWithValue("@circuit_id", DriverDataFields.CircuitCode.Trim());
                cmd.Parameters.AddWithValue("@circuit_boi_url", DriverDataFields.CircuitBioUrl.Trim());
                cmd.Parameters.AddWithValue("@circuit_name", DriverDataFields.CircuitName.Trim());
                cmd.Parameters.AddWithValue("@circuit_latitude", DriverDataFields.CircuitLatitude.Trim());
                cmd.Parameters.AddWithValue("@circuit_longitude", DriverDataFields.CircuitLongitude.Trim());
                cmd.Parameters.AddWithValue("@circuit_locality", DriverDataFields.Locality.Trim());
                cmd.Parameters.AddWithValue("@circuit_country", DriverDataFields.Country.Trim());
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
        public string UserName { get; set; }
        public string CircuitCode { get; set; }
        public string CircuitBioUrl { get; set; }
        public string CircuitName { get; set; }
        public string CircuitLatitude { get; set; }
        public string CircuitLongitude { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
    }
}