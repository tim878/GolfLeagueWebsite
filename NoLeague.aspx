<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NoLeague.aspx.cs" Inherits="NoLeague" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <table style="width: 900px" cellpadding="20">
        <tr>
            <td>
                 <b style="font-size:12px;color:Black">We can't find your League.  Select your League from the Dropdown: </b>              
            </td>
            <td>
                <asp:DropDownList ID="DropdownLeagues" runat="server" AutoPostBack="True" width="200px" OnSelectedIndexChanged="DropdownLeagueSelectedIndexChanged" /> 
            </td>
        </tr>
        <tr>
            <td colspan="2">
                 <b style="font-size:12px;color:Black"> If you would like to signup to manage a league start by creating a User Account.</b>
            </td>
        </tr>
       
       
    </table>
</asp:Content>

