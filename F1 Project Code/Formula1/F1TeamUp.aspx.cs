using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Formula1_F1TeamUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "GettingUsersData();TeamAddDropDownValues();localStorage.removeItem('TeamEdited');", true);

        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, TableOutput = "", BGColour = "", TextColour = "";
        SqlConnection cnn = new SqlConnection(constr);
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand("SELECT ID, TEAM_NAME, TEAM_IMG, " +
                                        "CASE " +
                                            "WHEN APPROVAL_TYPE = 'False' THEN 'Approval' " +
                                            "WHEN APPROVAL_TYPE = 'True' THEN 'Auto' " +
                                        "END AS [APPROVAL_TYPE], " +
                                        "CAST( " +
                                            "(CASE WHEN MEMBER_2 != '' AND MEMBER_2 IS NOT NULL THEN 1 ELSE 0 END + " +
                                            "CASE WHEN MEMBER_3 != '' AND MEMBER_3 IS NOT NULL THEN 1 ELSE 0 END + " +
                                            "CASE WHEN MEMBER_4 != '' AND MEMBER_4 IS NOT NULL THEN 1 ELSE 0 END + 1) " +
                                            "AS VARCHAR) + ' / 4' AS [MEMBERS] " +
                                        "FROM F1_MASTER_TEAM  " +
                                        "WHERE STATUS = 'Active';", cnn);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        int i = 1;
        foreach (DataRow dtrow in dt.Rows)
        {
            TableOutput += "<tr>";
            TableOutput += "<td><input onclick='GetTeamInfo(" + '"' + "" + dtrow["ID"].ToString() + "" + '"' + ")' class='form-check-input' type='radio' name='flexRadioDefault' id='flexRadioDefault" + i + "'></td>";
            TableOutput += "<td>";
            TableOutput += "<div style='justify-content:left !important; display: flex; flex-direction: row; align-items: center; gap:20px;' class='avatar avatar-xl me-5'>";
            TableOutput += "<img id='TeamImage" + i + "' src='" + dtrow["TEAM_IMG"].ToString() + "' alt='' srcset=''>";
            TableOutput += "<p id='TeamName" + i + "' style='margin: 5px 0 0 0;'>" + dtrow["TEAM_NAME"].ToString() + "</p>";
            TableOutput += "</div></td>";
            if (dtrow["MEMBERS"].ToString() == "1 / 4") { BGColour = "#198754"; TextColour = "white"; }
            if (dtrow["MEMBERS"].ToString() == "2 / 4") { BGColour = "#435EBE"; TextColour = "white"; }
            if (dtrow["MEMBERS"].ToString() == "3 / 4") { BGColour = "#FFC107"; TextColour = "black"; }
            if (dtrow["MEMBERS"].ToString() == "4 / 4") { BGColour = "#DD3545"; TextColour = "white"; }
            TableOutput += "<td><i style='font-size: 25px;' class='bi bi-people-fill'></i>&nbsp;&nbsp;<span class='MemberClass' style='color: " + TextColour + "; background-color: " + BGColour + ";'>" + dtrow["MEMBERS"].ToString() + "</span></td>";
            TableOutput += "<td>" + dtrow["APPROVAL_TYPE"].ToString() + "</td>";
            TableOutput += "</tr>";
            i++;
        }
        CreatedTeamsTableBodyPH.Controls.Add(new Literal { Text = TableOutput });
    }
    public static class DataStorage
    {
        public static string IsHeAdmin { get; set; }
        public static string IsHeTeamMember { get; set; }
    }
    [WebMethod]
    public static DropDownDetails[] DropDownValueMethod(string SessionUserName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry = "SELECT DISTINCT 'User Name' AS [DDNAME], USER_NAME AS [DDVALUE] FROM FORMULA_ONE_USER_DETAILS WHERE STATUS = 'Active' AND USER_ID NOT IN ('" + SessionUserName + "') AND " +
                     "USER_NAME NOT IN (SELECT MEMBER_1 AS [EXISTING_MEMBERS] FROM F1_MASTER_TEAM WHERE STATUS = 'Active' " +
                     "UNION SELECT MEMBER_2 FROM F1_MASTER_TEAM WHERE STATUS = 'Active' " +
                     "UNION SELECT MEMBER_3 FROM F1_MASTER_TEAM WHERE STATUS = 'Active' " +
                     "UNION SELECT MEMBER_4 FROM F1_MASTER_TEAM WHERE STATUS = 'Active') " +
                     "ORDER BY 1,2;";

        DataTable dt = new DataTable();
        List<DropDownDetails> details = new List<DropDownDetails>();
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

        return details.ToArray();
    }
    public class DropDownDetails
    {
        public string ddname { get; set; }
        public string ddvalue { get; set; }
    }
    [WebMethod]
    public static string GettingUsersDataFromDB(string UserID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
        SqlConnection cnn, cnnn;
        SqlCommand cmd, cmdd;
        SqlDataReader SqlReader;
        object response = new { }; bool Exists = false, Exists1 = false;
        int ng = 0;
        string ID = "", ApprovalBtn = "";

        try
        {
            qry = "SELECT * FROM F1_MASTER_TEAM WHERE MEMBER_1 = '" + UserID + "' AND STATUS = 'Active'";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                DataTable dt = new DataTable();
                cnnn = new SqlConnection(constr);
                cmdd = new SqlCommand("SELECT * FROM F1_TEAM_ADMIN_APPROVALS WHERE TEAM_ADMIN = '" + UserID + "' AND STATUS = 'Active'", cnnn);
                SqlDataAdapter da = new SqlDataAdapter(cmdd);
                da.Fill(dt);
                string ApprovalTable = "";
                if (dt.Rows.Count > 0) { ApprovalBtn = "<button type='button' id='ApprovalTeamId' class='btn btn-primary' data-bs-toggle='modal' data-bs-backdrop='false' data-bs-target='#primary'>Approvals</button>"; }
                foreach (DataRow dr in dt.Rows)
                {
                    ApprovalTable += "<tr>";
                    ApprovalTable += "<td>" +
                                     "<div style='justify-content:left !important; display: flex; flex-direction: row; align-items: center; gap:20px;' class='avatar avatar-xl me-5'>" +
                                     "<img src='" + dr["IMAGE_URL"].ToString() + "' alt='' srcset=''>" +
                                     "<p style='margin: 5px 0 0 0;'>" + dr["REQUESTED_BY"].ToString() + "</p>" +
                                     "</div>" +
                                     "</td>";
                    ApprovalTable += "<td>" + dr["MAIL_ID"].ToString() + "</td>";
                    ApprovalTable += "<td>" + dr["DOB"].ToString() + "</td>";
                    ApprovalTable += "<td>" + dr["MOBILE_NO"].ToString() + "</td>";
                    ApprovalTable += "<td>" + dr["GENDER"].ToString() + "</td>";
                    ApprovalTable += "<td>" + dr["COUNTRY"].ToString() + "</td>";
                    ApprovalTable += "<td>" + dr["UPDATED_ON"].ToString() + "</td>";
                    ApprovalTable += "<td><button onclick='ApproveorReject(" + '"' + "" + dr["UPDATE_ID"].ToString() + "" + '"' + ", " + '"' + "Approve" + '"' + ", " + '"' + "" + dr["REQUESTED_BY"].ToString() + "" + '"' + ", " + '"' + "" + dr["ID"].ToString() + "" + '"' + ")' type='button' class='badge bg-success'>Approve</button></td>";
                    ApprovalTable += "<td><button onclick='ApproveorReject(" + '"' + "" + dr["ID"].ToString() + "" + '"' + ", " + '"' + "Reject" + '"' + ", " + '"' + "NA" + '"' + ", " + '"' + "NA" + '"' + ")' type='button' class='badge bg-danger'>Reject</button></td>";
                    ApprovalTable += "</tr>";
                }

                response = new
                {
                    TeamName = SqlReader["TEAM_NAME"].ToString(),
                    TeamSlogan = SqlReader["SLOGAN"].ToString(),
                    ImageUrl = SqlReader["TEAM_IMG"].ToString(),
                    TeamMember1 = SqlReader["MEMBER_1"].ToString(),
                    TeamMember2 = SqlReader["MEMBER_2"].ToString(),
                    TeamMember3 = SqlReader["MEMBER_3"].ToString(),
                    TeamMember4 = SqlReader["MEMBER_4"].ToString(),
                    ApprovalType = SqlReader["APPROVAL_TYPE"].ToString(),
                    ApprovalTableStr = ApprovalTable,
                    TopButtonValue = "<button type='button' id='CreateTeamId' class='btn btn-success' data-bs-toggle='modal' data-bs-backdrop='false' data-bs-target='#CreateTeamPreview'>Edit Team</button>" +
                                     "" + ApprovalBtn + "",
                    SaveorDeletebtn = "<button type='button' class='btn btn-primary me-1 mb-1' onclick='FormulaOneDataSave(" + '"' + "" + SqlReader["ID"].ToString() + "" + '"' + ")' title='Save'><i style='font-size: 20px;' class='bi bi-floppy2-fill'></i></button>" +
                                      "<button type='button' title='Edit' onclick='EditUserProfile()' class='btn btn-success me-1 mb-1'><i style='font-size: 20px;' class='bi bi-pencil-fill'></i></button>" +
                                      "<button type='button' title='Delete' onclick='DeleteUserProfile()' class='btn btn-danger me-1 mb-1'><i style='font-size: 20px;' class='bi bi-trash3'></i></button>",
                    Message = "Success"
                };

                Exists = true;
                break;
            }
            cnn.Close();

            qry = "SELECT * FROM F1_MASTER_TEAM WHERE (MEMBER_2 = '" + UserID + "' OR MEMBER_3 = '" + UserID + "' OR MEMBER_4 = '" + UserID + "') AND STATUS = 'Active'";
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = null;
            SqlReader = cmd.ExecuteReader();
            string TeamNameStr = "";
            while (SqlReader.Read())
            {
                ID = SqlReader["ID"].ToString();
                TeamNameStr = SqlReader["TEAM_NAME"].ToString();
                Exists1 = true;
            }
            cnn.Close();

            if (!Exists1) { DataStorage.IsHeTeamMember = "No"; ng++; }
            else
            {
                DataStorage.IsHeTeamMember = "Yes";

                response = new
                {
                    TeamName = TeamNameStr,
                    TopButtonValue = "<button name='" + ID + "' type='button' id='ExitfromTeamId' class='btn btn-danger' data-bs-toggle='modal' data-bs-backdrop='false' data-bs-target='#success1'>Exit Team</button>",
                    Message = "TeamMember"
                };
            }

            if (!Exists)
            {
                DataStorage.IsHeAdmin = "No"; ng++;

                if (!Exists1)
                {
                    response = new
                    {
                        ImageUrl = "../assets/images/Formula1/UserImage/NoImage.jpg",
                        SaveorDeletebtn = "<button type='button' class='btn btn-primary me-1 mb-1' onclick='FormulaOneDataSave('Save')' title='Save'><i style='font-size: 20px;' class='bi bi-floppy2-fill'></i></button>",
                        Message = "NotSuccess"
                    };
                }
            }
            else { DataStorage.IsHeAdmin = "Yes"; }

            if (ng == 2)
            {
                response = new
                {
                    ImageUrl = "../assets/images/Formula1/UserImage/NoImage.jpg",
                    TopButtonValue = "<button type='button' id='CreateTeamId' class='btn btn-success' data-bs-toggle='modal' data-bs-backdrop='false' data-bs-target='#CreateTeamPreview'>Create Team</button>",
                    SaveorDeletebtn = "<button type='button' class='btn btn-primary me-1 mb-1' onclick='FormulaOneDataSave('Save')' title='Save'><i style='font-size: 20px;' class='bi bi-floppy2-fill'></i></button>",
                    Message = "NewTeamMember"
                };
            }
        }
        catch (Exception ER)
        {
            response = new
            {
                Message = ER.Message
            };
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(response);
    }

    [WebMethod(EnableSession = true)]
    public static string FormulaOneInsertData(List<DataList> DataList)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "", Exists = "No";
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        try
        {
            foreach (var FormulaOneFields in DataList)
            {
                if (FormulaOneFields.EType == "Save")
                {
                    cmd = new SqlCommand("SELECT COUNT(*) AS [MaxCount] FROM F1_MASTER_TEAM WHERE TEAM_NAME = '" + FormulaOneFields.TeamName + "' AND STATUS = 'Active'", cnn);
                    cnn.Open();
                    SqlDataReader SqlReader = cmd.ExecuteReader();
                    while (SqlReader.Read())
                    {
                        int Count = Convert.ToInt32(SqlReader["MaxCount"]);
                        if (Count > 0) { return "Team Name Already Exists."; }
                    }
                    cnn.Close();

                    qry = "INSERT INTO F1_MASTER_TEAM (TEAM_NAME,TEAM_IMG,MEMBER_1,MEMBER_2,MEMBER_3,MEMBER_4,APPROVAL_TYPE,SLOGAN,UPDATED_BY) " +
                          "VALUES (@TeamName,@TeamImg,@Member1,@Member2,@Member3,@Member4,@ApprovalType,@Slogan,@ub)";

                    Exists = "Insert";
                }
                else
                {
                    string ColumnName = "TEAM_IMG=@TeamImg,";
                    if (FormulaOneFields.UserImageUrl == "../assets/images/Formula1/UserImage/NoImage.jpg")
                    {
                        ColumnName = "";                        
                    }
                    else { Exists = "Update"; }
                    qry = "UPDATE F1_MASTER_TEAM SET TEAM_NAME=@TeamName, " + ColumnName + " MEMBER_1=@Member1, MEMBER_2=@Member2, MEMBER_3=@Member3, MEMBER_4=@Member4, " +
                          "APPROVAL_TYPE=@ApprovalType, SLOGAN=@Slogan, UPDATED_BY=@ub Where ID = '" + FormulaOneFields.EType + "' And STATUS = 'Active';";
                }

                cmd = new SqlCommand(qry, cnn);

                cmd.Parameters.AddWithValue("@ub", FormulaOneFields.UserName.Trim());
                cmd.Parameters.AddWithValue("@TeamName", FormulaOneFields.TeamName.Trim());
                if (Exists != "No")
                {
                    cmd.Parameters.AddWithValue("@TeamImg", FormulaOneFields.UserImageUrl.Trim());
                }                
                cmd.Parameters.AddWithValue("@Member1", FormulaOneFields.TeamMember1.Trim());
                cmd.Parameters.AddWithValue("@Member2", FormulaOneFields.TeamMember2.Trim());
                cmd.Parameters.AddWithValue("@Member3", FormulaOneFields.TeamMember3.Trim());
                cmd.Parameters.AddWithValue("@Member4", FormulaOneFields.TeamMember4.Trim());
                cmd.Parameters.AddWithValue("@ApprovalType", FormulaOneFields.ApprovalType.Trim());
                cmd.Parameters.AddWithValue("@Slogan", FormulaOneFields.TeamSlogan.Trim());
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
        public string TeamName { get; set; }
        public string TeamSlogan { get; set; }
        public string EType { get; set; }
        public string TeamMember1 { get; set; }
        public string TeamMember2 { get; set; }
        public string TeamMember3 { get; set; }
        public string TeamMember4 { get; set; }
        public string ApprovalType { get; set; }
        public string UserImageUrl { get; set; }
        public string UserName { get; set; }
    }
    [WebMethod(EnableSession = true)]
    public static string DeleteUserProfile(string UserID, string TeamName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        try
        {
            cmd = new SqlCommand("UPDATE F1_MASTER_TEAM SET STATUS = 'Deleted', UPDATED_BY = @UserID WHERE TEAM_NAME = @TeamName", cnn);

            cmd.Parameters.AddWithValue("@UserID", UserID.Trim());
            cmd.Parameters.AddWithValue("@TeamName", TeamName.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        catch (Exception ex)
        { return ex.Message; }

        return "Deleted";
    }

    [WebMethod]
    public static string GetTeamInfo(string TableId)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        object response = new { };

        try
        {
            string qry = "SELECT * FROM F1_MASTER_TEAM WHERE ID = '" + TableId + "' AND STATUS = 'Active'", TeamInfoTabStructure = "";

            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            qry = "SELECT USER_NAME, IMAGE_URL, MAIL_ID FROM FORMULA_ONE_USER_DETAILS WHERE STATUS = 'Active'";

            DataTable dt1 = new DataTable();
            cmd = new SqlCommand(qry, cnn);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt1);

            foreach (DataRow dtrow in dt.Rows)
            {
                TeamInfoTabStructure += "<div class='card'><div class='card-body text-center'>";
                TeamInfoTabStructure += "<h3 style='text-align: left;'>Team Info</h3><hr />";
                TeamInfoTabStructure += "<div style='gap: 20px;' class='d-flex flex-wrap align-items-center justify-content-center'>";
                TeamInfoTabStructure += "<div style='width: 110px; height: 110px;' class='avatar avatar-xl'>";
                TeamInfoTabStructure += "<img style='width: 110px; height: 110px;' src='" + dtrow["TEAM_IMG"].ToString() + "' alt='Team Image'></div>";
                TeamInfoTabStructure += "<div class='ms-3 name'><h3 class='font-bold'>" + dtrow["TEAM_NAME"].ToString() + "</h3>";
                TeamInfoTabStructure += "<h4 title='slogan' class='text-muted mb-0'>@" + dtrow["SLOGAN"].ToString() + "</h4></div></div><hr />";
                TeamInfoTabStructure += "<h3 style='text-align: left;'><i style='position: relative; top: -2px;' class='bi bi-person-lines-fill'></i>&nbsp;&nbsp;Admin</h3>";
                TeamInfoTabStructure += "<div style='gap: 20px;' class='d-flex flex-wrap align-items-center justify-content-center'>";
                TeamInfoTabStructure += "<div style='width: 80px; height: 80px;' class='avatar avatar-xl'>";
                string Member1Img = "", Member2Img = "", Member3Img = "", Member4Img = "";
                string Member1mail = "", ButtonText = "", HTMLButton = "";

                if (dtrow["APPROVAL_TYPE"].ToString() == "False") { ButtonText = "Request to Join"; }
                if (dtrow["APPROVAL_TYPE"].ToString() == "True") { ButtonText = "Join"; }

                foreach (DataRow dtrow1 in dt1.Rows)
                {
                    if (dtrow1["USER_NAME"].ToString() == dtrow["MEMBER_1"].ToString()) { Member1Img = dtrow1["IMAGE_URL"].ToString(); Member1mail = dtrow1["MAIL_ID"].ToString(); }
                    if (dtrow1["USER_NAME"].ToString() == dtrow["MEMBER_2"].ToString()) { Member2Img = dtrow1["IMAGE_URL"].ToString(); }
                    if (dtrow1["USER_NAME"].ToString() == dtrow["MEMBER_3"].ToString()) { Member3Img = dtrow1["IMAGE_URL"].ToString(); }
                    if (dtrow1["USER_NAME"].ToString() == dtrow["MEMBER_4"].ToString()) { Member4Img = dtrow1["IMAGE_URL"].ToString(); }
                }
                TeamInfoTabStructure += "<img style='width: 80px; height: 80px;' src='" + Member1Img + "' alt='Member 1'></div>";
                TeamInfoTabStructure += "<div class='ms-3 name'>";
                TeamInfoTabStructure += "<h4 class='font-bold'><i style='position: relative; top: -4px;' class='bi bi-person-check'></i>&nbsp;" + dtrow["MEMBER_1"].ToString() + "</h4>";
                TeamInfoTabStructure += "<h5 class='text-muted mb-0'>@" + Member1mail + "</h5></div></div>";
                if ((dtrow["MEMBER_2"].ToString() == "" || dtrow["MEMBER_2"].ToString() == null) && (dtrow["MEMBER_3"].ToString() == "" || dtrow["MEMBER_3"].ToString() == null) && (dtrow["MEMBER_4"].ToString() == "" || dtrow["MEMBER_4"].ToString() == null)) { }
                else
                {
                    TeamInfoTabStructure += "<hr /><h3 style='text-align: left;'><i style='position: relative; top: -3px;' class='bi bi-people-fill'></i>&nbsp;&nbsp;Team Members</h3>";
                    TeamInfoTabStructure += "<div class='d-flex flex-wrap justify-content-center' style='margin-top: 20px; gap: 20px;'>";

                    if (dtrow["MEMBER_2"].ToString() != "" && dtrow["MEMBER_2"].ToString() != null)
                    {
                        TeamInfoTabStructure += "<div style='display: flex; flex-direction: column; align-items: center; gap: 10px;' class='avatar avatar-xl me-1'>";
                        TeamInfoTabStructure += "<img src='" + Member2Img + "' alt='Member 2'><h4>" + dtrow["MEMBER_2"].ToString() + "</h4>";
                        TeamInfoTabStructure += "</div>";
                    }
                    if (dtrow["MEMBER_3"].ToString() != "" && dtrow["MEMBER_3"].ToString() != null)
                    {
                        TeamInfoTabStructure += "<div style='display: flex; flex-direction: column; align-items: center; gap: 10px;' class='avatar avatar-xl me-1'>";
                        TeamInfoTabStructure += "<img src='" + Member3Img + "' alt='Member 3'><h4>" + dtrow["MEMBER_3"].ToString() + "</h4>";
                        TeamInfoTabStructure += "</div>";
                    }
                    if (dtrow["MEMBER_4"].ToString() != "" && dtrow["MEMBER_4"].ToString() != null)
                    {
                        TeamInfoTabStructure += "<div style='display: flex; flex-direction: column; align-items: center; gap: 10px;' class='avatar avatar-xl me-1'>";
                        TeamInfoTabStructure += "<img src='" + Member4Img + "' alt='Member 4'><h4>" + dtrow["MEMBER_4"].ToString() + "</h4>";
                        TeamInfoTabStructure += "</div>";
                    }
                    TeamInfoTabStructure += "</div>";
                }
                if ((dtrow["MEMBER_2"].ToString() == "" || dtrow["MEMBER_2"].ToString() == null) || (dtrow["MEMBER_3"].ToString() == "" || dtrow["MEMBER_3"].ToString() == null) || (dtrow["MEMBER_4"].ToString() == "" || dtrow["MEMBER_4"].ToString() == null))
                {
                    if (DataStorage.IsHeTeamMember == "No" && DataStorage.IsHeAdmin == "No")
                    {
                        HTMLButton = "<button type='button' onclick='Jointheteam(" + '"' + "" + TableId + "" + '"' + ", " + '"' + "" + ButtonText + "" + '"' + ", " + '"' + "" + dtrow["MEMBER_1"].ToString() + "" + '"' + ", " + '"' + "" + Member1mail + "" + '"' + ")' style='height: 55px !important;' class='btn btn-block btn-xl btn-outline-primary font-bold mt-3'>" + ButtonText + "</button>";
                    }
                }
                TeamInfoTabStructure += HTMLButton + "</div></div>";
            }

            response = new
            {
                Message = "Success",
                TeamInfo = TeamInfoTabStructure
            };
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
    public static string Jointheteam(string id, string Type, string OriginalSessionUserName, string SessionUserName, string Admin, string MailtoSend)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        object response = new { };

        try
        {
            if (Type == "Join")
            {
                cmd = new SqlCommand("UPDATE F1_MASTER_TEAM " +
                     "SET MEMBER_2 = CASE " +
                                      "WHEN MEMBER_2 IS NULL OR MEMBER_2 = '' THEN @Value " +
                                      "ELSE MEMBER_2 " +
                                    "END, " +
                         "MEMBER_3 = CASE " +
                                      "WHEN (MEMBER_2 IS NOT NULL AND MEMBER_2 <> '') " +
                                      "THEN CASE WHEN MEMBER_3 IS NULL OR MEMBER_3 = '' THEN @Value ELSE MEMBER_3 END " +
                                      "ELSE MEMBER_3 " +
                                    "END, " +
                         "MEMBER_4 = CASE " +
                                      "WHEN (MEMBER_2 IS NOT NULL AND MEMBER_2 <> '') AND (MEMBER_3 IS NOT NULL AND MEMBER_3 <> '') " +
                                      "THEN CASE WHEN MEMBER_4 IS NULL OR MEMBER_4 = '' THEN @Value ELSE MEMBER_4 END " +
                                      "ELSE MEMBER_4 " +
                                    "END " +
                     "WHERE ID = @id AND STATUS = 'Active'", cnn);

                cmd.Parameters.AddWithValue("@Value", OriginalSessionUserName.Trim());
                cmd.Parameters.AddWithValue("@id", id.Trim());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                response = new
                {
                    Message = "Success",
                    OriginalMessage = "Joined Successfully"
                };
            }
            else if (Type == "Request to Join")
            {
                string mailermessage = "";
                try
                {
                    if (MailtoSend != "" && MailtoSend != null)
                    {
                        MailMessage msg = new MailMessage();
                        SmtpClient client = new SmtpClient();

                        msg.From = new MailAddress("testing@group.in", "ICSA Group"); // From Address is commen for both sending and receiving                        
                        msg.To.Add(MailtoSend);
                        msg.Subject = "Request for Team Up"; //Mail Subject                        

                        //Body Of the Mail
                        msg.Body = @"<!DOCTYPE html>
                                    <html lang='en'>
                                    <head>
	                                    <meta charset='UTF-8'>
	                                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
	                                    <title>Ultra F1 Fantasy League</title>
	                                    <style>
		                                    body {
			                                    font-family: Arial, sans-serif;
			                                    margin: 0;
			                                    padding: 0;
			                                    background-color: #f2f2f2;
		                                    }
		                                    .email-container {
			                                    max-width: 600px;
			                                    margin: 20px auto;
			                                    background-color: #141414;
			                                    border-radius: 10px;
			                                    overflow: hidden;
			                                    box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.15);
		                                    }
		                                    .header {
			                                    background-color: #1e1e1e;
			                                    text-align: center;
			                                    padding: 30px 0;
			                                    border-bottom: 1px solid #383838;
		                                    }
		                                    .header img {
			                                    width: 100px;
		                                    }
		                                    .header h1 {
			                                    color: #fff;
			                                    margin-top: 10px;
			                                    font-size: 24px;
			                                    text-transform: uppercase;
		                                    }
		                                    .content {
			                                    padding: 40px 20px;
			                                    text-align: center;
			                                    background-color: #1e1e1e;
		                                    }
		                                    .content h2 {
			                                    color: #00ff00;
			                                    font-size: 28px;
			                                    margin-bottom: 20px;
		                                    }
		                                    .content p {
			                                    color: #c7c7c7;
			                                    font-size: 16px;
			                                    margin-bottom: 20px;
		                                    }
		                                    .cta {
			                                    background-color: #ff4500;
			                                    color: #fff;
			                                    padding: 15px 30px;
			                                    text-transform: uppercase;
			                                    text-decoration: none;
			                                    border-radius: 5px;
			                                    display: inline-block;
			                                    font-weight: bold;
		                                    }
		                                    .cta:hover {
			                                    background-color: #ff5714;
		                                    }
		                                    .footer {
			                                    background-color: #1e1e1e;
			                                    padding: 20px;
			                                    text-align: center;
			                                    color: #777;
			                                    font-size: 14px;
			                                    border-top: 1px solid #383838;
		                                    }
	                                    </style>
                                    </head>
                                    <body>
	                                    <div class='email-container'>
		                                    <div class='header'>
			                                    <img src='http://216.48.183.76:8095/assets/images/Formula1/LoginImage/logo.png' data-filename='logo.png' style='width: 100px;'>
			                                    <h1>Ultra F1 Fantasy League</h1>
		                                    </div>

		                                    <div class='content'>
			                                    <h2>Team Up Request</h2>
			                                    <p>
				                                    Hi " + Admin + @",
				                                    <br><br>
				                                    You have a new team member request! Please review the details on the website.				                                    
				                                    <br><br>
				                                    To approve or decline, follow this path : <strong>Login → Dashboard → Team → Approvals</strong>
			                                    </p>
			                                    <a href='http://216.48.183.76:8095/Formula1Login.aspx' class='cta'>Login</a>
		                                    </div>

		                                    <div class='footer'>
			                                    <p>Thanks,<br>Team Ultra F1 Fantasy League</p>
		                                    </div>
	                                    </div>
                                    </body>
                                    </html>";

                        //SMTP options to send Mail
                        msg.IsBodyHtml = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("testing@group.in", "password");
                        client.Port = 587;
                        client.Host = "mail.outlook.in";
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;

                        //Send the Mail using Client
                        client.Send(msg);
                    }
                }
                catch (Exception)
                {
                    mailermessage = "";
                }

                cmd = new SqlCommand("INSERT INTO F1_TEAM_ADMIN_APPROVALS (TEAM_ADMIN, REQUESTED_BY, MAIL_ID, IMAGE_URL, DOB, MOBILE_NO, GENDER, COUNTRY, UPDATED_BY, UPDATE_ID) " +
                                     "SELECT '" + Admin + "' AS [Admin], USER_NAME, MAIL_ID, IMAGE_URL, DATE_OF_BIRTH, PHONE_NO, GENDER, COUNTRY_OF_ORIGIN, USER_ID, '" + id + "' AS [UPDATE_ID] FROM FORMULA_ONE_USER_DETAILS WHERE USER_ID = @UserId AND STATUS = 'Active'", cnn);

                cmd.Parameters.AddWithValue("@UserId", SessionUserName.Trim());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                if (mailermessage == "") { mailermessage = "Request Sent Successfully to this Email - [" + MailtoSend + "]"; }

                response = new
                {
                    Message = "Success",
                    OriginalMessage = mailermessage
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
    public static string Exittheteam(string id, string OriginalSessionUserName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        object response = new { };

        try
        {
            cmd = new SqlCommand("UPDATE F1_MASTER_TEAM " +
                                     "SET MEMBER_2 = CASE " +
                                                      "WHEN MEMBER_2 = @Value THEN '' " +
                                                      "ELSE MEMBER_2 " +
                                                    "END, " +
                                         "MEMBER_3 = CASE " +
                                                      "WHEN MEMBER_3 = @Value THEN '' " +
                                                      "ELSE MEMBER_3 " +
                                                    "END, " +
                                         "MEMBER_4 = CASE " +
                                                      "WHEN MEMBER_4 = @Value THEN '' " +
                                                      "ELSE MEMBER_4 " +
                                                    "END " +
                                     "WHERE ID = @id AND STATUS = 'Active'", cnn);

            cmd.Parameters.AddWithValue("@Value", OriginalSessionUserName.Trim());
            cmd.Parameters.AddWithValue("@id", id.Trim());
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();

            response = new
            {
                Message = "Success",
                OriginalMessage = "Exited Successfully"
            };
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
    public static string ApproveorReject(string UpdateId, string Type, string RequestedBy, string id)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        string ReturnMessage = "";

        try
        {
            if (Type == "Approve")
            {
                string qry = "SELECT * FROM F1_MASTER_TEAM WHERE (MEMBER_1 = '" + RequestedBy + "' OR MEMBER_2 = '" + RequestedBy + "' OR MEMBER_3 = '" + RequestedBy + "' OR MEMBER_4 = '" + RequestedBy + "') AND STATUS = 'Active'";

                DataTable dt = new DataTable();
                cnn = new SqlConnection(constr);
                cmd = new SqlCommand(qry, cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    cmd = new SqlCommand("UPDATE F1_TEAM_ADMIN_APPROVALS SET STATUS = 'already a member' WHERE ID = @id", cnn);

                    cmd.Parameters.AddWithValue("@id", id.Trim());
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();

                    return "This user is already a member of another team.";
                }
                else
                {
                    DataTable dt1 = new DataTable();
                    SqlConnection cnnn = new SqlConnection(constr);
                    SqlCommand cmdd = new SqlCommand("SELECT * FROM F1_MASTER_TEAM WHERE ID = '" + UpdateId + "' AND STATUS = 'Active';", cnnn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmdd);
                    da1.Fill(dt1);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if ((dr["MEMBER_2"].ToString() != "" && dr["MEMBER_2"].ToString() != null) && (dr["MEMBER_3"].ToString() != "" && dr["MEMBER_3"].ToString() != null) && (dr["MEMBER_4"].ToString() != "" && dr["MEMBER_4"].ToString() != null))
                        {
                            cmd = new SqlCommand("UPDATE F1_TEAM_ADMIN_APPROVALS SET STATUS = 'Team is Full' WHERE ID = @id", cnn);

                            cmd.Parameters.AddWithValue("@id", id.Trim());
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            cnn.Close();

                            return "Team is Full.";
                        }
                    }

                    cmdd = new SqlCommand("UPDATE F1_MASTER_TEAM " +
                                     "SET MEMBER_2 = CASE " +
                                                      "WHEN MEMBER_2 IS NULL OR MEMBER_2 = '' THEN @Value " +
                                                      "ELSE MEMBER_2 " +
                                                    "END, " +
                                         "MEMBER_3 = CASE " +
                                                      "WHEN (MEMBER_2 IS NOT NULL AND MEMBER_2 <> '') " +
                                                      "THEN CASE WHEN MEMBER_3 IS NULL OR MEMBER_3 = '' THEN @Value ELSE MEMBER_3 END " +
                                                      "ELSE MEMBER_3 " +
                                                    "END, " +
                                         "MEMBER_4 = CASE " +
                                                      "WHEN (MEMBER_2 IS NOT NULL AND MEMBER_2 <> '') AND (MEMBER_3 IS NOT NULL AND MEMBER_3 <> '') " +
                                                      "THEN CASE WHEN MEMBER_4 IS NULL OR MEMBER_4 = '' THEN @Value ELSE MEMBER_4 END " +
                                                      "ELSE MEMBER_4 " +
                                                    "END " +
                                     "WHERE ID = @id AND STATUS = 'Active'", cnnn);


                    cmdd.Parameters.AddWithValue("@Value", RequestedBy.Trim());
                    cmdd.Parameters.AddWithValue("@id", UpdateId.Trim());
                    cnnn.Open();
                    cmdd.ExecuteNonQuery();
                    cnnn.Close();

                    cmd = new SqlCommand("UPDATE F1_TEAM_ADMIN_APPROVALS SET STATUS = 'Approved' WHERE ID = @id", cnn);

                    cmd.Parameters.AddWithValue("@id", id.Trim());
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();

                    return "Approved Successfully.";
                }
            }
            else if (Type == "Reject")
            {
                cmd = new SqlCommand("UPDATE F1_TEAM_ADMIN_APPROVALS SET STATUS = 'Rejected' WHERE ID = @id", cnn);

                cmd.Parameters.AddWithValue("@id", UpdateId.Trim());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                return "Reject Successfully.";
            }
        }
        catch (Exception ex)
        {
            ReturnMessage = ex.Message;
        }

        return ReturnMessage;
    }
}