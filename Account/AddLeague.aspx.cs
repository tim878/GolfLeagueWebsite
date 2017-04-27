using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class AddLeague : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUserID = ((SiteMaster)this.Master).currentUserID;  
    }

    protected void CreateLeague(object sender, CommandEventArgs e)
    {
        if (TextBox_LeagueName.Text != "")
        {
            DatabaseFunctions.addLeague(currentUserID, TextBox_LeagueName.Text);
            Response.Redirect("ManageLeagues.aspx");
        }
    }
}