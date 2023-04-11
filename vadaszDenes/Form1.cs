using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace vadaszDenes
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Initializeing the Form1.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //loads the cards data into a global variable.
            Globals.data = JObject.Parse(File.ReadAllText("carddata.json"));

            //sets the background image of the form and the panel.
            Bitmap newImage = new Bitmap(1236, 1065);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(Image.FromFile("Resources/bgimg.png"), 0, 0, 1236, 1065);
            }
            this.BackgroundImage = newImage;
            panel1.BackgroundImage = newImage;

            //request a new card to be the first.
            newCard();
        }

        /// <summary>
        /// Get a random new card that can be putted down if there's place for it.
        /// </summary>
        private void newCard()
        {
            //display the card on the bottom left corner.
            int number = Globals.randomNumber.Next(0, Globals.everyCard.Length);
            using (var image = Image.FromFile(Globals.everyCard[number].FullName))
            {
                currentCardPicture.Image = new Bitmap(image);
            }
            Globals.currentCardName = Globals.everyCard[number].FullName;

            //store the card for later use.
            string cardName = Globals.everyCard[number].Name.Substring(0, Globals.everyCard[number].Name.Length - 4);
            var cardData = Globals.data[cardName].Values().ToList();
            Globals.currentCard = new card(Convert.ToChar(cardData[0]),
                Convert.ToChar(cardData[1]),
                Convert.ToChar(cardData[2]),
                Convert.ToChar(cardData[3]),
                Convert.ToChar(cardData[4]));

            //A garbage collector is used to save memory usage.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Ask's the player if he/she truly wants to close the game. If yes then it closes it.
        /// </summary>
        private void quitButton_Click(object sender, EventArgs e)
        {
            var wantToExit = MessageBox.Show("Biztosan ki szeretne lépni?", "Kilépés", MessageBoxButtons.YesNo);
            switch (wantToExit)
            {
                case DialogResult.Yes:
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// when the map is full this asks the player if the player wants to save the game.
        /// </summary>
        /// <param name="point">The number of points the player achieved.</param>
        public void endGame(int point)
        {
            switch (MessageBox.Show("betelt a térkép ennyi pontod van: " + point + "!\n El szeretnéd menteni?", "Játék vége", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    savePoint1.Visible = true;
                    savePoint1.pointLabel.Text = "A pontjai: ";
                    leaderboardButton.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Occurst when the player press any of the tiles.
        /// </summary>
        private void tileClick(object sender, EventArgs e)
        {
            //gets what tile was pressed and set's it's background image to the card that was shown on the bottom left corner.
            //also resize the image to 150*150
            Button pressedButton = (Button)sender;
            Globals.gameMap[Convert.ToInt32(pressedButton.Name.Substring(6, 1)) + 1,
                    Convert.ToInt32(pressedButton.Name.Substring(7, 1)) + 1] = Globals.currentCard;
            Bitmap newImage = new Bitmap(150, 150);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(Image.FromFile(Globals.currentCardName), 0, 0, 150, 150);
            }
            pressedButton.BackgroundImage = newImage;
            pressedButton.FlatAppearance.BorderSize = 0;

            //the game go through the map if it's full or not
            bool televan = true;
            for (int i = 1; i < Globals.gameMap.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < Globals.gameMap.GetLength(1) - 1; j++)
                {
                    if (Globals.gameMap[i, j] == null)
                    {
                        televan = false;
                        goto end;
                    }
                }
            }
        end:
            if (televan == true)
                endGame(pointCount(true));

            //Request a new card.
            newCard();

            //Variables to determine if it's possible to put the card on a specific tile.
            bool volteegyis = false;
            bool körülöttevane = false;
            bool voltefel = true;
            bool voltele = true;
            bool voltejobbra = true;
            bool voltebalra = true;

            //makes every button transparent and disabled so the player cant press them.
            foreach (var item in Controls.OfType<Button>())
            {
                if (item.Name.Contains("button"))
                {
                    item.Enabled = false;
                    item.BackColor = Color.Transparent;
                }
            }

            //the program go through the full game map looking for places where the new card can be placed.
            for (int i = 1; i < Globals.gameMap.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < Globals.gameMap.GetLength(1) - 1; j++)
                {
                    //the tile must be empty so the player can put the new card there.
                    if (Globals.gameMap[i, j] == null)
                    {
                        //if there's any not empty card around that tile we need to exemine if the card could fit with the cards next to it.
                        if (Globals.gameMap[i + 1, j] != null || Globals.gameMap[i - 1, j] != null || Globals.gameMap[i, j + 1] != null || Globals.gameMap[i, j - 1] != null)
                        {
                            körülöttevane = true;

                            //looks the card under the tile
                            if (Globals.gameMap[i + 1, j] != null)
                            {
                                if (Globals.gameMap[i + 1, j].top != Globals.currentCard.bottom)
                                {
                                    voltefel = false;
                                }
                            }
                            //looks the card top of the tile
                            if (Globals.gameMap[i - 1, j] != null)
                            {
                                if (Globals.gameMap[i - 1, j].bottom != Globals.currentCard.top)
                                {
                                    voltele = false;
                                }
                            }
                            //looks the card next to the right of the tile
                            if (Globals.gameMap[i, j + 1] != null)
                            {
                                if (Globals.gameMap[i, j + 1].left != Globals.currentCard.right)
                                {
                                    voltejobbra = false;
                                }
                            }
                            //looks the card next to the left of the tile
                            if (Globals.gameMap[i, j - 1] != null)
                            {
                                if (Globals.gameMap[i, j - 1].right != Globals.currentCard.left)
                                {
                                    voltebalra = false;
                                }
                            }
                        }
                    }
                    //if it's possible to put the new card there the program enables that tile and colors it light Cyan to help the player.
                    if (voltefel && voltele && voltejobbra && voltebalra && körülöttevane)
                    {
                        volteegyis = true;
                        foreach (var item in Controls.OfType<Button>())
                        {
                            if (item.Name == $"button{i - 1}{j - 1}")
                            {
                                item.Enabled = true;
                                item.BackColor = Color.LightCyan;
                            }
                        }
                    }

                    //reset the variables
                    körülöttevane = false;
                    voltebalra = true;
                    voltefel = true;
                    voltejobbra = true;
                    voltele = true;
                }
            }
            //if there weren't any tiles to put the new card then the game is ended and ask the player if he/she wants to save the points.
            if (volteegyis == false)
            {
                int pontok = pointCount(false);
                switch (MessageBox.Show("nem tudod hova rakni pontszámod: " + pontok + "!\n Elszeretnéd menteni?", "Játék vége", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        savePoint1.Visible = true;
                        savePoint1.pointLabel.Text = "A pontjai: " + pontok;
                        leaderboardButton.Enabled = false;
                        break;
                    default:
                        Application.Exit();
                        break;
                }
            }
        }

        /// <summary>
        /// Summarize the points of the player at the end of the game.
        /// </summary>
        /// <param name="mapIsFUll"></param>
        /// <returns>The points of the player.</returns>
        public int pointCount(bool mapIsFUll)
        {
            //total of the points
            int pointSum = 0;

            //if the map is full then it's 10 plus points
            if (mapIsFUll)
                pointSum += 10;

            //the program goes through the hole game map to see every tiles
            for (int i = 1; i < Globals.gameMap.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < Globals.gameMap.GetLength(1) - 1; j++)
                {
                    //the empty tiles are not worth a single points so we need tiles with cards on them.
                    if (Globals.gameMap[i, j] != null)
                    {
                        //Count the castles on the map and add point based on that.
                        if (Globals.gameMap[i, j].center == 'C')
                        {
                            pointSum += 2;
                        }
                        //Count the roades on the map and add point based on that.
                        if (Globals.gameMap[i, j].center == 'R')
                        {
                            pointSum++;
                        }
                        //Count the points after churces.
                        //Looks every tile around all the way, even aslope.
                        if (Globals.gameMap[i, j].center == 'J')
                        {
                            if (Globals.gameMap[i - 1, j - 1] != null)
                                pointSum++;
                            if (Globals.gameMap[i - 1, j] != null)
                                pointSum++;
                            if (Globals.gameMap[i - 1, j + 1] != null)
                                pointSum++;
                            if (Globals.gameMap[i, j + 1] != null)
                                pointSum++;
                            if (Globals.gameMap[i + 1, j + 1] != null)
                                pointSum++;
                            if (Globals.gameMap[i + 1, j] != null)
                                pointSum++;
                            if (Globals.gameMap[i + 1, j - 1] != null)
                                pointSum++;
                            if (Globals.gameMap[i, j - 1] != null)
                                pointSum++;
                        }
                    }
                }
            }

            //to the total of the points it's add the full castles and full roads.
            pointSum += searchForFullCastle(pointSum);
            Console.WriteLine("<=========>");
            Console.WriteLine();
            Console.WriteLine("<===========>");
            pointSum += searchForFullRoad();

            //returns the total points
            return pointSum;
        }

        /// <summary>
        /// Search for finished roads and if theres is any then returns points after them.
        /// </summary>
        /// <returns>The points after the finished roads</returns>
        public int searchForFullRoad()
        {
            return 0;
        }

        /// <summary>
        /// Search for finished castles and if theres is any then returns points after them.
        /// </summary>
        /// <returns>The points after the finished castles</returns>
        public int searchForFullCastle(int pointSum)
        {
            return 0;
        }

        /// <summary>
        /// Loads the leaderboard.
        /// </summary>
        private void leaderboardButton_Click(object sender, EventArgs e)
        {
            //makes the leaderboard panel visible
            panel1.Visible = true;
            quitButton.BringToFront();

            //clears the rows and load the leaderboard txt into them by sordting from highest to lowest
            leaderboard.Rows.Clear();
            List<leaderboardUsers> entries = new List<leaderboardUsers>();
            foreach (var item in File.ReadLines("leaderboard.txt",Encoding.UTF8))
            {
                if(item!="")
                entries.Add(new leaderboardUsers(item));
            }
            var sortedEntries = entries.OrderByDescending(x => x.point);
            foreach (var item in sortedEntries)
            {
                leaderboard.Rows.Add(item.name, item.point);
            }
        }

        /// <summary>
        /// Load back to the game.
        /// </summary>
        private void backToGame_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}