using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;

namespace FirstResourceClient.net
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawned);
            EventHandlers.Add("sp:clientPlayerSpawned", new Action<float, float, float>(PlayerSpawned));
        }

        private void PlayerSpawned(float x, float y, float z)
        {
            SetEntityCoords(GetPlayerPed(-1), x, y, z, false, false, false, false);
        }

        private bool IsFirstSpawn = true;
        private void OnPlayerSpawned(dynamic spawnInfo)
        {
            if (IsFirstSpawn)
            {
                TriggerServerEvent("sp:serverPlayerSpawned", "useless");
                IsFirstSpawn = false;
            }
        }
    }
}
