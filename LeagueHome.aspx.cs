using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class LeagueHome : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;

    private class nextEventGridData
    {
        public string Matchup { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());

        AddScrollBarContent();
        AddNextEventContent();

        //ScrollLabel1.Text = "Team Scores for " + lastEventWithScores.EventName + " (" + lastEventWithScores.Date + ") "; 
    }

    private void AddNextEventContent()
    {
        List<nextEventGridData> gridData = new List<nextEventGridData>();
        EventInfo eventInfo = DatabaseFunctions.GetNextEvent(leagueID.ToString());
        Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(eventInfo.EventID);
        Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);

        LabelNextEventTitle1.Text = eventInfo.EventName;
        LabelNextEventTitle2.Text = "Date: " + eventInfo.Date;
        LabelNextEventTitle3.Text = "Course:" + eventInfo.CourseName;

        foreach(int team1ID in matchups.Keys)
        {
            string matchupText = teamNames[team1ID] + " vs. " + teamNames[matchups[team1ID]];
            MatchupsList.Controls.Add(AddListElement(matchupText));
        }
        //grdNextEventMatchups.DataBind();
    }

    private void AddScrollBarContent()
    {
        EventInfo lastEventWithScores = DatabaseFunctions.GetMostRecentEventWithScoresPosted(leagueID.ToString());
        if(lastEventWithScores.EventID == 0)
        {
            return;
        }
        Scoring.EventStats eventStats = Scoring.GetEventResults(leagueID, lastEventWithScores.EventID, null);
        Dictionary<int, Golfer> golfers = DatabaseFunctions.GetGolfersInfo(leagueID.ToString());
        Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);

        ScrollLabel1.Text += "Last Event: " + lastEventWithScores.EventName + " (" + lastEventWithScores.Date + "):   ";

        var items = from pair in eventStats.grossScores
                    orderby pair.Value ascending
                    select pair;

        ScrollLabel1.Text += "      Scores: ";
        foreach (KeyValuePair<int, int> pair in items)
        {
            ScrollLabel1.Text += golfers[pair.Key].firstName + " " +golfers[pair.Key].lastName + " - " + pair.Value.ToString() + "   |"; 
            //AddTableRowLeaderboard(Table_GrossScoreLeaderboard, DatabaseFunctions.GetGolferName(pair.Key), (decimal)pair.Value);
        }

        items = from pair in eventStats.netScores
                orderby pair.Value ascending
                select pair;

        ScrollLabel1.Text += "|^|Net Scores:  ";
        foreach (KeyValuePair<int, int> pair in items)
        {
            ScrollLabel1.Text += golfers[pair.Key].firstName + " " +golfers[pair.Key].lastName + " - " + pair.Value.ToString() + "   |"; 
            //AddTableRowLeaderboard(Table_NetScoreLeaderboard, DatabaseFunctions.GetGolferName(pair.Key), (decimal)pair.Value);
        }

        var items2 = from pair in eventStats.teamPts
                     orderby pair.Value descending
                     select pair;

        ScrollLabel1.Text += "|^|Team Scores:   ";

        foreach (KeyValuePair<int, decimal> pair in items2)
        {
            ScrollLabel1.Text += teams[pair.Key] + " " + pair.Value.ToString() + "   |"; 
            //AddTableRowLeaderboard(Table_TeamPts, teamNames[pair.Key], pair.Value);
        }
    }

    private HtmlGenericControl AddListElement(string text)
    {
        HtmlGenericControl retVal = new HtmlGenericControl("li");
        retVal.Attributes.Add("class", "list-group-item");
        retVal.InnerText = text;
        return retVal;
    }
       
}