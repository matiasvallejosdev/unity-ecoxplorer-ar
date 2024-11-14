using Infrastructure;
using UnityEngine;
using ViewModel;

namespace Contracts
{
    public class SessionStartDto
    {
        public GameManagerViewModel gameManagerViewModel { get; set; }
        public MenuManageViewModel menuManageViewModel { get; set; }
        public LevelBuilderServiceViewModel levelBuilderServiceViewModel { get; set; }
        public ILevelBuilderGateway levelBuilderGateway { get; set; }
    }
}
