<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Login_Page.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Login_Page" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%">
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Width="192px" Height="32px" Font-Size="14pt" Font-Names="Arial" BackColor="Transparent">Gebruikersnaam:</asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="TextBox12" runat="server" Height="32px" Width="280px" AutoPostBack="false" OnTextChanged="OnTextBoxMandatoryChanged"  ></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Width="192px" Height="32px" Font-Size="14pt" Font-Names="Arial">Wachtwoord:</asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="TextBox13" runat="server" Height="32px" Width="120px" AutoPostBack="false" TextMode="Password" OnTextChanged="OnTextBoxMandatoryChanged"  ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Width="138px" Height="32" Text="Login" TabIndex="3" OnClick="Button1_Click1"></asp:Button>

            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label11" runat="server" Width="192px" Height="32px" Visible="false" BackColor="Transparent">Lettergrootte:</asp:Label>

            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="TextBox11" runat="server" OnTextChanged="OnIntTextBoxChanged" AutoPostBack="false" Width="40px" Height="32px"  ></asp:TextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="4" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

