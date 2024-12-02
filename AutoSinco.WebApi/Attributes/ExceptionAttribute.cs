using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AutoSinco.Shared.GeneralDTO;

namespace AutoSinco.WebApi.Attributes
{
    public class ExcepcionAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(RespuestaDto.ErrorInterno())
            {
                StatusCode = 500,
            };
        }
    }
}
