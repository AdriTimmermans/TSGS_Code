<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Create_email_to_application_manager.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Create_email_to_application_manager"%>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header boven topic content -->

    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Competitie ...</asp:Label>,
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Deelnemerlijst</asp:Label>    <br />
    <table style="width: 100%">
        <tr>
            <td><asp:TextBox ID="TextBox1" runat="server" Height="237px" TextMode="MultiLine" Rows="10" Width="612px"></asp:TextBox>  </td>
            <td><asp:Image ID="Image2" runat="server" AlternateText="Afbeelding niet gevonden" ImageUrl="~/images/Person_CoffeeBreak_Male_Dark.png" Width="250"></asp:Image></td>
        </tr>  
    </table>
<!-- Einde Topic content -->

<!-- Footer van contenct: links een voortgangsmelding, rechts de verwerkingsbuttons -->

    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">
                    &nbsp;</td>
            <td style="text-align: right">
<!-- einde content block

<!-- additional footer buttons, Button 1 text = algemeen: cancel, button 1 text is verwerk topic/update, upload, tc. etc. -->
		<asp:Button ID="Button2" runat="server" Text="Send email" Font-Size="12pt" OnClick="Button2_Click"></asp:Button> 
                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>            
	</td>
        </tr>
    </table>
</asp:Content>
