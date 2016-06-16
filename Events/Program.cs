using System;

namespace Events
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Main: create several data files, encode them, send them...");
            var videoFile = new DataFile {Title = "Hello", Duration = 20, Type = DataType.Video};
            var audioFile = new DataFile {Title = "MySong", Duration = 30, Type = DataType.Audio};

            var vencoder = new VideoEncoder(); //publisher
            var aencoder = new AudioEncoder();
            var mailService = new MailService(); //subscriber


            //encoder.VideoEncoded is a list of pointers to methods to call when event raised
            vencoder.VideoEncoded += mailService.OnVideoEncoded;
            aencoder.AudioEncoded += mailService.OnVideoEncoded;
            vencoder.EncodeData(videoFile);
            aencoder.EncodeData(audioFile);

            Console.ReadKey();
        }
    }

    internal class MailService
    {
        public void OnVideoEncoded(object sender, EventArgs args)
        {
            Console.WriteLine("MailService: Video has been encoded. Sending an email...");
        }
    }

    internal enum DataType
    {
        Audio,
        Video
    }
}