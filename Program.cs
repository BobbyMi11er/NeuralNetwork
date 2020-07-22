using System;

namespace c__nn
{
    class Program
    {        
        static void Main(string[] args)
        {
            Neural_Network NN = new Neural_Network(2, 2, 3, 1);

            double[,] training_inputs = new double[,] {{1,0}, {0,1}, {1,1}, {0,0}};
            double[] training_targets = new double[4] {1, 1, 0, 0};

            train(50000, NN, training_inputs, training_targets);
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
            int error_counter = 0;
            double error_treshold = 0.40;

            for (int i = 0; i < training_inputs.GetLength(0); i ++) {
                for (int j = 0; j < training_inputs.GetLength(1); j ++) {
                    X[j,0] = training_inputs[i,j];
                }
                double guess = network.predict(X);
                double error = training_targets[i] - guess;
                if (error > error_treshold || error < (-1 * error_treshold))
                    error_counter ++;
                Console.WriteLine("Output: " + guess + "  Expected: " + training_targets[i] + "  Error: " + error);
            }
            if (error_counter >= 2) {
                Console.Write("Excess error detected. Retraining for better results");
                train(50000, network, training_inputs, training_targets);
            }   
        }
    }
}
