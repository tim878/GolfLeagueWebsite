<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Announcements.aspx.cs" Inherits="Announcements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:Label ID="constuctionLabel" runat="server" Text=" Page Under Construction. " Font-Size="XX-Large" Font-Bold="true" Visible="true"/>
                <asp:HyperLink ID="RulesHyperlink" runat="server" Visible="false" >League Rules</asp:HyperLink>
            </td>
             
        </tr>
    </table>
</asp:Content>

