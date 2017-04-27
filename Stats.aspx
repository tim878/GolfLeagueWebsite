<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Stats.aspx.cs" Inherits="Stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link href="Styles/Grid.css" rel="stylesheet" />

    <%--<script src="Scripts\stats.js" language="javascript" type="text/javascript"></script>--%>
   <%-- <script type="text/javascript">
        function ShowTable(control)
        {
            HideAllControls();
            var element = document.getElementById(control.id);
            element.style.display = 'block';
        }

        function HideAllControls()
        {
            document.getElementById('MainContent_Table_AverageScores').style.display = 'none';
            document.getElementById('MainContent_Table_LowestGrossSingleRoundScores').style.display = 'none';
            document.getElementById('MainContent_Table_LowestNetSingleRoundScores').style.display = 'none';
            document.getElementById('MainContent_Panel_AverageScoresByCourse').style.display = 'none';
            document.getElementById('MainContent_Table_PtsScored').style.display = 'none'; 
            document.getElementById('MainContent_Table_Skins').style.display = 'none';
            document.getElementById('MainContent_Table_Birdies').style.display = 'none';
            document.getElementById('MainContent_Table_Pars').style.display = 'none';
            document.getElementById('MainContent_Table_Bogeys').style.display = 'none';
            document.getElementById('MainContent_Table_DoubleBogeys').style.display = 'none';
            document.getElementById('MainContent_Table_Eagles').style.display = 'none';
            document.getElementById('MainContent_Table_Par3Scoring').style.display = 'none';
            document.getElementById('MainContent_Table_Par4Scoring').style.display = 'none';
            document.getElementById('MainContent_Table_Par5Scoring').style.display = 'none';
            document.getElementById('MainContent_Panel_HoleScores').style.display = 'none';

            document.getElementById('MainContent_CourseStats').style.display = 'none';
            document.getElementById('MainContent_Graph').style.display = 'none';
            document.getElementById('MainContent_Panel_AllScores').style.display = 'none';
        }
    </script>--%>

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
   <asp:ScriptManager runat="server">
   </asp:ScriptManager>
    <table cellpadding="20px">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Season Stats" Font-Bold="true" Font-Size="16" runat="server"/> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:DropDownList Width="200" ID="Dropdown_Seasons" runat="server" AutoPostBack="False" OnSelectedIndexChanged="Dropdown_Season_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                 <asp:Menu ID="NavigationMenu" runat="server" CssClass="menu" EnableViewState="false" Orientation="Vertical"  OnMenuItemClick="StatsMenu_MenuItemClick" >
                                    <staticselectedstyle backcolor="#bfcbd6" ForeColor="#465c71" borderstyle="Solid" bordercolor="Black" borderwidth="1"/>
                                    <Items>
                                        <asp:MenuItem Text="Average Score All Courses"  Value="AvgAll"   />
                                        <asp:MenuItem Text="Lowest Net Rounds"          Value="LowNet"    />
                                        <asp:MenuItem Text="Lowest Gross Rounds"        Value="LowGross"    />
                                        <asp:MenuItem Text="Average Score By Course"    Value="AvgByCourse"    />
                                        <asp:MenuItem Text="Individual Points Scored"   Value="IndvPts"    />
                                        <asp:MenuItem Text="Skins"                      Value="Skins"     />
                                        <asp:MenuItem Text="Birdies"                    Value="Birdies"     />
                                        <asp:MenuItem Text="Pars"                       Value="Pars"     />
                                        <asp:MenuItem Text="Bogeys"                     Value="Bogeys"     />
                                        <asp:MenuItem Text="Double Bogeys"              Value="Double Bogeys"     />
                                        <asp:MenuItem Text="Eagles"                     Value="Eagles"      />
                                        <asp:MenuItem Text="Par 3 Scoring"              Value="Par3"     />
                                        <asp:MenuItem Text="Par 4 Scoring"              Value="Par4"    />
                                        <asp:MenuItem Text="Par 5 Scoring"              Value="Par5"     />
                                        <asp:MenuItem Text="Individual Hole Stats"      Value="IndividualHoles"      />
                                    </Items>
                                 </asp:Menu>
                             </ContentTemplate>
                             <Triggers>
                                <asp:PostBackTrigger ControlID="NavigationMenu" />
                             </Triggers>
                              </asp:UpdatePanel>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Individual Stats" Font-Bold="true" Font-Size="16" runat=server OnSelectedIndexChanged="Dropdown_Season_SelectedIndexChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList Width="200" ID="DropDown_PlayerSelect" runat="server" AutoPostBack="False" OnSelectedIndexChanged="Dropdown_Player_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Menu ID="IndividualStatsMenu" runat="server" CssClass="menu" EnableViewState="false" Orientation="Vertical" OnMenuItemClick="StatsMenu_MenuItemClick"  >
                                <Items>
                                    <asp:MenuItem Text="Course Statistics" Value="CourseStats" />
                                    <asp:MenuItem Text="Graph" Value="Graph" />
                                    <asp:MenuItem Text="Event Scores" Value="EventScores"/>
                                </Items>
                             </asp:Menu>
                        </td>
                    </tr>
                </table>   
            </td>
           <td>
           
          
           
                   <%-- League Stats--%>
                   <asp:table ID="Table_AverageScores"                  runat="server" style="display:none"/>
                   <asp:table ID="Table_LowestGrossSingleRoundScores"   runat="server" style="display:none"/>
                   <asp:table ID="Table_LowestNetSingleRoundScores"     runat="server" style="display:none"/>
                   <asp:Panel ID="Panel_AverageScoresByCourse"          runat="server" style="display:none"/>
                   <asp:table ID="Table_PtsScored"                      runat="server" style="display:none"/>
                   <asp:table ID="Table_Skins"                          runat="server" style="display:none"/>
                   <asp:table ID="Table_Birdies"                        runat="server" style="display:none"/>
                   <asp:table ID="Table_Pars"                           runat="server" style="display:none"/>
                   <asp:table ID="Table_Bogeys"                         runat="server" style="display:none"/>
                   <asp:table ID="Table_DoubleBogeys"                   runat="server" style="display:none"/>
                   <asp:table ID="Table_Eagles"                         runat="server" style="display:none"/>
                   <asp:table ID="Table_Par3Scoring"                    runat="server" style="display:none"/>
                   <asp:table ID="Table_Par4Scoring"                    runat="server" style="display:none"/>
                   <asp:table ID="Table_Par5Scoring"                    runat="server" style="display:none"/>
                   <asp:panel ID="Panel_HoleScores"                     runat="server" style="display:none"/>
                   <%--Win loss records
                   Most points on individual basis --%>

                   <%--Individual Stats--%>
                   <asp:panel ID="CourseStats"      runat="server" style="display:none"/> <%--Scorecard shows: Best Score, Worst Score, Average Score, Number of Skins--%>
                   <asp:panel ID="Graph"            runat="server" style="display:none"/> <%--Graph shows average score and handicap over time--%>
                   <asp:panel ID="Panel_IndividualScores"  runat="server" style="display:none">
                         <asp:Label ID="lbGridTitle" runat="server"  Text="Golfer Name" Font-Bold="true" Font-Size="X-Large"/>
                         <asp:GridView ID="gridScores" runat="server" 
                                CssClass="Grid"                    
                                AlternatingRowStyle-CssClass="alt"
                                PagerStyle-CssClass="pgr"
                                AutoGenerateColumns="false"
                                EmptyDataText="No Scores for Selected Season(s)"
                                CellPadding="20">
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField >
                                        <HeaderTemplate>
                                            <asp:Label ID="lbScore" runat="server"  Text="Score" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("Score")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbDate" runat="server"  Text="Date"/>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                           <%# ((DateTime)Eval("Date")).ToString("d")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                         <HeaderTemplate>
                                            <asp:Label ID="lbCourse" runat="server"  Text="Course"/>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("CourseName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                         <HeaderTemplate>
                                            <asp:Label ID="lbSeason" runat="server"  Text="Season"/>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("SeasonName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbEventName" runat="server"  Text="EventName"/>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("EventName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>      
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbHandicap" runat="server"  Text="Handicap"/>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("Handicap") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>             
                                </Columns>
                            </asp:GridView>

                   </asp:panel>
                    <%--shows all scores entered for this player and links to details--%>
                   <%--<asp:panel ID="Panel_Handicaps" runat="server" style="display:none"/>--%> <%--table of handicaps and scores--%>

                  
            
            </td>
            
        </tr>
        <%--<tr>
            <td>
                <button onclick="toggleVisibility('testDiv'); return false;"  />
            </td>
        </tr>--%>
    </table>
    

    <%--<asp:Panel ID="Panel_LeagueStats" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label Text="Average Scores All Courses -" Font-Bold="true" Font-Size="14" runat=server/>  
                            </td>
                            <td>
                                <button onclick="toggleVisibility('MainContent_Table_AverageScores'); return false;" style="background-image:url(images/expand_blue.jpg);height:13px; width:13px" />
                            </td>
                        </tr>
                    </table>             
                </td>
                <td>
                    
                </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td valign=top>
                    <asp:table ID="Table_AverageScores" runat="server" Visible="false"/>
                </td>
                <td valign=top>
                    <asp:table ID="Table_LowestGrossSingleRoundScores" runat="server"/>
                </td>
                <td valign=top>
                    <asp:table ID="Table_LowestNetSingleRoundScores" runat="server"/>
                </td>
            </tr>
            <tr>
                <td colspan=3>
                    <asp:Panel ID="Panel_AverageScoresByCourse" runat="server"/>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:table ID="Table_Skins" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_Birdies" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_Pars" runat="server"/>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:table ID="Table_Bogeys" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_DoubleBogeys" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_Eagles" runat="server"/>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:table ID="Table_Par3Scoring" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_Par4Scoring" runat="server"/>
                </td>
                <td>
                    <asp:table ID="Table_Par5Scoring" runat="server"/>
                </td>
            </tr>
            <tr>
                <td colspan=3>
                    <asp:panel ID="Panel_HoleScores" runat="server"/>
                </td>
            </tr>
        </table>

    </asp:Panel>
    
    <asp:Panel ID="Panel_IndividualStats" runat="server" Visible="false">
    </asp:Panel>--%>

    
</asp:Content>

