using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;

public partial class Formula1_RulesandRegulation : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString, qry = "";
    SqlConnection cnn;
    SqlCommand cmd;
    SqlDataReader SqlReader;
    protected void Page_Load(object sender, EventArgs e)
    {        
        qry = "SELECT * FROM F1_MASTER_RULES_AND_REGULATION WHERE STATUS = 'Active';";
        cnn = new SqlConnection(constr);
        cmd = new SqlCommand(qry, cnn);
        StringBuilder ConditionBuliding = new StringBuilder();
        cnn.Open();
        SqlReader = cmd.ExecuteReader();        
        while (SqlReader.Read())
        {                        
            ConditionBuliding.Append("<li class='list-group-item' style='font-size:20px; text-align:match-parent;' id='" + SqlReader["ID"].ToString() + "'><b>♦ " + SqlReader["RULES_AND_REGULATION"].ToString() + "</b></li>");
        }
        cnn.Close();

        RulesandRegulation.Text = ConditionBuliding.ToString();
    }    
}