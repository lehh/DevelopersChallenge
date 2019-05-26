using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChampionshipManager.Model;

namespace ChampionshipManager.Utils
{
    //Class which contains support methods in general.
    public class Tools
    {
        //From Greg Hewgill response in stackoverflow -> https://stackoverflow.com/questions/600293/how-to-check-if-a-number-is-a-power-of-2
        /// <summary>
        /// Verify if a number is a power of two.
        /// </summary>
        public static bool IsPowerOfTwo(int number)
        {
            return (number != 0) && ((number & (number - 1)) == 0);
        }

        /// <summary>
        /// Returns, based on the leaves, the zero based number of nodes in the binary tree.
        /// </summary>
        public static int GetNumberOfNodes(int numberOfLeaves)
        {
            return (numberOfLeaves * 2) - 2; //Zero based
        }

        /// <summary>
        /// Randomize the positions, based on the number of teams, to define the matches.
        /// </summary>
        /// <param name="idList"></param>
        /// <returns>Hashset with numbers(indexes) in a random order</returns>
        public static HashSet<int> RandomizePositions(int teamsCount)
        {
            var treeNodesHash = new HashSet<int>();

            //Get the total number of nodes in the tree.
            var totalNodes = GetNumberOfNodes(teamsCount);

            //Define which numbers(indexes) will be randomized and returned.
            while (treeNodesHash.Count != teamsCount)
            {
                treeNodesHash.Add(totalNodes);
                totalNodes--;
            }

            var random = new Random();
            treeNodesHash = treeNodesHash.OrderBy(number => random.Next()).ToHashSet();

            //Return the hash in a random order.
            return treeNodesHash;
        }
    }
}