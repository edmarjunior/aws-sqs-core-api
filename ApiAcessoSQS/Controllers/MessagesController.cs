using Amazon.SQS.Model;
using ApiAcessoSQS.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiAcessoSQS.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : MainController
    {
        [HttpGet("{queueName}")]
        public IActionResult Get(string queueName)
        {
            /* buscando a url da fila */
            var queueUrl = GetQueueUrl(queueName);

            if (string.IsNullOrEmpty(queueUrl))
                return BadRequest("Queue not found");

            /* buscando mensagens na fila */
            var messages = _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest(queueUrl)).Result.Messages;

            return Ok(messages);
        }

        [HttpPost]
        public IActionResult Post(PostMessageDto message)
        {
            /* buscando a url da fila */
            var queueUrl = GetQueueUrl(message.QueueName);

            if (string.IsNullOrEmpty(queueUrl))
                return BadRequest("Queue not found");

            /* enviando uma mensagem para a fila */
            var response = _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = message.MessageBody
            });

            return Ok(response.Result);
        }

        [HttpDelete("{queueName}")]
        public IActionResult Delete(string queueName)
        {
            /* buscando a url da fila */
            var queueUrl = GetQueueUrl(queueName);

            if (string.IsNullOrEmpty(queueUrl))
                return BadRequest("Queue not found");

            /* buscando uma mensagem na fila */
            var message = _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest(queueUrl)).Result.Messages.FirstOrDefault();

            if (message == null)
                return BadRequest("No message in the queue");

            /* removendo a mensagem da fila */
            var result =_sqsClient.DeleteMessageAsync(new DeleteMessageRequest(queueUrl, message.ReceiptHandle)).Result;

            return Ok(result);
        }

        private string GetQueueUrl(string queueName)
        {
            try
            {
                return _sqsClient.GetQueueUrlAsync(queueName).Result.QueueUrl;
            } catch
            {
                return string.Empty;
            }
        }
    }
}
