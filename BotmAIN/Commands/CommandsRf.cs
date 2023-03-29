using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;
using System.Linq;
using OxyPlot.Core.Drawing;
using OxyPlot.Legends;
using OxyPlot.Annotations;
using System.Net.Configuration;

namespace BotmAIN.Commands
{
    public class CommandsRf : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommand(CommandContext ctx)
        {
            string Stroka = "";
            /* string H = ":green_square:";
             string O = ":white_large_square:";*/
            string H = ":green_square:";
            string O = ":black_large_square:";
            string[,] map ={
                          { H, H, H, H, H, H, H, H, H},
                          { H, O, O, O, O, O, O, O, H},
                          { H, O, H, O, H, H, O, O, H},
                          { H, O, H, O, O, H, O, O, H},
                          { H, O, H, O, O, H, O, O, H},
                          { H, O, H, O, H, H, O, O, H},
                          { H, O, H, O, O, O, O, O, H},
                          { H, O, H, O, H, H, H, O, H},
                          { H, O, O, O, O, O, O, O, H},
                          { H, H, H, H, H, H, H, H, H},
            };


            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Stroka += map[i, j];

                }
                Stroka += "\n";
            }
            await ctx.Channel.SendMessageAsync(Stroka);



        }

        private Dictionary<string, string> godsIcon = new Dictionary<string, string>();
        [Command("top")]
        public async Task Top500(CommandContext ctx, string name)
        {
            Emoji[] emojj =
            {
                new Emoji('0', ":zero:"),
                new Emoji('1', ":one:"),
                new Emoji('2', ":two:"),
                new Emoji('3', ":three:"),
                new Emoji('4', ":four:"),
                new Emoji('5', ":five:"),
                new Emoji('6', ":six:"),
                new Emoji('7', ":seven:"),
                new Emoji('8', ":eight:"),
                new Emoji('9', ":nine:"),
            };

            Icon[] godsIcon =
          {
                new Icon("AMATERASU", "https://webcdn.hirezstudios.com/dko/assets/Avatar_AMATERASU_Mastery.jpg"),
                new Icon("ARTHUR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ARTHUR_Mastery.jpg"),
                new Icon("ATHENA", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ATHENA_Mastery.jpg"),
                new Icon("HERCULES", "https://webcdn.hirezstudios.com/dko/assets/Avatar_HERCULES_Mastery.jpg"),
                new Icon("IZANAMI", "https://webcdn.hirezstudios.com/dko/assets/Avatar_IZANAMI_Mastery.jpg"),
                new Icon("LOKI", "https://webcdn.hirezstudios.com/dko/assets/Avatar_LOKI_Mastery.jpg"),
                new Icon("SOL", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SOL_Mastery.jpg"),
                new Icon("SUNWUKONG", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SUNWUKONG_Mastery.jpg"),
                new Icon("SUSANO", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SUSANO_Mastery.jpg"),
                new Icon("THANATOS", "https://webcdn.hirezstudios.com/dko/assets/Avatar_THANATOS_Mastery.jpg"),
                new Icon("THOR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_THOR_Mastery.jpg"),
                new Icon("YMIR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_YMIR_Mastery.jpg"),
                new Icon("ZEUS", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ZEUS_Mastery.jpg"),
            };



            string input = null;

            using (WebClient client = new WebClient())
                input = client.DownloadString("https://api2.hirezstudios.com/stats/leaderboard");

            input = input.Substring(input.IndexOf("[{"), input.IndexOf("}]") + 1 - input.IndexOf("[{")) + "]";
            var jsonData = JsonConvert.DeserializeObject<List<PlayerData>>(input);

            var y = jsonData.FindAll(d => d.match_queue == "1v1_Duel_Competitive");
            var z = jsonData.FindAll(d => d.match_queue == "2v2 Brawl");


            var player2 = z.Find((a) => a.player_name == name);

            var player = y.Find((a) => a.player_name == name);



            string topEmoji = "";

            var SavedData = new SavedData(DateTime.Now, jsonData);

            if (DateTime.Now.Hour < 14)
            {
                if (File.Exists($"Data/{DateTime.Today.AddDays(-1).ToString("d")}.json"))
                {

                }
                else
                    File.WriteAllText($"Data/{DateTime.Today.AddDays(-1).ToString("d")}.json", JsonConvert.SerializeObject(jsonData));
            }
            else
            {
                if (File.Exists($"Data/{DateTime.Today.ToString("d")}.json"))
                {

                }
                else
                    File.WriteAllText($"Data/{DateTime.Today.ToString("d")}.json", JsonConvert.SerializeObject(jsonData));
            }



            if (player != null)
            {
                if (player.god_2 == null)
                    player.god_2 = "";
                if (player.god_3 == null)
                    player.god_3 = "";
                string text = player.ranking.ToString();
                topEmoji = "";
                NumberToEmoji(text);
                var embed = new DiscordEmbedBuilder();
                embed.WithTitle(":one::regional_indicator_v::regional_indicator_s::one:");
                embed.AddField("Ник: ", name + ".");
                embed.AddField("TOP: ", topEmoji + ".");
                embed.AddField("Gods: ", player.god_1.ToString() + " " + player.god_2.ToString() + " " + player.god_3.ToString());

                for (int i = 0; i < godsIcon.Length; i++)
                {
                    if (godsIcon[i].Name == player.god_1)
                    {
                        embed.ImageUrl = godsIcon[i].Url;

                        break;
                    }

                }
                await ctx.Channel.SendMessageAsync("", embed.Build());
            }
            if (player2 != null)
            {
                if (player2.god_2 == null)
                    player2.god_2 = "";
                if (player2.god_3 == null)
                    player2.god_3 = "";
                string text2 = player2.ranking.ToString();
                topEmoji = "";
                NumberToEmoji(text2);
                var embed2 = new DiscordEmbedBuilder();
                embed2.WithTitle(":two::regional_indicator_v::regional_indicator_s::two:");
                embed2.AddField("Ник: ", name + ".");
                embed2.AddField("TOP: ", topEmoji + ".");
                embed2.AddField("Gods: ", player2.god_1.ToString() + ", " + player2.god_2.ToString() + ", " + player2.god_3.ToString() + ".");

                for (int i = 0; i < godsIcon.Length; i++)
                {
                    if (godsIcon[i].Name == player2.god_1)
                    {
                        embed2.ImageUrl = godsIcon[i].Url;

                        break;
                    }

                }
                await ctx.Channel.SendMessageAsync("", embed2.Build());

            }

            string NumberToEmoji(string txt)
            {
                string topEmoj = "";
                int b = 0;
                for (int i = 0; i < emojj.Length && b < txt.Length; i++)
                {
                    if (emojj[i].number == txt[b])
                    {

                        b += 1;
                        topEmoji += emojj[i].emoji;
                        i = -1;
                    }
                }
                return topEmoj;

            }
        }

        [Command("PR")]
        public async Task Stats(CommandContext ctx)
        {
            int GodsAll = 0;
            Icon[] godsIcon =
         {
                new Icon("AMATERASU", "https://webcdn.hirezstudios.com/dko/assets/Avatar_AMATERASU_Mastery.jpg"),
                new Icon("ARTHUR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ARTHUR_Mastery.jpg"),
                new Icon("ATHENA", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ATHENA_Mastery.jpg"),
                new Icon("HERCULES", "https://webcdn.hirezstudios.com/dko/assets/Avatar_HERCULES_Mastery.jpg"),
                new Icon("IZANAMI", "https://webcdn.hirezstudios.com/dko/assets/Avatar_IZANAMI_Mastery.jpg"),
                new Icon("LOKI", "https://webcdn.hirezstudios.com/dko/assets/Avatar_LOKI_Mastery.jpg"),
                new Icon("SOL", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SOL_Mastery.jpg"),
                new Icon("SUNWUKONG", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SUNWUKONG_Mastery.jpg"),
                new Icon("SUSANO", "https://webcdn.hirezstudios.com/dko/assets/Avatar_SUSANO_Mastery.jpg"),
                new Icon("THANATOS", "https://webcdn.hirezstudios.com/dko/assets/Avatar_THANATOS_Mastery.jpg"),
                new Icon("THOR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_THOR_Mastery.jpg"),
                new Icon("YMIR", "https://webcdn.hirezstudios.com/dko/assets/Avatar_YMIR_Mastery.jpg"),
                new Icon("ZEUS", "https://webcdn.hirezstudios.com/dko/assets/Avatar_ZEUS_Mastery.jpg"),
            };
            string input = null;

            using (WebClient client = new WebClient())
                input = client.DownloadString("https://api2.hirezstudios.com/stats/leaderboard");

            input = input.Substring(input.IndexOf("[{"), input.IndexOf("}]") + 1 - input.IndexOf("[{")) + "]";
            var jsonData = JsonConvert.DeserializeObject<List<PlayerData>>(input);

            var y = jsonData.FindAll(d => d.match_queue == "1v1_Duel_Competitive");

            DateTime dateTime = new DateTime();
            Console.WriteLine(DateTime.Now);



            for (int i = 0; i < y.Count; i++)
            {
                for (int s = 0; s < godsIcon.Length; s++)
                {
                    if (y[i].god_1 == godsIcon[s].Name || y[i].god_2 == godsIcon[s].Name || y[i].god_3 == godsIcon[s].Name)
                    {
                        godsIcon[s].Kol += 1;
                    }
                }

            }
            for (int i = 0; i < godsIcon.Length; i++)
            {
                GodsAll += godsIcon[i].Kol;
                Console.WriteLine(godsIcon[i].Name + " " + godsIcon[i].Kol);

            }

            Console.WriteLine(GodsAll);
            for (int i = 0; i < godsIcon.Length; i++)
            {
                godsIcon[i].PickRate = godsIcon[i].Kol / 5;

            }
            for (int i = 0; i < godsIcon.Length; i++)
            {
                Console.WriteLine(godsIcon[i].PickRate);
            }
            Icon tmp = null;
            var embed3 = new DiscordEmbedBuilder();
            embed3.WithTitle(":regional_indicator_p::regional_indicator_i::regional_indicator_c::regional_indicator_k:  :regional_indicator_r::regional_indicator_a::regional_indicator_t::regional_indicator_e:");

            for (int i = 0; i < godsIcon.Length; i++)
                for (int j = i + 1; j < godsIcon.Length; j++)
                    if (godsIcon[i].PickRate < godsIcon[j].PickRate)
                    {
                        tmp = godsIcon[j];
                        godsIcon[j] = godsIcon[i];
                        godsIcon[i] = tmp;
                    }
            for (int i = 0; i < godsIcon.Length; i++)
            {
                int Num = 1 + i;
                embed3.AddField(Num + ". " + godsIcon[i].Name, godsIcon[i].PickRate + "%");
            }
            await ctx.Channel.SendMessageAsync("", embed3.Build());


            var minDate = DateTime.Now.AddDays(-7);
            var model = new PlotModel() { Background = OxyColor.FromRgb(255, 255, 255), Title = "PickRate" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minDate.AddDays(-1).Day, Maximum = DateTime.Now.AddDays(+1).Day, Title = "Date", MajorStep = 1, MinorStep = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, Title = "PickRate%" });
            for (byte i = 0; i < godsIcon.Length; i++)
            {
                godsIcon[i].line = new LineSeries() { Title = godsIcon[i].Name, Color = OxyColor.FromRgb((byte)(10 * i), (byte)(10 * i), (byte)(10 * i)) };
            }
            var l = new Legend();
            l.LegendTitle = "Legends";
            model.Legends.Add(l);
            for (int i = 7; i >= 0; i--)
            {
                var Res = File.ReadAllText($"Data/{DateTime.Today.AddDays(-i).ToString("d")}.json");
                List<PlayerData> jsonData2 = JsonConvert.DeserializeObject<List<PlayerData>>(Res);
                for (int b = 0; b < godsIcon.Length; b++)
                {
                    godsIcon[b].line.Points.Add(new DataPoint(DateTime.Today.AddDays(-i).Day, godsIcon[b].PickRate));
                    var an = new PointAnnotation { X = DateTime.Today.AddDays(-i).Day, Y = godsIcon[b].PickRate };
                    an.Text = an.Y.ToString();
                    model.Annotations.Add(an);
                    /*  model.Series.Add(godsIcon[b].line);*/

                }
            }


            var width = 1024;
            var height = 768;


            PngExporter.Export(model, "graf.png", width, height);

            using (var fs = new FileStream("graf.png", FileMode.Open, FileAccess.Read))
            {
                var msg = await new DiscordMessageBuilder()
                .AddFile(fs)
                .SendAsync(ctx.Channel);
            }
        }


        [Command("sts")]
        public async Task StatsPlayer(CommandContext ctx, string name)
        {
            var minDate = DateTime.Now.AddDays(-7);
            var model = new PlotModel() { Background = OxyColor.FromRgb(255, 255, 255), Title = name };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minDate.AddHours(-1).Day, Maximum = DateTime.Now.AddHours(1).Day, Title = "Date", MajorStep = 1, MinorStep = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 1, Maximum = 500, Title = "Ranking" });
            var ls1 = new LineSeries() { Title = "1 VS 1", Color = OxyColors.Purple };
            var ls2 = new LineSeries() { Title = "2 VS 2", Color = OxyColors.RoyalBlue };
            var l = new Legend();
            l.LegendTitle = "Legend";
            model.Legends.Add(l);
            for (int i = 7; i >= 0; i--)
            {
                var Res = File.ReadAllText($"Data/{DateTime.Today.AddDays(-i).ToString("d")}.json");
                List<PlayerData> jsonData = JsonConvert.DeserializeObject<List<PlayerData>>(Res);
                var info = jsonData.FindAll(x => x.player_name == name);

                var vs1 = info.FirstOrDefault(x => x.match_queue_id == 511);
                if (vs1 != null)
                {
                    ls1.Points.Add(new DataPoint(DateTime.Today.AddDays(-i).Day, vs1.ranking));
                    var an = new PointAnnotation { X = DateTime.Today.AddDays(-i).Day, Y = vs1.ranking };
                    an.Text = an.Y.ToString();
                    model.Annotations.Add(an);
                }
                else
                    ls1.Points.Add(new DataPoint(DateTime.Today.AddDays(-i).Day, 500));

                var vs2 = info.FirstOrDefault(x => x.match_queue_id == 469);
                if (vs2 != null)
                {
                    ls2.Points.Add(new DataPoint(DateTime.Today.AddDays(-i).Day, vs2.ranking));
                    var an = new PointAnnotation { X = DateTime.Today.AddDays(-i).Day, Y = vs2.ranking };
                    an.Text = an.Y.ToString();
                    model.Annotations.Add(an);
                }
                else
                    ls2.Points.Add(new DataPoint(DateTime.Today.AddDays(-i).Day, 500));
            }

            model.Series.Add(ls1);
            model.Series.Add(ls2);
            var width = 1024;
            var height = 768;


            PngExporter.Export(model, "graf.png", width, height);

            using (var fs = new FileStream("graf.png", FileMode.Open, FileAccess.Read))
            {
                var msg = await new DiscordMessageBuilder()
                .AddFile(fs)
                .SendAsync(ctx.Channel);
            }
        }


        class Icon
        {
            public string Name;
            public string Url;
            public int Kol;
            public int PickRate;
            public LineSeries line;

            public Icon(string name, string url, int kol = 0, LineSeries line = null)
            {
                Name = name;
                Url = url;
                Kol = kol;
                this.line = line;
            }
        }
        class Emoji
        {
            public char number;
            public string emoji;

            public Emoji(char number, string emoji)
            {
                this.number = number;
                this.emoji = emoji;
            }
        }
        class SavedData
        {
            public DateTime SavedTime;
            public List<PlayerData> Data;
            public SavedData(DateTime savedTime, List<PlayerData> data)
            {
                SavedTime = savedTime;
                Data = data;
            }
        }




    }

}






