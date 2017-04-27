<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Events_Updated.aspx.cs" Inherits="Events_Updated" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: auto; 
        }
    </style>
    
    <link href="../Styles/Grid.css" rel="stylesheet" />

    <%--<link href="~/Styles/Bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-theme.css" rel="stylesheet" />
    <link href="~/Styles/bootstrap-overrides.css" rel="stylesheet" />--%>

    <link href="http://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet"/>
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

   <%-- Use JQuery Styling for Datepickers and Buttons--%>
    <script type="text/javascript">
        $(function () 
        {
            $("#<%= datepicker.ClientID  %>").datepicker();
        });
    </script>
    
    <script type="text/javascript">
        $(function () {
            $("input[type=submit]").button();
        });
    </script>


    <style type="text/css">
          #TeamOneList, #TeamTwoList, #AvailableTeamsList{
            border: 1px solid #eee;
            width: 352px;
            min-height: 20px;
            list-style-type: none;
            margin: 0;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
          }
          #vsList {
            border: 1px solid #eee;
            width: 52px;
            min-height: 20px;
            list-style-type: none;
            margin: 0;
            padding: 5px 0 0 0;
            float: left;
            margin-right: 10px;
          }
          #TeamOneList li, #TeamTwoList li{
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 1.2em;
            width: 330px;
          }
          #vsList li{
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 1.2em;
            width: 40px;
          }
  </style>

  <script type="text/javascript">
      $(function () {
          $("#TeamOneList, #TeamTwoList, #AvailableTeamsList").sortable({
              connectWith: ".connectedSortable",
              stop: function (event, ui) {
                  var getTeamOneList = "";
                  var getTeamTwoList = "";

                  $("#TeamOneList .ui-state-default").each(function (index, element) 
                  {
                      //getTeamOneList += element.innerHTML + ',';
                      getTeamOneList += element.getAttribute("teamID") + ',';
                  });
                  $("#TeamTwoList .ui-state-default").each(function (index, element) {
                      getTeamTwoList += element.getAttribute("teamID") + ',';
                  });

                  $("#<%= TeamOneListValues.ClientID %>").val(getTeamOneList);
                  $("#<%= TeamTwoListValues.ClientID %>").val(getTeamTwoList); 
              },
              create: function (event, ui) {
                  var getTeamOneList = "";
                  var getTeamTwoList = "";

                  $("#TeamOneList .ui-state-default").each(function (index, element) {
                      //getTeamOneList += element.innerHTML + ',';
                      getTeamOneList += element.getAttribute("teamID") + ',';
                  });
                  $("#TeamTwoList .ui-state-default").each(function (index, element) {
                      getTeamTwoList += element.getAttribute("teamID") + ',';
                  });

                  $("#<%= TeamOneListValues.ClientID %>").val(getTeamOneList);
                  $("#<%= TeamTwoListValues.ClientID %>").val(getTeamTwoList);
              }
          }).disableSelection();
      });
  </script>
      
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>      


<cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style = "display:none">
    <asp:GridView ID="GrdViewMatchups" runat="server" 
                    AutoGenerateColumns="false"
                    EmptyDataText="No Matchups."
                    AllowSorting="false">
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbTeam1" runat="server"  Text="Team 1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Team1")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbTeam2" runat="server"  Text="Team 2" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("Team2") %>
                            </ItemTemplate>
                        </asp:TemplateField>                    
                    </Columns>
                   <%-- <PagerStyle CssClass="pgr"></PagerStyle>--%>
                </asp:GridView>
    <asp:Button ID="btnClose" runat="server" Text="Close" />
</asp:Panel>

<cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel2" TargetControlID="btnShow2" CancelControlID="Button_Cancel" BackgroundCssClass="modalBackground" BehaviorID="popup2">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="center" style = "display:none">
    <table>
        <tr>
            <td>
                <table style="width: 450px" cellpadding="10">
                    <tr>
                       <td>
                             <b style="font-size:12px;color:Black">Event Name</b>&nbsp &nbsp &nbsp
                             <b style="font-size:12px;color:Black">(ex. Week 1)</b>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_EventName" runat="server" Width="200px" /> 
                        </td>
                    </tr>    
                    <tr>
                        <td>
                             <b style="font-size:12px;color:Black">Date</b>
                        </td>
                        <td>
                         <asp:TextBox runat="server" type="text" id="datepicker"/>
                           <%-- <p>Date: <input type="text" id="datepicker"></p>--%>
                            <%--<asp:Calendar ID="Calendar_Date" runat="server" Width="200px" Visible="false"/> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b style="font-size:12px;color:Black">Select Course</b>
                        </td>
                        <td>
                             <asp:DropDownList Width="300" ID="Dropdown_Course" runat="server" />
                        </td>
                    </tr>      
                </table>
            </td>
            <td> 
                <asp:Label Text="Select Number of Matchups" runat="server" ID="LabelSelectNumMatchups"/>             
                <asp:DropDownList Width="200" ID="DropDownNumberOfMatchups" runat="server" OnSelectedIndexChanged="Dropdown_NumberOfMatchups_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="2" Value="2"/>
                <asp:ListItem Text="3" Value="3"/>
                <asp:ListItem Text="4" Value="4"/>
                <asp:ListItem Text="5" Value="5"/>
                <asp:ListItem Text="6" Value="6"/>
                <asp:ListItem Text="7" Value="7"/>
                <asp:ListItem Text="8" Value="8"/>
                <asp:ListItem Text="9" Value="9"/>
                <asp:ListItem Text="10" Value="10"/>
                <asp:ListItem Text="11" Value="11"/>
                <asp:ListItem Text="12" Value="12"/>
                <asp:ListItem Text="13" Value="13"/>
                <asp:ListItem Text="14" Value="14"/>
                <asp:ListItem Text="15" Value="15"/>
                <asp:ListItem Text="16" Value="16"/>
                </asp:DropDownList>
                 
               <input type="hidden"  id="TeamOneListValues" name="TeamOneListValues" runat="server" />
               <input type="hidden"  id="TeamTwoListValues" name="TeamTwoListValues" runat="server"/>
                      
                <div style="float:none" id="MatchupsDiv" runat="server">
                    
                        <ul id="TeamOneList" class="connectedSortable" style="float:left" runat="server" clientidmode="Static">
                           <%-- <li class="ui-state-default" teamID="1">Team 1</li>
                            <li class="ui-state-default" teamID="2">Team 2</li>
                            <li class="ui-state-default" teamID="3">Team 3</li>--%>
                        </ul>

                        <ul id="vsList" style="float:left" runat="server" clientidmode="static">
                           <%-- <li class="ui-state-highlight">vs.</li>
                            <li class="ui-state-highlight">vs.</li>
                            <li class="ui-state-highlight">vs.</li>--%>
                        </ul>

                        <ul id="TeamTwoList" class="connectedSortable" style="float:left" runat="server" clientidmode="Static">
                            <%--<li class="ui-state-default" teamID='4'>Team 4</li>
                            <li class="ui-state-default" teamID='5'>Team 5</li>
                            <li class="ui-state-default" teamID='6'>Team 6</li>--%>
                        </ul>
                        <ul id="AvailableTeamsList" class="connectedSortable" style="float:left" runat="server" clientidmode="Static">                           
                        </ul>
                </div>              
            </td>
           
        </tr>
        <tr>
            <td align=center>
                <asp:Button ID="Button_Save" runat="server" OnCommand="SaveLeagueEventButtonPress" CommandName="Add" Text="Save Event" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
            <td align=center>
                <asp:Button ID="Button_Cancel" runat="server" Text="Cancel" style="font-size:90%;font-weight:bold;width:140px;height:40px;border-style:solid;border-width:thin;border-color:Black; background-image:url(images/CustomButton1.jpg)" /> 
            </td>
        </tr>
    </table>
</asp:Panel>
       
     <asp:table ID="Table_MainContent" runat="Server" style="width: 900px" cellpadding="2">
       <asp:TableRow>
            <asp:TableCell ColumnSpan="5">
                <asp:Label ID="Label_ExistingEvents" Text="Scheduled Events" Font-Bold="true" Font-Size="16" runat="server"/> 
                <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
                <asp:Button ID="btnShow2" runat="server" Text="" style = "display:none" />
            </asp:TableCell>
        </asp:TableRow>   
     </asp:table>   
        
     <asp:GridView ID="grdScheduledEvents" runat="server" 
                    CssClass="Grid"                    
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"
                    AutoGenerateColumns="false"
                    EmptyDataText="No Scheduled Events."
                    AllowSorting="false"                   >
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbName" runat="server"  Text="Event Name" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("EventName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderTemplate>
                                <asp:Label ID="lbDate" runat="server"  Text="Date" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# (string)Eval("Date") == "" ?  "" :Convert.ToDateTime((string)Eval("Date")).ToString("d")%>
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
                                <asp:Label ID="lbHasScores" runat="server"  Text="Scores Posted"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("HasScores") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                
                            </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:Button Text="View Matchups" runat="server" CommandName="View Matchups" CommandArgument=<%#Eval("EventID") %> OnCommand="ViewMatchupButtonClick" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button Text="Edit" class="btn btn-primary" runat="server" CommandName="Edit Event" CommandArgument=<%#Eval("EventID") %> OnCommand="EditEventButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button Text="Delete" class="btn btn-primary" runat="server" CommandName="Delete Event" CommandArgument=<%#Eval("EventID") %> OnCommand="DeleteEventButtonClick" />    
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
               <%-- <asp:Button ID="Button1" Text="Add New Event" runat="server" OnClientClick="javascript:$find('popup2').show();return false;" />--%>
                <asp:Button ID="AddEventButton" Text="Add New Event" runat="server" OnCommand="AddEventButtonClick" />
                <hr />
     

    
</asp:Content>