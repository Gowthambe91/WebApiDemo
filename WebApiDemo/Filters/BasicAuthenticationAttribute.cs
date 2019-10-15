using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Text;
using WebApiDemo.Models.BusinessLogic;
using System.Security.Principal;
using System.Threading;
using WebApiDemo.Models.DomainModel;

namespace WebApiDemo.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private const string Realm = "My Realm";  //Not sure of what is this, even without this work fine.
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.Add("WWW-authenticate", string.Format("Basic realm=\"{0}\"", Realm));
                }
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;

                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                string[] userNamePasswordArray = decodedAuthenticationToken.Split(':');

                string userName = userNamePasswordArray[0];

                string password = userNamePasswordArray[1];

                var userdetail = ValidateUser.GetUserDetails(userName, password);

                if (ValidateUser.Login(userName, password))
                {
                    var identity = new GenericIdentity(userName);

                    var principal = new GenericPrincipal(identity, userdetail.Roles.Split(','));

                    Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

        }
    }
}