using System.Threading.Tasks;
using R3;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IStorytellerGateway
    {
        Task<Result<StorytellerGptResponse>> GenerateStory(StorytellerViewModel storytellerViewModel, EnvironmentalImageAnalysisResponse imageAnalysisResponse, string language="es");
    }
}
