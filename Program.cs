using System;
using System.Collections.Generic;
namespace c__nn
{
    class Program
    {        
        static void Main(string[] args)
        {
            Neural_Network NN = new Neural_Network(2, 2, 3, 1);

            mnist_data_processor processor = new mnist_data_processor("xor_problem.txt");
            processor.format();
            double[,] training_inputs = ListToDouble(processor.inputs);
            double[] training_targets = ListToDouble(processor.answers);

            train(35000, NN, training_inputs, training_targets);
            test(NN, training_inputs, training_targets);
        }

        public static void train(int num_times, Neural_Network network, double[,] training_inputs, double [] training_targets) {
            Random rand = new Random();
            double [,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < num_times; i ++) {
                int num = rand.Next(0, training_inputs.GetLength(0));
                for (int k = 0; k < X.GetLength(0); k ++) {
                    X[k,0] = training_inputs[num, k];
                }
                for (int j = 0; j < Y.Length; j ++ ) {
                    Y[j] = training_targets[num];
                }
                network.train(X, Y);
            }
        }

        public static void test(Neural_Network network, double[,] training_inputs, double [] training_targets) {
            double[,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < training_inputs.GetLength(0); i ++) {
                for (int j = 0; j < training_inputs.GetLength(1); j ++) {
                    X[j,0] = training_inputs[i,j];
                }
                Y[0] = training_targets[i];
                double[] guess = network.predict(X);
                double[] error = Matrix.subtract(Y, guess);
                
                Console.WriteLine("Output: " + arrToString(guess) + "  Expected: " + training_targets[i] + "  Error: " + arrToString(error));
            }  
        }

        public static String arrToString(double[] arr) {
            String str = "[";
            for (int i = 0; i <arr.Length-1; i ++) {
                str += arr[i] + ",";
            }
            str += arr[arr.Length-1] + "]";
            return str;
        }

        public static double[,] ListToDouble(List<List<double>> list) {
            double[,] arr = new double[list.Count, list[0].Count];

            for (int i = 0; i <list.Count; i ++) {
                for (int j = 0; j < list[0].Count; j ++) {
                    arr[i,j] = list[i][j];
                }
            }
            return arr;
        }

        public static double[] ListToDouble(List<double> list) {
            double[] arr = new double[list.Count];

            for (int i = 0; i < list.Count; i ++) {
                arr[i] = list[i];
            }
            return arr;
        }
    }
}
