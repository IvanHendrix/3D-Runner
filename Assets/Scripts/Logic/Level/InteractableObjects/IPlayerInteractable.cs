using System;
using Logic.Player;

namespace Logic.Level.InteractableObjects
{
    public interface IPlayerInteractable
    {
        event Action OnPlayerInteracted;
    }
}