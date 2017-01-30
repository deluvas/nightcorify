using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Nightcorify.Controllers;
using Nightcorify.Convertors;
using Nightcorify.Persistence;
using Dapper;
using NLog;
using System.IO;
using Nightcorify.Config;
using AutoMapper;
using Nightcorify.Models;
using Nightcorify.Helpers;
using System.Net.Http;

namespace Nightcorify.Container
{
    internal class UnityConfig
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private IUnityContainer _container;
        AppConfig _cfg;

        public UnityConfig( IUnityContainer container )
        {
            _container = container;
            _cfg = _container.Resolve<AppConfig>();
        }

        public void Register()
        {
            RegisterSqlite();
            RegisterAutoMapper();
            RegisterFfmpegRelated();
            RegisterOthers();
        }

        public void RegisterSqlite()
        {
            log.Info( "Registering SQLite" );

            var context = new SqliteContext( _cfg.SqliteConnection );
            context.ApplySchema();

            _container.RegisterType<SqliteContext>( new HierarchicalLifetimeManager(),
                new InjectionFactory( ( c ) => {
                    var ctx = new SqliteContext( _cfg.SqliteConnection );
                    return ctx;
                } ) );

            _container.RegisterInstance<Func<SqliteContext>>( () => 
                new SqliteContext( _cfg.SqliteConnection ) );
        }

        public void RegisterAutoMapper()
        {
            log.Info( "Registering AutoMapper" );

            var mapper = new MapperConfiguration( conf => {
                conf.AddProfile( new AutoMapperProfile( _cfg.Content ) );
            } );
            _container.RegisterInstance( mapper.CreateMapper() );
        }

        public void RegisterFfmpegRelated()
        {
            log.Info( "Registering ffmpeg services" );

            _container.RegisterInstance( new FfmpegProcessFactory( _cfg.Ffmpeg.Path ) );
        }

        public void RegisterOthers()
        {
            _container.RegisterInstance<Func<MultipartFormDataStreamProvider>>( () =>
                new MultipartFormDataStreamProvider( _cfg.Content.TmpDir ) );
        }
    }
}
