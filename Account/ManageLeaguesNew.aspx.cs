using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageLeaguesNew : System.Web.UI.Page
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
       
        ImageButtonSeasons.PostBackUrl = "SeasonsNew.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonGolfers.PostBackUrl = "GolfersNew.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonTeams.PostBackUrl = "TeamsNew.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonScores.PostBackUrl = "ScoreEntry_New.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonHelp.PostBackUrl = "Help.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonSettings.PostBackUrl = "LeagueSettings.aspx?LeagueID=" + leagueID.ToString();
        ImageButtonEvents.PostBackUrl = "Events_Updated.aspx?LeagueID=" + leagueID.ToString();

        //Get Current Season
        Season currentSeasonInfo = DatabaseFunctions.GetCurrentSeasonInfo(leagueID.ToString());
        LabelSeasons.Text = currentSeasonInfo == null ? "No Seasons Created." : "Current Season: " + currentSeasonInfo.SeasonName;
        
        //Get Number of Golfers
        Dictionary<int, string> golfers = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());
        LabelGolfers.Text = golfers.Keys.Count.ToString() + " Golfers.";
   
        //Get Teams Active in Current Season
        List<Team> teamIDs = DatabaseFunctions.GetTeams(leagueID).Values.ToList();
        LabelTeams.Text = teamIDs == null ? "No Teams Created." : teamIDs.Count.ToString() + " Teams.";

        //Get Events in Current Season
        List<EventInfo> eventInfos = DatabaseFunctions.GetEvents(leagueID, currentSeasonInfo.SeasonID);
        LabelEvents.Text = eventInfos.Count == 0 ? "No Events Scheduled in Current Season." : eventInfos.Count.ToString() + " Events Scheduled.";

        //Get Number of Events with Scores
        Dictionary<int, EventInfo> eventsWithScoresPosted = DatabaseFunctions.GetEventsWithScoresPosted(leagueID, currentSeasonInfo.SeasonID);
        LabelScores.Text = eventsWithScoresPosted.Keys.Count + " Events have Scores Posted.";  

    }
}