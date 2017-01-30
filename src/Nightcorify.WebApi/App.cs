using System;
using NLog;
using Microsoft.Owin.Hosting;
using Nightcorify.WebApi;
using Microsoft.Owin;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Reflection;

[assembly: OwinStartup( typeof( Startup ) )]
[assembly: AssemblyVersion( "0.1.0.*" )]

namespace Nightcorify.WebApi
{
    public class App
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger(); 

        public static string Version = typeof( App ).Assembly.GetName().Version.ToString();

        const string defaultListenAddress = "http://localhost:9010"; 

        /// <summary>
        /// Application entry point
        /// </summary>
        static void Main( string[] args )
        { 
            var listen = GetListenAddress( args ) ?? defaultListenAddress;

            using ( WebApp.Start<Startup>( listen ) ) {
                log.Debug( $"Server running <nightcorify@{Version}> on {listen}" );
                Console.ReadLine();
            }
        }

        static string GetListenAddress( string[] args )
        {
            if ( args.Length == 2 ) {
                if ( args[0] == "--listen" ) {
                    return args[1];
                }
            }

            return null;
        }
    }
}
