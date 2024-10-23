using System.Threading.Tasks;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest request)
    {
        var tcs = new TaskCompletionSource<UnityWebRequest>();
        var operation = request.SendWebRequest();

        operation.completed += _ =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                tcs.SetResult(request);
            }
            else
            {
                tcs.SetException(new UnityWebRequestException(request.error));
            }
        };

        return tcs.Task;
    }

    public class UnityWebRequestException : System.Exception
    {
        public UnityWebRequestException(string message) : base(message) { }
    }
}
