using Microsoft.Extensions.Logging;
using System.IO;

namespace UserManagement.WebMS.Controllers
{
    public class UserLogsController : Controller
    {
        private readonly ILogger<UserLogsController> _logger;

        public UserLogsController(ILogger<UserLogsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(long userId)
        {
            string logFilePath = "log.txt";

            if (System.IO.File.Exists(logFilePath))
            {
                try
                {
                    List<LogEntry> userLogs = new List<LogEntry>();

                    using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            string? logEntry;
                            while ((logEntry = streamReader.ReadLine()) != null)
                            {
                                if (logEntry.Contains($",{userId},"))
                                {
                                    LogEntry entry = ParseLogEntry(logEntry);
                                    userLogs.Add(entry);
                                }
                            }
                        }
                    }

                    ViewBag.PageTitle = "User Logs";
                    ViewBag.UserLogs = true;
                    return View("~/Views/Logs/Logs.cshtml", userLogs);
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

        // Helper method to parse log entry string and create LogEntry model instance
        private LogEntry ParseLogEntry(string logEntry)
        {
            // Split log entry string and create LogEntry model instance
            string[] parts = logEntry.Split(',');
            return new LogEntry
            {
                Time = parts[0],
                ID = long.Parse(parts[1]).ToString(),
                FirstName = parts[2],
                LastName = parts[3],
                DateOfBirth = parts[4],
                Email = parts[5],
                Active = bool.Parse(parts[6]),
                Action = parts[7]
            };
        }
    }
}
