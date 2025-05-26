using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using CVParserAPI.Services;
using CVParserAPI.Services.Interfaces;
using System.IO;

namespace CVParserAPI.Tests.Services;

public class MockOcrServiceTests : TestBase
{
    private readonly Mock<ILogger<MockOcrService>> _loggerMock;
    private readonly MockOcrService _service;

    public MockOcrServiceTests()
    {
        _loggerMock = new Mock<ILogger<MockOcrService>>();
        _service = new MockOcrService(_loggerMock.Object);
    }

    [Fact]
    public async Task ExtractText_ValidPdf_ReturnsText()
    {
        // Arrange
        var mockFile = CreateMockFile(GetSampleResumeText());

        // Act
        var result = await _service.ExtractText(mockFile.Object);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("John Smith");
        result.Should().Contain("Full Stack Developer");
    }

    [Fact]
    public async Task ExtractText_NullFile_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.ExtractText(null));
    }

    [Fact]
    public async Task ExtractText_EmptyFile_ReturnsEmptyString()
    {
        // Arrange
        var mockFile = CreateMockFile("");

        // Act
        var result = await _service.ExtractText(mockFile.Object);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExtractText_StreamThrowsException_ThrowsIOException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100); // Archivo no vacío
        mockFile.Setup(f => f.OpenReadStream())
            .Throws(new IOException("Error reading file stream"));

        // Act & Assert
        await Assert.ThrowsAsync<IOException>(() => _service.ExtractText(mockFile.Object));
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<IOException>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
} 