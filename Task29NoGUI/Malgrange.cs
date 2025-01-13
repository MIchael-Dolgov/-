namespace Task29NoGUI
{
    public static class Malgrange
    {
        // v and u are orgraph vertexes
        static void DFSInversed(List<List<int>> graph, bool[] used, int v)
        {
            used[v] = true;
            for (int u = 0; u < graph.Count; u++)
                foreach (int w in graph[u])
                    if (w == v && !used[u])
                        DFSInversed(graph, used, u);
        }

        static void DFS(List<List<int>> graph, bool[] used, int v)
        {
            used[v] = true;
            foreach (int u in graph[v])
                if (!used[u])
                    DFS(graph, used, u);
        }

        static bool[] TransitiveClosure(List<List<int>> graph, int u)
        {
            bool[] used = new bool[graph.Count];
            DFS(graph, used, u);
            return used;
        }

        static bool[] ReversedTransitiveClosure(List<List<int>> graph, int u)
        {
            bool[] used = new bool[graph.Count];
            DFSInversed(graph, used, u);
            return used;
        }

        // Определяет компоненту сильной связности, в которую входит эта вершина
        static void FindComponent(bool[] directTransitive, bool[] inverseTransitive, bool[] used,
            ref List<int> component)
        {
            for (int u = 0; u < used.Length; u++)
            {
                if (!used[u])
                {
                    used[u] = directTransitive[u] && inverseTransitive[u];
                    if (used[u])
                        component.Add(u);
                }
            }
        }

        static void MalgrangeAlg(List<List<int>> graph, List<List<int>> components)
        {
            // Игнорируем вершины, принадлежащие найденным компонентам
            bool[] used = new bool[graph.Count];
            for (int u = 0; u < graph.Count; u++)
            {
                if (!used[u])
                {
                    List<int> component = new List<int>();
                    // Получение прямого транзитивного замыкания
                    bool[] directTransitive = TransitiveClosure(graph, u);
                    // Получение обратного транзитивного замыкания
                    bool[] inverseTransitive = ReversedTransitiveClosure(graph, u);
                    // Нахождение компоненты сильной связности
                    FindComponent(directTransitive, inverseTransitive, used, ref component);
                    components.Add(component);
                }
            }
        }

        public static void Solve(string pathToFileWithOrGraph)
        {
            List<List<int>> graph = new List<List<int>>();
            using (StreamReader reader = new StreamReader(pathToFileWithOrGraph))
            {
                int n = int.Parse(reader.ReadLine()!); // Количество вершин
                graph = Enumerable.Range(0, n).Select(_ => new List<int>()).ToList();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!.Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split();
                    if (parts.Length != 2)
                        throw new FormatException($"Invalid edge format: '{line}'");

                    int u = int.Parse(parts[0]) - 1;
                    int v = int.Parse(parts[1]) - 1;
                    graph[u].Add(v);
                }
            }

            // Поиск компонент сильной связности
            List<List<int>> components = new List<List<int>>();
            MalgrangeAlg(graph, components);

            // Вывод компонент
            for (int i = 0; i < components.Count; i++)
            {
                Console.Write("Component:");
                foreach (int v in components[i])
                    Console.Write(" " + (v + 1));
                Console.WriteLine();
            }
        }
    }
}