using AuthClient.Services.Identity;
using AuthClient.Services.Identity.Requests;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinUtility.Services.Implementation;

public class XamarinIdentityClient(HttpClient httpClient) : CrossIdentityClient(httpClient)
{
    public override async Task<AuthorizationResponse> Authorize(AuthorizationRequest request)
    {
        var authUrl = request.BuildUrl();
        var callbackUrl = new Uri(request.RedirectUri);
        var result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
        var serialized = JsonSerializer.Serialize(result.Properties);
        var authorizationResponse = JsonSerializer.Deserialize<AuthorizationResponse>(serialized);
        return authorizationResponse!;
    }
}
