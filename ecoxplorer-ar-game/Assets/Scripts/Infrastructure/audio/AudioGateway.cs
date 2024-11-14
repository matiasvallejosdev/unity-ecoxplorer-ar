
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure
{
    public class AudioGateway : IAudioGateway
    {
        public async Task<Result<AudioClip>> DownloadAudio(VoiceGeneratorResponse audioResult)
        {
            try
            {
                var audioUrl = audioResult.voice_story_output;
                using var www = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.UNKNOWN);
                await www.SendWebRequestAsync();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to download audio clip: {www.error}");
                    return Result<AudioClip>.Failure(new Exception(www.error));
                }

                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                if (audioClip == null)
                {
                    Debug.LogError("Downloaded audio clip is null");
                    return Result<AudioClip>.Failure(new Exception("Downloaded audio clip is null"));
                }

                return Result<AudioClip>.Success(audioClip);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error downloading audio: {ex.Message}");
                return Result<AudioClip>.Failure(ex);
            }
        }
    }
}