using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            Response.Redirect("account/ManageLeaguesNew.aspx");
            //currentUserID = Membership.GetUser().ProviderUserKey.ToString();
            ////try to look up the league for this user, if one is not found redirect to the Add League Page
            //try
            //{
            //    leagueID = DatabaseFunctions.GetLeagueIDFromLoggedInUserID(currentUserID);
            //    //redirect to the manage page and put LeagueID in URL
                
            //}
            //catch(Exception ex)
            //{
            //    Response.Redirect("account/AddLeague.aspx", true);
            //}
            //Response.Redirect("Account/ManageLeagues.aspx?LeagueID=" + leagueID.ToString(), true);
        }
        else
        {
            Response.Redirect("NoLeague.aspx");
        }
    }
}
