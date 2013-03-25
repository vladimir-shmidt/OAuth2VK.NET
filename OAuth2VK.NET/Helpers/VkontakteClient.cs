using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;

namespace OAuth2VK.NET.Helpers
{
    public class VkontakteClient : OAuth2Client
    {
        public static readonly string Name = "Vkontakte";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Permissions _scope;
        private int UserId { get; set; }
        private const string AuthorizationEndpoint = @"https://oauth.vk.com/authorize";
        private const string TokenEndpoint = @"https://oauth.vk.com/access_token";
        private const string ApiEndpoint = @"https://api.vk.com/method";
        public VkontakteClient(string clientId, string clientSecret, Permissions scope)
            : base(Name)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            dynamic requestParams = new ExpandoObject();
            requestParams.client_id = _clientId;
            requestParams.scope = _scope.ToString();
            requestParams.redirect_uri = HttpUtility.UrlEncode(returnUrl.ToString());
            requestParams.response_type = "code";
            return new Uri(CombineRequestString(AuthorizationEndpoint, (IDictionary<string, object>)requestParams));
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            dynamic requestParams = new ExpandoObject();
            requestParams.client_id = _clientId;
            requestParams.client_secret = _clientSecret;
            requestParams.uids = UserId;
            requestParams.access_token = accessToken;
            string response = GetResponse(CombineRequestString(string.Format("{0}/{1}", ApiEndpoint, "users.get"), (IDictionary<string, object>)requestParams));
            ResponseWrapper<UserInfo> usersInfo = new JsonSerializer().Deserialize<ResponseWrapper<UserInfo>>(new JsonTextReader(new StringReader(response)));
            var user = usersInfo.Response.SingleOrDefault();
            var result = new Dictionary<string, string>();
            if (user == null) return result;
            result.Add("UserId", user.UserId);
            result.Add("FirstName", user.FirstName);
            result.Add("LastName", user.LastName);
            result.Add("PhotoUrl", user.PhotoUrl);
            return result;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            dynamic requestParams = new ExpandoObject();
            requestParams.client_id = _clientId;
            requestParams.client_secret = _clientSecret;
            requestParams.code = authorizationCode;
            requestParams.redirect_uri = HttpUtility.UrlEncode(returnUrl.ToString());
            string response = GetResponse(CombineRequestString(TokenEndpoint, (IDictionary<string, object>)requestParams));
            AccessTokenResponse accessTokenResponse = new JsonSerializer().Deserialize<AccessTokenResponse>(new JsonTextReader(new StringReader(response)));
            UserId = accessTokenResponse.UserId;
            return accessTokenResponse.AccessToken;
        }

        public override AuthenticationResult VerifyAuthentication(HttpContextBase context, Uri returnPageUrl)
        {
            string code = context.Request.QueryString["code"];
            if (string.IsNullOrEmpty(code))
            {
                return AuthenticationResult.Failed;
            }
            string accessToken = QueryAccessToken(returnPageUrl, code);
            if (accessToken == null)
            {
                return AuthenticationResult.Failed;
            }
            IDictionary<string, string> userData = GetUserData(accessToken);
            if (userData == null)
            {
                return AuthenticationResult.Failed;
            }
            userData["AccessToken"] = accessToken;
            return new AuthenticationResult(true, Name, userData["UserId"], userData["FirstName"] + " " + userData["LastName"], userData);
        }

        private static string CombineRequestString(string endPoint, IDictionary<string, object> data)
        {
            return string.Format("{0}?{1}", endPoint, string.Join("&", data.Select(o => string.Format("{0}={1}", o.Key, o.Value))));
        }

        private static string GetResponse(string requestString)
        {
            string resp = string.Empty;
            using (var response = WebRequest.Create(requestString).GetResponse())
            {
                var steam = response.GetResponseStream();
                if (steam != null)
                    resp = new StreamReader(steam).ReadToEnd();
            }
            return resp;
        }

        private class AccessTokenResponse
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }
            [JsonProperty(PropertyName = "expires_in")]
            public int ExpiresIn { get; set; }
            [JsonProperty(PropertyName = "user_id")]
            public int UserId { get; set; }
        }

        private class ResponseWrapper<T>
        {
            [JsonProperty(PropertyName = "response")]
            public List<T> Response { get; set; }
        }

        [JsonObject]
        private class UserInfo
        {
            [JsonProperty(PropertyName = "uid")]
            public string UserId { get; set; }
            [JsonProperty(PropertyName = "first_name")]
            public string FirstName { get; set; }
            [JsonProperty(PropertyName = "last_name")]
            public string LastName { get; set; }
            [JsonProperty(PropertyName = "photo_50")]
            public string PhotoUrl { get; set; }
        }

        [Flags]
        public enum Permissions
        {
            Def = 0,
            Notify = 1,
            Friends = 2,
            Photos = 4,
            Audio = 8,
            Video = 16,
            Docs = 131072,
            Notes = 2048,
            Pages = 128,
            Status = 1024,
            Offers = 32,
            Questions = 64,
            Wall = 8192,
            Groups = 262144,
            Messages = 4096,
            Notifications = 524588,
            Stats = 1048576,
            Ads = 32768,
            Offline,
            Nohttps,
            All = Notify | Friends | Photos | Audio | Video | Docs | Notes | Pages | Status | Offers | Questions | Wall | Groups | Messages | Notifications | Stats | Ads | Offline | Nohttps
        }
    }
}