using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using AjaxControlToolkit;

public partial class WeeklyResults : System.Web.UI.Page
{
    private int leagueID;
    private string SKINS_VIEW_HANDICAPPED = "HandicappedSkins";
    private string SKINS_VIEW = "Skins";
    private string WEEKLY_RESULTS_VIEW = "Team Results";
    private string WEEKLY_LEADERBOARD_VIEW = "Weekly Leaderboards";
    Dictionary<int, CheckBox> skinsCheckboxes = new Dictionary<int, CheckBox>();
    private bool toggle;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        
        if (!Page.IsPostBack)
        {
            int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
            AddEventsToDropdown(currentSeasonID);

            List<Season> seasons = DatabaseFunctions.GetSeasons(leagueID);
            int i = 0;
            foreach (Season season in seasons)
            {
                ListItem newItem = new ListItem(season.SeasonName, season.SeasonID.ToString());
                Dropdown_Seasons.Items.Add(newItem);
                if (season.isCurrentSeason)
                {
                    Dropdown_Seasons.SelectedIndex = i;
                }
                i++;              
            }
            //populate views dropdown
            ListItem initialItem = new ListItem("Select View to Continue", "0");
            DropDown_View.Items.Add(initialItem);
            DropDown_View.Items.Add(SKINS_VIEW);
            DropDown_View.Items.Add(SKINS_VIEW_HANDICAPPED);
            DropDown_View.Items.Add(WEEKLY_LEADERBOARD_VIEW);
            DropDown_View.Items.Add(WEEKLY_RESULTS_VIEW);

            ViewState["PlayersToExcludeFromSkins"] = new List<int>();
            ListItem initialItem2 = new ListItem("Select Golfers(s) to Exclude From Skins", "0");
            DropdownList_SkinsPlayers.Items.Add(initialItem2);
        }

        

    }

    private void AddEventsToDropdown(int seasonID)
    {
        Dictionary<int, EventInfo> events = DatabaseFunctions.GetEventsWithScoresPosted(leagueID, seasonID);
        SortedDictionary<DateTime, int> sortedEventIDs = new SortedDictionary<DateTime, int>();
 
        ListItem initialItem = new ListItem("Select Event to Continue", "0");
        Dropdown_LeagueEvent.Items.Add(initialItem);
        //TODO only add events in current season?
        foreach (int eventID in events.Keys)
        {
            sortedEventIDs.Add(DateTime.Parse(events[eventID].Date), eventID);
        }

        foreach (int eventID in sortedEventIDs.Values)
        {
            ListItem eventItem = new ListItem(events[eventID].EventName, eventID.ToString());
            Dropdown_LeagueEvent.Items.Add(eventItem);
        }
        Dropdown_LeagueEvent.SelectedIndex = 0;
    }

    

    private GolferInfo GetGolferNames(int Team1_A_PlayerID, int Team2_A_PlayerID, int Team1_B_PlayerID, int Team2_B_PlayerID)
    {
        GolferInfo retVal = new GolferInfo();

        if (Team1_A_PlayerID != 0)
        {
            retVal.Aplayer1Name = DatabaseFunctions.GetGolferName(Team1_A_PlayerID);
        }
        else
        {
            retVal.Aplayer1Name = "No Show";
        }

        if (Team2_A_PlayerID != 0)
        {
            retVal.Aplayer2Name = DatabaseFunctions.GetGolferName(Team2_A_PlayerID);
        }
        else
        {
            retVal.Aplayer2Name = "No Show";
        }

        if (Team1_B_PlayerID != 0)
        {
            retVal.Bplayer1Name = DatabaseFunctions.GetGolferName(Team1_B_PlayerID);
        }
        else
        {
            retVal.Bplayer1Name = "No Show";
        }

        if (Team2_B_PlayerID != 0)
        {
            retVal.Bplayer2Name = DatabaseFunctions.GetGolferName(Team2_B_PlayerID);
        }
        else
        {
            retVal.Bplayer2Name = "No Show";
        }

        return retVal;
    }

    struct GolferInfo
    {
        public string Aplayer1Name;
        public string Aplayer2Name;
        public string Bplayer1Name;
        public string Bplayer2Name;

        public int Aplayer1Hcp;
        public int Bplayer1Hcp;
        public int Aplayer2Hcp;
        public int Bplayer2Hcp;

        public string Team1Name;
        public string Team2Name;
    }



    protected void Dropdown_Season_SelectedIndexChanged(object sender, EventArgs e)
    {
        int seasonID = int.Parse(Dropdown_Seasons.SelectedValue);
        Dropdown_LeagueEvent.Items.Clear();
        AddEventsToDropdown(seasonID);
        UpdateContent();
    }

    protected void Dropdown_View_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateContent();
    }

    protected void SkinsPlayersChanged(object sender, EventArgs e)
    {
        if (DropdownList_SkinsPlayers.SelectedValue != "0")
        {
            List<int> temp = (List<int>)ViewState["PlayersToExcludeFromSkins"];
            temp.Add(int.Parse(DropdownList_SkinsPlayers.SelectedValue));
            ViewState["PlayersToExcludeFromSkins"] = temp;
            DisplaySkins();
        }
    }

    private void UpdateContent()
    {
        if (DropDown_View.Text == WEEKLY_RESULTS_VIEW)
        {
            Panel_Leaderboards.Visible = false;
            Panel_Matchups.Visible = true;
            Panel_Skins.Visible = false;
            DisplayWeeklyResults();
            ViewState["PlayersToExcludeFromSkins"] = new List<int>();
        }
        else if (DropDown_View.Text == WEEKLY_LEADERBOARD_VIEW)
        {
            Panel_Leaderboards.Visible = true;
            Panel_Matchups.Visible = false;
            Panel_Skins.Visible = false;
            DisplayWeeklyLeaderboard();
            ViewState["PlayersToExcludeFromSkins"] = new List<int>();
        }
        else if (DropDown_View.Text == SKINS_VIEW)
        {
            Panel_Leaderboards.Visible = false;
            Panel_Matchups.Visible = false;
            Panel_Skins.Visible = true;
            DisplaySkins();
        }
        else if (DropDown_View.Text == SKINS_VIEW_HANDICAPPED)
        {
            Panel_Leaderboards.Visible = false;
            Panel_Matchups.Visible = false;
            Panel_Skins.Visible = true;
            DisplayHandicappedSkins();
        }
    }

    protected void Dropdown_LeagueEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateContent();      
    }

    private void RemoveGolfersFromSkins(Dictionary<int, List<byte>> scores)
    {
        List<int> golfersToExclude = (List<int>)ViewState["PlayersToExcludeFromSkins"];
        foreach (int GolferID in golfersToExclude)
        {
            if (scores.ContainsKey(GolferID))
            {
                scores.Remove(GolferID);
            }  
        }
    }

    private void DisplaySkins()
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);
            RemoveGolfersFromSkins(scores);
            //skinsCheckboxes = new Dictionary<int, CheckBox>();
            //Panel_Skins.ContentTemplateContainer.Controls.Clear();
           
            //Table SkinsTable = new Table();

            Table_SkinsResults.Width = 800;
            Table_SkinsResults.Rows.Clear();

            AddCourseInfoRowsSkinsTable(Table_SkinsResults, DatabaseFunctions.GetCourseInfo(selectedEventID));

            List<int> skins = Scoring.calculateSkins(scores);
            
            //reset skins dropdown
            DropdownList_SkinsPlayers.Items.Clear();
            ListItem initialItem = new ListItem("Select Golfers(s) to Exclude From Skins", "0");
            DropdownList_SkinsPlayers.Items.Add(initialItem);

            foreach (int GolferID in scores.Keys)
            {
                ListItem item = new ListItem(DatabaseFunctions.GetGolferName(GolferID), GolferID.ToString());
                DropdownList_SkinsPlayers.Items.Add(item);
                AddTableRow(Table_SkinsResults, GolferID, scores[GolferID], skins);
            }
            //TableRow bottomRow = new TableRow();
            //TableCell cell1 = new TableCell();
            //Button recalculateButton = new Button();
            //recalculateButton.Text = "Recalculate Skins Using Only Selected Players";
            ////recalculateButton.Command += RecalculateSkinsButtonPress;
            //cell1.Controls.Add(recalculateButton);
            //bottomRow.Cells.Add(cell1);
            //SkinsTable.Rows.Add(bottomRow);

            //Panel_Skins.ContentTemplateContainer.Controls.Add(SkinsTable);
        }
       
    }

    private void DisplayHandicappedSkins()
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);
            Dictionary<int, int> handicaps = Scoring.GetPlayerHandicapsForEvent(leagueID, selectedEventID, scores.Keys.ToList());

                Table_SkinsResults.Width = 800;
            Table_SkinsResults.Rows.Clear();
            var courseInfo = DatabaseFunctions.GetCourseInfo(selectedEventID);
            AddCourseInfoRowsSkinsTable(Table_SkinsResults, courseInfo);

            Scoring.ApplyHandicapsToScoresForHandicappedSkins(scores, handicaps, courseInfo);
            List<int> skins = Scoring.calculateSkins(scores);

            //reset skins dropdown
            DropdownList_SkinsPlayers.Items.Clear();
            ListItem initialItem = new ListItem("Select Golfers(s) to Exclude From Skins", "0");
            DropdownList_SkinsPlayers.Items.Add(initialItem);

            foreach (int GolferID in scores.Keys)
            {
                ListItem item = new ListItem(DatabaseFunctions.GetGolferName(GolferID), GolferID.ToString());
                DropdownList_SkinsPlayers.Items.Add(item);
                AddTableRow(Table_SkinsResults, GolferID, scores[GolferID], skins);
            }         
        }

    }



    private void DisplayWeeklyLeaderboard()
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            Scoring.EventStats eventStats = Scoring.GetEventResults(leagueID, selectedEventID, null);
            Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);

            //Clear Tables and Add Title Rows
            Table_GrossScoreLeaderboard.Rows.Clear();
            Table_NetScoreLeaderboard.Rows.Clear();
            Table_TeamPts.Rows.Clear();

            AddLeaderboardTitleRow(Table_GrossScoreLeaderboard, "Gross Score");
            AddLeaderboardTitleRow(Table_NetScoreLeaderboard, "Net Score");
            AddLeaderboardTitleRow(Table_TeamPts, "Team Points");


            var items = from pair in eventStats.grossScores
                    orderby pair.Value ascending
                    select pair;
            
            foreach (KeyValuePair<int, int> pair in items)
	        {
                AddTableRowLeaderboard(Table_GrossScoreLeaderboard, DatabaseFunctions.GetGolferName(pair.Key), (decimal)pair.Value);   
	        }

            items = from pair in eventStats.netScores
                        orderby pair.Value ascending
                        select pair;

            foreach (KeyValuePair<int, int> pair in items)
            {
                AddTableRowLeaderboard(Table_NetScoreLeaderboard, DatabaseFunctions.GetGolferName(pair.Key), (decimal)pair.Value);
            }

            var items2 = from pair in eventStats.teamPts
                    orderby pair.Value descending
                    select pair;

            foreach (KeyValuePair<int, decimal> pair in items2)
            {
                AddTableRowLeaderboard(Table_TeamPts, teamNames[pair.Key], pair.Value);
            }
        }
    }

    private void AddLeaderboardTitleRow(Table table, string Name)
    {
        TableRow tr = new TableRow();
        tr.BackColor = System.Drawing.ColorTranslator.FromHtml("#465c71");
        TableCell cell = AddCell(tr, Name);
        cell.ColumnSpan = 2;
        //cell.Font.Bold = true;
        cell.Font.Size = 18;
        cell.ForeColor = Color.White;
        table.Rows.Add(tr);
    }

    private void AddTableRowLeaderboard(Table table, string Name, decimal Pts)
    {
        TableRow tr = new TableRow();
        if(toggle)
        {
            tr.BackColor = Color.WhiteSmoke;
        }
        TableCell cell = AddCell(tr, Name);
        cell.HorizontalAlign = HorizontalAlign.Left;
        AddCell(tr, Pts.ToString());
        table.Rows.Add(tr);
        toggle = !toggle;
    }

    private void AddTableRow(Table SkinsTable, int golferID, List<byte> scores, List<int> skins)
    {
        TableRow tr = new TableRow();
        
        //CheckBox checkBox = new CheckBox();
        //checkBox.ID = "Checkbox_" + golferID.ToString();
        //checkBox.Checked = true;
        //checkBox.Text = DatabaseFunctions.GetGolferName(golferID);
        //skinsCheckboxes.Add(golferID, checkBox);

        //TableCell cell = new TableCell();
        //cell.Controls.Add(checkBox);
        //tr.Cells.Add(cell);
        AddCell(tr, DatabaseFunctions.GetGolferName(golferID));

        for (int i = 0; i < 9; i++)
        {
            TableCell c = AddCell(tr, scores[i].ToString());
            if (skins[i] == golferID)
            {
                c.BackColor = Color.LightBlue;    
            }
            
        }
        SkinsTable.Rows.Add(tr);
    }

    private void DisplayWeeklyResults()
    {
        int selectedEventID = int.Parse(Dropdown_LeagueEvent.SelectedValue);
        if (selectedEventID != 0)
        {
            ViewState["EventID"] = selectedEventID;
            Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);
            Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(selectedEventID);
            Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);
            var handicaps = Scoring.GetPlayerHandicapsForEvent(leagueID, selectedEventID, scores.Keys.ToList());
            var results = Scoring.GetEventResults(leagueID, selectedEventID, handicaps);
            CourseInfo courseInfo = DatabaseFunctions.GetCourseInfo(selectedEventID);
            int index = 0;
            foreach (int team1ID in results.matchupResultsA.Keys)
            {
                //build info for display
                GolferInfo golferInfo = GetGolferNames(results.matchupResultsA[team1ID].Team1PlayerID, results.matchupResultsA[team1ID].Team2PlayerID, results.matchupResultsB[team1ID].Team1PlayerID, results.matchupResultsB[team1ID].Team2PlayerID);
                golferInfo.Team1Name = teamNames[team1ID];
                golferInfo.Team2Name = teamNames[matchups[team1ID]];
                golferInfo.Aplayer1Hcp = handicaps[results.matchupResultsA[team1ID].Team1PlayerID];
                golferInfo.Aplayer2Hcp = handicaps[results.matchupResultsA[team1ID].Team2PlayerID];
                golferInfo.Bplayer1Hcp = handicaps[results.matchupResultsB[team1ID].Team1PlayerID];
                golferInfo.Bplayer2Hcp = handicaps[results.matchupResultsB[team1ID].Team2PlayerID];
                AddCollapsablePanel(index.ToString(), golferInfo, results.matchupResultsA[team1ID], results.matchupResultsB[team1ID], courseInfo);
                index++;
            }

            //Dictionary<int, int> matchups = DatabaseFunctions.GetMatchups(selectedEventID);
            //Dictionary<int, string> teamNames = DatabaseFunctions.GetTeamNames(leagueID);
            //Dictionary<int, List<byte>> scores = DatabaseFunctions.GetScores(selectedEventID);
            //CourseInfo courseInfo = DatabaseFunctions.GetCourseInfo(selectedEventID);
            //Dictionary<int, int> subs = DatabaseFunctions.GetSubs(selectedEventID);
            //List<int> noShows = new List<int>(); //DatabaseFunctions.GetNoShows(selectedEventID);
            //Dictionary<int, int> handicaps = DatabaseFunctions.GetHandicapOverrides(selectedEventID);
            //int Team1PlayerA_ID, Team1PlayerB_ID, Team2PlayerA_ID, Team2PlayerB_ID;
            ////Add No shows to Handicaps and scores
            //scores.Add(0, Scoring.GetNoShowScores(courseInfo));
            //handicaps.Add(0, 0);

            //int index = 0;
            ////foreach matchup
            //foreach (int team1ID in matchups.Keys)
            //{
            //    Scoring.GetGolferIDs(team1ID, selectedEventID, subs, handicaps, scores, out Team1PlayerA_ID, out Team1PlayerB_ID);
            //    Scoring.GetGolferIDs(matchups[team1ID],selectedEventID , subs, handicaps, scores, out Team2PlayerA_ID, out Team2PlayerB_ID);
            //    GolferInfo golferInfo = GetGolferNames(Team1PlayerA_ID, Team2PlayerA_ID, Team1PlayerB_ID, Team2PlayerB_ID);
            //    golferInfo.Team1Name = teamNames[team1ID];
            //    golferInfo.Team2Name = teamNames[matchups[team1ID]];
            //    Scoring.MatchupResults results_A = Scoring.GetMatchupResults(scores[Team1PlayerA_ID], scores[Team2PlayerA_ID], handicaps[Team1PlayerA_ID], handicaps[Team2PlayerA_ID], courseInfo);
            //    Scoring.MatchupResults results_B = Scoring.GetMatchupResults(scores[Team1PlayerB_ID], scores[Team2PlayerB_ID], handicaps[Team1PlayerB_ID], handicaps[Team2PlayerB_ID], courseInfo);
                
            //    //Add handicaps to golferNames
            //    golferInfo.Aplayer1Hcp = handicaps[Team1PlayerA_ID];
            //    golferInfo.Aplayer2Hcp = handicaps[Team2PlayerA_ID];
            //    golferInfo.Bplayer1Hcp = handicaps[Team1PlayerB_ID];
            //    golferInfo.Bplayer2Hcp = handicaps[Team2PlayerB_ID];
            //    AddCollapsablePanel(index.ToString(), golferInfo, results_A, results_B, courseInfo);
            //    index++;
            //}

        }
    }

    private TableCell AddCell(TableRow row, string text)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.ForeColor = Color.Black;
        row.Cells.Add(cell);
        return cell;
    }
    
    private void AddCell(TableRow row, string text, bool bold)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.Font.Bold = bold;
        cell.ForeColor = Color.Black;
        cell.BorderStyle = BorderStyle.Solid;
        cell.BorderWidth = 1;
        row.Cells.Add(cell);
    }

    private void AddCell(TableRow row, string text, bool BlueBackGround, bool BirdieCircle)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.ForeColor = Color.Black;
        if (BlueBackGround)
        {
            cell.BackColor = Color.LightCoral;
        }
        if (BirdieCircle)
        {
            cell.Attributes.Add("background-image", "url(images/circle.gif)");
        }
        row.Cells.Add(cell);
    }

    private void AddCourseInfoRows(Table scoreCardTable, CourseInfo courseInfo)
    {
        //Top Row
        TableRow holeRow = new TableRow();
        holeRow.BackColor = Color.LightBlue;
        AddCell(holeRow, "Hole", true);
        for (int i = 1; i < 10; i++)
        {
            AddCell(holeRow, i.ToString(), true);
        }
        AddCell(holeRow, "", true);
        AddCell(holeRow, "", true);
        AddCell(holeRow, "", true);
        scoreCardTable.Rows.Add(holeRow);

        //Row 2
        TableRow lengthRow = new TableRow();
        AddCell(lengthRow, "Length", true);
        int totalLength = 0;
        for (int i = 0; i < 9; i++)
        {
            totalLength += courseInfo.holeLengths[i];
            AddCell(lengthRow, courseInfo.holeLengths[i].ToString(), false);
        }
        AddCell(lengthRow, totalLength.ToString(), true);
        AddCell(lengthRow, "", false);
        AddCell(lengthRow, "", false);
        scoreCardTable.Rows.Add(lengthRow);

        //Row 3
        TableRow parRow = new TableRow();
        AddCell(parRow, "Mens Par", true);
        int parTotal = 0;
        foreach(int parRating in courseInfo.holeParRatings)
        {
            parTotal += parRating;
            AddCell(parRow, parRating.ToString(), false);
        }
        AddCell(parRow, parTotal.ToString(), true);
        AddCell(parRow, "", false);
        AddCell(parRow, "", false);
        scoreCardTable.Rows.Add(parRow);

        //Row 4
        TableRow handicapRow = new TableRow();
        AddCell(handicapRow, "Handicap", true);
        handicapRow.BackColor = Color.LightGreen;
        
        foreach(int handicap in courseInfo.holeHandicaps)
        {
            AddCell(handicapRow, handicap.ToString(), false);
        }
        AddCell(handicapRow, "Gross Score", true);
        AddCell(handicapRow, "Net Score", true);
        AddCell(handicapRow, "Total Points", true);
        
        scoreCardTable.Rows.Add(handicapRow);
    }


    private void AddCourseInfoRowsSkinsTable(Table scoreCardTable, CourseInfo courseInfo)
    {
        //Top Row
        TableRow holeRow = new TableRow();
        holeRow.BackColor = Color.LightBlue;
        AddCell(holeRow, "Hole", true);
        for (int i = 1; i < 10; i++)
        {
            AddCell(holeRow, i.ToString(), true);
        }
        scoreCardTable.Rows.Add(holeRow);

        //Row 2
        TableRow lengthRow = new TableRow();
        AddCell(lengthRow, "Length", true);
        int totalLength = 0;
        for (int i = 0; i < 9; i++)
        {
            totalLength += courseInfo.holeLengths[i];
            AddCell(lengthRow, courseInfo.holeLengths[i].ToString(), false);
        }
        scoreCardTable.Rows.Add(lengthRow);

        //Row 3
        TableRow parRow = new TableRow();
        AddCell(parRow, "Mens Par", true);
        int parTotal = 0;
        foreach (int parRating in courseInfo.holeParRatings)
        {
            parTotal += parRating;
            AddCell(parRow, parRating.ToString(), false);
        }
        scoreCardTable.Rows.Add(parRow);

        //Row 4
        TableRow handicapRow = new TableRow();
        AddCell(handicapRow, "Handicap", true);
        handicapRow.BackColor = Color.LightGreen;

        foreach (int handicap in courseInfo.holeHandicaps)
        {
            AddCell(handicapRow, handicap.ToString(), false);
        }
        scoreCardTable.Rows.Add(handicapRow);
    }

    private void AddGolferScores(Table scoreCardTable, GolferInfo golferInfo, Scoring.MatchupResults resultsA, Scoring.MatchupResults resultsB, CourseInfo courseInfo)
    {
        //Row 5
        TableRow team1APlayerRow = new TableRow();
        AddCell(team1APlayerRow, golferInfo.Aplayer1Name + " (" + golferInfo.Aplayer1Hcp.ToString() + ")", true);
        TableRow team2APlayerRow = new TableRow();
        AddCell(team2APlayerRow, golferInfo.Aplayer2Name + " (" + golferInfo.Aplayer2Hcp.ToString() + ")", true);
        TableRow team1BPlayerRow = new TableRow();
        AddCell(team1BPlayerRow, golferInfo.Bplayer1Name + " (" + golferInfo.Bplayer1Hcp.ToString() + ")", true);
        TableRow team2BPlayerRow = new TableRow();
        AddCell(team2BPlayerRow, golferInfo.Bplayer2Name + " (" + golferInfo.Bplayer2Hcp.ToString() + ")", true);
       
        for (int i = 0; i < 9; i++)
        {
            AddCell(team1APlayerRow, resultsA.player1Scores[i].ToString(), resultsA.whoWonHole[i] == 1, courseInfo.holeParRatings[i] > (int)resultsA.player1Scores[i]);
            AddCell(team2APlayerRow, resultsA.player2Scores[i].ToString(), resultsA.whoWonHole[i] == 2, courseInfo.holeParRatings[i] > (int)resultsA.player2Scores[i]);
            AddCell(team1BPlayerRow, resultsB.player1Scores[i].ToString(), resultsB.whoWonHole[i] == 1, courseInfo.holeParRatings[i] > (int)resultsB.player1Scores[i]);
            AddCell(team2BPlayerRow, resultsB.player2Scores[i].ToString(), resultsB.whoWonHole[i] == 2, courseInfo.holeParRatings[i] > (int)resultsB.player2Scores[i]);
        }
        AddCell(team1APlayerRow, resultsA.grossScorePlayer1.ToString(), true);
        AddCell(team2APlayerRow, resultsA.grossScorePlayer2.ToString(), true);
        AddCell(team1BPlayerRow, resultsB.grossScorePlayer1.ToString(), true);
        AddCell(team2BPlayerRow, resultsB.grossScorePlayer2.ToString(), true);

        AddCell(team1APlayerRow, resultsA.netScorePlayer1.ToString(), true);
        AddCell(team2APlayerRow, resultsA.netScorePlayer2.ToString(), true);
        AddCell(team1BPlayerRow, resultsB.netScorePlayer1.ToString(), true);
        AddCell(team2BPlayerRow, resultsB.netScorePlayer2.ToString(), true);

        AddCell(team1APlayerRow, resultsA.totalPtsPlayer1.ToString(), true);
        AddCell(team2APlayerRow, resultsA.totalPtsPlayer2.ToString(), true);
        AddCell(team1BPlayerRow, resultsB.totalPtsPlayer1.ToString(), true);
        AddCell(team2BPlayerRow, resultsB.totalPtsPlayer2.ToString(), true);

        scoreCardTable.Rows.Add(team1APlayerRow);
        scoreCardTable.Rows.Add(team2APlayerRow);
        scoreCardTable.Rows.Add(team1BPlayerRow);
        scoreCardTable.Rows.Add(team2BPlayerRow);

    }




    private string[] CreateTitleLabelTextForMatchup(GolferInfo golferNames, Scoring.MatchupResults resultsA, Scoring.MatchupResults resultsB, out Boolean APlayersTied, out Boolean BPlayersTied)
    {
        List<string> retVal = new List<string>();
        string aPlayerString, bPlayerString;
        APlayersTied = false;
        BPlayersTied = false;
        if(resultsA.netScorePlayer1 <= resultsA.netScorePlayer2)
        {
            aPlayerString = golferNames.Aplayer1Name + " (" + resultsA.grossScorePlayer1 + ") vs. " + golferNames.Aplayer2Name + " (" + resultsA.grossScorePlayer2 + ") - ";
            if (resultsA.netScorePlayer1 == resultsA.netScorePlayer2)
            {
                APlayersTied = true;
            }
        }
        else 
        {
            aPlayerString = golferNames.Aplayer2Name + " (" + resultsA.grossScorePlayer2 + ") vs. " + golferNames.Aplayer1Name + " (" + resultsA.grossScorePlayer1 + ") - ";
        }

        if (resultsB.netScorePlayer1 <= resultsB.netScorePlayer2)
        {
            bPlayerString = golferNames.Bplayer1Name + " (" + resultsB.grossScorePlayer1 + ") vs. " + golferNames.Bplayer2Name + " (" + resultsB.grossScorePlayer2 + ") - ";
            if (resultsB.netScorePlayer1 == resultsB.netScorePlayer2)
            {
                APlayersTied = true;
            }
        }
        else
        {
            bPlayerString = golferNames.Bplayer2Name + " (" + resultsB.grossScorePlayer2 + ") vs. " + golferNames.Bplayer1Name + " (" + resultsB.grossScorePlayer1 + ") - ";
        }

        retVal.Add(aPlayerString);
        retVal.Add(bPlayerString);

        return retVal.ToArray();
    }

    private Label CreateTitleLable(string text)
    {
        Label lblHead1 = new Label();
        lblHead1.ForeColor = Color.White;
        lblHead1.Font.Size = 16;
        lblHead1.Text = text;
        return lblHead1;
    }

    private System.Web.UI.WebControls.Image AddTrophy()
    {
        System.Web.UI.WebControls.Image trophy1 = new System.Web.UI.WebControls.Image();
        trophy1.ImageUrl = "~/images/trophy4.png";
        trophy1.Height = new Unit(30, UnitType.Pixel);
        trophy1.Width = new Unit(26, UnitType.Pixel);
        return trophy1;
    }

    private void AddCollapsablePanel(string index, GolferInfo golferNames, Scoring.MatchupResults resultsA, Scoring.MatchupResults resultsB, CourseInfo courseInfo)
    {
        Table scoreCardTable = new Table();

        AddCourseInfoRows(scoreCardTable, courseInfo);
        AddGolferScores(scoreCardTable, golferNames, resultsA, resultsB, courseInfo);

        decimal team1totalPts = 0, team2totalPts = 0;
        team1totalPts = resultsA.totalPtsPlayer1 + resultsB.totalPtsPlayer1;
        team2totalPts = resultsA.totalPtsPlayer2 + resultsB.totalPtsPlayer2;
        string medalPlayWinner = "";
        if ((resultsA.netScorePlayer1 + resultsB.netScorePlayer1) < (resultsA.netScorePlayer2 + resultsB.netScorePlayer2))
        {
            medalPlayWinner = golferNames.Team1Name + "  (" + (resultsA.netScorePlayer1 + resultsB.netScorePlayer1).ToString() + " to " + (resultsA.netScorePlayer2 + resultsB.netScorePlayer2).ToString() + ")"; 
            team1totalPts += 2;
        }
        else if ((resultsA.netScorePlayer1 + resultsB.netScorePlayer1) > (resultsA.netScorePlayer2 + resultsB.netScorePlayer2))
        {
            medalPlayWinner = golferNames.Team2Name + "  (" + (resultsA.netScorePlayer2 + resultsB.netScorePlayer2).ToString() + " to " + (resultsA.netScorePlayer1 + resultsB.netScorePlayer1).ToString() + ")"; 
             team2totalPts += 2;
        }
        else
        {
            team1totalPts += 1;
            team2totalPts += 1;
            medalPlayWinner = "Teams Tied";
        }
        //Add Row for Medal Play
        TableRow row = new TableRow();
        row.BorderStyle = BorderStyle.Solid;
        TableCell mpCell1 = AddCell(row, "Medal Play Winner (2pts): ");
        TableCell mpCell2 = AddCell(row, medalPlayWinner);
        mpCell1.ColumnSpan = 3;
        mpCell2.ColumnSpan = 10;
        mpCell1.Font.Size = 14;
        mpCell2.Font.Size = 14;
        mpCell1.Font.Bold = true;
        //medalPlayCell.BorderStyle = BorderStyle.Solid;
        scoreCardTable.Rows.Add(row);

        TableRow total_pts_row = new TableRow();
        total_pts_row.BorderStyle = BorderStyle.Solid;
        TableCell c1 = AddCell(total_pts_row, "Total Pts: ");
        c1.ColumnSpan = 1;
        c1.HorizontalAlign = HorizontalAlign.Center;
        c1.Font.Size = 14;
        c1.Font.Bold = true;
        TableCell c2 = AddCell(total_pts_row, golferNames.Team1Name + " " + team1totalPts.ToString() + " -----");
        c2.ColumnSpan = 8;
        c2.HorizontalAlign = HorizontalAlign.Center;
        c2.Font.Size = 14;
        //c2.BorderStyle = BorderStyle.Solid;
        
        TableCell c3 = AddCell(total_pts_row, golferNames.Team2Name + " " + team2totalPts.ToString());
        c3.ColumnSpan = 4;
        c3.HorizontalAlign = HorizontalAlign.Center;
        c3.Font.Size = 14;
        //c3.BorderStyle = BorderStyle.Solid;
        scoreCardTable.Rows.Add(total_pts_row);

        //Boolean AplayersTied, BplayersTied;
        //string[] titleLabelText = CreateTitleLabelTextForMatchup(golferNames, resultsA, resultsB, out AplayersTied, out BplayersTied);


        // Create Header Panel
        Panel panelHead = new Panel();
        panelHead.ID = "pH" + index;
        panelHead.BackImageUrl = "~/images/bg-menu-main.png";


        if (resultsA.totalPtsPlayer1 > resultsA.totalPtsPlayer2)
        {
            panelHead.Controls.Add(AddTrophy());
        }
        string text = golferNames.Aplayer1Name + " (" + resultsA.grossScorePlayer1 + ") vs. ";
        panelHead.Controls.Add(CreateTitleLable(text));

        if (resultsA.totalPtsPlayer2 > resultsA.totalPtsPlayer1)
        {
            panelHead.Controls.Add(AddTrophy());
        }
        text = golferNames.Aplayer2Name + " (" + resultsA.grossScorePlayer2 + ") ----- ";
        panelHead.Controls.Add(CreateTitleLable(text));


        if (resultsB.totalPtsPlayer1 > resultsB.totalPtsPlayer2)
        {
            panelHead.Controls.Add(AddTrophy());
        }
        text = golferNames.Bplayer1Name + " (" + resultsB.grossScorePlayer1 + ") vs. ";
        panelHead.Controls.Add(CreateTitleLable(text));

        if (resultsB.totalPtsPlayer2 > resultsB.totalPtsPlayer1)
        {
            panelHead.Controls.Add(AddTrophy());
        }
        text = golferNames.Bplayer2Name + " (" + resultsB.grossScorePlayer2 + ")";
        panelHead.Controls.Add(CreateTitleLable(text));


        // Add Label inside header panel to display text
        //Label lblHead1 = new Label();
        //lblHead1.ForeColor = Color.White;
        //lblHead1.ID = "lblHeader" + index + "_1";
        //lblHead1.Font.Size = 16;
        //lblHead1.Text = titleLabelText[0];

        //Label lblHead2 = new Label();
        //lblHead2.ForeColor = Color.White;
        //lblHead2.Font.Size = 16;
        //lblHead2.ID = "lblHeader" + index + "_2";
        //lblHead2.Text = titleLabelText[1];

        //if (!AplayersTied)//if they didnt tie then the winning player is listed first, insert trophy
        //{
        //    System.Web.UI.WebControls.Image trophy1 = new System.Web.UI.WebControls.Image();
        //    trophy1.ImageUrl = "~/images/trophy4.png";
           
        //    //trophy1.Height = new Unit(30, UnitType.Pixel);
        //    //trophy1.Width = new Unit(20, UnitType.Pixel);
        //    //trophy1.Width = new Unit(51, UnitType.Pixel);
        //    panelHead.Controls.Add(trophy1);
        //}
        //panelHead.Controls.Add(lblHead1);

        //if (!BplayersTied)//if they didnt tie then the winning player is listed first, insert trophy
        //{
        //    System.Web.UI.WebControls.Image trophy2 = new System.Web.UI.WebControls.Image();
        //    trophy2.ImageUrl = "~/images/trophy4.png";
        //    //trophy2.Height = new Unit(60, UnitType.Pixel);
        //    panelHead.Controls.Add(trophy2);
        //}
        //panelHead.Controls.Add(lblHead2);


        System.Web.UI.WebControls.Image expandButton = new System.Web.UI.WebControls.Image();
        expandButton.ID = "expandButton"+ index;
        expandButton.ImageAlign = ImageAlign.Right;
        panelHead.Controls.Add(expandButton);
        panelHead.HorizontalAlign = HorizontalAlign.Left;
        

        //Create Body Panel
        Panel panelBody = new Panel();
        panelBody.ID = "pB" + index;
        
        // Add Label inside body Panel to display text
        //Label lblB = new Label();
        //lblB.ID = "lblBody" + index;
        //lblB.Text = "This panel was added dynamically";
        panelBody.Controls.Add(scoreCardTable);

        // Create CollapsiblePanelExtender
        CollapsiblePanelExtender cpe = new CollapsiblePanelExtender();
        cpe.ID = "cpe_" + index;
        cpe.TargetControlID = panelBody.ID;
        cpe.ExpandControlID = panelHead.ID;
        cpe.CollapseControlID = panelHead.ID;
        cpe.ScrollContents = false;
        cpe.Collapsed = true;
        cpe.ExpandDirection = CollapsiblePanelExpandDirection.Vertical;
        cpe.SuppressPostBack = true;
        //cpe.TextLabelID = lblHead.ID;
        cpe.ExpandedImage="~/images/collapse_blue.jpg";
        cpe.CollapsedImage = "~/images/expand_blue.jpg";
        cpe.ImageControlID = expandButton.ID;
        //cpe.CollapsedText = "Click to Show Content..";
        //cpe.ExpandedText = "";

        this.Panel_Matchups.ContentTemplateContainer.Controls.Add(panelHead);
        this.Panel_Matchups.ContentTemplateContainer.Controls.Add(panelBody);
        this.Panel_Matchups.ContentTemplateContainer.Controls.Add(cpe);
        this.Panel_Matchups.ContentTemplateContainer.Controls.Add(new LiteralControl("&nbsp"));
    }
}