using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Nightcorify.Convertors;
using Nightcorify.Helpers;
using Nightcorify.Models;
using Nightcorify.ViewModels;
using Nightcorify.WebApi.ViewModels;

namespace Nightcorify.Controllers
{
    public class DefaultController : ApiController
    {
        FfmpegProcessManager _manager;
        IMapper _mapper;
        Func<MultipartFormDataStreamProvider> _streamProviderFactory;

        public DefaultController( 
            FfmpegProcessManager ffmpegManager,
            IMapper mapper,
            Func<MultipartFormDataStreamProvider> streamProviderFactory )
        {
            _mapper = mapper;
            _manager = ffmpegManager;
            _streamProviderFactory = streamProviderFactory;
        }

        /// <summary>
        /// Upload mp3 file for nightcorification. 
        /// Max file size 10mb 
        /// </summary>
        /// <param name="rate">Default 1.2</param>
        /// <returns>Job</returns>
        [HttpPost]
        [Route( "v1/nightcorify" )]
        public ResponseDto<IntegerIdentifierDto> AddJob( 
            [FromUri] float? rate = 1.2f )
        {
            if ( rate != null && rate <= 1 || rate > 3 ) {
                throw new Exception( "Rate is out of range." );
            } 
            if ( !Request.Content.IsMimeMultipartContent() 
                || Request.Content.Headers.ContentLength <= 1024 ) {
                throw new InvalidOperationException( "No file found." );
            }
            var content = Request.Content
                .ReadAsMultipartAsync( _streamProviderFactory() )
                .Result;
            var filedata = content.FileData.First();

            if ( filedata.Headers.ContentType?.MediaType != "audio/mp3" ) {
                throw new Exception( "No mp3 file found." );
            }

            var job = _manager.RunJobAsync( new FfmpegJobRequest {
                InputFile = filedata.LocalFileName,
                Rate = rate.Value
            } );

            return new ResponseDto<IntegerIdentifierDto> {
                Data = new IntegerIdentifierDto( job.Id.Value )
            };
        }

        [HttpGet]
        [Route("v1/nightcorify/{jobId}/status")]
        public ResponseDto<JobDto> GetJobStatus( [FromUri] int jobId )
        {
            var job = _manager.GetJob( jobId );
            if ( job == null ) {
                throw new Exception( "not found" );
            }
            return new ResponseDto<JobDto> {
                Data = _mapper.Map<JobDto>( job )
            };
        }
    }
}
