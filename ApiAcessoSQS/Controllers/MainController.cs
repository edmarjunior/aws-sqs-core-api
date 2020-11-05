using Amazon;
using Amazon.SQS;
using Microsoft.AspNetCore.Mvc;

namespace ApiAcessoSQS.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        protected readonly IAmazonSQS _sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
    }
}
