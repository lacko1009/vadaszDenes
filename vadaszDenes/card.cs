using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vadaszDenes
{
    internal class card
    {
        /// <summary>
        /// Determinate what the top part of the card ends with.
        /// </summary>
        public char top { get; set; }

        /// <summary>
        /// Determinate what the bottom part of the card ends with.
        /// </summary>
        public char bottom { get; set; }

        /// <summary>
        /// Determinate what the right part of the card ends with.
        /// </summary>
        public char right { get; set; }

        /// <summary>
        /// Determinate what the left part of the card ends with.
        /// </summary>
        public char left { get; set; }
        
        /// <summary>
        /// Determinate what the center part of the card is.
        /// </summary>
        public char center { get; set; }

        /// <summary>
        /// To create new Card objects.
        /// </summary>
        /// <param name="top">The top end of the card.</param>
        /// <param name="bottom">The bottom end of the card.</param>
        /// <param name="right">The right end of the card.</param>
        /// <param name="left">The left end of the card.</param>
        /// <param name="center">The center part of the card.</param>
        /// <returns>A <see cref="Card" /> object.</returns>
        public card(char top, char bottom, char right, char left, char center)
        {
            this.top = top;
            this.bottom = bottom;
            this.right = right;
            this.left = left;
            this.center = center;
        }
        //Possible params:
        // N : Nothing
        // R : Road
        // C : Castle
        // J : Church
    }
}
