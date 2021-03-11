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
