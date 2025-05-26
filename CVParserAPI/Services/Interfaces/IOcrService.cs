using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CVParserAPI.Services.Interfaces;

public interface IOcrService
{
    Task<string> ExtractText(IFormFile file);
} 