using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GolfersNew : System.Web.UI.Page
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
        List<Golfer> golfers = DatabaseFunctions.GetGolfersInfo(leagueID.ToString()).Values.ToList(); //DatabaseFunctions.GetAllGolfersFullInfo(leagueID).OrderBy(x => x.lastName).ToList();
        grdGolfers.DataSource = golfers;
        grdGolfers.DataBind();
    }

    

   
    protected void Save(object sender, CommandEventArgs e)
    {
        if (true)
        {
            int golferID = int.Parse((string)e.CommandArgument);
            if(golferID == 0)//add
            {
                int newGolferId = DatabaseFunctions.AddGolfer(TextBoxFirstName.Text, TextBoxLastName.Text, TextBoxNickName.Text, TextBoxEmailAddress.Text);
                DatabaseFunctions.AddLeagueAffiliation(newGolferId, leagueID);
            }
            else//edit
            {
                DatabaseFunctions.EditGolfer(golferID, TextBoxFirstName.Text, TextBoxLastName.Text, TextBoxNickName.Text, TextBoxEmailAddress.Text);
            }
            BindDataToGrid();
        }
        else
        {
            Response.Write("<script language='javascript'>alert('Golfer Not saved.  Verify that first/last name is not blank');</script>");
            ModalPopupExtender1.Show();//Keep Score Entry Modal Open.
        }
        
    }    

    
    
    protected void EditButtonClick(object sender, CommandEventArgs e)
    {
        int golferID = int.Parse((string)e.CommandArgument);
        Golfer golferInfo = DatabaseFunctions.GetGolferInfo(golferID);
        TextBoxFirstName.Text = golferInfo.firstName;
        TextBoxLastName.Text = golferInfo.lastName;
        TextBoxEmailAddress.Text = golferInfo.emailAddress;
        TextBoxNickName.Text = golferInfo.nickName;
        btnSave.CommandArgument = golferID.ToString();
        ModalPopupExtender1.Show();
    }

    protected void AddButtonClick(object sender, CommandEventArgs e)
    {
        TextBoxFirstName.Text = "";
        TextBoxLastName.Text = "";
        TextBoxEmailAddress.Text = "";
        TextBoxNickName.Text = "";
        btnSave.CommandArgument = "0";
        ModalPopupExtender1.Show();
    }

     
    
}