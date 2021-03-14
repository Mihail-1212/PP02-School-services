using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_02.Utils
{
    public enum Sort
    {
        asc,
        desc
    }

    public static class Constants
    {
        public static readonly String ServiceImgFolder = Directory.GetCurrentDirectory() + "\\services_photos";
        public static readonly String ServiceAdditiveImgFolder = Directory.GetCurrentDirectory() + "\\additive_services_photos";
        public static String Service404Img = "hqdefault.jpg";
    }
}
