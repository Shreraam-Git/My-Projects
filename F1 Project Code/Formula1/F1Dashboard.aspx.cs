﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;

public partial class Formula1_F1Dashboard : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
    string qry;
    SqlCommand cmd;
    SqlConnection cnn;
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "CalculatingtheresultPoints();", true);
    }
    [WebMethod]
    public static string CalculatingtheresultPoints(string UserName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, Year = "", Race = "";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        bool Exists = false;
        object response = new { };

        cmd = new SqlCommand("SELECT MAX(CAST(ROUND AS INT)) AS [ROUND], YEAR FROM F1_MASTER_RACE_RESULT WHERE YEAR = YEAR(GETDATE()) AND STATUS = 'Active' GROUP BY [YEAR];", cnn);
        cmd.Connection = cnn;
        cnn.Open();
        SqlDataReader sqlrd = cmd.ExecuteReader();
        while (sqlrd.Read())
        {
            Year = sqlrd["YEAR"].ToString();
            Race = sqlrd["ROUND"].ToString();
            Exists = true;
        }
        cnn.Close();

        if (Exists)
        {
            try
            {
                string qry = "SELECT DISTINCT TOP 1 USER_ID FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + UserName + "' AND STATUS = 'Active';";

                DataTable dt = new DataTable();
                cnn = new SqlConnection(constr);
                cmd = new SqlCommand(qry, cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string ActualUserName = "";

                foreach (DataRow dtrow in dt.Rows)
                {
                    ActualUserName = dtrow["USER_ID"].ToString();
                }

                qry = "SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = '" + Year + "' AND ROUND = '" + Race + "' AND STATUS = 'Active' AND UPDATED_BY = '" + ActualUserName + "' ";
                cmd = new SqlCommand(qry);
                cmd.Connection = cnn;
                cnn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar().GetHashCode());
                cnn.Close();

                if (count > 0)
                {
                    qry = "EXEC FORMULA_ONE_POINTS_CONDITION @Year = '" + Year + "', @Round = '" + Race + "', @UserId = '" + ActualUserName + "';";
                    dt = new DataTable();
                    cmd = new SqlCommand(qry, cnn);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    int sno = 1;
                    string DriverTabStructure = "", DriverRandomOrderPoints = "";
                    string ConstructorTabStructure = "", ConstructorRandomOrderPoints = "";
                    string PolePositionTabStructure = "", ConstraintsTabStructure = "";
                    string SprintEntryTabStructure = "";

                    foreach (DataRow dtrow in dt.Rows)
                    {
                        if (sno == 1) { DriverRandomOrderPoints = dtrow["DRIVER_RANDOM_ORDER"].ToString(); }
                        if (sno == 1) { ConstructorRandomOrderPoints = dtrow["CONSTRUCTOR_RANDOM_ORDER"].ToString(); }

                        string Itag = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_D"].ToString() == "0") { Itag = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPath = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_D"].ToString() == "0") { IPath = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        string ItagSD = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_SD"].ToString() == "0") { ItagSD = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPathSD = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_D"].ToString() == "0") { IPathSD = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        string ItagC = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_C"].ToString() == "0") { ItagC = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPathC = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_C"].ToString() == "0") { IPathC = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        string ItagPP = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_PP"].ToString() == "0") { ItagPP = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPathPP = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_PP"].ToString() == "0") { IPathPP = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        string ItagFL = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_FL"].ToString() == "0") { ItagFL = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPathFL = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_FL"].ToString() == "0") { IPathFL = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        string ItagMPG = "fill='green' class='bi bi-check-circle-fill'"; if (dtrow["POINTS_MPG"].ToString() == "0") { ItagMPG = "fill='red' class='bi bi-x-circle-fill'"; }
                        string IPathMPG = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z' />";
                        if (dtrow["POINTS_MPG"].ToString() == "0") { IPathMPG = "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z' />"; }

                        DriverTabStructure += "<tr>";
                        DriverTabStructure += "<td>" + sno + "</td>";
                        DriverTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                        DriverTabStructure += "<img id='YEDriveroneImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_D"].ToString() + "' alt='Image'>";
                        DriverTabStructure += "<p id='YEDriveroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_D"].ToString() + "</p></div></td>";
                        DriverTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                        DriverTabStructure += "<img id='REDriveroneImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_D"].ToString() + "' alt='Image'>";
                        DriverTabStructure += "<p id='REDriveroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_D"].ToString() + "</p></div></td>";
                        DriverTabStructure += "<td>";
                        DriverTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + Itag + " viewBox='0 0 16 16'>";
                        DriverTabStructure += IPath;
                        DriverTabStructure += "</svg></td>";
                        DriverTabStructure += "<td>" + dtrow["POINTS_D"].ToString() + "</td>";
                        DriverTabStructure += "<td>" + dtrow["AGAINST_THE_GRAIN_D"].ToString() + "</td>";
                        DriverTabStructure += "</tr>";

                        if (dtrow["YOUR_ENTRY_SD"].ToString() != "" || dtrow["RACE_RESULT_SD"].ToString() != "")
                        {
                            SprintEntryTabStructure += "<tr>";
                            SprintEntryTabStructure += "<td>" + sno + "</td>";
                            SprintEntryTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            SprintEntryTabStructure += "<img id='SDYEDriveroneImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_SD"].ToString() + "' alt='Image'>";
                            SprintEntryTabStructure += "<p id='SDYEDriveroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_SD"].ToString() + "</p></div></td>";
                            SprintEntryTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            SprintEntryTabStructure += "<img id='SDREDriveroneImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_SD"].ToString() + "' alt='Image'>";
                            SprintEntryTabStructure += "<p id='SDREDriveroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_SD"].ToString() + "</p></div></td>";
                            SprintEntryTabStructure += "<td>";
                            SprintEntryTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + ItagSD + " viewBox='0 0 16 16'>";
                            SprintEntryTabStructure += IPathSD;
                            SprintEntryTabStructure += "</svg></td>";
                            SprintEntryTabStructure += "<td>" + dtrow["POINTS_SD"].ToString() + "</td>";
                            SprintEntryTabStructure += "</tr>";
                        }

                        if (dtrow["YOUR_ENTRY_C"].ToString() != "" || dtrow["RACE_RESULT_C"].ToString() != "")
                        {
                            ConstructorTabStructure += "<tr>";
                            ConstructorTabStructure += "<td>" + sno + "</td>";
                            ConstructorTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstructorTabStructure += "<img id='YEConstructoroneImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_C"].ToString() + "' alt='Image'>";
                            ConstructorTabStructure += "<p id='YEConstructoroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_C"].ToString() + "</p></div></td>";
                            ConstructorTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstructorTabStructure += "<img id='REConstructoroneImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_C"].ToString() + "' alt='Image'>";
                            ConstructorTabStructure += "<p id='REConstructoroneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_C"].ToString() + "</p></div></td>";
                            ConstructorTabStructure += "<td>";
                            ConstructorTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + ItagC + " viewBox='0 0 16 16'>";
                            ConstructorTabStructure += IPathC;
                            ConstructorTabStructure += "</svg></td>";
                            ConstructorTabStructure += "<td>" + dtrow["POINTS_C"].ToString() + "</td>";
                            ConstructorTabStructure += "</tr>";
                        }

                        if (dtrow["YOUR_ENTRY_PP"].ToString() != "" || dtrow["RACE_RESULT_PP"].ToString() != "")
                        {
                            PolePositionTabStructure += "<tr>";
                            PolePositionTabStructure += "<td>" + sno + "</td>";
                            PolePositionTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            PolePositionTabStructure += "<img id='YEPolePositiononeImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_PP"].ToString() + "' alt='Image'>";
                            PolePositionTabStructure += "<p id='YEPolePositiononeptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_PP"].ToString() + "</p></div></td>";
                            PolePositionTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            PolePositionTabStructure += "<img id='REPolePositiononeImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_PP"].ToString() + "' alt='Image'>";
                            PolePositionTabStructure += "<p id='REPolePositiononeptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_PP"].ToString() + "</p></div></td>";
                            PolePositionTabStructure += "<td>";
                            PolePositionTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + ItagPP + " viewBox='0 0 16 16'>";
                            PolePositionTabStructure += IPathPP;
                            PolePositionTabStructure += "</svg></td>";
                            PolePositionTabStructure += "<td>" + dtrow["POINTS_PP"].ToString() + "</td>";
                            PolePositionTabStructure += "<td>" + dtrow["AGAINST_THE_GRAIN_PP"].ToString() + "</td>";
                            PolePositionTabStructure += "</tr>";
                        }

                        if (dtrow["YOUR_ENTRY_FL"].ToString() != "" || dtrow["RACE_RESULT_FL"].ToString() != "")
                        {
                            ConstraintsTabStructure += "<tr>";
                            ConstraintsTabStructure += "<td>Fastest Lap</td>";
                            ConstraintsTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstraintsTabStructure += "<img id='YEFastestLaponeImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_FL"].ToString() + "' alt='Image'>";
                            ConstraintsTabStructure += "<p id='YEFastestLaponeptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_FL"].ToString() + "</p></div></td>";
                            ConstraintsTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstraintsTabStructure += "<img id='REFastestLaponeImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_FL"].ToString() + "' alt='Image'>";
                            ConstraintsTabStructure += "<p id='REFastestLaponeptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_FL"].ToString() + "</p></div></td>";
                            ConstraintsTabStructure += "<td>";
                            ConstraintsTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + ItagFL + " viewBox='0 0 16 16'>";
                            ConstraintsTabStructure += IPathFL;
                            ConstraintsTabStructure += "</svg></td>";
                            ConstraintsTabStructure += "<td>" + dtrow["POINTS_FL"].ToString() + "</td>";
                            ConstraintsTabStructure += "</tr>";
                        }

                        if (dtrow["YOUR_ENTRY_MPG"].ToString() != "" || dtrow["RACE_RESULT_MPG"].ToString() != "")
                        {
                            ConstraintsTabStructure += "<tr>";
                            ConstraintsTabStructure += "<td>Most Places Gained</td>";
                            ConstraintsTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstraintsTabStructure += "<img id='YEMPGoneImage" + sno + "' src='" + dtrow["YOUR_ENTRY_IMG_MPG"].ToString() + "' alt='Image'>";
                            ConstraintsTabStructure += "<p id='YEMPGoneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["YOUR_ENTRY_MPG"].ToString() + "</p></div></td>";
                            ConstraintsTabStructure += "<td style='padding: 10px 0 10px 40px;'><div style='cursor: pointer; display: flex; flex-direction: column; align-items: center; position: relative;' class='avatar avatar-xl me-5'>";
                            ConstraintsTabStructure += "<img id='REMPGoneImage" + sno + "' src='" + dtrow["RACE_RESULT_IMG_MPG"].ToString() + "' alt='Image'>";
                            ConstraintsTabStructure += "<p id='REMPGoneptag" + sno + "' style='margin: 5px 0 0 0;'>" + dtrow["RACE_RESULT_MPG"].ToString() + "</p></div></td>";
                            ConstraintsTabStructure += "<td>";
                            ConstraintsTabStructure += "<svg xmlns='http://www.w3.org/2000/svg' style='height: 40px; width: 40px;' " + ItagMPG + " viewBox='0 0 16 16'>";
                            ConstraintsTabStructure += IPathMPG;
                            ConstraintsTabStructure += "</svg></td>";
                            ConstraintsTabStructure += "<td>" + dtrow["POINTS_MPG"].ToString() + "</td>";
                            ConstraintsTabStructure += "</tr>";
                        }
                        sno++;
                    }

                    response = new
                    {
                        Message = "Success",
                        DriverTable = DriverTabStructure,
                        SprintEntryTable = SprintEntryTabStructure,
                        DriverRandomOrder = DriverRandomOrderPoints,
                        ConstructorTable = ConstructorTabStructure,
                        ConstructorRandomOrder = ConstructorRandomOrderPoints,
                        PolePositionTable = PolePositionTabStructure,
                        ConstraintsTable = ConstraintsTabStructure,
                        LadderChallengeEntryTable = "",
                        DbTitle = "- " + Year + " / " + Race
                    };
                }
                else
                {
                    response = new
                    {
                        Message = "Not given any race entries"
                    };
                }
            }
            catch (Exception ex)
            {
                response = new
                {
                    Message = ex.Message
                };
            }
        }
        else
        {
            response = new
            {
                Message = "No Data Found for this Year"
            };
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(response);
    }
    [WebMethod]
    public static string GettingTimeandRaceNameValuesFromDB(string Dummy)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry;
        SqlCommand cmd;
        SqlConnection cnn;
        object response = new { };

        try
        {
            qry = "SELECT TOP 1 RACE_NAME, YEAR, ROUND, DEAD_LINE_TIME " +
                  "FROM F1_MASTER_SCHEDULE " +
                  "WHERE FORMAT(DATEADD(day, 2, SECOND_PRACTICE_DATE), 'yyyy-MM-dd') >= CAST(GETDATE() AS DATE) AND " +
                  "YEAR = YEAR(GETDATE()) AND " +
                  "STATUS = 'Active';";

            DataTable dt = new DataTable();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                string RaceDateTime = "", RaceName = dtrow["RACE_NAME"].ToString();

                if (dtrow["DEAD_LINE_TIME"].ToString() != null && dtrow["DEAD_LINE_TIME"].ToString() != "")
                {
                    RaceDateTime = Convert.ToDateTime(dtrow["DEAD_LINE_TIME"]).ToString("MMM dd, yyyy HH:mm");                    
                }
                else { RaceDateTime = DateTime.Now.ToString("MMM dd, yyyy 20:25:00"); }
                
                response = new
                {
                    Message = "Success",
                    DateTimeValue = RaceDateTime,
                    RaceName = "Race Entry For : " + RaceName,
                    Year = dtrow["YEAR"].ToString(),
                    Round = dtrow["ROUND"].ToString()
                };
            }

            if (dt.Rows.Count == 0)
            {
                response = new
                {
                    Message = "Success",
                    DateTimeValue = DateTime.Now.ToString("MMM dd, yyyy 00:00:00"),
                    RaceName = "",
                    Year = "",
                    Round = ""
                };
            }
        }
        catch (Exception ex)
        {
            response = new
            {
                Message = ex.Message
            };
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(response);
    }
    [WebMethod]
    public static string GettingUsersImageDataFromDB(string UserID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry;
        SqlCommand cmd;
        SqlConnection cnn;
        object response = new { };

        try
        {
            qry = "SELECT IMAGE_URL FROM FORMULA_ONE_USER_DETAILS WHERE USER_ID = '" + UserID + "' AND STATUS = 'Active'";

            DataTable dt = new DataTable();
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                response = new
                {
                    Message = "Success",
                    ImageSrc = dtrow["IMAGE_URL"].ToString()
                };
            }

            if (dt.Rows.Count == 0)
            {
                response = new
                {
                    Message = "Success",
                    ImageSrc = "../assets/images/Formula1/UserImage/NoImage.jpg"
                };
            }
        }
        catch (Exception ex)
        {
            response = new
            {
                Message = ex.Message
            };
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(response);
    }
    [WebMethod]
    public static string CheckingtheUserDetailsFromDB(string UserID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader SqlReader;

        qry = "SELECT * FROM FORMULA_ONE_USER_DETAILS WHERE USER_ID = '" + UserID + "' AND STATUS = 'Active'";
        cnn = new SqlConnection(constr);
        cmd = new SqlCommand(qry, cnn);
        cnn.Open();
        SqlReader = cmd.ExecuteReader();
        while (SqlReader.Read())
        {
            if (SqlReader["DATE_OF_BIRTH"].ToString() == "" || SqlReader["PHONE_NO"].ToString() == "" || SqlReader["GENDER"].ToString() == "" || SqlReader["COUNTRY_OF_ORIGIN"].ToString() == "")
            {
                return "Not Success";
            }
        }
        cnn.Close();

        return "Success";
    }
}