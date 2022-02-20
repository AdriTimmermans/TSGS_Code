<%@ Page Title="" Language="C#" MasterPageFile="~/TSGS_CS_Master.Master" AutoEventWireup="true" CodeBehind="TSGS_CS_Competition_System_System_Monitor.aspx.cs" Inherits="TSGS_CS_Login_Client.TSGS_CS_Competition_System_System_Monitor" %>

<%@ MasterType VirtualPath="~/TSGS_CS_Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%">
        <tr >
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label202" runat="server" Font-Names="Arial" Text="Toegang" />
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label102" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button2" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button2_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label118" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button18" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button18_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label142" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button42" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button42_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label105" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button5" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button5_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">&nbsp;</td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label203" runat="server" Font-Names="Arial" Text="Competitie Systeem" />
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label109" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button9" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button9_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label108" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button8" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button8_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label110" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button10" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button10_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label112" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button12" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button12_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label113" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button13" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button13_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label119" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button19" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button19_Click"></asp:Button>
            </td>

        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label137" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button37" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button37_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label111" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button11" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button11_Click"></asp:Button>
            </td>


            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label116" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button16" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button16_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label115" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button15" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button15_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label114" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button14" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button14_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label117" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button17" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button17_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label122" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button22" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button22_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label144" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button44" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button44_Click"></asp:Button>
            </td>
            <td  style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label145" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button45" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button45_Click"></asp:Button>
            </td>

        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label135" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button35" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button35_Click" style="margin-bottom: 0px"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label103" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button3" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button3_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label138" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button38" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button38_Click" Enabled="False"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label204" runat="server" Font-Names="Arial" Text="Beheer" />
            </td>

        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label106" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button6" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button6_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label107" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button7" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button7_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label120" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button20" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button20_Click"></asp:Button>
            </td>
        </tr>

        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">

                <asp:Label ID="Label121" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button21" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button21_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label123" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button23" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button23_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label133" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button33" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button33_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label136" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button36" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button36_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label143" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button43" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button43_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label134" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button34" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button34_Click" Height="27px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label130" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button30" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button30_Click"></asp:Button>
            </td>
            
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label139" runat="server" Visible="false" Text="label" />
                <asp:Button ID="Button39" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button39_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label205" runat="server" Font-Names="Arial" Text="Systeem Beheer" />
            </td>

        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label124" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button24" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button24_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label140" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button40" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button40_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                 &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label206" runat="server" Font-Names="Arial" Text="Snelschaak- rapid competitie" />
            </td>

        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label125" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button25" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button25_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                                <asp:Label ID="Label126" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button26" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button26_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                                <asp:Label ID="Label127" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button27" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button27_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                                <asp:Label ID="Label129" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button29" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button29_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">

                <asp:Label ID="Label128" runat="server" Font-Names="Arial" Visible="false" Text="label" />

                <asp:Button ID="Button28" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button28_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
            </td>
        </tr>

        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label207" runat="server"  BackColor="" Font-Names="Arial" Text="Zwitsers Systeem" />
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label51" runat="server" Font-Names="Arial" Visible="false" Text="Inschrijvingen" />
                <asp:Button ID="Button51" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button51_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label52" runat="server" Font-Names="Arial" Visible="false" Text="Verbinden aan toernooi" />
                <asp:Button ID="Button52" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button52_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label53" runat="server" Font-Names="Arial" Visible="false" Text="Aanmelden" />
                <asp:Button ID="Button53" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button53_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label54" runat="server" Font-Names="Arial" Visible="false" Text="Byes" />
                <asp:Button ID="Button54" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button54_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label55" runat="server" Font-Names="Arial" Visible="false" Text="Indelen" />
                <asp:Button ID="Button55" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button55_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label56" runat="server" Font-Names="Arial" Visible="false" Text="Toernooiparameters" />
                <asp:Button ID="Button56" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button56_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label57" runat="server" Font-Names="Arial" Visible="false" Text="Wachtscherm uitslagen" />
                <asp:Button ID="Button57" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button57_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label58" runat="server" Font-Names="Arial" Visible="false" Text="Inlezen uitslagen per invoerpunt" />
                <asp:Button ID="Button58" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button58_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label59" runat="server" Font-Names="Arial" Visible="false" Text="Ronde Indelen" />
                <asp:Button ID="Button59" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button59_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label60" runat="server" Font-Names="Arial" Visible="false" Text="Corrigeren speler gegevens" />
                <asp:Button ID="Button60" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button60_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label61" runat="server" Font-Names="Arial" Visible="false" Text="Corrigeren uitslagen" />
                <asp:Button ID="Button61" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button61_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label62" runat="server" Font-Names="Arial" Visible="false" Text="Corrigeren toernooi parameters" />
                <asp:Button ID="Button62" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button62_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label63" runat="server" Font-Names="Arial" Visible="false" Text="Kroeglopers module" />
                <asp:Button ID="Button63" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button63_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label64" runat="server" Font-Names="Arial" Visible="false" Text="Display deelnemers" />
                <asp:Button ID="Button64" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button64_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label65" runat="server" Font-Names="Arial" Visible="false" Text="Display Indeling" />
                <asp:Button ID="Button65" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button65_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label66" runat="server" Font-Names="Arial" Visible="false" Text="Ranglijst stand" />
                <asp:Button ID="Button66" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button66_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label67" runat="server" Font-Names="Arial" Visible="false" Text="Ranglijst winst-verlies" />
                <asp:Button ID="Button67" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button67_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label68" runat="server" Font-Names="Arial" Visible="false" Text="Persoonlijke scores" />
                <asp:Button ID="Button68" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button68_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label69" runat="server" Font-Names="Arial" Visible="false" Text="Prijsbrieven" />
                <asp:Button ID="Button69" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button69_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label70" runat="server" Font-Names="Arial" Visible="false" Text="Handmatige indeling" />
                <asp:Button ID="Button70" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button70_Click"></asp:Button>       
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label71" runat="server" Font-Names="Arial" Visible="false" Text="Norm bepaling" />
                <asp:Button ID="Button71" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button71_Click"></asp:Button>       
            </td>
        </tr>
                <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label72" runat="server" Font-Names="Arial" Visible="false" Text="FIDE rapport" />
                <asp:Button ID="Button72" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button72_Click"></asp:Button>
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                &nbsp;
            </td>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" bgcolor ="#EFF3FB">
                <asp:Label ID="Label208" runat="server"  BackColor="" Font-Names="Arial" Text="Diverse Tools" />
            </td>
        </tr>
        <tr>
            <td style="border-style:solid; border-width:1px; text-align: center; width:33%;">
                <asp:Label ID="Label131" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button31" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button31_Click"></asp:Button>
            </td>
            <td>
                &nbsp;
<%--                <asp:Label ID="Label132" runat="server" Font-Names="Arial" Visible="false" Text="label" />
                <asp:Button ID="Button32" Width="100%" runat="server" Text="" Font-Size="12pt" OnClick="Button32_Click"></asp:Button>--%>
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>