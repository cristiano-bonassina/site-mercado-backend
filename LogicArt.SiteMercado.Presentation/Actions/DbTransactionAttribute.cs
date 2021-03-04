using System.Threading.Tasks;
using LogicArt.Arch.Application.Repositories.Abstractions.Transaction;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LogicArt.SiteMercado.Presentation.Actions
{
    public class DbTransactionAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            await unitOfWork.BeginTransactionAsync();
            await base.OnActionExecutionAsync(context, next);
            await unitOfWork.EndTransactionAsync();
        }
    }
}
