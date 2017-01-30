using System;
using System.Web.Http;
using NLog;
using Owin;
using System.Threading.Tasks;
using Nightcorify.Formatting;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Nightcorify.Container;
using Nightcorify.Config;
using Nightcorify.Models;
using System.IO;
using System.Data.SQLite;
using Nightcorify.WebApi.Middleware;
using Nightcorify.Helpers;
using Nightcorify.Middleware;

namespace Nightcorify.WebApi
{
    public class Startup
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public void Configuration( IAppBuilder app )
        {
            app.Use<ProxyPassMiddleware>();
            app.UseLogging( LogHelper.ParseHttpLogEntry );
            app.UseCors( DefaultCorsOptions.Instance );
            app.UseWebApi( ConfigureWebApi() ); 
        }

        HttpConfiguration ConfigureWebApi()
        {
            log.Info( "Starting app" );

            var http = new HttpConfiguration();
            var container = new UnityContainer();
            var cfg = AppConfig.FromFile( AppConstants.AppConfigPath );
            CheckAppConfig( cfg );

            container.RegisterInstance( cfg );
            container.RegisterInstance( cfg.Content );

            try {
                http.DependencyResolver = new UnityDependencyResolver( container );
                new UnityConfig( container ).Register();
                http.UseJsonSnakeCaseFormatter();
                http.UseErrorHandler( new ErrorHandler() );
                http.MapHttpAttributeRoutes();
                http.EnsureInitialized();
            } catch ( Exception ex ) {
                log.Error( ex );
                throw;
            }

            return http;
        }

        void CheckAppConfig( AppConfig cfg )
        {
            log.Debug( "Checking config file and initializing directories.." );

            Directory.CreateDirectory( cfg.Content.Dir );
            Directory.CreateDirectory( cfg.Content.TmpDir );

            if ( !File.Exists( cfg.Ffmpeg.Path ) ) {
                throw new FileNotFoundException( "ffmpeg path" );
            }

            var cb = new SQLiteConnectionStringBuilder( cfg.SqliteConnection );
            Directory.CreateDirectory( Path.GetDirectoryName( cb.DataSource ) );
        }
    }
}
