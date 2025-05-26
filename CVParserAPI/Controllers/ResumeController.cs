using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CVParserAPI.Services.Interfaces;
using CVParserAPI.Models;
using Microsoft.Extensions.Logging;

namespace CVParserAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ResumeController : ControllerBase
{
    private readonly ILogger<ResumeController> _logger;
    private readonly IOcrService _ocrService;
    private readonly ILlmService _llmService;
    private readonly string[] _allowedContentTypes = new[] { "application/pdf", "image/jpeg", "image/png" };

    public ResumeController(
        ILogger<ResumeController> logger,
        IOcrService ocrService,
        ILlmService llmService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ocrService = ocrService ?? throw new ArgumentNullException(nameof(ocrService));
        _llmService = llmService ?? throw new ArgumentNullException(nameof(llmService));
    }

    [HttpPost]
    public async Task<IActionResult> ProcessResume(IFormFile file)
    {
        try
        {
            if (file == null)
                return BadRequest("No file was uploaded");

            if (file.Length == 0)
                return BadRequest("The file is empty");

            if (!_allowedContentTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Unsupported file type. Only PDF and image files are allowed.");

            var extractedText = await _ocrService.ExtractText(file);
            var resumeData = await _llmService.ExtractInformation(extractedText);

            return Ok(resumeData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing resume");
            return StatusCode(500, "An error occurred while processing the resume");
        }
    }
} 