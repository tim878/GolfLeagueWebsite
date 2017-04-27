<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LeagueHome.aspx.cs" Inherits="LeagueHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
        <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
        <script src="/GolfLeagueWebsite/Scripts/marquee.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div  style="background-color:#4b6c9e;width:1100px">
        <marquee behavior="scroll" direction="left" width="1100"><asp:Label runat="server" ClientIDMode="static" ID="ScrollLabel1" Font-Bold="true" Font-Size="Larger" ForeColor="White" /></marquee>
    </div>
    <table width="1100px">
        <tr>
            <td>
                <table>
                    <tr>
                        <td><asp:Label runat="server" ID="LabelNextEventTitle1">Next Event: Week 1</asp:Label></td>
                        </tr>
                        <tr>
                         <td><asp:Label runat="server" ID="LabelNextEventTitle2">Date:4/3/2016    Course:Hiawatha-Front Nine</asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                             <asp:GridView ID="grdNextEventMatchups" runat="server" 
                                CssClass="Grid"                    
                                AlternatingRowStyle-CssClass="alt"
                                PagerStyle-CssClass="pgr"
                                AutoGenerateColumns="false"
                                EmptyDataText="No Matchups Found"
                                >
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                <asp:TemplateField >
                                    <HeaderTemplate>
                                        <asp:Label ID="lbTitle" runat="server"  Text="Matchups" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("Matchup")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                             </asp:GridView>
                        </td>
                    </tr> 
                </table>
            </td>
            <td>
                
            </td>    
        </tr>
        <tr>
            <td>
                
            </td>
            <td>
                
            </td>
        </tr>
    </table>
</asp:Content>

