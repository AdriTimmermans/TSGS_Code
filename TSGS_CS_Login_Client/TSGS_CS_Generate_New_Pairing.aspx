<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Generate_New_Pairing.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Generate_New_Pairing" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:UpdatePanel ID="Updatepanel1" runat="Server">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="300" DynamicLayout="true">
                            <ProgressTemplate>
                                <img src="images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="Button1" runat="server" Text="Indelen" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
                        <asp:Button ID="Button2" runat="server" Font-Size="12pt" OnClick="Button2_Click" Text="Upload motivatie" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label3" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Indeling gemaakt</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">
                <asp:Button ID="Button3" runat="server" Text="Done" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
