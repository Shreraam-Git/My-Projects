using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Formula1_RaceResult : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        qry = "SELECT 'Drivers' AS [DETAILS], FILE_NAME AS [Image Name], DRIVER_NUMBER AS [Driver_Constructor ID], DRIVER_FIRST_NAME + ' ' + DRIVER_LAST_NAME AS [Driver_Constructor Name], " +
          "DRIVER_BIO_URL AS [Driver_Constructor Url] FROM F1_MASTER_DRIVER WHERE YEAR = YEAR(GETDATE()) AND STATUS = 'Active' " +
          "UNION " +
          "SELECT 'Constructors' AS [DETAILS], FILE_NAME, '@' + CONSTRUCTOR_ID, CONSTRUCTOR_NAME, CONSTRUCTOR_BOI_URL FROM F1_MASTER_CONSTRUCTORS " +
          "WHERE YEAR = YEAR(GETDATE()) AND STATUS = 'Active' " +
          "ORDER BY 1,2";

        DataTable dt = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        StringBuilder DriversMasterDetailsStr = new StringBuilder();
        StringBuilder ConstructorsMasterDetailsStr = new StringBuilder();

        foreach (DataRow dtrow in dt.Rows)
        {
            string ImageName = dtrow["Image Name"].ToString();
            if (ImageName == "" || ImageName == null) { ImageName = "images.png"; }

            if (dtrow["DETAILS"].ToString() == "Drivers")
            {
                DriversMasterDetailsStr.Append("<a target='_blank' href='" + dtrow["Driver_Constructor Url"].ToString() + "' style='flex: 50%;' class='recent-message d-flex px-4 py-3'>");
                DriversMasterDetailsStr.Append("<div class='avatar avatar-xl'>");
                DriversMasterDetailsStr.Append("<img ondragstart='dragFunction(event, " + '"' + "" + dtrow["DETAILS"].ToString() + "" + '"' + ")' draggable='true' data-info='" + dtrow["Driver_Constructor Name"].ToString() + "' src='../assets/images/Formula1/DriversImages/" + ImageName + "'></div>");
                DriversMasterDetailsStr.Append("<div class='name ms-4'>");
                DriversMasterDetailsStr.Append("<h5 class='mb-1'>" + dtrow["Driver_Constructor Name"].ToString() + "</h5>");
                DriversMasterDetailsStr.Append("<h6 class='text-muted mb-0'>" + dtrow["Driver_Constructor ID"].ToString() + "</h6></div></a>");
            }
            else
            {
                ConstructorsMasterDetailsStr.Append("<a target='_blank' href='" + dtrow["Driver_Constructor Url"].ToString() + "' style='flex: 50%;' class='recent-message d-flex px-4 py-3'>");
                ConstructorsMasterDetailsStr.Append("<div class='avatar avatar-xl'>");
                ConstructorsMasterDetailsStr.Append("<img ondragstart='dragFunction(event, " + '"' + "" + dtrow["DETAILS"].ToString() + "" + '"' + ")' draggable='true' data-info='" + dtrow["Driver_Constructor Name"].ToString() + "' src='../assets/images/Formula1/ConstructorsImage/" + ImageName + "'></div>");
                ConstructorsMasterDetailsStr.Append("<div class='name ms-4'>");
                ConstructorsMasterDetailsStr.Append("<h5 class='mb-1'>" + dtrow["Driver_Constructor Name"].ToString() + "</h5>");
                ConstructorsMasterDetailsStr.Append("<h6 class='text-muted mb-0'>" + dtrow["Driver_Constructor ID"].ToString() + "</h6></div></a>");
            }
        }

        DriversMasterDetails.Controls.Add(new Literal { Text = DriversMasterDetailsStr.ToString() });
        ConstructorsMasterDetails.Controls.Add(new Literal { Text = ConstructorsMasterDetailsStr.ToString() });
    }

    [WebMethod]
    public static string GettingRaceName(string Year)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry = "SELECT ROUND, RACE_NAME FROM F1_MASTER_SCHEDULE WHERE YEAR = '" + Year + "' AND STATUS = 'Active'";

        DataTable dt = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        string Result = "";

        foreach (DataRow dtrow in dt.Rows)
        {
            Result += "<option value='" + dtrow["ROUND"].ToString() + "'>" + dtrow["RACE_NAME"].ToString() + "</option>";
        }

        return Result;
    }
    [WebMethod]
    public static string GettingSprintEntry(string Year, string Round)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry = "SELECT " +
                     "CASE WHEN (SPRINT_DATE != '' AND SPRINT_TIME != '') OR (SPRINT_DATE IS NOT NULL AND SPRINT_TIME IS NOT NULL) THEN 'Sprint Entry' ELSE 'No' END AS [SPRINT] " +
                     "FROM F1_MASTER_SCHEDULE " +
                     "WHERE ROUND = '" + Round + "' " +
                     "AND YEAR = '" + Year + "' AND STATUS = 'Active';";

        DataTable dt = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        string Result = "";

        foreach (DataRow dtrow in dt.Rows)
        {
            Result = dtrow["SPRINT"].ToString();
        }

        return Result;
    }

    [WebMethod(EnableSession = true)]
    public static string RaceResultInsertData(List<DataList> DataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT COUNT(*) AS [MAXCOUNT] FROM F1_MASTER_RACE_RESULT WHERE YEAR = '" + DataList[0].DataYear.Trim() + "' AND ROUND = '" + DataList[0].DataRound.Trim() + "' AND STATUS = 'Active'", cnn);
            cnn.Open();
            SqlDataReader sqlreader = null;
            sqlreader = cmd.ExecuteReader();
            sqlreader.Read();
            int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
            cnn.Close();

            if (DupCount > 0)
            {
                foreach (var DriverDataFields in DataList)
                {
                    cmd = new SqlCommand("UPDATE F1_MASTER_RACE_RESULT SET DRIVER_1 = @Driver1, DRIVER_2 = @Driver2, DRIVER_3 = @Driver3, DRIVER_4 = @Driver4, DRIVER_5 = @Driver5, DRIVER_6 = @Driver6," +
                                         "SPRINT_DRIVER_1 = @sprint_driver_1, SPRINT_DRIVER_2 = @sprint_driver_2, SPRINT_DRIVER_3 = @sprint_driver_3, " +
                                         "CONSTRUCTOR_1 = @Constructor_1, Constructor_2 = @Constructor_2, Constructor_3 = @Constructor_3," +
                                         "FASTEST_LAP = @Fastest_lap, POLE_POSITION = @Pole_Position, MOST_PLACE_GAINED = @Most_Place_Gained," +
                                         "DRIVER_1_IMG = @Driver1img, DRIVER_2_IMG = @Driver2img, DRIVER_3_IMG = @Driver3img, DRIVER_4_IMG = @Driver4img, DRIVER_5_IMG = @Driver5img, DRIVER_6_IMG = @Driver6img," +
                                         "SPRINT_DRIVER_1_IMG = @sprint_driver_1_img, SPRINT_DRIVER_2_IMG = @sprint_driver_2_img, SPRINT_DRIVER_3_IMG = @sprint_driver_3_img, " +
                                         "CONSTRUCTOR_1_IMG = @Constructor_1img, Constructor_2_IMG = @Constructor_2img, Constructor_3_IMG = @Constructor_3img," +
                                         "FASTEST_LAP_IMG = @Fastest_lapimg, POLE_POSITION_IMG = @Pole_Positionimg, MOST_PLACE_GAINED_IMG = @Most_Place_Gainedimg " +
                                         "WHERE YEAR = @Year AND ROUND = @Round AND " +
                                         "STATUS = 'Active'", cnn);

                    cmd.Parameters.AddWithValue("@Driver1", DriverDataFields.DataDriver1.Trim());
                    cmd.Parameters.AddWithValue("@Driver2", DriverDataFields.DataDriver2.Trim());
                    cmd.Parameters.AddWithValue("@Driver3", DriverDataFields.DataDriver3.Trim());
                    cmd.Parameters.AddWithValue("@Driver4", DriverDataFields.DataDriver4.Trim());
                    cmd.Parameters.AddWithValue("@Driver5", DriverDataFields.DataDriver5.Trim());
                    cmd.Parameters.AddWithValue("@Driver6", DriverDataFields.DataDriver6.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_1", DriverDataFields.DataSprintDriver1.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_2", DriverDataFields.DataSprintDriver2.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_3", DriverDataFields.DataSprintDriver3.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_1", DriverDataFields.DataConstructor1.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_2", DriverDataFields.DataConstructor2.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_3", DriverDataFields.DataConstructor3.Trim());
                    cmd.Parameters.AddWithValue("@Fastest_lap", DriverDataFields.DataFastestLap.Trim());
                    cmd.Parameters.AddWithValue("@Pole_Position", DriverDataFields.DataPolePosition.Trim());
                    cmd.Parameters.AddWithValue("@Most_Place_Gained", DriverDataFields.DataMostPlacedGained.Trim());

                    cmd.Parameters.AddWithValue("@Driver1img", DriverDataFields.DataDriver1Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver2img", DriverDataFields.DataDriver2Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver3img", DriverDataFields.DataDriver3Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver4img", DriverDataFields.DataDriver4Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver5img", DriverDataFields.DataDriver5Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver6img", DriverDataFields.DataDriver6Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_1_img", DriverDataFields.DataSprintDriver1Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_2_img", DriverDataFields.DataSprintDriver2Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_3_img", DriverDataFields.DataSprintDriver3Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_1img", DriverDataFields.DataConstructor1Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_2img", DriverDataFields.DataConstructor2Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_3img", DriverDataFields.DataConstructor3Img.Trim());
                    cmd.Parameters.AddWithValue("@Fastest_lapimg", DriverDataFields.DataFastestLapImg.Trim());
                    cmd.Parameters.AddWithValue("@Pole_Positionimg", DriverDataFields.DataPolePositionImg.Trim());
                    cmd.Parameters.AddWithValue("@Most_Place_Gainedimg", DriverDataFields.DataMostPlacedGainedImg.Trim());

                    cmd.Parameters.AddWithValue("@Year", DriverDataFields.DataYear.Trim());
                    cmd.Parameters.AddWithValue("@Round", DriverDataFields.DataRound.Trim());
                    cmd.Parameters.AddWithValue("@ub", DriverDataFields.DataUserName.Trim());
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
            else
            {
                foreach (var DriverDataFields in DataList)
                {
                    cmd = new SqlCommand("INSERT INTO F1_MASTER_RACE_RESULT (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6, SPRINT_DRIVER_1, SPRINT_DRIVER_2, SPRINT_DRIVER_3, CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3, FASTEST_LAP, POLE_POSITION, MOST_PLACE_GAINED," +
                                         "DRIVER_1_IMG, DRIVER_2_IMG, DRIVER_3_IMG, DRIVER_4_IMG, DRIVER_5_IMG, DRIVER_6_IMG, SPRINT_DRIVER_1_IMG, SPRINT_DRIVER_2_IMG, SPRINT_DRIVER_3_IMG, CONSTRUCTOR_1_IMG, CONSTRUCTOR_2_IMG, CONSTRUCTOR_3_IMG, FASTEST_LAP_IMG, POLE_POSITION_IMG, MOST_PLACE_GAINED_IMG," +
                                         " YEAR, ROUND, UPDATED_BY)" +
                                         "VALUES (@Driver1, @Driver2, @Driver3, @Driver4, @Driver5, @Driver6, @sprint_driver_1, @sprint_driver_2, @sprint_driver_3, @Constructor_1, @Constructor_2, @Constructor_3, @Fastest_lap, @Pole_Position, @Most_Place_Gained," +
                                         "@Driver1img, @Driver2img, @Driver3img, @Driver4img, @Driver5img, @Driver6img, @sprint_driver_1_img, @sprint_driver_2_img, @sprint_driver_3_img, @Constructor_1img, @Constructor_2img, @Constructor_3img, @Fastest_lapimg, @Pole_Positionimg, @Most_Place_Gainedimg," +
                                         " @Year, @Round, @ub)", cnn);

                    cmd.Parameters.AddWithValue("@Driver1", DriverDataFields.DataDriver1.Trim());
                    cmd.Parameters.AddWithValue("@Driver2", DriverDataFields.DataDriver2.Trim());
                    cmd.Parameters.AddWithValue("@Driver3", DriverDataFields.DataDriver3.Trim());
                    cmd.Parameters.AddWithValue("@Driver4", DriverDataFields.DataDriver4.Trim());
                    cmd.Parameters.AddWithValue("@Driver5", DriverDataFields.DataDriver5.Trim());
                    cmd.Parameters.AddWithValue("@Driver6", DriverDataFields.DataDriver6.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_1", DriverDataFields.DataSprintDriver1.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_2", DriverDataFields.DataSprintDriver2.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_3", DriverDataFields.DataSprintDriver3.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_1", DriverDataFields.DataConstructor1.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_2", DriverDataFields.DataConstructor2.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_3", DriverDataFields.DataConstructor3.Trim());
                    cmd.Parameters.AddWithValue("@Fastest_lap", DriverDataFields.DataFastestLap.Trim());
                    cmd.Parameters.AddWithValue("@Pole_Position", DriverDataFields.DataPolePosition.Trim());
                    cmd.Parameters.AddWithValue("@Most_Place_Gained", DriverDataFields.DataMostPlacedGained.Trim());

                    cmd.Parameters.AddWithValue("@Driver1img", DriverDataFields.DataDriver1Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver2img", DriverDataFields.DataDriver2Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver3img", DriverDataFields.DataDriver3Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver4img", DriverDataFields.DataDriver4Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver5img", DriverDataFields.DataDriver5Img.Trim());
                    cmd.Parameters.AddWithValue("@Driver6img", DriverDataFields.DataDriver6Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_1_img", DriverDataFields.DataSprintDriver1Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_2_img", DriverDataFields.DataSprintDriver2Img.Trim());
                    cmd.Parameters.AddWithValue("@sprint_driver_3_img", DriverDataFields.DataSprintDriver3Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_1img", DriverDataFields.DataConstructor1Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_2img", DriverDataFields.DataConstructor2Img.Trim());
                    cmd.Parameters.AddWithValue("@Constructor_3img", DriverDataFields.DataConstructor3Img.Trim());
                    cmd.Parameters.AddWithValue("@Fastest_lapimg", DriverDataFields.DataFastestLapImg.Trim());
                    cmd.Parameters.AddWithValue("@Pole_Positionimg", DriverDataFields.DataPolePositionImg.Trim());
                    cmd.Parameters.AddWithValue("@Most_Place_Gainedimg", DriverDataFields.DataMostPlacedGainedImg.Trim());

                    cmd.Parameters.AddWithValue("@Year", DriverDataFields.DataYear.Trim());
                    cmd.Parameters.AddWithValue("@Round", DriverDataFields.DataRound.Trim());
                    cmd.Parameters.AddWithValue("@ub", DriverDataFields.DataUserName.Trim());
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }
        catch (Exception ex)
        { return ex.Message; }

        return "Saved";
    }

    public class DataList
    {
        public string DataYear { get; set; }
        public string DataRound { get; set; }
        public string DataDriver1 { get; set; }
        public string DataDriver2 { get; set; }
        public string DataDriver3 { get; set; }
        public string DataSprintDriver1 { get; set; }
        public string DataSprintDriver2 { get; set; }
        public string DataSprintDriver3 { get; set; }
        public string DataSprintDriver1Img { get; set; }
        public string DataSprintDriver2Img { get; set; }
        public string DataSprintDriver3Img { get; set; }
        public string DataDriver4 { get; set; }
        public string DataDriver5 { get; set; }
        public string DataDriver6 { get; set; }
        public string DataConstructor1 { get; set; }
        public string DataConstructor2 { get; set; }
        public string DataConstructor3 { get; set; }
        public string DataFastestLap { get; set; }
        public string DataPolePosition { get; set; }
        public string DataMostPlacedGained { get; set; }
        public string DataDriver1Img { get; set; }
        public string DataDriver2Img { get; set; }
        public string DataDriver3Img { get; set; }
        public string DataDriver4Img { get; set; }
        public string DataDriver5Img { get; set; }
        public string DataDriver6Img { get; set; }
        public string DataConstructor1Img { get; set; }
        public string DataConstructor2Img { get; set; }
        public string DataConstructor3Img { get; set; }
        public string DataFastestLapImg { get; set; }
        public string DataPolePositionImg { get; set; }
        public string DataMostPlacedGainedImg { get; set; }
        public string DataUserName { get; set; }
        public string ErrorMessage { get; set; }
    }
    [WebMethod]
    public static DataList[] CheckingtheRaceResult(string Year, string Round)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry = "SELECT * FROM F1_MASTER_RACE_RESULT WHERE YEAR = '" + Year + "' AND ROUND = '" + Round + "' AND STATUS = 'Active'";

        DataTable dt = new DataTable();
        List<DataList> details = new List<DataList>();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        if (dt.Rows.Count == 0)
        {
            DataList columns = new DataList();
            columns.ErrorMessage = "No Data Found";
            details.Add(columns);
        }
        foreach (DataRow dtrow in dt.Rows)
        {
            DataList columns = new DataList();
            columns.DataDriver1 = dtrow["DRIVER_1"].ToString();
            columns.DataDriver2 = dtrow["DRIVER_2"].ToString();
            columns.DataDriver3 = dtrow["DRIVER_3"].ToString();
            columns.DataDriver4 = dtrow["DRIVER_4"].ToString();
            columns.DataDriver5 = dtrow["DRIVER_5"].ToString();
            columns.DataDriver6 = dtrow["DRIVER_6"].ToString();
            columns.DataSprintDriver1 = dtrow["SPRINT_DRIVER_1"].ToString();
            columns.DataSprintDriver2 = dtrow["SPRINT_DRIVER_2"].ToString();
            columns.DataSprintDriver3 = dtrow["SPRINT_DRIVER_3"].ToString();
            columns.DataConstructor1 = dtrow["CONSTRUCTOR_1"].ToString();
            columns.DataConstructor2 = dtrow["CONSTRUCTOR_2"].ToString();
            columns.DataConstructor3 = dtrow["CONSTRUCTOR_3"].ToString();
            columns.DataFastestLap = dtrow["FASTEST_LAP"].ToString();
            columns.DataPolePosition = dtrow["POLE_POSITION"].ToString();
            columns.DataMostPlacedGained = dtrow["MOST_PLACE_GAINED"].ToString();

            columns.DataDriver1Img = dtrow["DRIVER_1_IMG"].ToString();
            columns.DataDriver2Img = dtrow["DRIVER_2_IMG"].ToString();
            columns.DataDriver3Img = dtrow["DRIVER_3_IMG"].ToString();
            columns.DataDriver4Img = dtrow["DRIVER_4_IMG"].ToString();
            columns.DataDriver5Img = dtrow["DRIVER_5_IMG"].ToString();
            columns.DataDriver6Img = dtrow["DRIVER_6_IMG"].ToString();
            columns.DataSprintDriver1Img = dtrow["SPRINT_DRIVER_1_IMG"].ToString();
            columns.DataSprintDriver2Img = dtrow["SPRINT_DRIVER_2_IMG"].ToString();
            columns.DataSprintDriver3Img = dtrow["SPRINT_DRIVER_3_IMG"].ToString();
            columns.DataConstructor1Img = dtrow["CONSTRUCTOR_1_IMG"].ToString();
            columns.DataConstructor2Img = dtrow["CONSTRUCTOR_2_IMG"].ToString();
            columns.DataConstructor3Img = dtrow["CONSTRUCTOR_3_IMG"].ToString();
            columns.DataFastestLapImg = dtrow["FASTEST_LAP_IMG"].ToString();
            columns.DataPolePositionImg = dtrow["POLE_POSITION_IMG"].ToString();
            columns.DataMostPlacedGainedImg = dtrow["MOST_PLACE_GAINED_IMG"].ToString();

            columns.ErrorMessage = "";
            details.Add(columns);
        }

        return details.ToArray();
    }

    [WebMethod]
    public static string GettingRaceResultValuesFromAPI(string UserID, string Year, string Race, string Sprint)
    {
        if (Year != "" && UserID != "")
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, url = "";
            SqlConnection cnn = new SqlConnection(constr);

            using (HttpClient client = new HttpClient())
            {
                url = "http://ergast.com/api/f1/" + Year + "/" + Race + "/results";

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

                            XElement mrDataElement = xmlDoc.Element(ns + "MRData");
                            string totalValue = mrDataElement.Attribute("total").Value;

                            if (totalValue != "0")
                            {
                                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS [MAXCOUNT] FROM F1_MASTER_RACE_RESULT WHERE YEAR = '" + Year + "' AND ROUND = '" + Race + "' AND STATUS = 'Active'", cnn);
                                cnn.Open();
                                SqlDataReader sqlreader = null;
                                sqlreader = cmd.ExecuteReader();
                                sqlreader.Read();
                                int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
                                cnn.Close();

                                if (DupCount > 0)
                                {
                                    cmd = new SqlCommand("UPDATE F1_MASTER_RACE_RESULT SET STATUS = 'Deleted' WHERE ROUND = '" + Race + "' AND YEAR = '" + Year + "'", cnn);
                                    cnn.Open();
                                    cmd.ExecuteNonQuery();
                                    cnn.Close();
                                }

                                DataTable Driverdt = new DataTable();
                                cmd = new SqlCommand("SELECT DRIVER_FIRST_NAME + ' ' + DRIVER_LAST_NAME AS [DRIVER_NAME], DRIVER_NUMBER, " +
                                                     "CASE WHEN FILE_NAME = '' OR FILE_NAME IS NULL THEN '../assets/images/Formula1/DriversImages/images.png' " +
                                                     "ELSE '../assets/images/Formula1/DriversImages/' + FILE_NAME " +
                                                     "END AS [FILE_NAME] " +
                                                     "FROM F1_MASTER_DRIVER WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(Driverdt);

                                DataTable Teamdt = new DataTable();
                                cmd = new SqlCommand("SELECT CONSTRUCTOR_ID, CONSTRUCTOR_NAME, " +
                                                     "CASE WHEN FILE_NAME = '' OR FILE_NAME IS NULL THEN '../assets/images/Formula1/ConstructorsImage/images.png' " +
                                                     "ELSE '../assets/images/Formula1/ConstructorsImage/' + FILE_NAME " +
                                                     "END AS [FILE_NAME] " +
                                                     "FROM F1_MASTER_CONSTRUCTORS WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                                da = new SqlDataAdapter(cmd);
                                da.Fill(Teamdt);

                                string Driver1 = "", Driver2 = "", Driver3 = "", Driver4 = "", Driver5 = "", Driver6 = "",
                                       SprintDriver1 = "", SprintDriver2 = "", SprintDriver3 = "",
                                       Constructor1 = "", Constructor2 = "", Constructor3 = "",
                                       FastestLap = "", PolePosition = "", MostPlacesGained = "";

                                string Driver1Img = "", Driver2Img = "", Driver3Img = "", Driver4Img = "", Driver5Img = "", Driver6Img = "",
                                       SprintDriver1Img = "", SprintDriver2Img = "", SprintDriver3Img = "",
                                       Constructor1Img = "", Constructor2Img = "", Constructor3Img = "",
                                       FastestLapImg = "", PolePositionImg = "", MostPlacesGainedImg = "";

                                int FindPosition = 0, FindPolePosition = 0, FindFastestLap = 0;

                                DataTable Pointsdt = new DataTable();
                                Pointsdt.Columns.Add("ConstructorID", typeof(string));
                                Pointsdt.Columns.Add("DriverPoints", typeof(int));

                                DataTable MPGdt = new DataTable();
                                MPGdt.Columns.Add("DriverNumber", typeof(string));
                                MPGdt.Columns.Add("MostPlacesGained", typeof(int));

                                var ResultTag = xmlDoc.Descendants(ns + "Result");

                                foreach (var ResultPos in ResultTag)
                                {
                                    var DriverTag = ResultPos.Elements(ns + "Driver");
                                    var ConstructorTag = ResultPos.Elements(ns + "Constructor");

                                    foreach (var constructor in ConstructorTag)
                                    {
                                        Pointsdt.Rows.Add(constructor.Attribute("constructorId").Value, Convert.ToInt32(ResultPos.Attribute("points").Value));
                                    }

                                    foreach (var driver in DriverTag)
                                    {
                                        FindPosition = Convert.ToInt32(ResultPos.Attribute("position").Value);
                                        FindPolePosition = Convert.ToInt32(ResultPos.Element(ns + "Grid").Value);
                                        try { FindFastestLap = Convert.ToInt32(ResultPos.Element(ns + "FastestLap").Attribute("rank").Value); }
                                        catch (Exception) { FindFastestLap = 0; }

                                        int SubtractedValue = FindPolePosition - FindPosition;
                                        MPGdt.Rows.Add(driver.Element(ns + "PermanentNumber").Value.Trim(), SubtractedValue);

                                        if (FindFastestLap == 1)
                                        {
                                            foreach (DataRow dtrow in Driverdt.Rows)
                                            {
                                                if (driver.Element(ns + "PermanentNumber").Value.Trim() == dtrow["DRIVER_NUMBER"].ToString().Trim())
                                                {
                                                    FastestLap = dtrow["DRIVER_NAME"].ToString().Trim();
                                                    FastestLapImg = dtrow["FILE_NAME"].ToString().Trim();
                                                }
                                            }
                                        }

                                        if (FindPolePosition == 1)
                                        {
                                            foreach (DataRow dtrow in Driverdt.Rows)
                                            {
                                                if (driver.Element(ns + "PermanentNumber").Value.Trim() == dtrow["DRIVER_NUMBER"].ToString().Trim())
                                                {
                                                    PolePosition = dtrow["DRIVER_NAME"].ToString().Trim();
                                                    PolePositionImg = dtrow["FILE_NAME"].ToString().Trim();
                                                }
                                            }
                                        }

                                        if (FindPosition <= 6)
                                        {
                                            foreach (DataRow dtrow in Driverdt.Rows)
                                            {
                                                if (driver.Element(ns + "PermanentNumber").Value.Trim() == dtrow["DRIVER_NUMBER"].ToString().Trim())
                                                {
                                                    if (FindPosition == 1) { Driver1 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver1Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                    if (FindPosition == 2) { Driver2 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver2Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                    if (FindPosition == 3) { Driver3 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver3Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                    if (FindPosition == 4) { Driver4 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver4Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                    if (FindPosition == 5) { Driver5 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver5Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                    if (FindPosition == 6) { Driver6 = dtrow["DRIVER_NAME"].ToString().Trim(); Driver6Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                }
                                            }
                                        }
                                    }
                                }

                                // Sort the DataTable based on the "MostPlacesGained" column in descending order.
                                var MPGsortedData = MPGdt.AsEnumerable()
                                                          .OrderByDescending(row => row.Field<int>("MostPlacesGained"));

                                var topRow = MPGsortedData.FirstOrDefault();

                                foreach (DataRow dtrow in Driverdt.Rows)
                                {
                                    if (topRow["DriverNumber"].ToString().Trim() == dtrow["DRIVER_NUMBER"].ToString().Trim())
                                    {
                                        MostPlacesGained = dtrow["DRIVER_NAME"].ToString().Trim();
                                        MostPlacesGainedImg = dtrow["FILE_NAME"].ToString().Trim();
                                    }
                                }

                                // Group by the "Team" column and sum the "Points" column.
                                var groupedData = from row in Pointsdt.AsEnumerable()
                                                  group row by row.Field<string>("ConstructorID") into teamGroup
                                                  select new
                                                  {
                                                      Team = teamGroup.Key,
                                                      TotalPoints = teamGroup.Sum(r => r.Field<int>("DriverPoints"))
                                                  };

                                // Sort the result in descending order based on the total points.
                                var ConstructorsortedData = groupedData.OrderByDescending(g => g.TotalPoints); int c = 1;

                                var topThreeRows = ConstructorsortedData.Take(3);

                                foreach (var item in topThreeRows)
                                {
                                    if (c == 1)
                                    {
                                        foreach (DataRow dtrow in Teamdt.Rows)
                                        {
                                            if (dtrow["CONSTRUCTOR_ID"].ToString().Trim() == item.Team)
                                            {
                                                Constructor1 = dtrow["CONSTRUCTOR_NAME"].ToString().Trim();
                                                Constructor1Img = dtrow["FILE_NAME"].ToString().Trim();
                                            }
                                        }
                                    }
                                    if (c == 2)
                                    {
                                        foreach (DataRow dtrow in Teamdt.Rows)
                                        {
                                            if (dtrow["CONSTRUCTOR_ID"].ToString().Trim() == item.Team)
                                            {
                                                Constructor2 = dtrow["CONSTRUCTOR_NAME"].ToString().Trim();
                                                Constructor2Img = dtrow["FILE_NAME"].ToString().Trim();
                                            }
                                        }
                                    }
                                    if (c == 3)
                                    {
                                        foreach (DataRow dtrow in Teamdt.Rows)
                                        {
                                            if (dtrow["CONSTRUCTOR_ID"].ToString().Trim() == item.Team)
                                            {
                                                Constructor3 = dtrow["CONSTRUCTOR_NAME"].ToString().Trim();
                                                Constructor3Img = dtrow["FILE_NAME"].ToString().Trim();
                                            }
                                        }
                                    }
                                    c++;
                                }

                                if (Sprint == "Sprint")
                                {
                                    using (HttpClient Sprintclient = new HttpClient())
                                    {
                                        url = "http://ergast.com/api/f1/" + Year + "/" + Race + "/sprint";

                                        Sprintclient.BaseAddress = new Uri(url);

                                        try
                                        {
                                            HttpResponseMessage response1 = Sprintclient.GetAsync("").Result;
                                            if (response1.IsSuccessStatusCode)
                                            {
                                                string xmlResponse1 = response1.Content.ReadAsStringAsync().Result;

                                                // Check if the content type is XML
                                                if (response1.Content.Headers.ContentType.MediaType == "application/xml" ||
                                                    response1.Content.Headers.ContentType.MediaType == "text/xml")
                                                {
                                                    XDocument xmlDoc1 = XDocument.Parse(xmlResponse1);
                                                    XNamespace ns1 = "http://ergast.com/mrd/1.5";

                                                    XElement mrDataElement1 = xmlDoc1.Element(ns1 + "MRData");
                                                    string totalValue1 = mrDataElement1.Attribute("total").Value;

                                                    if (totalValue1 != "0")
                                                    {
                                                        var ResultTag1 = xmlDoc1.Descendants(ns1 + "SprintResult");

                                                        foreach (var ResultPos in ResultTag1)
                                                        {
                                                            var DriverTag = ResultPos.Elements(ns1 + "Driver");

                                                            foreach (var driver in DriverTag)
                                                            {
                                                                FindPosition = Convert.ToInt32(ResultPos.Attribute("position").Value);

                                                                if (FindPosition <= 3)
                                                                {
                                                                    foreach (DataRow dtrow in Driverdt.Rows)
                                                                    {
                                                                        if (driver.Element(ns1 + "PermanentNumber").Value.Trim() == dtrow["DRIVER_NUMBER"].ToString().Trim())
                                                                        {
                                                                            if (FindPosition == 1) { SprintDriver1 = dtrow["DRIVER_NAME"].ToString().Trim(); SprintDriver1Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                                            if (FindPosition == 2) { SprintDriver2 = dtrow["DRIVER_NAME"].ToString().Trim(); SprintDriver2Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                                            if (FindPosition == 3) { SprintDriver3 = dtrow["DRIVER_NAME"].ToString().Trim(); SprintDriver3Img = dtrow["FILE_NAME"].ToString().Trim(); }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return "No Data Found for Sprint Entry";
                                                    }
                                                }
                                                else
                                                {
                                                    return "Response is not XML for Sprint Entry";
                                                }
                                            }
                                            else
                                            {
                                                return "Error in API Contact for Sprint Entry - " + response.StatusCode;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            return "Error in Code for Sprint Entry - " + ex.Message;
                                        }
                                    }
                                }                                

                                cmd = new SqlCommand("INSERT INTO F1_MASTER_RACE_RESULT (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6, SPRINT_DRIVER_1, SPRINT_DRIVER_2, SPRINT_DRIVER_3, CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3, FASTEST_LAP, POLE_POSITION, MOST_PLACE_GAINED," +
                                 "DRIVER_1_IMG, DRIVER_2_IMG, DRIVER_3_IMG, DRIVER_4_IMG, DRIVER_5_IMG, DRIVER_6_IMG, SPRINT_DRIVER_1_IMG, SPRINT_DRIVER_2_IMG, SPRINT_DRIVER_3_IMG, CONSTRUCTOR_1_IMG, CONSTRUCTOR_2_IMG, CONSTRUCTOR_3_IMG, FASTEST_LAP_IMG, POLE_POSITION_IMG, MOST_PLACE_GAINED_IMG," +
                                 " YEAR, ROUND, UPDATED_BY)" +
                                 "VALUES (@Driver1, @Driver2, @Driver3, @Driver4, @Driver5, @Driver6, @SprintDriver1, @SprintDriver2, @SprintDriver3, @Constructor_1, @Constructor_2, @Constructor_3, @Fastest_lap, @Pole_Position, @Most_Place_Gained," +
                                 "@Driver1img, @Driver2img, @Driver3img, @Driver4img, @Driver5img, @Driver6img, @SprintDriver1Img, @SprintDriver2Img, @SprintDriver3Img, @Constructor_1img, @Constructor_2img, @Constructor_3img, @Fastest_lapimg, @Pole_Positionimg, @Most_Place_Gainedimg," +
                                 " @Year, @Round, @ub)", cnn);

                                cmd.Parameters.AddWithValue("@Driver1", Driver1.Trim());
                                cmd.Parameters.AddWithValue("@Driver2", Driver2.Trim());
                                cmd.Parameters.AddWithValue("@Driver3", Driver3.Trim());
                                cmd.Parameters.AddWithValue("@Driver4", Driver4.Trim());
                                cmd.Parameters.AddWithValue("@Driver5", Driver5.Trim());
                                cmd.Parameters.AddWithValue("@Driver6", Driver6.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver1", SprintDriver1.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver2", SprintDriver2.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver3", SprintDriver3.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_1", Constructor1.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_2", Constructor2.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_3", Constructor3.Trim());
                                cmd.Parameters.AddWithValue("@Fastest_lap", FastestLap.Trim());
                                cmd.Parameters.AddWithValue("@Pole_Position", PolePosition.Trim());
                                cmd.Parameters.AddWithValue("@Most_Place_Gained", MostPlacesGained.Trim());

                                cmd.Parameters.AddWithValue("@Driver1img", Driver1Img.Trim());
                                cmd.Parameters.AddWithValue("@Driver2img", Driver2Img.Trim());
                                cmd.Parameters.AddWithValue("@Driver3img", Driver3Img.Trim());
                                cmd.Parameters.AddWithValue("@Driver4img", Driver4Img.Trim());
                                cmd.Parameters.AddWithValue("@Driver5img", Driver5Img.Trim());
                                cmd.Parameters.AddWithValue("@Driver6img", Driver6Img.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver1Img", SprintDriver1Img.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver2Img", SprintDriver2Img.Trim());
                                cmd.Parameters.AddWithValue("@SprintDriver3Img", SprintDriver3Img.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_1img", Constructor1Img.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_2img", Constructor2Img.Trim());
                                cmd.Parameters.AddWithValue("@Constructor_3img", Constructor3Img.Trim());
                                cmd.Parameters.AddWithValue("@Fastest_lapimg", FastestLapImg.Trim());
                                cmd.Parameters.AddWithValue("@Pole_Positionimg", PolePositionImg.Trim());
                                cmd.Parameters.AddWithValue("@Most_Place_Gainedimg", MostPlacesGainedImg.Trim());

                                cmd.Parameters.AddWithValue("@Year", Year.Trim());
                                cmd.Parameters.AddWithValue("@Round", Race.Trim());
                                cmd.Parameters.AddWithValue("@ub", UserID.Trim());
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();

                            }
                            else
                            {
                                return "No Data Found";
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
}