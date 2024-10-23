using System;
using System.Collections.Generic;

public class StorytellerGptResponse
{
    public string language { get; set; }
    public string narrator { get; set; }
    public string primary_subject { get; set; }
    public List<string> climate_concepts { get; set; }
    public string title { get; set; }
    public string story { get; set; }
    public string call_to_action { get; set; }
    public string story_id { get; set; }
    public string image_recognition_output { get; set; }
    public string image_id { get; set; }
    public string image_url { get; set; }
    public DateTime date_created { get; set; }
}
