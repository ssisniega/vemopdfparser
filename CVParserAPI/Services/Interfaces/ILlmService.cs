using System.Threading.Tasks;
using CVParserAPI.Models;

namespace CVParserAPI.Services.Interfaces;

public interface ILlmService
{
    Task<ResumeData> ExtractInformation(string text);
} 