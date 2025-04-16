using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml.Linq;

public partial class Formula1_ScheduleDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        YearValue.Text = DateTime.Now.Year.ToString();

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "GettingScheduleDataFromDB();", true);
    }
    [WebMethod]
    public static string GettingScheduleValuesFromAPI(string UserID, string Year)
    {
        if (Year != "" && UserID != "")
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, url = "";
            SqlConnection cnn = new SqlConnection(constr);

            using (HttpClient client = new HttpClient())
            {
                url = "http://ergast.com/api/f1/" + Year + "";

                client.BaseAddress = new Uri(url);

                try
                {
                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string xmlResponse = response.Content.ReadAsStringAsync().Result;

                        //Check if the content type is XML
                        if (response.Content.Headers.ContentType.MediaType == "application/xml" ||
                            response.Content.Headers.ContentType.MediaType == "text/xml")
                        {
                            XDocument xmlDoc = XDocument.Parse(xmlResponse);
                            XNamespace ns = "http://ergast.com/mrd/1.5";

                            var Races = xmlDoc.Descendants(ns + "Race");

                            SqlCommand cmd = new SqlCommand("SELECT COUNT(YEAR) AS [MAXCOUNT] FROM F1_MASTER_SCHEDULE WHERE YEAR = '" + Year + "' AND STATUS = 'Active'", cnn);
                            cnn.Open();
                            SqlDataReader sqlreader = null;
                            sqlreader = cmd.ExecuteReader();
                            sqlreader.Read();
                            int DupCount = Convert.ToInt32(sqlreader["MAXCOUNT"].ToString());
                            cnn.Close();

                            if (DupCount > 0)
                            {
                                cmd = new SqlCommand("UPDATE F1_MASTER_SCHEDULE SET STATUS = 'Deleted' WHERE YEAR = '" + Year + "'", cnn);
                                cnn.Open();
                                cmd.ExecuteNonQuery();
                                cnn.Close();
                            }

                            foreach (var Race in Races)
                            {
                                var CircuitElement = Race.Element(ns + "Circuit");
                                var LocationElement = CircuitElement.Element(ns + "Location");

                                //----------------Calculating the Race Start and End Date--------------------
                                DateTime StringDate = DateTime.Parse(Race.Element(ns + "Date").Value);
                                string EndDate = StringDate.ToString("dd");
                                string EndMonth = StringDate.ToString("MMM");

                                string dateTime = StringDate.AddDays(-2).ToString(), FinalMonth = "";
                                DateTime AddedDate = DateTime.Parse(dateTime);
                                string StartDate = AddedDate.ToString("dd");
                                string StartMonth = AddedDate.ToString("MMM");

                                if (StartMonth == EndMonth) { FinalMonth = StartMonth; }
                                else { FinalMonth = StartMonth + " - " + EndMonth; }
                                //----------------Calculating the Race Start and End Date--------------------

                                //-------------------------Getting Country Flags URL-------------------------
                                Task<string> task = GetFlagUrlAsync(LocationElement.Element(ns + "Country").Value);
                                task.Wait();
                                string flagUrl = task.Result;
                                //-------------------------Getting Country Flags URL-------------------------

                                //-----------------------------Getting Race Time-----------------------------
                                string CountryName = LocationElement.Element(ns + "Country").Value;
                                if (CountryName == "UAE") { CountryName = "United Arab Emirates"; }

                                string RaceLocalandTrackTime = Race.Element(ns + "Time").Value;

                                var FirstPracticeElement = Race.Element(ns + "FirstPractice");
                                var SecondPracticeElement = Race.Element(ns + "SecondPractice");
                                var QualifyingElement = Race.Element(ns + "Qualifying");
                                var ThirdPracticeElement = Race.Element(ns + "ThirdPractice");
                                var SprintElement = Race.Element(ns + "Sprint");
                                string third_practice_time = DBNull.Value.ToString(), sprint_time = DBNull.Value.ToString();

                                string first_practice_time = FirstPracticeElement.Element(ns + "Time").Value;
                                string second_practice_time = SecondPracticeElement.Element(ns + "Time").Value;
                                string qualifying_time = QualifyingElement.Element(ns + "Time").Value;
                                if (ThirdPracticeElement != null) { third_practice_time = ThirdPracticeElement.Element(ns + "Time").Value; }
                                if (SprintElement != null) { sprint_time = SprintElement.Element(ns + "Time").Value; }
                                //-----------------------------Getting Race Time-----------------------------

                                cmd = new SqlCommand("INSERT INTO F1_MASTER_SCHEDULE (COUNTRY_NAME, YEAR, ROUND, CIRCUIT_ID, RACE_BOI_URL, RACE_NAME, RACE_DATE, RACE_MONTH, FLAG_URL," +
                                                     "ACTUAL_RACE_DATE,ACTUAL_RACE_MONTH,ACTUAL_RACE_TIME,FIRST_PRACTICE_DATE,FIRST_PRACTICE_TIME,SECOND_PRACTICE_DATE,SECOND_PRACTICE_TIME,THIRD_PRACTICE_DATE,THIRD_PRACTICE_TIME,QUALIFYING_DATE,QUALIFYING_TIME,SPRINT_DATE,SPRINT_TIME, " +
                                                     "UPDATED_BY)" +
                                                     " VALUES (@country_name, @year, @round, @circuit_id, @race_boi_url, @race_name, @race_date, @race_month, @flag_url," +
                                                     "@actual_race_date,@actual_race_month,@actual_race_time,@first_practice_date,@first_practice_time,@second_practice_date,@second_practice_time,@third_practice_date,@third_practice_time,@qualifying_date,@qualifying_time,@sprint_date,@sprint_time, " +
                                                     "@ub)", cnn);

                                cmd.Parameters.AddWithValue("@country_name", CountryName); //Year
                                cmd.Parameters.AddWithValue("@year", Year); //Year
                                cmd.Parameters.AddWithValue("@round", Race.Attribute("round").Value); //Race Round
                                cmd.Parameters.AddWithValue("@circuit_id", CircuitElement.Attribute("circuitId").Value); //Circuit ID
                                cmd.Parameters.AddWithValue("@race_boi_url", Race.Attribute("url").Value); //Race Boi Url
                                cmd.Parameters.AddWithValue("@race_name", Race.Element(ns + "RaceName").Value); //Race Name
                                cmd.Parameters.AddWithValue("@race_date", StartDate.Trim() + " - " + EndDate.Trim()); //Race Date                               
                                cmd.Parameters.AddWithValue("@race_month", FinalMonth.Trim()); // Race Month
                                cmd.Parameters.AddWithValue("@flag_url", flagUrl.Trim()); //Country Flag Image Url
                                cmd.Parameters.AddWithValue("@actual_race_date", EndDate.Trim()); //Actual Race Date
                                cmd.Parameters.AddWithValue("@actual_race_month", EndMonth.Trim()); //Actual Race Month
                                cmd.Parameters.AddWithValue("@actual_race_time", RaceLocalandTrackTime.Trim()); //Actual Race Time
                                cmd.Parameters.AddWithValue("@first_practice_date", FirstPracticeElement.Element(ns + "Date").Value.Trim()); //1st Practice Date
                                cmd.Parameters.AddWithValue("@first_practice_time", first_practice_time.Trim()); //1st Practice Time
                                cmd.Parameters.AddWithValue("@second_practice_date", SecondPracticeElement.Element(ns + "Date").Value.Trim()); //2nd Practice Date
                                cmd.Parameters.AddWithValue("@second_practice_time", second_practice_time.Trim()); //2nd Practice Time                        
                                cmd.Parameters.AddWithValue("@qualifying_date", QualifyingElement.Element(ns + "Date").Value.Trim()); //Qualifying Date
                                cmd.Parameters.AddWithValue("@qualifying_time", qualifying_time.Trim()); //Qualifying Time
                                if (ThirdPracticeElement != null) { cmd.Parameters.AddWithValue("@third_practice_date", ThirdPracticeElement.Element(ns + "Date").Value.Trim()); }//3rd Practice Date                        
                                else { cmd.Parameters.AddWithValue("@third_practice_date", DBNull.Value); }
                                cmd.Parameters.AddWithValue("@third_practice_time", third_practice_time.Trim()); //3rd Practice Time
                                if (SprintElement != null) { cmd.Parameters.AddWithValue("@sprint_date", SprintElement.Element(ns + "Date").Value.Trim()); }//Sprint Date                        
                                else { cmd.Parameters.AddWithValue("@sprint_date", DBNull.Value); }
                                cmd.Parameters.AddWithValue("@sprint_time", sprint_time.Trim()); //Sprint Time
                                cmd.Parameters.AddWithValue("@ub", UserID); //User Id
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
    static string getTrackTime(string utcTimeStr, string country)
    {
        string Tracktime = "";

        //----------------------Calculating Track Time----------------------
        DateTime utcDateTime = DateTime.ParseExact(utcTimeStr, "HH:mm:ss'Z'", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);

        // Convert DateTime to Instant
        Instant utcInstant = Instant.FromDateTimeUtc(utcDateTime);

        // Map country to time zone using Noda Time
        DateTimeZone timeZone = GetTimeZoneForCountry(country);
        if (timeZone != null)
        {
            // Convert UTC time to local time in the specified time zone
            ZonedDateTime localTime = utcInstant.InZone(timeZone);
            Tracktime = localTime.ToString("HH:mm", null);
        }
        //----------------------Calculating Track Time----------------------       

        return Tracktime;
    }
    static string getMyTime(string utcTimeStr)
    {
        string MyTime = "";

        //----------------------Calculating Local Time----------------------
        // Parse the UTC time string to a DateTime object
        DateTime utcTime = DateTime.Parse(utcTimeStr).ToUniversalTime();

        // Get the local time zone of the system
        TimeZoneInfo localZone = TimeZoneInfo.Local;

        // Convert the UTC time to local time
        DateTime localMyTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localZone);

        // Output the results       
        MyTime = localMyTime.ToString("HH:mm", null);
        //----------------------Calculating Local Time----------------------

        return MyTime;
    }

    static DateTimeZone GetTimeZoneForCountry(string country)
    {
        var countryCodeMap = TzdbDateTimeZoneSource.Default.ZoneLocations;

        foreach (var location in countryCodeMap)
        {
            if (location.CountryName.Equals(country, StringComparison.OrdinalIgnoreCase))
            {
                return DateTimeZoneProviders.Tzdb[location.ZoneId];
            }
        }
        return null;
    }
    static Task<string> GetFlagUrlAsync(string countryName)
    {
        return Task.Run(() =>
        {
            string apiUrl = "https://restcountries.com/v3.1/name/" + countryName + "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    response.EnsureSuccessStatusCode();

                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    JsonDocument jsonDoc = JsonDocument.Parse(responseBody);
                    JsonElement root = jsonDoc.RootElement;

                    if (root.GetArrayLength() > 0)
                    {
                        JsonElement countryData = root[0];
                        JsonElement flagsElement;

                        if (countryData.TryGetProperty("flags", out flagsElement))
                        {
                            JsonElement flagUrlElement;

                            if (flagsElement.TryGetProperty("png", out flagUrlElement))
                            {
                                return flagUrlElement.GetString();
                            }
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    return "Request error: " + e.Message;
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
            return null;
        });
    }

    [WebMethod]
    public static string SaveDeadLineTime(string UpdateId, string DateTimeValue)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("UPDATE F1_MASTER_SCHEDULE Set DEAD_LINE_TIME = @DateTimeValue Where ID = @id And STATUS = 'Active';", cnn);

            cmd.Parameters.AddWithValue("@id", UpdateId);
            if (DateTimeValue != "" && DateTimeValue != null) { cmd.Parameters.AddWithValue("@DateTimeValue", Convert.ToDateTime(DateTimeValue)); }
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        catch (Exception Ex)
        {
            return Ex.Message;
        }

        return "Success";
    }

    [WebMethod]
    public static string GettingScheduleDataFromDB(string Year)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;

        string qry = "SELECT * FROM F1_MASTER_SCHEDULE WHERE YEAR = '" + Year + "' AND STATUS = 'Active'";
        DataTable DT = new DataTable(), DT1 = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT); int i = 1;

        qry = "SELECT * FROM F1_MASTER_CIRCUITS WHERE YEAR = '" + Year + "' AND STATUS = 'Active';";
        DT1 = new DataTable();
        cnn = new SqlConnection(constr);
        cmd = new SqlCommand(qry, cnn);
        da = new SqlDataAdapter(cmd);
        cb = new SqlCommandBuilder(da);
        da.Fill(DT1);

        foreach (DataRow dtrow in DT.Rows)
        {
            string ImageName = "", Country = "", ImageUrl = "../assets/images/Formula1/CircuitsImages/", Style = "", Style1 = "", MouseOverFunc = "", href = "";

            foreach (DataRow SqlReader in DT1.Rows)
            {
                if (SqlReader["CIRCUIT_ID"].ToString() == dtrow["CIRCUIT_ID"].ToString())
                {
                    ImageName = SqlReader["CIRCUIT_FILE_NAME"].ToString();
                    Country = SqlReader["CIRCUIT_COUNTRY"].ToString();
                    href = SqlReader["CIRCUIT_BOI_URL"].ToString();
                }
            }

            ConditionBuliding.Append("<div class='col-3'>");
            ConditionBuliding.Append("<div class='card'>");
            ConditionBuliding.Append("<div class='card-body'>");

            ConditionBuliding.Append("<div class='ThreeDotdropdown'>");
            ConditionBuliding.Append("<button type='button' class='dropbtn' onmouseover='toggleDropdown(" + '"' + "myDropdown" + i + "" + '"' + ")'>⋮</button>");
            ConditionBuliding.Append("<div class='ThreeDotdropdown-content' id='myDropdown" + i + "'>");
            ConditionBuliding.Append("<a style='cursor:pointer;' data-bs-toggle='modal' data-bs-target='#SummaryDetailsPopup' onclick='RaceTimingFunc(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ")'><b>Race Timing</b></a><div style='margin:-18px 0 -18px 0;'><hr/></div>");
            ConditionBuliding.Append("<a style='cursor:pointer;' onclick='RemoveImageFromDBandFiles(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "Delete" + '"' + ")'><b>Delete Schedule</b></a>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("</div>");

            ConditionBuliding.Append("<div class='d-flex justify-content-center align-items-center flex-column'>");
            ConditionBuliding.Append("<h5 style='color: red;'>Round - " + dtrow["ROUND"].ToString() + "</h5>");
            if (ImageName == "") { Style = "style='display:none;'"; }
            else { Style1 = "style='display:none;'"; MouseOverFunc = "oncontextmenu='onmouseoverFunc(event, this)' title='Right click to Zoom' onmouseout='onmouseoutFunc(this)'"; }
            ConditionBuliding.Append("<div " + MouseOverFunc + " class='round-div' id='image-container" + i + "'>");
            ConditionBuliding.Append("<a href='" + href + "' target='_blank'><img " + Style + " id='uploaded-image" + i + "' src='" + ImageUrl + "" + ImageName + "' alt='Circuit Image'></a>");
            ConditionBuliding.Append("<div " + Style1 + " class='upload-icon'>Upload Image</div>");
            ConditionBuliding.Append("</div>");

            ConditionBuliding.Append("<div class='divcontainer'><div class='divdate'>");
            ConditionBuliding.Append("<span class='divday'>" + dtrow["RACE_DATE"].ToString() + "</span>");
            ConditionBuliding.Append("<span class='divmonth'>" + dtrow["RACE_MONTH"].ToString() + "</span>");
            ConditionBuliding.Append("</div><div class='flag'>");
            ConditionBuliding.Append("<img style='border: 2px solid black;' src='" + dtrow["FLAG_URL"].ToString() + "' alt='Flag'>");
            ConditionBuliding.Append("</div></div>");

            ConditionBuliding.Append("<div class='col-md-12'><div class='form-group'>");
            ConditionBuliding.Append("<h6 class='mt-2' style='text-align: center;'>Set Dead Line Time</h6>");
            ConditionBuliding.Append("<div class='input-group'>");
            if (dtrow["DEAD_LINE_TIME"].ToString() != "" && dtrow["DEAD_LINE_TIME"].ToString() != null) { ConditionBuliding.Append("<input style='text-align:center;' value='" + Convert.ToDateTime(dtrow["DEAD_LINE_TIME"]).ToString("yyyy-MM-dd hh:mm") + "' type='datetime-local' class='form-control' id='DeadlineTime" + i + "' />"); }
            else { ConditionBuliding.Append("<input style='text-align:center;' type='datetime-local' class='form-control' id='DeadlineTime" + i + "' />"); }
            ConditionBuliding.Append("<button class='btn btn-primary' onclick='SaveDeadLineTime(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ", " + '"' + "DeadlineTime" + i + "" + '"' + ")' type='button' title='Save' id='DeadlineTimeSave" + i + "'><i style='font-size: 20px;' class='bi bi-floppy2-fill'></i></button>");
            ConditionBuliding.Append("</div></div></div>");

            ConditionBuliding.Append("<h3 class='mt-1' style='text-align:center;'>" + Country + "</h3>");
            ConditionBuliding.Append("<p class='text-small'><b>" + dtrow["RACE_NAME"].ToString() + "</b></p>");
            ConditionBuliding.Append("<button type='button' style='margin-top: -10px;' data-bs-toggle='modal' data-bs-target='#ImagePreview' class='custom-btn Animebtn-5' onclick='AppendIframe(" + '"' + "" + dtrow["RACE_BOI_URL"].ToString() + "" + '"' + ")'><span>Read More &#x27A4;</span></button>");
            ConditionBuliding.Append("</div></div></div></div>");

            i++;
        }

        return ConditionBuliding.ToString();
    }
    [WebMethod]
    public static string GettingRaceTimeDataFromDB(string TableID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, ActualpassingDateTime = "";
        object response = new { };

        string qry = "SELECT * FROM F1_MASTER_SCHEDULE WHERE ID = '" + TableID + "' AND STATUS = 'Active'";
        DataTable DT = new DataTable(), DT1 = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT); int i = 1;

        foreach (DataRow dtrow in DT.Rows)
        {
            //For First Practice Time
            string first_practice_Local_Time = "", Actual_first_practice_Local_Time = "", first_practice_Track_Time = "";
            if (dtrow["FIRST_PRACTICE_TIME"].ToString() != null && dtrow["FIRST_PRACTICE_TIME"].ToString() != "")
            {
                string MyTime = getMyTime(dtrow["FIRST_PRACTICE_TIME"].ToString()), TrackTime = getTrackTime(dtrow["FIRST_PRACTICE_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());

                if (MyTime != null && MyTime != "")
                {
                    first_practice_Local_Time = MyTime + " - " + Convert.ToDateTime(MyTime).AddHours(1).ToString("HH:mm");
                    Actual_first_practice_Local_Time = MyTime;
                }
                if (TrackTime != null && TrackTime != "") { first_practice_Track_Time = TrackTime + " - " + Convert.ToDateTime(TrackTime).AddHours(1).ToString("HH:mm"); }
            }
            //For Second Practice Time
            string Sec_practice_Local_Time = "", Actual_Sec_practice_Local_Time = "", Sec_practice_Track_Time = "";
            if (dtrow["SECOND_PRACTICE_TIME"].ToString() != null && dtrow["SECOND_PRACTICE_TIME"].ToString() != "")
            {
                string MyTime = getMyTime(dtrow["SECOND_PRACTICE_TIME"].ToString()), TrackTime = getTrackTime(dtrow["SECOND_PRACTICE_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());

                if (MyTime != null && MyTime != "")
                {
                    Sec_practice_Local_Time = MyTime + " - " + Convert.ToDateTime(MyTime).AddHours(1).ToString("HH:mm");
                    Actual_Sec_practice_Local_Time = MyTime;
                }
                if (TrackTime != null && TrackTime != "")
                {
                    Sec_practice_Track_Time = TrackTime + " - " + Convert.ToDateTime(TrackTime).AddHours(1).ToString("HH:mm");
                }
            }
            //For Third Practice Time
            string Third_practice_Local_Time = "", Actual_Third_practice_Local_Time = "", Third_practice_Track_Time = "";
            if (dtrow["THIRD_PRACTICE_TIME"].ToString() != null && dtrow["THIRD_PRACTICE_TIME"].ToString() != "")
            {
                string MyTime = getMyTime(dtrow["THIRD_PRACTICE_TIME"].ToString()), TrackTime = getTrackTime(dtrow["THIRD_PRACTICE_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());

                if (MyTime != null && MyTime != "")
                {
                    Third_practice_Local_Time = MyTime + " - " + Convert.ToDateTime(MyTime).AddHours(1).ToString("HH:mm");
                    Actual_Third_practice_Local_Time = MyTime;
                }
                if (TrackTime != null && TrackTime != "")
                {
                    Third_practice_Track_Time = TrackTime + " - " + Convert.ToDateTime(TrackTime).AddHours(1).ToString("HH:mm");
                }
            }
            //For Sprint Time
            string Sprint_Local_Time = "", Actual_Sprint_Local_Time = "", Sprint_Track_Time = "";
            if (dtrow["SPRINT_TIME"].ToString() != null && dtrow["SPRINT_TIME"].ToString() != "")
            {
                string MyTime = getMyTime(dtrow["SPRINT_TIME"].ToString()), TrackTime = getTrackTime(dtrow["SPRINT_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());

                if (MyTime != null && MyTime != "")
                {
                    Sprint_Local_Time = MyTime + " - " + Convert.ToDateTime(MyTime).AddHours(1).ToString("HH:mm");
                    Actual_Sprint_Local_Time = MyTime;
                }
                if (TrackTime != null && TrackTime != "")
                {
                    Sprint_Track_Time = TrackTime + " - " + Convert.ToDateTime(TrackTime).AddHours(1).ToString("HH:mm");
                }
            }
            //For Qualifying Time
            string Qualifying_Local_Time = "", Actual_Qualifying_Local_Time = "", Qualifying_Track_Time = "";
            if (dtrow["QUALIFYING_TIME"].ToString() != null && dtrow["QUALIFYING_TIME"].ToString() != "")
            {
                string MyTime = getMyTime(dtrow["QUALIFYING_TIME"].ToString()), TrackTime = getTrackTime(dtrow["QUALIFYING_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());

                if (MyTime != null && MyTime != "")
                {
                    Qualifying_Local_Time = MyTime + " - " + Convert.ToDateTime(MyTime).AddHours(1).ToString("HH:mm");
                    Actual_Qualifying_Local_Time = MyTime;
                }
                if (TrackTime != null && TrackTime != "")
                {
                    Qualifying_Track_Time = TrackTime + " - " + Convert.ToDateTime(TrackTime).AddHours(1).ToString("HH:mm");
                }
            }
            //For Race Time
            string Race_Local_Time = "", Race_Track_Time = "";
            if (dtrow["ACTUAL_RACE_TIME"].ToString() != null && dtrow["ACTUAL_RACE_TIME"].ToString() != "")
            {
                Race_Local_Time = getMyTime(dtrow["ACTUAL_RACE_TIME"].ToString());
                Race_Track_Time = getTrackTime(dtrow["ACTUAL_RACE_TIME"].ToString(), dtrow["COUNTRY_NAME"].ToString());
            }

            //Spliting Date and month for First Practice
            DateTime FirstparsedDate; string FPDate = "", FPMonth = ""; bool FPD = false;
            if (dtrow["FIRST_PRACTICE_DATE"].ToString() != "")
            {
                string FirstPracticeDateTime = dtrow["FIRST_PRACTICE_DATE"].ToString() + " " + Actual_first_practice_Local_Time;
                DateTime firstPracticeDateTime = DateTime.Parse(FirstPracticeDateTime);
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime > firstPracticeDateTime) { ActualpassingDateTime = ""; }
                else { FPD = true; ActualpassingDateTime = firstPracticeDateTime.ToString(); }

                if (DateTime.TryParse(dtrow["FIRST_PRACTICE_DATE"].ToString(), out FirstparsedDate))
                {
                    FPDate = FirstparsedDate.Day.ToString();
                    FPMonth = FirstparsedDate.ToString("MMM");
                }
            }
            //Spliting Date and month for Sec Practice
            DateTime SecparsedDate; string SPDate = "", SPMonth = ""; bool SPD = false;
            if (dtrow["SECOND_PRACTICE_DATE"].ToString() != "")
            {
                if (!FPD)
                {
                    string SecPracticeDateTime = dtrow["SECOND_PRACTICE_DATE"].ToString() + " " + Actual_Sec_practice_Local_Time;
                    DateTime SecondPracticeDateTime = DateTime.Parse(SecPracticeDateTime);
                    DateTime currentDateTime = DateTime.Now;

                    if (currentDateTime > SecondPracticeDateTime) { ActualpassingDateTime = ""; }
                    else { SPD = true; ActualpassingDateTime = SecondPracticeDateTime.ToString(); }
                }

                if (DateTime.TryParse(dtrow["SECOND_PRACTICE_DATE"].ToString(), out SecparsedDate))
                {
                    SPDate = SecparsedDate.Day.ToString();
                    SPMonth = SecparsedDate.ToString("MMM");
                }
            }
            //Spliting Date and month for Third Practice
            DateTime ThirdparsedDate; string TPDate = "", TPMonth = ""; bool TPD = false;
            if (dtrow["THIRD_PRACTICE_DATE"].ToString() != "")
            {
                if (!FPD && !SPD)
                {
                    string ThirdPracticeDateTime = dtrow["THIRD_PRACTICE_DATE"].ToString() + " " + Actual_Third_practice_Local_Time;
                    DateTime ThiPracticeDateTime = DateTime.Parse(ThirdPracticeDateTime);
                    DateTime currentDateTime = DateTime.Now;

                    if (currentDateTime > ThiPracticeDateTime) { ActualpassingDateTime = ""; }
                    else { TPD = true; ActualpassingDateTime = ThiPracticeDateTime.ToString(); }
                }

                if (DateTime.TryParse(dtrow["THIRD_PRACTICE_DATE"].ToString(), out ThirdparsedDate))
                {
                    TPDate = ThirdparsedDate.Day.ToString();
                    TPMonth = ThirdparsedDate.ToString("MMM");
                }
            }
            //Spliting Date and month for Qualify
            DateTime QualifyparsedDate; string QPDate = "", QPMonth = ""; bool QPD = false;
            if (dtrow["QUALIFYING_DATE"].ToString() != "")
            {
                if (!FPD && !SPD && !TPD)
                {
                    string QualifyPracticeDateTime = dtrow["QUALIFYING_DATE"].ToString() + " " + Actual_Qualifying_Local_Time;
                    DateTime QuaPracticeDateTime = DateTime.Parse(QualifyPracticeDateTime);
                    DateTime currentDateTime = DateTime.Now;

                    if (currentDateTime > QuaPracticeDateTime) { ActualpassingDateTime = ""; }
                    else { QPD = true; ActualpassingDateTime = QuaPracticeDateTime.ToString(); }
                }

                if (DateTime.TryParse(dtrow["QUALIFYING_DATE"].ToString(), out QualifyparsedDate))
                {
                    QPDate = QualifyparsedDate.Day.ToString();
                    QPMonth = QualifyparsedDate.ToString("MMM");
                }
            }
            //Spliting Date and month for Sprint
            DateTime SprintparsedDate; string SPIDate = "", SPIMonth = ""; bool SIPD = false;
            if (dtrow["SPRINT_DATE"].ToString() != "")
            {
                if (!FPD && !SPD && !TPD && !QPD)
                {
                    string SpiPracticeDateTime = dtrow["SPRINT_DATE"].ToString() + " " + Actual_Sprint_Local_Time;
                    DateTime SprintPracticeDateTime = DateTime.Parse(SpiPracticeDateTime);
                    DateTime currentDateTime = DateTime.Now;

                    if (currentDateTime > SprintPracticeDateTime) { ActualpassingDateTime = ""; }
                    else { SIPD = true; ActualpassingDateTime = SprintPracticeDateTime.ToString(); }
                }

                if (DateTime.TryParse(dtrow["SPRINT_DATE"].ToString(), out SprintparsedDate))
                {
                    SPIDate = SprintparsedDate.Day.ToString();
                    SPIMonth = SprintparsedDate.ToString("MMM");
                }
            }
            //Spliting Date and month for Race            
            if (dtrow["ACTUAL_RACE_DATE"].ToString() != "")
            {
                if (!FPD && !SPD && !TPD && !QPD && !SIPD)
                {
                    string RacePracticeDateTime = dtrow["ACTUAL_RACE_DATE"].ToString() + "-" + dtrow["ACTUAL_RACE_MONTH"].ToString() + "-" + dtrow["YEAR"].ToString() + " " + Race_Local_Time;
                    DateTime RCPracticeDateTime = DateTime.Parse(RacePracticeDateTime);
                    DateTime currentDateTime = DateTime.Now;

                    if (currentDateTime > RCPracticeDateTime) { ActualpassingDateTime = ""; }
                    else { ActualpassingDateTime = RCPracticeDateTime.ToString(); }
                }
            }

            ConditionBuliding.Append("<h5 class='countdown-container' style='color:white; text-align:center;'>GRAND PRIX WEEKEND</h5>");
            ConditionBuliding.Append("<div class='countdown-container'><div class='countdown-timer'>");
            ConditionBuliding.Append("<div class='time-box'>");
            ConditionBuliding.Append("<span class='countdowntime' id='CountDowndays'>00</span>");
            ConditionBuliding.Append("<span class='label'>DAYS</span>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("<div class='time-box'>");
            ConditionBuliding.Append("<span class='countdowntime' id='CountDownhours'>00</span>");
            ConditionBuliding.Append("<span class='label'>HRS</span>");
            ConditionBuliding.Append("</div>");
            ConditionBuliding.Append("<div class='time-box'>");
            ConditionBuliding.Append("<span class='countdowntime' id='CountDownminutes'>00</span>");
            ConditionBuliding.Append("<span class='label'>MINS</span>");
            ConditionBuliding.Append("</div></div>");
            ConditionBuliding.Append("<div class='clock'><div class='outer-clock-face'><div class='marking marking-one'></div><div class='marking marking-two'></div>");
            ConditionBuliding.Append("<div class='marking marking-three'></div><div class='marking marking-four'></div><div class='inner-clock-face'><div class='hand hour-hand'></div>");
            ConditionBuliding.Append("<div class='hand min-hand'></div><div class='hand second-hand'></div></div></div></div></div>");
            ConditionBuliding.Append("<div class='tabs'>");
            ConditionBuliding.Append("<span id='MyTime' onclick='Changetabstoactive(this.id)' class='tab active'>MY TIME</span>");
            ConditionBuliding.Append("<span id='TrackTime' onclick='Changetabstoactive(this.id)' class='tab'>TRACK TIME</span>");
            ConditionBuliding.Append("</div>");

            //------------------------Local Time-----------------------------
            ConditionBuliding.Append("<div class='schedule' id='MyTimeSchedule'>");
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + FPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + FPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Practice 1</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + first_practice_Local_Time + "</span>");
            ConditionBuliding.Append("</div></div>");
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + SPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + SPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            if (dtrow["SPRINT_DATE"].ToString() != "" && dtrow["SPRINT_DATE"].ToString() != null && dtrow["SPRINT_TIME"].ToString() != "" && dtrow["SPRINT_TIME"].ToString() != null)
            {
                ConditionBuliding.Append("<span class='title'>Sprint Qualifying</span>");
            }
            else
            {
                ConditionBuliding.Append("<span class='title'>Practice 2</span>");
            }
            ConditionBuliding.Append("<span class='tabtime'>" + Sec_practice_Local_Time + "</span>");
            ConditionBuliding.Append("</div></div>");
            if (dtrow["SPRINT_DATE"].ToString() != "" && dtrow["SPRINT_DATE"].ToString() != null && dtrow["SPRINT_TIME"].ToString() != "" && dtrow["SPRINT_TIME"].ToString() != null)
            {
                ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
                ConditionBuliding.Append("<span class='tabday'>" + SPIDate + "</span>");
                ConditionBuliding.Append("<span class='tabmonth'>" + SPIMonth + " </span>");
                ConditionBuliding.Append("</div><div class='details'>");
                ConditionBuliding.Append("<span class='title'>Sprint</span>");
                ConditionBuliding.Append("<span class='tabtime'>" + Sprint_Local_Time + "</span>");
                ConditionBuliding.Append("</div></div>");
            }
            else
            {
                ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
                ConditionBuliding.Append("<span class='tabday'>" + TPDate + "</span>");
                ConditionBuliding.Append("<span class='tabmonth'>" + TPMonth + " </span>");
                ConditionBuliding.Append("</div><div class='details'>");
                ConditionBuliding.Append("<span class='title'>Practice 3</span>");
                ConditionBuliding.Append("<span class='tabtime'>" + Third_practice_Local_Time + "</span>");
                ConditionBuliding.Append("</div></div>");
            }
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + QPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + QPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Qualifying</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + Qualifying_Local_Time + "</span>");
            ConditionBuliding.Append("</div></div>");

            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + dtrow["ACTUAL_RACE_DATE"].ToString() + " </span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + dtrow["ACTUAL_RACE_MONTH"].ToString() + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Race</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + Race_Local_Time + "</span>");
            ConditionBuliding.Append("</div></div></div>");

            //------------------------Track Time-----------------------------            
            ConditionBuliding.Append("<div class='schedule' style='display:none;' id='TrackTimeSchedule'>");
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + FPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + FPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Practice 1</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + first_practice_Track_Time + "</span>");
            ConditionBuliding.Append("</div></div>");
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + SPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + SPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            if (dtrow["SPRINT_DATE"].ToString() != "" && dtrow["SPRINT_DATE"].ToString() != null && dtrow["SPRINT_TIME"].ToString() != "" && dtrow["SPRINT_TIME"].ToString() != null)
            {
                ConditionBuliding.Append("<span class='title'>Sprint Qualifying</span>");
            }
            else
            {
                ConditionBuliding.Append("<span class='title'>Practice 2</span>");
            }
            ConditionBuliding.Append("<span class='tabtime'>" + Sec_practice_Track_Time + "</span>");
            ConditionBuliding.Append("</div></div>");
            if (dtrow["SPRINT_DATE"].ToString() != "" && dtrow["SPRINT_DATE"].ToString() != null && dtrow["SPRINT_TIME"].ToString() != "" && dtrow["SPRINT_TIME"].ToString() != null)
            {
                ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
                ConditionBuliding.Append("<span class='tabday'>" + SPIDate + "</span>");
                ConditionBuliding.Append("<span class='tabmonth'>" + SPIMonth + " </span>");
                ConditionBuliding.Append("</div><div class='details'>");
                ConditionBuliding.Append("<span class='title'>Sprint</span>");
                ConditionBuliding.Append("<span class='tabtime'>" + Sprint_Track_Time + "</span>");
                ConditionBuliding.Append("</div></div>");
            }
            else
            {
                ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
                ConditionBuliding.Append("<span class='tabday'>" + TPDate + "</span>");
                ConditionBuliding.Append("<span class='tabmonth'>" + TPMonth + " </span>");
                ConditionBuliding.Append("</div><div class='details'>");
                ConditionBuliding.Append("<span class='title'>Practice 3</span>");
                ConditionBuliding.Append("<span class='tabtime'>" + Third_practice_Track_Time + "</span>");
                ConditionBuliding.Append("</div></div>");
            }
            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + QPDate + "</span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + QPMonth + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Qualifying</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + Qualifying_Track_Time + "</span>");
            ConditionBuliding.Append("</div></div>");

            ConditionBuliding.Append("<div class='event'><div class='tabdate'>");
            ConditionBuliding.Append("<span class='tabday'>" + dtrow["ACTUAL_RACE_DATE"].ToString() + " </span>");
            ConditionBuliding.Append("<span class='tabmonth'>" + dtrow["ACTUAL_RACE_MONTH"].ToString() + " </span>");
            ConditionBuliding.Append("</div><div class='details'>");
            ConditionBuliding.Append("<span class='title'>Race</span>");
            ConditionBuliding.Append("<span class='tabtime'>" + Race_Track_Time + "</span>");
            ConditionBuliding.Append("</div></div></div>");

            i++;
        }

        response = new
        {
            DateTime = ActualpassingDateTime,
            RaceInfoStyle = ConditionBuliding.ToString()
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(response);
    }

    [WebMethod]
    public static string UpdateScheduleName(string ImageID, string DeleteCode)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
            SqlConnection cnn;
            SqlCommand cmd;

            if (DeleteCode == "Delete") { qry = "UPDATE F1_MASTER_SCHEDULE SET STATUS = 'Deleted' WHERE ID = '" + ImageID + "';"; }

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
}