using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RévisionCSHARP
{
    internal class Player
    {
        string _name;
        int _score = 0;
        List<string> _words = new List<string>();

        public Player(string name)
        {
            _name = name;
        }

        public string GetName
        { get => _name; }

        public int GetScore 
        { get => _score; }

        public bool Contain(string word)
        {
            return _words.Contains(word);
        }

        /// <summary>
        /// Adds the word to the player's list if it is not already in it.
        /// Increases the player's score according to the game rule.
        /// </summary>
        /// <param name="word">The word to add to the player's list</param>
        public void Add_Word(string word)
        {
            word = word.ToUpper();
            if (Contain(word)) return;

            if (word.Length < 7)
                _score += word.Length - 1;
            else
                _score += 11;
            _words.Add(word);
        }

        public override string ToString()
        {
            string to_string = String.Format("The player {0} with score {1} has found ", _name, _score);
            to_string += _words.Count > 0 ? "\n"+String.Join(", ", _words.ToArray()) : "no words yet";
            return to_string + ".";
        }
    }
}
