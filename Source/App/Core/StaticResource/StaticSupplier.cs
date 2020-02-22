using System.Collections.Generic;

namespace Project.Core.StaticResource
{
    public class StaticSupplier
    {
        public const string Monalisa = "Monalisa";
        public const string Sanita = "Sanita";
        public const string Euro = "Euro";
        public const string Rak = "RAK";
        public const string Mirpur = "Mirpur";
        public const string Dbl = "DBL";

        public static List<string> GetSuppliers()
        {
            return new List<string>
            {
                Monalisa,
                Sanita,
                Euro,
                Rak,
                Mirpur,
                Dbl
            };
        }
    }
}