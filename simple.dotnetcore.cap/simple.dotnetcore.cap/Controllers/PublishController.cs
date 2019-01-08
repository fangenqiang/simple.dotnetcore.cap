using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace simple.dotnetcore.cap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;
        public PublishController(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 不使用事务
        /// </summary>
        /// <returns></returns>
        [Route("/Publish/WithoutTransacton")]
        public IActionResult WithoutTransacton()
        {
            _capPublisher.PublishAsync("simple.dotnetcore.cap.showtime", DateTime.Now, "callback-show-execute-time");
            return Ok();
        }

        [CapSubscribe("callback-show-execute-time")]   //对应发送的 callbackName
        public void ShowPublishTimeAndReturnExecuteTime(DateTime time)
        {
            Console.WriteLine(time);
        }
    }
}