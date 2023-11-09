using System.Net.Http.Headers;

namespace Common;

public class HackTheFutureClient : HttpClient
{
    public HackTheFutureClient()
    { 
        BaseAddress = new Uri("https://app-involved-htf-api.azurewebsites.net");
    }

    //Use this method after creating a HackTheFutureClient instance to authenticate yourself with the server
    public async Task Login(string teamName, string password)
    {
        var response = await GetAsync($"/api/team/token?teamname={teamName}&password={password}");
        if (!response.IsSuccessStatusCode)
            throw new Exception("You weren't able to log in, did you provide the correct credentials?");
        var token = await response.Content.ReadAsStringAsync();
        DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}