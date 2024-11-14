using System;
using System.Collections.Generic;
using Contracts;
using R3;
using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Menu Manager", menuName = "Manager/Menu Manager")]
    public class MenuManageViewModel : ScriptableObject
    {
        public ReactiveProperty<bool> isLoading = new(false);
        public ReactiveProperty<string> languageSelected = new("es");
        public ReactiveProperty<TopicDataViewModel> currentTopic = new();
    }
}
