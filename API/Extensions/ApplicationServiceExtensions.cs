using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using API.Helpers;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackReplyRepository, FeedbackReplyRepository>();
            //services.AddScoped<IVoteRepository, VotesRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IVVoteRepository, VotesRepository>();
            services.AddScoped<LogUserActivity>(); 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeparmtentRepo, DepartmentRepository>();
            services.AddScoped<IPetitionRepository, PetitionRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}