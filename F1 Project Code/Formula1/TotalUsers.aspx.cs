using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Formula1_TotalUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, TableOutput = "";
        SqlConnection cnn = new SqlConnection(constr);
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand("SELECT " +
                                        "A.USER_NAME, A.IMAGE_URL, A.MAIL_ID, A.DATE_OF_BIRTH, A.PHONE_NO, A.GENDER, A.COUNTRY_OF_ORIGIN, '' AS [Subscription Status], '' AS [Amount], '' AS [Payment Date], " +
                                        "CASE WHEN (B.TEAM_IMG IS NULL OR B.TEAM_IMG = '') AND (B.TEAM_NAME IS NOT NULL OR B.TEAM_NAME <> '') THEN '../assets/images/Formula1/UserImage/NoImage.jpg' ELSE B.TEAM_IMG END AS [TEAM_IMG], " +
                                        "CASE WHEN B.TEAM_NAME = '' OR B.TEAM_NAME IS NULL THEN 'No Team' ELSE B.TEAM_NAME END AS [TEAM_NAME] " +
                                        "FROM FORMULA_ONE_USER_DETAILS A " +
                                        "LEFT JOIN F1_MASTER_TEAM B ON A.STATUS = B.STATUS AND (A.USER_NAME = B.MEMBER_1 OR A.USER_NAME = B.MEMBER_2 OR A.USER_NAME = B.MEMBER_3 OR A.USER_NAME = B.MEMBER_4) " +
                                        "WHERE A.STATUS = 'Active'", cnn);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        int i = 1;
        foreach (DataRow dtrow in dt.Rows)
        {
            TableOutput += "<tr>";
            TableOutput += "<td>" + i + "</td>";
            TableOutput += "<td>";
            TableOutput += "<div style='justify-content:left !important; display: flex; flex-direction: row; align-items: center; gap:20px;' class='avatar avatar-xl me-5'>";
            TableOutput += "<img id='UserImage" + i + "' src='" + dtrow["IMAGE_URL"].ToString() + "' alt=''>";
            TableOutput += "<p id='UserName" + i + "' style='margin: 5px 0 0 0;'>" + dtrow["USER_NAME"].ToString() + "</p>";
            TableOutput += "</div></td>";
            TableOutput += "<td>";
            TableOutput += "<div style='justify-content:left !important; display: flex; flex-direction: row; align-items: center; gap:20px;' class='avatar avatar-xl me-5'>";
            if (dtrow["TEAM_IMG"].ToString() != "" && dtrow["TEAM_IMG"].ToString() != null)
            {
                TableOutput += "<img id='TeamImage" + i + "' src='" + dtrow["TEAM_IMG"].ToString() + "' alt=''>";
            }            
            TableOutput += "<p id='TeamName" + i + "' style='margin: 5px 0 0 0;'>" + dtrow["TEAM_NAME"].ToString() + "</p>";
            TableOutput += "</div></td>";
            TableOutput += "<td>" + dtrow["MAIL_ID"].ToString() + "</td>";
            TableOutput += "<td>" + Convert.ToDateTime(dtrow["DATE_OF_BIRTH"]).ToString("dd-MM-yyyy") + "</td>";
            TableOutput += "<td>" + dtrow["PHONE_NO"].ToString() + "</td>";
            TableOutput += "<td>" + dtrow["GENDER"].ToString() + "</td>";
            TableOutput += "<td>" + dtrow["COUNTRY_OF_ORIGIN"].ToString() + "</td>";
            TableOutput += "<td>" + dtrow["Subscription Status"].ToString() + "</td>";
            TableOutput += "<td>" + dtrow["Amount"].ToString() + "</td>";
            TableOutput += "<td>" + dtrow["Payment Date"].ToString() + "</td>";
            TableOutput += "<td style='cursor:pointer;' title='Delete User'><svg xmlns='http://www.w3.org/2000/svg' style='width: 35px; height: 35px;' fill='red' class='bi bi-trash3-fill' viewBox='0 0 16 16'>\r\n  <path d='M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5'/>\r\n</svg></td>";
            TableOutput += "</tr>";
            i++;
        }
        CompetitorsTableBodyPH.Controls.Add(new Literal { Text = TableOutput });
    }
}