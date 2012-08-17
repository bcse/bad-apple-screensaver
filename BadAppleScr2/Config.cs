using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BadAppleScr2
{
    [DataContract]
    public class Config
    {
        [DataMember(IsRequired = true)]
        public double Volume = 1d;

        [DataMember(IsRequired = true)]
        public double Chrominance = 0.75d;

        [DataMember(IsRequired = true)]
        public System.Windows.Media.Stretch Stretch = System.Windows.Media.Stretch.UniformToFill;

        [DataMember(IsRequired = true)]
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
                string json = File.ReadAllText(path, Encoding.Unicode);
                cfg = Serialize<Config>.FromJsonString(json);
            }
            catch
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
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            string json = Serialize<Config>.ToJsonString(this);
            File.WriteAllText(path, json, Encoding.Unicode);
        }
    }

    public class Serialize<T> where T : class
    {
        static DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        static MemoryStream stream;

        public static string ToJsonString(T graph)
        {
            stream = new MemoryStream();
            serializer.WriteObject(stream, graph);
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static T FromJsonString(string json)
        {
            stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return serializer.ReadObject(stream) as T;
        }
    }

}
