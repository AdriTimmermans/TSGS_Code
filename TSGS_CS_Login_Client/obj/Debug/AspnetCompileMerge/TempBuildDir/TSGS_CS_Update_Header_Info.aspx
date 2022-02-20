<%@ Page Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Update_Header_Info.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Update_Header_Info" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Volgende clubavond: </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                
                <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
                
            </td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button1" TabIndex="2" runat="server" Text="Update Header" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
                <asp:Button ID="Button2" TabIndex="3" runat="server" Text="Onderbreek" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
