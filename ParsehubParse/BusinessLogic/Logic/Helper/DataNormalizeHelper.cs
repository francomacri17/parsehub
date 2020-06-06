using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ParsehubParse.BusinessLogic.Logic.Helper
{
    public class DataNormalizeHelper
    {
        public static string Decompress(string s)
        {
            var decodedStringInBytes = Convert.TryFromBase64String(s);
            return Encoding.ASCII.GetBytes(decodedStringInBytes);
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }
    }
}
