using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace BadAppleScr2
{
    [DataContract]
    public class Config
    {
        [DataMember]
        public double Volume = 1d;

        [DataMember]
        public double Chrominance = 0d;

        [DataMember]
        public System.Windows.Media.Stretch Stretch = System.Windows.Media.Stretch.UniformToFill;

        [DataMember]
        public System.Uri Video = new System.Uri(@"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv");

        static string file_path = null;

        private Config()
        {
        }

        public static Config Open(string path)
        {
            Config cfg;
            file_path = path;

            try
            {
                using (Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(File.ReadAllText(path))))
                {
                    stream.Position = 0;
                    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                    DataContractSerializer ser = new DataContractSerializer(typeof(Config));
                    cfg = (Config)ser.ReadObject(reader, true);
                    reader.Close();
                }
            }
            catch (SerializationException)
            {
                cfg = new Config();
            }
            catch (FileNotFoundException)
            {
                cfg = new Config();
            }

            return cfg;
        }

        public void Save()
        {
            Save(file_path);
        }

        public void Save(string path)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (Stream writer = new FileStream(path, FileMode.Create))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(Config));
                    ser.WriteObject(writer, this);
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}
