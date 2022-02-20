<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Display_MobileMessages.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Display_MobileMessages" %>

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
                    <asp:Label ID="RDate" runat="server" Width="120px" Text='<%#Eval("Date_Entry", "{0:dd-MM HH:mm:ss}")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Competitie ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="CID" runat="server" Text='<%#Eval("Competition_id")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remark">
                <ItemTemplate>
                    <asp:Label ID="Remark" runat="server" Width="450px" Text='<%#Eval("Remark")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sender">
                <ItemTemplate>
                    <asp:Label ID="Sender" runat="server" Text='<%#Eval("Sender")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Handled">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbHandled" runat="server" AutoPostBack="true" OnCheckedChanged="OnHandledChanged" Text=" " />
                </ItemTemplate>
            </asp:TemplateField>    
            <asp:TemplateField HeaderText="Recordnr" Visible="false">
                <EditItemTemplate></EditItemTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="tbRecordnr" runat="server" Text='<%#Eval("record_id")%>'/>
                </ItemTemplate>
            </asp:TemplateField>        
       </Columns>
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

