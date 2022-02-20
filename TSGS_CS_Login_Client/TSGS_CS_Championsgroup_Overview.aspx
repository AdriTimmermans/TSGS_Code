<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Championsgroup_Overview.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Championsgroup_Overview" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->

    <table style="width: 100%">
        <tr>
            <td colspan="3">
                <asp:Literal ID="ltrl_Html" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
                <asp:Button ID="Button2" runat="server" Text="Upload" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>             </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

<!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>            </td>
        </tr>
    </table>
</asp:Content>
