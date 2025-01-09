using System;
using Ink.Runtime;

namespace DialogueSystem
{
    public static class DialogueEvents
    {
        public static Action wallEvent;
    }
    
    public class InkExternalFunctions
    {
        private const string ActivateWallEventFuncName = "activateWallEvent";
        
        public void Bind(Story story)
        {
            story.BindExternalFunction(ActivateWallEventFuncName, () =>
            {
                DialogueEvents.wallEvent?.Invoke();
            });
        }

        public void Unbind(Story story)
        {
            story.UnbindExternalFunction(ActivateWallEventFuncName);
        }
    }
}