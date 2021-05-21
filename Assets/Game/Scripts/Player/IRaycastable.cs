namespace Project.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerCamera callingController);
    }
}