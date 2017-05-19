using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string LeagueName;
    public int? LeagueID;
    public string currentUserID;

    protected void Page_Init(object sender, EventArgs e)
    {
        //if authenticated populate league name and league id
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            currentUserID = Membership.GetUser().ProviderUserKey.ToString();

            try
            {
                LeagueID = DatabaseFunctions.GetLeagueIDFromLoggedInUserID(currentUserID);
                Session["LeagueID"] = LeagueID;
                LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
                Session["LeagueName"] = LeagueName;
                //redirect to the manage page and put LeagueID in URL
            }
            catch (Exception ex)
            {
                if (!Request.Url.GetLeftPart(UriPartial.Path).Contains("AddLeague"))//Logged in User without a league to manage
                {
                    Response.Redirect("account/AddLeague.aspx", true);
                }
            }


            //if (Session["LeagueID"] == null)
            //{
                
            //}
            //else
            //{
            //    LeagueID = (int)Session["LeagueID"];
            //    LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
            //}
        }
        else//if not authenticated try to get League ID from URL or from Cache(s)
        {

            if (Request.QueryString["LeagueID"] != null)
            {
                LeagueName = DatabaseFunctions.ValidateLeagueID(Request.QueryString["LeagueID"]);
                //Populate League Event Dropdown
                if (LeagueName == null && !Request.Url.GetLeftPart(UriPartial.Path).Contains("NoLeague") && !Request.Url.GetLeftPart(UriPartial.Path).Contains("Login") && !Request.Url.GetLeftPart(UriPartial.Path).Contains("Register"))
                {
                    //Redirect to league selection page
                    Response.Redirect("NoLeague.aspx", true);
                }
                else if (LeagueName != null)
                {
                    LeagueID = int.Parse(Request.QueryString["LeagueID"]);
                    Session["LeagueID"] = LeagueID;
                }

            }
            else if (Session["LeagueID"] != null)//no cached
            {
                LeagueID = (int)Session["LeagueID"];
                LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
               
            }

            //if (Session["LeagueID"] == null)//no cached league ID
            //{
            //    LeagueName = DatabaseFunctions.ValidateLeagueID(Request.QueryString["LeagueID"]);

            //    //Populate League Event Dropdown
            //    if (LeagueName == null && !Request.Url.GetLeftPart(UriPartial.Path).Contains("NoLeague") && !Request.Url.GetLeftPart(UriPartial.Path).Contains("Login") && !Request.Url.GetLeftPart(UriPartial.Path).Contains("Register"))
            //    {
            //        //Redirect to league selection page
            //        Response.Redirect("NoLeague.aspx", true);
            //    }
            //    else if (LeagueName != null)
            //    {   
            //        LeagueID = int.Parse(Request.QueryString["LeagueID"]);
            //        Session["LeagueID"] = LeagueID;

            //        //Add League ID to the menu links
            //        //foreach (MenuItem item in NavigationMenu.Items)
            //        //{
            //        //    if (!item.NavigateUrl.Contains("LeagueID"))
            //        //    {
            //        //        item.NavigateUrl += "?LeagueID=" + LeagueID.ToString();
            //        //    }
            //        //}
            //    }
            //}
            //else
            //{
            //    LeagueID = (int)Session["LeagueID"];
            //    LeagueName = DatabaseFunctions.ValidateLeagueID(LeagueID.ToString());
            //    //Add League ID to the menu links
            //    //foreach (MenuItem item in NavigationMenu.Items)
            //    //{
            //    //    if (!item.NavigateUrl.Contains("LeagueID"))
            //    //    {
            //    //        item.NavigateUrl += "?LeagueID=" + LeagueID.ToString();
            //    //    }
            //    //}
            //}
        }
        if(LeagueName != null)
        {
            GolfLeagueName.Text = "League: " + LeagueName;
        }
        else
        {
            if (!Request.Url.GetLeftPart(UriPartial.Path).Contains("NoLeague"))
            {
                Response.Redirect("NoLeague.aspx", true);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}
