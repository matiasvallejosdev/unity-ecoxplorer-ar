using Infrastructure;
using UnityEngine;
using ViewModel;

namespace Contracts
{
    public class StoryLoadCmdDto
    {
        public GameManagerViewModel gameManagerViewModel { get; set; }
        public RecognitionServiceViewModel recognitionService { get; set; }
        public StorytellerViewModel storytellerViewModel { get; set; }
        public VoiceGeneratorViewModel voiceGeneratorViewModel { get; set; }
        public IImageRecognitionGateway ImageAnalysisGateway { get; set; }
        public IStorytellerGateway StorytellerGateway { get; set; }
        public IVoiceGeneratorGateway VoiceGeneratorGateway { get; set; }
        public IAudioGateway AudioGateway { get; set; }
    }
}
