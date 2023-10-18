namespace PhotoSharing.Configuration
{
    public class AppConfig
    {
        public string FileDirectory { get; private set; }

        public AppConfig(string fileDirectory) {
            FileDirectory = fileDirectory;
        }
    }
}