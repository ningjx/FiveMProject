using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static CitizenFX.Core.Native.API;

namespace FirstResourceServer.net
{
    public class Class1 : BaseScript
    {
        private const string Path = "PlayerData.json";
        public Class1()
        {
            EventHandlers.Add("sp:serverPlayerSpawned", new Action<Player, string>(PlayerSpawned));
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }

        private async void PlayerSpawned([FromSource] Player player, string useless)
        {
            await Delay(0);
            if (File.Exists(Path))
            {
                var text = File.ReadAllText(Path);
                if (!string.IsNullOrEmpty(text))
                {
                    var datas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(text);
                    var licenseIdentifier = player.Identifiers["license"];
                    if (datas.TryGetValue(licenseIdentifier, out Dictionary<string, object> data))
                    {
                        object[] arg = new object[]
                        {
                            data["X"],
                            data["Y"],
                            data["Z"],
                            data["Heading"],
                            data["Model"],
                            data["Vehicle"],
                            data["Waepon"]
                        };
                        TriggerClientEvent("sp:clientPlayerSpawned", arg);
                    }
                }
            }
        }

        private async void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var ped = GetPlayerPed(player.Handle);
            var playerData = new Dictionary<string, object>
            {
                {"X",player.Character.Position.X},
                {"Y",player.Character.Position.Y},
                {"Z",player.Character.Position.Z},
                {"Heading", player.Character.Heading },
                {"Model",player.Character.Model },
                {"Vehicle", GetEntityModel(GetVehiclePedIsIn(ped, true))},
                {"Waepon",GetSelectedPedWeapon(ped)}
            };

            Debug.WriteLine(JsonConvert.SerializeObject(player.Identifiers));
            Debug.WriteLine(JsonConvert.SerializeObject(player.State));

            var licenseIdentifier = player.Identifiers["license"];

            await Delay(0);
            var datas = new Dictionary<string, Dictionary<string, object>>();
            if (File.Exists(Path))
            {
                var text = File.ReadAllText(Path);
                if (!string.IsNullOrEmpty(text))
                    datas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(text);

                if (datas.ContainsKey(licenseIdentifier))
                    datas[licenseIdentifier] = playerData;
                else
                    datas.Add(licenseIdentifier, playerData);
            }
            else
            {
                datas.Add(licenseIdentifier, playerData);
            }
            File.WriteAllText(Path, JsonConvert.SerializeObject(datas));
        }
    }
}
