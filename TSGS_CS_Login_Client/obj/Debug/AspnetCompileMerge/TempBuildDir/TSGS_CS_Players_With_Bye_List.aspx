<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Players_With_Bye_List.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Players_With_Bye_List" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header boven topic content -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Competitie ...</asp:Label>
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Deelnemerlijst</asp:Label>

<!-- Topic content, gridview, etc. etc.-->
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Spelernaam">
                <ItemTemplate>
                    <%#Eval("SpelerNaam")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Datum">
                <ItemTemplate>
                    <%#Eval("Rdate")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Competitie" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%#Eval("Naam_Competitie")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ronde" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#Eval("RNR")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
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

<!-- Einde Topic content -->

<!-- Footer van contenct: links een voortgangsmelding, rechts de verwerkingsbuttons -->

    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">
<!-- einde content block

<!-- additional footer buttons, Button 1 text = algemeen: cancel, button 1 text is verwerk topic/update, upload, tc. etc. -->
                <asp:Button ID="Button1" runat="server" Text="Upload" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>            
                <asp:Button ID="Button2" runat="server" Text="Gereed" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>          
	</td>
        </tr>
    </table>
</asp:Content>

