using System.Collections.Generic;

namespace CrissCross.Models
{
    public class PermutationGenerator
    {
        public static IEnumerable<List<string>> GeneratePermutations(List<string> list)
        {
            var stack = new Stack<(List<string> current, int index)>();
            stack.Push((new List<string>(list), 0));

            while (stack.Count > 0)
            {
                var (currentList, index) = stack.Pop();

                if (index == currentList.Count)
                {
                    yield return new List<string>(currentList);
                }
                else
                {
                    for (int i = index; i < currentList.Count; i++)
                    {
                        // Swap elements at index and i
                        var temp = currentList[index];
                        currentList[index] = currentList[i];
                        currentList[i] = temp;

                        stack.Push((new List<string>(currentList), index + 1));

                        // Swap back to restore the list to the original state
                        temp = currentList[index];
                        currentList[index] = currentList[i];
                        currentList[i] = temp;
                    }
                }
            }
        }
    }
}