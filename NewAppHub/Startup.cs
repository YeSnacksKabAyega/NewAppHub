using AppHub;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Cors;
using System;
using System.Configuration;
using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
namespace AngularAspNetCoreSignalR
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200");
            }));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
             //   map.UseCors(CorsOptions.AllowAll);

                if (ConfigurationManager.AppSettings.AllKeys.Contains("TransportConnectTimeout"))
                    GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["TransportConnectTimeout"]));

                if (ConfigurationManager.AppSettings.AllKeys.Contains("ConnectionTimeout"))
                    GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionTimeout"]));

                GlobalHost.Configuration.KeepAlive = null;
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    EnableJSONP = true,
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true

                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
            //    map.RunSignalR(hubConfiguration);
            });
        }
    }
}
