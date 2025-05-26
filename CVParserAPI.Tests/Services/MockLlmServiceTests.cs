using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using CVParserAPI.Services;
using CVParserAPI.Models;
using CVParserAPI.Services.Interfaces;

namespace CVParserAPI.Tests.Services;

public class MockLlmServiceTests : TestBase
{
    private readonly Mock<ILogger<MockLlmService>> _loggerMock;
    private readonly MockLlmService _service;

    public MockLlmServiceTests()
    {
        _loggerMock = new Mock<ILogger<MockLlmService>>();
        _service = new MockLlmService(_loggerMock.Object);
    }

    [Fact]
    public async Task ExtractInformation_ValidText_ReturnsResumeData()
    {
        // Arrange
        var text = GetSampleResumeText();

        // Act
        var result = await _service.ExtractInformation(text);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Smith");
        result.Contact.Should().NotBeNull();
        result.Contact.Email.Should().Be("john.smith@email.com");
        result.Contact.Phone.Should().Be("+1234567890");
        result.Contact.Location.Should().Be("San Francisco, CA");
        result.WorkExperience.Should().NotBeEmpty();
        result.Skills.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ExtractInformation_NullText_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ExtractInformation(null));
    }

    [Fact]
    public async Task ExtractInformation_EmptyText_ReturnsEmptyResumeData()
    {
        // Act
        var result = await _service.ExtractInformation("");

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().BeEmpty();
        result.Contact.Should().NotBeNull();
        result.Contact.Email.Should().BeEmpty();
        result.Contact.Phone.Should().BeEmpty();
        result.Contact.Location.Should().BeEmpty();
        result.WorkExperience.Should().BeEmpty();
        result.Skills.Should().BeEmpty();
    }

    [Fact]
    public async Task ExtractInformation_JsonFormatText_ReturnsResumeData()
    {
        // Arrange
        var jsonText = @"{
            'name': 'Jane Doe',
            'email': 'jane@email.com',
            'phone': '555-0123',
            'location': 'New York',
            'experience': [
                {
                    'company': 'Tech Corp',
                    'position': 'Developer',
                    'period': '2020-2023'
                }
            ],
            'skills': ['C#', '.NET', 'Azure']
        }";

        // Act
        var result = await _service.ExtractInformation(jsonText);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Smith"); // Verificamos que el mock sigue devolviendo los datos predefinidos
        result.Contact.Email.Should().Be("john.smith@email.com");
    }

    [Fact]
    public async Task ExtractInformation_VeryLongText_ThrowsArgumentException()
    {
        // Arrange
        var longText = new string('x', 1_000_000); // Texto de 1 millón de caracteres

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.ExtractInformation(longText));
    }
} 