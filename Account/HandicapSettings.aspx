<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HandicapSettings.aspx.cs" Inherits="HandicapSettings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:Table Style="width: 1040px" CellPadding="10" runat="server" BorderStyle="Solid"
        BorderWidth="5">
        <asp:TableRow HorizontalAlign="Center">
            <asp:TableCell>
                <b style="font-size:16px;color:Black">Averaging</b>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow HorizontalAlign="Center">
            <asp:TableCell>
                <asp:Label runat="server" Text=" Use Best" Font-Bold="true" />
                &nbsp
                <asp:TextBox runat="server" ID="TextBox_BestRounds" Width="60px" Rows="1" />
                &nbsp
                <asp:Label ID="Label1" runat="server" Text="Out of Last" Font-Bold="true" />
                &nbsp
                <asp:TextBox runat="server" ID="TextBox_TotalRounds" Width="60px" Rows="1" />
                &nbsp
                <asp:Label ID="Label2" runat="server" Text="Scores." Font-Bold="true" />
            </asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label5" runat="server" Text="For Players with insufficent rounds: "
                    Font-Bold="true" />
                &nbsp
                <asp:DropDownList runat="server" ID="DropDown_insufficientRounds" Width="60px" Rows="1" />
            </asp:TableCell></asp:TableRow>
    </asp:Table>
    <asp:Table ID="Table1" Style="width: 1040px" CellPadding="10" runat="server" BorderStyle="Solid"
        BorderWidth="5">
        <asp:TableRow HorizontalAlign="Center">
            <asp:TableCell>
                <b style="font-size:16px;color:Black">Score Calculation</b>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow HorizontalAlign="Center">
            <asp:TableCell>
                <asp:Label ID="Label3" runat="server" Text="For Each Round take Score Minus" Font-Bold="true" />
                &nbsp
                <asp:DropDownList runat="server" ID="DropdownScoreBaseline" Width="60px" Rows="1" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label4" runat="server" Text="Maximum Score: " Font-Bold="true" />
                &nbsp
                <asp:DropDownList runat="server" ID="DropdownMaxScore" Width="60px" Rows="1" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table runat="server" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="Button1" runat="server" OnCommand="CreateSeasonButtonPress" Text="Save Settings"
                    Style="font-size: 90%; font-weight: bold; width: 140px; height: 40px; border-style: solid;
                    border-width: thin; border-color: Black; background-image: url(images/CustomButton1.jpg)" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <cc3:NumericUpDownExtender ID="NumericUpDownExtender_BestRounds" runat="server" TargetControlID="TextBox_BestRounds"
        Minimum="1" Maximum="25" Width="100" />
    <cc3:NumericUpDownExtender ID="NumericUpDownExtender_TotalRounds" runat="server"
        TargetControlID="TextBox_TotalRounds" Minimum="1" Maximum="25" Width="100" />
</asp:Content>
