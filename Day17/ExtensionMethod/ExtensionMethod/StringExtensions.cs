

using System;
using System.Collections.Generic;
using System.Text;

namespace RetailOrderConsole
{


    public static class StringExtensions
    {

        public static String Clean(this string msg)
        {
            return msg.Trim();
        }

        public static string UpperCase(this string msg)
        {
            return msg.ToUpper();
        }

        public static string AddPrefix(this string msg, string prefix)
        {
            return $"{prefix} " + msg;
        }

        public static string AddSuffix(this string msg, string suffix)
        {
            return $"{msg} " + $" - {suffix}";

        }


        public static int Add(this int a, int b)
        {
            return a + b;
        }

        public static int mul(this int a, int b)
        {
            return a * b;
        }
    }

        public class LearnExtensionMethods
        {
            public void Run()
            {

                string s1 = "Sakthivel";
                Console.WriteLine(s1);
                string s2 = s1.Clean().UpperCase().AddPrefix("Mr.");
                Console.WriteLine(s2);

                string s3 = s1.Clean().
                                UpperCase().
                                AddPrefix("Mr.").
                                AddSuffix("Tech Lead");
                Console.WriteLine(s3);

                string s4 = "   Hello World   ";
                s4.Trim(); // s4.trim(s4);


                int x = 10;
                x.Add(20); // x.Add(x, 20);
                Console.WriteLine(x.mul(40));

            }
        }

    }


