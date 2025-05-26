using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using CVParserAPI.Services;
using CVParserAPI.Controllers;
using CVParserAPI.Models;
using CVParserAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CVParserAPI.Tests.Controllers;

public class ResumeControllerTests : TestBase
{
    private readonly Mock<ILogger<ResumeController>> _loggerMock;
    private readonly Mock<IOcrService> _ocrServiceMock;
    private readonly Mock<ILlmService> _llmServiceMock;
    private readonly ResumeController _controller;

    public ResumeControllerTests()
    {
        _loggerMock = new Mock<ILogger<ResumeController>>();
        _ocrServiceMock = new Mock<IOcrService>();
        _llmServiceMock = new Mock<ILlmService>();
        _controller = new ResumeController(_loggerMock.Object, _ocrServiceMock.Object, _llmServiceMock.Object);
    }

    [Theory]
    [InlineData("resume.pdf", "application/pdf")]
    [InlineData("resume.jpg", "image/jpeg")]
    [InlineData("resume.png", "image/png")]
    public async Task ProcessResume_ValidFileTypes_ReturnsOkResult(string fileName, string contentType)
    {
        // Arrange
        var mockFile = CreateMockFile(GetSampleResumeText(), fileName, contentType);
        var expectedText = GetSampleResumeText();
        var expectedData = new ResumeData
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
        };

        _ocrServiceMock.Setup(x => x.ExtractText(It.IsAny<IFormFile>()))
            .ReturnsAsync(expectedText);
        _llmServiceMock.Setup(x => x.ExtractInformation(expectedText))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _controller.ProcessResume(mockFile.Object);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var resumeData = okResult.Value.Should().BeOfType<ResumeData>().Subject;
        resumeData.Should().BeEquivalentTo(expectedData);
    }

    [Fact]
    public async Task ProcessResume_EmptyFile_ReturnsBadRequest()
    {
        // Arrange
        var mockFile = CreateEmptyFile("empty.pdf", "application/pdf");

        // Act
        var result = await _controller.ProcessResume(mockFile.Object);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("The file is empty");
    }

    [Fact]
    public async Task ProcessResume_NullFile_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.ProcessResume(null);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task ProcessResume_OcrServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var mockFile = CreateMockFile(GetSampleResumeText());
        _ocrServiceMock.Setup(x => x.ExtractText(It.IsAny<IFormFile>()))
            .ThrowsAsync(new Exception("OCR service error"));

        // Act
        var result = await _controller.ProcessResume(mockFile.Object);

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task ProcessResume_UnsupportedFileType_ReturnsBadRequest()
    {
        // Arrange
        var mockFile = CreateMockFile(GetSampleResumeText(), "resume.txt", "text/plain");

        // Act
        var result = await _controller.ProcessResume(mockFile.Object);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Unsupported file type. Only PDF and image files are allowed.");
    }
} 