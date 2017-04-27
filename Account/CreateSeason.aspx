<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CreateSeason.aspx.cs" Inherits="CreateSeason" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width: 900px" cellpadding="20">
        <tr>
            <td>
                 <b style="font-size:12px;color:Black">Season Name (ex. Summer 2009)</b>
            </td>
            <td>
                <asp:TextBox ID="TextBox_SeasonName" runat="server" Width="200px" /> 
            </td>
            <td>
                <asp:Checkbox ID="Checkbox_CurrentSeason" runat="server" Width="200px" Text="Make Current Season" Checked=true /> 
            </td> 
        </tr>
        <tr>
            <td>
                <b style="font-size:12px;color:Black">Select Teams</b>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Panel ID="Panel_Teams" runat="server" HorizontalAlign=Left/>
            </td>
        </tr>
        <tr>
            <td colspan="3" align=center>
               <asp:Button ID="Button_Save" runat="server" OnCommand="CreateSeasonButtonPress" Text="Create Season" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
       
    </table>
</asp:Content>

