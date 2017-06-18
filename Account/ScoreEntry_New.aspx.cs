using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ScoreEntry_New : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;
    EventsAndGolfers eventsAndGolfers;
    private int RowCount { get; set; }

    //Used for binding to Gridview
    protected class ScoresView
    {
        public string Golfer { get; set; }
        public string Sub { get; set; }
        public string ScoreHole1 { get; set; } 
        public string ScoreHole2 { get; set; }
        public string ScoreHole3 { get; set; }
        public string ScoreHole4 { get; set; }
        public string ScoreHole5 { get; set; }
        public string ScoreHole6 { get; set; }
        public string ScoreHole7 { get; set; }
        public string ScoreHole8 { get; set; }
        public string ScoreHole9 { get; set; }
        public string TotalScore { get; set; }
        public string GolferID   { get; set; }


        public ScoresView(string golferName, string sub, List<byte> scores, int golferId)
        {
            Golfer = golferName;
            GolferID = golferId.ToString();
            Sub = sub == null ? "" : sub;
            if (scores != null)
            {
                ScoreHole1 = scores[0].ToString();
                ScoreHole2 = scores[1].ToString();
                ScoreHole3 = scores[2].ToString();
                ScoreHole4 = scores[3].ToString();
                ScoreHole5 = scores[4].ToString();
                ScoreHole6 = scores[5].ToString();
                ScoreHole7 = scores[6].ToString();
                ScoreHole8 = scores[7].ToString();
                ScoreHole9 = scores[8].ToString();
                int totalScore = 0;
                foreach (byte score in scores)
                {
                    totalScore += score;
                }
                TotalScore = totalScore.ToString();
            }
            else
            {
                ScoreHole1 = "";
                ScoreHole2 = "";
                ScoreHole3 = "";
                ScoreHole4 = "";
                ScoreHole5 = "";
                ScoreHole6 = "";
                ScoreHole7 = "";
                ScoreHole8 = "";
                ScoreHole9 = "";
                TotalScore = "";
            }
        }
    }

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

        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        eventsAndGolfers = DatabaseFunctions.GetEventsAndPlayers(leagueID, currentSeasonID);
        //Dictionary<int, string> courses = DatabaseFunctions.GetAllCourses();
        //Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID, currentSeasonID);
       
        
        if (!Page.IsPostBack)
        {
            ListItem initialItem = new ListItem("Select Event to Continue", "0");
            Dropdown_LeagueEvent.Items.Add(initialItem);
            //TODO only add events in current season?

            foreach (KeyValuePair<int, EventInfo> eventPair  in eventsAndGolfers.events.OrderBy(x => x.Value.EventID))
            {
                int eventID = eventPair.Key;
                ListItem eventItem = new ListItem(eventsAndGolfers.events[eventID].EventName, eventID.ToString());
                Dropdown_LeagueEvent.Items.Add(eventItem);
            }
            Dropdown_LeagueEvent.SelectedIndex = 0;   
        }

    }

    protected void Dropdown_LeagueEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            ViewState["EventID"] = selectedEventID;           
            if (!string.IsNullOrEmpty(eventsAndGolfers.events[selectedEventID].Date) && eventsAndGolfers.events[selectedEventID].CourseID != 0)
            {
                BindDataToGridview(selectedEventID);               
            }
            else
            {
                //Response error message
            }
             
        }
    }

    private void BindDataToGridview(int selectedEventID)
    {
        List<ScoresView> dataToBind = new List<ScoresView>();
        Dictionary<int, string> golferNames = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());
        Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
        Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);
        List<int> golferIDs = DatabaseFunctions.GetGolfersByEvent(selectedEventID);
      

        foreach (int golferID in golferIDs)
        {
            try
            {
                string sub = subs.ContainsKey(golferID) ? golferNames[subs[golferID]] : "";
                List<byte> scoresToUse;

                if (sub == "")
                {
                    scoresToUse = scores.ContainsKey(golferID) ? scores[golferID] : null;
                }
                else
                {
                    scoresToUse = scores[subs[golferID]];
                }
                ScoresView scoresView = new ScoresView(golferNames[golferID], sub, scoresToUse, golferID);
                dataToBind.Add(scoresView);
            }
            catch { }
        }

        grdScores.DataSource = dataToBind;
        grdScores.DataBind();
    }

   
    protected void Save(object sender, CommandEventArgs e)
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        int scheduledGolferID = int.Parse((string)e.CommandArgument);
        List<byte> scores = GetDelimitedScores(TextBoxScores.Text);

        if (scores != null && scores.Count == 9)
        {
            int golferID = scheduledGolferID;
            //Delete any previous subs and rounds for the scheduled golfer
            Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
            if (subs.ContainsKey(scheduledGolferID))
            {
                DatabaseFunctions.DeleteSub(selectedEventID, subs[scheduledGolferID]);
            }
            else
            {
                DatabaseFunctions.DeleteScore(selectedEventID, golferID);
            }

            //Update Sub
            if (DropdownSub.SelectedValue != "0")
            {
                golferID = int.Parse(DropdownSub.SelectedValue);
                //Add this sub
                DatabaseFunctions.AddSubs(selectedEventID, golferID, scheduledGolferID);
            }
            //Update sub or originally scheduled golfers score
            DatabaseFunctions.AddScore(golferID, selectedEventID, scores);

            //Add handicap override if necessary
            if(TextBoxHandicapOverride.Text.Length > 0)
            {
                AddHandicapOverrides(golferID, selectedEventID, TextBoxHandicapOverride.Text);
            }

            BindDataToGridview(selectedEventID);
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Scores not saved.  Verify that scores are entered correctly.(Must enter a score for exactly nine holes)');</script>");
            ModalPopupExtender1.Show();//Keep Score Entry Modal Open.
        }
        
    }    

    private void AddHandicapOverrides(int golferID, int EventID, string handicap)
    {
        int handicapOverride;
        if (int.TryParse(handicap, out handicapOverride))
        {
            DatabaseFunctions.InsertHandicapOverride(golferID, EventID, handicapOverride);
        }
       
    }

    

    private List<byte> GetDelimitedScores(string DelimitedText)
    {
        try
        {
            DelimitedText = DelimitedText.Trim();
            String[] scores = DelimitedText.Split(new char[] { ',', ' ', '\t' });
            List<byte> scoreList = new List<byte>();
            foreach (string score in scores)
            {
                scoreList.Add(byte.Parse(score));
            }
            if (scoreList.Count != 9)
            {
                return null;
            }
            return scoreList;
        }
        catch
        {
            return null;
        }
    }

    protected void DeleteButtonClick(object sender, CommandEventArgs e)
    {
        int scheduledGolferID = int.Parse((string)e.CommandArgument);
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
        
        int golferID = scheduledGolferID;
        if(subs.ContainsKey(scheduledGolferID))
        {
            golferID = subs[scheduledGolferID];
            DatabaseFunctions.DeleteSub(selectedEventID, golferID);
        }

        DatabaseFunctions.DeleteScore(selectedEventID, golferID);
        BindDataToGridview(selectedEventID);
    }

    protected void EditButtonClick(object sender, CommandEventArgs e)
    {
        int scheduledGolferID = int.Parse((string)e.CommandArgument);
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);

        //Dictionary<int, string> golferNames = DatabaseFunctions.GetAllGolfers(leagueID);
        Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
        Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);

        int golferID;

        if (subs.ContainsKey(scheduledGolferID))
        {
            golferID = subs[scheduledGolferID];
            DropdownSub.SelectedValue = golferID.ToString();
        }
        else
        {
            golferID = scheduledGolferID;
            DropdownSub.SelectedValue = "0";
        }

        AddSubsToDropdown(DropdownSub, selectedEventID);
        lbGolferToEdit.Text = eventsAndGolfers.golfers[scheduledGolferID];
        if (scores.ContainsKey(golferID))
        {
            TextBoxScores.Text = delimitScores(scores[golferID]);
        }
        else
        {
            TextBoxScores.Text = "";
        }
        btnSave.CommandArgument = scheduledGolferID.ToString();

        ModalPopupExtender1.Show();
    }

    private string delimitScores(List<byte> scores)
    {
        string retVal = "";
        foreach (byte score in scores)
        {
            retVal += score.ToString() + " "; 
        }
        return retVal;
    }

    private void AddSubsToDropdown(DropDownList dropdown, int selectedEventID)
    {
        Dictionary<int, string> allGolfers = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());//get all players
        List<int> scheduledGolfers = DatabaseFunctions.GetGolfersByEvent(selectedEventID);
        Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
        foreach (int golferID in scheduledGolfers)
        {
            if (!subs.ContainsKey(golferID))
            {
                allGolfers.Remove(golferID);
            }
        }
        foreach (int golferID in subs.Values)
        {
            allGolfers.Remove(golferID);
        }
       

        ListItem selectionItem = new ListItem("None", "0");
        dropdown.Items.Add(selectionItem);
        foreach (int golferID in allGolfers.Keys)
        {
            ListItem playerItem = new ListItem(allGolfers[golferID], golferID.ToString());
            dropdown.Items.Add(playerItem);
        }
    }



    protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                {
                    RowCount = 0;
                }
                break;
            case DataControlRowType.DataRow:
                {
                    RowCount += 1;
                    if (RowCount == 4)
                    {
                        e.Row.CssClass = "BorderRow";
                        RowCount = 0;
                    }
                }
                break;
        }
    }

     
    
}