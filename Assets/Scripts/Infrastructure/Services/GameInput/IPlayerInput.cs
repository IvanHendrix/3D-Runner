namespace Infrastructure.Services.GameInput
{
    public interface IPlayerInput
    {
        void UpdateInput();
        bool TryGetStrafe(out int direction); // -1 = left, 1 = right
        bool TryGetJump(); // true if tap
    }
}