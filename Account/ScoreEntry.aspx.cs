using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ScoreEntry : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;
    List<TextBox> Player1ScoreTextBoxes;
    List<TextBox> Player2ScoreTextBoxes;
    List<TextBox> Player3ScoreTextBoxes;
    List<TextBox> Player4ScoreTextBoxes;

    protected void Page_Load(object sender, EventArgs e)
    {
        Player1ScoreTextBoxes = new List<TextBox>() { TextBox_P1_Hole1, TextBox_P1_Hole2, TextBox_P1_Hole3, TextBox_P1_Hole4, TextBox_P1_Hole5, TextBox_P1_Hole6, TextBox_P1_Hole7, TextBox_P1_Hole8, TextBox_P1_Hole9 };
        Player2ScoreTextBoxes = new List<TextBox>() { TextBox_P2_Hole1, TextBox_P2_Hole2, TextBox_P2_Hole3, TextBox_P2_Hole4, TextBox_P2_Hole5, TextBox_P2_Hole6, TextBox_P2_Hole7, TextBox_P2_Hole8, TextBox_P2_Hole9 };
        Player3ScoreTextBoxes = new List<TextBox>() { TextBox_P3_Hole1, TextBox_P3_Hole2, TextBox_P3_Hole3, TextBox_P3_Hole4, TextBox_P3_Hole5, TextBox_P3_Hole6, TextBox_P3_Hole7, TextBox_P3_Hole8, TextBox_P3_Hole9 };
        Player4ScoreTextBoxes = new List<TextBox>() { TextBox_P4_Hole1, TextBox_P4_Hole2, TextBox_P4_Hole3, TextBox_P4_Hole4, TextBox_P4_Hole5, TextBox_P4_Hole6, TextBox_P4_Hole7, TextBox_P4_Hole8, TextBox_P4_Hole9 };

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
        EventsAndGolfers eventsAndGolfers = DatabaseFunctions.GetEventsAndPlayers(leagueID, currentSeasonID);
        Dictionary<int, string> courses = DatabaseFunctions.GetAllCourses();

        Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);
       
        
        if (!Page.IsPostBack)
        {
            ListItem initialItem = new ListItem("Select Event to Continue", "0");
            Dropdown_LeagueEvent.Items.Add(initialItem);
            //TODO only add events in current season?
            foreach (int eventID in eventsAndGolfers.events.Keys)
            {
                ListItem eventItem = new ListItem(eventsAndGolfers.events[eventID].EventName, eventID.ToString());
                Dropdown_LeagueEvent.Items.Add(eventItem);
            }
            Dropdown_LeagueEvent.SelectedIndex = 0;

            

            ListItem itemToAdd = new ListItem("Select Course", "0");
            DropDownCourseSelect.Items.Add(itemToAdd);
            foreach (int courseID in courses.Keys)
            {
                ListItem newItem = new ListItem(courses[courseID], courseID.ToString());
                DropDownCourseSelect.Items.Add(newItem);
            }

            //Add All Players in League to the Sub Selection Box
        }

    }

    //private void modifyTextBoxVisibility(List<TextBox> textBoxes, bool visible)
    //{
    //    foreach (TextBox textb in textBoxes)
    //    {
    //        textb.Visible = visible;
    //    }
    //}

    protected void Dropdown_LeagueEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            ViewState["EventID"] = selectedEventID;
            Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(selectedEventID);
            Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);
            DateTime? eventDate = DatabaseFunctions.GetEventDate(selectedEventID);
            if (eventDate == null)
            {
                Panel_EventDetails.Visible = true;
                Panel_SelectMatchup.Visible = false;
            }
            else
            {
                Panel_SelectMatchup.Visible = true;
                Panel_EventDetails.Visible = false;
            }

            Dropdown_MatchupSelect.Items.Clear();

            ListItem initialItem = new ListItem("Select Matchup", "0");
            Dropdown_MatchupSelect.Items.Add(initialItem);

            foreach (int team1ID in matchups.Keys)
            {
                string matchupText = teams[team1ID] + " vs. " + teams[matchups[team1ID]];
                string matchupID = team1ID.ToString() + "_" + matchups[team1ID].ToString();
                ListItem matchupItem = new ListItem(matchupText, matchupID);
                Dropdown_MatchupSelect.Items.Add(matchupItem);
            }

            Dropdown_MatchupSelect.Enabled = true;
        }
    }

    private void AddScoresToTextBoxes(List<TextBox> textBoxes, List<Byte> scores)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            textBoxes[i].Text = scores[i].ToString();
        }
    }

    protected void Dropdown_MatchupSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Dropdown_MatchupSelect.SelectedValue == "0")
        {
            return;
        }


        ScoreEntryTable.Visible = true;

        //if (radiobutton_holebyhole.Checked)
        //{
        //    ScoreEntryTable.Visible = true;
        //}
        //else
        //{
        //    Session["ScoreEntryMethod"] = "csv";
        //    CSVTable.Visible = true;
        //}

        string[] teamIDs = Dropdown_MatchupSelect.SelectedValue.Split('_');

        //Add Player names to the scorecard based on the selected Matchups
        //TODO support more than 2 golfers

        Dictionary<int, string> golfers = DatabaseFunctions.GetGolfers(int.Parse(teamIDs[0]));
        List<int> temp = new List<int>();
        foreach (int GolferID in golfers.Keys)
        {
            temp.Add(GolferID);
        }
        Label_Player1Name.Text = golfers[temp[0]];
        ViewState["Player1ID"] = temp[0];
        ViewState["Player1OriginalID"] = temp[0];
        Label_Player2Name.Text = golfers[temp[1]];
        ViewState["Player2ID"] = temp[1];
        ViewState["Player2OriginalID"] = temp[1];
        //Label_Player1Name_CSV.Text = golfers[temp[0]];
        //Label_Player2Name_CSV.Text = golfers[temp[1]];

        golfers = DatabaseFunctions.GetGolfers(int.Parse(teamIDs[1]));
        temp.Clear();
        foreach (int GolferID in golfers.Keys)
        {
            temp.Add(GolferID);
        }
        Label_Player3Name.Text = golfers[temp[0]];
        ViewState["Player3ID"] = temp[0];
        ViewState["Player3OriginalID"] = temp[0];
        Label_Player4Name.Text = golfers[temp[1]];
        ViewState["Player4ID"] = temp[1];
        ViewState["Player4OriginalID"] = temp[1];
        //Label_Player3Name_CSV.Text = golfers[temp[0]];
        //Label_Player4Name_CSV.Text = golfers[temp[1]];


        //Add previously entered scores
        clearScoreTextBoxes();
        Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores((int)ViewState["EventID"]);
        
        if (scores.ContainsKey((int)ViewState["Player1ID"]))
        {
            AddScoresToTextBoxes(Player1ScoreTextBoxes, scores[(int)ViewState["Player1ID"]]); 
        }
        if (scores.ContainsKey((int)ViewState["Player2ID"]))
        {
            AddScoresToTextBoxes(Player2ScoreTextBoxes, scores[(int)ViewState["Player2ID"]]);
        }
        if (scores.ContainsKey((int)ViewState["Player3ID"]))
        {
            AddScoresToTextBoxes(Player3ScoreTextBoxes, scores[(int)ViewState["Player3ID"]]);
        }
        if (scores.ContainsKey((int)ViewState["Player4ID"]))
        {
            AddScoresToTextBoxes(Player4ScoreTextBoxes, scores[(int)ViewState["Player4ID"]]);
        }
    }

    private void UpdateTotalScore(List<TextBox> scoreTextBoxes, TextBox totalScoreTextBox)
    {
        int temp;
        int totalScore = 0;
        foreach (TextBox textBox in scoreTextBoxes)
        {
            if (int.TryParse(textBox.Text, out temp))
            {
                totalScore += temp;
            }
        }
        if (totalScore != 0)
        {
            totalScoreTextBox.Text = totalScore.ToString();
        }
    }

    private void CSV_UpdateTotalScore(TextBox TotalScoreTextBox, string CSVText)
    {
        List<byte> scores = GetCSVScores(CSVText);
        if (scores != null)
        {
            int totalScore = 0;
            foreach (byte score in scores)
            {
                totalScore += score;
            }
            TotalScoreTextBox.Text = totalScore.ToString();
        }
    }

    protected void UpdateTotal(object sender, CommandEventArgs e)
    {
        if (radiobutton_holebyhole.Checked)
        {
            UpdateTotalScore(Player1ScoreTextBoxes, TextBox_P1_Total);
            UpdateTotalScore(Player2ScoreTextBoxes, TextBox_P2_Total);
            UpdateTotalScore(Player3ScoreTextBoxes, TextBox_P3_Total);
            UpdateTotalScore(Player4ScoreTextBoxes, TextBox_P4_Total);
        }
        else
        {
            CSV_UpdateTotalScore(TextBox_P1_Total, TextBox_Player1_CSV.Text);
            CSV_UpdateTotalScore(TextBox_P2_Total, TextBox_Player2_CSV.Text);
            CSV_UpdateTotalScore(TextBox_P3_Total, TextBox_Player3_CSV.Text);
            CSV_UpdateTotalScore(TextBox_P4_Total, TextBox_Player4_CSV.Text);
        }
    }

    private void SaveScore(List<Byte> scores, int playerID, string HandicapOvveride)
    {
        DatabaseFunctions.AddScore(playerID, (int)ViewState["EventID"], scores);
        int handicapOverrideInt;
        if (int.TryParse(HandicapOvveride, out handicapOverrideInt))
        {
            DatabaseFunctions.InsertHandicapOverride(playerID, (int)ViewState["EventID"], handicapOverrideInt);
        } 
    }

   
    protected void SaveScores(object sender, CommandEventArgs e)
    {
        int golferID;
        int eventID = (int)ViewState["EventID"];
        try
        {
            golferID = (int)ViewState["Player1ID"];
            List<byte> scores = null;
            if (ViewState["Player1NoShow"] == null || (bool)ViewState["Player1NoShow"] == false)
            {
                if (radiobutton_csv.Checked)
                {
                    scores = GetCSVScores(TextBox_Player1_CSV.Text, DatabaseFunctions.GetGolferName(golferID));
                }
                else
                {
                    scores = GetScores(Player1ScoreTextBoxes, DatabaseFunctions.GetGolferName(golferID));
                }
                if (scores != null)
                {
                    SaveScore(scores, golferID, TextBox_P1_HandicapOvveride.Text);
                }
            }
            else
            {
                DatabaseFunctions.AddNoShow(eventID, (int)ViewState["Player1OriginalID"]);
            }

            golferID = (int)ViewState["Player2ID"];
            if (ViewState["Player2NoShow"] == null || (bool)ViewState["Player2NoShow"] == false)
            {
                if (radiobutton_csv.Checked)
                {
                    scores = GetCSVScores(TextBox_Player2_CSV.Text, DatabaseFunctions.GetGolferName(golferID));
                }
                else
                {
                    scores = GetScores(Player2ScoreTextBoxes, DatabaseFunctions.GetGolferName(golferID));
                }
                if (scores != null)
                {
                    SaveScore(scores, golferID, TextBox_P2_HandicapOvveride.Text);
                }
            }
            else
            {
                DatabaseFunctions.AddNoShow(eventID, (int)ViewState["Player2OriginalID"]);
            }

            golferID = (int)ViewState["Player3ID"];
            if (ViewState["Player3NoShow"] == null || (bool)ViewState["Player3NoShow"] == false)
            {
                if (radiobutton_csv.Checked)
                {
                    scores = GetCSVScores(TextBox_Player3_CSV.Text, DatabaseFunctions.GetGolferName(golferID));
                }
                else
                {
                    scores = GetScores(Player3ScoreTextBoxes, DatabaseFunctions.GetGolferName(golferID));
                }
                if (scores != null)
                {
                    SaveScore(scores, golferID, TextBox_P3_HandicapOvveride.Text);
                }
            }
            else
            {
                DatabaseFunctions.AddNoShow(eventID, (int)ViewState["Player3OriginalID"]);
            }

            golferID = (int)ViewState["Player4ID"];
            if (ViewState["Player4NoShow"] == null || (bool)ViewState["Player4NoShow"] == false)
            {
                if (radiobutton_csv.Checked)
                {
                    scores = GetCSVScores(TextBox_Player4_CSV.Text, DatabaseFunctions.GetGolferName(golferID));
                }
                else
                {
                    scores = GetScores(Player4ScoreTextBoxes, DatabaseFunctions.GetGolferName(golferID));
                }
                if (scores != null)
                {
                    SaveScore(scores, golferID, TextBox_P4_HandicapOvveride.Text);
                }
            }
            else
            {
                DatabaseFunctions.AddNoShow(eventID, (int)ViewState["Player4OriginalID"]);
            }
        }
        catch(Exception ex)
        {
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>");
            return;
        }

        AddSubs();
      
        ScoreEntryTable.Visible = false;
        clearScoreTextBoxes();
        Dropdown_MatchupSelect.Items.RemoveAt(Dropdown_MatchupSelect.SelectedIndex);
        Dropdown_MatchupSelect.SelectedIndex = 0;
        Response.Write("<script language='javascript'>alert('Scores Added Successfully.');</script>");
    }

    private void AddSubs()
    {
        List<int> playerIDs =  new List<int>(){(int)ViewState["Player1ID"],(int)ViewState["Player2ID"],(int)ViewState["Player3ID"],(int)ViewState["Player4ID"]};
        List<int> originalPlayerIDs =  new List<int>(){(int)ViewState["Player1OriginalID"],(int)ViewState["Player2OriginalID"],(int)ViewState["Player3OriginalID"],(int)ViewState["Player4OriginalID"]};
        for (int i = 0; i < 4; i++)
        {
            if (playerIDs[i] != originalPlayerIDs[i])
            {
                DatabaseFunctions.AddSubs((int)ViewState["EventID"], playerIDs[i], originalPlayerIDs[i]);
            }
        }
    }

    private void clearScoreTextBoxes(int playerIndex)
    {
        if (playerIndex == 1)
        {
            foreach (TextBox t in Player1ScoreTextBoxes)
            {
                t.Text = "";
            }
            TextBox_P1_Total.Text = "";
            TextBox_P1_HandicapOvveride.Text = "";
        }
        if (playerIndex == 2)
        {
            foreach (TextBox t in Player2ScoreTextBoxes)
            {
                t.Text = "";
            }
            TextBox_P2_Total.Text = "";
            TextBox_P2_HandicapOvveride.Text = "";
        }
        if (playerIndex == 3)
        {
            foreach (TextBox t in Player3ScoreTextBoxes)
            {
                t.Text = "";
            }
            TextBox_P3_Total.Text = "";
            TextBox_P3_HandicapOvveride.Text = "";
        }
        if (playerIndex == 1)
        {
            foreach (TextBox t in Player4ScoreTextBoxes)
            {
                t.Text = "";
            }
            TextBox_P4_Total.Text = "";
            TextBox_P4_HandicapOvveride.Text = "";
        }
        
    }

    private void clearScoreTextBoxes()
    {
        foreach (TextBox t in Player1ScoreTextBoxes)
        {
            t.Text = "";
        }
        foreach (TextBox t in Player2ScoreTextBoxes)
        {
            t.Text = "";
        }
        foreach (TextBox t in Player3ScoreTextBoxes)
        {
            t.Text = "";
        }
        foreach (TextBox t in Player4ScoreTextBoxes)
        {
            t.Text = "";
        }
        TextBox_P1_Total.Text = "";
        TextBox_P2_Total.Text = "";
        TextBox_P3_Total.Text = "";
        TextBox_P4_Total.Text = "";
        TextBox_P1_HandicapOvveride.Text = "";
        TextBox_P2_HandicapOvveride.Text = "";
        TextBox_P3_HandicapOvveride.Text = "";
        TextBox_P4_HandicapOvveride.Text = "";
        TextBox_Player1_CSV.Text = "";
        TextBox_Player2_CSV.Text = "";
        TextBox_Player3_CSV.Text = "";
        TextBox_Player4_CSV.Text = "";
    }


    private void AddHandicapOverrides(string player1Override, string player2Ovveride, string player3Ovveride, string player4Override)
    {
        int handicapOverride;
        if (int.TryParse(player1Override, out handicapOverride))
        {
            DatabaseFunctions.InsertHandicapOverride((int)ViewState["Player1ID"], (int)ViewState["EventID"], handicapOverride);
        }
        if (int.TryParse(player2Ovveride, out handicapOverride))
        {
            DatabaseFunctions.InsertHandicapOverride((int)ViewState["Player2ID"], (int)ViewState["EventID"], handicapOverride);
        }
        if (int.TryParse(player3Ovveride, out handicapOverride))
        {
            DatabaseFunctions.InsertHandicapOverride((int)ViewState["Player3ID"], (int)ViewState["EventID"], handicapOverride);
        }
        if (int.TryParse(player4Override, out handicapOverride))
        {
            DatabaseFunctions.InsertHandicapOverride((int)ViewState["Player4ID"], (int)ViewState["EventID"], handicapOverride);
        }
    }

    private List<byte> GetScores(List<TextBox> scoreTextBoxes, string PlayerName)
    {
        if (!ValidateScores(scoreTextBoxes))
        {
            throw new Exception("Problem with Scores Entered for " + PlayerName);
        }
        List<byte> retVal = new List<byte>();
        foreach (TextBox textBox in scoreTextBoxes)
        {
            retVal.Add(byte.Parse(textBox.Text));
        }
        return retVal;
    }

    private Boolean ValidateScores(List<TextBox> scoreTextBoxes)
    {
        int score;
        foreach (TextBox t in scoreTextBoxes)
        {
            if (!int.TryParse(t.Text, out score))
            {
                return false;
            }
        }
        
        return true;
      
    }

    /// <summary>
    /// Check that scores consists of 9 numbers 
    /// </summary>
    /// <param name="scores"></param>
    /// <returns></returns>
    private Boolean ValidateScores(List<string> scores, string golferNumber)
    {
        byte s;
        if (scores.Count != 9)
        {
            Response.Write("<script language='javascript'>alert('Problem Adding Scores to Database... Golfer " + golferNumber +" does not have 9 scores');</script>");
            return false;
        }
        foreach (string score in scores)
        {
            if (!byte.TryParse(score, out s))
            {
                Response.Write("<script language='javascript'>alert('Problem Adding Scores to Database... Golfer " + golferNumber + " has an invalid score');</script>");
                return false;
            }
        }
        return true;
    }

    //protected void SaveScoreCSV(object sender, CommandEventArgs e)
    //{
    //    if (!AddCSVScores(TextBox_Player1_CSV.Text, (int)ViewState["Player1ID"], "1"))
    //    {
    //        return;
    //    }
    //    if(!AddCSVScores(TextBox_Player2_CSV.Text, (int)ViewState["Player2ID"], "2"))
    //    {
    //        return;
    //    }
    //    if (!AddCSVScores(TextBox_Player3_CSV.Text, (int)ViewState["Player3ID"], "3"))
    //    {
    //        return;
    //    }
    //    if (!AddCSVScores(TextBox_Player4_CSV.Text, (int)ViewState["Player4ID"], "4"))
    //    {
    //        return;
    //    }
       
    //    AddHandicapOverrides(TextBox_P1_HandicapOvverideCSV.Text, TextBox_P2_HandicapOvverideCSV.Text, TextBox_P3_HandicapOvverideCSV.Text, TextBox_P4_HandicapOvverideCSV.Text);

    //    CSVTable.Visible = false;
    //    TextBox_Player1_CSV.Text = "";
    //    TextBox_Player2_CSV.Text = "";
    //    TextBox_Player3_CSV.Text = "";
    //    TextBox_Player4_CSV.Text = "";
    //    Dropdown_MatchupSelect.Items.RemoveAt(Dropdown_MatchupSelect.SelectedIndex);
    //    Dropdown_MatchupSelect.SelectedIndex = 0;
    //    Response.Write("<script language='javascript'>alert('Scores Added Successfully.');</script>");

    //}

    private List<byte> GetCSVScores(string CSVText)
    {
        try
        {
            CSVText = CSVText.Trim();
            String[] scores = CSVText.Split(new char[] { ',', ' ', '\t' });
            List<byte> scoreList = new List<byte>();
            foreach (string score in scores)
            {
                scoreList.Add(byte.Parse(score));
            }
            return scoreList;
        }
        catch
        {
            return null;
        }
    }


    private List<byte> GetCSVScores(string CSVText, string golferIndex)
    {
        try
        {
            CSVText = CSVText.Trim();
            String[] scores = CSVText.Split(new char[]{',',' ','\t'});

            if (!ValidateScores(scores.ToList(), golferIndex))
            {           
                return null;
            }

            List<byte> scoreList = new List<byte>();
            foreach (string score in scores)
            {
                scoreList.Add(byte.Parse(score));
            }

            return scoreList;
            //DatabaseFunctions.AddScore(GolferID, (int)ViewState["EventID"], scoreList);
        }
        catch
        {
            Response.Write("<script language='javascript'>alert('Problem Adding Scores to Database.. verify input contains only commas and numbers.');</script>");
            //TODO Add error message
            return null;
        }      
    }

    private void modifyRowVisibility(bool visible, Label playerNameLabel, List<TextBox> scoreTextBoxes, string viewStateKey, Button subButton, DropDownList subDropdownList)
    {
        foreach (TextBox textBox in scoreTextBoxes)
        {
            textBox.Visible = visible;
        }
        playerNameLabel.Visible = visible;
        subButton.Visible = visible;
        subDropdownList.Visible = false;
        if (!visible)
        {
            ViewState[viewStateKey] = true;
        }
        else
        {
            ViewState[viewStateKey] = false;
        }
    }

    protected void SaveEventDetails(object sender, CommandEventArgs e)
    {
        if (Calendar_EventDate.SelectedDate != DateTime.MinValue)
        {
            DatabaseFunctions.UpdateEventDetails((int)ViewState["EventID"], Calendar_EventDate.SelectedDate, int.Parse(DropDownCourseSelect.SelectedValue));
            Panel_SelectMatchup.Visible = true;
            Panel_EventDetails.Visible = false;
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Must Select Date and Course Before Entering Scores.');</script>");
        }
    }

    protected void NoShowButtonClicked(object sender, CommandEventArgs e)
    {
        Button b = (Button)sender;

        if (b.ID == "Button_Player1NoShow")
        {
            bool visible = !Label_Player1Name.Visible;
            modifyRowVisibility(visible, Label_Player1Name, Player1ScoreTextBoxes, "Player1NoShow", Button_Player1Sub, DropdownPlayer1Sub);
        }
        else if (b.ID == "Button_Player2NoShow")
        {
            bool visible = !Label_Player2Name.Visible;
            modifyRowVisibility(visible, Label_Player2Name, Player2ScoreTextBoxes, "Player2NoShow", Button_Player2Sub, DropdownPlayer2Sub);
        }
        else if (b.ID == "Button_Player3NoShow")
        {
            bool visible = !Label_Player3Name.Visible;
            modifyRowVisibility(visible, Label_Player3Name, Player3ScoreTextBoxes, "Player3NoShow", Button_Player3Sub, DropdownPlayer3Sub);
        }
        else if (b.ID == "Button_Player4NoShow")
        {
            bool visible = !Label_Player4Name.Visible;
            modifyRowVisibility(visible, Label_Player4Name, Player4ScoreTextBoxes, "Player4NoShow", Button_Player4Sub, DropdownPlayer4Sub);
        } 
    }

    private void HandleSubClicked(DropDownList dropdown, Label golferNameLabel)
    {
        dropdown.Items.Clear();
        dropdown.Visible = true;
        golferNameLabel.Visible = false;
        AddAllPlayersToDropdown(dropdown);
    }

    protected void EnterSubButtonClicked(object sender, CommandEventArgs e)
    {
        
        Button b = (Button)sender;
       
        if (b.ID == "Button_Player1Sub")
        {
            clearScoreTextBoxes(1);
            HandleSubClicked(DropdownPlayer1Sub, Label_Player1Name);
        }
        else if (b.ID == "Button_Player2Sub")
        {
            clearScoreTextBoxes(2);
            HandleSubClicked(DropdownPlayer2Sub, Label_Player2Name);
        }
        else if (b.ID == "Button_Player3Sub")
        {
            clearScoreTextBoxes(3);
            HandleSubClicked(DropdownPlayer3Sub, Label_Player3Name);
        }
        else if (b.ID == "Button_Player4Sub")
        {
            clearScoreTextBoxes(4);
            HandleSubClicked(DropdownPlayer4Sub, Label_Player4Name);
        }
        //else if (b.ID == "Button_Player1SubCSV")
        //{
        //    HandleSubClicked(DropdownPlayer1SubCSV, Label_Player1Name_CSV);
        //}
        //else if (b.ID == "Button_Player2SubCSV")
        //{
        //    HandleSubClicked(DropdownPlayer2SubCSV, Label_Player2Name_CSV);
        //}
        //else if (b.ID == "Button_Player3SubCSV")
        //{
        //    HandleSubClicked(DropdownPlayer3SubCSV, Label_Player3Name_CSV);
        //}
        //else if (b.ID == "Button_Player4SubCSV")
        //{
        //    HandleSubClicked(DropdownPlayer4SubCSV, Label_Player4Name_CSV);
        //}
    }


    private void AddAllPlayersToDropdown(DropDownList dropdown)
    {
        Dictionary<int, string> allGolfers = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());//get all players
        ListItem selectionItem = new ListItem("Select Golfer", "0");
        dropdown.Items.Add(selectionItem);
        foreach (int golferID in allGolfers.Keys)
        {
            ListItem playerItem = new ListItem(allGolfers[golferID], golferID.ToString());
            dropdown.Items.Add(playerItem);
        }
    }

    protected void ScoreEntryModeChanged(object sender, EventArgs e)
    {
        if (radiobutton_csv.Checked)
        {
            //modifyTextBoxVisibility(Player1ScoreTextBoxes, false);
            Table_P1.Visible = false;
            Table_P2.Visible = false;
            Table_P3.Visible = false;
            Table_P4.Visible = false;
            TextBox_Player1_CSV.Visible = true;
            TextBox_Player2_CSV.Visible = true;
            TextBox_Player3_CSV.Visible = true;
            TextBox_Player4_CSV.Visible = true;
        }
        else
        {
            //modifyTextBoxVisibility(Player1ScoreTextBoxes, true);
            Table_P1.Visible = true;
            Table_P2.Visible = true;
            Table_P3.Visible = true;
            Table_P4.Visible = true;
            TextBox_Player1_CSV.Visible = false;
            TextBox_Player2_CSV.Visible = false;
            TextBox_Player3_CSV.Visible = false;
            TextBox_Player4_CSV.Visible = false;
        }
    }
  
    protected void SubSelected(object sender, EventArgs e)
    {
        DropDownList dropdown = (DropDownList)sender;
        if (dropdown.SelectedValue == "0")
        {
            return;
        }
        if (dropdown.ID ==  "DropdownPlayer1Sub")
        {
            DropdownPlayer1Sub.Visible = false;
            Label_Player1Name.Visible = true;
            ViewState["Player1ID"] = int.Parse(dropdown.SelectedValue);
            Label_Player1Name.Text = dropdown.SelectedItem.Text;
           
        }
        else if (dropdown.ID == "DropdownPlayer2Sub")
        {
            DropdownPlayer2Sub.Visible = false;
            Label_Player2Name.Visible = true;
            ViewState["Player2ID"] = int.Parse(dropdown.SelectedValue);
            Label_Player2Name.Text = dropdown.SelectedItem.Text;
        }
        else if (dropdown.ID == "DropdownPlayer3Sub")
        {
            DropdownPlayer3Sub.Visible = false;
            Label_Player3Name.Visible = true;
            ViewState["Player3ID"] = int.Parse(dropdown.SelectedValue);
            Label_Player3Name.Text = dropdown.SelectedItem.Text;
        }
        else if (dropdown.ID == "DropdownPlayer4Sub")
        {
            DropdownPlayer4Sub.Visible = false;
            Label_Player4Name.Visible = true;
            ViewState["Player4ID"] = int.Parse(dropdown.SelectedValue);
            Label_Player4Name.Text = dropdown.SelectedItem.Text;
        }
        //else if (dropdown.ID == "DropdownPlayer4SubCSV")
        //{
        //    DropdownPlayer4SubCSV.Visible = false;
        //    Label_Player4Name_CSV.Visible = true;
        //    ViewState["Player4ID"] = int.Parse(dropdown.SelectedValue);
        //    Label_Player4Name_CSV.Text = dropdown.SelectedItem.Text;
        //}
        //else if (dropdown.ID == "DropdownPlayer1SubCSV")
        //{
        //    DropdownPlayer1SubCSV.Visible = false;
        //    Label_Player1Name_CSV.Visible = true;
        //    ViewState["Player1ID"] = int.Parse(dropdown.SelectedValue);
        //    Label_Player1Name_CSV.Text = dropdown.SelectedItem.Text;
        //}
        //else if (dropdown.ID == "DropdownPlayer2SubCSV")
        //{
        //    DropdownPlayer2SubCSV.Visible = false;
        //    Label_Player2Name_CSV.Visible = true;
        //    ViewState["Player2ID"] = int.Parse(dropdown.SelectedValue);
        //    Label_Player2Name_CSV.Text = dropdown.SelectedItem.Text;
        //}
        //else if (dropdown.ID == "DropdownPlayer3SubCSV")
        //{
        //    DropdownPlayer3SubCSV.Visible = false;
        //    Label_Player3Name_CSV.Visible = true;
        //    ViewState["Player3ID"] = int.Parse(dropdown.SelectedValue);
        //    Label_Player3Name_CSV.Text = dropdown.SelectedItem.Text;
        //}
    }
}