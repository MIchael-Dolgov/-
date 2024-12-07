using System.Text.RegularExpressions;

namespace Task19
{
    class Program
    {
        static void Main()
        {
            string inputFilePath = "Computer_science_Wikipedia.txt";
            string resultsFilePath = "results.txt";

            string tagPattern = @"</?[a-zA-Z][a-zA-Z0-9]*>";

            MyHashMap<string, int> myHashMap = new MyHashMap<string, int>();
            
            try
            {
                string[] lines = File.ReadAllLines(inputFilePath);

                Console.WriteLine("Извлечённые теги:");
                foreach (string line in lines)
                {
                    // Удаляем пробелы из строки
                    string trimmedLine = line.Replace(" ", "");

                    MatchCollection matches = Regex.Matches(trimmedLine, tagPattern);

                    foreach (Match match in matches)
                    {
                        string tag = match.Value;
                        Console.WriteLine(tag);
                        if (myHashMap.ContainsKey(tag))
                        {
                            myHashMap.Put(tag, myHashMap.Get(tag)+1);
                        }
                        else
                        { 
                            myHashMap.Put(tag, 1);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File: {inputFilePath} not found.");
            }
            /*
            catch (Exception ex)
            {
                Console.WriteLine($"Error has been occured: {ex.Message}");
            }
            */
            HashSet<KeyValuePair<string, int>> hashedSet = myHashMap.EntrySet();
            using (StreamWriter writer = new StreamWriter(resultsFilePath))
            {
                foreach (KeyValuePair<string, int> pair in hashedSet)
                {
                    //Console.WriteLine($"Тег: {pair.Key} Найден {pair.Value} раз.");
                    writer.WriteLine($"Тег: {pair.Key} Найден {pair.Value} раз.");
                }
            }
        }
    }
}