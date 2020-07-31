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
            mnist_data_processor processor1 = new mnist_data_processor("xor_problem.txt");
            processor1.format();
            stopwatch.Stop();
            Console.WriteLine("File processed. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            stopwatch.Start();
            double[,] training_inputs = new double[processor1.inputs.Count, processor1.inputs[0].Count];
            double[,] training_targets = new double[processor1.answers.Count, 1];
            for (int i = 0; i < processor1.inputs.Count; i ++) {
                for (int k = 0; k < processor1.inputs[i].Count; k ++) {
                    training_inputs[i,k] = processor1.inputs[i][k];
                }
                training_targets[i,0] = processor1.answers[i];
            }
            
            // for (int i = 0; i < processor1.inputs.Count; i ++) {
            //     for (int j = 0; j < processor1.inputs[i].Count; j ++) {
            //         Console.Write(training_inputs[i,j] + " ");
            //     }
            //     Console.WriteLine("\t answer: " + training_targets[i,0]);
            // }
            stopwatch.Stop();
            Console.WriteLine("Training_inputs & training_targets created and filled. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();           
            
            Neural_Network NN = new Neural_Network(3, 2, 4, 1);

            Console.WriteLine("Begining training");
            stopwatch.Start();
            train(50000, NN, training_inputs, training_targets);
            stopwatch.Stop();
            Console.WriteLine("Training finished. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            test(NN, training_inputs, training_targets);


//             mnist_data_processor processor2 = new mnist_data_processor("mnist_test_formatted.txt");
//             processor2.format();
//             double[,] testing_inputs = new double[processor2.inputs.Count, processor2.inputs[0].Count];
//             double[,] testing_targets = new double[processor2.answers.Count, 4];

//             for (int i = 0; i < processor2.inputs.Count; i ++) {
//                 for (int k = 0; k < processor2.inputs[i].Count; k ++) {
//                     testing_inputs[i,k] = processor2.inputs[i][k];
//                 }
//                 for (int j = 0; j < testing_targets.GetLength(1); j ++) {
//                     testing_targets[i,j] = 0;
//                 }
//                 testing_targets[i, (int)processor1.answers[i]] = 1.0;
//             }

//             stopwatch.Start();
//             Console.WriteLine("Testing values now");
//             test(NN, training_inputs, training_targets);
//             stopwatch.Stop();
//             Console.WriteLine("Testing finished. Elapsed time is {0} milliseconds", stopwatch.ElapsedMilliseconds);
//             stopwatch.Reset();
        }

        public static void train(int num_times, Neural_Network network, double[,] training_inputs, double [,] training_targets) {
            if (network.input_nodes != training_inputs.GetLength(1)) {
                Console.WriteLine("Input nodes doesn't match training_inputs");
                Console.WriteLine("input nodes: " + network.input_nodes + "\ttraining_inputs: " + training_inputs.GetLength(0));
                throw new Exception("ERROR ^");
            }
            if (training_targets.GetLength(1) != network.output_nodes) {
                Console.WriteLine("Output nodes don't equal training_targets cols");
                throw new Exception("ERROR ^");
            }
            Random rand = new Random();
            double [,] X = new double[network.input_nodes, 1];
            double[] Y = new double[network.output_nodes];

            for (int i = 0; i < num_times; i ++) {
                int num = rand.Next(0, training_inputs.GetLength(0));
                for (int k = 0; k < X.GetLength(1); k ++) {
                    try {
                        X[k,0] = training_inputs[num, k];
                    }
                    catch(IndexOutOfRangeException) {
                        Console.WriteLine("k: " + k);
                    }
                }
                for (int j = 0; j < Y.Length; j ++ ) {
                    Y[j] = training_targets[num, j];
                }
                network.train(X, Y);
            }
        }
        public static void test(Neural_Network network, double[,] testing_inputs, double[,] testing_targets) {
            // only for xor problem
            double[,] X = new double[testing_inputs.GetLength(1),1];
            double[] Y = new double[1];
            for (int i = 0; i < testing_targets.Length; i ++) {
                Console.Write("inputs: ");
                for (int j = 0; j < 2; j ++) {
                    X[j,0] = testing_inputs[i,j];
                    Console.Write(X[j,0] + " ");
                }
                Y[0] = testing_targets[i,0];
                double[] prediction = network.predict(X);
                Console.WriteLine("\tprediction: " + prediction[0]);
            }
        }

        // public static void test(Neural_Network network, double[,] testing_inputs, double [,] testing_targets) {
        //     double[,] X = new double[network.input_nodes, 1];
        //     // int error_counter = 0;
        //     // double error_treshold = 0.40;
        //    double[] total_error = new double[network.output_nodes];
        //    double[] avg_error = new double[network.output_nodes];
        //    string error_str;

        //     for (int i = 0; i < testing_inputs.GetLength(0); i ++) {
        //         for (int j = 0; j < testing_inputs.GetLength(1); j ++) {
        //             X[j,0] = testing_inputs[i,j];
        //         }
        //         double[] guess = network.predict(X);
        //         double[] error = new double[network.output_nodes];
        //         total_error = Matrix.add(total_error, error);
        //         if (i % 1000 == 0) {
        //             for (int k = 0; k < avg_error.Length; k ++) {
        //                 avg_error[k] = total_error[k]/i;
        //             }
        //             error_str = "[";
        //             for (int j = 0; j < avg_error.Length - 1; j ++) {
        //                 error_str += avg_error[j] + ", ";
        //             }
        //             error_str += avg_error[avg_error.Length - 1] + "]";
        //             Console.WriteLine("Average Error through " + i + " iterations is " + error_str);
        //         }
        //     }
        //     for (int k = 0; k < avg_error.Length; k ++) {
        //         avg_error[k] = total_error[k]/testing_inputs.GetLength(0);
        //     }
        //     error_str = "[";
        //     for (int j = 0; j < avg_error.Length - 1; j ++) {
        //         error_str += avg_error[j] + ", ";
        //     }
        //     error_str += avg_error[avg_error.Length - 1] + "]";
        //     Console.WriteLine(" Total Average Error is " + error_str);
        // }
    }
}
