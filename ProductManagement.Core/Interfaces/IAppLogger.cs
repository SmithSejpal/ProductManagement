﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Core.Interfaces
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogError(Exception exception, string message, params object[] args);

        void LogTrace(string message, params object[] args);

        void LogDebug(string message, params object[] args);
    }
}
