using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.AccessControl;
using Yofi_ASP_Net.DataBase;
using static System.Net.Mime.MediaTypeNames;

namespace Yofi_ASP_Net.Global
{
   
    public class AllFunctions
    {
        public static int GetLastInt(string path)
        {
            if (!Path.Exists(path)) Directory.CreateDirectory(path);
            List<int> intlist = new List<int>();
           var list = Directory.GetFiles(path).ToList();
            if(list.Count ==0)
            {
                return 1;
            }
            foreach(var item in list)
            {
               var i= item.Split('\\');
                intlist.Add(int.Parse(i.Last().Split('.').First()));
            }
            intlist.Sort();
          

            return intlist.Count;
        }
        public static List<string> DirectorySearch(string dir)
        {
            List<string> result = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    result.Add(Path.GetFullPath(f));
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    result.Add(Path.GetFullPath(d));
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }


        public static string ArrayContainsString(string[]Array,string text)
        {
            Console.WriteLine(JsonConvert.SerializeObject(Array));
            foreach (string op in Array)
            {
                if (op.Contains(text))
                {
                    return op;
                }
            }
            return null;

        }
        public static void Sleep(int Second)
        {
            Task.Delay(TimeSpan.FromSeconds(Second)).GetAwaiter().GetResult();
        }

     
    }
}
