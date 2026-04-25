using System;
using DefaultNamespace.Systems.Base;
using DefaultNamespace.Systems.Data;
using Systems.Dungeon.Data;

namespace DefaultNamespace.Systems
{
    public interface IPlayerManager
    {
        PlayerData Player { get; }
        void InitializePlayer(PlayerData player);
        event Action<PlayerData> PlayerChanged;
    }

    public class PlayerManager : GameSystem, IPlayerManager
    {
        public event Action<PlayerData> PlayerChanged;

        public PlayerData Player { get; private set; }

        public void InitializePlayer(PlayerData player)
        {
            if (Player != null)
                throw new System.Exception("Player is already initialized");

            Player = player;
            Player.Changed += OnPlayerChanged;

            PlayerChanged?.Invoke(Player);
        }

        private void OnPlayerChanged()
        {
            PlayerChanged?.Invoke(Player);
        }
    }
}