using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FirstResourceClient.net
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawned);//绑定生成玩家事件
            EventHandlers["sp:clientPlayerSpawned"] += new Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic>(PlayerSpawned);//注册一个服务器可以调用的事件
            //Tick += Class1_Tick;//tick上委托一个显示玩家光标的事件
        }

        //private Dictionary<int, Blip> Blips = new Dictionary<int, Blip>();
        
        //private Task Class1_Tick()
        //{
        //    return new Task(() =>
        //    {
        //        foreach (var player in Players)
        //        {
        //            if (Game.Player.Equals(player))
        //                continue;

        //            Blip blip;
        //            if (Blips.TryGetValue(player.Handle, out blip))
        //            {
        //                blip.Position = player.Character.Position;
        //                blip.Rotation = (int)player.Character.Heading;
        //                SetSprite(player, blip);
        //                Blips[player.Handle] = blip;
        //            }
        //            else
        //            {
        //                blip = World.CreateBlip(player.Character.Position);
        //                blip.Name = player.Name;
        //                blip.IsFriend = blip.IsFriendly = true;
        //                blip.Rotation = (int)player.Character.Heading;
        //                SetBlipDisplay(blip.Handle, 10);
        //                SetSprite(player, blip);
        //                Blips.Add(player.Handle, blip);
        //            }
        //        }

        //        //delete offline players
        //        var expireHandles = Blips.Keys.ToList().Except(Players.Select(t => t.Handle)).ToList();
        //        expireHandles.ForEach(t => Blips[t].Delete()); ;
        //    });
        //}

        //private void SetSprite(Player player, Blip blip)
        //{
        //    if (player.Character.IsInVehicle())
        //    {
        //        switch (player.Character.CurrentVehicle.ClassType)
        //        {
        //            case VehicleClass.Boats:
        //                blip.Sprite = BlipSprite.Boat;
        //                break;
        //            case VehicleClass.Helicopters:
        //                blip.Sprite = BlipSprite.Helicopter;
        //                break;
        //            case VehicleClass.Planes:
        //                blip.Sprite = BlipSprite.Jet;
        //                break;
        //            default:
        //                blip.Sprite = BlipSprite.RaceCar;
        //                break;
        //        }
        //    }
        //    else
        //        blip.Sprite = BlipSprite.Player;
        //}

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
                if (weapon.GetHashCode() == currentWeapon)
                    Game.PlayerPed.Weapons.Select(weapon);
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
