using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vadaszDenes
{
    internal class leaderboardUsers
    {
        /// <summary>
        /// The name of the player in the txt file.
        /// </summary>
        public string name;

        /// <summary>
        /// The points of the player in the txt file.
        /// </summary>
        public int point;

        /// <summary>
        /// Makes an object from a line in the txt.
        /// </summary>
        /// <param name="line">The line from the txt.</param>
        public leaderboardUsers(string line)
        {
            this.name = line.Split(';')[0];
            this.point = Convert.ToInt32(line.Split(';')[1]);
        }
    }
}
