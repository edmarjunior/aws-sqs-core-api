
namespace ApiAcessoSQS.Dto
{
    public class PostMessageDto
    {
        public string QueueName { get; set; }
        public string MessageBody { get; set; }
    }
}
