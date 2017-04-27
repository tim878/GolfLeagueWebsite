<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Matchups.aspx.cs" Inherits="Matchups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 500px" cellpadding="20">
        <tr>
            <td >
                 <asp:Panel ID="Panel_Matchups" runat="server" HorizontalAlign=Left/>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:DropDownList ID="Dropdown_Team1" runat=server Width=200/>
            </td>           
        </tr>
        <tr>
           <td>
                  <asp:DropDownList ID="Dropdown_Team2" runat=server Width=200/>
            </td>
        </tr>
        <tr>
            <td>
                  <asp:Button ID="AddMatchupButton" runat="server" OnCommand="AddMatchupButtonClick" Text="Add Matchup" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
         <tr>
            <td  align=center>
                 <asp:Button ID="Button_Save" runat="server" OnCommand="SaveMatchups" Text="Save Matchups" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>  
        </tr>  
    </table>
</asp:Content>