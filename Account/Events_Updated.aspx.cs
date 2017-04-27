using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

public partial class Events_Updated : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
    private int currentSeasonID;
    Dictionary<int, string> Courses;
    Dictionary<int, int> SelectedMatchups = new Dictionary<int,int>();
    Dictionary<int, string> Teams;
   
   
    protected void Page_Load(object sender, EventArgs e)
    {
        currentUserID = ((SiteMaster)this.Master).currentUserID;
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        Teams = DatabaseFunctions.GetTeamNames(leagueID);
        
        if (!Page.IsPostBack)
        {
            GetAndBindCurrentlyScheduleEvents(currentSeasonID);
            PopulateCourses();
        }      
    }

    private void GetAndBindCurrentlyScheduleEvents(int currentSeasonID)
    {             
        List<EventInfo> events = DatabaseFunctions.GetEvents(leagueID, currentSeasonID);
        Dictionary<int, EventInfo> eventsWithScores =  DatabaseFunctions.GetEventsWithScores(leagueID);

        for (int i = 0; i < events.Count; i++)
        {
            if (eventsWithScores.ContainsKey(events[i].EventID))
            {
                events[i].HasScores = true;
            }
            else
            {
                events[i].HasScores = false;
            }
        }
       
        events = events.OrderBy(x => x.EventID).ToList();

        grdScheduledEvents.DataSource = events;
        grdScheduledEvents.DataBind();
    }

    private void PopulateCourses()
    {
        //populate courses dropdown
        Courses = DatabaseFunctions.GetAllCourses();

        ListItem itemToAdd = new ListItem("Select Course", "0");
        Dropdown_Course.Items.Add(itemToAdd);
        foreach (int courseID in Courses.Keys)
        {
            ListItem newItem = new ListItem(Courses[courseID], courseID.ToString());
            Dropdown_Course.Items.Add(newItem);
        }
    }

    
    protected void ViewMatchupButtonClick(object sender, CommandEventArgs e)
    {
        string eventID = (string)e.CommandArgument;
        Dictionary<int, int> matchupIds  = DatabaseFunctions.GetMatchups(int.Parse(eventID));
        //int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);
        List<MatchupView> matchups = new List<MatchupView>();
        
        foreach (int team1ID in matchupIds.Keys)
        {
            matchups.Add(new MatchupView(teamNames[team1ID], teamNames[matchupIds[team1ID]]));
        }

        GrdViewMatchups.DataSource = matchups;
        GrdViewMatchups.DataBind();
        
        ModalPopupExtender1.Show();
    }

    //Used for binding to Gridview
    protected class MatchupView
    {
        public string Team1 {get;set;}
        public string Team2 {get; set;}

        public MatchupView(string team1, string team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
    }

    protected void AddEventButtonClick(object sender, CommandEventArgs e)
    {
        //int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        ModalPopupExtender2.Show();
        Button_Save.Text = "Add Event";
        Button_Save.CommandName = "Add";
        Session["EventsPageCurrentAction"] = "Add";
    }

    protected void Dropdown_NumberOfMatchups_SelectedIndexChanged(object sender, EventArgs e)
    {
        int numberOfMatchups = int.Parse(DropDownNumberOfMatchups.SelectedValue);
        List<EventInfo> events = DatabaseFunctions.GetEvents(leagueID, DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString()));
        Dictionary<int, int> matchupsPrevEvent = new Dictionary<int, int>();
        if (events != null && events.Count != 0)
        {
            EventInfo currentSeasonEvent = events[0];
            matchupsPrevEvent = DatabaseFunctions.GetMatchups(currentSeasonEvent.EventID);
        }
   
        Dictionary<int, Team> teams = DatabaseFunctions.GetTeams(leagueID);
        if((teams.Count * .5) < numberOfMatchups)
        {
            numberOfMatchups = (int)Math.Ceiling((decimal)(teams.Count / 2));  
        }

        List<int> teamIDsPrevEvent = matchupsPrevEvent.Keys.ToList();
        List<int> teamIDsRemainingTeams = teams.Keys.ToList();
        
        //loop once and add matchups
        for (int i = 0; i < numberOfMatchups; i++)
        {
            if (teamIDsPrevEvent.Count > 1)
            {
                int team1ID = teamIDsPrevEvent[0];
                int team2ID = teamIDsPrevEvent[1];
                AddMatchup(team1ID, team2ID, teams[team1ID].TeamName, teams[team2ID].TeamName);
                teamIDsRemainingTeams.Remove(team1ID);
                teamIDsRemainingTeams.Remove(team2ID);
                teamIDsPrevEvent.RemoveAt(0);
                teamIDsPrevEvent.RemoveAt(0);
            }
            else//use remaining teams
            {
                int team1ID = teamIDsRemainingTeams[0];
                int team2ID = teamIDsRemainingTeams.Count > 1 ? teamIDsRemainingTeams[1] : 0;
                AddMatchup(team1ID, team2ID, teams[team1ID].TeamName, teams[team2ID].TeamName);
                teamIDsRemainingTeams.RemoveAt(0);
                if (team2ID != 0)
                {
                    teamIDsRemainingTeams.RemoveAt(0);
                }
            }
        }


        //loop again and add the remaining teams into the available teams list.
        for (int i = 0; i < teamIDsRemainingTeams.Count; i++)
        {
            if (!teams[teamIDsRemainingTeams[i]].hidden)
            {
                AddToRemainingTeams(teamIDsRemainingTeams[i], teams[teamIDsRemainingTeams[i]].TeamName);
            }
        }

        RestoreSaveButtonState();
        ModalPopupExtender2.Show();

       
    }

    private void RestoreSaveButtonState()
    {
        string currentAction = (string)Session["EventsPageCurrentAction"];
        if (currentAction == "Edit")
        {
            Button_Save.Text = "Save Edits";
            Button_Save.CommandName = "Edit";
            Button_Save.CommandArgument = (string)Session["EventIDBeingEdited"];
        }
        else if (currentAction == "Add")
        {
            Button_Save.Text = "Add Event";
            Button_Save.CommandName = "Add";
        }
       
    }

    protected void EditEventButtonClick(object sender, CommandEventArgs e)
    {
        Session["EventsPageCurrentAction"] = "Edit";
        string eventID = (string)e.CommandArgument;
        Session["EventIDBeingEdited"] = eventID;
        populateEventDetailsInModal(int.Parse(eventID));
        Button_Save.Text = "Save Edits";
        Button_Save.CommandName = "Edit";
        Button_Save.CommandArgument = eventID;
        ModalPopupExtender2.Show();
    }

    protected void DeleteEventButtonClick(object sender, CommandEventArgs e)
    {    
        int eventID = int.Parse((string)e.CommandArgument);

        Dictionary<int, EventInfo> eventsWithScores = DatabaseFunctions.GetEventsWithScores(leagueID);
        if(!eventsWithScores.ContainsKey(eventID))
        {
            DatabaseFunctions.DeleteEvent(eventID);
            GetAndBindCurrentlyScheduleEvents(currentSeasonID);
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Event Was Not Deleted Because it has Scores Posted.  Delete Scores first then Event Can be Deleted.');</script>");
        }
    }

    protected void SaveLeagueEventButtonPress(object sender, CommandEventArgs e)
    {
        if (ValidateInput())
        {
            string commandName = ((Button)sender).CommandName.ToString();

            int courseID = int.Parse(Dropdown_Course.SelectedValue);
            int seasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
            string EventDateToParse = datepicker.Text;
            string EventName = TextBox_EventName.Text;
            DateTime EventDate = DateTime.Parse(EventDateToParse);

            string team1IDsCommaSeperated = TeamOneListValues.Value;
            string team2IDsCommaSeperated = TeamTwoListValues.Value;

            //take out the last comma
            team1IDsCommaSeperated = team1IDsCommaSeperated.TrimEnd(',');
            team2IDsCommaSeperated = team2IDsCommaSeperated.TrimEnd(',');

            string[] team1IDsSplit = team1IDsCommaSeperated.Split(',');
            string[] team2IDsSplit = team2IDsCommaSeperated.Split(',');

            if(team1IDsSplit.Length != team2IDsSplit.Length)
            {
                Response.Write("<script language='javascript'>alert('Problem Adding/Editing Event.  Matchups entered incorrectly.(Every team must play another team or have a bye.)');</script>");
                ModalPopupExtender2.Show(); 
                return;
            }

            int eventID = 0;

            if (commandName == "Edit")
            {
                eventID = int.Parse((string)e.CommandArgument);
                DatabaseFunctions.EditEvent(eventID, EventDateToParse, EventName, courseID);
                //Response.Write("<script language='javascript'>alert('Event Edited Successfully.');</script>")
                DatabaseFunctions.DeleteMatchups(eventID);
            }
            else if (commandName == "Add")
            {
                //AddLeagueEvent(int LeagueID, int SeasonID, DateTime? date, int? CourseID, int? CourseIDSecondNine, int? Tees,  string notes, int EventNumber, string EventName)
                eventID = DatabaseFunctions.AddLeagueEvent(leagueID, seasonID, EventDate, courseID, null, null, null, EventName);      
            }

            for (int i = 0; i < team1IDsSplit.Length; i++)
            {
                DatabaseFunctions.AddMatchup(eventID, team1IDsSplit[i], team2IDsSplit[i]);
            }  
       
            GetAndBindCurrentlyScheduleEvents(currentSeasonID);
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Problem Adding/Editing Event.  Check that all information is filled out.');</script>");
            ModalPopupExtender2.Show();           
        }
    }

    private void populateEventDetailsInModal(int EventID)
    {
         //int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
         List<EventInfo> events = DatabaseFunctions.GetEvents(leagueID, currentSeasonID);
        
         EventInfo eventInfo = events.Where(x => x.EventID == EventID).ToList().First();

         Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(eventInfo.EventID);
         Dictionary<int, Team> teams = DatabaseFunctions.GetTeams(leagueID);
      
         foreach (int team1ID in matchups.Keys)
         {
             AddMatchup(team1ID, matchups[team1ID], teams[team1ID].TeamName, teams[matchups[team1ID]].TeamName);
             teams.Remove(team1ID);
             teams.Remove(matchups[team1ID]);
         }

         if (!eventInfo.HasScores)
         {
             foreach(Team team in teams.Values)
             {
                 if(!team.hidden)
                 {
                     AddToRemainingTeams(team.TeamID, team.TeamName);
                 }
             }      
         }

         TextBox_EventName.Text = eventInfo.EventName;
         datepicker.Text = eventInfo.Date;
         Dropdown_Course.SelectedValue = eventInfo.CourseID.ToString();
    }

    

    private void AddMatchup(int team1ID, int team2ID, string team1Name, string team2Name)
    {
        HtmlGenericControl liTeamOne = new HtmlGenericControl("li");
        liTeamOne.Attributes.Add("teamID", team1ID.ToString());
        liTeamOne.Attributes.Add("class", "ui-state-default");
        liTeamOne.InnerText = team1Name;
        TeamOneList.Controls.Add(liTeamOne);

        HtmlGenericControl liTeamTwo = new HtmlGenericControl("li");
        liTeamTwo.Attributes.Add("teamID", team2ID.ToString());
        liTeamTwo.Attributes.Add("class", "ui-state-default");
        liTeamTwo.InnerText = team2ID == 0 ? "Bye" : team2Name;
        TeamTwoList.Controls.Add(liTeamTwo);


        HtmlGenericControl liVersus = new HtmlGenericControl("li");
        liVersus.Attributes.Add("class", "ui-state-highlight");
        liVersus.InnerText = "vs.";
        vsList.Controls.Add(liVersus);
    }

    private void AddToRemainingTeams(int teamID, string teamName)
    {
        HtmlGenericControl liTeamOne = new HtmlGenericControl("li");
        liTeamOne.Attributes.Add("teamID", teamID.ToString());
        liTeamOne.Attributes.Add("class", "ui-state-default");
        liTeamOne.InnerText = teamName;
        AvailableTeamsList.Controls.Add(liTeamOne);
    }

   
    

    private bool ValidateInput()
    {
        if (TextBox_EventName.Text == "" && Dropdown_Course.SelectedValue != "0")
        {
            return false;
        }
      
        return true;
    }
    
}