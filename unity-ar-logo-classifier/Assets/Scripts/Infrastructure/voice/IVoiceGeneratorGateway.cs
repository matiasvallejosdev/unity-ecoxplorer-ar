using System.Threading.Tasks;
using R3;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IVoiceGeneratorGateway
    {
        Task<Result<VoiceGeneratorResponse>> GenerateVoice(VoiceGeneratorViewModel voiceGeneratorViewModel, string story, string story_id, string narrator);
    }
}
