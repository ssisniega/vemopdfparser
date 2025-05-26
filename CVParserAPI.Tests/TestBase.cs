using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace CVParserAPI.Tests;

public abstract class TestBase
{
    protected Mock<IFormFile> CreateMockFile(string content, string fileName = "test.pdf", string contentType = "application/pdf")
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var file = new Mock<IFormFile>();
        var ms = new MemoryStream(bytes);

        file.Setup(f => f.OpenReadStream()).Returns(ms);
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.Length).Returns(bytes.Length);
        file.Setup(f => f.ContentType).Returns(contentType);

        return file;
    }

    protected Mock<IFormFile> CreateEmptyFile(string fileName = "empty.pdf", string contentType = "application/pdf")
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.Length).Returns(0);
        file.Setup(f => f.ContentType).Returns(contentType);
        return file;
    }

    protected string GetSampleResumeText()
    {
        return @"John Smith
        Software Developer
        San Francisco, CA
        john.smith@email.com | +1234567890

        Experience:
        Tech Solutions Inc (2019-2024)
        Full Stack Developer
        - Developed scalable web applications
        - Led team of 3 developers
        - Implemented CI/CD pipelines

        Skills:
        JavaScript, Python, Docker, AWS, React";
    }
} 