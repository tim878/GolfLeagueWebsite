<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WeeklyResults.aspx.cs" Inherits="WeeklyResults" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="scriptManager" runat="server" />
    <table>
        <tr>
            <td>
                <asp:DropDownList Width="300" ID="Dropdown_Seasons" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdown_Season_SelectedIndexChanged" />
            </td>
            <td>
                <asp:DropDownList Width="300" ID="Dropdown_LeagueEvent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdown_LeagueEvent_SelectedIndexChanged" />
            </td>
            <td>
                <asp:DropDownList Width="300" ID="DropDown_View" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdown_View_SelectedIndexChanged" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:UpdatePanel ID="Panel_Matchups" runat="server"/>
                <asp:UpdatePanel ID="Panel_Skins" runat="server" Visible="false" >
                    <ContentTemplate>
                        <asp:table ID="Table_SkinsResults" runat="server"/>
                        <asp:DropDownList ID="DropdownList_SkinsPlayers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SkinsPlayersChanged" Width="400"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Panel_Leaderboards" runat="server" Visible="false" >
                    <ContentTemplate>
                        <asp:table ID="Table_Leaderboards" runat="server" CellPadding="30">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:table ID="Table_GrossScoreLeaderboard" runat="server"/>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:table ID="Table_NetScoreLeaderboard" runat="server"/>
                                </asp:TableCell>
                                <asp:TableCell VerticalAlign="Top">
                                    <asp:table ID="Table_TeamPts" runat="server"/>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
       <%-- <tr>
            <td>
                <asp:Button ID="Button_RecalculateSkins" runat="server" OnCommand="RecalculateSkinsButtonPress" Text="Recalculate Skins Using Selected Players" Visible="false" style="width:340px;" /> 
            </td>
        </tr>--%>
    </table>
</asp:Content>

