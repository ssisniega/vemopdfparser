using System.Threading.Tasks;
using System.Collections.Generic;
using CVParserAPI.Models;
using CVParserAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CVParserAPI.Services;

public class MockLlmService : ILlmService
{
    private readonly ILogger<MockLlmService> _logger;
    private const int MaxTextLength = 500_000;

    public MockLlmService(ILogger<MockLlmService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResumeData> ExtractInformation(string text)
    {
        _logger.LogInformation("Extracting information from text");

        if (text == null)
            throw new ArgumentNullException(nameof(text));

        if (text.Length > MaxTextLength)
            throw new ArgumentException($"Text is too long. Maximum allowed length is {MaxTextLength} characters.");

        if (string.IsNullOrEmpty(text))
            return new ResumeData();

        // Mock data for testing
        return await Task.FromResult(new ResumeData
        {
            Name = "John Smith",
            Contact = new ContactInfo
            {
                Email = "john.smith@email.com",
                Phone = "+1234567890",
                Location = "San Francisco, CA"
            },
            WorkExperience = new List<Experience>
            {
                new Experience
                {
                    Company = "Tech Solutions Inc",
                    Position = "Full Stack Developer",
                    Period = "2019-2024",
                    Description = "Developed scalable web applications"
                }
            },
            Skills = new List<string> { "JavaScript", "Python", "Docker", "AWS", "React" }
        });
    }
} 