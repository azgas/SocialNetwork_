using SocNet_EPX.TemplateUtilities.AppConfiguration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using Thinktecture.IdentityModel.Client;
using Thinktecture.IdentityServer.Core;

namespace SocNet_EPX
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Thinktecture.IdentityServer.Core.Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "Cookies",
                CookieName = ".EPX.Cookie"
            });

            AppConfig applicationAuthConfiguration = AppConfig.GetAppConfig;

            applicationAuthConfiguration.ClientId = "Socnet3";
            applicationAuthConfiguration.ResponseType = "id_token token";
            applicationAuthConfiguration.Scope = "openid profile epx_resources";

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
            {
                ClientId = applicationAuthConfiguration.ClientId,
                Authority = @"https://auth.epx-platform.org/identity",
                ResponseType = applicationAuthConfiguration.ResponseType,
                Scope = applicationAuthConfiguration.Scope,
                SignInAsAuthenticationType = "Cookies",


                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                {
                    var id = n.AuthenticationTicket.Identity;

                    // we want to keep first name, last name, subject and roles
                    var givenName = id.FindFirst(Constants.ClaimTypes.GivenName) ?? new Claim(Constants.ClaimTypes.GivenName, "");
                    var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName) ?? new Claim(Constants.ClaimTypes.FamilyName, "");
                    var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                    var lang = id.FindFirst(Constants.ClaimTypes.Locale) ?? new Claim(Constants.ClaimTypes.Locale, "en-US");
                    //var roles = id.FindAll(Constants.ClaimTypes.Role);

                    // create new identity and set name and role claim type
                    var nid = new ClaimsIdentity(
                        id.AuthenticationType,
                        Constants.ClaimTypes.GivenName,
                        Constants.ClaimTypes.Role);

                    nid.AddClaim(givenName);
                    nid.AddClaim(familyName);
                    nid.AddClaim(sub);
                    nid.AddClaim(lang);
                    //nid.AddClaims(roles);

                    // add some other app specific claim
                    nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                    nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));
                    n.AuthenticationTicket = new AuthenticationTicket(
                        nid,
                        n.AuthenticationTicket.Properties);
                },
                    RedirectToIdentityProvider = async n =>
                    {
                        n.ProtocolMessage.RedirectUri = (n.OwinContext.Request.IsSecure ? @"https://" : @"http://") + n.OwinContext.Request.Uri.Host + (n.OwinContext.Request.Uri.IsDefaultPort ? "" : ":" + n.OwinContext.Request.Uri.Port) + "/";
                        n.ProtocolMessage.PostLogoutRedirectUri = (n.OwinContext.Request.IsSecure ? @"https://" : @"http://") + n.OwinContext.Request.Uri.Host + (n.OwinContext.Request.Uri.IsDefaultPort ? "" : ":" + n.OwinContext.Request.Uri.Port) + "/";
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                            n.ProtocolMessage.IdTokenHint = idTokenHint;
                        }
                    }
                }
            });



            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
    }
}