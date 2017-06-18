using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NoLeague : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Dictionary<int, string> leagues = DatabaseFunctions.GetLeagues();
            GolfLeagueWebsiteGlobals.PopulateDropdownList(leagues, DropdownLeagues, leagues.FirstOrDefault().Key);
        }
    }

    protected void DropdownLeagueSelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLeagueID = DropdownLeagues.SelectedValue;
        Session["LeagueID"] = int.Parse(selectedLeagueID);
        Session["LeagueName"] = DropdownLeagues.SelectedItem.Text;
        Response.Redirect("LeagueHome.aspx", true);
    }
}