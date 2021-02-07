using System;
using System.Drawing;

namespace BadApple.Net.Utils
{
    public class Parser
    {
        public static ConsoleColor[][] Parse(string img)
        {
            var image = new Bitmap(img);
            var colors = new ConsoleColor[image.Height][];
            
            for (var i = 0; i < colors.Length; i++)
                colors[i] = new ConsoleColor[image.Width];

            for (var x = 0; x < image.Width; x++)
            {
                for(var y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);

                    colors[y][x] = color.G > 196 ? ConsoleColor.White :
                        color.G > 128 ? ConsoleColor.Gray :
                        color.G > 64 ? ConsoleColor.DarkGray : ConsoleColor.Black;
                }
            }
            
            return colors;
        }
    }
}