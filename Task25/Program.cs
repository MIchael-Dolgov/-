namespace Task25
{
    internal class Program
    {
        static void Main()
        {
            MyHashSet<WordListWrapper> set = new MyHashSet<WordListWrapper>();
            string path =
                "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task25/Task25/input.txt";

            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    string line;
                    while ((line = stream.ReadLine()!) != null)
                    {
                        // Разделяем строку на слова, удаляем пробелы и сортируем слова по длине
                        var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(word => word.Trim())
                            .OrderBy(word => word.Length)
                            .ToList();

                        // Добавляем список слов (обёрнутый в WordListWrapper) в множество
                        set.Add(new WordListWrapper(words));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return;
            }

            WordListWrapper[] sortedLines = set.ToArray();

            Array.Sort(sortedLines);

            // Вывод отсортированных списков слов
            foreach (var wrapper in sortedLines)
            {
                Console.WriteLine(wrapper.ToString());
                Console.WriteLine();
            }
        }
    }
}