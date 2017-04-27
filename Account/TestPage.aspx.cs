using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

public partial class TestPage : System.Web.UI.Page
{
    private string currentUserID;
    private int leagueID;
    Dictionary<int, string> Courses;
    Dictionary<int, int> SelectedMatchups = new Dictionary<int,int>();
    Dictionary<int, string> Teams;
   
   
    protected void Page_Load(object sender, EventArgs e)
    {
        

    }

    
    
}