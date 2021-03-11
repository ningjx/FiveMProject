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
            try
            {
                if (File.Exists("position.json"))
                {
                    var data = File.ReadAllText("position.json");
                    if (!string.IsNullOrEmpty(data))
                    {
                        var positions = JsonConvert.DeserializeObject<Dictionary<string, Vector3>>(data);
                        var licenseIdentifier = player.Identifiers["license"];
                        if (positions.TryGetValue(licenseIdentifier, out Vector3 position))
                        {
                            player.Character.Position = position;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private async void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var position = player.Character.Position;
            var licenseIdentifier = player.Identifiers["license"];
            Dictionary<string, Vector3> positions = new Dictionary<string, Vector3>();
            await Delay(0);
            if (File.Exists("position.json"))
            {
                position.Z += 1;
                var data = File.ReadAllText("position.json");
                if (!string.IsNullOrEmpty(data))
                    positions = JsonConvert.DeserializeObject<Dictionary<string, Vector3>>(data);

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
