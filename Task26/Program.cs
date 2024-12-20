using System.Text;
using Task25;

namespace Task26
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyHashSet<string> uniqueWords = new MyHashSet<string>();
            string path = "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task26/Task26/input.txt";

            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    string line;
                    while ((line = stream.ReadLine()!) != null)
                    {
                        // Извлекаем слова из строки
                        var words = ExtractWords(line);

                        // Добавляем слова в множество (MyHashSet)
                        foreach (var word in words)
                        {
                            uniqueWords.Add(word.ToLower()); // Приводим к нижнему регистру
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return;
            }

            // Преобразуем множество в массив и выводим результат
            string[] result = uniqueWords.ToArray();
            Console.WriteLine("Уникальные слова (с точностью до регистра):");
            foreach (var word in result)
            {
                Console.WriteLine(word);
            }
        }
        
        private static IEnumerable<string> ExtractWords(string line)
        {
            var words = new List<string>();
            var currentWord = new StringBuilder();

            foreach (char c in line)
            {
                if (char.IsLetter(c) && c <= 'z' && c >= 'A') // Проверяем, что символ - латинская буква
                {
                    currentWord.Append(c);
                }
                else if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }

            // Добавляем последнее слово, если строка на этом не закончилась
            if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
            }

            return words;
        }
    }
}