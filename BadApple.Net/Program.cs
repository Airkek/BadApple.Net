using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using BadApple.Net.Utils;

namespace BadApple.Net
{
    internal static class Program
    {
        private static void Main()
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


            const double fps = 25;
            const double frameTime = 1000f / fps;

            var sp = new SoundPlayer("audio.wav");
            sp.Play();
            var sw = Stopwatch.StartNew();

            var lastFrame = -1;
            for (var frameIndex = 0; frameIndex < frames.Count; frameIndex = (int)(sw.ElapsedMilliseconds / frameTime))
            {
                if (lastFrame == frameIndex)
                {
                    continue;
                }

                lastFrame = frameIndex;
                var frame = frames[frameIndex];
                Console.SetCursorPosition(0, 0);

                foreach (var x in frame)
                {
                    Console.BackgroundColor = x.Color;
                    Console.Write(x.Text);
                }
            }

            Console.Title = "End";
            Console.Clear();
            Console.ReadLine();
        }
    }
}