//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using WebAPI.Models;

//public class Startup
//{
//    public Startup(IConfiguration configuration)
//    {
//        Configuration = configuration;
//    }

//    public IConfiguration Configuration { get; }

//    // This method gets called by the runtime. Use this method to add services to the container.
//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

//        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

//        services.AddDbContext<AuthenticationContext>(options =>
//            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

//        services.AddDefaultIdentity<ApplicationUser>()
//            .AddRoles<IdentityRole>()
//            .AddEntityFrameworkStores<AuthenticationContext>();

//        services.Configure<IdentityOptions>(options =>
//        {
//            options.Password.RequireDigit = false;
//            options.Password.RequireNonAlphanumeric = false;
//            options.Password.RequireLowercase = false;
//            options.Password.RequireUppercase = false;
//            options.Password.RequiredLength = 4;
//        });

//        services.AddIdentity<ApplicationUser, IdentityRole>()
//            .AddEntityFrameworkStores<AuthenticationContext>()
//            .AddDefaultTokenProviders();

//        services.AddCors();

//        var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

//        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.RequireHttpsMetadata = false;
//                options.SaveToken = false;
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero
//                };
//            });
//    }


//    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//    public void Configure(WebApplication app, IHostApplicationLifetime lifetime)
//    {
//        app.UseCors(builder =>
//            builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
//            .AllowAnyHeader()
//            .AllowAnyMethod());

//        app.Use(async (ctx, next) =>
//        {
//            await next();
//            if (ctx.Response.StatusCode == 204)
//            {
//                ctx.Response.ContentLength = 0;
//            }
//        });

  


//        app.UseAuthentication();

//        app.UseHttpsRedirection();

//        app.UseRouting();

//        app.UseAuthorization();


//        //app.UseMvc();
//    }
//}
