using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("MangoAPI");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data is not null)
            {
                var objectSerialized = JsonConvert.SerializeObject(requestDto.Data);
                message.Content = new StringContent(objectSerialized, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage? apiResponse = null;

            switch (requestDto.ApiType)
            {
                case ApiType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Not Found" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiReponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiReponseDto;
            }
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto()
            {
                Message = ex.Message.ToString(),
                IsSuccess = false
            };
            return dto;
        }
       
    }
}
