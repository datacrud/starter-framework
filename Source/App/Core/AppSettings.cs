using System;
using System.Configuration;
using System.Web;

namespace Project.Core
{
    public class AppSettings
    {
        public static string PoweredBy = ConfigurationManager.AppSettings["App:PoweredBy"];
        public static string ApplicationName = ConfigurationManager.AppSettings["App:Name"];


        public class Emailing
        {
            
        }



        public class Sms
        {
            
        }
    }
}