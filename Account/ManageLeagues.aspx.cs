using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ManageLeagues : System.Web.UI.Page
{
    private int leagueID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (((SiteMaster)this.Master).LeagueName != null)
        {
            leagueID = (int)((SiteMaster)this.Master).LeagueID;
        }
        else
        {
            //error?  
        }
        Link_AddScoresNew.NavigateUrl = "ScoreEntry_New.aspx?LeagueID=" + leagueID.ToString();
        Link_AddEvent.NavigateUrl = "Events.aspx?LeagueID=" + leagueID.ToString();
        Link_AddEventNew.NavigateUrl = "Events_Updated.aspx?LeagueID=" + leagueID.ToString();
        Link_AddPlayers.NavigateUrl = "Golfers.aspx?LeagueID=" + leagueID.ToString();
        Link_AddScores.NavigateUrl = "ScoreEntry.aspx?LeagueID=" + leagueID.ToString();
        Link_AddTeam.NavigateUrl = "Teams.aspx";
        Link_CreateNewSeason.NavigateUrl = "CreateSeason.aspx";
        //Link_CreateNewLeague.NavigateUrl = "AddLeague.aspx?LeagueID=" + leagueID.ToString();
        //Link_SelectMatchups.NavigateUrl = "Matchups.aspx?LeagueID=" + leagueID.ToString();
        Link_Handicaps.NavigateUrl = "HandicapSettings.aspx";
        Link_GenerateSchedule.NavigateUrl = "GenerateSchedule.aspx";
        Link_TestPage.NavigateUrl = "TestPage.aspx";
        Link_ManageLeaguesNew.NavigateUrl = "ManageLeaguesNew.aspx?LeagueID=" + leagueID.ToString();

    }
}