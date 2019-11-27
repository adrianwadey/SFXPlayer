using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SFXPlayer {
    internal static class Extensions {
        public static string SerializeToXmlString<T>(this T value) {
            if (value == null) {
                return string.Empty;
            }
            try {
                var xmlserializer = new XmlSerializer(typeof(T));
                
                using (var sw = new StringWriter()) {
                    xmlserializer.Serialize(sw, value);
                    return sw.ToString();
                }
            } catch (Exception ex) {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
