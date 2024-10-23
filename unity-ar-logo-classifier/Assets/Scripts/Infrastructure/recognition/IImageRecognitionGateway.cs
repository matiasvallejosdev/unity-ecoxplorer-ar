using System;
using System.Threading.Tasks;
using R3;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IImageRecognitionGateway 
    {
        public Task<Result<EnvironmentalImageAnalysisResponse>> GetAnalysis(RecognitionServiceViewModel recognitionService, string imageUrl);
    }
}