using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Formula1_F1UserProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(GetType(), "Javascript", "OnloadFunction();", true);

        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";

        qry = "SELECT 'Country Name' AS [DDNAME], COUNTRY AS [DDVALUE] FROM F1_MASTER_COUNTRY WHERE STATUS = 'Active' " +
              "UNION " +
              "SELECT 'Country Code' AS [DDNAME], COUNTRY_CODE AS [DDVALUE] FROM F1_MASTER_COUNTRY WHERE STATUS = 'Active' " +
              "ORDER BY 1,2;";

        DataTable dt = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        string CountryName = "", CountryCode = "";

        foreach (DataRow dtrow in dt.Rows)
        {
            if (dtrow["DDNAME"].ToString() == "Country Name")
            {
                CountryName += "<option value='" + dtrow["DDVALUE"].ToString() + "'>" + dtrow["DDVALUE"].ToString() + "</option>";
            }
            if (dtrow["DDNAME"].ToString() == "Country Code")
            {
                CountryCode += "<option value='" + dtrow["DDVALUE"].ToString() + "'>" + dtrow["DDVALUE"].ToString() + "</option>";
            }
        }
        OverallCountryList.Controls.Add(new Literal { Text = CountryName });
        CountryCodeListDD.Controls.Add(new Literal { Text = CountryCode });
    }

    [WebMethod]
    public static string GettingUsersDataFromDB(string UserID)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader SqlReader;
        object response = new { };

        try
        {
            qry = "SELECT * FROM FORMULA_ONE_USER_DETAILS WHERE USER_ID = '" + UserID + "' AND STATUS = 'Active'";
            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                string DOB = "";
                if (SqlReader["DATE_OF_BIRTH"].ToString() != "" && SqlReader["DATE_OF_BIRTH"].ToString() != null)
                {
                    DOB = String.Format("{0:yyyy-MM-dd}", SqlReader["DATE_OF_BIRTH"]);
                }
                response = new
                {
                    UserName = SqlReader["USER_NAME"].ToString(),
                    MailId = SqlReader["MAIL_ID"].ToString(),
                    ImageUrl = SqlReader["IMAGE_URL"].ToString(),
                    DateofBirth = DOB,
                    PhoneNo = SqlReader["PHONE_NO"].ToString(),
                    CountryCode = SqlReader["COUNTRY_CODE"].ToString(),
                    Gender = SqlReader["GENDER"].ToString(),
                    CountryofOrigin = SqlReader["COUNTRY_OF_ORIGIN"].ToString(),
                    Message = "Success"
                };

                break;
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

    [WebMethod]
    public static string GettingCountryDataFromDB(string Value, string appendid)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader SqlReader;
        object response = new { };

        try
        {
            if (appendid == "CountryofOrigin") { qry = "SELECT COUNTRY AS [OUTPUT] FROM F1_MASTER_COUNTRY WHERE COUNTRY_CODE = '" + Value + "' AND STATUS = 'Active'"; }
            if (appendid == "CountryCode") { qry = "SELECT COUNTRY_CODE AS [OUTPUT] FROM F1_MASTER_COUNTRY WHERE COUNTRY = '" + Value + "' AND STATUS = 'Active'"; }

            cnn = new SqlConnection(constr);
            cmd = new SqlCommand(qry, cnn);
            cnn.Open();
            SqlReader = cmd.ExecuteReader();
            while (SqlReader.Read())
            {
                response = new
                {
                    OutputValue = SqlReader["OUTPUT"].ToString(),
                    Message = "Success"
                };

                break;
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
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;

        try
        {
            foreach (var FormulaOneFields in DataList)
            {
                string Condition = "";
                if (FormulaOneFields.UserImageUrl != "") { Condition = "IMAGE_URL = @imageurl,"; }

                cmd = new SqlCommand("UPDATE FORMULA_ONE_USER_DETAILS SET DATE_OF_BIRTH = @dateofbirth, " + Condition + " PHONE_NO = @phoneno," +
                                     " GENDER = @gender, COUNTRY_OF_ORIGIN = @country, COUNTRY_CODE = @countrycode WHERE USER_ID = '" + FormulaOneFields.UserName + "' AND STATUS = 'Active'", cnn);

                if (FormulaOneFields.UserDateofBirth != "") { cmd.Parameters.AddWithValue("@dateofbirth", Convert.ToDateTime(FormulaOneFields.UserDateofBirth.Trim()).Date); }
                else { cmd.Parameters.AddWithValue("@dateofbirth", DBNull.Value); }
                if (FormulaOneFields.UserImageUrl != "") { cmd.Parameters.AddWithValue("@imageurl", FormulaOneFields.UserImageUrl.Trim()); }
                cmd.Parameters.AddWithValue("@phoneno", FormulaOneFields.UserPhoneNo.Trim());
                cmd.Parameters.AddWithValue("@gender", FormulaOneFields.UserGender.Trim());
                cmd.Parameters.AddWithValue("@country", FormulaOneFields.UserContryofOrigin.Trim());
                cmd.Parameters.AddWithValue("@countrycode", FormulaOneFields.UserContryofOriginCode.Trim());
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
        public string UserPhoneNo { get; set; }
        public string UserDateofBirth { get; set; }
        public string UserGender { get; set; }
        public string UserContryofOrigin { get; set; }
        public string UserContryofOriginCode { get; set; }
        public string UserImageUrl { get; set; }
        public string UserName { get; set; }
    }
}