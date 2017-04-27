using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GenerateSchedule : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
    }

    protected void CreateSchedule(object sender, CommandEventArgs e)
    {
        int NumEvents = 0;
        //get number of events
        try
        {
            NumEvents = int.Parse(TextBox_NumEvents.Text);
        }
        catch
        {
            //todo: show error pop up
            return;
        }

        //get a list of teams (in random order)
        Dictionary<int,string> teamInfo = DatabaseFunctions.GetTeamNames(leagueID);

        Dictionary<int, List<GolfLeagueWebsiteGlobals.Matchup>> weeklyMatchups = new Dictionary<int, List<GolfLeagueWebsiteGlobals.Matchup>>();

        List<string> allMatchups = new List<string>();

        List<int> teamIDs = new List<int>(teamInfo.Keys);
        //add a bye in if there is an odd number of teams
        if(teamIDs.Count % 2 == 1)
        {
            teamIDs.Add(-1);
        }

        List<int> teamIDGroupOne = new List<int>();
        List<int> teamIDGroupTwo = new List<int>();

        
        teamIDGroupOne.AddRange(teamIDs.GetRange(teamIDs.Count/2, teamIDs.Count/2));
        teamIDGroupTwo.AddRange(teamIDs.GetRange(0, teamIDs.Count/2));

        //foreach event
        for (int i = 0; i < NumEvents; i++)
        {
            List<GolfLeagueWebsiteGlobals.Matchup> currentWeekMatchups = new List<GolfLeagueWebsiteGlobals.Matchup>();
            for (int j = 0; j < teamIDGroupOne.Count; j++)
            {
                GolfLeagueWebsiteGlobals.Matchup matchup = new GolfLeagueWebsiteGlobals.Matchup();
                matchup.Team1ID = teamIDGroupOne[j];
                matchup.Team2ID = teamIDGroupTwo[j];
                currentWeekMatchups.Add(matchup);
            }
            weeklyMatchups.Add(i+1, currentWeekMatchups);
            RotateLists(teamIDGroupOne, teamIDGroupTwo);
        }

        int SeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        DatabaseFunctions.SaveLeagueSchedule(weeklyMatchups, leagueID, SeasonID);
    }

    private void RotateLists(List<int> List1, List<int> List2)
    {
        List1.Insert(1, List2[0]);
        List2.Add(List1[List1.Count - 1]);
        List1.RemoveAt(List1.Count - 1);
        List2.RemoveAt(0);
    }
   


}