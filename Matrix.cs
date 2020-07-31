using System;

namespace c__nn {

    class Matrix { 
        public int Dim {get; set;}
        public int Rows {get; set;}
        public int Cols {get; set;}
        public double[,] Data {get; set;}

        public Matrix(int rows, int cols) {
            Rows = rows;
            Cols = cols;
            if (rows != 1)
                Dim = 2;
            else 
                Dim = 1;
            Data = new double[rows, cols];
        }
        public Matrix(Matrix a) {
            Rows = a.Rows;
            Cols = a.Cols;
            Data = a.Data;
        }

        public static Matrix transpose(Matrix a){
            Matrix newMatrix = new Matrix(a.Cols, a.Rows);
            Transposer t = new Transposer(a);
            newMatrix.map(t);
            return newMatrix;
        }
        public static Matrix dot(Matrix a, Matrix b) {
            if (a.Cols != b.Rows) {
                Console.WriteLine("Shape Error: A Columns must equal B Rows!!!");
                return null;
            }
            Matrix newMatrix = new Matrix(a.Rows, b.Cols);
            Dot dot = new Dot(a, b);
            newMatrix.map(dot);
            return newMatrix;
        }

        public Array toArray() {
            if (this.Dim == 1) {
                double[] newArray = new double[this.Cols];
                for (int i = 0; i < this.Cols; i ++) {
                    newArray[i] = this.Data[0,i];
                }
                return newArray;
            }
            else {
                double [,] newArray = this.Data;
                return newArray;
            }
        }

        public static Matrix fromArray(double[,] arr) {
            Matrix m = new Matrix(arr.GetLength(0), arr.GetLength(1));
            for (int i = 0; i <m.Rows; i ++) {
                for (int j = 0; j < m.Cols; j ++) {
                    m.Data[i,j] = arr[i,j];
                }
            }
            return m;
        }
        public static Matrix fromArray(double[] arr) {
            Matrix m = new Matrix(1, arr.Length);
            for (int j = 0; j < m.Cols; j ++) {
                m.Data[0,j] = arr[j];
            }
            return m;
        }

        public static Matrix subtract(Matrix a, Matrix b) {
            if (a.Rows != b.Rows || a.Cols != b.Cols) {
                Console.WriteLine("Columns and Rows of a must match columns and rows of b");
                return null;
            }
            MathOperation sub = new MathOperation(b, "-");
            Matrix m = new Matrix(a.Data.GetLength(0), a.Data.GetLength(1));
            m.Data = Function.map(a.Data, sub);
            return m;
        }
        public static double[] subtract(double[] a, double[] b) {
            double[,] A = new double[1, a.Length];
            for (int i = 0; i < a.Length; i ++) {
                A[0,i] = a[i];
            }
            MathOperation sub = new MathOperation(b, "-");
            double[,] m = new double[1, a.Length];
            m = Function.map(A, sub);
            double [] M = new double[a.Length];
            for (int i = 0; i < m.Length; i ++) {
                M[i] = m[0,i];
            }
            return M;
        }
        public void subtract(Matrix b) {
            if (this.Rows != b.Rows || this.Cols != b.Cols) {
                Console.WriteLine("Subtract(): Columns and rows of this array must match columns and rows of inputted array");
            }
            else {
                MathOperation sub = new MathOperation(b, "-");
                this.Data = Function.map(this.Data, sub);
            }
        }

        public void add(Matrix b) {
            // Matrix Addition
            if (this.Rows != b.Rows || this.Cols != b.Cols) {
                Console.WriteLine("Add(): Columns and rows of this array must match columns and rows of inputted array");
            }
            else {
                MathOperation add = new MathOperation(b, "+");
                this.Data = Function.map(this.Data, add);
            }
        }

        public static double[] add(double[] a, double[] b) {
             double[,] A = new double[1, a.Length];
            for (int i = 0; i < a.Length; i ++) {
                A[0,i] = a[i];
            }
            MathOperation sub = new MathOperation(b, "+");
            double[,] m = new double[1, a.Length];
            m = Function.map(A, sub);
            double [] M = new double[a.Length];
            for (int i = 0; i < m.Length; i ++) {
                M[i] = m[0,i];
            }
            return M;
        }
        public void add(double x) {
            // Scalar Addition
            MathOperation add = new MathOperation(x, "+");
            this.Data = Function.map(this.Data, add);
        }

        public void multiply(Matrix b) {
            if (this.Rows != b.Rows || this.Cols != b.Cols) {
                Console.WriteLine("Multiply(): Columns and rows of this array must match columns and rows of inputted array");
            }
            else {
                // Hadamard Product
                MathOperation mult = new MathOperation(b, "*");
                this.Data = Function.map(this.Data, mult);
            }
        }
        public void multiply(double x) {
            // scalar multiplication
            MathOperation mult = new MathOperation(x, "*");
            this.Data = Function.map(this.Data, mult);
        }

        public void randomize() {
            Rand rand = new Rand(-1,1);
            this.Data = Function.map(this.Data, rand);
        }

        public void map(Function function, bool deriv = false) {
            this.Data = Function.map(this.Data, function, deriv);
        }

        public static Matrix map(Matrix a, Function function, bool deriv = false) {
            Matrix matrix = new Matrix(a);
            matrix.map(function, deriv);
            return matrix;
        }

        public void print(string message = null) {
            if (message != null) {
                Console.WriteLine(message);
            }
            for (int i = 0; i < this.Rows; i ++) {
                Console.Write("[");
                for (int j = 0; j < this.Cols - 1; j ++) {
                    Console.Write(this.Data[i,j] + ",");
                }
                Console.WriteLine(this.Data[i,this.Cols - 1] + "]");
            }
            Console.WriteLine();
        }

        public static void print(Matrix matrix, string message = null) {
            if (message != null) {
                Console.WriteLine(message);
            }
            for (int i = 0; i < matrix.Rows; i ++) {
                Console.Write("[");
                for (int j = 0; j < matrix.Cols - 1; j ++) {
                    Console.Write(matrix.Data[i,j] + ", ");
                }
                Console.WriteLine(matrix.Data[i,matrix.Cols - 1] + "]");
            }
            Console.WriteLine();
        }
    }
}