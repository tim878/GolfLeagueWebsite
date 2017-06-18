<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LeagueHome.aspx.cs" Inherits="LeagueHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
        <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
        <script src="/GolfLeagueWebsite/Scripts/marquee.js"></script>

       <!-- Latest compiled and minified CSS -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

        <script type='text/javascript'>

            var curMarqueeIndex = 0;
            var marqueeArr = [];
            function scrollMarqueeForward(){
                curMarqueeIndex++;
                console.log(curMarqueeIndex);
                setMarqueeContent();
            }
            function scrollMarqueeBackward(){
                curMarqueeIndex--;
                console.log(curMarqueeIndex);
                setMarqueeContent();
            }

            function setMarqueeContent() {
                $(".marquee-content").html(marqueeArr[curMarqueeIndex]);
            }

            $(document).ready(function () {
                var eventString = $("#ScrollLabel1").html();

                var splitStrings = eventString.split("|");
                var eventString = "<span class='marquee-content' id='marquee-header-0'>";
                var curChar = 0;

                for (var i = 0; i < splitStrings.length; i++) {
                    if (splitStrings[i] === "^") { //Allows Score Titles to be in different panels
                        curChar = 101;
                    } else {


                        curChar += splitStrings[i].length;
                        if (curChar > 95) {
                            eventString += "</span>";
                            curChar = 0;
                            marqueeArr.push(eventString);
                            eventString = "<span class='marquee-content' id='marquee-header-" + marqueeArr.length + "'>";
                        }
                        eventString += splitStrings[i] + " ";
                    }
                }
                eventString += "</span>";
                marqueeArr.push(eventString);

                $("#ScrollLabel1").html("<input type='button' class='marquee-button-backward' value='<' onClick='scrollMarqueeBackward()'/>" + marqueeArr[0] + "</span><input type='button' class='marquee-button-forward' value='>' onClick='scrollMarqueeForward()'/>");
                console.log(marqueeArr);

            });
        
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div  style="background-color:#4b6c9e;width:1100px">
        <%--<marquee behavior="scroll" direction="left" width="1100"><asp:Label runat="server" ClientIDMode="static" ID="ScrollLabel1" Font-Bold="true" Font-Size="Larger" ForeColor="White" /></marquee> --%>
        <div id='marquee'>
            <asp:Label runat="server" ClientIDMode="Static" ID="ScrollLabel1" Font-Bold="true" Font-Size="Larger" ForeColor="White" />
        </div>
    </div>
    <table width="1100px" >
        <tr>
            <td style="padding: 10px;">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Next Event</h3>
                    </div>
                    <div class="panel-body">
                         <table>
                                <tr>
                                    <td><asp:Label Font-Bold="true" runat="server" ID="LabelNextEventTitle1">Next Event:</asp:Label></td>
                                    </tr>
                                    <tr>
                                     <td><asp:Label Font-Bold="true" runat="server" ID="LabelNextEventTitle2">Date:</asp:Label></td>
                                </tr>
                                <tr>
                                     <td><asp:Label Font-Bold="true" runat="server" ID="LabelNextEventTitle3">Course:</asp:Label></td>
                                </tr>
                                <tr>
                                    <td> 
                                        <ul id="MatchupsList" runat="server" class="list-group" >
                                        </ul>
                                    </td>
                                   
                                    <%--<td colspan="2">
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
                                    </td>--%>
                                </tr> 
                            </table>
                    </div>
              </div>
               
            </td>
            
            <td style="padding: 10px;">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Recent Announcements/Discussions</h3>
                        </div>
                        <div class="panel-body">
                            Panel content
                        </div>
                    </div>
            </td>    
        </tr>
        <tr>
            <td style="padding: 10px;">
                <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Random Stat</h3>
                        </div>
                        <div class="panel-body">
                            Under Construction
                        </div>
                    </div>
            </td>
            <td style="padding: 10px;">
                <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Standings</h3>
                        </div>
                        <div class="panel-body">
                            Under Construction
                        </div>
                    </div>
            </td>
        </tr>
    </table>
</asp:Content>

