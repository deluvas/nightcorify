using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Nightcorify.Convertors
{
    internal class FfmpegProcess
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        Process _ffmpeg;

        public FfmpegProcess( Process ffmpeg )
        {
            _ffmpeg = ffmpeg;
        }

        public bool Execute()
        {
            log.Debug( "Starting up ffmpeg process {0} with args {1}", _ffmpeg.StartInfo.FileName, _ffmpeg.StartInfo.Arguments );

            _ffmpeg.Start();
            _ffmpeg.WaitForExit();

            if ( _ffmpeg.ExitCode != 0 ) {
                log.Error( "ffmpeg exit code {0}", _ffmpeg.ExitCode );
                return false;
            }

            return true;
        }

        public void ExecuteAsync()
        {
            log.Debug( "Starting up ffmpeg process {0} with args {1}", _ffmpeg.StartInfo.FileName, _ffmpeg.StartInfo.Arguments );

            _ffmpeg.Start();
        }
    }
}
