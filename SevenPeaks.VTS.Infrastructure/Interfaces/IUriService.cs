using System;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);
    }
}
