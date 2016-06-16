namespace Events
{
    class DataFile
    {
        public string Extension { get; set; }

        public DataType Type
        {
            set {
                Extension = value == DataType.Video ? ".avi" : ".mp3";
            }
        }

        public int Duration { get; set; }
        public string Title { get; set; }
    }
}