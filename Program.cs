using System;
using System.Collections.Generic;
namespace c__nn
{
    class Program
    {        
        static void Main(string[] args)
        {
            Neural_Network NN = new Neural_Network(3, 784, 50, 4);

            mnist_data_processor processor = new mnist_data_processor("mnist_train_formatted.txt");
            processor.format();
            Console.WriteLine("training data formatted");
            double[,] training_inputs = ListToDouble(processor.inputs);
            double[][] training_targets = prepTargets(processor.answers, NN.output_nodes);
            Console.WriteLine("training arrays filled");

            mnist_data_processor testing = new mnist_data_processor("mnist_test_formatted.txt");
            testing.format();
            Console.WriteLine("testing data formatted");
            double[,] testing_inputs = ListToDouble(testing.inputs);
            double[][] testing_targets = prepTargets(testing.answers, NN.output_nodes);
            Console.WriteLine("testing arrays filled");

            // for (int i = 0; i < training_targets.Length; i ++) {
            //     Console.WriteLine(i + " " + arrToString(training_targets[i]));
            // }
            Console.WriteLine("Begining training");
            train(40000, NN, training_inputs, training_targets);
            Console.WriteLine("Training Finished\nBegining Testing");
            test(NN, testing_inputs, testing_targets);
        }

        public static void train(int num_times, Neural_Network network, double[,] training_inputs, double [][] training_targets) {
            Random rand = new Random();
            double [,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < num_times; i ++) {
                int num = rand.Next(0, training_inputs.GetLength(0));
                for (int k = 0; k < X.GetLength(0); k ++) {
                    X[k,0] = training_inputs[num, k];
                }
                Y = training_targets[num];
                network.train(X, Y);
            }
        }

        public static void test(Neural_Network network, double[,] training_inputs, double [][] training_targets) {
            double[,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < training_inputs.GetLength(0); i += 500) {
                for (int j = 0; j < training_inputs.GetLength(1); j ++) {
                    X[j,0] = training_inputs[i,j];
                }

                Y = training_targets[i];
                double[] guess = network.predict(X);
                double[] error = Matrix.subtract(Y, guess);
                
                Console.WriteLine("Output: " + arrToString(guess) + "  Expected: " + arrToString(training_targets[i]) + "  Error: " + arrToString(error) + "\n");
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

        public static double[][] ListToJagged(List<List<double>> list) {
            double[][] arr = new double[list.Count][];
            for (int i = 0; i < list.Count; i ++) {
                arr[i] = new double[list[i].Count];
            }

            for (int i = 0; i <list.Count; i ++) {
                for (int j = 0; j < list[0].Count; j ++) {
                    arr[i][j] = list[i][j];
                }
            }
            return arr;
        }

        public static double[][] ListToJagged(List<double> list, int len = 1) {
            double[][] arr = new double[list.Count][];
            for (int i = 0; i < list.Count; i ++) {
                arr[i] = new double[len];
            }

            for (int i = 0; i <list.Count; i ++) {
                arr[i][0] = list[i];
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

        public static double[][] prepTargets(List<double> targets, int in_nodes) {
            double[][] x = ListToJagged(targets, in_nodes);
            for (int i = 0; i < x.Length; i ++) {
                for (int k = 0; k < in_nodes; k ++) {
                    x[i][k] = 0;
                }
                x[i][(int)targets[i]] = 1;
            }
            return x;
        }
    }
}
