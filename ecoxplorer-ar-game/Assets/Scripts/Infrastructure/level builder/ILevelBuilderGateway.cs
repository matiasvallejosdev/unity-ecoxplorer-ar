using System.Threading.Tasks;
using R3;
using ViewModel;

namespace Infrastructure
{
    public interface ILevelBuilderGateway
    {
        Task<Result<LevelBuilderResponse>> GetLevelBuilder(LevelBuilderServiceViewModel levelBuilderServiceViewModel, string topic, string language);
    }
}