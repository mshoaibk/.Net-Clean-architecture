using HRM_Domain.Model;
using HRM_Infrastructure.HRMDataBaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class SubscriptionActionFilter : IAsyncActionFilter
{
    private readonly HRMContexts dbContextHRM;
    public SubscriptionActionFilter(HRMContexts context)
    {
        dbContextHRM = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //int? companyID = context.HttpContext.Session.GetInt32("companyID");
        var companyID = context.HttpContext.Session.GetInt32("companyID");
        var controllerName = context.RouteData.Values["controller"];
        var actionName = context.RouteData.Values["action"];
        var CompanyID = context.RouteData.Values["companyID"];
       
        if(companyID == null && CompanyID !=null)
        {
        context.HttpContext.Session.SetString("companyID",CompanyID.ToString());
        }
       
        string messgae ="";
        // Perform subscription checks here
        if (!IsUserSubscribed(controllerName.ToString(), actionName.ToString(), companyID,ref messgae))
        {
            context.Result = new ObjectResult(messgae)
            {
                StatusCode = 403, // Forbidden
            };
            return;
        }

        // Continue with the action if subscription is valid
        await next();
    }

    private bool IsUserSubscribed(string controllerName, string Action, int? companyID, ref string messgae)
    {

        //alow login and registration
        if (controllerName == "ApplicationUsers" && Action == "Login") { return true; } //allow login request
        if (controllerName == "ApplicationUsers" && Action == "RegisterCompany") { return true; } //allow registration
        if (controllerName == "CompanyRegistration" && Action == "RegisterCompanyDetail") { return true; }

        //after that check if company has Subscription or not

        if (!(dbContextHRM.TblCompanySubscription.Where(x => x.CompanyId == companyID && x.IsPaid == true && x.ExpiryDate > DateTime.Now).Any()))
            {
            messgae = "Subscription required";
            return true;
            }
        

        //check if Subscription is expired or not


        messgae = "Subscription required";
        return true;
       // return true;
        // Implement logic to check the user's subscription status in your database or subscription service
        // Return true if the user is subscribed, false otherwise
    }
}
