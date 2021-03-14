using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_02.Utils
{
    public class ImageManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldImage"></param>
        /// <returns>Название файла</returns>
        public static String LoadImage(String oldImage, String folderImage)
        {
            if (File.Exists(oldImage))
            {
                String newFileName = Guid.NewGuid() + "." + Path.GetExtension(oldImage);
                File.Copy(oldImage, $"{folderImage}\\{newFileName}");
                return newFileName;
            }
            return null;
        }
    }
}
