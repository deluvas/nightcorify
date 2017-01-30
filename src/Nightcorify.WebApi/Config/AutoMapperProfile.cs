using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nightcorify.Models;
using Nightcorify.ViewModels;

namespace Nightcorify.Config
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile( AppConfig.ContentSection contentCfg )
        {
            CreateMap<FfmpegJob, JobDto>()
                .ForMember( d => d.DownloadUrl, ( opt ) => 
                    opt.ResolveUsing( ( s ) => $"{contentCfg.BaseUrl}/{s.OutputFile}" ) );
        }
    }
}
