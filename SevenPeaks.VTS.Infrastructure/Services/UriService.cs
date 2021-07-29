using System;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Services.Interfaces
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = new Uri(string.Concat(_baseUri, route));

            var modifiedUri =
                $"{endpointUri}?pageNumber={filter.PageNumber}&pageSize={filter.PageSize}";

            return new Uri(modifiedUri);
        }
    }
}
