using System;
namespace AuthenticationService.Core.Interfaces
{
	public interface IHttpClientService
	{
		Task<TRes> PostRequestAsync<Treq, TRes>(string baseUrl, string requestUrl, Treq requestModel, string token = null)
			where TRes : class
			where Treq : class;

		Task<TRes> GetRequestAsync<TRes>(string baseUrl, string requestUrl, string token = null)
			where TRes : class;
    }
}

