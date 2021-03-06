﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cimpress.Extensions.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task LogAndThrowIfNotSuccessStatusCode(this HttpResponseMessage message, ILogger logger)
        {
            if (!message.IsSuccessStatusCode)
            {
                var formattedMsg = await LogMessage(message, logger);
                throw new Exception(formattedMsg);
            }
        }

        public static async Task LogIfNotSuccessStatusCode(this HttpResponseMessage message, ILogger logger)
        {
            if (!message.IsSuccessStatusCode)
            {
                await LogMessage(message, logger);
            }
        }

        public static async Task<string> LogMessage(HttpResponseMessage message, ILogger logger)
        {
            var msg = await message.Content.ReadAsStringAsync();
            var formattedMsg = $"Error processing request. Status code was {message.StatusCode} when calling '{message.RequestMessage.RequestUri}', message was '{msg}'";
            logger.LogError(formattedMsg);
            return formattedMsg;
        }
    }
}
