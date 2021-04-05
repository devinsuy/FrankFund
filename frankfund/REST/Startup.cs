using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace REST
{
    public class Startup
    {
        static void authenticateGCP()
        {
            // Devin's Credentials
            //string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";

            // Autumn's Credentials
            string pathToCreds = "/Users/steve/OneDrive/Documents/GitHub/FrankFund/Credentials/AuthAutumn.json";

            // Kenneth's Credentials
            //string pathToCreds = "/Users/015909177/Desktop/Github Repos/FrankFund/Credentials/AuthKenny.json";

            //Rachel's Credentials 
            //string pathToCreds = "C:/Data/Spring 2021/CECS 491B/Senior Project/Credentials/AuthRachel.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                authenticateGCP();
            }
            // Serve landing page for non API routing
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
