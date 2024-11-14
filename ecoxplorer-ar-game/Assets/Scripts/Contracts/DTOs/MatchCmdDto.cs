using Infrastructure;
using UnityEngine;
using ViewModel;

namespace Contracts
{
    public class MatchCmdDto
    {
        public Camera arCamera { get; set; }
        public GameManagerViewModel gameManagerViewModel { get; set; }
        public StorageServiceViewModel storageService { get; set; }
        public MatchServiceViewModel matchService;

        public IStorageImageGateway StorageImageGateway { get; set; }
        public IMatchGateway MatchGateway { get; set; }
    }
}
