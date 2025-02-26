namespace CollegeApp.Logger
{
    public class LogToDB : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
