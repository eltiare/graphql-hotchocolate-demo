using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotChocolateServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .Create()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.Use(async (ctx, next) =>
            {
                var headers = ctx.Response.Headers;
                headers.Add("Access-Control-Allow-Origin", "*");
                headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
                headers.Add("Access-Control-Allow-Headers", "Content-Type,token,Accept,Accept-Encoding,Accept-Language,Content-Length,Cookie,Host,Origin,Referer,Sec-Fetch-Dest,Sec-Fetch-Mode,User-Agent");
                headers.Add("Access-Control-Allow-Credentials", "true");
                if (ctx.Request.Method.ToLowerInvariant() != "options")
                    await next();
            });
            app.UseGraphiQL();
            app.UseGraphQL();
        }
    }
}
