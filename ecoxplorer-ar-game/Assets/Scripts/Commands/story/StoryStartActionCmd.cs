using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public class StoryStartActionCmd : ICommand
    {
        private PlayerStoriesViewModel _story;
        private GameManagerViewModel _gameManagerViewModel;

        public StoryStartActionCmd(GameManagerViewModel gameManagerViewModel, PlayerStoriesViewModel story)
        {
            _gameManagerViewModel = gameManagerViewModel;
            _story = story;
        }

        public void Execute()
        {
            Debug.Log("Story was started");
            Debug.Log("Story Id: " + _story.story_id);
            Debug.Log("Story Audio: " + _story.audioClip);
            Debug.Log("Story Story: " + _story.story);
            Debug.Log("Story Image Analysis: " + _story.imageAnalysis);
            Debug.Log("Story Image: " + _story.image);
            Debug.Log("Story Image Id: " + _story.image_id);
            Debug.Log("Story Image Url: " + _story.image_url);
            
            _gameManagerViewModel.isLoadingPlay.Value = false;
            _gameManagerViewModel.isPlayingStory.Value = true;
            _gameManagerViewModel.actionIndications.Value = "...";
            _gameManagerViewModel.playerStories.Add(_story);
        }
    }
}
