<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_ZS_Attendance_Registration.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_ZS_Attendance_Registration" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="14pt" Height="24px" Width="700px">Ronde nummer: </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="14pt" Width="700px">Aantal deelnemers:</asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button3" runat="server" Height="44px" Font-Size="18pt" Width="288px" Text="Zet iedereen op  aanwezig" OnClick="Button3_Click"></asp:Button>&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <table id="Table1" border="0">
        <tr>
            <td>
                <asp:DataList ID="DataList1" runat="server" RepeatColumns="5" RepeatDirection="Vertical" OnItemDataBound="DataList1_ItemBound">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnAfwezigChanged" Text=" " />
                        &nbsp;<%#Eval("SpelerNaam")%>
                        <asp:TextBox ID="tb_Speler_Id" runat="server" Visible="false" Text='<%#Eval("Speler_Id")%>'></asp:TextBox>
                    </ItemTemplate>

                </asp:DataList>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Weet je het zeker?</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
                <asp:Button ID="Button2" TabIndex="2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
