using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThreadingDemoProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome To The Parallel Tasks Program");
            //Retreiving Goncharavos Oblomov from gutenberg.org
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParrallelTasks
            //Perform Three Tasks in pararllel on the source array
            Parallel.Invoke(() => {
                                Console.WriteLine("Begin First Task......");
                                GetLongestWords(words);
                            }, //close first action
                            () => {
                                Console.WriteLine("Begin Second Task......");
                                GetMostCommonWords(words);
                            }, //close second action
                            () => {
                                Console.WriteLine("Begin Third Task.......");
                                GetCountOfWords(words, "sleep");
                            }//close third action
            );//close parallel invoke
            #endregion
        }

        //Method to get the longest words
        private static void GetLongestWords(string[] words)
        {
            var longestWord = (from word in words
                                orderby word.Length descending
                                select word).First();
            Console.WriteLine($"Task 1 : The Longest Words is {longestWord}");
            Console.ReadLine();
        }

        //Method to get the most common words
        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonWords = frequencyOrder.Take(10);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 : The Most Common Words Are : ");
            foreach (var word in commonWords)
            {
                sb.AppendLine(" "+word);
            }
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }

        //Method to get the cound of given word
        private static void GetCountOfWords(string[] words, string term)
        {
            var findWord = from word in words
                          where word.ToUpper().Contains(term.ToUpper())
                          select word;
            Console.WriteLine($@"Task 3 : The Word {term} occurs {findWord.Count()} times.");
            Console.ReadLine();
        }

        //An http request performed synchronously for simplicity
        public static string[] CreateWordArray(string uri)
        {
            Console.WriteLine($"Retreiving from {uri}");
            //Download the web page easy way
            string blog = new WebClient().DownloadString(uri);
            //separating string into an array of words, removing some common punctuation
            return blog.Split(new char[] { ' ', '\u000A', ',', '.', ':', '_', '-', '/'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
