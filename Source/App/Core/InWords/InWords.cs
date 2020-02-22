using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.InWords
{
    public static class InWords
    {
        public static string ConvertToInWords(this double number)
        {
            string isNegative = "";

                string numberString = Convert.ToDouble(number).ToString();

                if (numberString.Contains("-"))
                {
                    isNegative = "Minus ";
                    numberString = numberString.Substring(1, numberString.Length - 1);
                }
                if (numberString == "0")
                {
                    return "Zero Only";
                }
                else
                {
                    return isNegative + ConvertToInWords(numberString);
                }
            
        }

        public static string ConvertToInWords(this float number)
        {
            string isNegative = "";

            string numberString = Convert.ToDouble(number).ToString();

            if (numberString.Contains("-"))
            {
                isNegative = "Minus ";
                numberString = numberString.Substring(1, numberString.Length - 1);
            }
            if (numberString == "0")
            {
                return "Zero Only";
            }
            else
            {
                return isNegative + ConvertToInWords(numberString);
            }

        }


        public static string ConvertToInWords(this decimal number)
        {
            string isNegative = "";

            string numberString = Convert.ToDouble(number).ToString();

            if (numberString.Contains("-"))
            {
                isNegative = "Minus ";
                numberString = numberString.Substring(1, numberString.Length - 1);
            }
            if (numberString == "0")
            {
                return "Zero Only";
            }
            else
            {
                return isNegative + ConvertToInWords(numberString);
            }

        }

        public static string ConvertToInWords(this int number)
        {
            string isNegative = "";

            string numberString = Convert.ToDouble(number).ToString();

            if (numberString.Contains("-"))
            {
                isNegative = "Minus ";
                numberString = numberString.Substring(1, numberString.Length - 1);
            }
            if (numberString == "0")
            {
                return "Zero Only";
            }
            else
            {
                return isNegative + ConvertToInWords(numberString);
            }

        }

        private static String ConvertWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = number.StartsWith("0");

                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = Ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = Tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (number.Substring(0, pos) != "0" && number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(number.Substring(0, pos)) + place + ConvertWholeNumber(number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(number.Substring(0, pos)) + ConvertWholeNumber(number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch
            {
                // ignored
            }

            return word.Trim();
        }

        private static String ConvertToInWords(String numb)
        {
            String val = "", wholeNo = numb;
            String andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    var points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = $"{ConvertWholeNumber(wholeNo).Trim()} {andStr}{pointStr} {endStr}";
            }
            catch
            {
                // ignored
            }

            return val;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = Ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private static String Ones(String number)
        {
            int _number = Convert.ToInt32(number);
            String name = "";
            switch (_number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static String Tens(String number)
        {
            int _number = Convert.ToInt32(number);
            String name = null;
            switch (_number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_number > 0)
                    {
                        name = Tens(number.Substring(0, 1) + "0") + " " + Ones(number.Substring(1));
                    }
                    break;
            }
            return name;
        }
    }
}
