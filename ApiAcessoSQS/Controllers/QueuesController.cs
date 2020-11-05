using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiAcessoSQS.Controllers
{
    [Route("api/[controller]")]
    public class QueuesController : MainController
    {
        [HttpPost]
        public IActionResult Post([FromBody] string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                return BadRequest("Missing parameter [queueName]");

            // Maximum message size of 256 KiB (1,024 bytes * 256 KiB = 262,144 bytes).
            var maxMessage = 256 * 1024;

            var response = _sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = queueName,
                Attributes = new Dictionary<string, string>
                {
                    { QueueAttributeName.DelaySeconds, TimeSpan.FromSeconds(5).TotalSeconds.ToString() },
                    { QueueAttributeName.MaximumMessageSize, maxMessage.ToString() },
                    { QueueAttributeName.MessageRetentionPeriod, TimeSpan.FromDays(4).TotalSeconds.ToString() },
                    { QueueAttributeName.ReceiveMessageWaitTimeSeconds, TimeSpan.FromSeconds(5).TotalSeconds.ToString() },
                    { QueueAttributeName.VisibilityTimeout, TimeSpan.FromMinutes(5).TotalSeconds.ToString() }
                }
            }).Result;

            return Ok(response);
        }
    }
}
