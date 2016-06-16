using System;
using System.Threading;

namespace Events
{
    #region VideoEncoder
    internal class VideoEncoder : IDataEncoder
    {
        //1 - Define delegate
        //2 - Define an event based on delegate
        //3 - Raise the event
        public delegate void VideoEncodedEventHandler(object source, EventArgs args);

        public event VideoEncodedEventHandler VideoEncoded;

        public void EncodeData(DataFile p_data)
        {
            Console.WriteLine("VideoEncoder: Start encoding " + p_data.Title + p_data.Extension);

            var job = new Thread(() =>
            {
                Thread.Sleep(2000);
                VideoEncodingCompleted();
            });
            job.Start();
        }

        protected virtual void VideoEncodingCompleted()
        {
            if (VideoEncoded != null)
                VideoEncoded(this, EventArgs.Empty);
        }
    } 
    #endregion

    #region AudioEncoder
    internal class AudioEncoder : IDataEncoder
    {
        //1 - Define delegate
        //2 - Define an event based on delegate
        //3 - Raise the event
        public delegate void AudioEncodedEventHandler(object source, EventArgs args);

        public event AudioEncodedEventHandler AudioEncoded;

        public void EncodeData(DataFile p_data)
        {
            var job = new Thread(() =>
            {
                Console.WriteLine("AudioEncoder: Start encoding " + p_data.Title + p_data.Extension);
                Thread.Sleep(3000);
                AudioEncodingCompleted();
            });
            job.Start();
        }

        protected virtual void AudioEncodingCompleted()
        {
            if (AudioEncoded != null)
            {
                AudioEncoded(this, EventArgs.Empty);
            }
        }
    } 
    #endregion
}