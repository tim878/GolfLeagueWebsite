using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing; 

public partial class Teams : System.Web.UI.Page
{
    private int leagueID;
    private EventsAndGolfers eventsAndGolfers;
    Dictionary<int, CheckBox> selectedTeams = new Dictionary<int, CheckBox>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        leagueID = (int)((SiteMaster)this.Master).LeagueID;


        if(Panel_Teams.Controls.Count == 0)
        {

            eventsAndGolfers = DatabaseFunctions.GetEventsAndPlayers((int)leagueID);
            int currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
            Dictionary<int, string> teams = DatabaseFunctions.GetTeamNames((int)leagueID);

            Table teamTable = new Table();
            teamTable.Width = 800;
            TableRow titleRow1 = new TableRow();
            teamTable.Rows.Add(titleRow1);
            TableCell cell3 = new TableCell();
            cell3.Text = "Edit Current Teams";
            cell3.Font.Bold = true;
            cell3.ForeColor = Color.Black;
            cell3.Font.Size = 20;
            cell3.HorizontalAlign = HorizontalAlign.Center;
            cell3.ColumnSpan = 4;
            cell3.BorderStyle = BorderStyle.Solid;
            titleRow1.Cells.Add(cell3);

            TableRow titleRow2 = new TableRow();
            AddCell(titleRow2, "Hide for future Events", true, 1);
            AddCell(titleRow2, "Team Name", true, 1);
            AddCell(titleRow2, "Golfer 1", true, 1);
            AddCell(titleRow2, "Golfer 2", true, 1);
            teamTable.Rows.Add(titleRow2);

            foreach (int teamID in teams.Keys)
            {
                TableRow teamRow = new TableRow();
                CheckBox cBox = new CheckBox();
                //cBox.Width = 80;
                //cBox.Height = 80;
                //cBox.Checked = DatabaseFunctions.isTeamActiveInSeason(teamID, currentSeasonID);
                cBox.CheckedChanged += SetTeamActive;
                cBox.AutoPostBack = true;
                cBox.ID = "TeamActive_" + teamID.ToString();
                TableCell cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Controls.Add(cBox);
                teamRow.Cells.Add(cell);

                TableCell cell2 = new TableCell();
                TextBox teamName = new TextBox();
                teamName.Width = 300;
                teamName.Text = teams[teamID];
                cell2.Controls.Add(teamName);
                Button editButton = new Button();
                editButton.ID = "Team_" + teamID.ToString();
                editButton.Text = "Save";
                editButton.Command += SaveTeamName;
                cell2.Controls.Add(editButton);
                teamRow.Cells.Add(cell2);

                Dictionary<int, string> golferNames = DatabaseFunctions.GetGolfers(teamID);
                AddCell(teamRow, golferNames.Values.ToList()[0], true, 1);
                AddCell(teamRow, golferNames.Values.ToList()[1], true, 1);
                teamTable.Rows.Add(teamRow);

                selectedTeams.Add(teamID, cBox);

            }

                Panel_Teams.Controls.Add(teamTable);
        }
            //Panel_Teams.Controls.Add(new LiteralControl("<br/>"));
        if (!Page.IsPostBack)
        {
            //Dropdown_Player1Select.Items.Clear();
            //Dropdown_Player2Select.Items.Clear();
            ListItem player1 = new ListItem("Select First Player", "0");
            ListItem player2 = new ListItem("Select Second Player", "0");
            Dropdown_Player1Select.Items.Add(player1);
            Dropdown_Player2Select.Items.Add(player2);
            foreach (int golferID in eventsAndGolfers.golfers.Keys)
            {
                ListItem temp = new ListItem(eventsAndGolfers.golfers[golferID], golferID.ToString());
                Dropdown_Player1Select.Items.Add(temp);
                Dropdown_Player2Select.Items.Add(temp);
            }
        }
        
       
    }

    private void AddCell(TableRow row, string text, bool bold, int colSpan)
    {
        TableCell cell = new TableCell();
        cell.Text = text;
        cell.Font.Bold = bold;
        cell.ForeColor = Color.Black;
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = colSpan;
        //cell.BorderStyle = BorderStyle.Solid;
        row.Cells.Add(cell);
    }

    protected void AddTeam(object sender, CommandEventArgs e)
    {
       string teamName;
       if (TextBox_TeamName.Text != "")
       {
           teamName = TextBox_TeamName.Text; 
       }
       else
       {
           teamName = Dropdown_Player1Select.SelectedItem.Text + " / " + Dropdown_Player2Select.SelectedItem.Text;
       }

       //DatabaseFunctions.AddTeam((int)leagueID, teamName, getPlayerID(Dropdown_Player1Select.SelectedItem.Text, eventsAndGolfers.golfers), getPlayerID(Dropdown_Player2Select.SelectedItem.Text, eventsAndGolfers.golfers), Checkbox_ActiveInCurrentSeason.Checked);
       resetFields();
       //Response.Write("<script language='javascript'>alert('Team Added Successfully.');</script>");
       Response.Redirect("Teams.aspx");
    }

    private void resetFields()
    {
        TextBox_TeamName.Text = "";
        Dropdown_Player1Select.SelectedIndex = 0;
        Dropdown_Player2Select.SelectedIndex = 0;
        Checkbox_ActiveInCurrentSeason.Checked = false;
    }

    protected void SaveTeamName(object sender, CommandEventArgs e)
    {
        TextBox t = (TextBox)sender;
        if (t.Text != "")
        {
            try
            {
                string teamIDtoModify = t.ID.Split('_')[1];
                DatabaseFunctions.UpdateTeamName(teamIDtoModify, t.Text);
                Response.Write("<script language='javascript'>alert('Team Name Updated.');</script>");
                //Response.Redirect("account/AddTeam.aspx");
            }
            catch
            {
                Response.Write("<script language='javascript'>alert('Error Updating Team Name.');</script>");
                return;
            }
        }
    }

    protected void SetTeamActive(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
       
        try
        {
            string teamIDtoModify = c.ID.Split('_')[1];
            DatabaseFunctions.UpdateTeamActiveInCurrentSeason(DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString()), teamIDtoModify, c.Checked);
            Response.Write("<script language='javascript'>alert('Team Status Updated');</script>");
        }
        catch
        {
            Response.Write("<script language='javascript'>alert('Error Updating Status.  Checkbox may not reflect correct status, try refreshing page.');</script>");
            return;
        }
    }

    private int getPlayerID(string playerName, Dictionary<int, string> golferDictionary)
    {
        foreach (int playerID in golferDictionary.Keys)
        {
            if (golferDictionary[playerID] == playerName)
            {
                return playerID;
            }
        }
        return 0;
    }
}