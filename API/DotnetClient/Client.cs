
public class Client
{/*
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:3000/"),
    };*/
    public HttpClient _Client { get; private set; }
    public Client(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://localhost:5059/");
        _Client = httpClient;

    }
}