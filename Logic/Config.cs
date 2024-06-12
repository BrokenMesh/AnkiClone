using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnkiClone.Logic
{
    public class Config
    {
        public static string FilePath = "./config.json";

        public string CardStore { get; set; }

        // The maximum card delay that will still be graded:
        // If a card is due in for example 5 Minutes it should still be checked!
        public int MaxCardDelayForDue_Min { get; set; }

        public Config(string cardStore, int maxCardDelayForDue_Min) {
            CardStore = cardStore;
            MaxCardDelayForDue_Min = maxCardDelayForDue_Min;
        }

        public Config() {
            CardStore = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AnkiClone";
            MaxCardDelayForDue_Min = 3;
        }

        public static Config LoadConfig() {
            Config _config = LoadOrStandartConfig();
            SaveConfig(_config);
            return _config;
        }

        private static Config LoadOrStandartConfig() {
            try {
                string _file = File.ReadAllText(FilePath);
                Config? _config = JsonSerializer.Deserialize<Config>(_file);
                if (_config == null) throw new Exception("Value of '_config' is null.");
                return _config;
            }
            catch (Exception _e) {
                Console.WriteLine("Error loading config: " + _e.Message);
                return new Config();
            }
        }

        public static void SaveConfig(Config _config) {

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

            string _file = JsonSerializer.Serialize<Config>(_config);
            File.WriteAllText(FilePath, _file);
        }

    }
}
