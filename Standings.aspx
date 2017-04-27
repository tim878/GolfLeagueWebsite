<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Standings.aspx.cs" Inherits="Standings" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table Width="1240">
        <tr>
            <td>
                <asp:Panel ID="Panel_Standings"  runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Chart ID="ChartWeeklyStandings" runat="server" Width="1220" Height="800">
                    <Legends>
                        <asp:Legend Name="Default" Alignment="Center" /> 
                    </Legends>
                    <BorderSkin SkinStyle="Emboss" PageColor="Transparent" />
                    <Titles>
                            <asp:Title Name="Title1" Text="Week By Week Standings (Points Behind)" Font="18" />
                    </Titles>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>   
            </td>
        </tr>
        <tr>
            <td>
                  <asp:table ID="Table_WeeklyScores" runat="server"/>
            </td>
        </tr>
    </table>
</asp:Content>

