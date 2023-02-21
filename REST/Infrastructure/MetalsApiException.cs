using System;
using System.Net;

namespace AcumaticaMetalsAPI
{
	[Serializable]
	public class MetalsApiException : ApplicationException
	{
		public HttpStatusCode HttpStatusCode { get; set; }
		public MetalsApiError MetalsApiError { get; set; }

		public MetalsApiException()
		{
		}

		public MetalsApiException(HttpStatusCode statusCode, MetalsApiError metalsApiError, string message) : base(message)
		{
			HttpStatusCode = statusCode;
			MetalsApiError = metalsApiError;
		}
	}
}

