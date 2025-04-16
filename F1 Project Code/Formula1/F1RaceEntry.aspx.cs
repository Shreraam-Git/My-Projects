using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Formula1_F1RaceEntry : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
    SqlConnection cnn;
    SqlCommand cmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        qry = "SELECT TOP 1 RACE_NAME, YEAR, ROUND, DEAD_LINE_TIME, " +
              "CASE WHEN (SPRINT_DATE != '' AND SPRINT_TIME != '') OR (SPRINT_DATE IS NOT NULL AND SPRINT_TIME IS NOT NULL) THEN 'Sprint Entry' ELSE 'No' END AS [SPRINT] " +
              "FROM F1_MASTER_SCHEDULE " +
              "WHERE FORMAT(DATEADD(day, 2, SECOND_PRACTICE_DATE), 'yyyy-MM-dd') >= CAST(GETDATE() AS DATE) " +
              "AND YEAR = YEAR(GETDATE()) AND STATUS = 'Active';";

        DataTable dt1 = new DataTable();
        cnn = new SqlConnection(constr);
        cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
        sqlda.Fill(dt1);

        foreach (DataRow dtrow in dt1.Rows)
        {
            Dummyvalue.Text = dtrow["SPRINT"].ToString();

            if (dtrow["SPRINT"].ToString() == "Sprint Entry")
            {
                SprintEntryDiv.Style["display"] = "block";
            }

            string RaceDateTimeString = "";

            if (dtrow["DEAD_LINE_TIME"].ToString() != null && dtrow["DEAD_LINE_TIME"].ToString() != "")
            {
                RaceDateTimeString = Convert.ToDateTime(dtrow["DEAD_LINE_TIME"]).ToString("MMM dd, yyyy HH:mm");
            }
            else { RaceDateTimeString = DateTime.Now.ToString("MMM dd, yyyy 00:00:00"); }

            ButtonsPlaceholder.Controls.Add(new Literal
            {
                Text = "<div class='buttons' style='margin-top: 10px;'>" +
                               "<button class='btn btn-danger' style='margin-top: -10px;' type='button' title='Remove all Data' onclick='RemoveAllData()'><i class='bi bi-trash3-fill'></i></button>" +
                               "<button class='btn btn-primary' style='margin-top: -10px;' type='button' title='Edit Race Entry' onclick='EnablingDiv();'><i class='bi bi-pencil-fill'></i></button>" +
                               "<button class='btn btn-success' style='margin-top: -10px;' title='Save Race Entry' onclick='RaceEntrySave()' type='button'><i class='bi bi-floppy2-fill'></i></button>" +
                               "</div>"
            });

            DateTime RaceDateTime;
            if (DateTime.TryParse(RaceDateTimeString, out RaceDateTime))
            {
                if (DateTime.Now < RaceDateTime)
                {
                    ButtonsPlaceholder.Controls.Add(new Literal
                    {
                        Text = "<div class='buttons' style='margin-top: 10px;'>" +
                               "<button class='btn btn-danger' style='margin-top: -10px;' type='button' title='Remove all Data' onclick='RemoveAllData()'><i class='bi bi-trash3-fill'></i></button>" +
                               "<button class='btn btn-primary' style='margin-top: -10px;' type='button' title='Edit Race Entry' onclick='EnablingDiv();'><i class='bi bi-pencil-fill'></i></button>" +
                               "<button class='btn btn-success' style='margin-top: -10px;' title='Save Race Entry' onclick='RaceEntrySave()' type='button'><i class='bi bi-floppy2-fill'></i></button>" +
                               "</div>"
                    });
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "CommonErrormsg('error', 'Race Entry has been closed.');", true);
                }
            }
        }

        qry = "SELECT 'Drivers' AS [DETAILS], FILE_NAME AS [Image Name], DRIVER_NUMBER AS [Driver_Constructor ID], DRIVER_FIRST_NAME + ' ' + DRIVER_LAST_NAME AS [Driver_Constructor Name], " +
              "DRIVER_BIO_URL AS [Driver_Constructor Url] FROM F1_MASTER_DRIVER WHERE YEAR = YEAR(GETDATE()) AND STATUS = 'Active' " +
              "UNION " +
              "SELECT 'Constructors' AS [DETAILS], FILE_NAME, '@' + CONSTRUCTOR_ID, CONSTRUCTOR_NAME, CONSTRUCTOR_BOI_URL FROM F1_MASTER_CONSTRUCTORS " +
              "WHERE YEAR = YEAR(GETDATE()) AND STATUS = 'Active' " +
              "ORDER BY 1,2";

        DataTable dt = new DataTable();
        cmd = new SqlCommand(qry, cnn);
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

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "Savingthetimedata('RaceEntry');", true);
    }
    [WebMethod(EnableSession = true)]
    public static string RaceEntryInsertData(List<DataList> DataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("SELECT COUNT(*) AS [MAXCOUNT] FROM F1_RACE_ENTRY WHERE YEAR = '" + DataList[0].DataYear.Trim() + "' AND ROUND = '" + DataList[0].DataRound.Trim() + "' AND UPDATED_BY = '" + DataList[0].DataUserName.Trim() + "' AND STATUS = 'Active'", cnn);
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
                    cmd = new SqlCommand("UPDATE F1_RACE_ENTRY SET DRIVER_1 = @Driver1, DRIVER_2 = @Driver2, DRIVER_3 = @Driver3, DRIVER_4 = @Driver4, DRIVER_5 = @Driver5, DRIVER_6 = @Driver6, " +
                                         "SPRINT_DRIVER_1 = @sprint_driver_1, SPRINT_DRIVER_2 = @sprint_driver_2, SPRINT_DRIVER_3 = @sprint_driver_3, " +
                                         "CONSTRUCTOR_1 = @Constructor_1, Constructor_2 = @Constructor_2, Constructor_3 = @Constructor_3," +
                                         "FASTEST_LAP = @Fastest_lap, POLE_POSITION = @Pole_Position, MOST_PLACE_GAINED = @Most_Place_Gained," +
                                         "DRIVER_1_IMG = @Driver1img, DRIVER_2_IMG = @Driver2img, DRIVER_3_IMG = @Driver3img, DRIVER_4_IMG = @Driver4img, DRIVER_5_IMG = @Driver5img, DRIVER_6_IMG = @Driver6img, " +
                                         "SPRINT_DRIVER_1_IMG = @sprint_driver_1_img, SPRINT_DRIVER_2_IMG = @sprint_driver_2_img, SPRINT_DRIVER_3_IMG = @sprint_driver_3_img, " +
                                         "CONSTRUCTOR_1_IMG = @Constructor_1img, Constructor_2_IMG = @Constructor_2img, Constructor_3_IMG = @Constructor_3img," +
                                         "FASTEST_LAP_IMG = @Fastest_lapimg, POLE_POSITION_IMG = @Pole_Positionimg, MOST_PLACE_GAINED_IMG = @Most_Place_Gainedimg " +
                                         "WHERE YEAR = @Year AND ROUND = @Round AND " +
                                         "UPDATED_BY = @ub AND STATUS = 'Active'", cnn);

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
                    cmd = new SqlCommand("INSERT INTO F1_RACE_ENTRY (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6, SPRINT_DRIVER_1, SPRINT_DRIVER_2, SPRINT_DRIVER_3, CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3, FASTEST_LAP, POLE_POSITION, MOST_PLACE_GAINED," +
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
        public string DataSprintDriver1Img { get; set; }
        public string DataSprintDriver2Img { get; set; }
        public string DataSprintDriver3Img { get; set; }
        public string DataConstructor1Img { get; set; }
        public string DataConstructor2Img { get; set; }
        public string DataConstructor3Img { get; set; }
        public string DataFastestLapImg { get; set; }
        public string DataPolePositionImg { get; set; }
        public string DataMostPlacedGainedImg { get; set; }
        public string DataUserName { get; set; }
    }
    [WebMethod]
    public static DataList[] CheckingtheRaceEntry(string Year, string Round, string SessionUserName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry = "SELECT * FROM F1_RACE_ENTRY WHERE YEAR = '" + Year + "' AND ROUND = '" + Round + "' AND STATUS = 'Active' AND UPDATED_BY = '" + SessionUserName + "'";

        DataTable dt = new DataTable();
        List<DataList> details = new List<DataList>();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
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
            details.Add(columns);
        }

        return details.ToArray();
    }
    [WebMethod(EnableSession = true)]
    public static string TestingNewFunction(string Year, string Round)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "", ReturnValue = "";
        SqlConnection cnn;
        SqlCommand cmd;

        qry = "SELECT " +
            "CASE WHEN (SPRINT_DATE != '' AND SPRINT_TIME != '') OR (SPRINT_DATE IS NOT NULL AND SPRINT_TIME IS NOT NULL) THEN 'Sprint Entry' ELSE 'No' END AS [SPRINT] " +
            "FROM F1_MASTER_SCHEDULE " +
            "WHERE YEAR = YEAR(GETDATE()) And ROUND = '" + Round + "' AND STATUS = 'Active';";

        DataTable dt1 = new DataTable();
        cnn = new SqlConnection(constr);
        cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
        sqlda.Fill(dt1);

        foreach (DataRow dtrow in dt1.Rows)
        {
            ReturnValue = dtrow["SPRINT"].ToString();
        }

        return ReturnValue;
    }
}