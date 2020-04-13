using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;


namespace SearchLibrary
{
    public delegate void DisplaySearchResultsDelegate(FileInfo[] filesAndDirs);
    public delegate void SearchFindErrorDelegate(Exception ErrorMessage);
    public class SearchClass
    {
        //Search results status
        public enum SearchResult
        {
            SuccessfulSearch,
            DirNotFound,
            SearchError
        }
        //Every UI can implement search result display as it wish
        public event DisplaySearchResultsDelegate DisplayResults;
        public event SearchFindErrorDelegate DisplayError;

        //Creating the DB Connection
        TheSearchInPCEntities1 dbConection = new TheSearchInPCEntities1();
        //Declare the search function
        public SearchResult AddNewSearch(string searchWordFromUser)
        {
            return AddNewSearch(searchWordFromUser, @"C:\");
        }

        public SearchResult AddNewSearch(string searchWordFromUser, string fil)
        {
            SearchResult result = SearchResult.SuccessfulSearch;
            try
            {
                SearchMethod newSearch = new SearchMethod();
              
                newSearch.RootFile = fil;
                newSearch.SearchText = searchWordFromUser;
                dbConection.SearchMethods.Add(newSearch);
                dbConection.SaveChanges();
                result = SearchFileIn(newSearch.SearchText, newSearch.RootFile);
            }
            catch (Exception ex)
            {
                result = SearchResult.SearchError;
                
                DisplayError(ex);
            }
            return result;

        }
        
        //Declare the directory of searching
        public SearchResult SearchFileIn(string filename)
        {
            SearchResult result = SearchResult.SuccessfulSearch;
            SearchFileIn(filename, @"C:\");
            return result;
        }

        public SearchResult SearchFileIn(string filename, string directoryPath)
        {
            SearchResult result = SearchResult.SuccessfulSearch;
            SearchAnswer newSearchAnswer = new SearchAnswer();
            newSearchAnswer.SearchName = filename.ToLower();
            FileInfo[] filesAndDirs = null;
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var dirInfo = new DirectoryInfo(directoryPath);
                   
                    {
                        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(directoryPath);
                        //filesAndDirs = hdDirectoryInWhichToSearch.GetFiles("*" + SearchName.ToLower() + "*.*", SearchOption.AllDirectories);
                        filesAndDirs = hdDirectoryInWhichToSearch.GetFiles("*" + filename.ToLower() + "*.*");
                        DisplayResults(filesAndDirs);
                        //get all subdirs then operate SearchFileIn on each subdir
                        var dirs = Directory.GetDirectories(directoryPath);
                        foreach (var d in dirs)
                        {
                            SearchFileIn(filename, d); // recursive call
                        }
                    }
                }
                else result = SearchResult.DirNotFound;

                if (filesAndDirs != null)
                    foreach (FileInfo foundFile in filesAndDirs)
                    {
                        string fullName = foundFile.FullName;
                        newSearchAnswer.SearchResult = fullName;
                        //dbConection.SearchAnswers.Add(newSearchAnswer);
                    }
                //dbConection.SaveChanges();
            }
            catch (Exception ex)
            {
                result = SearchResult.SearchError;
                //Console.Write(ex.ToString());
                DisplayError(ex);
            }
            return result;
        }



    }
}
