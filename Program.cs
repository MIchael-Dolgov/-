using System.Text.RegularExpressions;

namespace Task20
{
    internal enum VarType
    {
        Int,
        Float,
        Double
    }

    internal class Variable
    {
        public VarType Type { get; }
        public string Value { get; }

        public Variable(VarType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => $"{Type} => {Value}";
    }

    class Program
    {
        static void Main()
        {
            string inputFilePath = "definitions.txt";
            string outputFilePath = "results.txt";

            //Создадим регулярку с группировкой регулярных выражений по нужным на м тегам
            string pattern = @"(?<type>[a-zA-Z_][a-zA-Z0-9_]*)\s+(?<name>[a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(?<value>\d+)\s*;";
            MyHashMap<string, Variable> myHashMap = new MyHashMap<string, Variable>();
            List<string> errors = new();

            try
            {
                string fileContent = File.ReadAllText(inputFilePath);
                //fileContent = Regex.Replace(fileContent, @"\s+", " ");

                //Избавимся от ненужных знаков отступов и новых строк
                fileContent = fileContent.Replace("\r", "").Replace("\n", " ");

                MatchCollection matches = Regex.Matches(fileContent, pattern);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        string typeStr = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string value = match.Groups["value"].Value;
                        VarType type;
                        
                        if (!Enum.TryParse(typeStr, true, out type))
                        {
                            errors.Add($"Некорректный тип: {typeStr} для переменной {name}.");
                            continue;
                        }

                        if (myHashMap.ContainsKey(name))
                        {
                            errors.Add($"Переопределение переменной: {name}. Значение {value} игнорируется. " +
                                       $"Оставляем первую определённую переменную");
                            continue;
                        }

                        myHashMap.Put(name, new Variable(type, value));
                    }

                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        foreach (KeyValuePair<string, Variable> entry in myHashMap.EntrySet())
                        {
                            string name = entry.Key;
                            Variable variable = entry.Value;
                            writer.WriteLine($"{variable.Type} => {name}({variable.Value})");
                        }

                        writer.WriteLine("\nОшибки:");
                        foreach (string error in errors)
                        {
                            writer.WriteLine(error);
                        }
                    }

                    Console.WriteLine($"Результаты сохранены в {outputFilePath}");
                }
                else
                {
                    Console.WriteLine("Определения переменных не найдены.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {inputFilePath} не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
