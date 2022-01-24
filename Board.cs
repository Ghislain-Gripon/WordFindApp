using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RévisionCSHARP
{
    internal class Board
    {
        Dice[,] _gameBoard = new Dice[4,4];

        public Board(string DiceFile)
        {
            _gameBoard = BuildGameBoard(DiceFile);
        }

        /// <summary>
        /// Builds the gameboard from the dices as a string file.
        /// </summary>
        /// <param name="DiceFile">The dices as string</param>
        /// <returns>The dice two dimensional array making up the board</returns>
        private Dice[,] BuildGameBoard(string DiceFile)
        {
            Dice[,] GameBoard = new Dice[4, 4];
            Dice[] Dices = new Dice[16];
            Random random = new Random();

            try
            {

                using StringReader DiceFacesValues = new StringReader(DiceFile);
                
                for(int i = 0; i < 16; i++)
                {
                    char[] Faces = Array.ConvertAll(DiceFacesValues.ReadLine().Split(";"), new Converter<string, char>(s => Convert.ToChar(s)));
                    Dices[i] = new Dice(Faces, new Random());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    int index = random.Next(i*4+j, 16);
                    GameBoard[i, j] = Dices[index];
                    Dices[index] = Dices[(i*4)+j];
                    Dices[(i*4)+j] = GameBoard[i, j];
                }
            }

            return GameBoard;
            
        }

        /// <summary>
        /// Shuffles the board using the supplied Random to choose which dice are moved iteratively.
        /// </summary>
        /// <param name="Random">An instance of the System.Random class.</param>
        public void ShuffleBoard(Random Random)
        {
            Dice[] Dices = new Dice[16];

            //Dices are collected for shuffling.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Dices[(i * 4) + j] = _gameBoard[i, j];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //A dice is selected between all the remaning dices not yet choosen in a linear fashion.
                    //When a dice is selected, it is moved to the position on the board according to i and j
                    //and moved in the array at the first place available not occupied by an already moved dice.
                    int index = Random.Next(i * 4 + j, 16);
                    _gameBoard[i, j] = Dices[index];
                    Dices[index] = Dices[(i * 4) + j];
                    Dices[(i * 4) + j] = _gameBoard[i, j];
                    _gameBoard[i,j].Launch(Random);
                }
            }
        }

        /// <summary>
        /// Searches the word in the board by iteratively seeking if the first letter is found, if yes launches the recursive calls searching neighbouring cells in all eight directions.
        /// </summary>
        /// <param name="Word">The searched word as a char array</param>
        /// <returns>True if the word is found, false if it is not</returns>
        public bool TestBoard(char[] Word)
        {
            bool Found = false;
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (_gameBoard[i, j].ToString() == Word[0].ToString())
                    {
                        List<Tuple<int, int>> VisitedCellsCoordinates = new List<Tuple<int, int>>();
                        VisitedCellsCoordinates.Add(new Tuple<int, int>(i, j));
                        SearchNeighbouringCells(Word, 1, VisitedCellsCoordinates, i, j, ref Found);
                    }
                        
                }
            }
            return Found;
        }

        /// <summary>
        /// Searches neighbouring cells to the one at (CellAbscissa, CellOrdinate) in the board which were not already visited for the letter at WordCharacterCheckIncrement in Word array
        /// </summary>
        /// <param name="Word">The searched word as a char array</param>
        /// <param name="WordCharacterCheckIncrement">Integer representing which letter from the char word array to check for</param>
        /// <param name="VisitedCellsCoordinates">List of tuples of the abscissa and ordinate of visited cells</param>
        /// <param name="CellAbscissa">Abscissa of the cell whose neighbours are to be searched</param>
        /// <param name="CellOrdinate">Ordinate of the cell whose neighbours are to be searched</param>
        /// <param name="WordFound">A boolean passed by reference to set it true if the word is found, remains false otherwise</param>
        private void SearchNeighbouringCells(char[] Word, int WordCharacterCheckIncrement, List<Tuple<int, int>> VisitedCellsCoordinates, int CellAbscissa, int CellOrdinate, ref bool WordFound)
        {
            if(WordCharacterCheckIncrement == Word.Length)
            {
                WordFound = true;
                return;
            }
                
            for(int i = CellAbscissa - 1; i <= CellAbscissa + 1 && i < 4; i++ )
            {
                for(int j = CellOrdinate - 1; j <= CellOrdinate + 1 && j < 4; j++)
                {
                    if(i >= 0 && j >= 0 && !VisitedCellsCoordinates.Contains(new Tuple<int, int>(i, j)) && _gameBoard[i, j].ToString() == Word[WordCharacterCheckIncrement].ToString())
                    {
                        VisitedCellsCoordinates.Add(new Tuple<int, int>(i, j));
                        SearchNeighbouringCells(Word, WordCharacterCheckIncrement + 1, VisitedCellsCoordinates, i, j, ref WordFound);
                    }
                }
            }
        }


        public override string ToString()
        {
            string BoardState = "";
            for(int i = 0; i<4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    BoardState += _gameBoard[i, j].ToString() + " ";
                }
                BoardState += i != 3 ? "\n" : "";
            }

            return BoardState;
        }

    }
}
