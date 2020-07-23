using System;
using System.Collections.Generic;
using System.Drawing;

namespace c__nn
{
    class mnist_data_processor
    {
        static String FILEPATH = "C:\\Users\\GuestUser\\Downloads\\";
        // static String InputLine = "";
        public static List<List<double>> inputs {get; private set;}
        public List<int> answers {get; private set;}

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
            answers = new List<int>();
            string [] lines = System.IO.File.ReadAllLines(FILEPATH);

            int k = 0;
            foreach(string line in lines) {
                if (k > 3)
                    break;
                String [] inArray = line.Split(",");
                inputs.Add(new List<double>());

                // answers is first number in line
                answers.Add(int.Parse(inArray[0]));
                for (int i = 1; i < inArray.Length; i ++) {
                    // if (int.Parse(inArray[i]) > 50)
                    //     inputs[k].Add(1);
                    // else
                    //     inputs[k].Add(0);
                    inputs[k].Add((int.Parse(inArray[i])/255.0));
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