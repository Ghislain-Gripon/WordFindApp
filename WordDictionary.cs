using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace RévisionCSHARP
{
    internal class WordDictionary
    {
        /// <summary>
        /// The structure for the dictionary files is perfect for a System.Collections.Generic.Dictionary as allows to associate the same-sized ordered lines of words with their length as key.
        /// </summary>
        Dictionary<int, string[]> _dictionary = new Dictionary<int, string[]>();

        /// <summary>
        /// Creates a dictionary storing words by (key,value), key being the words lenght and value a string array of said words.
        /// Since dictionaries as text file are quite small and we need to load them into memory anyway, there is no need to open a stream on the file, the file is passed as string.
        /// </summary>
        /// <param name="DictionaryFile">The read dictionary file as a string.</param>
        public WordDictionary(string DictionaryFile)
        {
            StringReader reader = new StringReader(DictionaryFile);
            string line = "";
            while((line = reader.ReadLine()) != null)
            {
                int WordLength = Convert.ToInt32(line);
                string[] words = reader.ReadLine().ToUpper().Split(' ');
                _dictionary.Add(WordLength, words);
            }
            
        }

        /// <summary>
        /// Binary Search is very quick to find if a word is or is not in the dictionary. It divides roughly by two the array each iteration with only one read.
        /// If we take L the lenght of the array and N the number of calls to be made, we have L/2^N = 1 since we exit either if we find the word in the two last comparisons or if the last one doesn't yield any.
        /// If we use e^ln(x) = x on this equation we find that ln(L)/l(2) = N, thus by rounding it up we get the number of calls. Since logarithm is a function that grows exponentially slowly as the abscissa increases,
        /// the quality let us know that we need a considerably large total length to increase drastically the number of calls required to verify if the word is there or not.
        /// With around 50k in length we round up N to 16 calls.
        /// It is universe faster than a bruteforce approach of comparing with each element of the array.
        /// </summary>
        /// <param name="Word">The word to be searched, case insensitive.</param>
        /// <param name="Start">For internal use of the binary search algorithm, represent the left bound of the sub array being searched.</param>
        /// <param name="End">The right bound of the search sub array.</param>
        /// <returns>True is the word is in, false otherwise.</returns>
        public bool ResearchWordRecursive(string Word, int Start = -1, int End = -1)
        {
            bool result = false;
            if(Start == -1 && End == -1)
            {
                Start = 0;
                End = _dictionary[Word.Length].Length - 1;
            }

            if(End >= Start)
            {
                switch(string.Compare(Word, _dictionary[Word.Length][Start + (End - Start) / 2], StringComparison.InvariantCultureIgnoreCase))
                {
                    case > 0:
                        result = ResearchWordRecursive(Word, Start + (End - Start) / 2 + 1, End);
                        break;
                    case 0:
                        result = true;
                        break;
                    case < 0:
                        result = ResearchWordRecursive(Word, Start, Start + (End - Start) / 2 - 1);
                        break;
                }
            }

            return result;
        }

    }
}
