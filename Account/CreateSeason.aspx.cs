using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class CreateSeason : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
    Dictionary<int, string> teams;
    Dictionary<int, CheckBox> selectedTeams = new Dictionary<int,CheckBox>();

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        teams = DatabaseFunctions.GetTeamNames(leagueID);
        foreach (int teamID in teams.Keys)
        {
            CheckBox cBox = new CheckBox();
            cBox.Checked = true;
            cBox.Text = teams[teamID];
            Panel_Teams.Controls.Add(cBox);
            selectedTeams.Add(teamID, cBox);
            Panel_Teams.Controls.Add(new LiteralControl("<br/>"));
        }
    }

    protected void CreateSeasonButtonPress(object sender, CommandEventArgs e)
    {
        if (TextBox_SeasonName.Text != "")
        {
            int SeasonID = DatabaseFunctions.CreateSeason(leagueID, TextBox_SeasonName.Text, Checkbox_CurrentSeason.Checked);
            foreach (int teamID in selectedTeams.Keys)
            {
                if (selectedTeams[teamID].Checked)
                {
                    DatabaseFunctions.AddSeasonAffiliation(teamID, SeasonID);
                }
            }
            Response.Write("<script language='javascript'>alert('Season Created Successfully.');</script>");
            TextBox_SeasonName.Text = "";
        }
    }
}