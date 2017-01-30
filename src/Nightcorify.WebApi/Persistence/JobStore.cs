using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Nightcorify.Models;

namespace Nightcorify.Persistence
{
    public class JobStore
    {
        IDbConnection _db;

        public JobStore( IDbConnection db )
        {
            _db = db;
        }

        public FfmpegJob GetById( int id )
        {
            return _db.Query<FfmpegJob>( "select * from [Jobs] where Id = @id", new {
                id = id
            } ).FirstOrDefault();
        }


        public FfmpegJob FindByHash( string hash )
        {
            return _db.Query<FfmpegJob>( "select * from [Jobs] where Hash = @hash", new {
                hash = hash
            } ).FirstOrDefault();
        }

        public FfmpegJob FindBy( string hash, float rate )
        {
            return _db.Query<FfmpegJob>( "select * from [Jobs] where Hash = @hash and Rate = @rate", new {
                hash = hash,
                rate = rate
            } ).FirstOrDefault();
        }

        public void Add( FfmpegJob job )
        {
            job.Id = _db.Query<int>( @"
                insert into [Jobs] values ( NULL, @InputFile, @OutputFile, @Hash, @Rate, @Status );
                select last_insert_rowid()", job ).FirstOrDefault();
        }

        public void Update( FfmpegJob job )
        {
            _db.Execute( @"
                update [Jobs] 
                set 
                  InputFile = @InputFile,     
                  OutputFile = @OutputFile,     
                  Hash = @Hash,     
                  Rate = @Rate,     
                  Status = @Status
                where Id = @Id", job );
        }

        public static string GetSchema()
        {
            var sql = @"
                create table if not exists Jobs (
                    Id integer not null primary key AUTOINCREMENT,
                    InputFile nvarchar(300) not null,
                    OutputFile nvarchar(300) not null,
                    Hash nvarchar(256) not null,
                    Rate decimal(5,2) not null default 1.2,
                    Status integer(3) not null default 0
                )";
            return sql;
        }
    }
}
