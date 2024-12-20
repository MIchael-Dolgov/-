namespace Task25
{
    public class WordListWrapper : IComparable<WordListWrapper>
    {
        public List<string> Words { get; }

        public WordListWrapper(List<string> words)
        {
            Words = words;
        }

        public int CompareTo(WordListWrapper? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            // Сравниваем списки слов по длине их элементов
            for (int i = 0; i < Math.Min(Words.Count, other.Words.Count); i++)
            {
                int comparison = Words[i].Length.CompareTo(other.Words[i].Length);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            // Если все слова равны по длине, сравниваем по количеству слов
            return Words.Count.CompareTo(other.Words.Count);
        }

        public override string ToString()
        {
            return string.Join(" ", Words);
        }
    }
}
