using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace simple.dotnetcore.cap.Services
{
    /// <summary>
    /// 订阅服务
    /// </summary>
    public class SubscriberService: ICapSubscribe
    {
        [CapSubscribe("simple.dotnetcore.cap.showtime")]
        public DateTime ShowPublishTimeAndReturnExecuteTime(DateTime time)
        {
            Console.WriteLine(time); // 这是发送的时间
            return time;
        }
    }
}
