<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" CodeBehind="TSGS_CS_Competition_Overall_Status.aspx.cs" AutoEventWireup="true" Inherits="TSGS_CS_Login_Client.TSGS_CS_Competition_Overall_Status" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label4" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Initialisatie nieuwe ronde uitgevoerd</asp:Label><br />

    <asp:CheckBox ID="CheckBox12" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label3" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Initialisatie nieuwe ronde uitgevoerd</asp:Label><br />

    <asp:CheckBox ID="CheckBox1" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label5" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Afmeldlijst ingevuld</asp:Label><br />

    <asp:CheckBox ID="CheckBox2" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label6" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Indeling gemaakt van  ronde:</asp:Label><br />

    <asp:CheckBox ID="CheckBox4" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label7" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Indeling afgedrukt</asp:Label><br />

    <asp:CheckBox ID="CheckBox5" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label8" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Handindeling gemaakt</asp:Label><br />

    <asp:CheckBox ID="CheckBox6" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label9" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Resultaten verwerkt</asp:Label><br />

    <asp:CheckBox ID="CheckBox7" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label10" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Correcties uitgevoerd</asp:Label><br />

    <asp:CheckBox ID="CheckBox8" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label11" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Competitie ranglijst afgedrukt</asp:Label><br />

    <asp:CheckBox ID="CheckBox9" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label12" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Winst/verlies lijst afgedrukt</asp:Label><br />

    <asp:CheckBox ID="CheckBox10" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" "></asp:CheckBox>
    <asp:Label ID="Label13" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">ELO lijst afgedrukt</asp:Label><br />

    <asp:CheckBox ID="CheckBox11" runat="server" Width="24px" Height="44px" TextAlign="Left" Enabled="False" Text=" " ForeColor="Black"></asp:CheckBox>
    <asp:Label ID="Label14" runat="server" Height="44px" BackColor="Transparent" Font-Size="14pt">Internet publicatie gedaan</asp:Label><br />
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">&nbsp;&nbsp;
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button1" Text="done" runat="server" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

