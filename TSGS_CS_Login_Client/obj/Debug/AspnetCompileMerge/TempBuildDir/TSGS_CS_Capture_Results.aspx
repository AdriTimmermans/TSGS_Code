<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Capture_Results.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Capture_Results" %>
<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Speler Id Wit" Visible="false">
                            <ItemTemplate>
                            <asp:TextBox ID="PID_Wit" runat="server" text='<%#Eval("PID_Wit")%>' BackColor="Transparent"></asp:TextBox>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Speler Id Zwart" Visible="false">
                            <ItemTemplate>
                            <asp:TextBox ID="PID_Zwart" runat="server" text='<%#Eval("PID_Zwart")%>' BackColor="Transparent"></asp:TextBox>
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
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Zwart">
                            <ItemTemplate>
                            <asp:label ID="SpelerNaamZwart" runat="server" text='<%#Eval("SpelerNaamZwart")%>' BackColor="Transparent"></asp:label>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Result">
                            <ItemTemplate>
                            <asp:DropDownList ID="Result" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_SelectedChange"></asp:DropDownList>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Wedstrijdtype" Visible="false">
                            <ItemTemplate>
                            <asp:TextBox ID="Wedstrijdtype" runat="server" text='<%#Eval("Wedstrijdtype")%>' BackColor="Transparent"></asp:TextBox>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="Kroongroep" Visible="false">
                            <ItemTemplate>
                            <asp:TextBox ID="Kroongroep" runat="server" text='<%#Eval("NumberChampionsgroupGame")%>' BackColor="Transparent"></asp:TextBox>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="ResultValue" Visible="false">
                            <ItemTemplate>
                            <asp:TextBox ID="ResultValue" runat="server" text='<%#Eval("Resultaat_Wedstrijd")%>' BackColor="Transparent"></asp:TextBox>
                            </ItemTemplate> 
                            <EditItemTemplate></EditItemTemplate> 
                            <FooterTemplate></FooterTemplate>
                        </asp:TemplateField>                          </Columns>
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
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">&nbsp;&nbsp;
                    </td>
                    <td class="auto-style2">&nbsp;</td>
                    <td style="text-align:right">                        
                        <asp:Button ID="Button1" text="done" runat="server" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
</asp:Content>
