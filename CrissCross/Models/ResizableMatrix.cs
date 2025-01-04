using System;

namespace CrissCross.Models
{
    public class ResizableMatrix
    {
        public bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < Rows && col >= 0 && col < Cols;
        }
        
        public char[,] _matrix;
        //private int _baseRow; // Референсная строка
        //private int _baseCol; // Референсный столбец
        private int _baseRow;
        private int _baseCol;
        private bool _baseInitialized; // Указывает, была ли установлена базовая точка

        public ResizableMatrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Matrix dimensions must be positive.");
            _matrix = new char[rows, cols];
            //_baseInitialized = false;
            //
            _baseInitialized = true;
            _baseRow = 0;
            _baseCol = 0;
        }

        public int Rows => _matrix.GetLength(0);
        public int Cols => _matrix.GetLength(1);

        // Получение значения по абсолютным координатам
        public char GetByAbsoluteIndex(int row, int col)
        {
            ValidateAbsoluteIndex(row, col);
            return _matrix[row, col];
        }

        // Установка значения по абсолютным координатам
        public void SetByAbsoluteIndex(int row, int col, char value)
        {
            ValidateAbsoluteIndex(row, col);
            _matrix[row, col] = value;

            if (!_baseInitialized)
            {
                _baseRow = row;
                _baseCol = col;
                _baseInitialized = true;
            }
        }

        // Получение значения по относительным координатам
        public char GetByRelativeIndex(int rowOffset, int colOffset)
        {
            var (absoluteRow, absoluteCol) = RelativeToAbsolute(rowOffset, colOffset);
            return GetByAbsoluteIndex(absoluteRow, absoluteCol);
        }

        // Установка значения по относительным координатам
        public void SetByRelativeIndex(int rowOffset, int colOffset, char value)
        {
            var (absoluteRow, absoluteCol) = RelativeToAbsolute(rowOffset, colOffset);
            SetByAbsoluteIndex(absoluteRow, absoluteCol, value);
        }

        // Преобразование абсолютных координат в относительные
        public (int rowOffset, int colOffset) AbsoluteToRelative(int row, int col)
        {
            ValidateAbsoluteIndex(row, col);
            if (!_baseInitialized)
                throw new InvalidOperationException("Base coordinates are not set.");
            return (row - _baseRow, col - _baseCol);
        }

        // Преобразование относительных координат в абсолютные
        public (int row, int col) RelativeToAbsolute(int rowOffset, int colOffset)
        {
            if (!_baseInitialized)
                throw new InvalidOperationException("Base coordinates are not set.");
            int absoluteRow = _baseRow + rowOffset;
            int absoluteCol = _baseCol + colOffset;
            ValidateAbsoluteIndex(absoluteRow, absoluteCol);
            return (absoluteRow, absoluteCol);
        }

        // Изменение размера матрицы
        public void Resize(int rowsToAddTop, int rowsToAddBottom, int colsToAddLeft, int colsToAddRight)
        {
            int newRows = Rows + rowsToAddTop + rowsToAddBottom;
            int newCols = Cols + colsToAddLeft + colsToAddRight;

            if (newRows <= 0 || newCols <= 0)
                throw new ArgumentException("Matrix dimensions must be positive.");

            // Ограничение на уменьшение больше текущего размера
            if (rowsToAddTop < -Rows || rowsToAddBottom < -Rows || colsToAddLeft < -Cols || colsToAddRight < -Cols)
                throw new ArgumentException("Resize values cannot exceed current dimensions.");

            var newMatrix = new char[newRows, newCols];

            // Определение границ копирования
            int startRow = Math.Max(0, rowsToAddTop);
            int startCol = Math.Max(0, colsToAddLeft);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    // Убедиться, что старый индекс находится в пределах новой матрицы
                    if (i + startRow < newRows && j + startCol < newCols)
                    {
                        newMatrix[i + startRow, j + startCol] = _matrix[i, j];
                    }
                }
            }

            // Обновление матрицы
            _matrix = newMatrix;

            // Обновление базовых координат
            if (_baseInitialized)
            {
                _baseRow += rowsToAddTop;
                _baseCol += colsToAddLeft;

                _baseRow = Math.Max(0, _baseRow);
                _baseCol = Math.Max(0, _baseCol);
            }
        }
        
        /*
        public void Resize(int rowsToAddTop, int rowsToAddBottom, int colsToAddLeft, int colsToAddRight)
        {
            int newRows = Rows + rowsToAddTop + rowsToAddBottom;
            int newCols = Cols + colsToAddLeft + colsToAddRight;

            if (newRows <= 0 || newCols <= 0)
                throw new ArgumentException("Matrix dimensions must be positive.");

            // Ограничение на уменьшение больше текущего размера
            if (rowsToAddTop < -Rows || rowsToAddBottom < -Rows || colsToAddLeft < -Cols || colsToAddRight < -Cols)
                throw new ArgumentException("Resize values cannot exceed current dimensions.");

            var newMatrix = new char[newRows, newCols];

            // Определение границ копирования
            int startRow = Math.Max(0, rowsToAddTop);
            int startCol = Math.Max(0, colsToAddLeft);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    // Убедиться, что старый индекс находится в пределах новой матрицы
                    if (i + startRow < newRows && j + startCol < newCols)
                    {
                        newMatrix[i + startRow, j + startCol] = _matrix[i, j];
                    }
                }
            }

            // Обновление матрицы
            _matrix = newMatrix;

            // Обновление базовых координат
            if (_baseInitialized)
            {
                _baseRow += rowsToAddTop;
                _baseCol += colsToAddLeft;

                _baseRow = Math.Max(0, _baseRow);
                _baseCol = Math.Max(0, _baseCol);
            }
        }
        */

        // Уменьшение матрицы до содержимого
        public void Fit()
        {
            int top = 0, bottom = Rows - 1, left = 0, right = Cols - 1;

            // Найти границы
            while (top <= bottom && IsRowEmpty(top)) top++;
            while (bottom >= top && IsRowEmpty(bottom)) bottom--;
            while (left <= right && IsColEmpty(left)) left++;
            while (right >= left && IsColEmpty(right)) right--;

            if (top > bottom || left > right)
            {
                _matrix = new char[1, 1];
                //_baseInitialized = false;
                _baseInitialized = true;
                _baseRow = 0;
                _baseCol = 0;
                return;
            }

            int newRows = bottom - top + 1;
            int newCols = right - left + 1;

            var newMatrix = new char[newRows, newCols];

            for (int i = 0; i < newRows; i++)
            for (int j = 0; j < newCols; j++)
                newMatrix[i, j] = _matrix[top + i, left + j];

            _matrix = newMatrix;

            if (_baseInitialized)
            {
                _baseRow -= top;
                _baseCol -= left;
                _baseRow = Math.Max(0, _baseRow);
                _baseCol = Math.Max(0, _baseCol);
            }
        }

        // Проверка, пуста ли строка
        private bool IsRowEmpty(int row)
        {
            for (int col = 0; col < Cols; col++)
                if (_matrix[row, col] != '\0') return false;
            return true;
        }

        // Проверка, пуст ли столбец
        private bool IsColEmpty(int col)
        {
            for (int row = 0; row < Rows; row++)
                if (_matrix[row, col] != '\0') return false;
            return true;
        }

        // Валидация координат
        private void ValidateAbsoluteIndex(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                throw new ArgumentOutOfRangeException("Indices are out of bounds.");
        }
    }
}