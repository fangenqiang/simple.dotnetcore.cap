using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        [Route("/Publish/PublishMessageWithoutTransacton")]
        public IActionResult PublishMessageWithoutTransacton()

        {
            _capPublisher.PublishAsync("simple.dotnetcore.cap.showtime", DateTime.Now, "callback-show-execute-time");
            return Ok();
        }

        [CapSubscribe("callback-show-execute-time")]   //对应发送的 callbackName
        public void ShowPublishTimeAndReturnExecuteTime(DateTime time)
        {
            Console.WriteLine(time);
        }
        /// <summary>
        /// 使用事务
        /// </summary>
        /// <returns></returns>
        [Route("/Publish/PublishMessageWithTransaction")]
        public IActionResult PublishMessageWithTransaction()
        {
            using (var connection = new SqlConnection(""))
            {   
                using (var transaction = connection.BeginTransaction(_capPublisher, autoCommit: true))
                {
                    //业务代码


                    _capPublisher.PublishAsync("simple.dotnetcore.cap.showtime", DateTime.Now, "callback-show-execute-time");

                    transaction.Commit();
                }
            }
            return Ok();
        }
    }
}