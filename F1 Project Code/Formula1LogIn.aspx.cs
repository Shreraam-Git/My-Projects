using Google.Apis.Auth;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail; //Mail Reference
using System.Web.Script.Serialization;
using System.Web.Services;

public partial class Formula1LogIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod(EnableSession = true)]
    public static string CheckCredential(string LoginUserName, string LoginPassword)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry;
        SqlCommand cmd;
        SqlConnection cnn, cnnn;
        object response = new { };

        try
        {
            DataTable dt = new DataTable();
            qry = "SELECT * FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = @ec AND PASSWORD = @pwd AND STATUS = 'Active';";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cmd.Parameters.AddWithValue("@ec", LoginUserName);
            cmd.Parameters.AddWithValue("@pwd", LoginPassword);
            SqlDataAdapter da = new SqlDataAdapter(cmd);            
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                response = new
                {
                    Message = "Create an account if you haven't signed up yet."
                };
            }
            else
            {
                foreach (DataRow dtrow in dt.Rows)
                {
                    if (LoginUserName == dtrow["USER_NAME"].ToString() && LoginPassword == dtrow["PASSWORD"].ToString())
                    {
                        qry = "SELECT USER_ID FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + LoginUserName + "' AND PASSWORD = '" + LoginPassword + "' AND STATUS = 'Active';";
                        cnnn = new SqlConnection(constr);
                        cmd = new SqlCommand(qry, cnnn);
                        cnnn.Open();
                        SqlDataReader SqlReader = cmd.ExecuteReader();
                        while (SqlReader.Read())
                        {
                            response = new
                            {
                                UserID = SqlReader["USER_ID"].ToString(),
                                Message = "Success"
                            };
                        }
                    }
                    else
                    {
                        response = new
                        {
                            Message = "Invalid User Name or Password! Please try again!"
                        };
                    }
                }
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
    public class DropDownDetails
    {
        public string ErrorMessage { get; set; }
    }

    [WebMethod(EnableSession = true)]
    public static string AddCredentialSignUp(string SignUpUserName, string SignUpEmailId, string SignUpPassword)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry;
        SqlCommand cmd;
        SqlConnection cnn = new SqlConnection(constr);
        string UserID = "";

        try
        {
            cmd = new SqlCommand("SELECT COUNT(*) AS [COUNT] FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + SignUpUserName + "' AND STATUS = 'Active'", cnn);
            cnn.Open();
            SqlDataReader sqlreader = null;
            sqlreader = cmd.ExecuteReader();
            sqlreader.Read();
            int DupCount1 = Convert.ToInt32(sqlreader["COUNT"]);
            cnn.Close();

            if (DupCount1 > 0)
            {
                return "Username Already Exists.";
            }

            cmd = new SqlCommand("SELECT COUNT(*) AS [COUNT] FROM FORMULA_ONE_USER_DETAILS WHERE MAIL_ID = '" + SignUpEmailId + "' AND STATUS = 'Active'", cnn);
            cnn.Open();
            sqlreader = null;
            sqlreader = cmd.ExecuteReader();
            sqlreader.Read();
            int DupCount2 = Convert.ToInt32(sqlreader["COUNT"]);
            cnn.Close();

            if (DupCount2 > 0)
            {
                return "Mail-Id Already Exists.";
            }

            cmd = new SqlCommand("SELECT COUNT(*) AS [COUNT] FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + SignUpUserName + "' AND MAIL_ID = '" + SignUpEmailId + "' AND PASSWORD = '" + SignUpPassword + "' AND STATUS = 'Active'", cnn);
            cnn.Open();
            sqlreader = null;
            sqlreader = cmd.ExecuteReader();
            sqlreader.Read();
            int DupCount = Convert.ToInt32(sqlreader["COUNT"]);
            cnn.Close();

            if (DupCount == 0)
            {
                int u = 1;
                for (int t = 1; t <= u; t++)
                {
                    string randomString = GenerateRandomString(10);

                    cmd = new SqlCommand("SELECT COUNT(*) AS [USER_ID_COUNT] FROM FORMULA_ONE_USER_DETAILS WHERE USER_ID = '" + randomString + "' AND STATUS = 'Active'", cnn);
                    cnn.Open();
                    sqlreader = null;
                    sqlreader = cmd.ExecuteReader();
                    sqlreader.Read();
                    int user_id_count = Convert.ToInt32(sqlreader["USER_ID_COUNT"]);
                    cnn.Close();

                    if (user_id_count > 0) { u++; }
                    if (user_id_count == 0) { UserID = randomString; }
                }                

                qry = "INSERT INTO FORMULA_ONE_USER_DETAILS (USER_ID, USER_NAME, MAIL_ID, PASSWORD, UPDATED_BY, IMAGE_URL) VALUES (@UserID, @UserName, @MailId, @password, @ub, @image_url)";
                cmd = new SqlCommand(qry, cnn);

                cmd.Parameters.AddWithValue("@UserID", UserID.Trim());
                cmd.Parameters.AddWithValue("@UserName", SignUpUserName.Trim());
                cmd.Parameters.AddWithValue("@MailId", SignUpEmailId.Trim());
                cmd.Parameters.AddWithValue("@password", SignUpPassword.Trim());
                cmd.Parameters.AddWithValue("@ub", SignUpUserName.Trim());                
                cmd.Parameters.AddWithValue("@image_url", "../assets/images/Formula1/UserImage/NoImage.jpg");
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            else
            {
                return "Account Already Exists.";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "Success";
    }

    [WebMethod(EnableSession = true)]
    public static string ChangeCredentialSignUp(string CPUserName, string CPPassword, string CPNewPassword)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry;
        SqlCommand cmd;
        SqlConnection cnn = new SqlConnection(constr);

        try
        {
            cmd = new SqlCommand("SELECT COUNT(*) AS [COUNT] FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + CPUserName + "' AND PASSWORD = '" + CPPassword + "' AND STATUS = 'Active'", cnn);
            cnn.Open();
            SqlDataReader sqlreader = null;
            sqlreader = cmd.ExecuteReader();
            sqlreader.Read();
            int DupCount = Convert.ToInt32(sqlreader["COUNT"]);
            cnn.Close();

            if (DupCount > 0)
            {
                qry = "UPDATE FORMULA_ONE_USER_DETAILS SET PASSWORD = @Newpassword, UPDATED_BY = @ub, UPDATED_ON = GETDATE() WHERE USER_NAME = @UserName AND PASSWORD = @password AND STATUS = 'Active'";
                cmd = new SqlCommand(qry, cnn);

                cmd.Parameters.AddWithValue("@UserName", CPUserName.Trim());
                cmd.Parameters.AddWithValue("@password", CPPassword.Trim());
                cmd.Parameters.AddWithValue("@Newpassword", CPNewPassword.Trim());
                cmd.Parameters.AddWithValue("@ub", CPUserName.Trim());
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            else
            {
                return "Account doesn't Exists or May be the old password is wrong.";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "Success";
    }

    [WebMethod(EnableSession = true)]
    public static string ForgotCredentialSignUp(string ForgotUserName)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        string qry, MailId = "";
        SqlCommand cmd;
        SqlConnection cnn;
        object response = new { };

        try
        {
            DataTable dt = new DataTable();
            qry = "SELECT PASSWORD FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = @ec";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cmd.Parameters.AddWithValue("@ec", ForgotUserName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                response = new
                {
                    Message = "UserName is In-valid."
                };
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Sending Mail
                    if (dr["PASSWORD"].ToString() != "")
                    {
                        try
                        {
                            MailMessage msg = new MailMessage();
                            SmtpClient client = new SmtpClient();

                            msg.From = new MailAddress("testing@group.in", "ICSA Group"); // From Address is commen for both sending and receiving

                            qry = "SELECT MAIL_ID FROM FORMULA_ONE_USER_DETAILS WHERE USER_NAME = '" + ForgotUserName + "' AND STATUS = 'Active'";
                            cmd = new SqlCommand(qry, cnn);
                            SqlDataReader sqlrd = null;
                            cnn.Open();
                            sqlrd = cmd.ExecuteReader();
                            while (sqlrd.Read())
                            {
                                MailId = sqlrd["MAIL_ID"].ToString();
                                msg.To.Add(sqlrd["MAIL_ID"].ToString());
                            }
                            cnn.Close();

                            msg.Subject = "Password Recovery"; //Mail Subject                            

                            //Body Of the Mail
                            //<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAAB2CAYAAAA+/DbEAAAACXBIWXMAAAsTAAALEwEAmpwYAAAGn2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDUgNzkuMTYzNDk5LCAyMDE4LzA4LzEzLTE2OjQwOjIyICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJHb29nbGUiIHhtcDpDcmVhdGVEYXRlPSIyMDE5LTA0LTExVDE2OjAwOjQ2KzA1OjMwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAxOS0wNC0xMVQxNjoxNToyMCswNTozMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxOS0wNC0xMVQxNjoxNToyMCswNTozMCIgZGM6Zm9ybWF0PSJpbWFnZS9wbmciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDplNGIwMDZlZi1kMjEzLWYwNDQtOTcwYy1jNjkzZDg5ODQ4YTciIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6ZmVlOTliZTItZjYwMi1kODQwLTkxNDItMmVhMjIxNDkwNWQ5IiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZmVlOTliZTItZjYwMi1kODQwLTkxNDItMmVhMjIxNDkwNWQ5Ij4gPGRjOnJpZ2h0cz4gPHJkZjpBbHQ+IDxyZGY6bGkgeG1sOmxhbmc9IngtZGVmYXVsdCI+U2FoYXJhIEZvcmNlIEluZGlhPC9yZGY6bGk+IDwvcmRmOkFsdD4gPC9kYzpyaWdodHM+IDxkYzpjcmVhdG9yPiA8cmRmOlNlcT4gPHJkZjpsaT5KYW1lcyBNb3kgUGhvdG9ncmFwaHk8L3JkZjpsaT4gPC9yZGY6U2VxPiA8L2RjOmNyZWF0b3I+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmZlZTk5YmUyLWY2MDItZDg0MC05MTQyLTJlYTIyMTQ5MDVkOSIgc3RFdnQ6d2hlbj0iMjAxOS0wNC0xMVQxNjoxMzo1NyswNTozMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTkgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJzYXZlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDplNGIwMDZlZi1kMjEzLWYwNDQtOTcwYy1jNjkzZDg5ODQ4YTciIHN0RXZ0OndoZW49IjIwMTktMDQtMTFUMTY6MTU6MjArMDU6MzAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE5IChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7ZEuabAAA5KklEQVR4nO2ddZxdxfn/38eur/tm4+4GEUIgwYo7xaH8Wlqk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' data-filename='logo.png' style='width: 100px;'>

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
			                                    <h2>Password Recovery Request</h2>
			                                    <p>
				                                    Hi " + ForgotUserName + @",
				                                    <br><br>
				                                    We have received a request to recover your password. Here is your password : [<strong>" + dr["PASSWORD"].ToString() + @"</strong>],
				                                    <br><br>
				                                    For security reasons, we recommend changing your password after logging in.
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
                        catch (Exception ErrorMsg)
                        {
                            response = new
                            {
                                Message = ErrorMsg.Message
                            };
                        }
                    }

                    break;
                }

                response = new
                {
                    Message = "DataSent",
                    MailValue = MailId
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

    //[WebMethod(EnableSession = true)]
    //public static string SignInGoogle(string id_token)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
    //    //string qry;
    //    //SqlCommand cmd;
    //    SqlConnection cnn = new SqlConnection(constr);

    //    try
    //    {
    //        var payload = GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings
    //        {
    //            Audience = new[] { "859693609337-1auodfdkuek1i3hb17pnlkfftqiiooi5.apps.googleusercontent.com" }
    //        }).Result;

    //        // The payload object contains information about the user
    //        string userId = payload.Subject;
    //        string email = payload.Email;
    //        string name = payload.Name;
    //        string picture = payload.Picture;

    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }

    //    return "Success";
    //}

    [WebMethod(EnableSession = true)]
    public static string SignInGoogle(string id_token)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);

        try
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "859693609337-1auodfdkuek1i3hb17pnlkfftqiiooi5.apps.googleusercontent.com" }
            }).Result;

            // The payload object contains information about the user
            string userId = payload.Subject;
            string email = payload.Email;
            string name = payload.Name;
            string picture = payload.Picture;

            // Log payload details for debugging
            //System.Diagnostics.Debug.WriteLine($"UserID: {userId}, Email: {email}, Name: {name}, Picture: {picture}");

            // Here you can add code to save these details to your database

        }
        catch (Exception ex)
        {
            //System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            return ex.Message;
        }

        return "Success";
    }
    static string GenerateRandomString(int length)
    {
        // Define the character set for text and numbers
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Create a random number generator
        Random random = new Random();

        // Generate the random string
        string result = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return result;
    }
}