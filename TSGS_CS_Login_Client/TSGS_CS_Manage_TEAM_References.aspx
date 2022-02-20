<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Manage_TEAM_References.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Manage_TEAM_References" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header boven topic content -->
    <asp:Label ID="Label3" runat="server" Font-Names="Arial">Competitie ...</asp:Label>&nbsp;
    <asp:Label ID="Label4" runat="server" Font-Names="Arial">Deelnemerlijst</asp:Label>

    <!-- Topic content, gridview, etc. etc.-->
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True" OnRowCommand="GridView1_RowCommand">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField ControlStyle-Width="50">
                <ItemTemplate>
                    <asp:LinkButton ID="lbEdit" runat="server" CommandName="EditRow" CommandArgument='<%#Eval("Team_Record_Nummer")%>'><img src="images/edit.png" width="20"></asp:LinkButton>
                    <asp:LinkButton ID="lbDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%#Eval("Team_Record_Nummer")%>'><img src="images/delete.png" width="20"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="lbUpdate" runat="server" CommandName="UpdateRow" CommandArgument='<%#Eval("Team_Record_Nummer")%>'><img src="images/update.png" width="20"></asp:LinkButton>
                    <asp:LinkButton ID="lbCancel" runat="server" CommandName="CancelUpdate" CommandArgument='<%#Eval("Team_Record_Nummer")%>'><img src="images/Delete-icon.png" width="20"></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Vereniging">
                <ItemTemplate>
                    <asp:Label ID="VerenigingNaam" runat="server" Text='<%#Eval("Vereniging_Naam")%>' BackColor="Transparent"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="VerenigingNaam" runat="server" Text='<%#Eval("Vereniging_Naam")%>' BackColor="Transparent"></asp:Label>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbInsert" runat="server" CommandName="InsertRow"><img src="images/folder-full-add-icon.png" width="20"></asp:LinkButton>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Vereniging_Id">
                <ItemTemplate>
                    <%#Eval("Vereniging_Id")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="Vereniging_Id" runat="server" Text='<%#Eval("Vereniging_Id")%>' BackColor="Transparent"></asp:Label>
                </EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Teamnaam">
                <ItemTemplate>
                    <%#Eval("Team_Naam")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox11" Width="100" runat="server" Text='<%#Eval("Team_Naam")%>'></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="TextBox13" Width="100" runat="server" Text=""></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Team_Nr">
                <ItemTemplate>
                    <%#Eval("Team_Nr")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <div style="float: right">
                        <asp:TextBox ID="TextBox15" Width="25" runat="server" Text='<%#Eval("Team_Nr")%>'></asp:TextBox>
                    </div>
                </EditItemTemplate>
                <FooterTemplate>
                    <div style="float: right">
                    <asp:TextBox ID="TextBox16" onkeydown = "return (event.keyCode!=13);" Width="25" runat="server" Text=""></asp:TextBox>
                    </div>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="URL">
                <ItemTemplate>
                    <%#Eval("URL")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox12" Width="250" runat="server" Text='<%#Eval("URL")%>'></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="TextBox14" Width="250" runat="server" Text=""></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="URL" Visible=" false">
                <ItemTemplate>
                    <%#Eval("URL")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="RecNr" runat="server" Text='<%#Eval("Team_Record_Nummer")%>' BackColor="Transparent"></asp:Label>
                </EditItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="false" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>

    <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound" Visible=" false">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Foto">
                <ItemTemplate>
                    <asp:Image ID="imgPicture" runat="server"></asp:Image>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Naam">
                <ItemTemplate>
                    <asp:Label ID="PL" runat="server" Text='<%#Eval("SpelerNaam")%>' BackColor="Transparent"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Team_nr">
                <ItemTemplate>
                    <%#Eval("Team_nr")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="KNSBnummer">
                <ItemTemplate>
                    <%#Eval("KNSBnummer")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="KNSBrating">
                <ItemTemplate>
                    <%#Eval("KNSBRating", "{0:#000.0}")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FIDEnummer">
                <ItemTemplate>
                    <%#Eval("FIDEnummer")%>
                </ItemTemplate>
                <EditItemTemplate></EditItemTemplate>
                <FooterTemplate></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FIDErating">
                <ItemTemplate>
                    <%#Eval("FIDERating", "{0:#000.0}")%>
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
<!-- additional footer buttons, Button 1 text = algemeen: cancel, button 1 text is verwerk topic/update, upload, tc. etc. -->

    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">&nbsp;</td>
            </tr>
        <tr>
            <td class="auto-style1">
                <asp:Button ID="Button3" runat="server" Text="Upload" Font-Size="12pt" OnClick="Button3_Click" Visible="false"></asp:Button>
                <asp:Button ID="Button2" runat="server" Text="Create Overview" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
            <td class="auto-style2">&nbsp;</td>
            <td style="text-align: right">                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
</td>
            </tr>
            </table>
</asp:Content>
