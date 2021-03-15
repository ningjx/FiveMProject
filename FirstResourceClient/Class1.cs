using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstResourceClient.net
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawned);
            EventHandlers.Add("sp:clientPlayerSpawned", new Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(PlayerSpawned));
            Tick += Class1_Tick;
        }

        private List<Blip> Blips = new List<Blip>();

        private Task Class1_Tick()
        {
            return new Task(() =>
            {
                Blips.ForEach(t => t.Delete());
                Blips.Clear();

                foreach (var player in Players)
                {
                    if (Game.Player.Equals(player))
                        continue;

                    var blip = World.CreateBlip(player.Character.Position);
                    blip.Name = player.Name;
                    blip.Rotation = (int)player.Character.Heading;
                    blip.IsFriend = blip.IsFriendly = true;

                    if (player.Character.IsInVehicle())
                    {
                        switch (player.Character.CurrentVehicle.ClassType)
                        {
                            case VehicleClass.Boats:
                                blip.Sprite = BlipSprite.Boat;
                                break;
                            case VehicleClass.Helicopters:
                                blip.Sprite = BlipSprite.Helicopter;
                                break;
                            case VehicleClass.Planes:
                                blip.Sprite = BlipSprite.Jet;
                                break;
                            default:
                                blip.Sprite = BlipSprite.RaceCar;
                                break;
                        }
                    }
                    else
                        blip.Sprite = BlipSprite.Player;

                    Blips.Add(blip);
                }
            });
        }

        private async void PlayerSpawned(dynamic x, dynamic y, dynamic z, dynamic heading, dynamic model, dynamic vehicle, dynamic currentWeapon)
        {
            Game.Player.ChangeModel(new Model(model));
            Game.PlayerPed.Position = new Vector3((float)x, (float)y, (float)z);
            Game.PlayerPed.Heading = (float)heading;
            await Delay(0);
            if (vehicle != 0)
            {
                var ve = await World.CreateVehicle(new Model((int)vehicle), new Vector3((float)x, (float)y, (float)z), (float)heading);
                Game.PlayerPed.SetIntoVehicle(ve, VehicleSeat.Driver);
            }

            var allWeapons = Enum.GetValues(typeof(WeaponHash));
            foreach (WeaponHash weapon in allWeapons)
            {
                Game.PlayerPed.Weapons.Give(weapon, 1000, false, true);
                //Game.PlayerPed.Weapons.Select((uint)currentWeapon);
            }
            //SetCurrentPedWeapon(Game.Player.Handle, (uint)currentWeapon, true);
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
