using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using ClientAPI.Commands;
using ClientAPI.Repository;
using ClientAPI.Repository.IRepository;
using ClientAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Messaging;
using Messaging.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ClientAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add DBContext classes
            var sqlConnectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ClientAPIDbContext>(options => options.UseSqlServer(sqlConnectionString));

            //add unitofwork for repository pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IValidator<TransferFundCommand>, TransferFundValidator>();
            services.AddTransient<IValidator<WithdrawCommand>, WithdrawValidator>();
            services.AddTransient<IValidator<DepositCommand>, DepositValidator>();

            // add messagepublisher classes
            var configSection = _configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "RBank"));

            // Add framework services.
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson()
                .AddFluentValidation();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" });
            });

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                checks.AddSqlCheck("ClientAPICN", _configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(_configuration)
                 .Enrich.WithMachineName()
                 .CreateLogger();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank API - v1");
            });

            // auto migrate db
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<ClientAPIDbContext>().MigrateDB();
            }
        }
    }
}
