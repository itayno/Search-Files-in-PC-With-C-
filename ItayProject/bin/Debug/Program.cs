using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchLibrary;
using System.IO;
using System.ComponentModel.DataAnnotations;


namespace ItayProject
{
    class Program
    {
        static void Main(string[] args)
        {
            char choice;
            string SearchWordFromUser;
            string SearchDirectoryFromUser;
            SearchClass newSearchFile = new SearchClass();
            newSearchFile.DisplayResults += new DisplaySearchResultsDelegate(printSearchResult);
            newSearchFile.DisplayError += new SearchFindErrorDelegate(PrintSearchErrors);
            SearchClass.SearchResult result = SearchClass.SearchResult.SuccessfulSearch;

            try
            {
                do
                {
                    Console.Write("1. Enter file name to search.\n" +
                                    "2. Enter file name to search + parent directory to search in.\n" +
                                    "3. Exit.");
                    Console.WriteLine(); ;
                    choice = Console.ReadKey().KeyChar;
                    Console.Clear();

                    switch (choice)
                    {
                        case '1':
                            Console.WriteLine("Insert filename");
                            SearchWordFromUser = Console.ReadLine();
                            Console.Clear();
                            result = newSearchFile.AddNewSearch(SearchWordFromUser);
                            Console.WriteLine("Start searching... \n-------------------------------------");
                            Console.WriteLine(result.ToString());
                            break;
                            //IMPORTENT:The results are being saved and the developer can see that in the SQL Server Object explorer
                        case '2':
                            Console.WriteLine("Insert directory to search in");
                            SearchDirectoryFromUser = Console.ReadLine();
                            Console.WriteLine("Insert file name");
                            SearchWordFromUser = Console.ReadLine();
                            Console.Clear();
                            result = newSearchFile.AddNewSearch(SearchWordFromUser, SearchDirectoryFromUser);
                            Console.WriteLine("Start searching... \n-------------------------------------");
                            Console.WriteLine(result.ToString());
                            break;
                        default:
                            Console.Clear();
                            break;

                    }

                } while (choice != '3');
            }
            catch (Exception ex)
            {
               

                Console.WriteLine("An error has occured: " + ex.Message);
                

            }
        }
        static void printSearchResult(FileInfo[] filesAndDirs)
        {
            if (filesAndDirs.Length > 0)
                foreach (FileInfo foundFile in filesAndDirs)
                {
                    Console.WriteLine(foundFile.FullName);
                }
        }
        static void PrintSearchErrors(Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }
}
