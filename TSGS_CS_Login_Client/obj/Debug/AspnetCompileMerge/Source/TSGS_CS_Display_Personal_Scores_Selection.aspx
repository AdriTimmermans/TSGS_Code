<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Personal_Scores_Selection.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Personal_Scores_Selection" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server"></asp:UpdateProgress>
                </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">
                <asp:Timer ID="Timer2" runat="server" Interval= "1000" OnTick="Timer2_Tick">
                </asp:Timer>
                <asp:Button ID="Button2" runat="server" Text="Upload" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
                <asp:Button ID="Button3" runat="server" Text="Done" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
