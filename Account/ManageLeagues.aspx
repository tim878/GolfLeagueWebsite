<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageLeagues.aspx.cs" Inherits="Account_ManageLeagues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <table style="width: 900px" cellpadding="20">
        <tr>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddScores"> Enter Scores </asp:HyperLink>
                
            </td>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddScoresNew"> Enter Scores(New) </asp:HyperLink>
                
            </td>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddPlayers"> Golfers  </asp:HyperLink>
            </td>   
        </tr>
        <tr>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddTeam"> Teams </asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddEvent"> Add Events  </asp:HyperLink>
                
            </td> 
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_AddEventNew"> New Events Page  </asp:HyperLink>
                
            </td> 
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_TestPage"> Test Page  </asp:HyperLink>
            </td>       
        </tr>
        <tr>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_CreateNewSeason"> Create New Season  </asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_Handicaps">Modify Handicap Settings </asp:HyperLink>    
            </td>   
        </tr>     
       
        <tr>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_GenerateSchedule"> Generate Schedule  </asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink runat=server Target="" ID="Link_ManageLeaguesNew"> Manage Leagues(New)  </asp:HyperLink>
            </td>   
        </tr>               
       
       
    </table>

</asp:Content>

