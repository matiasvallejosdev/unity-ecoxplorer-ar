using System.Threading.Tasks;
using R3;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IStorageImageGateway
    {
        Task<Result<string>> UploadImageAndGetUrl(StorageServiceViewModel storageService, Texture2D texture);
    }
}
