using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.AccessControl;
using Yofi_ASP_Net.DataBase;
using static System.Net.Mime.MediaTypeNames;

namespace Yofi_ASP_Net.Global
{
    public class EmbarkationResponse
    {
        public bool IsDone { get; set; }
        public object Msg { get; set; }

    }
    public class Image
    {
        public enum Things
        {
            User,
            Products
        }
        public int id { get; set; }
        private static string MainPath { get; set; }
        private static Things Flagthing { get; set; }
        private bool embarkation = false;
        private static string[] OpnedPaths { get; set; }
        public Image(string path, Things FLag, int ID)
        {
            id = ID;
            MainPath = path;
            Flagthing = FLag;
        }
        public EmbarkationResponse RemoveImage(int number)
        {
            if (!embarkation) return new EmbarkationResponse() { IsDone = false, Msg = "the image class not embarkation" };
            if (Flagthing is not Things.Products) return new EmbarkationResponse() { IsDone = false, Msg = "you cant insert image the flag not  product" };
            var mainpath = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString()) + "\\Images";
            var detectedpath = AllFunctions.ArrayContainsString(Directory.GetFiles(mainpath), number.ToString()+".jpeg");
            File.Delete(detectedpath);
            return new EmbarkationResponse() { IsDone = true, Msg = "done remove image" };

        }
        public EmbarkationResponse RemoveImage()
        {
            if (!embarkation) return new EmbarkationResponse() { IsDone = false, Msg = "the image class not embarkation" };
            if (Flagthing == Things.User)
            {
                var imagepath = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString());
                File.Delete(imagepath);
                return new EmbarkationResponse() { IsDone = true, Msg = "done remove image" };

            }
            else
            {
                var imagepath = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString());
                if(imagepath is null)
                {
                    return new EmbarkationResponse() { IsDone = false, Msg = "Product Not Found in Images" };
                }
                var z= Directory.GetFiles(imagepath, "*",searchOption:SearchOption.AllDirectories);
                Console.WriteLine(z.Length);
                foreach(var f in z)
                {
                    Console.WriteLine(f);
                    if (f.Contains('.'))
                    {
                        File.Delete(f);
                    }
                   
                }
                if(Path.Exists(imagepath))
                {
                   var g= Directory.GetDirectories(imagepath, "*");
                    foreach (var f in g)
                    {
                        Directory.Delete(f);
                    }
                        Directory.Delete(imagepath);

                }
                return new EmbarkationResponse() { IsDone = true, Msg = "done remove image" };
            }

        }
        public EmbarkationResponse InsertImage(System.Drawing.Image Image, bool MainImage)
        {
            if (!embarkation) return new EmbarkationResponse() { IsDone = false, Msg = "the image class not embarkation" };
            if (Flagthing is not Things.Products) return new EmbarkationResponse() { IsDone = false, Msg = "you cant insert image the flag not  product" };

            if (!MainImage)
            {
                return InsertImage(Image);
            }
            else
            {
                
                var SearchDirectory = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString());
                if (SearchDirectory is null)
                {
                   
                    var pathforImages = OpnedPaths[0].Replace("1",id.ToString());
                    Console.WriteLine(pathforImages);

                    Directory.CreateDirectory(@$"{pathforImages}");
                    Image.Save($@"{pathforImages}\MainImage.jpeg", ImageFormat.Jpeg);
                    return new EmbarkationResponse() { IsDone = true, Msg = "done add image" };
                }
                File.Delete($@"{SearchDirectory}\MainImage.jpeg");
                Image.Save($@"{SearchDirectory}\MainImage.jpeg", ImageFormat.Jpeg);
                return new EmbarkationResponse() { IsDone = true, Msg = "done update Image" };

            }

        }
        public EmbarkationResponse InsertImage(System.Drawing.Image Image)
        {
            if (!embarkation) return new EmbarkationResponse() { IsDone = false, Msg = "the image class not embarkation" };
            if (Flagthing == Things.User)
            {
                var nulbl = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString());
                var savedpath = OpnedPaths[0].Split('\\').ToList();
                savedpath.Remove(savedpath.Last());
                var path = savedpath.ToString();
                if (nulbl != null)
                {

                    Directory.Delete(nulbl);
                    Image.Save(@$"{path}\{id}.jpeg", ImageFormat.Jpeg);
                    return new EmbarkationResponse() { IsDone = true, Msg = "done update Image" };


                }
                else
                {
                    Image.Save(@$"{path}\{id}.jpeg", ImageFormat.Jpeg);
                    return new EmbarkationResponse() { IsDone = true, Msg = "done add image" };

                }
            }
            else
            {
                var SearchDirectory = AllFunctions.ArrayContainsString(OpnedPaths, id.ToString());
                if (SearchDirectory == null)
                {
                    var pathforImages = OpnedPaths[0].Replace("1", id.ToString());
                    Directory.CreateDirectory(@$"{pathforImages}");
                    pathforImages +=  "\\images";
                    Directory.CreateDirectory(pathforImages);
                    Image.Save(@$"{pathforImages}\{AllFunctions.GetLastInt(@$"{pathforImages}")}.jpeg", ImageFormat.Jpeg);
                    return new EmbarkationResponse() { IsDone = true, Msg = "done add Image and creat folder" };

                }
                else
                {
                    Image.Save(@$"{SearchDirectory}\Images\{AllFunctions.GetLastInt(@$"{SearchDirectory}\Images")}.jpeg", ImageFormat.Jpeg);
                    return new EmbarkationResponse() { IsDone = true, Msg = "done add Image" };
                }

            }
        }
        public EmbarkationResponse Embarkation()
        {
            if(!Path.Exists(MainPath))
            {
                return  new EmbarkationResponse() { IsDone = false, Msg = $"{MainPath} not exists" };
            }
            if (Flagthing == Things.User)
            {
                var z = AllFunctions.ArrayContainsString(Directory.GetDirectories(MainPath), "UsersImages");
                if (z is not null)
                {
                    OpnedPaths = Directory.GetFiles(z);
                    embarkation = true;
                    return new EmbarkationResponse() { IsDone = true, Msg = "Done" };

                }
                else
                {
                    return new EmbarkationResponse() { IsDone = false, Msg = "The Path Not Exists" };
                }


            }
            else
            {
                var z = AllFunctions.ArrayContainsString(Directory.GetDirectories(MainPath), "ProductsImages");
                if (z is not null)
                {
                    OpnedPaths = AllFunctions.DirectorySearch(z).ToArray();
                    embarkation = true;
                    return new EmbarkationResponse() { IsDone = true, Msg = "Done" };

                }
                else
                {
                    return new EmbarkationResponse() { IsDone = false, Msg = "The Path Not Exists" };
                }


            }
        }
    }
}