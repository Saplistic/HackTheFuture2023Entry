using System.Net.Http.Json;
using Common;
using Microsoft.VisualBasic.CompilerServices;

namespace HTF2023
{
    internal class Program
    {
        public static HackTheFutureClient client = new HackTheFutureClient();

        static async Task Main(string[] args)
        {
            client.Login("The Lawbenders", "CZ2zmwJ5Gv").Wait();

            // A-1
            await client.GetAsync("/api/path/a/easy/start");
            //await postHieroglief("/api/path/a/easy/sample"); // Sample oplossing posten
            //await postHieroglief("/api/path/a/easy/puzzle"); // Oplossing posten

            // A-2 
            await client.GetAsync("/api/path/a/medium/start");
            await postNavigatieLianen("/api/path/a/medium/sample");
            await postNavigatieLianen("/api/path/a/medium/puzzle");
        }

        public static async Task postHieroglief(string endPoint)
        {
            var response = await client.GetAsync(endPoint);
            string strResponse = await response.Content.ReadAsStringAsync();

            string oplossing = decodeHieroglief(strResponse);

            var postResponse = await client.PostAsJsonAsync(endPoint, oplossing);
            var postResponseStatus = await postResponse.Content.ReadAsStringAsync();
            Console.WriteLine(postResponseStatus);
        }

        // Challenge A-1
        public static string decodeHieroglief(string str)
        {
            HieroglyphAlphabet alphabet = new HieroglyphAlphabet();
            string input = str;
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (HieroglyphAlphabet.Characters.TryGetValue(input[i], out char value))
                {
                    output += value;
                }
                else
                {
                    output += " ";
                }
            }

            //Console.WriteLine("Decoded string: " + output);
            return output;
        }

        public static async Task postNavigatieLianen(string endPoint)
        {
            var response = await client.GetFromJsonAsync<VineNavigationChallengeDto>(endPoint);

            string[] directions = response.Directions;
            Console.WriteLine(response.Start);
            string strX = response.Start.Split(',').ToList<string>()[0];
            string strY = response.Start.Split(',').ToList<string>()[1];
            int xPos = int.Parse(strX);
            int yPos = int.Parse(strY);
            int border = (int)Math.Sqrt(response.AmountOfVines);

            for (int i = 0; i < directions.Length; i++)
            {
                for (int j = 0; j < directions[i].Length; j++)
                {
                    switch (directions[i][j])
                    {
                        case 'R':
                            if (xPos < border-1)
                            {
                                xPos++;
                                Console.WriteLine("moved right");
                            }
                            else
                            {
                                Console.WriteLine("Can't move right");
                            }
                            break;
                        case 'L':
                            if (xPos > 0)
                            {
                                xPos--;
                                Console.WriteLine("moved left");
                            }
                            else
                            {
                                Console.WriteLine("Can't move left");
                            }
                            break;
                        case 'U':
                            if (yPos < border-1)
                            {
                                yPos++;
                                Console.WriteLine("moved up");
                            }
                            else
                            {
                                Console.WriteLine("Can't move up");
                            }
                            break;
                        case 'D':
                            if (yPos > 0)
                            {
                                yPos--;
                                Console.WriteLine("moved down");
                            }
                            else
                            {
                                Console.WriteLine("Can't move down");
                            }
                            break;
                    }

                    Console.WriteLine("x:" + xPos + "y:" + yPos);

                }
            }
            string oplossing = xPos + "," + yPos;

            var postResponse = await client.PostAsJsonAsync(endPoint, oplossing);

            var postResponseStatus = await postResponse.Content.ReadAsStringAsync();
            Console.WriteLine(postResponseStatus);
        }
    }
}

