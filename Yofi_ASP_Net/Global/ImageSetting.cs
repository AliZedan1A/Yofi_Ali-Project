using System.IO;

namespace Yofi_ASP_Net.Global
{
    public class ImageSetting
    {
        public MemoryStream Image { get; set; }
        public string Format { get; set; }
        public System.Drawing.Image GetImage()
        {
            return System.Drawing.Image.FromStream(Image);
            
        }
    }
}
