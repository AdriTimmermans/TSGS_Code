<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_Alarm_Panel.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_Alarm_Panel" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Competitie ...</asp:Label>
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Deelnemerlijst</asp:Label>

    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <asp:Label ID="RDate" runat="server" Width="150px" Text='<%#Eval("Dated", "{0:dd-MM HH:mm:ss}")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Competitie ID">
                <ItemTemplate>
                    <asp:Label ID="CID" runat="server" Text='<%#Eval("CID")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Module">
                <ItemTemplate>
                    <asp:Label ID="Module" runat="server" Text='<%#Eval("Module")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="DebugLevel">
                <ItemTemplate>
                    <asp:Label ID="DebugLevel" runat="server" Text='<%#Eval("DebugLevel")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="MessageLine">
                <ItemTemplate>
                    <asp:Label ID="MessageLine" runat="server" Text='<%#Eval("MessageLine")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Gelezen">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbHandled" runat="server" AutoPostBack="true" OnCheckedChanged="OnHandledChanged" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Recordnr" >
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="tbRecordnr" runat="server" Text='<%#Eval("Recordnr")%>'/>
                </ItemTemplate>
            </asp:TemplateField>        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <table style="width: 100%">
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->
                <asp:Button ID="Button2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>

