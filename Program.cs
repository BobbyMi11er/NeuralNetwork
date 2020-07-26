using System;
using System.Diagnostics;

namespace c__nn
{
    class Program
    {        
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            mnist_data_processor processor1 = new mnist_data_processor("testing_set.txt");
            processor1.format();
            stopwatch.Stop();
            Console.WriteLine("File processed. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            stopwatch.Start();
            double[,] training_inputs = new double[processor1.inputs.Count, processor1.inputs[0].Count];
            double[,] training_targets = new double[processor1.answers.Count, 4];
            for (int i = 0; i < processor1.inputs.Count; i ++) {
                for (int k = 0; k < processor1.inputs[i].Count; k ++) {
                    training_inputs[i,k] = processor1.inputs[i][k];
                }
                for (int j = 0; j < training_targets.GetLength(1); j ++) {
                    training_targets[i,j] = 0;
                }
                training_targets[i, (int)processor1.answers[i]] = 1.0;
            }
            stopwatch.Stop();
            Console.WriteLine("Training_inputs created and filled. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            stopwatch.Start();
            // TARGETS ARE BEING DONE WRONG!!!
            double[] int_targets = processor1.answers.ToArray();


            stopwatch.Stop();
            Console.WriteLine("Training targets created and filled. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            
            Neural_Network NN = new Neural_Network(3, 784, 50, 4);

            Console.WriteLine("Begining training");
            stopwatch.Start();
            train(1, NN, training_inputs, training_targets);
            stopwatch.Stop();
            Console.WriteLine("Training finished. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();


            mnist_data_processor processor2 = new mnist_data_processor("mnist_test.csv");
            processor2.format();
            double[,] testing_inputs = new double[processor2.inputs.Count, processor2.inputs[0].Count];
            double[,] testing_targets = new double[processor2.answers.Count, 4];

            for (int i = 0; i < processor2.inputs.Count; i ++) {
                for (int k = 0; k < processor2.inputs[i].Count; k ++) {
                    testing_inputs[i,k] = processor2.inputs[i][k];
                }
                for (int j = 0; j < testing_targets.GetLength(1); j ++) {
                    testing_targets[i,j] = 0;
                }
                testing_targets[i, (int)processor1.answers[i]] = 1.0;
            }
        

            stopwatch.Start();
            Console.WriteLine("Testing values now");
            test(NN, training_inputs, training_targets);
            stopwatch.Stop();
            Console.WriteLine("Testing finished. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
        }

        public static void train(int num_times, Neural_Network network, double[,] training_inputs, double [,] training_targets) {
            Random rand = new Random();
            double [,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < num_times; i ++) {
                int num = rand.Next(0, training_inputs.GetLength(0));
                for (int k = 0; k < X.GetLength(0); k ++) {
                    X[k,0] = training_inputs[num, k];
                }
                for (int j = 0; j < Y.Length; j ++ ) {
                    Y[j] = training_targets[num, j];
                }
                network.train(X, Y);
            }
        }

        public static void test(Neural_Network network, double[,] testing_inputs, double [,] testing_targets) {
            double[,] X = new double[network.input_nodes, 1];
            // int error_counter = 0;
            // double error_treshold = 0.40;
           double[] total_error = new double[network.output_nodes];
           double[] avg_error = new double[network.output_nodes];

            for (int i = 0; i < testing_inputs.GetLength(0); i ++) {
                for (int j = 0; j < testing_inputs.GetLength(1); j ++) {
                    X[j,0] = testing_inputs[i,j];
                }
                double[] guess = network.predict(X);
                double[] error = new double[network.output_nodes];
                for (int k = 0; k < network.output_nodes; k ++) {
                    error[k] = guess[k] - testing_targets[i,k];
                    total_error[k] += error[k];
                }
                if (i % 1000 == 0) {
                    for (int k = 0; k < avg_error.Length; k ++) {
                        avg_error[k] = total_error[k]/i;
                    }
                    Console.WriteLine("Average Error through " + i + " iterations is " + avg_error);
                }
            }
        }
    }
}
