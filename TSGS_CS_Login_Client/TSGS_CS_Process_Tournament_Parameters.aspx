<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Process_Tournament_Parameters.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Process_Tournament_Parameters" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border="1" >
        <tr>
            <td style="align-items: flex-start">
                <asp:Label ID="Label32" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:DropDownList ID="DDLBasedOn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLBasedOnChange" Width="750px"></asp:DropDownList>
                <asp:Label ID="Label33" runat="server" BackColor="Transparent" Visible="false"></asp:Label><br />
            </td>
        </tr>
    </table>
    <table border="1">
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox3" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox4" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox5" runat="server" Width="120"></asp:TextBox>
            </td>
        </tr>

        <tr>

            <td>
                <asp:Label ID="Label6" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox6" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox7" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label8" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox8" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Label9" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox9" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label10" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox10" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label11" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox11" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
        </tr>

        <tr>

            <td>
                <asp:Label ID="Label12" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox12" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>

            <td>
                <asp:Label ID="Label13" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox13" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="30"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label14" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox14" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="50"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Label15" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox15" runat="server" OnTextChanged="OnIntTextBoxChanged" Width="40"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label21" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox21" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label22" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox22" runat="server" Width="250"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label23" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox23" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label25" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox25" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label26" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox26" runat="server" Width="250"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="Label27" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox27" runat="server"  Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label28" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox28" runat="server" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="DDLCompetitionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_CT_SelectedChange" Height="17px" Width="201px"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label29" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox29" runat="server" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="DDLStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_SelectedChange" Width="201px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label34" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox34" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label35" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:TextBox ID="TextBox35" runat="server" Width="250"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label30" runat="server" Font-Size="14pt">Foto:</asp:Label><br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:FileUpload ID="FileUpload1" runat="server" Font-Size="12pt" Width="360px" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button3" runat="server" Text="Toon foto" Style="display: none" OnClick="Button3_Click"></asp:Button>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="Button3" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Image ID="Image2" TabIndex="9" runat="server" AlternateText="Afbeelding niet gevonden" ImageUrl="~/images/nofoto.png"></asp:Image>
                <asp:ImageButton ID="RotateButton" ImageUrl="~/images/sub_black_rotate_cw.png" Width="40" TabIndex="9" runat="server" Text="" OnClick="RotateButton_Click"></asp:ImageButton>
                <br />
                <br />
            </td>
            <td style="align-items: flex-start">
                <asp:Label ID="Label31" runat="server" BackColor="Transparent"></asp:Label><br />
                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnIntegrateChanged"  />
                &nbsp;<asp:TextBox ID="TextBox30" runat="server" Visible="false"></asp:TextBox>
                <br />
                <asp:DropDownList ID="DDLToernooien" runat="server" AutoPostBack="true" visible="false" OnSelectedIndexChanged="DDL_SelectToernooiChange" Width="200px"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label16" runat="server" BackColor="Transparent" Visible="false"></asp:Label><br />
                <asp:TextBox ID="TextBox16" runat="server" Width="250" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label17" runat="server" BackColor="Transparent" Visible="false"></asp:Label><br />
                <asp:TextBox ID="TextBox17" runat="server" Width="250" Visible="false"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label18" runat="server" BackColor="Transparent" Visible="false"></asp:Label><br />
                <asp:TextBox ID="TextBox18" runat="server" Width="250" Visible="false"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label19" runat="server" BackColor="Transparent" Visible="false"></asp:Label><br />
                <asp:TextBox ID="TextBox19" runat="server" Width="250" Visible="false"></asp:TextBox>
            </td>
        </tr>
    </table>

    <asp:Button ID="Button2" runat="server" Text="" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
    <table style="width: 100%">
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" BackColor="Transparent" Font-Names="Arial" Font-Size="14pt" Height="32px"></asp:Label>
            </td>
            <td class="auto-style2">&nbsp;                     
            </td>
            <td style="text-align: right">
                <!-- additional footer buttons -->
                <asp:Button ID="Button1" runat="server" Text="Cancel" Font-Size="12pt" OnClick="Button1_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=Button3.ClientID %>").click();
            }
        }
    </script>
</asp:Content>
