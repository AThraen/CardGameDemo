using CardGameLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CardGameWeb.Business
{
    //Responsible for loading and saving games
    public class GameService
    {
        private readonly string AppDataPath;

        public GameService()
        {
            AppDataPath= Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home");
        }

        private string GetFilePath(int GameId)
        {
            return Path.Combine(AppDataPath, GameId.ToString() + ".json");
        }

        public void SaveGame(Game g)
        {
            string filename = GetFilePath(g.GameId);
            File.WriteAllText(filename, g.SerializeGame());
        }


        public Game LoadGame(int id)
        {
            return Game.DeserializeGame(File.ReadAllText(GetFilePath(id)));
        }

        public void DeleteGame(int id)
        {
            File.Delete(GetFilePath(id));
        }

        public bool GameExist(int id)
        {
            return File.Exists(GetFilePath(id));
        }
    }
}
