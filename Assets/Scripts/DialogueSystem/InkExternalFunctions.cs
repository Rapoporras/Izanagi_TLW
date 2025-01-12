using System;
using Ink.Runtime;

namespace DialogueSystem
{
    public static class DialogueEvents
    {
        public static Action wallEvent;
        public static Action finalSceneEvent;
    }
    
    public class InkExternalFunctions
    {
        private const string ActivateWallEventFuncName = "activateWallEvent";
        private const string FinalSceneTransitionFuncName = "finalSceneTransition";
        
        public void Bind(Story story)
        {
            story.BindExternalFunction(ActivateWallEventFuncName, () =>
            {
                DialogueEvents.wallEvent?.Invoke();
            });
            story.BindExternalFunction(FinalSceneTransitionFuncName, () =>
            {
                DialogueEvents.finalSceneEvent?.Invoke();
            });
        }

        public void Unbind(Story story)
        {
            story.UnbindExternalFunction(ActivateWallEventFuncName);
            story.UnbindExternalFunction(FinalSceneTransitionFuncName);
        }
    }
}