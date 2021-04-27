using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Data.DataAccess.Implementation;
using FacilityManagement.Services.Models.AppSettingModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FacilityManagement.Services.API.Extensions
{
    /// <summary>
    /// DIServiceExtensions class
    /// </summary>
    public static class DIServiceExtensions
    {
        /// <summary>
        /// AddDependencyInjection method
        /// </summary>
        /// <param name="services"></param>
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGraphApiService, GraphApiService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFeedService, FeedService>();
            services.AddScoped<IComplaintService, ComplaintServices>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReplyService, ReplyService>();
            services.AddScoped<IRatingService, RatingService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFeedRepository, FeedRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReplyRepository, ReplyRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
        }

        /// <summary>
        /// AddConfiguration method
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AccountSettings>(configuration.GetSection("AccountSettings"));
        }
    }
}
