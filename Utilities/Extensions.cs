namespace BinanceLockedStakingAlert
{
    static class Extensions
    {
        // https://stackoverflow.com/questions/47988989/get-string-between-two-words-using-c-sharp-regex
        public static string GetBetween(this string text, string firstWord, string secondWord)
        {
            var firstWordIndex = text.IndexOf(firstWord) + firstWord.Length;
            var secondWordIndex = text.IndexOf(secondWord);

            return text.Substring(firstWordIndex, secondWordIndex - firstWordIndex);
        }

        public static string GetBefore(this string text, string word, int offset = 0)
        {
            var wordIndex = text.IndexOf(word);
            var result = text.Substring(0, wordIndex + word.Length - offset);

            return result;
        }

        public static string GetAfter(this string text, string word, int offset = 0)
        {
            var wordIndex = text.IndexOf(word);
            var result = text.Substring(wordIndex + word.Length + offset);

            return result;
        }
    }
}