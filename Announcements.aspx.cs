using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Security;

public partial class Announcements : System.Web.UI.Page
{
    private int leagueID;
    private int currentSeasonID;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isUserCommissioner = false;
        leagueID = (int)((SiteMaster)this.Master).LeagueID;
        currentSeasonID = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        if (leagueID == 3)
        {

            RulesHyperlink.Visible = true;
            RulesHyperlink.NavigateUrl = "http://www.golfleagueinfo.com/2017ProgCOSLeagueRules.htm";
        }

        List<Announcement> announcements = DatabaseFunctions.GetAnnouncements(leagueID);

        foreach (var announcement in announcements)
        {
            AddAnnouncementRow(announcement);
        }

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            var currentUserID = Membership.GetUser().ProviderUserKey.ToString();
            var commisioners = DatabaseFunctions.GetCommissionerIds(leagueID);
            isUserCommissioner = commisioners.Contains(currentUserID);
            if(isUserCommissioner)
            {
                this.ButtonAddAnnouncement.Visible = true;
            }
        }



    }

    private void AddAnnouncementRow(Announcement announcement)
    {
        TableRow row = new TableRow();
        string html = "<div class='card'><div class='card-header'>{Date}</div><div class='card-body'><h5 class='card-title'>{Title}</h5><p class='card-text'>{Announcement}</p></div></div>";
        html = html.Replace("{Date}", announcement.Created.ToShortDateString());
        html = html.Replace("{Title}", announcement.Title);
        html = html.Replace("{Announcement}", announcement.Content);

        TableCell tc = new TableCell();
        tc.ColumnSpan = 2;
        Literal l = new Literal();
        l.Text = html;
        tc.Controls.Add(l);
        row.Cells.Add(tc);
        this.MainTable.Rows.Add(row);
    }


    protected void ModifyAnnouncementsClicked(object sender, CommandEventArgs e)
    {
        //TextBoxFirstName.Text = "";
        //TextBoxLastName.Text = "";
        //TextBoxEmailAddress.Text = "";
        //TextBoxNickName.Text = "";
        //btnSave.CommandArgument = "0";
        ModalPopupExtender1.Show();
    }

    protected void Save(object sender, CommandEventArgs e)
    {
        Announcement announcement = new Announcement();
        announcement.LeagueId = leagueID;
        announcement.SeasonId = DatabaseFunctions.GetCurrentSeasonID(leagueID.ToString());
        announcement.Title = this.TitleTextBox.Text;
        announcement.Content = this.AnnouncementTextBox.Text;
        DatabaseFunctions.AddAnnouncement(announcement);
    }
}