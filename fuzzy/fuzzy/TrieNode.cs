using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuzzy
{
    public class TrieNode
    {
        public static int NodeCount = 0;
        public static int WordCount = 0;

        public string Word { get; set; }
        public Dictionary<char, TrieNode> Children { get; private set; }

        public TrieNode()
        {
            TrieNode.NodeCount++;
            Children = new Dictionary<char, TrieNode>();
        }

        public void Insert(string word)
        {
            TrieNode node = this;
            for (int i = 0, j = word.Length; i < j; i++)
            {
                if (!node.Children.ContainsKey(word[i]))
                {
                    node.Children.Add(word[i], new TrieNode());
                }

                node = node.Children[word[i]];
            }

            node.Word = word;
            TrieNode.WordCount++;
        }

        public IEnumerable<string> Search(string word, int maxDistance)
        {
            int[] currentRow = Enumerable.Range(0, word.Length + 1).ToArray();

            foreach (char key in Children.Keys)
            {
                foreach (string result in SearchRecursive(Children[key], key, word, currentRow, maxDistance))
                    yield return result;
            }

            yield return string.Empty;
        }

        private IEnumerable<string> SearchRecursive(TrieNode node, char letter, string word, int[] previousRow, int maxDistance)
        {
            int columns = word.Length + 1;
            int[] currentRow = new int[columns];

            currentRow[0] = previousRow[0] + 1;

            for (int i = 1, j = columns; i < j; i++)
            {
                int insertCost = currentRow[i -1] + 1;
                int deleteCost = previousRow[i] + 1;

                int replaceCost = 0;
                if (word[i -1] != letter) 
                    replaceCost = previousRow[i -1] + 1;
                else
                    replaceCost = previousRow[i -1];

                currentRow[i] = Math.Min(Math.Min(insertCost, deleteCost), replaceCost);
            }

            if (currentRow[columns -1] <= maxDistance && node.Word != null)
                yield return node.Word;

            if (currentRow.Min() <= maxDistance)
            {
                foreach (char key in node.Children.Keys)
                {
                    foreach (string result in SearchRecursive(node.Children[key], key, word, currentRow, maxDistance))
                        yield return result;
                }
            }
        }
    }
}
