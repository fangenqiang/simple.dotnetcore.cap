using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using simple.dotnetcore.cap.Services;

namespace simple.dotnetcore.cap
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCap(options =>
            {
                //订阅者所属的默认消费者组	
                options.DefaultGroup = "simple.dotnetcore.cap";
                //成功的消息被删除的过期时间/秒
                options.SucceedMessageExpiredAfter = 3600;
                //失败最大重试次数	
                options.FailedRetryCount = 50;
                //失败重试间隔时间	/ 秒
                options.FailedRetryInterval = 60;
                //执行失败消息时的回调函数
                options.FailedThresholdCallback = null;
                //配置RabbitMQ
                options.UseRabbitMQ(rabbitMqOptions =>
                {
                    //宿主地址
                    rabbitMqOptions.HostName = "localhost";
                    //用户名	
                    rabbitMqOptions.UserName = "guest";
                    //密码
                    rabbitMqOptions.Password = "guest";
                    //虚拟主机	
                    rabbitMqOptions.VirtualHost = "/";
                    //端口号	
                    rabbitMqOptions.Port = -1;
                    //CAP默认Exchange名称	
                    rabbitMqOptions.ExchangeName = "cap.default.topic";
                    //RabbitMQ连接超时时间	
                    rabbitMqOptions.RequestedConnectionTimeout = 30000;
                    //RabbitMQ消息读取超时时间	
                    rabbitMqOptions.SocketReadTimeout = 30000;
                    //RabbitMQ消息写入超时时间	
                    rabbitMqOptions.SocketWriteTimeout = 30000;
                    //队列中消息自动删除时间 (10天) 毫秒	
                    rabbitMqOptions.QueueMessageExpires = 864000000;
                });
                    //配置SQLServer
                options.UseSqlServer(sqlServerOptions =>
                {
                    //数据库连接字符串	
                    sqlServerOptions.ConnectionString = Configuration.GetSection("ConnectionStrings")["Default"];
                    //Cap表架构	
                    sqlServerOptions.Schema = "Cap";
                });
                //启用面板
                options.UseDashboard();
            });
            services.AddTransient<ICapSubscribe, SubscriberService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
