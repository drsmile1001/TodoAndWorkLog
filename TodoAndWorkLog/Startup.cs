using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using TodoAndWorkLog.Entities;
using TodoAndWorkLog.Services;

namespace TodoAndWorkLog
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=TodoAndWorkLog.db"));
            services.AddScoped<AppService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext appDbContext)
        {
            appDbContext.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseWebSockets(new WebSocketOptions
            {
                ReceiveBufferSize = 4 * 1024
            });
            app.Use(async (context, next) =>
            {
                // 如果Request為WebSocket的
                if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/ws")
                {
                    // 容許WebSocket連線並取得WebSocket實例
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    while (webSocket.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult receivedData = null;
                        // 接收一次訊息中的所有段落
                        do
                        {
                            // 接收緩衝區
                            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4 * 1024]);

                            // 接收
                            receivedData = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                            // 回傳
                            await webSocket.SendAsync(
                                buffer.Take(receivedData.Count).ToArray(),
                                receivedData.MessageType,
                                receivedData.EndOfMessage,
                                CancellationToken.None);
                        } while (!receivedData.EndOfMessage); // 是否為最後一的段落
                    }
                }
                else
                {
                    await next();
                }

            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
