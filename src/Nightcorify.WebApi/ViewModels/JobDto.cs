using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nightcorify.Models;

namespace Nightcorify.ViewModels
{
    public class JobDto
    {
        public int? Id { get; set; }
        public string DownloadUrl { get; set; }
        public string Hash { get; set; }
        public float Rate { get; set; }
        public JobStatus Status { get; set; }
    }
}
