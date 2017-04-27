using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class SeasonsNew : System.Web.UI.Page
{
    private int leagueID;
    //private int currentSeasonID;
    //EventsAndGolfers eventsAndGolfers;
    private int RowCount { get; set; }
    List<Season> seasons;

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (((SiteMaster)this.Master).LeagueName != null)
        {
            this.leagueID = leagueID = (int)((SiteMaster)this.Master).LeagueID;
        }
        else
        {
            //error?
            Response.Redirect("NoLeague.aspx");
        }

        seasons = DatabaseFunctions.GetSeasons(leagueID).OrderBy(x => x.SeasonID).ToList();
               
        if (!Page.IsPostBack)
        {
            BindDataToGrid();
            LoadSeasonsDropdown();
        }

    }

    private void LoadSeasonsDropdown()
    {
        DropdownCurrentSeason.Items.Clear();
        ListItem initialItem = new ListItem("Change Current Season", "0");
        DropdownCurrentSeason.Items.Add(initialItem);
        foreach (Season season in seasons)
        {
            ListItem newItem = new ListItem(season.SeasonName, season.SeasonID.ToString());
            DropdownCurrentSeason.Items.Add(newItem);
        }
        DropdownCurrentSeason.SelectedIndex = 0;
    }

    private void BindDataToGrid()
    {  
        foreach(Season season in seasons)
        {
            List<EventInfo> eventInfos = DatabaseFunctions.GetEvents(leagueID, season.SeasonID);
            season.NumberOfEventsScheduled = eventInfos.Count();
        }

        grdSeasons.DataSource = seasons;
        grdSeasons.DataBind();
    }

    

   
    protected void Save(object sender, CommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(TextBoxSeasonName.Text))
        {
            int seasonId = int.Parse((string)e.CommandArgument);
            if (seasonId == 0)//add
            {
                DatabaseFunctions.AddSeason(leagueID, TextBoxSeasonName.Text);
            }
            else//edit
            {
                DatabaseFunctions.UpdateSeasonName(seasonId, TextBoxSeasonName.Text);
            }
            seasons = DatabaseFunctions.GetSeasons(leagueID).OrderBy(x => x.SeasonID).ToList();
            BindDataToGrid();
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Season Name Cannot be blank');</script>");
            ModalPopupExtender1.Show();//Keep Score Entry Modal Open.
        }
        
    }


    protected void GenerateEventsButtonClick(object sender, CommandEventArgs e)
    {
        int seasonID = int.Parse((string)e.CommandArgument);
        Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);
        List<int> activeTeams = DatabaseFunctions.GetTeamsActiveInSeason(leagueID, seasonID);

        foreach (int teamID in activeTeams)
        {
            AddTeam(teamID, teamNames[teamID], true);
            teamNames.Remove(teamID);
        }

        foreach (int teamID in teamNames.Keys)
        {
            AddTeam(teamID, teamNames[teamID], false);
        }

        btnAddWeeks.CommandArgument = seasonID.ToString();

        ModalPopupExtender2.Show();
    }
    
    
    protected void EditButtonClick(object sender, CommandEventArgs e)
    {
        List<Season> seasons = DatabaseFunctions.GetSeasons(leagueID);
        int seasonID = int.Parse((string)e.CommandArgument);

        TextBoxSeasonName.Text = seasons.SingleOrDefault(x => x.SeasonID == seasonID).SeasonName;

        btnSave.CommandArgument = seasonID.ToString();
        ModalPopupExtender1.Show();
    }

    protected void AddButtonClick(object sender, CommandEventArgs e)
    {
        TextBoxSeasonName.Text = "";  
        btnSave.CommandArgument = "0";
        ModalPopupExtender1.Show();
    }

    protected void GenerateEvents(object sender, CommandEventArgs e)
    {
        int seasonID = int.Parse((string)e.CommandArgument);

        int NumEvents = 0;
        //get number of events
        try
        {
            NumEvents = int.Parse(TextboxNumberOfWeeksToAdd.Text);
        }
        catch
        {
            Response.Write("<script language='javascript'>alert('Must Enter Number of Events to Create.');</script>");
            ModalPopupExtender2.Show();//Keep Generate Events Modal Open.
            return;
        }

        string teamIDsCommaSeperated = TeamsToUseValues.Value;

        //take out the last comma
        teamIDsCommaSeperated = teamIDsCommaSeperated.TrimEnd(',');
        string[] teamIDsSplit = teamIDsCommaSeperated.Split(',');
        List<int> teamIDs = new List<int>();
        foreach (string teamID in teamIDsSplit)
        {
            teamIDs.Add(int.Parse(teamID));
        }
        
        Dictionary<int, List<GolfLeagueWebsiteGlobals.Matchup>> weeklyMatchups = new Dictionary<int, List<GolfLeagueWebsiteGlobals.Matchup>>();
        List<string> allMatchups = new List<string>();
      
        //add a bye in if there is an odd number of teams
        if (teamIDs.Count % 2 == 1)
        {
            teamIDs.Add(-1);
        }

        List<int> teamIDGroupOne = new List<int>();
        List<int> teamIDGroupTwo = new List<int>();

        teamIDGroupOne.AddRange(teamIDs.GetRange(teamIDs.Count / 2, teamIDs.Count / 2));
        teamIDGroupTwo.AddRange(teamIDs.GetRange(0, teamIDs.Count / 2));

        int startingWeekIndex = DatabaseFunctions.GetEvents(leagueID, seasonID).Count;

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
            weeklyMatchups.Add(i + startingWeekIndex + 1, currentWeekMatchups);
            RotateLists(teamIDGroupOne, teamIDGroupTwo);
        }

        DatabaseFunctions.SaveLeagueSchedule(weeklyMatchups, leagueID, seasonID);
    }

    private void AddTeam(int team1ID, string teamName, bool inCurrentSeason)
    {
        HtmlGenericControl liTeamOne = new HtmlGenericControl("li");
        liTeamOne.Attributes.Add("teamID", team1ID.ToString());
        liTeamOne.Attributes.Add("class", "ui-state-default");
        liTeamOne.InnerText = teamName;
        if (inCurrentSeason)
        {
            TeamsToUseList.Controls.Add(liTeamOne);
        }
        else
        {
            AvailableTeamsList.Controls.Add(liTeamOne);
        }
    }

    protected void DropdownCurrentSeasonsSelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedSeasonID = int.Parse(DropdownCurrentSeason.SelectedValue);
        DatabaseFunctions.UpdateCurrentSeason(leagueID, selectedSeasonID);
        seasons = DatabaseFunctions.GetSeasons(leagueID).OrderBy(x => x.SeasonID).ToList();
        BindDataToGrid();
    }

    

    private void RotateLists(List<int> List1, List<int> List2)
    {
        List1.Insert(1, List2[0]);
        List2.Add(List1[List1.Count - 1]);
        List1.RemoveAt(List1.Count - 1);
        List2.RemoveAt(0);
    }


}