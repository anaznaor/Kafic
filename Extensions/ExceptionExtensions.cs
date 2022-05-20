using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafic.Extensions
{

    public static class ExceptionExtensions
    {
       
        public static string CompleteExceptionMessage(this Exception exc)
        {
            StringBuilder sb = new StringBuilder();
            while (exc != null)
            {
                sb.AppendLine(exc.Message);
                exc = exc.InnerException;
            }
            return sb.ToString();
        }
    }
}