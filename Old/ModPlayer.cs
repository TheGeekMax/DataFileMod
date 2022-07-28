using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using static Terraria.ModLoader.Mod;
using static Terraria.Item;
using static Terraria.Mount;

namespace DataFileMod
{

    public class dataFormated{
        public int life;
        public int maxLife;
        public int mana;
        public int maxMana;
        //public int armor;
        public ItemSave[] inventory;
        public ItemSave[] bank;
        public ItemSave[] safe;

        public int coins;
        /*
        public string[] accesory;
        public string[] armor;
        public string[] piggy;
        public string[] vault;
        public string[] void;
        public int money;
        */

        public dataFormated (){}
    }

    public class ItemSave{
        public int count;
        public string name;
    }

    public class FileDataPlayer : ModPlayer
    {
        public int frame = 0;

        private bool saveToFile(string data, string filename){
            try{
                FileStream stream = File.Open(Main.SavePath+"/"+filename, FileMode.Create);
                stream.Write(Encoding.ASCII.GetBytes(data));
            }catch(Exception e){
                
                return false;
            }
            return true;
        }

        private bool SaveToFile(){
            
                
                dataFormated data = new dataFormated();
                data.life = Player.statLife;
                data.maxLife = Player.statLifeMax;
                data.mana = Player.statMana;
                data.maxMana = Player.statManaMax2;
                data.coins = 0;

                data.inventory = new ItemSave[59];
                for(int i = 0; i < 59; i++){
                    data.inventory[i] = new ItemSave();
                    data.inventory[i].count = Player.inventory[i].stack;
                    data.inventory[i].name = Player.inventory[i].Name;
                    if(Player.inventory[i].Name == "Copper Coin"){
                        data.coins += Player.inventory[i].stack;
                    }else if(Player.inventory[i].Name == "Silver Coin"){
                        data.coins += Player.inventory[i].stack * 100;
                    }else if(Player.inventory[i].Name == "Gold Coin"){
                        data.coins += Player.inventory[i].stack * 10000;
                    }else if(Player.inventory[i].Name == "Platinum Coin"){
                        data.coins += Player.inventory[i].stack * 1000000;
                    }
                }

                data.bank = new ItemSave[40];
                for(int i = 0; i < 40; i++){
                    data.bank[i] = new ItemSave();
                    data.bank[i].count = Player.bank.item[i].stack;
                    data.bank[i].name = Player.bank.item[i].Name;
                    if(Player.bank.item[i].Name == "Copper Coin"){
                        data.coins += Player.bank.item[i].stack;
                    }else if(Player.bank.item[i].Name == "Silver Coin"){
                        data.coins += Player.bank.item[i].stack * 100;
                    }else if(Player.bank.item[i].Name == "Gold Coin"){
                        data.coins += Player.bank.item[i].stack * 10000;
                    }else if(Player.bank.item[i].Name == "Platinum Coin"){
                        data.coins += Player.bank.item[i].stack * 1000000;
                    }
                }

                data.safe = new ItemSave[40];
                for(int i = 0; i < 40; i++){
                    data.safe[i] = new ItemSave();
                    data.safe[i].count = Player.bank2.item[i].stack;
                    data.safe[i].name = Player.bank2.item[i].Name;
                    if(Player.bank2.item[i].Name == "Copper Coin"){
                        data.coins += Player.bank2.item[i].stack;
                    }else if(Player.bank2.item[i].Name == "Silver Coin"){
                        data.coins += Player.bank2.item[i].stack * 100;
                    }else if(Player.bank2.item[i].Name == "Gold Coin"){
                        data.coins += Player.bank2.item[i].stack * 10000;
                    }else if(Player.bank2.item[i].Name == "Platinum Coin"){
                        data.coins += Player.bank2.item[i].stack * 1000000;
                    }
                }
                
                bool error = false;
                
                string serializedData = JsonConvert.SerializeObject(data);
                if(!saveToFile(serializedData, "data.json")){
                    Main.NewText("unable to save data to file");
                    error = true;
                }
                /*
                string playerData = JsonConvert.SerializeObject(Player);
                if(!saveToFile(playerData, "player.json")){
                    Main.NewText("unable to save player to file");
                    error = true;
                }else{
                    Main.NewText("player saved to file");
                }
                */

                return !error;
        }

        public override void UpdateBadLifeRegen() {
            //execute tout les 5 secondes du code
            if(frame++ >= 60*5){
                frame = 0;
                //reccupère les pv et les imprime dans le chat
                int pv = Player.statLife;
                int maxpv = Player.statLifeMax2;
                SaveToFile();
                //Main.NewText("PV : " + pv + "/" + maxpv);
            }
        }
    }
}