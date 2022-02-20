<%@ Page Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Go_To_Previous_Round.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Go_To_Previous_Round" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Button ID="Button3" TabIndex="1" runat="server" Text="Terug naar vorige ronde" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
                <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Weet je het zeker?</asp:Label>
                <asp:Button ID="Button1" TabIndex="2" runat="server" Text="Ja" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label4" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>                

            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="3" runat="server" Text="Onderbreek" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
