using System.ComponentModel.DataAnnotations;

namespace StajyerTakip.Validation
{
    /// <summary>
    /// TC Kimlik doğrulaması:
    /// - 11 hane, ilk hane 0 olamaz
    /// - ((d1+d3+d5+d7+d9)*7 - (d2+d4+d6+d8)) % 10 == d10
    /// - (d1..d10 toplamı) % 10 == d11
    /// </summary>
    public class TcKimlikNoAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null) return true; 
            var s = value.ToString();
            if (string.IsNullOrWhiteSpace(s)) return true;

            if (s.Length != 11) return false;
            if (!ulong.TryParse(s, out _)) return false;
            if (s[0] == '0') return false;

            int[] d = new int[11];
            for (int i = 0; i < 11; i++) d[i] = s[i] - '0';

            int oddSum  = d[0] + d[2] + d[4] + d[6] + d[8];
            int evenSum = d[1] + d[3] + d[5] + d[7];
            int d10calc = ((oddSum * 7) - evenSum) % 10;
            if (d10calc < 0) d10calc += 10;

            if (d10calc != d[9]) return false;

            int sumFirst10 = 0;
            for (int i = 0; i < 10; i++) sumFirst10 += d[i];

            int d11calc = sumFirst10 % 10;
            return d11calc == d[10];
        }
    }
}
