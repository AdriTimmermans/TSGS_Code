document.write("\
<h1 align=left>Schaakclub Dordrecht, Najaarscompetitie 2018-2019</h1>\
<img src='data:image/png;base64,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'><br />\
<h2 align=left>Barends, H.  (Henk)                     </h2><br /><br />\
ELO rating: 1399,8, Competitie punten: 1458,1, KNSB Rating: 1370,0, FIDE Rating: 0,0<br />\
<table border='0'>\
<tr><td height='8'>&nbsp;<br></td></tr>\
<td style='width:  5%' >Ronde</td>\
<td style='width: 15%' >Datum</td>\
<td style='width: 30%' >Tegenstander</td>\
<td style='width: 10%' >Uitslag</td>\
<td style='width: 10%' >Kleur</td>\
<td style='width: 10%' >ELO winst/verlies</td>\
<td style='width: 10%' >CP winst/verlies</td>\
</tr>\
<tr><td height='8'></td></tr>\
<td style='width:  5%' >1</td>\
<td style='width: 15%' >12-09-2018</td>\
<td style='width: 30%' >Nugteren, D.  (Dick) </td>\
<td style='width: 10%' >   1 - 0</td>\
<td style='width: 10%' >Wit</td>\
<td style='width: 10%' >15,2</td>\
<td style='width: 10%' >30,5</td>\
</td></tr>\
<td style='width:  5%' >2</td>\
<td style='width: 15%' >19-09-2018</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >0,0</td>\
</td></tr>\
<td style='width:  5%' >3</td>\
<td style='width: 15%' >27-09-2018</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >0,0</td>\
</td></tr>\
<td style='width:  5%' >4</td>\
<td style='width: 15%' >03-10-2018</td>\
<td style='width: 30%' >Extern, - - (-) </td>\
<td style='width: 10%' >E0.5 - 0.5</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >10,0</td>\
</td></tr>\
<td style='width:  5%' >5</td>\
<td style='width: 15%' >10-10-2018</td>\
<td style='width: 30%' >Boersma, J. A.  (Hans) </td>\
<td style='width: 10%' >   1 - 0</td>\
<td style='width: 10%' >Wit</td>\
<td style='width: 10%' >18,5</td>\
<td style='width: 10%' >36,9</td>\
</td></tr>\
<td style='width:  5%' >6</td>\
<td style='width: 15%' >17-10-2018</td>\
<td style='width: 30%' >Monster, H.  (Henk) </td>\
<td style='width: 10%' >   0 - 1</td>\
<td style='width: 10%' >Zwart</td>\
<td style='width: 10%' >-14,3</td>\
<td style='width: 10%' >-28,7</td>\
</td></tr>\
<td style='width:  5%' >7</td>\
<td style='width: 15%' >31-10-2018</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >0,0</td>\
</td></tr>\
<td style='width:  5%' >8</td>\
<td style='width: 15%' >07-11-2018</td>\
<td style='width: 30%' >Extern, - - (-) </td>\
<td style='width: 10%' >E  1 - 0</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >10,0</td>\
</td></tr>\
<td style='width:  5%' >9</td>\
<td style='width: 15%' >14-11-2018</td>\
<td style='width: 30%' >Maertelaere, A. de (Anton) </td>\
<td style='width: 10%' >   0 - 1</td>\
<td style='width: 10%' >Wit</td>\
<td style='width: 10%' >-10,0</td>\
<td style='width: 10%' >-20,0</td>\
</td></tr>\
<td style='width:  5%' >10</td>\
<td style='width: 15%' >21-11-2018</td>\
<td style='width: 30%' >Poort, L  (Lennahrt) </td>\
<td style='width: 10%' >   1 - 0</td>\
<td style='width: 10%' >Zwart</td>\
<td style='width: 10%' >4,0</td>\
<td style='width: 10%' >20,0</td>\
</td></tr>\
<td style='width:  5%' >11</td>\
<td style='width: 15%' >29-11-2018</td>\
<td style='width: 30%' >Manen, B. van (Bert) </td>\
<td style='width: 10%' > 0.5 - 0.5</td>\
<td style='width: 10%' >Zwart</td>\
<td style='width: 10%' >5,7</td>\
<td style='width: 10%' >10,0</td>\
</td></tr>\
<td style='width:  5%' >12</td>\
<td style='width: 15%' >27-12-2018</td>\
<td style='width: 30%' >Vliet, W. Van (Wim) </td>\
<td style='width: 10%' >   1 - 0</td>\
<td style='width: 10%' >Zwart</td>\
<td style='width: 10%' >13,5</td>\
<td style='width: 10%' >27,1</td>\
</td></tr>\
<td style='width:  5%' >13</td>\
<td style='width: 15%' >14-01-2019</td>\
<td style='width: 30%' >Extern, - - (-) </td>\
<td style='width: 10%' >E  0 - 1</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >10,0</td>\
</td></tr>\
<td style='width:  5%' >14</td>\
<td style='width: 15%' >17-01-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >15</td>\
<td style='width: 15%' >30-01-2019</td>\
<td style='width: 30%' >Extern, - - (-) </td>\
<td style='width: 10%' >Extern</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >10,0</td>\
</td></tr>\
<td style='width:  5%' >16</td>\
<td style='width: 15%' >06-02-2019</td>\
<td style='width: 30%' >Heeren, A. van (Arie) </td>\
<td style='width: 10%' >   0 - 1</td>\
<td style='width: 10%' >Wit</td>\
<td style='width: 10%' >-5,2</td>\
<td style='width: 10%' >-20,0</td>\
</td></tr>\
<tr><td height='8'>&nbsp;<br></td></tr>\
</table>\
<br />\
<br />\
<h2 align=left>Frequency tabel</h2><br /><br />\
<table border='0'>\
<tr>\
<td style='width:  5%' >Nr</td>\
<td style='width: 30%' >Naam</td>\
<td style='width: 10%' >Gem. per jaar</td>\
<td style='width: 10%' >Aantal partijen</td>\
<td style='width: 10%' >Aantal competities</td>\
</tr>\
<tr><td height='8'></td></tr>\
<tr>\
<td style='width:  5%' >1</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2786'>Mulder, R  (Rien) </a></td>\
<td style='width: 10%' >1,88</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >8</td>\
</tr>\
<tr>\
<td style='width:  5%' >2</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2759'>Fafie, P.  (Peter) </a></td>\
<td style='width: 10%' >1,57</td>\
<td style='width: 10%' >12</td>\
<td style='width: 10%' >23</td>\
</tr>\
<tr>\
<td style='width:  5%' >3</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2846'>Vries, E de (Edward) </a></td>\
<td style='width: 10%' >1,50</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >2</td>\
</tr>\
<tr>\
<td style='width:  5%' >4</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2785'>Monster, H.  (Henk) </a></td>\
<td style='width: 10%' >1,45</td>\
<td style='width: 10%' >14</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >5</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2751'>Boersma, J. A.  (Hans) </a></td>\
<td style='width: 10%' >1,45</td>\
<td style='width: 10%' >14</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >6</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2765'>Hauser, J.  (Johann) </a></td>\
<td style='width: 10%' >1,41</td>\
<td style='width: 10%' >8</td>\
<td style='width: 10%' >17</td>\
</tr>\
<tr>\
<td style='width:  5%' >7</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2833'>Waziri, A.  (Asheq) </a></td>\
<td style='width: 10%' >1,20</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >5</td>\
</tr>\
<tr>\
<td style='width:  5%' >8</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2828'>Herk, R. van (Robert) </a></td>\
<td style='width: 10%' >1,00</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >6</td>\
</tr>\
<tr>\
<td style='width:  5%' >9</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2831'>Dees, A.  (Bram) </a></td>\
<td style='width: 10%' >1,00</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >6</td>\
</tr>\
<tr>\
<td style='width:  5%' >10</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2843'>Vliet, W. Van (Wim) </a></td>\
<td style='width: 10%' >1,00</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >3</td>\
</tr>\
<tr>\
<td style='width:  5%' >11</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2773'>Jelier, R.  (Rick) </a></td>\
<td style='width: 10%' >0,88</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >17</td>\
</tr>\
<tr>\
<td style='width:  5%' >12</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2789'>Nugteren, D.  (Dick) </a></td>\
<td style='width: 10%' >0,88</td>\
<td style='width: 10%' >7</td>\
<td style='width: 10%' >24</td>\
</tr>\
<tr>\
<td style='width:  5%' >13</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2764'>Harms, A.  (Aart) </a></td>\
<td style='width: 10%' >0,79</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >19</td>\
</tr>\
<tr>\
<td style='width:  5%' >14</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2792'>Peters, E  (Elroy) </a></td>\
<td style='width: 10%' >0,75</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >16</td>\
</tr>\
<tr>\
<td style='width:  5%' >15</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2835'>Dijk, J. van (Johan) </a></td>\
<td style='width: 10%' >0,75</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >4</td>\
</tr>\
<tr>\
<td style='width:  5%' >16</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2807'>Visser, T  (Theo) </a></td>\
<td style='width: 10%' >0,63</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >24</td>\
</tr>\
<tr>\
<td style='width:  5%' >17</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2796'>Reijenga, J.  (Jan) </a></td>\
<td style='width: 10%' >0,52</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >18</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2788'>Nugteren (+), B.  (Bas) </a></td>\
<td style='width: 10%' >0,52</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >19</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2761'>Groot, B.D.  (Dick) </a></td>\
<td style='width: 10%' >0,52</td>\
<td style='width: 10%' >5</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >20</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2724'>Schot, P  (Peter) </a></td>\
<td style='width: 10%' >0,46</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >13</td>\
</tr>\
<tr>\
<td style='width:  5%' >21</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2769'>Hobe, T.  (Ton) </a></td>\
<td style='width: 10%' >0,41</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >22</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2797'>Rijneveen, M.  (Martijn) </a></td>\
<td style='width: 10%' >0,41</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >23</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2813'>Wingerden, E. van (Erwin) </a></td>\
<td style='width: 10%' >0,35</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >17</td>\
</tr>\
<tr>\
<td style='width:  5%' >24</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2783'>Maertelaere, A. de (Anton) </a></td>\
<td style='width: 10%' >0,31</td>\
<td style='width: 10%' >3</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >25</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2784'>Manen, B. van (Bert) </a></td>\
<td style='width: 10%' >0,21</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >26</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2753'>Bonninga, A.  (Anko) </a></td>\
<td style='width: 10%' >0,21</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >27</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2809'>Vossen, C.  (Cor) </a></td>\
<td style='width: 10%' >0,21</td>\
<td style='width: 10%' >2</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >28</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2810'>Weert, G. van der (Gerben) </a></td>\
<td style='width: 10%' >0,10</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >29</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2763'>Haren, O.C. van (Otto) </a></td>\
<td style='width: 10%' >0,10</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >30</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2770'>Hoek, T. van (Thijs) </a></td>\
<td style='width: 10%' >0,10</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >29</td>\
</tr>\
<tr>\
<td style='width:  5%' >31</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2748&Player2=2798'>Schreuder, P.J.  (Hans) </a></td>\
<td style='width: 10%' >0,10</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >29</td>\
</tr>\
</table>\
<br />\
<br />\
");
