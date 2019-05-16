<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Announcements.aspx.cs" Inherits="Announcements" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
      <%-- <link href="~/Styles/Bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-theme.css" rel="stylesheet" />
    <link href="~/Styles/bootstrap-overrides.css" rel="stylesheet" />--%>

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">

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

    <%--styling for buttons--%>
    <%-- <script type="text/javascript">
         $(function () {
             $("input[type=submit]").button();
         });
     </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>  
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style = "display:none">
        <div style="display:flex; flex-direction:column">
            <asp:Label runat="server" style="font-size:x-large;margin:20px">Add Announcement</asp:Label>
            <div>
                <asp:Label runat="server">Title</asp:Label> <asp:TextBox runat="server" style="margin:10px" ID="TitleTextBox"></asp:TextBox>
            </div>
           <asp:TextBox runat="server" style="margin:10px; width:400px;height:150px" ID="AnnouncementTextBox"></asp:TextBox>
            <div>
                <asp:Button ID="ButtonSave" runat="server" Text="Save" OnCommand="Save" /><asp:Button ID="btnClose" runat="server" Text="Close" />
            </div>
        </div>
        
    </asp:Panel>

    <asp:table runat="server" ID="MainTable" Width="900px" CellPadding="10">
        <asp:TableRow>
            <asp:TableCell Height="70px">
                <asp:HyperLink ID="RulesHyperlink" runat="server" Visible="false" Font-Size="X-Large" Style="margin:10px; margin-bottom:30px">League Rules</asp:HyperLink>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnShow" runat="server" Text="" style = "display:none" />
                <asp:Button ID="ButtonAddAnnouncement" Visible="false" Text="Add Announcement" runat="server" OnCommand="ModifyAnnouncementsClicked" />
            </asp:TableCell>
        </asp:TableRow>
        <%--<asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <div class="card">
                  <div class="card-header">
                    5/4/2019
                  </div>
                  <div class="card-body">
                    <h5 class="card-title">Title</h5>
                    <p class="card-text">Announcement text. text text text text text text text text text text text text</p>
                  </div>
                </div>
            </asp:TableCell>
        </asp:TableRow>--%>
    </asp:table>
</asp:Content>

