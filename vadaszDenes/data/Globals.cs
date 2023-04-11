using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vadaszDenes
{
    internal class Globals
    {
        /// <summary>
        /// The current card and it's data.
        /// </summary>
        public static card currentCard = new card(' ', ' ', ' ', ' ', ' ');

        /// <summary>
        /// The current card name.png.
        /// </summary>
        public static string currentCardName = "";

        /// <summary>
        /// The map of the game, built from tiles.
        /// </summary>
        public static card[,] gameMap = new card[7, 10];

        /// <summary>
        /// The card database
        /// </summary>
        public static JObject data;

        /// <summary>
        /// The card images.
        /// </summary>
        public static FileInfo[] everyCard = new DirectoryInfo("Cards").GetFiles();

        /// <summary>
        /// Random number generator.
        /// </summary>
        public static Random randomNumber = new Random();
    }
}
