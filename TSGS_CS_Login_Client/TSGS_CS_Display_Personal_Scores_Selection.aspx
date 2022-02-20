<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Personal_Scores_Selection.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Personal_Scores_Selection" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Timer ID="Refresh" runat="server" OnTick="RefreshTick" Enabled="false"></asp:Timer>
    <asp:UpdatePanel ID="ProgressOverview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Label ID="Label2" runat="server" BackColor="Transparent" Height="32px" Visible="false">Generating</asp:Label>
            <asp:DataList ID="DataList1" runat="server" RepeatColumns="4" RepeatDirection="Vertical">
                <ItemTemplate>
                    <asp:CheckBox ID="cb_File_Created" runat="server" Checked='<%#Convert.ToBoolean(Eval("Created")) %>' />
                    <asp:CheckBox ID="cb_File_Uploaded" runat="server" Checked='<%#Convert.ToBoolean(Eval("Uploaded")) %>' />
                    <%#Eval("PlayerName")%>
                    <asp:TextBox ID="tb_Speler_Id" runat="server" Visible="false" Text='<%#Eval("PlayerId")%>'></asp:TextBox>
                </ItemTemplate>
            </asp:DataList>
        </ContentTemplate>
        <asp:Triggers>
            <asp:AsyncPostBackTrigger ControlID="Refresh" EventName="TickRefresh" />
        </asp:Triggers>
    </asp:UpdatePanel>

    <table style="width: 100%;">
        <tr>
            <td class="auto-style1">
                <asp:Button ID="Button11" runat="server" Text="Generate" Font-Size="12pt" OnClick="Button11_Click"></asp:Button>
                <asp:Button ID="Button2" runat="server" Text="Upload" Font-Size="12pt" Visible="false" OnClick="Button2_Click"></asp:Button>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Height="32px" Visible="false">Generation completed</asp:Label>
            </td>
            <td style="text-align: right">
                <asp:Button ID="Button3" runat="server" Text="Done" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
