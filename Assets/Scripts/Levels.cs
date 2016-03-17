using Assets.Models.WorldData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Assets.Scripts
{
    public class Levels
    {
        // level load stuff
        // ----------
        // each line is a stack
        // 'red blue white' => red block at the bottom
        // @random min [max=min] => makes a random tower 
        // with the height range-ing from min to max 


        // stackMax is not the amount of stacks
        // it's the stacks placed before and after (x = 0)
        // when stackMax = 3 that means there are 7 stacks (-3, -2, -1, 0, 1, 2, 3)
        // so the stack amount = stackMax * 2 + 1
        // public static because the section builder needs this data too
        public static int stackMax;
        public string currentLevel;

        public List<BlockStack> LoadLevel(string name, out bool levelExists, bool changeLevelExists = true)
        {
            currentLevel = name;

            levelExists = false;

            string[] nSplit = name.Split('/');
            string user = nSplit.Length > 1 ? nSplit[0] : "_default";

            string file = user == "_default" ? user + "/" + name + ".txt" : name + ".txt";
            string filePath = Path.Combine(levelPath, file);

            if (File.Exists(filePath))
            {
                if (changeLevelExists)
                    levelExists = true;
                return ParseFile(filePath);
            }
            else
            {
                string pathToWrite = Path.Combine(levelPath, file);
                string uri = levelUri + file;
                if (Ping(uri))
                {
                    DownloadLevel(uri, pathToWrite, user);

                    if (changeLevelExists)
                        levelExists = true;

                    return ParseFile(pathToWrite);
                }

                // level does not exist. Load new one and keep levelExists to false (third parameter)
                return LoadLevel("empty", out levelExists, false);
            }
        }
        private void DownloadLevel(string uri, string pathToWrite, string user)
        {
            Directory.CreateDirectory(levelPath + "/" + user);

            WebRequest webRequest = WebRequest.Create(uri);
            using (WebResponse resp = webRequest.GetResponse())
            {
                string data = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                File.WriteAllText(pathToWrite, data);
            }
        }
        private List<BlockStack> ParseFile(string path)
        {
            List<BlockStack> stacks = new List<BlockStack>();

            int lineCount = File.ReadAllLines(path).Length;
            bool needOneMoreStack = false;

            if ((lineCount - 1) % 2 != 0)
            {
                needOneMoreStack = true;
                stackMax = (lineCount) / 2;
            }
            else
            {
                stackMax = (lineCount - 1) / 2;
            }


            // add 'amount' empty stacks so you can place blocks there, 
            // because you cant place blocks on the ground if there is no stack
            int amount = 100;
            int c = -stackMax - amount;
            for (int i = 0; i < amount; i++)
            {
                stacks.Add(new BlockStack(c));
                c++;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    BlockStack stack = new BlockStack(c);
                    if (line != "" && !line.Contains("@random"))
                    {
                        string[] blocks = line.Split(' ');
                        int yCounter = 0;
                        foreach (string blockColor in blocks)
                        {
                            stack.Blocks.Push(new Block("("+c+"/"+yCounter+")", blockColor, c, yCounter));
                            yCounter++;
                        }
                    }
                    else if (line.Contains("@random"))
                    {
                        string[] data = line.Split(' ');
                        int min = data.Length > 1 ? int.Parse(data[1]) : 1;
                        int max = data.Length > 2 ? int.Parse(data[2]) : data.Length > 1 ? min : 1;

                        stack = GenerateStack(c, min, max);
                    }

                    stacks.Add(stack);
                    c++;
                }
            }

            // add 'amount' empty stacks so you can place blocks there...
            for (int i = 0; i < amount; i++)
            {
                stacks.Add(new BlockStack(c));
                c++;
            }

            if (needOneMoreStack)
                stacks.Add(new BlockStack(c));

            return stacks;
        }

        private bool Ping(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void DownloadAgain()
        {
            string[] nSplit = currentLevel.Split('/');
            string user = nSplit.Length > 1 ? nSplit[0] : "_default";
            string file = user == "_default" ? user + "/" + currentLevel + ".txt" : currentLevel + ".txt";

            string uri = levelUri + file;
            string pathToWrite = Path.Combine(levelPath, file);

            DownloadLevel(uri, pathToWrite, user);
        }

        private List<BlockStack> GenerateRandomLevel()
        {
            stackMax = defaultStackMax;

            List<BlockStack> stacks = new List<BlockStack>();
            for (int x = -stackMax; x <= stackMax; x++)
            {
                stacks.Add(GenerateStack(x, minCubes, maxCubes));
            }

            return stacks;
        }
        private BlockStack GenerateStack(int x, int min, int max)
        {
            BlockStack stack = new BlockStack(x);

            int minS = Math.Min(min, max);
            int maxS = Math.Max(min, max);

            int cubesAmount = rnd.Next(minS, maxS + 1);

            for (int i = 0; i < cubesAmount; i++)
            {
                string id = "(" + x + "/" + i + ")";
                string color = ((ColorEnum.Colors)rnd.Next(0, 4)).ToString();
                int y = stack.Blocks.Count();

                stack.Blocks.Push(new Block(id, color, x, y));
            }

            return stack;
        }



        private string levelPath = Path.GetFullPath(".") + "\\Levels\\";
        private string levelUri = "http://localhost/levels/";
        private Random rnd = new Random();
        private int minCubes = 1;
        private int maxCubes = 6;
        private int defaultStackMax = 10000;
    }
}