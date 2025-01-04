using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrissCross.Models
{
    public class CrossBoard
    {
        public struct PlacedWord
        {
            public bool isHorizDirection { get; set; }
            public string word { get; set; }
            public (int, int) place { get; set; }

            public PlacedWord(bool isHorizDirection, string word, (int, int) place)
            {
                this.isHorizDirection = isHorizDirection;
                this.word = word;
                this.place = place;
                //this.wordCrossovers = wordCrossovers;
            }
        }

        public void RemovePlacedWordByName(string wordToRemove)
        {
            var placedWord = listOfPlacedWords.FirstOrDefault(p => p.word == wordToRemove);
            if (placedWord.word != null)
            {
                listOfPlacedWords.Remove(placedWord);
            }
        }

        public static List<(int, int)> wordCrossovers { get; set; } = new List<(int, int)>();
        private List<string> listOfUnplacedWords { get; set; } = new List<string>();
        private List<PlacedWord> listOfPlacedWords = new List<PlacedWord>();
        public ResizableMatrix matr = new ResizableMatrix(1, 1);
        public double wordsDensityCoeff; // (wordCrossowers/matrixDimension)

        public CrossBoard(string wordsFilepath, double wordsDensityCoeff = 0.004)
        {
            LoadWords(wordsFilepath);
            this.wordsDensityCoeff = wordsDensityCoeff;
        }



        private bool isPerpendicularCrossover(int row, int col, bool isHorizontal)
        {
            if (isHorizontal)
            {
                // Проверяем, чтобы в перпендикулярных (вертикальных) направлениях
                // пересечение было возможным, а параллельные (горизонтальные) клетки были пустыми
                bool isAboveEmpty = row == 0 || matr.GetByAbsoluteIndex(row - 1, col) == '\0';
                bool isBelowEmpty = row == matr.Rows - 1 || matr.GetByAbsoluteIndex(row + 1, col) == '\0';

                bool isLeftEmpty = col == 0 || matr.GetByAbsoluteIndex(row, col - 1) == '\0';
                bool isRightEmpty = col == matr.Cols - 1 || matr.GetByAbsoluteIndex(row, col + 1) == '\0';

                // Пересечение допустимо, если сверху и снизу пусто, а слева и справа занято
                return !(isAboveEmpty && isBelowEmpty) && (isLeftEmpty && isRightEmpty);
            }
            else
            {
                // Проверяем, чтобы в перпендикулярных (горизонтальных) направлениях
                // пересечение было возможным, а параллельные (вертикальные) клетки были пустыми
                bool isLeftEmpty = col == 0 || matr.GetByAbsoluteIndex(row, col - 1) == '\0';
                bool isRightEmpty = col == matr.Cols - 1 || matr.GetByAbsoluteIndex(row, col + 1) == '\0';

                bool isAboveEmpty = row == 0 || matr.GetByAbsoluteIndex(row - 1, col) == '\0';
                bool isBelowEmpty = row == matr.Rows - 1 || matr.GetByAbsoluteIndex(row + 1, col) == '\0';

                // Пересечение допустимо, если слева и справа пусто, а сверху и снизу занято
                return !(isLeftEmpty && isRightEmpty) && (isAboveEmpty && isBelowEmpty);
            }
        }

        public bool isFreeToPlaceHoriz(int absRow, int absColumn, ref int charIndx, string word)
        {
            int indCopy = charIndx;
            bool isFree = true;
            bool extendedToLeft = false;
            bool extendedToRight = false;

            int startColumn = absColumn - charIndx;
            int endColumn = startColumn + word.Length; //- 1;
            int oldCol = matr.Cols;
            if (startColumn < 0)
            {
                matr.Resize(0, 0, -startColumn, 0);
                startColumn = 0;
                extendedToLeft = true;
                //charIndx = absColumn;
                charIndx = 0;
            }

            // Раньше здесь было matr.Cols, на заметку
            if (endColumn >= oldCol)
            {
                matr.Resize(0, 0, 0, endColumn - oldCol);
                extendedToRight = true;
            }

            for (int i = 0; i < word.Length && isFree; i++)
            {
                int checkCol = startColumn + i;
                char ch = matr.GetByAbsoluteIndex(absRow, checkCol);
                if ((ch != '\0' && ch != word[i])|| 
                    (ch == word[i] && !isPerpendicularCrossover(absRow, checkCol, true)))
                {
                    isFree = false;
                }
            }

            // Если вставка невозможна, возвращаем матрицу в исходное состояние
            if (!isFree && (extendedToLeft || extendedToRight))
            {
                if (extendedToLeft)
                {
                    //matr.Resize(0, 0, -(charIndx - absColumn), 0);
                }
                
                if (extendedToRight)
                {
                    //matr.Resize(0, 0, 0, -(absColumn + word.Length - charIndx - oldMatrCols + 1));
                }

                if (extendedToLeft || extendedToRight)
                {
                    matr.Fit();
                    charIndx = indCopy;
                }
            }

            return isFree;
        }

        public bool isFreeToPlaceVert(int absRow, int absColumn, ref int charIndx, string word)
        {
            int indCopy = charIndx;
            bool isFree = true;
            bool extendedToBottom = false;
            bool extendedToTop = false;
            
            int startRow = absRow - charIndx;
            int endRow = startRow + word.Length; //- 1;
            int oldRow = matr.Rows;
            if (startRow < 0)
            {
                matr.Resize(-startRow, 0, 0, 0);
                startRow = 0;
                extendedToTop = true;
                //charIndx = absRow;
                charIndx = 0;
            }

            // косяк в логике (был, при: endRow >= matr.Rows)
            if (endRow >= oldRow)
            {
                matr.Resize(0, endRow - oldRow, 0, 0);
                extendedToBottom = true;
            }

            for (int i = 0; i < word.Length && isFree; i++)
            {
                int checkRow = startRow + i;
                char ch = matr.GetByAbsoluteIndex(checkRow, absColumn);
                if ((ch != '\0' && ch != word[i]) || 
                    (ch == word[i] && !isPerpendicularCrossover(checkRow, absColumn, false)))
                {
                    isFree = false;
                }
            }

            if (!isFree && (extendedToBottom || extendedToTop))
            {
                if (extendedToBottom)
                {
                    //matr.Resize(0, -(absRow + word.Length - charIndx - oldMatrRows), 0, 0);
                }

                if (extendedToTop)
                {
                    //matr.Resize(-(charIndx - absRow), 0, 0, 0);
                }

                if (extendedToTop || extendedToBottom)
                {
                    matr.Fit();
                    charIndx = indCopy;
                }
            }

            return isFree;
        }

        // Для более "разреженного в плане расстановки решения, необходимо доп проверка условия матрицы на наличие
        // пустого элемента или границы матрицы слева/справа для вставки горизонтально и сверху/снизу для вставки
        // вертикально
        public void InsertWord(string word, bool isHoriz, int absRow, int absColumn)
        {
            // Вставка через абсолютные координаты
            if (isHoriz)
            {
                (int, int) RelCoordsVar;
                for (int i = 0; i < word.Length; i++)
                {
                    int checkColumn = absColumn + i; //- charIndx; аналогично
                    if (matr.GetByAbsoluteIndex(absRow, checkColumn) == word[i]) //Точно знаки одинаковые, вроде
                    {
                        RelCoordsVar = matr.AbsoluteToRelative(absRow, checkColumn);
                        wordCrossovers.Add((RelCoordsVar.Item1, RelCoordsVar.Item2));
                    }
                    else if (matr.GetByAbsoluteIndex(absRow, checkColumn) == '\0')
                    {
                        
                    }
                    else if (matr.GetByAbsoluteIndex(absRow, checkColumn) != word[i])
                    {
                        throw new Exception("Chars are not same");
                    }

                    matr.SetByAbsoluteIndex(absRow, checkColumn, word[i]);
                }

                RelCoordsVar = matr.AbsoluteToRelative(absRow, absColumn);
                listOfPlacedWords.Add(new PlacedWord(isHoriz, word, (RelCoordsVar.Item1, RelCoordsVar.Item2)));
            }
            else
            {
                (int, int) RelCoordsVar;
                for (int j = 0; j < word.Length; j++)
                {
                    int checkRow = absRow + j; //- charIndx;, нет смысла, т.к смещение уже посчитано
                    if (matr.GetByAbsoluteIndex(checkRow, absColumn) == word[j]) //Точно знаки одинаковые, вроде
                    {
                        RelCoordsVar = matr.AbsoluteToRelative(checkRow, absColumn);
                        wordCrossovers.Add((RelCoordsVar.Item1, RelCoordsVar.Item2));
                    }
                    else if (matr.GetByAbsoluteIndex(checkRow, absColumn) == '\0')
                    {
                        
                    }
                    else if (matr.GetByAbsoluteIndex(checkRow, absColumn) != word[j])
                    {
                        throw new Exception("Chars are not same");
                    }

                    matr.SetByAbsoluteIndex(checkRow, absColumn, word[j]);
                }

                RelCoordsVar = matr.AbsoluteToRelative(absRow, absColumn);
                listOfPlacedWords.Add(new PlacedWord(isHoriz, word, (RelCoordsVar.Item1, RelCoordsVar.Item2)));
            }
        }

        public bool RelativeWordInsert(string word)
        {
            bool isPlacedSuccessfully = false;
            bool isHoriz = false;
            (int, int) absCoords = (0,0);
            int index = 0;
            // Три цикла дла просмотра, куда можно засунуть слово.
            if (listOfPlacedWords.Count == 0)
            {
                isFreeToPlaceHoriz(0, 0, ref index, word);
                absCoords = (0, 0);
                isHoriz = true;
                isPlacedSuccessfully = true;
            }
            else
            {
                // Для увеличения связности, можно переписать так, чтобы сначала итерировались буквы слова для коорднат
                // а не наоборот
                for (int k = 0; k < word.Length && !isPlacedSuccessfully; k++)
                {
                    for (int i = 0; i < matr.Rows && !isPlacedSuccessfully; i++)
                    {
                        for (int j = 0; j < matr.Cols && !isPlacedSuccessfully; j++)
                        {
                            if (matr.GetByAbsoluteIndex(i, j) == word[k])
                            {
                                index = k;
                                if (isFreeToPlaceHoriz(i, j, ref index, word))
                                {
                                    //InsertWord(word, true, indx, i, j);
                                    isHoriz = true;
                                    isPlacedSuccessfully = true;
                                    absCoords = (i, j-index);
                                }
                                else if (isFreeToPlaceVert(i, j, ref index, word))
                                {
                                    //InsertWord(word, false, indx, i, j);
                                    isPlacedSuccessfully = true;
                                    absCoords = (i-index, j);
                                }
                            }
                        }
                    }
                }
            }

            if (isPlacedSuccessfully)
            {
                InsertWord(word, isHoriz,absCoords.Item1, absCoords.Item2);
                //listOfPlacedWords модифицируется в методе InsertWord
                //listOfUnplacedWords.Remove(word);
            }
            return isPlacedSuccessfully;
        }

        public bool RelativeWordDelete(string word)
        {
            bool isDeletedSucessfully = false;
            if (listOfPlacedWords.Count == 1)
            {
                RemovePlacedWordByName(word);
                matr = new ResizableMatrix(1, 1);
                isDeletedSucessfully = true;
            }
            else
            {
                foreach (PlacedWord wd in listOfPlacedWords)
                {
                    if (wd.word == word)
                    {
                        if (wd.isHorizDirection)
                        {
                            //(int, int) RelCoords = (wd.place.X, wd.place.Y);
                            (int, int) absCoords = matr.RelativeToAbsolute(wd.place.Item1, wd.place.Item2);
                            for (int i = 0; i < wd.word.Length; i++)
                            {
                                int checkColumn = absCoords.Item2 + i;
                                if (wordCrossovers.Contains((wd.place.Item1, wd.place.Item2 + i)))
                                {
                                    wordCrossovers.Remove((wd.place.Item1, wd.place.Item2 + i));
                                }
                                else
                                {
                                    matr.SetByAbsoluteIndex(absCoords.Item1, checkColumn, '\0');
                                }
                            }
                        }
                        else
                        {
                            (int, int) absCoords = matr.RelativeToAbsolute(wd.place.Item1, wd.place.Item2);
                            for (int i = 0; i < wd.word.Length; i++)
                            {
                                int checkRow = absCoords.Item1 + i;
                                if (wordCrossovers.Contains((wd.place.Item1 + i, wd.place.Item2)))
                                {
                                    wordCrossovers.Remove((wd.place.Item1 + i, wd.place.Item2));
                                }
                                else
                                {
                                    matr.SetByAbsoluteIndex(checkRow, absCoords.Item2, '\0');
                                }
                            }
                        }

                        isDeletedSucessfully = true;
                    }
                }
            }
            if (isDeletedSucessfully)
            {
                //
                matr.Fit();
                //RemovePlacedWordByName(word);
                //
                //listOfUnplacedWords.Add(word);
            }

            return isDeletedSucessfully;
        }

        public bool SolveCrissCross()
        {
            // Сортируем слова по длине в порядке убывания
            listOfUnplacedWords.Sort((a, b) => b.Length.CompareTo(a.Length));

            // Запускаем бэктрекинг
            List<string> usedWords = new List<string>();
            return BackTrackingSolutionAlg(usedWords, listOfUnplacedWords.Count);
        }

        public bool BackTrackingSolutionAlg(List<string> usedWords, int vol)
        {
            // Условие успешного завершения
            if (usedWords.Count == vol && 
                ((double)wordCrossovers.Count / (matr.Rows * matr.Cols) >= wordsDensityCoeff))
            {
                return true;
            }

            // Создаём копию списка, чтобы безопасно итерировать по словам
            var wordsToTry = new List<string>(listOfUnplacedWords);

            foreach (var word in wordsToTry)
            {
                if (usedWords.Contains(word)) continue; // Пропускаем уже использованные слова

                // Пытаемся вставить текущее слово
                if (RelativeWordInsert(word))
                {
                    Console.WriteLine($"Successfully placed: {word}");
                    usedWords.Add(word); // Отмечаем слово как использованное
                    listOfUnplacedWords.Remove(word);

                    // Рекурсивно продолжаем с обновлённым списком использованных слов
                    if (BackTrackingSolutionAlg(usedWords, vol))
                    {
                        return true;
                    }

                    // Если размещение не сработало, откатываем изменения
                    Console.WriteLine($"Backtracking, removing: {word}");
                    listOfUnplacedWords.Add(word);
                    RelativeWordDelete(word);
                    RemovePlacedWordByName(word);
                    usedWords.Remove(word);
                }
                else
                {
                    Console.WriteLine($"Failed to place: {word}");
                }
            }

            return false;
        }
        
        private static void ConvertWordsToLowercase(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].ToLowerInvariant();
                }

                File.WriteAllLines(filePath, lines);

                Console.WriteLine($"Все слова в файле \"{filePath}\" преобразованы в нижний регистр.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
            }
        }
        
        public void LoadWords(string wordFilePath)
        {
            ConvertWordsToLowercase(wordFilePath);
            try
            {
                string[] words = File.ReadAllLines(wordFilePath);

                listOfUnplacedWords.AddRange(words);
                Console.WriteLine(
                    $"Загружено {listOfUnplacedWords.Count} слов из файла: {wordFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке слов из файла: {ex.Message}");
            } 
        }
        
        public void LoadWordsWithSort(string wordFilePath)
        {
            try
            {
                string[] words = File.ReadAllLines(wordFilePath);

                listOfUnplacedWords.AddRange(words);

                listOfUnplacedWords.Sort((word1, word2) =>
                {
                    int lengthComparison = word1.Length.CompareTo(word2.Length);
                    if (lengthComparison != 0)
                    {
                        return lengthComparison;
                    }

                    return string.CompareOrdinal(word1, word2);
                });

                Console.WriteLine(
                    $"Загружено и отсортировано {listOfUnplacedWords.Count} слов из файла: {wordFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке слов из файла: {ex.Message}");
            }
        }
    }
}