using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrissCross.Models
{
    public class NewCrossBoard
    {
        private List<string> listOfUnplacedWords = new List<string>();

        private List<(string, int, int, bool)>
            listOfPlacedWords = new List<(string, int, int, bool)>(); // relative coords x and y, bool horiz direction

        public ResizableMatrix matr { get; set; } = new ResizableMatrix(1, 1);
        private List<(int, int)> wordCrossowersRelativeCoords = new List<(int, int)>();
        private List<(string, int, int, bool)> badPlacingPositionAbsoluteCoords = new List<(string, int, int, bool)>();
        public double wordsDensityCoeff; // (wordCrossowers/matrixDimension)

        public NewCrossBoard(string wordsFilepath, double wordsDensityCoeff = 0.1)
        {
            LoadWords(wordsFilepath);
            this.wordsDensityCoeff = wordsDensityCoeff;
        }

        private (bool HasBeenPlaced, int RelativePlacedCoordX, int RelativePlacedCoordY, bool Direction) PlaceWord(
            string word)
        {
            bool isWordPlaced = false;
            (int, int) RelativeCoords = (0, 0); // It's an impossible position for non-first placed words in the matrix
            (int, int) AbsoluteCoords = (0, 0);
            bool isHoriz = false;
            int charIndx = 0;

            if (listOfPlacedWords.Count == 0)
            {
                matr.Resize(0, 0, 0, word.Length - 1);
                for (int i = 0; i < word.Length; i++)
                {
                    matr.SetByAbsoluteIndex(0, i, word[i]);
                    isHoriz = true;
                    isWordPlaced = true;
                }
            }
            else
            {
                for (int k = 0; k < word.Length && !isWordPlaced; k++)
                {
                    for (int i = 0; i < matr.Rows && !isWordPlaced; i++)
                    {
                        for (int j = 0; j < matr.Cols && !isWordPlaced; j++)
                        {
                            if (matr.GetByAbsoluteIndex(i, j) == word[k])
                            {
                                if (IsFreeToPlaceHoriz(i, j, word, k, out AbsoluteCoords.Item1,
                                        out AbsoluteCoords.Item2) &&
                                    !badPlacingPositionAbsoluteCoords.Contains((word, i, j, true)))
                                {
                                    isWordPlaced = true;
                                    //AbsoluteCoords = (i, j-k);
                                    RelativeCoords =
                                        matr.AbsoluteToRelative(AbsoluteCoords.Item1, AbsoluteCoords.Item2);
                                    isHoriz = true;
                                }
                                else if (IsFreeToPlaceVert(i, j, word, k, out AbsoluteCoords.Item1,
                                             out AbsoluteCoords.Item2) &&
                                         !badPlacingPositionAbsoluteCoords.Contains((word, i, j, false)))
                                {
                                    isWordPlaced = true;
                                    //AbsoluteCoords = (i-k, j);
                                    RelativeCoords =
                                        matr.AbsoluteToRelative(AbsoluteCoords.Item1, AbsoluteCoords.Item2);
                                    isHoriz = false;
                                }
                            }
                        }
                    }
                }
            }

            if (isWordPlaced)
            {
                if (isHoriz)
                {
                    for (int k = 0; k < word.Length; k++)
                    {
                        matr.SetByAbsoluteIndex(AbsoluteCoords.Item1, AbsoluteCoords.Item2 + k, word[k]);
                    }
                }
                else
                {
                    for (int k = 0; k < word.Length; k++)
                    {
                        matr.SetByAbsoluteIndex(AbsoluteCoords.Item1 + k, AbsoluteCoords.Item2, word[k]);
                    }
                }
            }

            return (isWordPlaced, RelativeCoords.Item1, RelativeCoords.Item2, isHoriz);
        }

        private static (int relCoordX, int relCoordY, bool isHoriz) GetDataByWord(
            List<(string, int, int, bool)> listOfWords, string word)
        {
            foreach ((string, int, int, bool) wd in listOfWords)
            {
                if (wd.Item1 == word)
                {
                    return (wd.Item2, wd.Item3, wd.Item4);
                }
            }

            throw new Exception("Word not found");
        }


        private bool DeleteWord(string word)
        {

            bool isDeleted = false;
            if (listOfPlacedWords.Count == 1)
            {
                matr = new ResizableMatrix(1, 1);
                wordCrossowersRelativeCoords = new List<(int, int)>();
                isDeleted = true;
            }
            else
            {
                (int relCoordX, int relCoordY, bool isHoriz) = GetDataByWord(listOfPlacedWords, word);
                if (isHoriz)
                {
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (!wordCrossowersRelativeCoords.Remove((relCoordX, relCoordY + j)))
                        {
                            matr.SetByRelativeIndex(relCoordX, relCoordY + j, '\0');
                        }
                    }

                    isDeleted = true;
                }
                else
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (!wordCrossowersRelativeCoords.Remove((relCoordX + i, relCoordY)))
                        {
                            matr.SetByRelativeIndex(relCoordX + i, relCoordY, '\0');
                        }
                    }

                    isDeleted = true;
                }
            }

            if (isDeleted)
            {
                matr.Fit();
            }

            return isDeleted;
        }

        private bool IsFreeToPlaceHoriz(int rowCoord, int colCoord, string word, int charIndx, out int absCoordRow,
            out int absCoordCol)
        {
            bool isAvailbleToPlace = true;
            List<(int, int)> potentialCrossowers = new List<(int, int)>();

            bool isExtended = false;
            int startInsertionCol = colCoord - charIndx;
            int endColumn = startInsertionCol + word.Length; //- 1;
            int oldCol = matr.Cols;

            if (startInsertionCol < 0)
            {
                matr.Resize(0, 0, -startInsertionCol, 0);
                startInsertionCol = 0;
                charIndx = 0;
                isExtended = true;
            }

            if (endColumn >= oldCol)
            {
                matr.Resize(0, 0, 0, endColumn - oldCol);
                isExtended = true;
            }

            absCoordCol = startInsertionCol;
            absCoordRow = rowCoord;

            bool isFirstCrossower = true;
            int charsAfterCrossower = 0;
            for (int i = 0; i < word.Length && isAvailbleToPlace; i++)
            {
                /*
                bool isAboveEmpty = rowCoord == 0 || matr.GetByAbsoluteIndex(rowCoord - 1, startInsertionCol + i) == '\0';
                bool isBelowEmpty = rowCoord == matr.Rows - 1 || matr.GetByAbsoluteIndex(rowCoord + 1, startInsertionCol + i) == '\0';
                bool isLeftEmpty = startInsertionCol + i == 0 || matr.GetByAbsoluteIndex(rowCoord, startInsertionCol  + i - 1) == '\0';
                bool isRightEmpty = startInsertionCol + i == matr.Cols - 1 || matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i + 1) == '\0';
                bool isCrossower = matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i) == word[i];
                */
                bool isAboveEmpty = rowCoord - 1 < 0 ||
                                    matr.GetByAbsoluteIndex(rowCoord - 1, startInsertionCol + i) == '\0';
                bool isBelowEmpty = rowCoord + 1 >= matr.Rows ||
                                    matr.GetByAbsoluteIndex(rowCoord + 1, startInsertionCol + i) == '\0';
                bool isLeftEmpty = startInsertionCol + i - 1 < 0 ||
                                   matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i - 1) == '\0';
                bool isRightEmpty = startInsertionCol + i + 1 >= matr.Cols ||
                                    matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i + 1) == '\0';
                bool isCrossower = matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i) == word[i];

                //Console.WriteLine(word + " " + i + " " + rowCoord + " " + colCoord + " ");

                if (matr.GetByAbsoluteIndex(rowCoord, startInsertionCol + i) == '\0' || isCrossower)
                {
                    if (isCrossower && (!isLeftEmpty || !isRightEmpty))
                    {
                        {
                            isAvailbleToPlace = false;
                        }
                    }

                    if (!isCrossower && (!isAboveEmpty || !isBelowEmpty))
                    {
                        isAvailbleToPlace = false;
                    }

                    if (isFirstCrossower && !isLeftEmpty)
                    {
                        isAvailbleToPlace = false;
                    }

                    if (i == word.Length - 1 && !isRightEmpty)
                    {
                        isAvailbleToPlace = false;
                    }
                }
                else
                {
                    isAvailbleToPlace = false;
                }

                if (isCrossower && isAvailbleToPlace)
                {
                    isFirstCrossower = false;
                    charsAfterCrossower = 0;
                    potentialCrossowers.Add(matr.AbsoluteToRelative(rowCoord, startInsertionCol + i));
                }

                charsAfterCrossower++;
            }

            if (isExtended && !isAvailbleToPlace)
            {
                matr.Fit();
            }

            else if (isAvailbleToPlace && potentialCrossowers.Count > 0)
            {
                wordCrossowersRelativeCoords.AddRange(potentialCrossowers);
            }

            return isAvailbleToPlace;
        }

        private bool IsFreeToPlaceVert(int rowCoord, int colCoord, string word, int charIndx, out int absCoordRow,
            out int absCoordCol)
        {
            bool isAvailbleToPlace = true;
            List<(int, int)> potentialCrossowers = new List<(int, int)>();

            bool isExtended = false;
            int startInsertionRow = rowCoord - charIndx;
            int endRow = startInsertionRow + word.Length;
            int oldRow = matr.Rows;

            if (startInsertionRow < 0)
            {
                matr.Resize(-startInsertionRow, 0, 0, 0);
                startInsertionRow = 0;
                charIndx = 0;
                isExtended = true;
            }

            if (endRow >= oldRow)
            {
                matr.Resize(0, endRow - oldRow, 0, 0);
            }

            absCoordCol = colCoord;
            absCoordRow = startInsertionRow;

            bool isFirstCrossower = true;
            int charsAfterCrosswer = 0;
            for (int i = 0; i < word.Length && isAvailbleToPlace; i++)
            {
                /*
                bool isAboveEmpty = startInsertionRow + i - 1 == 0 || matr.GetByAbsoluteIndex(startInsertionRow + i - 1, colCoord) == '\0';
                bool isBelowEmpty = startInsertionRow + i + 1 == matr.Rows - 1 || matr.GetByAbsoluteIndex(startInsertionRow + i + 1, colCoord) == '\0';
                bool isLeftEmpty = colCoord == 0 || matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord- 1) == '\0';
                bool isRightEmpty = colCoord == matr.Cols - 1 || matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord + 1) == '\0';
                bool isCrossower = matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord) == word[i];
                */
                bool isAboveEmpty = startInsertionRow + i - 1 < 0 ||
                                    matr.GetByAbsoluteIndex(startInsertionRow + i - 1, colCoord) == '\0';
                bool isBelowEmpty = startInsertionRow + i + 1 >= matr.Rows ||
                                    matr.GetByAbsoluteIndex(startInsertionRow + i + 1, colCoord) == '\0';
                bool isLeftEmpty = colCoord - 1 < 0 ||
                                   matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord - 1) == '\0';
                bool isRightEmpty = colCoord + 1 >= matr.Cols ||
                                    matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord + 1) == '\0';
                bool isCrossower = matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord) == word[i];

                //Console.WriteLine(word + " " + i + " " + rowCoord + " " + colCoord + " ");

                if (matr.GetByAbsoluteIndex(startInsertionRow + i, colCoord) == '\0' || isCrossower)
                {
                    if (isCrossower && (!isAboveEmpty || !isBelowEmpty))
                    {
                        isAvailbleToPlace = false;
                    }

                    if (!isCrossower && (!isLeftEmpty || !isRightEmpty))
                    {
                        isAvailbleToPlace = false;
                    }

                    if (isFirstCrossower && !isAboveEmpty)
                    {
                        isAvailbleToPlace = false;
                    }

                    if (i == word.Length - 1 && !isBelowEmpty)
                    {
                        isAvailbleToPlace = false;
                    }
                }
                else
                {
                    isAvailbleToPlace = false;
                }

                if (isCrossower && isAvailbleToPlace)
                {
                    isFirstCrossower = false;
                    charsAfterCrosswer = 0;
                    //Console.WriteLine(word);
                    potentialCrossowers.Add(matr.AbsoluteToRelative(startInsertionRow + i, colCoord));
                }

                charsAfterCrosswer++;
            }

            if (isExtended && !isAvailbleToPlace)
            {
                matr.Fit();
            }

            else if (isAvailbleToPlace && potentialCrossowers.Count > 0)
            {
                wordCrossowersRelativeCoords.AddRange(potentialCrossowers);
            }

            return isAvailbleToPlace;
        }

        public bool Solve()
        {
            return BacktrackingAlg();
        }

        public bool BacktrackingAlg()
        {
            if (listOfUnplacedWords.Count == 0 &&
                (double)wordCrossowersRelativeCoords.Count / (matr.Rows * matr.Cols) >= wordsDensityCoeff)
            {
                return true;
            }

            foreach (string wd in listOfUnplacedWords.ToList())
            {
                (bool HasBeenPlaced, int RelX, int RelY, bool IsHoriz) = PlaceWord(wd); //
                if (HasBeenPlaced)
                {
                    Console.WriteLine($"Successfully placed: {wd}");
                    listOfPlacedWords.Add((wd, RelX, RelY, IsHoriz));
                    badPlacingPositionAbsoluteCoords.Add((wd, RelX, RelY, IsHoriz));
                    listOfUnplacedWords.Remove(wd);
                    if (BacktrackingAlg())
                    {
                        return true;
                    }

                    Console.WriteLine($"Backtracking removing: {wd}");
                    badPlacingPositionAbsoluteCoords.Remove((wd, RelX, RelY, IsHoriz));
                    DeleteWord(wd);
                    listOfPlacedWords.Remove((wd, RelX, RelY, IsHoriz));
                    listOfUnplacedWords.Add(wd);
                }
                else
                {
                    Console.WriteLine($"Failed to place: {wd}");
                }
            }

            return false;
        }


        private void LoadWords(string wordFilePath)
        {
            try
            {
                string[] words = File.ReadAllLines(wordFilePath);
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].ToLower().Trim();
                }

                listOfUnplacedWords.AddRange(words);
                listOfUnplacedWords.Sort((a, b) => b.Length.CompareTo(a.Length));
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].ToLower();
                }

                Console.WriteLine(
                    $"Загружено {listOfUnplacedWords.Count} слов из файла: {wordFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке слов из файла: {ex.Message}");
            }
        }
    }
}