using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reservations.Api.Middlewares
{
    public class CustomErrorMiddleware
    {
        private readonly RequestDelegate next;

        public CustomErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            try
            {
                var request = context.Request;
                var body = new StreamReader(request.Body);
                var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                var logEntry = "";
                if (body != null && body.BaseStream != null && body.BaseStream.CanSeek)
                {
                    body.BaseStream.Seek(0, SeekOrigin.Begin);
                    var requestBody = body.ReadToEnd();
                    if (requestBody != null)
                        Log.Error(exception, "{ErrorID} {Message} {RequestBody} and LogEntry: {LogEntry}", errorId, exception.Message, requestBody, logEntry);
                    else
                        Log.Error(exception, "{ErrorID} {Message} and LogEntry: {LogEntry}", errorId, exception.Message, logEntry);
                }
                else
                {
                    Log.Error(exception, "{ErrorID} {Message} and LogEntry: {LogEntry}", errorId, exception.Message, logEntry);
                }

                var result = JsonConvert.SerializeObject(new
                {
                    error = "An error occurred in our API.  Please refer to the error id with our support team.",
                    id = errorId
                });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception thrown inside HandleExceptionAsync()");
            }
        }
    }
}
