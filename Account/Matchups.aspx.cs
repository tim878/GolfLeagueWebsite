using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Matchups : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
    private int LeagueEventID;
    private int currentSeasonID;
    Dictionary<int, string> allTeams;
    //Dictionary<int, string> teamsLeftToBeSelected;
    Dictionary<int, int> SelectedMatchups;
    //Dictionary<string, Matchup> matchups;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["EventID"] != null)
        {
            LeagueEventID = int.Parse(Request.QueryString["EventID"]);
        }
        else
        {
            Response.Redirect("Matchups_EventSelection.aspx");
        }

        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        
        if (!Page.IsPostBack)
        {
            ListItem itemToAdd = new ListItem("Select First Team", "0");
            Dropdown_Team1.Items.Add(itemToAdd);
            ListItem itemToAdd2 = new ListItem("Select Second Team", "0");
            Dropdown_Team2.Items.Add(itemToAdd2);
        }

        if (ViewState["allTeams"] == null)
        {
            allTeams = DatabaseFunctions.GetTeamNames(leagueID);
            ViewState["allTeams"] = allTeams;
        }
        else
        {
            allTeams = (Dictionary<int, string>)ViewState["allTeams"];
        }

        if (!Page.IsPostBack)
        {
            foreach (int teamID in allTeams.Keys)
            {
                AddTeamToDropdowns(teamID);
            }
        }

        if (ViewState["MatchupControls"] != null)
        {
            SelectedMatchups = (Dictionary<int, int>)ViewState["MatchupControls"];
            foreach (int team1ID in SelectedMatchups.Keys)
            {
                AddMatchup(team1ID, SelectedMatchups[team1ID]);
            }
        }
        else
        {
            SelectedMatchups = new Dictionary<int, int>();
        }

    }

    private void AddTeamToDropdowns(int teamID)
    {
        ListItem itemToAdd = new ListItem(allTeams[teamID], teamID.ToString());
        Dropdown_Team1.Items.Add(itemToAdd);
        Dropdown_Team2.Items.Add(itemToAdd);
    }

    protected void AddMatchupButtonClick(object sender, CommandEventArgs e)
    {
        string team1ID = Dropdown_Team1.SelectedValue;
        string team2ID = Dropdown_Team2.SelectedValue;
        if (team1ID != "0" && team2ID != "0" && team2ID != team1ID)
        {
            SelectedMatchups.Add(int.Parse(team1ID), int.Parse(team2ID));
            AddMatchup(int.Parse(team1ID), int.Parse(team2ID));
            Dropdown_Team1.Items.Remove(Dropdown_Team1.Items.FindByValue(team1ID));
            Dropdown_Team1.Items.Remove(Dropdown_Team1.Items.FindByValue(team2ID));
            Dropdown_Team2.Items.Remove(Dropdown_Team2.Items.FindByValue(team1ID));
            Dropdown_Team2.Items.Remove(Dropdown_Team2.Items.FindByValue(team2ID));
            Dropdown_Team1.SelectedIndex = 0;
            Dropdown_Team2.SelectedIndex = 0;   
        }
        else
        {
            //popup error message
        }
    }



    protected void SaveMatchups(object sender, CommandEventArgs e)
    {
        
        foreach (int team1ID in SelectedMatchups.Keys)
        {
            DatabaseFunctions.AddMatchup(LeagueEventID, team1ID.ToString(), SelectedMatchups[team1ID].ToString());
        }
        
        //Dictionary<int, int> matchups = new Dictionary<int, int>();
        //foreach (Control c in Panel_Matchups.Controls)
        //{
        //    if (c is Button)
        //    {
        //        int team1ID, team2ID;
        //        MatchupIDtoTeamIDs(c.ID, out team1ID, out team2ID);
        //        DatabaseFunctions.AddMatchup(LeagueEventID, team1ID.ToString(), team2ID.ToString());
        //    }
        //}
        
    }


    private void AddMatchup(int team1ID, int team2ID)
    {
        string matchupID = getMatchupID(team1ID, team2ID);
        Label matchupLabel = new Label();
        matchupLabel.Text = allTeams[team1ID] + " vs. " + allTeams[team2ID] + "   ";
        matchupLabel.ID = "Label_" + matchupID;

        Button removeButton = new Button();
        removeButton.ID = matchupID;
        removeButton.Text = "Remove";
        removeButton.Command += RemoveButtonClick;

        LiteralControl breakLiteral = new LiteralControl("<br/>");
        breakLiteral.ID = "Break_" + matchupID;

        Panel_Matchups.Controls.Add(matchupLabel);
        Panel_Matchups.Controls.Add(removeButton);
        Panel_Matchups.Controls.Add(breakLiteral);
       

        ViewState["MatchupControls"] = SelectedMatchups;
    }

    private string getMatchupID(int team1ID, int team2ID)
    {
        return team1ID.ToString() + "-" + team2ID.ToString();
    }

    private void MatchupIDtoTeamIDs(string matchupID, out int team1ID, out int team2ID)
    {
        string[] teamIDs = matchupID.Split('-');
        team1ID = int.Parse(teamIDs[0]);
        team2ID = int.Parse(teamIDs[1]);
    }



    protected void RemoveButtonClick(object sender, CommandEventArgs e)
    {
        Button b = (Button)sender;
        string matchupIDToRemove = b.ID;
        int team1ID, team2ID;
        MatchupIDtoTeamIDs(matchupIDToRemove, out team1ID, out team2ID);
        List<Control> controlsToRemove = new List<Control>();
        foreach (Control control in Panel_Matchups.Controls)
        {
            if (control.ID.Contains(matchupIDToRemove))
            {
                controlsToRemove.Add(control);
            }
        }
        foreach (Control control in controlsToRemove)
        {
            Panel_Matchups.Controls.Remove(control);
        }
        AddTeamToDropdowns(team1ID);
        AddTeamToDropdowns(team2ID);
        SelectedMatchups.Remove(team1ID);
        ViewState["MatchupControls"] = SelectedMatchups;

    }

}