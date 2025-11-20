using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Helper;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Cooktel_E_commrece.Extenstions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeaders paginationHeaders)
        {
            var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeaders, jsonOption));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }


        public static void AddImageLink(this HttpContext context, List<String> img)
        {

            for (int i = 0; i < img.Count(); i++)
            {
                img[i] = $"{context.Request.Scheme}://{context.Request.Host}/Uploads/{Path.GetFileName(img[i])}";
            }
        }
    }
}
