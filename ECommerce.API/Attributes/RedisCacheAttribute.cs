using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ECommerce.API.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInSeconds;
        public RedisCacheAttribute(int durationInSeconds = 60)
        {
            _durationInSeconds = durationInSeconds; 
        }

        // work before and after the endpoint
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service [manual made so get requierd service]
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            #region HowToCreate CacheKey
            //1 URL http://localhost:3000/api/Products //BaseUrl/api/products
            //2 URL http://localhost:3000/api/Products?typeid = 2 etc
            //3 URL http://localhost:3000/api/Products?typeid = 2 & brandid = 3 etc
            // the base is 1 and will check specification for 2 and 3 if exist will inject it with the base 1
            //it is one end point get all products but if come queryparames [specification] must deal with this too and cache it
            #endregion
            //If date exist in cache  => get data from cache + skip endpoint
            var cachekey = CreateCacheKey(context.HttpContext.Request);
            var data = await cacheService.GetDataAsync(cachekey);
            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult() //ok result new __ no need endpoint  __ have data have status code 
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;//out  to kill [return base.OnActionExecutionAsync(context, next)] this call endpoint nooo no need it 
            }
            //if data not exist in cache => execute endpoint +store result in cache [status code 200Ok]

            var executedContext = await next.Invoke(); // skip cache and go to next step(run endpoint)
            if(executedContext.Result is OkObjectResult {Value : not null } ok)
            {
                await cacheService.SetDataAsync(cachekey, ok.Value, TimeSpan.FromSeconds(90));
            }

        }
        
        private static string CreateCacheKey(HttpRequest request)
        {
            var Key = new StringBuilder(); //one space on memory to append on it immutable 
            Key.Append(request.Path);//get all products without any specification //key = baseurl/api/products
            //specification => sort,search,pagination,brandid,typeid
            if (request.Query.Any())// anything after {?} in baseurl is a query
            {
                //key = baseurl/api/products
                Key.Append('?');//key = baseurl/api/products?
                foreach (var (k,v) in request.Query.OrderBy(X=>X.Key)) // many specification // {typeid = 2 & brandid =3} =={brandid = 3 & typeid =2} same thing but he will do another cache so orderby first 
                {
                    Key.Append(k);//key = baseurl/api/products?brandid
                    Key.Append('=');
                    Key.Append(v);//key = baseurl/api/products?brandid = 2
                    Key.Append('&'); 
                }
                //key = baseurl/api/products?brandid = 2 & typeid = 3 &   // last & will not effect my query
                
            }

            return Key.ToString();
        }


    }
}// here must be the location cause api has ActionFilterAttribute that i need to can add new custom attribute and 
//OnActionExecutionAsync before [if has cache value in redis take it] and after [ if no run end point the cache it and return respone]
