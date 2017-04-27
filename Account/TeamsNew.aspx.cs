using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TeamsNew : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;
    EventsAndGolfers eventsAndGolfers;
    private int RowCount { get; set; }

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
               
        if (!Page.IsPostBack)
        {
            BindDataToGrid();
        }

    }

    private void BindDataToGrid()
    {
        List<Team> teams = DatabaseFunctions.GetTeams(leagueID).Values.ToList();
        teams = teams.OrderBy(x => x.hidden).ToList(); 
        grdTeams.DataSource = teams;
        grdTeams.DataBind();
    }

    
    protected void SaveEdits(object sender, CommandEventArgs e)
    {
         int teamID = int.Parse((string)e.CommandArgument);
         DatabaseFunctions.EditTeam(teamID, TextBoxTeamName.Text, CheckBoxHide.Checked);
         BindDataToGrid();      
    }


    protected void AddSubmit(object sender, CommandEventArgs e)
    {
         string golfer1ID = DropdownGolfer1.SelectedValue;
         string golfer2ID = DropdownGolfer2.SelectedValue;
            string golfer3ID = DropDownGolfer3.SelectedValue;
        int? golfer3 = null;
        if(golfer3ID != "0")
        {
            golfer3 = int.Parse(golfer3ID);
        }
        DatabaseFunctions.AddTeam(leagueID, TextBoxAddTeamName.Text, int.Parse(golfer1ID), int.Parse(golfer2ID), golfer3, false);
         BindDataToGrid();
    }    

    
    
    protected void EditButtonClick(object sender, CommandEventArgs e)
    {
        int teamID = int.Parse((string)e.CommandArgument);
        Team team = DatabaseFunctions.GetTeams(leagueID)[teamID];
        TextBoxTeamName.Text = team.TeamName;
        CheckBoxHide.Checked = team.hidden;
        LabelGolfer1.Text = team.Golfer1Name;
        LabelGolfer2.Text = team.Golfer2Name;
      
        btnSaveEdits.CommandArgument = teamID.ToString();
        ModalPopupExtenderEdit.Show();
    }

    protected void AddButtonClick(object sender, CommandEventArgs e)
    {
        Dictionary<int, string> golferNames = DatabaseFunctions.GetGolferNamesAndIDs(leagueID.ToString());

        DropdownGolfer1.Items.Clear();
        DropdownGolfer2.Items.Clear();

        DropDownGolfer3.Items.Add(new ListItem("None", "0"));

        foreach (int golferID in golferNames.Keys)
        {
            ListItem temp = new ListItem(golferNames[golferID], golferID.ToString());
            DropdownGolfer1.Items.Add(temp);
            DropdownGolfer2.Items.Add(temp);
            DropDownGolfer3.Items.Add(temp);
        }

        ButtonAddSubmit.CommandArgument = "0";
        ModalPopupExtenderAdd.Show();
    }

     
    
}