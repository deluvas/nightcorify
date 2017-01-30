using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Nightcorify.WebApi.Errors
{
    public class ErrorResponseMessage<TResponse> : IHttpActionResult
    {
        Exception _exception;
        HttpConfiguration _httpConfig;
        HttpRequestMessage _req;
        Func<Exception,TResponse> _errorFactory;
        Func<Exception, HttpStatusCode> _httpCodeMapper;

        public ErrorResponseMessage( 
            Exception exception, 
            HttpRequestMessage req,
            Func<Exception,TResponse> errorFactory,
            Func<Exception, HttpStatusCode> httpCodeMapper )
        {
            _exception = exception;
            _httpConfig = req.GetConfiguration();
            _req = req;
            _errorFactory = errorFactory;
            _httpCodeMapper = httpCodeMapper;
        }

        public ErrorResponseMessage(
            Exception exception,
            HttpRequestMessage req,
            Func<Exception, TResponse> errorFactory )
                : this( exception, req, errorFactory, null )
        {
        }

        public Task<HttpResponseMessage> ExecuteAsync( CancellationToken cancellationToken )
        {
            return Task.FromResult( GetErrorResponse() );
        }

        protected HttpResponseMessage GetErrorResponse()
        {
            var jsonFormatter = _httpConfig.Formatters.JsonFormatter; 
            var errorResponse = _errorFactory( _exception );

            HttpResponseMessage httpResponse = new HttpResponseMessage() {
                RequestMessage = _req, 
                StatusCode = _httpCodeMapper?.Invoke( _exception ) ?? HttpStatusCode.InternalServerError,
                Content = new ObjectContent<TResponse>( errorResponse, jsonFormatter, "application/json" )
            };

            return httpResponse;
        }
    }
}