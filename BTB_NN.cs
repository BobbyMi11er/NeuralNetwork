using System;
using NumSharp;

namespace c__nn {

    class BTB_NN {
        // public int Input_nodes;
        // public int Hidden_nodes;
        // public int Output_nodes;
        Matrix weights_ih, weights_ho, bias_h, bias_o;
        public double learning_rate = 0.1;

        Function function;

        public BTB_NN(int input_nodes, int hidden_nodes, int output_nodes) {
            // Input_nodes = input_nodes;
            // Hidden_nodes = hidden_nodes;
            // Output_nodes = output_nodes;

            weights_ih = new Matrix(hidden_nodes, input_nodes);
            weights_ho =  new Matrix(output_nodes, hidden_nodes);
            weights_ih.randomize();
            weights_ho.randomize();

            bias_h =  new Matrix(hidden_nodes, 1);
            bias_o =  new Matrix(output_nodes, 1);
            bias_h.randomize();
            bias_o.randomize();

            function = new Sigmoid();
        }

        public double predict(double[,] input_array) {
            Matrix inputs = Matrix.fromArray(input_array);
            Matrix hidden = Matrix.dot(weights_ih, inputs);
            hidden.add(bias_h);
            hidden.map(function);

            Matrix output = Matrix.dot(weights_ho, hidden);
            output.add(bias_o);
            output.map(function);
            return output.Data[0,0];
        }

        public void train(double[,] input_array, double[] target_array, int i = 0) {
            Matrix inputs = Matrix.fromArray(input_array);

            Matrix hidden = Matrix.dot(weights_ih, inputs);
            hidden.add(bias_h);
            hidden.map(function);

            Matrix outputs = Matrix.dot(weights_ho, hidden);
            outputs.add(bias_o);
            outputs.map(function);

            Matrix targets = Matrix.fromArray(target_array);

            // find output errors (targets - outputs)
            Matrix output_errors = Matrix.subtract(targets, outputs);

            // calculate gradient
            Matrix gradients = Matrix.map(outputs, function, true);
            gradients.multiply(output_errors);
            gradients.multiply(learning_rate);

            // calculate deltas
            Matrix hidden_T = Matrix.transpose(hidden);
            Matrix weight_ho_deltas = Matrix.dot(gradients, hidden_T);

            // adjust weights by deltas
            weights_ho.add(weight_ho_deltas);
            // adjust biases by deltas (which is just the gradient)
            bias_o.add(gradients);

            // calculate hidden layer errors
            Matrix who_T = Matrix.transpose(weights_ho);
            Matrix hidden_errors = Matrix.dot(who_T, output_errors);

            // calculate hidden gradient
            Matrix hidden_gradient = Matrix.map(hidden, function, true);
            hidden_gradient.multiply(hidden_errors);
            hidden_gradient.multiply(learning_rate);

            // calculate hidden --> input deltas
            Matrix inputs_T = Matrix.transpose(inputs);
            Matrix weight_ih_deltas = Matrix.dot(hidden_gradient, inputs_T);

            // adjust by deltas
            weights_ih.add(weight_ih_deltas);
            bias_h.add(hidden_gradient);
        }
    }
}