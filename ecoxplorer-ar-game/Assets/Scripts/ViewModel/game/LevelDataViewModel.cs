using System.Collections.Generic;
using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Game/Level Data")]
    public class LevelDataViewModel : ScriptableObject
    {
        [Header("Level Information")]
        public string levelTitle;
        public string levelStoryIntroduction;
        public string levelStoryEnd;
        public string narratorName;
        public string primarySubject;

        [Header("Guidance")]
        public List<string> levelGuidance;
    }
}
