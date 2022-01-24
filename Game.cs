using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RévisionCSHARP
{
    internal class Game
    {
        private Board _board;
        private Player _currentPlayer;
        private WordDictionary _wordDictionary;

        /// <summary>
        /// Game class stores the player whose turn it is and the current board.
        /// </summary>
        /// <param name="DiceFile">Filename for the embedded ressource.</param>
        /// <param name="DictionaryFile">Filename for the embedded ressource.</param>
        public Game(string DiceFile, string DictionaryFile)
        {
            _board = new Board(EmbeddedAssemblyFileReader(DiceFile));
            _wordDictionary = new WordDictionary(EmbeddedAssemblyFileReader(DictionaryFile));
        }

        /// <summary>
        /// Read a text file embedded in the assembly.
        /// </summary>
        /// <param name="FileName">Name of the file in the assembly.</param>
        /// <returns>The file as a string.</returns>
        public string EmbeddedAssemblyFileReader(string FileName)
        {
            string File = "";
            Assembly assembly = Assembly.GetExecutingAssembly();
            try
            {
                using Stream Stream = assembly.GetManifestResourceStream(GetType(), FileName);
                StreamReader Reader = new StreamReader(Stream);
                File = Reader.ReadToEnd();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return File;
        }

        /// <summary>
        /// Runs the game. Each player enter words he finds that are three letters or longer without reusing letters. Letters used need to be adjacent in an 8 directional fashion in order to be valid.
        /// Each player's turns last one minute, the game lasts six.
        /// Whoever scores the most wins.
        /// Scores are as follows : for a word of length three to six, lenght - 1, for words seven or more, eleven points.
        /// </summary>
        public static void Main()
        {
            Game Party = new Game("Des.txt", "PossibleWords.txt");
            Player FirstPlayer = new Player("Alpha");
            Player SecondPlayer = new Player("Beta");
            
            Party._currentPlayer = SecondPlayer;
            Console.WriteLine($"Game is starting with player {FirstPlayer.GetName} and {SecondPlayer.GetName} for 6 minutes, 1 minute per turn." +
                $"\n\nEnter the words you find in the board that are three letters or longer, you can use any adjacent letters including diagonals.");

            DateTime PartyStart = DateTime.Now;
            DateTime TurnStart;
            TimeSpan TurnTime = TimeSpan.Zero;
            TimeSpan PartyTime = TimeSpan.Zero;
            while (PartyTime <= TimeSpan.FromMinutes(6))
            {
                Console.WriteLine(Party._board.ToString());
                TurnStart = DateTime.Now;
                TurnTime = TimeSpan.Zero;
                while (TurnTime <= TimeSpan.FromMinutes(1))
                {
                    Console.WriteLine(Party._currentPlayer.ToString());
                    Console.Write("Word : ");
                    string Input = Console.ReadLine().ToUpper();

                    if(Input.Length >= 3 && Party._board.TestBoard(Input.ToCharArray()) && Party._wordDictionary.ResearchWordRecursive(Input))
                    {
                        Party._currentPlayer.Add_Word(Input);
                    }
                    TurnTime = DateTime.Now - TurnStart;
                }
                Party._currentPlayer = Party._currentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
                Party._board.ShuffleBoard(new Random());
                PartyTime = DateTime.Now - PartyStart;
            }
            
            if(FirstPlayer.GetScore != SecondPlayer.GetScore)
            {
                Player Winner = FirstPlayer.GetScore > SecondPlayer.GetScore ? FirstPlayer : SecondPlayer;
                Player Loser = Winner == FirstPlayer ? SecondPlayer : FirstPlayer;
                Console.WriteLine($"{Winner.GetName} wins with a score of {Winner.GetScore}, besting {Loser.GetName} with only {Loser.GetScore} points.");
            }
            else
            {
                Console.WriteLine($"It's a tie, both {FirstPlayer.GetName} and {SecondPlayer.GetName} got the same score of {FirstPlayer.GetScore}.");
            }
            Console.ReadKey();
        }

        //Function used to convert english dictionary from https://github.com/dwyl/english-words/blob/master/words_alpha.txt
        //into the
        //wordlength
        //words by alphebetical order separated with a space
        //for use in the dictionary structure
        //public static void Main()
        //{
        //    using StreamReader reader = new StreamReader(@"C:\Users\akiraraiku\source\repos\RévisionCSHARP\PossibleWords.txt");
        //    string[] words = reader.ReadToEnd().Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //    var wordlist = words.OrderBy(str => str.Length).ThenBy(str => str);
        //    string[] stringwordlist = wordlist.ToArray<string>();

        //    int index_start = 0;
        //    int index_end = 0;
        //    int wordsize = 1;
        //    for (int i = 0; i < stringwordlist.Length; i++)
        //    {
        //        index_end = i;
        //        if (stringwordlist[i].Length == wordsize && i == stringwordlist.Length - 1)
        //        {
        //            index_end++;
        //        }
        //        if (!(stringwordlist[i].Length == wordsize) || (stringwordlist[i].Length == wordsize && i == stringwordlist.Length - 1))
        //        {
        //            using StreamWriter writer = new StreamWriter(@"C:\Users\akiraraiku\Documents\Code Informatique\PossibleWords.txt", true, Encoding.UTF8);
        //            writer.WriteLine(wordsize);
        //            writer.Write(stringwordlist[index_start]);
        //            for (int j = index_start + 1; j < index_end; j++)
        //            {
        //                writer.Write(" " + stringwordlist[j]);
        //            }
        //            writer.WriteLine();
        //            wordsize = stringwordlist[i].Length;
        //            index_start = i;

        //        }
        //    }
        //}
    }
}
