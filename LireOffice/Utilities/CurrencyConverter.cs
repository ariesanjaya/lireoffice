using System;

namespace LireOffice.Utilities
{
    public static class CurrencyConverter
    {
        public static string ConvertNumberToString(long n)
        {
            if (n < 0)
                return "";
            if (n == 0)
                return "Nol";
            if (n < 10)
                return ConvertDigitToString(n);
            if (n < 20)
                return ConvertTeensToString(n);
            if (n < 100)
                return ConvertHighTensToString(n);
            if (n < 1000)
                return ConvertBigNumberToString(n, (long)1e2, "ratus");
            if (n < 1000000)
                return ConvertBigNumberToString(n, (long)1e3, "ribu");
            if (n < 1000000000)
                return ConvertBigNumberToString(n, (long)1e6, "juta");
            if (n < 1000000000000)
                return ConvertBigNumberToString(n, (long)1e9, "milyar");

            return ConvertBigNumberToString(n, (long)1e12, "triliun");
        }

        private static string ConvertDigitToString(long n)
        {
            switch (n)
            {
                case 0: return "";
                case 1: return "satu";
                case 2: return "dua";
                case 3: return "tiga";
                case 4: return "empat";
                case 5: return "lima";
                case 6: return "enam";
                case 7: return "tujuh";
                case 8: return "delapan";
                case 9: return "sembilan";
                default: return "";
            }
        }

        // number between 10 and 19
        private static string ConvertTeensToString(long n)
        {
            switch (n)
            {
                case 10: return "sepuluh";
                case 11: return "sebelas";
                case 12: return "dua belas";
                case 13: return "tiga belas";
                case 14: return "empat belas";
                case 15: return "lima belas";
                case 16: return "enam belas";
                case 17: return "tujuh belas";
                case 18: return "delapan belas";
                case 19: return "sembilan belas";
                default: return "";
            }
        }

        // number between 20 and 99
        private static string ConvertHighTensToString(long n)
        {
            long tensDigit = (long)Math.Floor((decimal)n / 10);

            string tensStr;
            switch (tensDigit)
            {
                case 2: tensStr = "dua puluh"; break;
                case 3: tensStr = "tiga puluh"; break;
                case 4: tensStr = "empat puluh"; break;
                case 5: tensStr = "lima puluh"; break;
                case 6: tensStr = "enam puluh"; break;
                case 7: tensStr = "tujuh puluh"; break;
                case 8: tensStr = "delapan puluh"; break;
                case 9: tensStr = "sembilan puluh"; break;
                default: throw new IndexOutOfRangeException(string.Format("{0} not in range 20-99", n));
            }
            if (n % 10 == 0) return tensStr;
            string oneStr = ConvertDigitToString(n - tensDigit * 10);
            return tensStr + " " + oneStr;
        }

        private static string ConvertBigNumberToString(long n, long baseNum, string baseNumStr)
        {
            // special case: use commas to separate portions of the number, unless we are in the hundreds
            string separator = (baseNumStr != "ratus") ? ", " : " ";

            // Strategy: translate the first portion of the number, then recursively translate the remaining sections.
            // Step 1: strip off first portion, and convert it to string:
            long bigPart = (long)Math.Floor((decimal)n / baseNum);
            string bigPartStr = ConvertNumberToString(bigPart) + " " + baseNumStr;
            if (string.Equals(bigPartStr.ToLower(), "satu ratus")) bigPartStr = "seratus";
            if (string.Equals(bigPartStr.ToLower(), "satu ribu")) bigPartStr = "seribu";
            // Step 2: check to see whether we're done:
            if (n % baseNum == 0) return bigPartStr;
            // Step 3: concatenate 1st part of string with recursively generated remainder:
            long restOfNumber = n - bigPart * baseNum;
            return bigPartStr + separator + ConvertNumberToString(restOfNumber);
        }
    }
}