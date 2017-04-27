using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

public partial class Events : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
    Dictionary<int, string> Courses;
    //Dictionary<int, int> CourseIDtoDropdownIndexMapping = new Dictionary<int,int>();
    //Dictionary<int, int> SeasonIDtoDropdownIndexMapping = new Dictionary<int,int>();
    //Dictionary<int, string> allTeams;
    //Dictionary<int, string> teamsLeftToBeSelected;
    Dictionary<int, int> SelectedMatchups;
    //private int LeagueEventID;


    protected void Page_Load(object sender, EventArgs e)
    {
        currentUserID = ((SiteMaster)this.Master).currentUserID;
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());

        if (!Page.IsPostBack)
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

            List<Season> seasons = DatabaseFunctions.GetSeasons(leagueID);
            int i = 0;
            foreach (Season season in seasons)
            {
                ListItem newItem = new ListItem(season.SeasonName, season.SeasonID.ToString());
                DropDown_Seasons.Items.Add(newItem);
                if (season.isCurrentSeason)
                {
                    DropDown_Seasons.SelectedIndex = i;
                }
                i++;
                DropDown_Seasons.Enabled = false;
            }
        }

        Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);
        if (!Page.IsPostBack)
        {
            //populate tees dropdown
            foreach (string teeName in GolfLeagueWebsiteGlobals.Tees.Keys)
            {

                DropDown_Tees.Items.Add(teeName);
            }

            ListItem itemToAdd = new ListItem("Select First Team", "0");
            Dropdown_Team1.Items.Add(itemToAdd);
            ListItem itemToAdd2 = new ListItem("Select Second Team", "0");
            Dropdown_Team2.Items.Add(itemToAdd2);

            foreach (int teamID in teams.Keys)
            {
                AddTeamToDropdowns(teamID, teams);
            }
        }

        if (ViewState["MatchupControls"] != null)
        {
            SelectedMatchups = (Dictionary<int, int>)ViewState["MatchupControls"];
            foreach (int team1ID in SelectedMatchups.Keys)
            {
                AddMatchup(team1ID, SelectedMatchups[team1ID], teams);
            }
        }
        else
        {
            SelectedMatchups = new Dictionary<int, int>();
        }

    }

    //private void DisplayExistingEvents()
    //{
    //    int seasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
    //    List<EventInfo> events = DatabaseFunctions.GetEvents(leagueID, seasonID);

    //    var items = from eventInfo in events
    //                orderby eventInfo.EventNumber ascending
    //                select eventInfo;

    //    foreach (EventInfo eventInfo in items)
    //    {
    //        TableRow tr = new TableRow();
            



    //        TableCell numRdsCell = CreateTableCellWithTextbox("numRds_" + golfer.golferID, golfer.numberOfRoundsPlayed.ToString(), 50);
    //        numRdsCell.Width = 50;
    //        tr.Cells.Add(numRdsCell);

    //        string textBox1ID = "textBox_" + golfer.golferID.ToString();
    //        tr.Cells.Add(CreateTableCellWithTextbox(textBox1ID, golfer.firstName));

    //        string textBox2ID = "textBoxLastName_" + golfer.golferID.ToString();
    //        tr.Cells.Add(CreateTableCellWithTextbox(textBox2ID, golfer.lastName));

    //        string textBox3ID = "textBoxNickName_" + golfer.golferID.ToString();
    //        tr.Cells.Add(CreateTableCellWithTextbox(textBox3ID, golfer.nickName));

    //        string textBox4ID = "textBoxEmail_" + golfer.golferID.ToString();
    //        tr.Cells.Add(CreateTableCellWithTextbox(textBox4ID, golfer.emailAddress));

    //        Button SaveButton = new Button();
    //        SaveButton.Text = "Save";
    //        SaveButton.Width = 70;
    //        SaveButton.ID = "SaveButton_" + golfer.golferID.ToString();
    //        SaveButton.Style.Add("display", "none");
    //        //SaveButton.Command += SaveEdit;

    //        HtmlButton CancelButton = new HtmlButton();
    //        CancelButton.ID = "CancelButton_" + golfer.golferID.ToString();
    //        //CancelButton.Style("width") = "2em";
    //        //Button CancelButton = new Button();
    //        //CancelButton.Text = "Cancel";
    //        //CancelButton.Width = 70;
    //        CancelButton.ID = "CancelButton_" + golfer.golferID.ToString();
    //        CancelButton.Style.Add("display", "none");
    //        CancelButton.InnerText = "Cancel";

    //        HtmlButton EditButton = new HtmlButton();
    //        EditButton.InnerText = "Edit";
    //        //EditButton.t = "Edit";
    //        //EditButton.Width = 70;
    //        EditButton.ID = "EditButton_" + golfer.golferID.ToString();
    //        EditButton.Attributes.Add("onclick", "javascript:if(!editActive){ActivateTextBox('MainContent_" + textBox1ID + "');ActivateTextBox('MainContent_" + textBox2ID + "');ActivateTextBox('MainContent_" + textBox3ID + "');ActivateTextBox('MainContent_" + textBox4ID + "');ShowControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + EditButton.ID + "');}editActive=true;");
    //        //CancelButton.PostBackUrl = "javascript:HideControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + EditButton.ID + ");";
    //        CancelButton.Attributes.Add("onclick", "javascript:HideControl('MainContent_" + CancelButton.ID + "');HideControl('MainContent_" + SaveButton.ID + "');ShowControl('MainContent_" + EditButton.ID + "');DeactivateTextBox('MainContent_" + textBox1ID + "');DeactivateTextBox('MainContent_" + textBox2ID + "');DeactivateTextBox('MainContent_" + textBox3ID + "');DeactivateTextBox('MainContent_" + textBox4ID + "');editActive=false;");

    //        Table ButtonTable = new Table();
    //        TableRow buttonRow = new TableRow();
    //        TableCell tcButton = new TableCell();
    //        TableCell tcButton1 = new TableCell();
    //        TableCell tcButton2 = new TableCell();
    //        TableCell tcButton3 = new TableCell();
    //        tcButton1.Controls.Add(EditButton);
    //        tcButton2.Controls.Add(SaveButton);
    //        tcButton3.Controls.Add(CancelButton);
    //        buttonRow.Cells.Add(tcButton1);
    //        buttonRow.Cells.Add(tcButton2);
    //        buttonRow.Cells.Add(tcButton3);
    //        ButtonTable.Rows.Add(buttonRow);
    //        tcButton.Controls.Add(ButtonTable);
    //        tr.Cells.Add(tcButton);

    //        //Table_MainContent.Rows.AddAt(1, tr);
    //        Table_MainContent.Rows.Add(tr);
    //    }
    //}

    //private TableCell CreateTableCellwithCalendarTextbox(string controlID, DateTime currentDateValue)
    //{
    //    //CalendarExtender 
    //    TableCell tc = new TableCell();
    //    TextBox tb = new TextBox();
    //    tb.ID = textboxID;
    //    //textBoxLookup.Add(textboxID, tb);
    //    tb.Text = initialTextBoxText;
    //    tb.Enabled = false;
    //    tc.Controls.Add(tb);
    //    return tc;
    //}

    private TableCell CreateTableCellWithTextbox(string textboxID, string initialTextBoxText)
    {

        TableCell tc = new TableCell();
        TextBox tb = new TextBox();
        tb.ID = textboxID;
        //textBoxLookup.Add(textboxID, tb);
        tb.Text = initialTextBoxText;
        tb.Enabled = false;
        tc.Controls.Add(tb);
        return tc;
    }

    private TableCell CreateTableCellWithTextbox(string textboxID, string initialTextBoxText, int textBoxWidth)
    {
        TableCell tc = new TableCell();
        TextBox tb = new TextBox();
        tb.ID = textboxID;
        tb.Text = initialTextBoxText;
        tb.Enabled = false;
        tb.Width = 50;
        tc.Controls.Add(tb);
        return tc;
    }


    protected void CreateLeagueEventButtonPress(object sender, CommandEventArgs e)
    {
        if (ValidateInput())
        {
            DateTime selectedDate = Calendar_Date.SelectedDate;
            int courseID = int.Parse( Dropdown_Course.SelectedValue);
            int seasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());//int.Parse(DropDown_Seasons.SelectedValue);
            int teeSelected = GolfLeagueWebsiteGlobals.Tees[DropDown_Tees.SelectedValue];
            int eventID = DatabaseFunctions.AddLeagueEvent(leagueID, seasonID, int.Parse(TextBox_EventNum.Text), selectedDate, courseID, teeSelected, TextBox_EventName.Text);
            foreach (int team1ID in SelectedMatchups.Keys)
            {
                DatabaseFunctions.AddMatchup(eventID, team1ID.ToString(), SelectedMatchups[team1ID].ToString());
            }
            Response.Write("<script language='javascript'>alert('Event Added Successfully.');</script>");
            Response.Redirect("Events.aspx");
            //Response.Redirect("ManageLeagues.aspx");
            //Response.Redirect("Matchups_EventSelection.aspx");
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Problem Adding Event.');</script>");
            //TODO popup message saying event name cannot be blank 
        }
        
    }

    private bool ValidateInput()
    {
        if (TextBox_EventName.Text == "" && Dropdown_Course.SelectedValue != "0")
        {
            return false;
        }
        if (Dropdown_Team1.Items.Count > 1 || Dropdown_Team2.Items.Count > 1)
        {
            return false;
        }
        int unused;
        if (!int.TryParse(TextBox_EventNum.Text, out unused))
        {
            return false;
        }
        return true;
    }

    private void AddTeamToDropdowns(int teamID, Dictionary<int, string> teams)
    {
        ListItem itemToAdd = new ListItem(teams[teamID], teamID.ToString());
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
            int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
            Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);
            AddMatchup(int.Parse(team1ID), int.Parse(team2ID), teams);
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


    private void AddMatchup(int team1ID, int team2ID, Dictionary<int, string> teams)
    {
        string matchupID = getMatchupID(team1ID, team2ID);
        Label matchupLabel = new Label();
        matchupLabel.Text = teams[team1ID] + " vs. " + teams[team2ID] + "   ";
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
        int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames(leagueID);
        AddTeamToDropdowns(team1ID, teams);
        AddTeamToDropdowns(team2ID, teams);
        SelectedMatchups.Remove(team1ID);
        ViewState["MatchupControls"] = SelectedMatchups;

    }

}