using System;
using System.Collections.Generic;
using System.Drawing;

namespace c__nn
{
    class mnist_data_processor
    {
        static String FILEPATH = "C:\\Users\\GuestUser\\Downloads\\";
        // static String InputLine = "";
        public List<List<double>> inputs {get; private set;}
        public List<double> answers {get; private set;}

        public mnist_data_processor(String path) {
            if (path.Substring(0,2).Equals("C:")) {
                // allow people to put in full path if wanted
                FILEPATH = path;
            }
            else {
                FILEPATH += path;
            }
        }

        public void format() {
            inputs = new List<List<double>>();
            answers = new List<double>();
            string [] lines = System.IO.File.ReadAllLines(FILEPATH);

            int k = 0;
            foreach(string line in lines) {
                // gets rid of extra space at end of each line
                String newLine = line.Substring(0, line.Length - 1);

                String [] inArray = newLine.Split(" ");

                inputs.Add(new List<double>());

                // answers is first number in line
                answers.Add(Double.Parse(inArray[0]));
                
                for (int i = 1; i < inArray.Length; i ++) {
                    // if (int.Parse(inArray[i]) > 50)
                    //     inputs[k].Add(1);
                    // else
                    //     inputs[k].Add(0);
                    try {
                        inputs[k].Add(Double.Parse(inArray[i]));
                    }
                    catch (FormatException) {
                        Console.WriteLine(inArray[i].GetType());
                        Console.WriteLine("i: " + i + " k: " + k);
                        Console.WriteLine(inArray[i] + "end");
                        inputs[k].Add(Double.Parse(inArray[i]));
                        break;
                    }
                }
                k ++;
            } 
        }

        public void saveImg(int row, string path) {
             int img_size = (int)Math.Sqrt(inputs[row].Count);
            Bitmap b = new Bitmap(img_size, img_size);

            int q = 0;
            for (int y = 0; y < b.Height; y ++) {
                for (int x = 0; x < b.Width; x ++) {
                    int grey_val = (int) inputs[row][q] * 255;
                    Color c = Color.FromArgb(grey_val, grey_val, grey_val);
                    b.SetPixel(x, y, c);
                    q ++;
                }
            }
            b.Save("C:\\Users\\GuestUser\\Downloads\\" + path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}