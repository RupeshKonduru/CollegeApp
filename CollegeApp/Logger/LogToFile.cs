namespace CollegeApp.Logger
{
    public class LogToFile : IMyLogger
    {
        public void Log(string message)
        {
           Console.WriteLine(message);
        }
    }
}
