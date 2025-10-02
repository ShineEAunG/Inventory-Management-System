using Supabase;

namespace InventoryManagementSystem.Services.FileHandling;

public class SupaBaseService
{
    private readonly Client _client;
    private readonly string? _url;
    private readonly string? _serviceRoleKey;
    private readonly string? _bucketName;
    private readonly string? _directory;
    public SupaBaseService(IConfiguration config)
    {
        _url = config["Supabase:Url"];
        _serviceRoleKey = config["Supabase:ServiceRoleKey"];
        _bucketName = config["SupaBase:BucketName"];
        _directory = config["Supabase:StorageDirectory"];
        _client = new Client(_url!, _serviceRoleKey);
        _client.InitializeAsync();
    }
    public Client Client => _client;
    public string Url => _url;  //project ref is here
    public string ServiceRoleKey => _serviceRoleKey;
    public string BucketName => _bucketName;
    public string Directory => _directory;
    public bool CheckConfig() =>
        !string.IsNullOrEmpty(_url) &&
        !string.IsNullOrEmpty(_serviceRoleKey) &&
        !string.IsNullOrEmpty(_bucketName) &&
        !string.IsNullOrEmpty(_directory);
}