using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KeyHandler
{
    class Tab
    {
        private static string last_part = string.Empty;
        private static string[] path_parts = new string[0];
        private static string[] directories = new string[0];
        private static List<string> list_of_suggestions = new List<string>();

        public Tab(string CMD)
        {
            Reset();
            KeyPress(CMD);
        }

        public string Directories
        {
            get { return string.Join("\\", directories); }
        }

        public string[] List_of_Suggestions
        {
            get { return list_of_suggestions.ToArray(); }
        }

        private static void Reset()
        {
            last_part = string.Empty;
            path_parts = new string[0];
            directories = new string[0];
            list_of_suggestions = new List<string>();
        }

        private static void KeyPress(string command)
        {
            path_parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            last_part = path_parts.LastOrDefault();
            if (Collection.String.IsEmpty(last_part)) return;

            directories = last_part.Split(Path.DirectorySeparatorChar);

            string base_directory = Directory.GetCurrentDirectory();
            string[] Entries = new string[0];

            list_of_suggestions.Add(string.Join("\\", directories));
            if (directories.Length == 1)
            {
                Entries = Directory.GetFileSystemEntries(".", "*");
                foreach (string Entry in Entries)
                {
                    if (Entry.Substring(2).ToLower().StartsWith(directories.FirstOrDefault().ToLower()))
                        list_of_suggestions.Add(Entry.Substring(2));
                }
            }

            else
            {
                string path = string.Empty;
                for (int i = 0; i < directories.Length - 1; i++)
                    path = Path.Combine(path, directories[i]);

                if (!File.Exists(path)) return;

                Entries = Directory.GetFileSystemEntries(".", Path.Combine(path, "*"));
                foreach (string Entry in Entries)
                {
                    string[] subpath = Entry.Substring(2).Split(Path.DirectorySeparatorChar);
                    string folder_to_search = subpath.LastOrDefault().ToLower();

                    if (folder_to_search.StartsWith(directories.LastOrDefault().ToLower()))
                        list_of_suggestions.Add(Entry.Substring(2));
                }
            }
        }
    }
}
