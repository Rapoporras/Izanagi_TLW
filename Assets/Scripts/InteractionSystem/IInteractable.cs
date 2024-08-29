namespace InteractionSystem
{
    public interface IInteractable
    {
        void Interact(Interactor interactor);
        void ShowInteractionUI(bool showUI);
    }
}