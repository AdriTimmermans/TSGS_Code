<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Blitz_Capture_Results.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Blitz_Capure_Results" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td style="vertical-align: top">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id" Visible="false">
                            <ItemTemplate>
                                <%#Eval("Speler_ID")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Spelernaam">
                            <ItemTemplate>
                                <%#Eval("SpelerNaam")%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Groepnr">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="StringGroepnr" runat="server" width="40" AutoPostBack="false"   onTextChanged="OnGroepnrInputChanged" Text='<%#Eval("StringGroepnr")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MatchPunten">
                            <EditItemTemplate></EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="StringMatchPunten" width="40" runat="server" AutoPostBack="false"   onTextChanged="OnMatchpuntenInputChanged" Text='<%#Eval("StringMatchpunten")%>' />
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

            </td>
        </tr>
        <tr>
            <td>            
                <asp:Label ID="Label30" runat="server" >Controle getal: </asp:Label>
                <asp:TextBox ID="TextBox11" runat="server" Width="40px"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" >&nbsp;punten</asp:Label>
            </td>
        </tr>
    </table>


    <!-- layout and controls for function <xxxx>, at least 1 button is expected (but not compulsary). This is button 2 -->

    <asp:Button ID="Button2" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>


    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px">Feedback action</asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">

                <!-- additional footer buttons -->

                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <script>
        function ClearMatchpunten() {
            document.getElementById("StringGroepnr").value = "";
        }
        function ClearGroepnr() {
            document.getElementById("StringMatchPunten").value = "";
        }    </script>
</asp:Content>
