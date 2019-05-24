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
        public static bool IsPowerOfTwo(int number)
        {
            return (number != 0) && ((number & (number - 1)) == 0);
        }

        ///Returns, based on the leaves, the zero based number of nodes in the binary tree.
        public static int GetNumberOfNodes(int numberOfLeaves)
        {
            return ((numberOfLeaves * 2) - 1) - 1; //Zero based
        }

        public static HashSet<int> RandomizeBrackets(List<int> idList)
        {
            var teamCount = idList.Count;
            var availableNumbersHash = new HashSet<int>();
            var totalNodes = GetNumberOfNodes(teamCount);

            while (availableNumbersHash.Count != teamCount)
            {
                availableNumbersHash.Add(totalNodes);
                totalNodes--;
            }

            var random = new Random();
            availableNumbersHash.OrderBy(number => random.Next()).ToHashSet();

            return availableNumbersHash;
        }
    }
}