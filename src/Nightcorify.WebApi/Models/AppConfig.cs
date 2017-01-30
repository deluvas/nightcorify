using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nightcorify.Models
{
    public class AppConfig
    {
        public string SqliteConnection { get; set; }
        public ContentSection Content { get; set; }
        public FfmpegSection Ffmpeg { get; set; }

        public static AppConfig FromFile( string file )
        {
            return JsonConvert.DeserializeObject<AppConfig>( 
                File.ReadAllText( file ) );
        }

        public class FfmpegSection
        {
            public string Path { get; set; }
        }

        public class ContentSection
        {
            public string BaseUrl { get; set; }
            public string Dir { get; set; }
            public string TmpDir { get; set; }
        }
    }
}
