using UnityEngine;

namespace ViewModel
{    
    [CreateAssetMenu(fileName = "New ARObject", menuName = "AR/ARObject")]
    public class ARObjectViewModel : ScriptableObject 
    {
        public string objectLabel;
        public GameObject objectPrefab;
    }
}