using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Jobs
{
    //  Base Job class with methods for tracking skipped, succeeded, and failed counts in Jobs
    //  Also includes methods for logging information
    //  Custom job classes can inherit this class and have all these methods available
    public abstract class BaseJob
    {
        protected List<string> Skipped = new List<string>();
        protected List<string> Succeeded = new List<string>();
        protected List<string> Failed = new List<string>();
        private int Total => Skipped.Count + Succeeded.Count + Failed.Count;
        protected ILogger _logger;

        protected virtual void LogInformation(string message)
        {
            if (_logger != null)
            {
                _logger.LogInformation(message);
            }
        }

        protected virtual void LogSuccess(string message)
        {
            Succeeded.Add(message);
            if (_logger != null)
            {
                _logger.LogInformation($"Success -- {message}");
            }
        }

        protected virtual void LogFailure(string message)
        {
            Failed.Add(message);
            if (_logger != null)
            {
                _logger.LogError($"Failure -- {message}");
            }
        }

        protected virtual void LogSkip(string message)
        {
            Skipped.Add(message);
            if (_logger != null)
            {
                _logger.LogInformation($"Skipped -- {message}");
            }
        }

        protected virtual void LogProgress()
        {
            if (_logger != null)
            {
                _logger.LogInformation($"Found : {Total}. Failed: {Failed.Count}. Skipped: {Skipped.Count}. Succeeded: {Succeeded.Count}");
            }
        }
    }
}
