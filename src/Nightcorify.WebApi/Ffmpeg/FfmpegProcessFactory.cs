using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.Helpers
{
    public class FfmpegProcessFactory
    {
        string _ffmpegPath;

        public FfmpegProcessFactory( string ffmpegPath )
        {
            _ffmpegPath = ffmpegPath;
        }

        public Process CreateNightcoreConverter( string inputfile, string outputfile, float rate = 1.2f )
        {
            const string template = "-i \"{0}\" -loglevel 0 -af asetrate=r=44100*{1} -y \"{2}\""; // -y to force overwrite
            var args = string.Format( template, inputfile, rate, outputfile );

            var process = new Process() {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo( _ffmpegPath, args ) {
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            return process;
        }
    }
}
