using System.Threading.Tasks;
using R3;
using ViewModel;

namespace Infrastructure
{
    public interface IMatchGateway
    {
        Task<Result<ImageMatchResponse>> GetMatch(MatchServiceViewModel matchServiceViewModel, string topic, string url);
    }
}