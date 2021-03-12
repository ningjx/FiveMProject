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
        public Class1()
        {
            EventHandlers.Add("sp:serverPlayerSpawned", new Action<Player, string>(PlayerSpawned));
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }

        private async void PlayerSpawned([FromSource] Player player, string useless)
        {
            await Delay(0);
            if (File.Exists("position.json"))
            {
                var data = File.ReadAllText("position.json");
                if (!string.IsNullOrEmpty(data))
                {
                    var positions = JsonConvert.DeserializeObject<Dictionary<string, Tuple<float, float, float>>>(data);
                    var licenseIdentifier = player.Identifiers["license"];
                    if (positions.TryGetValue(licenseIdentifier, out Tuple<float, float, float> position))
                    {
                        TriggerClientEvent("sp:clientPlayerSpawned", position.Item1, position.Item2, position.Item3);
                    }
                }
            }
        }

        private async void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var position = new Tuple<float, float, float>(player.Character.Position.X, player.Character.Position.Y, player.Character.Position.Z);
            var licenseIdentifier = player.Identifiers["license"];
            var positions = new Dictionary<string, Tuple<float, float, float>>();
            await Delay(0);
            if (File.Exists("position.json"))
            {
                //position.Z += 1;
                var data = File.ReadAllText("position.json");
                if (!string.IsNullOrEmpty(data))
                    positions = JsonConvert.DeserializeObject<Dictionary<string, Tuple<float, float, float>>>(data);

                if (positions.ContainsKey(licenseIdentifier))
                    positions[licenseIdentifier] = position;
                else
                    positions.Add(licenseIdentifier, position);
            }
            else
            {
                positions.Add(licenseIdentifier, position);
            }
            File.WriteAllText("position.json", JsonConvert.SerializeObject(positions));
        }
    }
}
