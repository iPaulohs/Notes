using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Notes.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.ModelState.IsValid)
        {
            var messages = context.ModelState.SelectMany(ms => ms.Value.Errors).ToList();

            context.Result = new BadRequestObjectResult(messages);
        }
    }
}
