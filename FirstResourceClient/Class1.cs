using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;

namespace FirstResourceClient.net
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawned);
            EventHandlers.Add("sp:clientPlayerSpawned", new Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(PlayerSpawned));
        }

        private readonly string[] WeaponNames = { "WEAPON_KNIFE", "WEAPON_NIGHTSTICK", "WEAPON_HAMMER", "WEAPON_BAT",
       "WEAPON_GOLFCLUB", "WEAPON_CROWBAR", "WEAPON_PISTOL", "WEAPON_COMBATPISTOL",
       "WEAPON_APPISTOL", "WEAPON_PISTOL50", "WEAPON_MICROSMG", "WEAPON_SMG", "WEAPON_ASSAULTSMG",
       "WEAPON_ASSAULTRIFLE", "WEAPON_CARBINERIFLE", "WEAPON_ADVANCEDRIFLE", "WEAPON_MG",
       "WEAPON_COMBATMG", "WEAPON_PUMPSHOTGUN", "WEAPON_SAWNOFFSHOTGUN", "WEAPON_ASSAULTSHOTGUN",
       "WEAPON_BULLPUPSHOTGUN", "WEAPON_STUNGUN", "WEAPON_SNIPERRIFLE", "WEAPON_HEAVYSNIPER",
       "WEAPON_GRENADELAUNCHER", "WEAPON_GRENADELAUNCHER_SMOKE", "WEAPON_RPG", "WEAPON_MINIGUN",
       "WEAPON_GRENADE", "WEAPON_STICKYBOMB", "WEAPON_SMOKEGRENADE", "WEAPON_BZGAS",
       "WEAPON_MOLOTOV", "WEAPON_FIREEXTINGUISHER", "WEAPON_PETROLCAN", "WEAPON_FLARE",
       "WEAPON_SNSPISTOL", "WEAPON_SPECIALCARBINE", "WEAPON_HEAVYPISTOL", "WEAPON_BULLPUPRIFLE",
       "WEAPON_HOMINGLAUNCHER", "WEAPON_PROXMINE", "WEAPON_SNOWBALL", "WEAPON_VINTAGEPISTOL",
       "WEAPON_DAGGER", "WEAPON_FIREWORK", "WEAPON_MUSKET", "WEAPON_MARKSMANRIFLE",
       "WEAPON_HEAVYSHOTGUN", "WEAPON_GUSENBERG", "WEAPON_HATCHET", "WEAPON_RAILGUN",
       "WEAPON_COMBATPDW", "WEAPON_KNUCKLE", "WEAPON_MARKSMANPISTOL", "WEAPON_FLASHLIGHT",
       "WEAPON_MACHETE", "WEAPON_MACHINEPISTOL", "WEAPON_SWITCHBLADE", "WEAPON_REVOLVER",
       "WEAPON_COMPACTRIFLE", "WEAPON_DBSHOTGUN", "WEAPON_FLAREGUN", "WEAPON_AUTOSHOTGUN",
       "WEAPON_BATTLEAXE", "WEAPON_COMPACTLAUNCHER", "WEAPON_MINISMG", "WEAPON_PIPEBOMB",
       "WEAPON_POOLCUE", "WEAPON_SWEEPER", "WEAPON_WRENCH" };

        private async void PlayerSpawned(dynamic x, dynamic y, dynamic z, dynamic heading, dynamic model, dynamic vehicle, dynamic currentWeapon)
        {
            //var ped = GetPlayerPed(-1);
            await Game.Player.ChangeModel(new Model(model));
            Game.Player.Money = 99999999;
            Game.PlayerPed.Position = new Vector3((float)x, (float)y, (float)z);
            if (heading != 0)
                Game.PlayerPed.Heading = (float)heading;

            await Delay(0);
            if (vehicle != 0)
            {
                var ve = await World.CreateVehicle(new Model((int)vehicle), new Vector3((float)x, (float)y, (float)z), (float)heading);
                Game.PlayerPed.SetIntoVehicle(ve, VehicleSeat.Driver);
            }

            //foreach (var weapon in WeaponNames)
            //{
            //    try
            //    {
            //        GiveWeaponToPed(ped, Convert.ToUInt32(GetHashKey(weapon)), 99999, false, false);
            //    }
            //    catch { }
            //}

            //SetCurrentPedWeapon(ped, (uint)currentWeapon, true);
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
