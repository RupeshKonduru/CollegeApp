﻿namespace CollegeApp.Logger
{
    public class LogToServer : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
