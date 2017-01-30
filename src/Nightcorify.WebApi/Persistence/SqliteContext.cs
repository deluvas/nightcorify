using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NLog;

namespace Nightcorify.Persistence
{
    public class SqliteContext
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static HashSet<string> _dirCheckCache = new HashSet<string>();
        string _connectionString;
        IDbConnection _db;

        public JobStore Jobs { get; set; }

        public SqliteContext( string connectionString )
        {
            _connectionString = connectionString;
            _db = Factory();
            Initialize();
        }

        void Initialize()
        {
            Jobs = new JobStore( _db );
        }

        public void ApplySchema()
        {
            log.Info( "Attempting SQLite schema..." );

            using ( IDbConnection con = Factory() ) {
                con.Execute( JobStore.GetSchema() );
            }
        }

        IDbConnection Factory()
        {
            IDbConnection con = new SQLiteConnection( _connectionString );
            con.Open();
            return con;
        }
    }
}
