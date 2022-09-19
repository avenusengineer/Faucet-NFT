using System.Net.Http.Headers;
using System.Text;
using CSharpFunctionalExtensions;
using NftFaucetRadzen.Components.CardList;
using RestEase;

namespace NftFaucetRadzen.Plugins.UploadPlugins.Infura.Uploaders;

public class InfuraUploader : IUploader
{
    public Guid Id { get; } = Guid.Parse("c0d79c82-8e35-4cd6-ad35-bbe378088308");
    public string Name { get; } = "Infura";
    public string ShortName { get; } = "Infura";
    public string ImageName { get; } = "infura_black.svg";
    public bool IsSupported { get; } = true;
    public bool IsConfigured { get; private set; } = false;

    private const string DefaultGatewayUrl = "https://ipfs.infura.io:5001";
    private string ProjectId { get; set; }
    private string ProjectSecret { get; set; }
    private Uri DedicatedGatewayUrl { get; set; }

    public CardListItemProperty[] GetProperties()
    {
        var properties = new List<CardListItemProperty>();
        if (IsConfigured)
        {
            if (!string.IsNullOrEmpty(ProjectId))
            {
                properties.Add(new CardListItemProperty {Name = "ProjectId", Value = ProjectId});
            }

            if (DedicatedGatewayUrl != null)
            {
                properties.Add(new CardListItemProperty {Name = "DedicatedGatewayUrl", Value = DedicatedGatewayUrl.ToString()});
            }
        }
        else
        {
            properties.Add(new CardListItemProperty {Name = "Configured", Value = "NO", ValueColor = "red"});
        }

        return properties.ToArray();
    }

    public CardListItemConfiguration GetConfiguration()
    {
        var projectIdInput = new CardListItemConfigurationObject
        {
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Project ID",
            Placeholder = "<ProjectId>",
            Value = ProjectId,
        };
        var projectSecretInput = new CardListItemConfigurationObject
        {
            Type = CardListItemConfigurationObjectType.Input,
            Name = "API Key Secret",
            Placeholder = "<ProjectSecret>",
            Value = ProjectSecret,
        };
        var gatewayUrlInput = new CardListItemConfigurationObject
        {
            Type = CardListItemConfigurationObjectType.Input,
            Name = "Dedicated gateway URL (OPTIONAL)",
            Placeholder = "https://<your-subdomain>.infura-ipfs.io",
            Value = DedicatedGatewayUrl?.OriginalString,
        };
        return new CardListItemConfiguration
        {
            Objects = new[] { projectIdInput, projectSecretInput, gatewayUrlInput },
            ConfigureAction = async objects => await TryConfigure(objects[0].Value, objects[1].Value, objects[2].Value),
        };
    }

    public async Task<Result> TryConfigure(string projectId, string projectSecret, string dedicatedGatewayUrl)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            return Result.Failure("ProjectId is null or empty");
        }

        if (string.IsNullOrEmpty(projectSecret))
        {
            return Result.Failure("ProjectSecret is null or empty");
        }

        var apiClient = GetInfuraClient(projectId, projectSecret);
        var versionResponse = await apiClient.GetVersion();
        if (!versionResponse.ResponseMessage.IsSuccessStatusCode)
        {
            return Result.Failure<Uri>($"Status: {(int) versionResponse.ResponseMessage.StatusCode}. Reason: {versionResponse.ResponseMessage.ReasonPhrase}");
        }

        var uploadResponse = versionResponse.GetContent();
        if (string.IsNullOrEmpty(uploadResponse?.Version))
        {
            return Result.Failure<Uri>($"Unexpected response: {versionResponse.StringContent}");
        }

        if (!string.IsNullOrEmpty(dedicatedGatewayUrl))
        {
            if (!Uri.TryCreate(dedicatedGatewayUrl, UriKind.Absolute, out var parsedUrl))
            {
                return Result.Failure("Dedicated gateway URL is invalid");
            }

            if (parsedUrl.Scheme != "https")
            {
                return Result.Failure("Invalid url scheme for Dedicated gateway URL. Expected 'https'");
            }

            if (!parsedUrl.Host.EndsWith("infura-ipfs.io"))
            {
                return Result.Failure("Invalid url host for Dedicated gateway URL. Expected host ending with '.infura-ipfs.io'");
            }

            if (parsedUrl.PathAndQuery != "/")
            {
                return Result.Failure("Invalid url. Url should NOT contain path or query part.");
            }

            DedicatedGatewayUrl = parsedUrl;
        }
        else
        {
            DedicatedGatewayUrl = null;
        }

        ProjectId = projectId;
        ProjectSecret = projectSecret;
        IsConfigured = true;
        return Result.Success();
    }

    public async Task<Result<Uri>> Upload(IToken token)
    {
        var apiClient = GetInfuraClient(ProjectId, ProjectSecret);
        var fileUploadRequest = ToMultipartContent(token.Image.FileName, token.Image.FileType, token.Image.FileData);
        var response = await apiClient.UploadFile(fileUploadRequest);
        if (!response.ResponseMessage.IsSuccessStatusCode)
        {
            return Result.Failure<Uri>($"Status: {(int) response.ResponseMessage.StatusCode}. Reason: {response.ResponseMessage.ReasonPhrase}");
        }

        var uploadResponse = response.GetContent();
        if (string.IsNullOrEmpty(uploadResponse?.Hash))
        {
            return Result.Failure<Uri>($"Unexpected response: {response.StringContent}");
        }

        if (DedicatedGatewayUrl != null)
        {
            return new Uri(DedicatedGatewayUrl, "ipfs/" + uploadResponse.Hash);
        }

        return new Uri("ipfs://" + uploadResponse.Hash);
    }

    private static IInfuraIpfsApiClient GetInfuraClient(string projectId, string projectSecret, string gatewayUrl = DefaultGatewayUrl)
    {
        var uploadClient = RestClient.For<IInfuraIpfsApiClient>(gatewayUrl);
        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{projectId}:{projectSecret}"));
        uploadClient.Auth = new AuthenticationHeaderValue("Basic", auth);
        return uploadClient;
    }

    private MultipartContent ToMultipartContent(string fileName, string fileType, string fileData)
    {
        var content = new MultipartFormDataContent();

        var bytes = Base64DataToBytes(fileData);
        var imageContent = new ByteArrayContent(bytes);
        imageContent.Headers.Add("Content-Type", fileType);
        content.Add(imageContent, "\"file\"", $"\"{fileName}\"");

        return content;
    }

    private byte[] Base64DataToBytes(string fileData)
    {
        var index = fileData.IndexOf(';');
        var encoded = fileData.Substring(index + 8);
        return Convert.FromBase64String(encoded);
    }
}
