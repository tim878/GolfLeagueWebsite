<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageLeaguesNew.aspx.cs" Inherits="ManageLeaguesNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <table style="width: 300px" cellpadding="5">
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonSeasons" ImageUrl="~/images/seasons.jpg" Width="60" ToolTip="Manage Seasons" />               
            </td>
            <td>
                <asp:Label runat="server" ID="LabelSeasons"/> 
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonGolfers" ImageUrl="~/images/golfers.png" Width="60" ToolTip="Manage Golfers" />  
            </td>
            <td>
                <asp:Label runat="server" ID="LabelGolfers"/> 
            </td>   
        </tr>
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonTeams" ImageUrl="~/images/team.png" Width="60" ToolTip="Manage Teams" />  
            </td>
            <td>
                <asp:Label runat="server" ID="LabelTeams"/> 
            </td>   
        </tr>
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonEvents" ImageUrl="~/images/events.png" Width="60" ToolTip="Create and Edit Events" />  
            </td>
            <td>
                <asp:Label runat="server" ID="LabelEvents"/> 
            </td>   
        </tr>
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonScores" ImageUrl="~/images/scorecard.png" Width="60" ToolTip="Enter or Edit Scores" />  
            </td>
            <td>
                <asp:Label runat="server" ID="LabelScores"/> 
            </td>   
        </tr>
        <tr align="center">
            <td>
                <asp:ImageButton runat="server" ID="ImageButtonSettings" ImageUrl="~/images/settings.png" Width="60" ToolTip="Manage League and Scoring Settings" />  
            </td>
               
            <td >
                <asp:ImageButton runat="server" ID="ImageButtonHelp" ImageUrl="~/images/help.png" Width="60" ToolTip="Help" />  
            </td>              
        </tr>     
      
    </table>

</asp:Content>

