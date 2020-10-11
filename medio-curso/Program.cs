using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace medio_curso
{
    
    class Program
    {
        static double[][] matrix3;
        static void Main(string[] args)
        {
            double[][] matrix = new double[3][] { 
                new double[]{ 1, 2}, 
                new double[]{ 3, 4}, 
                new double[]{ 5, 6}
            };

            double[][] matrix2 = new double[2][] { 
                new double[]{7,9,11},
                new double[]{8,10,12}
                
                /* 
                7 ; i = 0 , j = 0
                8; i = 1, j = 0
                
                */
            };

            int newRows = matrix.Length; // Filas de la primer matriz
            int newCols = matrix2[0].Length; // Columnas de la segunda matriz

            // Console.Write($"{newRows} x {newCols}\n");

           matrix3 = new double[newRows][]; // Matriz resultado

            for(int i = 0; i < newRows; i++) { // Definiendo matriz resultado como arreglos anidados
                matrix3[i] = new double[newCols];
            }

            double[] matrix2OneDim = new double[matrix2[0].Length * matrix2.Length];
            int counter = 0;
            for(int j = 0; j < matrix2[0].Length;j++){
                for(int i = 0;i < matrix2.Length; i++){
                    matrix2OneDim[counter] = matrix2[i][j];
                    counter++;
                }
            }
            // for(int i = 0; i< matrix2OneDim.Length; i++) {
            //     Console.WriteLine($"{matrix2OneDim[i]}");
            // }

            // # iteraciones
            int n = 2;
            int hilos_init = 2;
            int[] var_hilos = new int[n];
            if(n <= 12) { // Hilos disponibles
                for(int i=0; i < n; i ++) { // Guardando variacion de hilos
                    var_hilos[i] = hilos_init;
                    hilos_init += 2;
                    Console.Write($"{var_hilos[i]}, ");
                }
              Console.Write("\n");  
            } else {
                throw new Exception("El numero de hilos supera la cantidad de hilos disponibles");
            }

            Thread[] hilos = new Thread[n]; // Serializando hilos


            for(int i=0; i<n;i++) {
                hilos[i] = new Thread(multiply); 
            }

            

            for(int k=0; k < n; k++) {
                for(int i=0; i < newRows; i++) { // Numero de filas = bloques
                    
                    double[] temp = new double[matrix[0].Length];

                    for(int j = 0; j < matrix[0].Length; j++) {
                        temp[j] = matrix[i][j];
                    }
                    hilos[k] = new Thread(multiply); 
                    hilos[k].Start(new object[] {temp,matrix2OneDim,i});
                    hilos[k].Join();
                    // while(!hilos[k].IsAlive){
                    //     Console.WriteLine("siiii");
                    // }
                }
            }  

            



            // multiply(new double[]{1,2},matrix2OneDim,0);

            for(int i=0; i< matrix3.Length; i++) {
                for(int j=0; j<matrix3[0].Length;j++) {
                    Console.Write(matrix3[i][j] + "-");
                }
                Console.Write("\n");
            }
          
        }
         //static void multiply(double[] array1, double[] array2, int row) {
        static void multiply(object data) {
            object[] data_objects = (object[]) data;
            double[] array1 = (double[]) data_objects[0];
            double[] array2 = (double[]) data_objects[1];
            int row = (int) data_objects[2];

            int interval = array1.Length;
            int counter = 0;
            double sum = 0;
            for(int i=0; i <(array2.Length / interval); i ++) { // Recorrer segundo arreglo en intervalos
                for(int j = 0; j < interval; j++) {
                    // Console.Write(array2[counter] + ", ");
                    sum += array1[j] * array2[counter];
                    counter++;
                }
                matrix3[row][i]= sum;
                sum = 0;
                // Console.Write("\n");
            }
        }

    }
}
