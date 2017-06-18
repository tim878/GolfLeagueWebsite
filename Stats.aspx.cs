using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;


public partial class Stats : System.Web.UI.Page
{
    private int leagueID;
    private bool toggle = false;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        //Menu test  = ((Menu)((SiteMaster)this.Master).FindControl("NavigationMenu"));
        //test.FindItem("Stats").Selected = true;

        int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());

        if (!Page.IsPostBack)
        {
            currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
            List<Season> seasons = DatabaseFunctions.GetSeasons(leagueID);
            List<int> CourseIDs = DatabaseFunctions.GetCourseIDs(leagueID);

            //Populate Dropdowns
            ListItem newItem = new ListItem("All", "0");
            Dropdown_Seasons.Items.Add(newItem);
           
            foreach (Season season in seasons)
            {
                newItem = new ListItem(season.SeasonName, season.SeasonID.ToString());
                Dropdown_Seasons.Items.Add(newItem);              
            }

            int selectedSeasonValue = Session["StatsSeasonDropdownValue"] == null ? 0 : int.Parse((string)Session["StatsSeasonDropdownValue"]);
            Dropdown_Seasons.SelectedValue = selectedSeasonValue.ToString(); ;

            Dictionary<int, string> golfers = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());
            newItem = new ListItem("Select Player", "0");
            DropDown_PlayerSelect.Items.Add(newItem);
           
            foreach (int golferID in golfers.Keys)
            {
                newItem = new ListItem(golfers[golferID], golferID.ToString());
                DropDown_PlayerSelect.Items.Add(newItem);   
            }

            int selectedPlayerValue = Session["StatsPlayerDropdownValue"] == null ? 0 : int.Parse((string)Session["StatsPlayerDropdownValue"]);
            DropDown_PlayerSelect.SelectedValue = selectedPlayerValue.ToString();

            //UpdateContent();
            //Dropdown_Season_SelectedIndexChanged(null, null);
            Scoring.LeagueStats LeagueStats = null;
            if (Session["LeagueStats"] == null)
            {
                LeagueStats = Scoring.GetSeasonStats(leagueID, 0, CourseIDs);
                Session["LeagueStats"] = LeagueStats;
            }
            else
            {
                LeagueStats = (Scoring.LeagueStats)Session["LeagueStats"];
            }

            UpdateContent(LeagueStats, CourseIDs, leagueID);
        }
        showCurrentlySelectedView();
        
    }

    private void showCurrentlySelectedView()
    {
        if (Session["SelectedStatView"] == null)
        {
            return;
        }

        if (Session["SelectedStatView"] == "AvgAll")
        {
            NavigationMenu.FindItem("AvgAll").Selected = true; 
            Table_AverageScores.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "LowNet")
        {
            //Table_LowestNetSingleRoundScores.Style.Remove("display");
            NavigationMenu.FindItem("LowNet").Selected = true; 
            Table_LowestNetSingleRoundScores.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "LowGross")
        {
            NavigationMenu.FindItem("LowGross").Selected = true; 
            Table_LowestGrossSingleRoundScores.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "AvgByCourse")
        {
            NavigationMenu.FindItem("AvgByCourse").Selected = true;
            Panel_AverageScoresByCourse.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "IndvPts")
        {
            //Table_.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Skins")
        {
            //Table_Skins.Style.Clear();
            NavigationMenu.FindItem("Skins").Selected = true; 
            Table_Skins.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Birdies")
        {
            NavigationMenu.FindItem("Birdies").Selected = true; 
            Table_Birdies.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Pars")
        {
            NavigationMenu.FindItem("Pars").Selected = true; 
            Table_Pars.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Bogeys")
        {
            NavigationMenu.FindItem("Bogeys").Selected = true; 
            Table_Bogeys.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "DoubleBogeys")
        {
            NavigationMenu.FindItem("DoubleBogeys").Selected = true; 
            Table_DoubleBogeys.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Eagles")
        {
            NavigationMenu.FindItem("Eagles").Selected = true; 
            Table_Eagles.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Par3")
        {
            NavigationMenu.FindItem("Par3").Selected = true; 
            Table_Par3Scoring.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Par4")
        {
            NavigationMenu.FindItem("Par4").Selected = true; 
            Table_Par4Scoring.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Par5")
        {
            NavigationMenu.FindItem("Par5").Selected = true; 
            Table_Par5Scoring.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Handicaps")
        {
            NavigationMenu.FindItem("Handicaps").Selected = true;
            Table_Handicaps.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "CourseStats")
        {
            IndividualStatsMenu.FindItem("CourseStats").Selected = true;
            CourseStats.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "Graph")
        {
            IndividualStatsMenu.FindItem("Graph").Selected = true;
            Graph.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "EventScores")
        {
            IndividualStatsMenu.FindItem("EventScores").Selected = true;
            Panel_IndividualScores.Style.Add("display", "block");
        }
        else if (Session["SelectedStatView"] == "IndividualHoles")
        {
            NavigationMenu.FindItem("IndividualHoles").Selected = true;
            Panel_HoleScores.Style.Add("display", "block");
        }

    }

    private void BuildHighestTotalTable(Table table, string title, Dictionary<int, int> data, Dictionary<int, int> numberOfRoundsPlayed)
    {
        table.CellSpacing = 20;
        TableRow tableTitleRow = new TableRow();
        TableCell titleCell = AddCell(tableTitleRow, title);
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleCell.Font.Bold = true;
        titleCell.Font.Size = 16;
        titleCell.ColumnSpan = 3;
        tableTitleRow.Cells.Add(titleCell);
        table.Rows.Add(tableTitleRow);

        table.Rows.Add(CreateHighestTotalColumnTitleRow());

        var items = from pair in data
                    orderby pair.Value descending
                    select pair;

        foreach (KeyValuePair<int, int> pair in items)
        {
            if (numberOfRoundsPlayed[pair.Key] > 0)
            {
                table.Rows.Add(CreateRow(new List<string>() { DatabaseFunctions.GetGolferName(pair.Key), Decimal.Round(data[pair.Key], 2).ToString(), numberOfRoundsPlayed[pair.Key].ToString() }));
            }
        }
    }


    private void BuildAvgScoreTable(Table table, string title, Dictionary<int, Scoring.WeightedAvgHelper> data, int titleFont, string thirdColumnTitle)
    {
        table.CellSpacing = 5;
        TableRow tableTitleRow = new TableRow();
        TableCell titleCell = AddCell(tableTitleRow, title);
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleCell.Font.Bold = true;
        titleCell.Font.Size = titleFont;
        titleCell.ColumnSpan = 3;
        tableTitleRow.Cells.Add(titleCell);
        table.Rows.Add(tableTitleRow);

        table.Rows.Add(CreateAvgScoreColumnTitleRow(thirdColumnTitle));

        var items = from pair in data
                    orderby pair.Value.averageValue ascending
                    select pair;

        foreach (KeyValuePair<int, Scoring.WeightedAvgHelper> pair in items)
        {
            if (pair.Value.numValues > 0)
            {
                table.Rows.Add(CreateRow(new List<string>() { DatabaseFunctions.GetGolferName(pair.Key), Decimal.Round(pair.Value.averageValue, 2).ToString(), pair.Value.numValues.ToString() }));
            }
        }
    }

    private void BuildAvgScoreTable(Table table, string title, Dictionary<int, decimal> data, Dictionary<int, int> numberOfRoundsPlayed, int titleFont)
    {
        table.CellSpacing = 5;
        TableRow tableTitleRow = new TableRow();
        TableCell titleCell = AddCell(tableTitleRow, title);
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleCell.Font.Bold = true;
        titleCell.Font.Size = titleFont;
        titleCell.ColumnSpan = 3;
        tableTitleRow.Cells.Add(titleCell);
        table.Rows.Add(tableTitleRow);

        table.Rows.Add(CreateAvgScoreColumnTitleRow("Number of Rounds"));

        var items = from pair in data
                    orderby pair.Value ascending
                    select pair;

        foreach (KeyValuePair<int, decimal> pair in items)
        {
            if (numberOfRoundsPlayed[pair.Key] > 0)
            {
                table.Rows.Add(CreateRow(new List<string>() { DatabaseFunctions.GetGolferName(pair.Key), Decimal.Round(data[pair.Key], 2).ToString(), numberOfRoundsPlayed[pair.Key].ToString() }));
            }
        }
    }

    private void BuildBestScoreTable(Table table, string title , Dictionary<Scoring.RoundInfo, int> data, int numberOfRows)
    {
        table.CellSpacing = 5;
        TableRow tableTitleRow = new TableRow();
        TableCell titleCell = AddCell(tableTitleRow, title);
        titleCell.ColumnSpan = 4;
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleCell.Font.Bold = true;
        titleCell.Font.Size = 16;
        tableTitleRow.Cells.Add(titleCell);
        table.Rows.Add(tableTitleRow);

        table.Rows.Add(CreateBestScoreColumnTitleRow());

        var items = from pair in data
                    orderby pair.Value ascending
                    select pair;

        int i = 0;
        foreach (KeyValuePair<Scoring.RoundInfo, int> pair in items)
        {
            if (i < numberOfRows)
            {
                table.Rows.Add(CreateRow(new List<string>() { DatabaseFunctions.GetGolferName(pair.Key.GolferID), DatabaseFunctions.GetCourseInfoFromCourseID(pair.Key.CourseID).name, ((DateTime)DatabaseFunctions.GetEventDate(pair.Key.LeagueEventID)).ToShortDateString(), pair.Value.ToString() })); 
                //table.Rows.Add(CreateRow(new List<string>() { DatabaseFunctions.GetGolferName(pair.Key), Decimal.Round(data[pair.Key], 2).ToString(), numberOfRoundsPlayed[pair.Key].ToString() }));
            }
            else
            {
                break;
            }
            i++;
        }

        //foreach(Scoring.RoundInfo roundInfo in data.Keys)
        //{
        //    table.Rows.Add(CreateRow(new List<string>() {data[roundInfo].ToString(), DatabaseFunctions.GetGolferName(roundInfo.GolferID), DatabaseFunctions.GetCourseInfoFromCourseID(roundInfo.CourseID).name, DatabaseFunctions.GetEventDate(roundInfo.LeagueEventID).ToShortDateString()}));   
        //}
    }

    private TableRow CreateRow(List<string> columnTexts)
    {
        TableRow tr = new TableRow();

        bool firstRow = true;
        foreach (string columnText in columnTexts)
        {
            if (toggle)
            {
                tr.BackColor = Color.WhiteSmoke;
            }
            TableCell cell = AddCell(tr, columnText);
            if (firstRow)
            {
                cell.HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                cell.HorizontalAlign = HorizontalAlign.Center;
            }
            firstRow = false;
        }
        toggle = !toggle;
       
        return tr;
    }


    private TableRow CreateHighestTotalColumnTitleRow()
    {
        TableRow tr = new TableRow();

        TableCell cell = AddCell(tr, "Golfer Name");
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Total");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Number of Rounds");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        return tr;
    }

    private TableRow CreateAvgScoreColumnTitleRow(string thirdColumnTitle)
    {
        TableRow tr = new TableRow();

        TableCell cell = AddCell(tr, "Golfer Name");
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Average");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        cell = AddCell(tr, thirdColumnTitle);
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        return tr;
    }

    private TableRow CreateBestScoreColumnTitleRow()
    {
        TableRow tr = new TableRow();
       
        TableCell cell = AddCell(tr, "Golfer Name");
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Course");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Date");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        cell = AddCell(tr, "Score");
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.Font.Bold = true;

        return tr;
    }

    private void AddScoreCardRows(Table ScoreCardTable, List<Decimal> AverageScoresByHole, List<byte> RingerNineByHole, List<byte> WorstScoresOnEachHole)
    {
        AddScoreCardRow(ScoreCardTable, "League Average", roundDecimalList(AverageScoresByHole));
        AddScoreCardRow(ScoreCardTable, "Ringer Nine", RingerNineByHole);
        AddScoreCardRow(ScoreCardTable, "Worst Scores", WorstScoresOnEachHole);
    }

    private List<decimal> roundDecimalList(List<decimal> decimalList)
    {
        List<decimal> retval = new List<decimal>();
        foreach (decimal d in decimalList)
        {
            retval.Add(Decimal.Round(d, 2));
        }
        return retval;
    }

    private List<byte> listOfIntToByte(List<int> intList)
    {
        List<byte> retval = new List<byte>();
        foreach (int i in intList)
        {
            retval.Add((byte)i);
        }
        return retval;
    }


    private void AddScoreCardRow(Table ScoreCardTable, string title, List<Decimal> values)
    {
        //Top Row
        TableRow row = new TableRow();
        if (toggle)
        {
            row.BackColor = Color.Wheat;
        }
        toggle = !toggle;

        AddCell(row, title, true, 14);
        decimal total = 0;
        foreach(decimal currentValue in values)
        {
            AddCell(row, currentValue.ToString(), false, 14);
            total += currentValue;
        }
        AddCell(row, total.ToString(), true, 14);
        ScoreCardTable.Rows.Add(row);
    }

    private void AddScoreCardRow(Table ScoreCardTable, string title, List<byte> values)
    {
        //Top Row
        TableRow row = new TableRow();
        if (toggle)
        {
            row.BackColor = Color.Wheat;
        }
        toggle = !toggle;

        AddCell(row, title, true, 14);
        decimal total = 0;
        foreach (byte currentValue in values)
        {
            AddCell(row, currentValue.ToString(), false, 14);
            total += currentValue;
        }
        AddCell(row, total.ToString(), true, 14);
        ScoreCardTable.Rows.Add(row);
    }

    private Table BuildEmptyScoreCardTable(CourseInfo courseInfo)
    {
        Table scoreCardTable = new Table();

        //Title Row
        TableRow titleRow = new TableRow();
        TableCell cell = AddCell(titleRow, courseInfo.name);
        cell.ColumnSpan = 11;
        cell.Font.Size = 18;
        cell.Font.Bold = true;
        titleRow.Cells.Add(cell);
        scoreCardTable.Rows.Add(titleRow);

        //Top Row
        TableRow holeRow = new TableRow();
        holeRow.BackColor = Color.LightBlue;
        AddCell(holeRow, "Hole", true, 14);
        for (int i = 1; i < 10; i++)
        {
            AddCell(holeRow, i.ToString(), true, 14);
        }
        AddCell(holeRow, "", true, 14);
        scoreCardTable.Rows.Add(holeRow);

        //Row 2
        TableRow lengthRow = new TableRow();
        AddCell(lengthRow, "Length", true, 14);
        int totalLength = 0;
        for (int i = 0; i < 9; i++)
        {
            totalLength += courseInfo.holeLengths[i];
            AddCell(lengthRow, courseInfo.holeLengths[i].ToString(), false, 14);
        }
        AddCell(lengthRow, totalLength.ToString(), true, 14);
        scoreCardTable.Rows.Add(lengthRow);

        //Row 3
        TableRow handicapRow = new TableRow();
        AddCell(handicapRow, "Handicap", true, 14);
        foreach (int handicap in courseInfo.holeHandicaps)
        {
            AddCell(handicapRow, handicap.ToString(), false, 14);
        }
        AddCell(handicapRow, "Total", true, 14);
        scoreCardTable.Rows.Add(handicapRow);


        //Row 4
        TableRow parRow = new TableRow();
        AddCell(parRow, "Mens Par", true, 14);
        int parTotal = 0;
        parRow.BackColor = Color.LightGreen;
        foreach(int parRating in courseInfo.holeParRatings)
        {
            parTotal += parRating;
            AddCell(parRow, parRating.ToString(), true, 14);
        }
        AddCell(parRow, parTotal.ToString(), true, 14);
        scoreCardTable.Rows.Add(parRow);
        
        return scoreCardTable;
    }


    private void AddCell(TableRow row, string text, bool bold, int fontSize)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.Font.Bold = bold;
        cell.Font.Size = fontSize;
        cell.ForeColor = Color.Black;
        cell.BorderStyle = BorderStyle.Solid;
        cell.BorderWidth = 1;
        row.Cells.Add(cell);
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

    

    protected void Dropdown_Season_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int seasonID = int.Parse(Dropdown_Seasons.SelectedValue);
        //ClearTableContents();

        //int seasonID = int.Parse(Dropdown_Seasons.SelectedValue);
        //Session["StatsSelectedSeason"] = seasonID;

        //List<int> CourseIDs = DatabaseFunctions.GetCourseIDs(leagueID);

       
        //Scoring.LeagueStats LeagueStats = null;
        //LeagueStats = Scoring.GetSeasonStats(leagueID, seasonID, CourseIDs);
        //Session["LeagueStats"] = LeagueStats;
       
        //UpdateContent(LeagueStats, CourseIDs);
    }


    protected void Dropdown_Player_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int golferID = int.Parse(DropDown_PlayerSelect.SelectedValue);
        //UpdatePlayerContent();
        //Session["StatsSelectedPlayer"] = int.Parse(DropDown_PlayerSelect.SelectedValue);
        //Scoring.LeagueStats LeagueStats = (Scoring.LeagueStats)Session["LeagueStats"];
        //UpdateContent(LeagueStats, DatabaseFunctions.GetCourseIDs(leagueID));
    }

    private TableCell AddCell(TableRow row, string text)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.ForeColor = Color.Black;
        row.Cells.Add(cell);
        return cell;
    }

    private void ClearTableContents()
    {
        Panel_AverageScoresByCourse.Controls.Clear();
        Table_AverageScores.Rows.Clear();
        Table_LowestGrossSingleRoundScores.Rows.Clear();
    }

    private void UpdatePlayerContent()
    {
        int golferID = int.Parse(DropDown_PlayerSelect.SelectedValue);
        if (golferID == 0)
        {
            return;
        }
        string golferName = DatabaseFunctions.GetGolferName(golferID);

        Scoring.LeagueStats LeagueStats = (Scoring.LeagueStats)Session["LeagueStats"];
        List<int> CourseIDs = DatabaseFunctions.GetCourseIDs(leagueID);

        //Build Course Statistics
        Table ScoreCardsTable = new Table();
        ScoreCardsTable.CellPadding = 40;
        foreach (int courseID in CourseIDs)
        {
            if (LeagueStats.NumRoundsByGolferAndCourse[courseID][golferID] == 0)
            {
                break;
            }
            CourseInfo courseInfo = DatabaseFunctions.GetCourseInfoFromCourseID(courseID);

            Table scoreCardTable = BuildEmptyScoreCardTable(courseInfo);

            //Title Row
            TableRow titleRow = new TableRow();
            TableCell cell = AddCell(titleRow, "Number of Rounds Played : " + LeagueStats.NumRoundsByGolferAndCourse[courseID][golferID]);//DatabaseFunctions.GetGolferName(golferID) +
            cell.ColumnSpan = 11;
            cell.Font.Size = 18;
            cell.Font.Bold = true;
            titleRow.Cells.Add(cell);
            scoreCardTable.Rows.AddAt(1, titleRow);
            
            scoreCardTable.BorderStyle = BorderStyle.Solid;
            scoreCardTable.BorderWidth = 5;

            AddScoreCardRow(scoreCardTable, "Best Score", LeagueStats.BestScoresByHole[courseID][golferID]);
            AddScoreCardRow(scoreCardTable, "Worst Score", LeagueStats.WorstScoresByHole[courseID][golferID]);
            AddScoreCardRow(scoreCardTable, "Average Score", roundDecimalList(LeagueStats.AverageScoresByHole[courseID][golferID]));
            AddScoreCardRow(scoreCardTable, "Skins Won", listOfIntToByte(LeagueStats.SkinsByCourseID[courseID][golferID]));

            TableRow scoreCardHolderRow = new TableRow();
            TableCell scoreCardHolderCell = new TableCell();
            scoreCardHolderCell.Controls.Add(scoreCardTable);
            scoreCardHolderRow.Cells.Add(scoreCardHolderCell);
            ScoreCardsTable.Rows.Add(scoreCardHolderRow);
        }
        CourseStats.Controls.Add(ScoreCardsTable);

        //Build Graph for scores and handicaps 

        FontFamily fontFamily = new FontFamily("Arial");
        Font titlefont = new Font(fontFamily, 16, FontStyle.Bold, GraphicsUnit.Pixel);

        Chart chartScores = new Chart();
        chartScores.Width = 800;
        chartScores.Height = 500;
        chartScores.ImageStorageMode = ImageStorageMode.UseImageLocation;
        //chartScores.Legends.Add(new Legend("Default"));
        //chartScores.Legends["Default"].Docking = Docking.Bottom;
        //chartScores.Legends["Default"].Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        chartScores.BorderSkin = new BorderSkin();
        chartScores.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        chartScores.BorderSkin.PageColor = Color.Transparent;
        chartScores.Titles.Add(new Title("Scores for " + golferName, Docking.Top, titlefont, Color.Black));
        ChartArea chartArea = new ChartArea();
        chartArea.Name = "ChartArea1";
        chartArea.AxisX.ScaleBreakStyle.Enabled = true;
        chartArea.AxisY.IsStartedFromZero = false;
        chartScores.ChartAreas.Add(chartArea);
        

        Chart chartHandicaps = new Chart();
        chartHandicaps.Width = 800;
        chartHandicaps.Height = 500;
        chartHandicaps.ImageStorageMode = ImageStorageMode.UseImageLocation;
        //chartHandicaps.Legends.Add(new Legend("Default"));
        //chartHandicaps.Legends["Default"].Docking = Docking.Bottom;
        //chartHandicaps.Legends["Default"].Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        chartHandicaps.BorderSkin = new BorderSkin();
        chartHandicaps.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        chartHandicaps.BorderSkin.PageColor = Color.Transparent;
        chartHandicaps.Titles.Add(new Title("Handicap for " + golferName, Docking.Top ,titlefont, Color.Black));
        ChartArea chartArea2 = new ChartArea();
        chartArea2.Name = "ChartArea2";
        chartArea2.AxisX.ScaleBreakStyle.Enabled = true;
        chartArea2.AxisX.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
        chartHandicaps.ChartAreas.Add(chartArea2);

        Series series = new Series("Handicap");
        series.ChartType = SeriesChartType.Point;
        series.XValueType = ChartValueType.Date;
        series.MarkerSize = 20;
        series.MarkerStyle = MarkerStyle.Diamond;
        chartHandicaps.Series.Add(series);

        series = new Series("Scores");
        series.ChartType = SeriesChartType.Point;
        series.XValueType = ChartValueType.Date;
        series.MarkerSize = 20;
        series.MarkerStyle = MarkerStyle.Star6;
        chartScores.Series.Add(series);
        
        //List<int> scores = new List<int>();
        //List<int> handicaps = new List<int>();
        //List<DateTime> dates = new List<DateTime>();

        List<IndividualScore> scores = new List<IndividualScore>();
        Dictionary<int, EventInfo> eventInfos = DatabaseFunctions.GetEventsWithScores(leagueID);

        foreach (int eventID in LeagueStats.ScoreByEvent[golferID].Keys)
        {
            IndividualScore score = new IndividualScore();
            //DateTime? eventDateTime = DatabaseFunctions.GetEventDate(eventID);
            DateTime eventDateTime = DateTime.Parse(eventInfos[eventID].Date);
            DataPoint point = new DataPoint();
            point.SetValueXY(eventDateTime, LeagueStats.ScoreByEvent[golferID][eventID]);
            chartScores.Series["Scores"].Points.Add(point);

            DataPoint point2 = new DataPoint();
            point2.SetValueXY(eventDateTime, LeagueStats.handicaps[golferID][eventID]);
            chartHandicaps.Series["Handicap"].Points.Add(point2);

            score.Handicap = LeagueStats.handicaps[golferID][eventID];
            score.Score = LeagueStats.ScoreByEvent[golferID][eventID];
            score.date =  eventDateTime; 
            score.CourseName = eventInfos[eventID].CourseName;
            score.SeasonName = eventInfos[eventID].SeasonName;
            score.EventName = eventInfos[eventID].EventName;

            scores.Add(score);
            //dates.Add((DateTime)eventDateTime);
            //scores.Add(LeagueStats.ScoreByEvent[golferID][eventID]);
            //handicaps.Add(LeagueStats.handicaps[golferID][eventID]);
        }

        Graph.Controls.Add(chartScores);
        Graph.Controls.Add(chartHandicaps);

        //BuildPlayerHandicapTable(scores, handicaps, dates);
        PopulateScoreGrid(scores.OrderBy(x => x.date).ToList());
        lbGridTitle.Text = "Scores for " + golferName;  
    }




    private void UpdateContent(Scoring.LeagueStats LeagueStats, List<int> CourseIDs, int LeagueID)
    {
       
        Table averageScoresByCourseTable = new Table();
        averageScoresByCourseTable.CellPadding = 5;
        TableRow titleRow = new TableRow();
        TableCell titleCell = AddCell(titleRow, "Scoring Averages by Course");
        titleCell.ColumnSpan = CourseIDs.Count;
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleCell.Font.Bold = true;
        titleCell.Font.Size = 16;
        averageScoresByCourseTable.Rows.Add(titleRow);

        Table ScoreCardsTable = new Table();
        ScoreCardsTable.CellPadding = 40;

        TableRow tr = new TableRow();
        //Add Scorecards for each course
        foreach (int courseID in CourseIDs)
        {
            CourseInfo courseInfo = DatabaseFunctions.GetCourseInfoFromCourseID(courseID);
            
            Table scoreCardTable = BuildEmptyScoreCardTable(courseInfo);
            scoreCardTable.BorderStyle = BorderStyle.Solid;
            scoreCardTable.BorderWidth = 5;
            AddScoreCardRows(scoreCardTable, LeagueStats.LeagueHoleAveragesByCourse[courseID], LeagueStats.LeagueRingerNineByCourse[courseID], LeagueStats.LeagueHighScoreByCourse[courseID]);
            TableRow scoreCardHolderRow = new TableRow();
            TableCell scoreCardHolderCell = new TableCell();
            scoreCardHolderCell.Controls.Add(scoreCardTable);
            scoreCardHolderRow.Cells.Add(scoreCardHolderCell);
            ScoreCardsTable.Rows.Add(scoreCardHolderRow);


            Table bestScoresTable = new Table();
            BuildAvgScoreTable(bestScoresTable, courseInfo.name, LeagueStats.AverageScoreByCourse[courseID], LeagueStats.NumRoundsByGolferAndCourse[courseID], 12);
            TableCell tc = new TableCell();
            tc.VerticalAlign = VerticalAlign.Top;
            tc.Controls.Add(bestScoresTable);
            tr.Cells.Add(tc);
        }
        averageScoresByCourseTable.Rows.Add(tr);
        Panel_AverageScoresByCourse.Controls.Add(averageScoresByCourseTable);
        Panel_HoleScores.Controls.Add(ScoreCardsTable);

        BuildAvgScoreTable(Table_AverageScores, "Average Score All Courses", LeagueStats.AvgScores, LeagueStats.NumberOfEventsPlayed, 16);
        BuildBestScoreTable(Table_LowestGrossSingleRoundScores, "Lowest Rounds (gross)", LeagueStats.LowestGrossSingleRoundScores, 30);
        BuildBestScoreTable(Table_LowestNetSingleRoundScores, "Lowest Rounds (Net)", LeagueStats.LowestNetSingleRoundScores, 30);
        BuildHighestTotalTable(Table_Skins, "Skins", LeagueStats.Skins, LeagueStats.NumberOfEventsPlayed);
        BuildHighestTotalTable(Table_Birdies, "Birdies", LeagueStats.Birdies, LeagueStats.NumberOfEventsPlayed);
        BuildHighestTotalTable(Table_Pars, "Pars", LeagueStats.Pars, LeagueStats.NumberOfEventsPlayed);
        BuildHighestTotalTable(Table_Bogeys, "Bogeys", LeagueStats.Bogeys, LeagueStats.NumberOfEventsPlayed);
        BuildHighestTotalTable(Table_DoubleBogeys, "Double Bogeys", LeagueStats.DoubleBogeys, LeagueStats.NumberOfEventsPlayed);
        BuildAvgScoreTable(Table_Par3Scoring, "Par 3 Scoring Averages", LeagueStats.ParThreeScoringAvg, 16, "Number of Par3's Played");
        BuildAvgScoreTable(Table_Par4Scoring, "Par 4 Scoring Averages", LeagueStats.ParFourScoringAvg, 16, "Number of Par4's Played");
        BuildAvgScoreTable(Table_Par5Scoring, "Par 5 Scoring Averages", LeagueStats.ParFiveScoringAvg, 16, "Number of Par5's Played");

        try
        {
            int lastEventID = DatabaseFunctions.GetMostRecentEventWithScoresPosted(LeagueID.ToString()).EventID;
            BuildHighestTotalTable(Table_Handicaps, "Handicaps", Scoring.GetHandicapsForEventFromHandicapByGolferIDDictionary(LeagueStats.handicaps, lastEventID), LeagueStats.NumberOfEventsPlayed);
        }
        catch { }
        //BuildHighestTotalTable(Table_Eagles, "Eagles", LeagueStats.Eagles, LeagueStats.NumberOfEventsPlayed);


        //BuildBestScoreTable(Table_LowestNetSingleRoundScores, "Lowest Rounds (net)", LeagueStats.LowestNetSingleRoundScores); 

        UpdatePlayerContent();
    }

    private bool DropdownsChangedOrNotIinitialized()
    {
        if (Session["StatsSeasonDropdownValue"] == null || Session["StatsPlayerDropdownValue"]== null)
        {
            return true;
        }
        if (Session["StatsSeasonDropdownValue"] != Dropdown_Seasons.SelectedValue)
        {
            return true;
        }
        if (Session["StatsPlayersDropdownValue"] != DropDown_PlayerSelect.SelectedValue)
        {
            return true;
        }
        return false;
    }

    protected void StatsMenu_MenuItemClick(Object sender, MenuEventArgs e)
    {
        int selectedSeason = int.Parse(Dropdown_Seasons.SelectedValue);
        int selectedPlayer = int.Parse(DropDown_PlayerSelect.SelectedValue);

        //Check dropdowns to see if they have changed since they were last saved.
        if (DropdownsChangedOrNotIinitialized())
        {
            Scoring.LeagueStats LeagueStats = null;
            List<int> CourseIDs = DatabaseFunctions.GetCourseIDs(leagueID);
            LeagueStats = Scoring.GetSeasonStats(leagueID, selectedSeason, CourseIDs);
            Session["LeagueStats"] = LeagueStats;
            UpdateContent(LeagueStats, CourseIDs, leagueID); 
            Session["StatsSeasonDropdownValue"] = Dropdown_Seasons.SelectedValue;
            Session["StatsPlayerDropdownValue"] = DropDown_PlayerSelect.SelectedValue;
        }
             
        if(e.Item.Value == "AvgAll")
        {
            Session["SelectedStatView"] = "AvgAll"; 
        }
        else if (e.Item.Value == "LowNet")
        {
            //Table_LowestNetSingleRoundScores.Style.Remove("display");
             Session["SelectedStatView"] = "LowNet";
        }
        else if (e.Item.Value == "LowGross")
        {
             Session["SelectedStatView"] = "LowGross";
        }
        else if (e.Item.Value == "IndvPts")
        {
            //Table_.Style.Add("display", "block");
        }
        else if (e.Item.Value == "Skins")
        {
            Table_Skins.Style.Clear();
             Session["SelectedStatView"] = "Skins";
        }
        else if (e.Item.Value == "Birdies")
        {
             Session["SelectedStatView"] = "Birdies";
        }
        else if (e.Item.Value == "Pars")
        {
             Session["SelectedStatView"] = "Pars";
        }
        else if (e.Item.Value == "Bogeys")
        {
             Session["SelectedStatView"] = "Bogeys";
        }
        else if (e.Item.Value == "DoubleBogeys")
        {
             Session["SelectedStatView"] =  "DoubleBogeys";
        }
        else if (e.Item.Value == "Eagles")
        {
             Session["SelectedStatView"] = "Eagles";
        }
        else if (e.Item.Value == "Par3")
        {
             Session["SelectedStatView"] = "Par3";
        }
        else if (e.Item.Value == "Par4")
        {
             Session["SelectedStatView"] = "Par4";
        }
        else if (e.Item.Value == "Par5")
        {
            Session["SelectedStatView"] = "Par5";
        }
        else if (e.Item.Value == "Handicaps")
        {
            Session["SelectedStatView"] = "Handicaps";
        }
        else if (e.Item.Value == "CourseStats")
        {
            Session["SelectedStatView"] = "CourseStats";
        }
        else if (e.Item.Value == "Graph")
        {
            Session["SelectedStatView"] = "Graph";
        }
        else if (e.Item.Value == "EventScores")
        {
            Session["SelectedStatView"] = "EventScores";
        }
        else if (e.Item.Value == "IndividualHoles")
        {
            Session["SelectedStatView"] = "IndividualHoles";
        }

        Response.Redirect("~/stats.aspx");
        
    }

    //private void HideAllTables()
    //{
    //    Table_AverageScores.Style.Add("display", "none");
    //    Table_LowestGrossSingleRoundScores.Style.Add("display", "none");
    //    Table_LowestNetSingleRoundScores.Style.Add("display", "none");
    //    Table_Birdies.Style.Add("display", "none");
    //    Table_Bogeys.Style.Add("display", "none");
    //}
 


    private void PopulateScoreGrid(List<IndividualScore> scores)
    {
        gridScores.DataSource = scores;
        gridScores.DataBind();
    }

    //deprecated
    private void BuildPlayerHandicapTable(List<int> scores, List<int> handicaps, List<DateTime> dates)
    {


        //iterate through event names and add column header
        Table handicapsTable = new Table();
        TableRow titleRow = new TableRow();
        titleRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#465c71");
       
        foreach (DateTime date in dates)
        {
            AddTitleCell(titleRow, date.ToShortDateString());
        }

        handicapsTable.Rows.Add(titleRow);

        TableRow scoresrow = AddRow(handicapsTable);
        foreach (int score in scores)
        {
            AddCell(scoresrow, score.ToString());
        }
        handicapsTable.Rows.Add(scoresrow);

        TableRow handicapsrow = AddRow(handicapsTable);
        foreach (int handicap in handicaps)
        {
            AddCell(handicapsrow, handicap.ToString());
        }
        handicapsTable.Rows.Add(handicapsrow);

        //Panel_AllScores.Controls.Add(handicapsTable);

    }

    private TableCell AddTitleCell(TableRow row, string text)
    {
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = text;
        cell.Font.Size = 14;
        cell.Font.Bold = false;
        cell.ForeColor = Color.White;
        cell.BorderStyle = BorderStyle.Solid;
        row.Cells.Add(cell);
        return cell;
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



}