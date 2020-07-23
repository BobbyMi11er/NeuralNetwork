using System;

namespace c__nn
{
    class Function { 

        public virtual double func(double x, int i, int j) {
            return 0;
        }

        public virtual double dfunc(double x) {
            return 0;
        }

        public static double[,] map(double[,] array, Function function, bool deriv = false) {
            // try
            // {
                double[,] newArray = array;
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (!deriv)
                            newArray[i, j] = function.func(array[i,j], i, j);
                        else
                            newArray[i, j] = function.dfunc(array[i,j]);
                    }
                }
                return newArray;
        }

    }
    class Sigmoid : Function
    {
        public override double func(double x, int i, int j) {
            double y = 1.0 / (1.0 + Math.Exp(-x));
            return y;
        }

        public override double dfunc(double x) {
            double y = x * (1-x);
            return y;
        }
    }

    class ReLU : Function
    {
        public override double func(double x, int i, int j) {
            if (x > 0)
                return x;
            return 0;
        }    
    }

    class Rand : Function {
        private int min;
        private int max;
        Random rand = new Random();

        public Rand(int min, int max) {
            // max IS NOT included
            this.min = min;
            this.max = max;
        }
        public override double func(double x, int i, int j) {
            if (max != min)
                return rand.NextDouble() * (max-min) + min;
            else {
                if (rand.NextDouble() <= 0.5) {
                    return rand.NextDouble() + min;
                }
                else {
                    return -1 * (rand.NextDouble() + min);
                }
            }
        }
    }

    class MathOperation : Function {
        Matrix B;
        String Str;
        bool isScalar = false;
        double Scalar;
        public MathOperation(Matrix b, string str) {
            B = b;
            Str = str;
        }
        public MathOperation(double scalar, string str) {
            Str = str;
            isScalar = true;
            Scalar = scalar;
        }

         public override double func(double x, int i, int j) {
            double value = 0;
            if (isScalar)
                value = Scalar;
            else {
                value = B.Data[i,j];
            }

            if (Str == "-")
                return (x - value);
            else if (Str == "*")
                return (x * value);
            else if (Str == "+")
                return (x + value);
            else if (Str == "/")
                return (x / value);
            else
                Console.WriteLine("Invalid Operation entered");
                return 0;
        }
    }

    class Dot : Function {
        Matrix A, B;

        public Dot(Matrix a, Matrix b) {
            A = a;
            B = b;
        }

        public override double func(double x, int i, int j) {
            double sum = 0;
            for (int k = 0; k < A.Cols; k++)
            {
                // i fucking hate this + sign. 
                sum += A.Data[i, k] * B.Data[k, j];
            }
            return sum;
        }
    }

    class Transposer : Function {
        Matrix Matrix;
        public Transposer(Matrix a) {
            Matrix = a;
        }

        public override double func(double x, int i, int j) {
            return Matrix.Data[j,i];
        }
    }
}