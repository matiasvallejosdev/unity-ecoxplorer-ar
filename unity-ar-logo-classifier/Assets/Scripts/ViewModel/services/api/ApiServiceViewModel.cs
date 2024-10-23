using UnityEngine;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "ApiService", menuName = "Services/Api Service", order = 0)]
    public class ApiServiceViewModel : ScriptableObject 
    {
        public string API_URL;
        public string API_VERSION;
        public string API_KEY;
        public string API_KEY_HEADER;   
    }
}