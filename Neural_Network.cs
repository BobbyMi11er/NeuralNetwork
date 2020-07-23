using System;

namespace c__nn {

    class Neural_Network {
        Matrix[] weights, biases;
        public double learning_rate = 0.1;
        public int num_layers, input_nodes, output_nodes;
        Function function;

        public Neural_Network(int num_Layers, int input_nodes, int hidden_nodes, int output_nodes) {
            this.input_nodes = input_nodes;
            this.output_nodes = output_nodes;
            num_layers = num_Layers;
            weights = new Matrix[num_layers];
            biases = new Matrix[num_layers];

            for (int i = 0; i < num_layers; i ++) {
                if (i == 0) {
                    weights[i] = new Matrix(hidden_nodes, input_nodes);
                    biases[i] = new Matrix(hidden_nodes, 1);
                }
                else if (i == num_layers - 1) {
                    weights[i] = new Matrix(output_nodes, hidden_nodes);
                    biases[i] = new Matrix(output_nodes, 1);
                }
                else {
                    weights[i] = new Matrix(hidden_nodes, hidden_nodes);
                    biases[i] = new Matrix(hidden_nodes, 1);
                }
                weights[i].randomize();
                biases[i].randomize();
            }
            function = new Sigmoid();
        }

        public double predict(double[,] input_array) {
            Matrix inputs = Matrix.fromArray(input_array);
            Matrix[] outputs = new Matrix[num_layers];
            for (int i = 0; i < num_layers; i ++) {
                if (i == 0)
                    outputs[i] = Matrix.dot(weights[i], inputs);
                else
                    outputs[i] = Matrix.dot(weights[i], outputs[i-1]);
                outputs[i].add(biases[i]);
                outputs[i].map(function);
            }
            // only works if output is one
            return outputs[num_layers - 1].Data[0,0];
        }

        public void train(double[,] input_array, double[] target_array) {
            Matrix inputs = Matrix.fromArray(input_array);

            Matrix[] outputs = new Matrix[num_layers];
            for (int i = 0; i < num_layers; i ++) {
                if (i == 0)
                    outputs[i] = Matrix.dot(weights[i], inputs);
                else
                    outputs[i] = Matrix.dot(weights[i], outputs[i-1]);
                outputs[i].add(biases[i]);
                outputs[i].map(function);
            }

            Matrix targets = Matrix.fromArray(target_array);

            Matrix[] errors = new Matrix[num_layers];
            Matrix[] gradients = new Matrix[num_layers];
            Matrix[] deltas = new Matrix[num_layers];

            for (int i = num_layers - 1;i >= 0; i --) {
                // FIND ERRORS
                if (i == num_layers -1)
                    errors[i] = Matrix.subtract(targets, outputs[i]);
                else {
                    Matrix who_T = Matrix.transpose(weights[i + 1]);
                    errors[i] = Matrix.dot(who_T, errors[i + 1]);
                }

                // FIND GRADIENTS AND DELTAS
                gradients[i] = Matrix.map(outputs[i], function, true);
                gradients[i].multiply(errors[i]);
                gradients[i].multiply(learning_rate);

                if (i != 0) {
                    Matrix hidden_T = Matrix.transpose(outputs[i-1]);
                    deltas[i] = Matrix.dot(gradients[i], hidden_T);
                }
                else {
                    Matrix inputs_T = Matrix.transpose(inputs);
                    deltas[i] = Matrix.dot(gradients[i], inputs_T);
                }

                // ADD GRADIENTS AND DELTAS
                weights[i].add(deltas[i]);
                biases[i].add(gradients[i]);
            }
        }
    }
}