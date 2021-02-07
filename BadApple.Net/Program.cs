using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using BadApple.Net.Utils;

namespace BadApple.Net
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var frameFiles = Directory.GetFiles("pics");
            Console.SetWindowSize(81, 30);

            var frames = new List<IEnumerable<ColoredString>>();

            for (var i = 0; i < frameFiles.Length; i++)
            {
                Console.Title = $"Importing frame {i + 1}/{frameFiles.Length}";
                var frame = Parser.Parse(frameFiles[i]);
                
                var strings = new List<ColoredString>();
                var x = new ColoredString();
                
                foreach (var height in frame)
                {
                    foreach (var color in height)
                    {
                        if (x.Color == color)
                        {
                            x.Text += " ";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(x.Text))
                                strings.Add(x);
                            
                            x = new ColoredString()
                            {
                                Color = color,
                                Text = " "
                            };
                        }
                    }
                    x.Text += "\n";
                }

                frames.Add(strings);
            }

            Console.Title = "Playing!";

            
            const double frameLimit = 1000f / 31.4;
            var sw = Stopwatch.StartNew();

            var sp = new SoundPlayer("audio.wav");
            sp.Play();
            
            foreach (var frame in frames)
            {
                var startDrawTime = sw.Elapsed.TotalMilliseconds;
                Console.SetCursorPosition(0, 0);

                foreach (var x in frame)
                {
                    Console.BackgroundColor = x.Color;
                    Console.Write(x.Text);
                }

                var durationTime = sw.Elapsed.TotalMilliseconds - startDrawTime;

                if (durationTime < frameLimit)
                    Thread.Sleep((int)(frameLimit - durationTime));
            }

            Console.Title = "End";
            Console.Clear();
            Console.ReadLine();
        }
    }
}