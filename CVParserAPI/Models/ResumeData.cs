using System.Collections.Generic;

namespace CVParserAPI.Models;

public class ResumeData
{
    public string Name { get; set; } = string.Empty;
    public ContactInfo Contact { get; set; } = new();
    public List<Experience> WorkExperience { get; set; } = new();
    public List<string> Skills { get; set; } = new();
} 