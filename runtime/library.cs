using Core;
using System.Reflection;

namespace Runtime
{
    public class Utils
    {

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        public static string GetFileVersion(string fileName)
        {
            return Assembly.LoadFile(fileName).GetName().Version.ToString();
        }
    }

    public class LibraryInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public LibraryInfo()
        {
            Name = "";
            Version = "";
        }

        public LibraryInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }

    public class LibraryManager
    {
        private List<LibraryInfo> Libs;

        public LibraryManager()
        {
            Libs = new List<LibraryInfo>();
        }

        public void Loaded()
        {
            Libs = this.GetFilesModules(Directory.GetCurrentDirectory());
        }


        public bool IsComparisonDependens(List<LibraryInfo> list)
        {
            bool result = true;

            foreach (var item in Libs)
            {
                var item2 = list.Find(x => x.Name == item.Name);
                if (item2 != null)
                {

                    if (item.Version != item2.Version)
                    {
                        result = false;
                        Terminal.ErrorWrite(string.Format("Error dependency: {0} (main:{1} <> modulue:{2})", item.Name, item.Version, item2.Version));
                    }
                }
            }

            return result;
        }

        public List<LibraryInfo> GetFilesModules(string path)
        {
            string[] files = Directory.GetFiles(path);
            List<LibraryInfo> list = new List<LibraryInfo>();

            foreach (var file in files)
            {
                if (file.Contains(".dll"))
                {
                    var ver = Utils.GetFileVersion(file);
                    var name = Path.GetFileName(file);

                    list.Add(new LibraryInfo(name, ver));
                }
            }

            return list;
        }
    }
}
