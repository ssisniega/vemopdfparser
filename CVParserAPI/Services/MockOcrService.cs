using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CVParserAPI.Services.Interfaces;

namespace CVParserAPI.Services;

public class MockOcrService : IOcrService
{
    private readonly ILogger<MockOcrService> _logger;

    public MockOcrService(ILogger<MockOcrService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ExtractText(IFormFile file)
    {
        _logger.LogInformation("Extracting text from file: {FileName}", file?.FileName);

        if (file == null)
            throw new ArgumentNullException(nameof(file));

        if (file.Length == 0)
            return string.Empty;

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Error reading file stream for {FileName}", file.FileName);
            throw;
        }
    }
} 