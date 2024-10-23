using System.Collections.Generic;

public class EnvironmentalImageAnalysisResponse
{
    public string image_id { get; set; }
    public string primary_subject { get; set; }
    public string environment_type { get; set; }
    public List<string> climate_indicators { get; set; }
    public Biodiversity biodiversity { get; set; }
    public List<string> human_elements { get; set; }
    public string weather_conditions { get; set; }
    public List<string> color_palette { get; set; }
    public string emotional_tone { get; set; }
    public List<string> educational_themes { get; set; }
    public List<string> storytelling_elements { get; set; }
    public double confidence_score { get; set; }
    public string analysis_notes { get; set; }
    public string image_url { get; set; }
    public string date_created { get; set; }
}

public class Biodiversity
{
    public List<string> flora { get; set; }
    public List<string> fauna { get; set; }
}