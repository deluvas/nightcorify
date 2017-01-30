using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightcorify.Models
{
    public class FfmpegJob
    {
        public int? Id { get; set; }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public string Hash { get; set; }
        public float Rate { get; set; } = 1.2f;
        public JobStatus Status { get; set; } = JobStatus.NotStarted;
    }

    public class FfmpegJobRequest
    {
        public string InputFile { get; set; }
        public float Rate { get; set; }
    }
}
