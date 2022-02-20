<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Adjust_Pairing.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Adjust_Pairing" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <asp:Label ID="Label3" runat="server" Font-Names="Arial" >Selecteer open te breken partij</asp:Label>

            <table>
                <tr>
                    <td style="width:40%" valign="top">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                            <asp:label ID="PID_Wit" runat="server" text='<%#Eval("PID_Wit")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Speler Id Zwart" Visible="false">
                            <ItemTemplate>
                            <asp:label ID="PID_Zwart" runat="server" text='<%#Eval("PID_Zwart")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wit">
                            <ItemTemplate>
                            <asp:label ID="SpelerNaamWit" runat="server" text='<%#Eval("SpelerNaamWit")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="tegen"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                            -
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Zwart">
                            <ItemTemplate>
                            <asp:label ID="SpelerNaamZwart" runat="server" text='<%#Eval("SpelerNaamZwart")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/notdone2.jpg" >
                        <ControlStyle Height="20px" />
                        </asp:CommandField>
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
                    <td>
                    &nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="width:25%" valign="top">
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView2_PageIndexChanging"  OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                            <asp:label ID="ID" runat="server" text='<%#Eval("ID")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wit">
                            <ItemTemplate>
                            <asp:label ID="Name" runat="server" text='<%#Eval("Name")%>' BackColor="Transparent"></asp:label>                  
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/done2.jpg" >
                        <ControlStyle Height="20px" />
                        </asp:CommandField>
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
                    <td>
                    &nbsp;&nbsp;&nbsp;
                    </td>

                    <td style="width:25%" valign="top">
                <asp:GridView ID="GridView3" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnPageIndexChanging="GridView3_PageIndexChanging"  OnSelectedIndexChanged="GridView3_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                            <asp:label ID="ID" runat="server" text='<%#Eval("ID")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zwart">
                            <ItemTemplate>
                            <asp:label ID="Name" runat="server" text='<%#Eval("Name")%>' BackColor="Transparent"></asp:label> 
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/done2.jpg" >
                        <ControlStyle Height="20px" />
                        </asp:CommandField>
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
             </table>
            <asp:Label ID="Label4" runat="server" Font-Names="Arial" >Handindeling</asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Font-Names="Arial" >-</asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;-&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Font-Names="Arial" >-</asp:Label>
                    </td>
                    <td>
                         <asp:Label ID="Label7" runat="server" Font-Names="Arial" Visibliy ="false"></asp:Label>
                         <asp:TextBox ID="InputKroongroep" runat="server" autocomplete="off" Text="0" Width="30px" />

                    </td>
                    <td style="text-align:right">
                        &nbsp;&nbsp;<asp:button id="Button2" runat="server" Text="Reset game" OnClick="Button2_Click"></asp:button>  
                        <asp:button id="Button1" runat="server" Text="Game OK"  OnClick="Button1_Click"></asp:button>  
                        <asp:button id="Button3" runat="server" Text="Gereed"  OnClick="Button3_Click"></asp:button>  
                    </td>
                </tr>
            </table>
</asp:Content>