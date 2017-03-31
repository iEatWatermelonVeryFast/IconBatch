using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace IconGenerator
{
    class GenerateSetting
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public ImageFormat Type { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}",
                Width, Height, Type.ToString(), Path, FileName);
        }
        public static GenerateSetting parse(string config) {
            string[] split = config.Split('|');
            if (split.Length != 5) {
                throw new Exception("cannot read this config");
            }
            return new GenerateSetting() {
                Width=Convert.ToInt32(split[0]),
                Height=Convert.ToInt32(split[1]),
                Type=getType(split[2]),
                Path=split[3],
                FileName=split[4]
            };
        }
        public string getFileName() {
            string filename = this.FileName 
                + "." + this.Type.ToString().ToLower();
            if (string.IsNullOrEmpty(Path))
            {
                return filename;
            }
            else {
                return Path + System.IO.Path.DirectorySeparatorChar + filename;
            }
        }
        private static ImageFormat getType(string type)
        {
            foreach (var format in getTypes()) {
                if (format.ToString().Equals(type)) {
                    return format;
                }
            }
            return ImageFormat.Png;//by default
        }
        private static ImageFormat[] imageFormats;
        public static ImageFormat[] getTypes() {
            if (imageFormats == null)
            {
                imageFormats = new ImageFormat[] {
                ImageFormat.Bmp,
                ImageFormat.Jpeg,
                ImageFormat.Icon,
                ImageFormat.Png
            };
            }
            return imageFormats;
        }
    }
}
