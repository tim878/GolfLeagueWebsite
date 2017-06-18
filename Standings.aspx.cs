using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Data;

public partial class Standings : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;
    private bool toggle;

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        Dictionary<int, string> allTeamNames = DatabaseFunctions.GetTeamNames(leagueID);
        Dictionary<int, string> teamNames = new Dictionary<int, string>();
        List<int> activeTeams = DatabaseFunctions.GetTeamsActiveInSeason(leagueID, currentSeasonID);

        foreach (int teamID in activeTeams)
        {
            teamNames.Add(teamID, allTeamNames[teamID]);
        }

        Dictionary<int, EventInfo> events = DatabaseFunctions.GetEventsWithScoresPosted(leagueID, currentSeasonID);

        Dictionary<int, decimal> teamTotalScores = new Dictionary<int, decimal>();
        Dictionary<int, Dictionary<int, decimal>> weekByWeekScores = new Dictionary<int, Dictionary<int, decimal>>();

        var settings = DatabaseFunctions.GetLeagueSettings(leagueID);
        bool useCurrentRound = settings.ContainsKey("UseCurrentRoundForHandicap") ? bool.Parse(settings["UseCurrentRoundForHandicap"]) : true;

        Dictionary<int, Dictionary<int, int>> handicaps = Scoring.GetHandicaps(leagueID, useCurrentRound);

        ChartWeeklyStandings.ImageStorageMode = ImageStorageMode.UseImageLocation;
        
        ChartWeeklyStandings.Legends[0].Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
        ChartWeeklyStandings.Palette = ChartColorPalette.Bright;
        ChartWeeklyStandings.ChartAreas[0].AxisY.Maximum = 5;

        foreach (KeyValuePair<int, EventInfo> eventPair in events.OrderBy(x => x.Value.Date))
        {
            Dictionary<int, int> handicapsForEvent = Scoring.GetHandicapsForEventFromHandicapByGolferIDDictionary(handicaps, eventPair.Key);
            Dictionary<int, decimal> eventPoints = Scoring.GetEventResults(leagueID, eventPair.Key, handicapsForEvent).teamPts;
            weekByWeekScores.Add(eventPair.Key, eventPoints);
            foreach(int teamID in eventPoints.Keys)
            {
                if (teamTotalScores.ContainsKey(teamID))
                {
                    teamTotalScores[teamID] += eventPoints[teamID];
                }
                else
                {
                    teamTotalScores.Add(teamID, eventPoints[teamID]);
                }
            }
        }

        BuildStandingsTable(teamTotalScores, teamNames);
        BuildChart(weekByWeekScores, teamNames, events);
        BuildWeeklyResultsTable(weekByWeekScores, teamNames, events);
    }

    private void BuildChart(Dictionary<int, Dictionary<int, decimal>> weekByWeekScores, Dictionary<int, string> teamNames, Dictionary<int, EventInfo> events)
    {
        //Dictionary<int, Series> teamPointsSeries = new Dictionary<int, Series>();

        Dictionary<int, Dictionary<int, decimal>> weekByWeekTotalScores = new Dictionary<int, Dictionary<int, decimal>>();

        foreach (int eventID in weekByWeekScores.Keys)
        //foreach (int TeamID in teamNames.Keys)
        {            
            weekByWeekTotalScores[eventID] = new Dictionary<int, decimal>();
        }

        foreach (int TeamID in teamNames. Keys)
        {
            decimal totalScore = 0;
            foreach (int eventID in weekByWeekScores.Keys)
            {
                totalScore +=  weekByWeekScores[eventID].ContainsKey(TeamID)?  weekByWeekScores[eventID][TeamID] : 0;
                weekByWeekTotalScores[eventID][TeamID] = totalScore;
            }
        }

        foreach(int TeamID in teamNames.Keys)
        {
            Series series = new Series(teamNames[TeamID]);
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.String;
            series.BorderWidth = 6;
            try
            {
                ChartWeeklyStandings.Series.Add(series);
            }
            catch { }
        }

        int weekIndex = 1;
        foreach (int eventID in weekByWeekScores.Keys)
        {
            var Scores = from pair in weekByWeekTotalScores[eventID].Values
                        orderby pair descending
                        select pair;

            decimal currentHighScore = Scores.First();

            foreach (int TeamID in teamNames.Keys)
            {
                DataPoint point = new DataPoint();
                point.SetValueXY(weekIndex , weekByWeekTotalScores[eventID][TeamID] - currentHighScore);
                ChartWeeklyStandings.Series[teamNames[TeamID]].Points.Add(point);
            }
            weekIndex++;
        }


        //foreach(int TeamID in teamNames.Keys)
        //{
        //    Series series = new Series(teamNames[TeamID]);
        //    series.ChartType = SeriesChartType.Line;
        //    series.XValueType = ChartValueType.String;
        //    //teamPointsSeries.Add(TeamID, series);

        //    decimal totalScore = 0;
        //    foreach (int eventID in weekByWeekScores.Keys)
        //    {
        //        totalScore += weekByWeekScores[eventID][TeamID];
        //        DataPoint point = new DataPoint();
        //        point.SetValueXY(events[eventID].Date, totalScore);
        //        series.Points.Add(point);
        //    }
        //    ChartWeeklyStandings.Series.Add(series);
        //}

    }

    private void BuildWeeklyResultsTable(Dictionary<int, Dictionary<int, decimal>> weekByWeekScores, Dictionary<int, string> teamNames, Dictionary<int, EventInfo> events)
    {
        //iterate through event names and add column header
        TableRow titleRow = new TableRow();
        titleRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#465c71");
        AddTitleCell(titleRow, "Team Name", 12);
        int index = 1;
        foreach (int eventID in weekByWeekScores.Keys)
        {
            AddTitleCell(titleRow, "Week " + index, 12);
            index++;
        }

        Table_WeeklyScores.Rows.Add(titleRow);

        //for each team add row 
        foreach (int teamID in teamNames.Keys)
        {
            TableRow teamrow = AddRow(Table_WeeklyScores);
            AddCell(teamrow, teamNames[teamID], 10);
            foreach (int eventID in weekByWeekScores.Keys)
            {
                if (weekByWeekScores[eventID].ContainsKey(teamID))
                {
                    AddCell(teamrow, weekByWeekScores[eventID][teamID].ToString("0.#"), 10);
                }
                else
                {
                    AddCell(teamrow, "0", 10);
                }
            }
        }

    }




    private void BuildStandingsTable(Dictionary<int, decimal> teamTotalScores, Dictionary<int, string> teamNames)
    {
        Table topLevelTable = new Table();
        topLevelTable.BorderStyle = BorderStyle.Solid;
        topLevelTable.Width = 1000;
        //Add header Row
        TableRow row = new TableRow();
        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#465c71");
        AddTitleCell(row, "Position");
        AddTitleCell(row, "Team");
        AddTitleCell(row, "Points");
        topLevelTable.Rows.Add(row);

        decimal currentHighScore;
        List<int> teamsWithHighestPoints = new List<int>();
        int numberOfTeams = teamTotalScores.Keys.Count;
        for (int i = 0; i < numberOfTeams; i++)
        {
            currentHighScore = 0;
            teamsWithHighestPoints.Clear();
            foreach (int teamID in teamTotalScores.Keys)
            {
                if (teamTotalScores[teamID] > currentHighScore)
                {
                    currentHighScore = teamTotalScores[teamID];
                    teamsWithHighestPoints.Clear();
                    teamsWithHighestPoints.Add(teamID);
                }
                else if (teamTotalScores[teamID] == currentHighScore)
                {
                    teamsWithHighestPoints.Add(teamID);
                }
            }
            int position = topLevelTable.Rows.Count;
            foreach (int teamID in teamsWithHighestPoints)
            {
                string positionString;
                if (teamsWithHighestPoints.Count > 1)
                {
                    positionString = "T" + position.ToString();
                }
                else
                {
                    positionString = position.ToString();
                }
                AddRow(teamNames[teamID], positionString, teamTotalScores[teamID], topLevelTable);
                teamTotalScores.Remove(teamID);
            }

        }
        Panel_Standings.Controls.Add(topLevelTable);
    }


    private TableRow AddRow(Table topLevelTable)
    {
        TableRow row = new TableRow();
        if (toggle)
        {
            row.BackColor = Color.WhiteSmoke;
        }
        toggle = !toggle;
        topLevelTable.Rows.Add(row);
        return row;
    }

    private void AddRow(string TeamName, string Position, decimal TeamPoints, Table topLevelTable)
    {
        TableRow row = new TableRow();
        AddCell(row, Position);
        AddCell(row, TeamName);
        AddCell(row, TeamPoints.ToString("0.#"));
        if (toggle)
        {
            row.BackColor = Color.WhiteSmoke;
        }
        toggle = !toggle;
        topLevelTable.Rows.Add(row);
    }

    private TableCell AddCell(TableRow row, string text)
    {
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Font.Size = 14;
        cell.Text = text;
        cell.ForeColor = Color.Black;
        row.Cells.Add(cell);
        return cell;
    }

    private TableCell AddCell(TableRow row, string text, int fontSize)
    {
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Font.Size = fontSize;
        cell.Text = text;
        cell.ForeColor = Color.Black;
        row.Cells.Add(cell);
        return cell;
    }

    private TableCell AddTitleCell(TableRow row, string text)
    {
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = text;
        cell.Font.Size = 20;
        cell.Font.Bold = false;
        cell.ForeColor = Color.White;
        cell.BorderStyle = BorderStyle.Solid;
        row.Cells.Add(cell);
        return cell;
    }

    private TableCell AddTitleCell(TableRow row, string text, int fontSize)
    {
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = text;
        cell.Font.Size = fontSize;
        cell.Font.Bold = false;
        cell.ForeColor = Color.White;
        cell.BorderStyle = BorderStyle.Solid;
        row.Cells.Add(cell);
        return cell;
    }

    
}