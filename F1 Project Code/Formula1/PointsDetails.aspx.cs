using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Formula1_PointsDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;

        string qry = "SELECT * FROM F1_MASTER_POINTS WHERE STATUS = 'Active'";
        DataTable DT = new DataTable();
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(qry, cnn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
        StringBuilder ConditionBuliding = new StringBuilder();
        da.Fill(DT);

        foreach (DataRow dtrow in DT.Rows)
        {
            ConditionBuliding.Append("<tr>");
            ConditionBuliding.Append("<td>" + dtrow["CRITERIA"].ToString() + "</td>");
            ConditionBuliding.Append("<td>" + dtrow["DESCRIPTION"].ToString() + "</td>");
            ConditionBuliding.Append("<td><div class='Same-line'>");

            for (int k = 3; k <= 14; k++)
            {
                if (dtrow[k].ToString() != "")
                {
                    ConditionBuliding.Append("<div class='multi-line'>");
                    if (k == 3) { ConditionBuliding.Append("<span>1st</span>"); }
                    if (k == 4) { ConditionBuliding.Append("<span>2nd</span>"); }
                    if (k == 5) { ConditionBuliding.Append("<span>3rd</span>"); }
                    if (k == 6) { ConditionBuliding.Append("<span>4th</span>"); }
                    if (k == 7) { ConditionBuliding.Append("<span>5th</span>"); }
                    if (k == 8) { ConditionBuliding.Append("<span>6th</span>"); }
                    if (k == 9) { ConditionBuliding.Append("<span>Won</span>"); }
                    if (k == 10) { ConditionBuliding.Append("<span>Loss</span>"); }
                    if (k == 11) { ConditionBuliding.Append("<span>Tie</span>"); }
                    if (k == 12) { ConditionBuliding.Append("<span>FL</span>"); }
                    if (k == 13) { ConditionBuliding.Append("<span>MPG</span>"); }
                    if (k == 14) { ConditionBuliding.Append("<span>PP</span>"); }
                    ConditionBuliding.Append("<input value='" + dtrow[k].ToString() + "' disabled='disabled' maxlength='2' style='font-weight: bolder; color: white; width: 40px; height: 25px; text-align: center; background-color: #272727; border:1px solid #A4A5A5;' />");
                    ConditionBuliding.Append("</div>");
                }
            }

            ConditionBuliding.Append("</div></td>");
            ConditionBuliding.Append("<td><button type='button' id='Edit" + dtrow["ID"].ToString() + "' onclick='TableEditbtn(this, " + '"' + "Save" + dtrow["ID"].ToString() + "" + '"' + ")' title='Edit' class='btn btn-outline-light'><i class='bi bi-pencil-fill'></i></button>" +
                                     "<button style='margin-left: 5px; display:none;' type='button' onclick='TableSavebtn(this)' title='Save' id='Save" + dtrow["ID"].ToString() + "' class='btn btn-outline-light'><i class='bi bi-floppy2-fill'></i></button></td>");
            ConditionBuliding.Append("</tr>");
        }

        PointsDetailsTablePlaceHolder.Controls.Add(new Literal { Text = ConditionBuliding.ToString() });
    }

    [WebMethod(EnableSession = true)]
    public static string TableSavebtnUpdatedata(List<DataList1> DataList1)
    {
        string constr = ConfigurationManager.ConnectionStrings["LogisticsaDEV"].ConnectionString;
        SqlConnection cnn = new SqlConnection(constr);
        SqlCommand cmd;
        DataTable export = new DataTable();

        try
        {
            foreach (var DataFields in DataList1)
            {
                cmd = new SqlCommand("UPDATE F1_MASTER_POINTS SET " + DataFields.COLUMN_NAME.Trim() + " = '" + DataFields.ROW_VALUE.Trim() + "', " +
                                     "UPDATED_BY = @ub, UPDATED_ON = GETDATE() WHERE ID = @id AND STATUS = 'Active'", cnn);


                cmd.Parameters.AddWithValue("@ub", DataFields.USER_NAME.Trim());
                cmd.Parameters.AddWithValue("@id", DataFields.KEY_COLUMN.Trim());

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
        catch (Exception ex)
        { return ex.Message; }

        return "Saved";
    }

    public class DataList1
    {
        public string COLUMN_NAME { get; set; }
        public string ROW_VALUE { get; set; }
        public string USER_NAME { get; set; }
        public string KEY_COLUMN { get; set; }
    }
}