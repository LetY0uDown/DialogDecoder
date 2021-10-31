using System;
using System.IO;
using System.Text;

namespace DialogDecoder
{
    class Program
    {
        static void Main()
        {
            string path = @"dialog\dialog.tlk";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc = Encoding.GetEncoding(0x4e3);

            byte[] dialogBytes = File.ReadAllBytes(path);

            using(BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                reader.ReadBytes(12);
                int linesAmount = reader.ReadInt32();
                int offsetToThirdPart = reader.ReadInt32();
                int currentIndex = 48;

                for (int i = 0; i < linesAmount; i++)
                {
                    reader.BaseStream.Seek(currentIndex, SeekOrigin.Begin);
                    int lineOffset = reader.ReadInt32();
                    int lineLength = reader.ReadInt32();
                    reader.ReadBytes(4);

                    currentIndex += 40;
                    reader.BaseStream.Seek(offsetToThirdPart + lineOffset, SeekOrigin.Begin);
                    Console.WriteLine(enc.GetString(reader.ReadBytes(lineLength)));
                }
            }
        }
    }
}
