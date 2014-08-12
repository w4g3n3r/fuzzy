using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuzzy
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            string query;
            int maxDistance;

            if (args.Length != 2 || !int.TryParse(args[1], out maxDistance))
            {
                Console.WriteLine("Usage: fuzzy <query> <distance>");
                return;
            }
            else
            {
                query = args[0];
            }

            timer.Start();
            TrieNode trie = new TrieNode();

            foreach (string name in Names()) trie.Insert(name);
            timer.Stop();

            Console.WriteLine("Loaded {0} words in {1} nodes in {2} seconds.", TrieNode.WordCount, TrieNode.NodeCount, timer.Elapsed.TotalSeconds);
            Console.WriteLine();
            Console.WriteLine("Searching for '{0}'...", query);
            Console.WriteLine();
            Console.WriteLine("Results:");
            timer.Reset();

            timer.Start();
            int resultCount = 0;
            foreach (string result in trie.Search(query, maxDistance))
            {
                resultCount++;
                Console.Write("{0} ", result);
            }

            timer.Stop();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Found {0} results for '{1}' with a max distance of {2} in {3} seconds.", 
                resultCount, query, maxDistance, timer.Elapsed.TotalSeconds);

        }


        static IEnumerable<string> Names()
        {
            using (StreamReader sw = new StreamReader("names.txt"))
            {
                while (!sw.EndOfStream)
                {
                    yield return sw.ReadLine();
                }
                sw.Close();
            }
        }
    }
}
