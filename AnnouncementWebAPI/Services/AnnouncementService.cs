using AnnouncementWebAPI.Models;

namespace AnnouncementWebAPI.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        public async Task<string> FindFirstRepeatedWordAsync(Announcement announcement)
        {
            // Implement the logic to find the first repeated word
            // using the properties of the announcement object
            string[] stopWords = { "a", "an", "the", "to", "is", "are", "in", "of", "that" }; // список службових слів

            string[] titleWords = announcement.Title.Split(' '); // розбиваємо заголовок на окремі слова
            string[] descriptionWords = announcement.Description.Split(' '); // розбиваємо опис на окремі слова

            // об'єднуємо слова з заголовка та опису
            string[] allWords = titleWords.Concat(descriptionWords).ToArray();

            foreach (string word in allWords)
            {
                string lowercaseWord = word.ToLower(); // переводимо слово до нижнього регістру для порівняння

                // перевіряємо, чи слово не є службовим словом
                if (!stopWords.Contains(lowercaseWord))
                {
                    // перевіряємо, чи слово повторюється в заголовку та описі
                    if (await CountOccurrencesAsync(allWords, lowercaseWord) > 1)
                    {
                        return word; // повертаємо перше повторюване слово, яке не є службовим словом
                    }
                }
            }

            return string.Empty; // якщо не знайдено жодного відповідного слова
        }

        private async Task<int> CountOccurrencesAsync(string[] words, string target)
        {
            int count = 0;
            await Task.Run(() =>
            {
                foreach (string word in words)
                {
                    if (string.Equals(word, target, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                    }
                }
            });
            return count;
        }
    }
}
