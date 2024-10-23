using System.Threading.Tasks;
using R3;
using UnityEngine;
using ViewModel;


namespace Infrastructure
{
    public interface IAudioGateway
    {
        Task<Result<AudioClip>> DownloadAudio(VoiceGeneratorResponse audioResult);
    }
}
