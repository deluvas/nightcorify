using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Nightcorify.Helpers;
using Nightcorify.Models;
using Nightcorify.Persistence;
using NLog;

namespace Nightcorify.Convertors
{
    public class FfmpegProcessManager
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        const int processTimeout = 15 * 1000;

        JobStore _jobs;
        SqliteContext _data;
        Func<SqliteContext> _ctxFactory;
        FfmpegProcessFactory _ffmpegFactory;
        string _cacheDir;
        string _tmpDir;

        public FfmpegProcessManager( 
            Func<SqliteContext> dataFactory, 
            FfmpegProcessFactory ffmpegFactory,
            AppConfig.ContentSection contentCfg )
        {
            _ctxFactory = dataFactory;
            _ffmpegFactory = ffmpegFactory;
            _data = dataFactory();
            _jobs = _data.Jobs;
            _cacheDir = contentCfg.Dir;
            _tmpDir = contentCfg.TmpDir;
        }

        public FfmpegJob RunJobAsync( FfmpegJobRequest request )
        {
            if ( request == null ) {
                throw new ArgumentNullException( "request" );
            }

            // Calculate hash for input file
            string sha256 = CalculateFileHash( request.InputFile );

            // If an existing job exists with the same hash
            // exists simply return it
            var pair = _jobs.FindBy( sha256, request.Rate );
            if ( pair != null ) {
                if ( pair.Status == JobStatus.NotStarted 
                    || pair.Status == JobStatus.Failed ) 
                {
                    pair.Status = JobStatus.Working;
                    _jobs.Update( pair );

                    Task.Run( () => {
                        ExecuteProcess( pair );
                    } );
                }
                return pair;
            }

            var job = new FfmpegJob {
                InputFile = Path.GetFileName( request.InputFile ),
                OutputFile = $"{sha256.Substring(30)}{request.Rate*100}.mp3",
                Rate = request.Rate,
                Hash = sha256,
                Status = JobStatus.Working
            };
            _jobs.Add( job );

            Task.Run( () => {
                ExecuteProcess( job );
            } );

            return job;
        }

        string CalculateFileHash( string filepath )
        {
            using ( var file = File.OpenRead( filepath ) ) {
                return CryptoHelper.ComputeSHA256( file );
            }
        }

        void ExecuteProcess( FfmpegJob job )
        {
            var proc = _ffmpegFactory.CreateNightcoreConverter(
                Path.GetFullPath( $"{_tmpDir}/{job.InputFile}" ),
                Path.GetFullPath( $"{_cacheDir}/{job.OutputFile}" ),
                job.Rate
            );
            log.Debug( "Executing ffmpeg process with args: {0}", proc.StartInfo.Arguments );

            // Execute process
            proc.Start();
            proc.WaitForExit( processTimeout ); // blocking

            // Process finished or timed out
            if ( proc.ExitCode != 0 ) {
                job.Status = JobStatus.Failed;
            }
            else {
                job.Status = JobStatus.Success;
            }

            log.Debug( "Job {0} finished with status code: {1}", job.Id, job.Status );

            var ctx = _ctxFactory();
            ctx.Jobs.Update( job );
        }

        public FfmpegJob GetJob( int jobId )
        {
            return _jobs.GetById( jobId );
        }
    }
}
