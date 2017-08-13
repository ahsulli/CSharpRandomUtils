using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer
{
    internal class RandomValueConverter
    {
        internal string ConvertToString(int randomValue)
        {
            string randomValueText = randomValue.ToString();
            List<string> randomChars = Split(randomValueText, 4).ToList();
            List<int> randomCharUnicodes = randomChars.Select(unicodes => Convert.ToInt32(unicodes)).ToList();
            string output = string.Empty;
            foreach (int unicode in randomCharUnicodes)
            {
                output += Convert.ToChar(unicode);
            }
            return output;
        }

        internal char ConvertToChar(int randomValue)
        {
            if (randomValue < 0 || randomValue > 9999)
            {
                throw new ArgumentException("The random generated value cannot be converted into a single char.");
            }
            return Convert.ToChar(randomValue);
        }

        private IEnumerable<string> Split(string str, int characterCount)
        {
            while (str.Length > 0)
            {
                yield return new string(str.Take(characterCount).ToArray());
                str = new string(str.Skip(characterCount).ToArray());
            }
        }
    }
}
