document.write("\
<h1 align=left>Schaakclub Dordrecht, Herfstcompetitie 2019</h1>\
<img src='data:image/png;base64,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'><br />\
<h2 align=left>Krul, R.  (Rene)                        </h2><br /><br />\
ELO rating: 2208,3, Competitie punten: 2118,5, KNSB Rating: 2183,0, FIDE Rating: 2199,0<br />\
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
<td style='width: 15%' >16-09-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >0,0</td>\
</td></tr>\
<td style='width:  5%' >2</td>\
<td style='width: 15%' >18-09-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >0,0</td>\
</td></tr>\
<td style='width:  5%' >3</td>\
<td style='width: 15%' >25-09-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >4</td>\
<td style='width: 15%' >02-10-2019</td>\
<td style='width: 30%' >Wilde, R. de (Rik) </td>\
<td style='width: 10%' > 0.5 - 0.5</td>\
<td style='width: 10%' >Wit</td>\
<td style='width: 10%' >-1,9</td>\
<td style='width: 10%' >-5,5</td>\
</td></tr>\
<td style='width:  5%' >5</td>\
<td style='width: 15%' >11-10-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >6</td>\
<td style='width: 15%' >16-10-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >7</td>\
<td style='width: 15%' >04-11-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >8</td>\
<td style='width: 15%' >06-11-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >9</td>\
<td style='width: 15%' >13-11-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<td style='width:  5%' >10</td>\
<td style='width: 15%' >20-11-2019</td>\
<td style='width: 30%' >Timmermans, M.A.  (Mark) </td>\
<td style='width: 10%' >   0 - 1</td>\
<td style='width: 10%' >Zwart</td>\
<td style='width: 10%' >-3,8</td>\
<td style='width: 10%' >-20,0</td>\
</td></tr>\
<td style='width:  5%' >11</td>\
<td style='width: 15%' >27-11-2019</td>\
<td style='width: 30%' >Afwezig, - - (-) </td>\
<td style='width: 10%' >Afwezig</td>\
<td style='width: 10%' >N.v.t.</td>\
<td style='width: 10%' >0,0</td>\
<td style='width: 10%' >-10,0</td>\
</td></tr>\
<tr><td height='8'>&nbsp;<br></td></tr>\
</table>\
<br />\
<br />\
<h2 align=left>PersonalScores-22</h2><br /><br />\
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
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2793'>Pluymert, P.W.  (Piet) </a></td>\
<td style='width: 10%' >2,14</td>\
<td style='width: 10%' >10</td>\
<td style='width: 10%' >14</td>\
</tr>\
<tr>\
<td style='width:  5%' >2</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2804'>Timmermans, M.A.  (Mark) </a></td>\
<td style='width: 10%' >1,62</td>\
<td style='width: 10%' >14</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >3</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2791'>Oliemans, C.  (Cor) </a></td>\
<td style='width: 10%' >1,15</td>\
<td style='width: 10%' >10</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >4</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2824'>Jong, J. de (Joop) </a></td>\
<td style='width: 10%' >1,00</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >3</td>\
</tr>\
<tr>\
<td style='width:  5%' >5</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2834'>Oevelen, P. van (Peter) </a></td>\
<td style='width: 10%' >1,00</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >3</td>\
</tr>\
<tr>\
<td style='width:  5%' >6</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2811'>Wilde, R. de (Rik) </a></td>\
<td style='width: 10%' >0,92</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >13</td>\
</tr>\
<tr>\
<td style='width:  5%' >7</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2750'>Boer, L. den (Lennard) </a></td>\
<td style='width: 10%' >0,92</td>\
<td style='width: 10%' >8</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >8</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2802'>Timmermans, A.L.  (Adri) </a></td>\
<td style='width: 10%' >0,81</td>\
<td style='width: 10%' >7</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >9</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2745'>Abee, K.  (Koos) </a></td>\
<td style='width: 10%' >0,75</td>\
<td style='width: 10%' >6</td>\
<td style='width: 10%' >24</td>\
</tr>\
<tr>\
<td style='width:  5%' >10</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2760'>Griend, J. van de (Johan) </a></td>\
<td style='width: 10%' >0,46</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >11</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2777'>Koppelaar, V.  (Victor) </a></td>\
<td style='width: 10%' >0,46</td>\
<td style='width: 10%' >4</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >12</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2767'>Hennekes, J.  (Jacques) </a></td>\
<td style='width: 10%' >0,38</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >8</td>\
</tr>\
<tr>\
<td style='width:  5%' >13</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2746'>Apon, H.  (Iwahn) </a></td>\
<td style='width: 10%' >0,38</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >8</td>\
</tr>\
<tr>\
<td style='width:  5%' >14</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2749'>Blommestein, V. van (Victor) </a></td>\
<td style='width: 10%' >0,35</td>\
<td style='width: 10%' >3</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >15</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2806'>Versloot, J.W.  (Jan Willem) </a></td>\
<td style='width: 10%' >0,35</td>\
<td style='width: 10%' >3</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >16</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2800'>Slagboom, T.  (Ton) </a></td>\
<td style='width: 10%' >0,12</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >26</td>\
</tr>\
<tr>\
<td style='width:  5%' >17</td>\
<td style='width: 30%; align:left'><a href='https://tsgsadministration.nl/TSGS_CS_Show_One_Game_History.aspx?Player1=2778&Player2=2799'>Sitton, B.  (Ben) </a></td>\
<td style='width: 10%' >0,11</td>\
<td style='width: 10%' >1</td>\
<td style='width: 10%' >27</td>\
</tr>\
</table>\
<br />\
<br />\
");
