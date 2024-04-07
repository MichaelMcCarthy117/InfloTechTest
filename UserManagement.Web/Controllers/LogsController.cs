using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace UserManagement.WebMS.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(bool userLogs = false)
        {
            ViewBag.PageTitle = userLogs ? "User Logs" : "System Logs";

            string logFilePath = "log.txt";

            if (System.IO.File.Exists(logFilePath))
            {
                try
                {
                    List<LogEntry> logs = new List<LogEntry>();

                    using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            string? logEntry;
                            while ((logEntry = streamReader.ReadLine()) != null)
                            {
                                // Parse log entry string to extract properties
                                LogEntry entry = ParseLogEntry(logEntry);
                                logs.Add(entry);
                            }
                        }
                    }

                    ViewBag.UserLogs = userLogs; // Pass the flag to the view
                    return View("Logs", logs);
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "Error while reading logs from the file.");
                    return View("Error");
                }
            }
            else
            {
                return View("NoLogs");
            }
        }

        // Method to parse log entry string and extract properties
        private LogEntry ParseLogEntry(string logEntry)
        {
            string[] parts = logEntry.Split(',');
            if (parts.Length >= 8)
            {
                return new LogEntry
                {
                    Time = parts[0],
                    ID = parts[1],
                    FirstName = parts[2],
                    LastName = parts[3],
                    DateOfBirth = parts[4],
                    Email = parts[5],
                    Active = parts[6].ToLower() == "true",
                    Action = parts[7]
                };
            }
            else
            {
                // Handle invalid log entry format
                throw new ArgumentException("Invalid log entry format: " + logEntry);
            }
        }
    }

    // Model representing a log entry
    public class LogEntry
    {
        public string? Time { get; set; }
        public string? ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; }
        public string? Action { get; set; }
    }
}
