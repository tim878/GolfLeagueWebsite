using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Matchups_EventSelection : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;

    protected void Page_Load(object sender, EventArgs e)
    {

        currentUserID = ((SiteMaster)this.Master).currentUserID;
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        if (!Page.IsPostBack)
        {
            EventsAndGolfers ev = DatabaseFunctions.GetEventsAndPlayers(leagueID);
            foreach (int eventID in ev.events.Keys)
            {
                ListItem itemToAdd = new ListItem(ev.events[eventID].EventName, eventID.ToString());
                Dropdown_Events.Items.Add(itemToAdd);
            }
        }
    }

    protected void EnterMatchupsButtonClick(object sender, CommandEventArgs e)
    {
        string SelectedEventID = Dropdown_Events.SelectedValue;
        Response.Redirect("Matchups.aspx?EventID=" + SelectedEventID);
    }
}