using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace SdbBackbone.Compression
{
    public class CompressionUtility
    {
        public static string CompressValue(string input)
        {
            var bf = new BinaryFormatter();

            var ms = new MemoryStream();

            bf.Serialize(ms, input);

            byte[] inbyt = ms.ToArray();

            var objStream = new MemoryStream();

            var objZS = new DeflateStream(objStream, CompressionMode.Compress);

            objZS.Write(inbyt, 0, inbyt.Length);

            objZS.Flush();

            objZS.Close();

            byte[] b = objStream.ToArray();

            return Convert.ToBase64String(b);
        }

        public static string DecompressValue(string input)
        {
            byte[] bytCook = Convert.FromBase64String(input);

            var inMs = new MemoryStream(bytCook);

            inMs.Seek(0, 0);

            var zipStream = new DeflateStream(inMs,
                                              CompressionMode.Decompress, true);

            byte[] outByt = ReadFullStream(zipStream);

            zipStream.Flush();

            zipStream.Close();

            var outMs = new MemoryStream(outByt);

            outMs.Seek(0, 0);

            var bf = new BinaryFormatter();

            object retval = bf.Deserialize(outMs, null);

            string returnValue = retval.ToString();

            return returnValue;
        }

        private static byte[] ReadFullStream(Stream stream)
        {
            var buffer = new byte[32768];

            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)

                        return ms.ToArray();

                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}