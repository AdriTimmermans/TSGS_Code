<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Cleanup.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Cleanup" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header boven topic content -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Clean up </asp:Label>,
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Beheerders functie</asp:Label>

<!-- Topic content, gridview, etc. etc.-->
<!--   -->
    <br />
    <br />
    <br />
        <br />
    <asp:CheckBox ID="CheckBox1" TabIndex="12" runat="server" Text="Verwijderen work images" TextAlign="Right"></asp:CheckBox>    <br />    <br />
    <asp:CheckBox ID="CheckBox2" TabIndex="12" runat="server" Text="Verwijderen oude competitie nummer:" TextAlign="Right"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox5" runat="server" Visible="false"></asp:TextBox><asp:DropDownList ID="DDLCompetitions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_SelectCompetition" Height="17px" Width="400px"></asp:DropDownList>
<br />    <br />
    <asp:CheckBox ID="CheckBox3" TabIndex="12" runat="server" Text="Verwijderen spelers" TextAlign="Right"></asp:CheckBox><br />    <br />
    <asp:CheckBox ID="CheckBox4" TabIndex="12" runat="server" Text="Backup maken" TextAlign="Right"></asp:CheckBox><br />    <br />
    <asp:CheckBox ID="CheckBox6" TabIndex="12" runat="server" Text="Vertalen" TextAlign="Right"></asp:CheckBox>

        <br />    <br />

<!-- Einde Topic content -->

<!-- Footer van contenct: links een voortgangsmelding, rechts de verwerkingsbuttons -->

    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
		<asp:Button ID="Button2" runat="server" Text="Uitvoeren" Font-Size="12pt" OnClick="Button2_Click"></asp:Button> 
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
<!-- einde content block

<!-- additional footer buttons, Button 1 text = algemeen: cancel, button 1 text is verwerk topic/update, upload, tc. etc. -->
                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>            
	</td>
        </tr>
    </table>
</asp:Content>
