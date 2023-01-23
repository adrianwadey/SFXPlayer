using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AJW.General
{
    public class SVGResources
    {
        public static Bitmap FromSvgResource(string resourcename)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath;// = resourcename;
                                // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            resourcePath = assembly.GetManifestResourceNames().First(str => str.EndsWith(resourcename));

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return SvgDocument.FromSvg<SvgDocument>(reader.ReadToEnd()).Draw();
            }

        }
    }
}
