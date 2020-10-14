using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace medio_curso
{
    class HilosTiempo{
        public double hilos;
        public double tiempo;
        public HilosTiempo(double hilos,double tiempo){
            this.hilos = hilos;
            this.tiempo = tiempo;
        }
    }
    class Program
    {
        static double[][] matrix3;

        static void Main(string[] args)
        {
            // double[][] matrix = new double[3][] { 
            //     new double[]{ 1, 2}, 
            //     new double[]{ 3, 4}, 
            //     new double[]{ 5, 6}
            // };

            // double[][] matrix2 = new double[2][] { 
            //     new double[]{7,9,11},
            //     new double[]{8,10,12}
                
            //     /* 
            //     7 ; i = 0 , j = 0
            //     8; i = 1, j = 0
                
            //     */
            // };


            double[][] matrix = new double[11][] { 
                new double[]{7,9,11},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{8,10,12},
                new double[]{9,2,32},
                /* 
                7 ; i = 0 , j = 0
                8; i = 1, j = 0
                
                */
            };

            double[][] matrix2 = new double[3][] {
                new double[]{ 1, 2,3,4,5,6,7,8,9,4}, 
                new double[]{ 3, 4,9,8,7,6,5,4,3,2},
                new double[]{ 5, 6,5,4,3,2,9,8,7,6 }
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

            // # iteraciones
            int n = 3;
            int hilos_init = 2;
            int[] var_hilos = new int[n];
            if(n <= 12) { // Hilos disponibles
                for(int i=0; i < n; i ++) { // Guardando variacion de hilos
                    var_hilos[i] = hilos_init;
                    hilos_init += 2;
                }
            } else {
                throw new Exception("El numero de hilos supera la cantidad de hilos disponibles");
            }
            
            List<HilosTiempo> vlsGrafica = new List<HilosTiempo>();

            for(int q=0; q < n; q ++) { // Ciclo de iteraciones
                TimeSpan tiempo = new TimeSpan();
                Thread[] hilos = new Thread[var_hilos[q]]; // Serializando hilos
                for(int i=0; i<var_hilos[q];i++) { 
                    hilos[i] = new Thread(multiply); 
                }

                int n_block = (int) Math.Ceiling((double) newRows / var_hilos[q]);
                int counterSeg = 0;
                int counterBlock = 0;
                for(int k=0; k < var_hilos[q]; k++) { //Ciclo de los hilos
                    if(k == var_hilos[q] - 1) { // Asignar faltante al ultimo hilo
                        n_block = newRows - counterSeg;
                    }
                    // Console.WriteLine($"Filas por bloque : {n_block} counter {counterBlock}");
                    double[][] tempBlock = new double[n_block][]; // Segmentos
                    for(int i=0; i < n_block; i++) { //Asignando bloques (filas) a los segmentos
                        tempBlock[i] = matrix[counterBlock+i];
                    }
                    // Numero de filas = bloques, inicializando hilos y realizando operaciones por bloques
                    for(int i=0; i < n_block; i++) {
                        
                        double[] temp = new double[tempBlock[0].Length]; // Bloque

                        for(int j = 0; j < tempBlock[0].Length; j++) { // Asignando valores de bloques
                            temp[j] = tempBlock[i][j];
                        }
                        Stopwatch sw = Stopwatch.StartNew();
                        //aqui cuenta el timpo
                        hilos[k] = new Thread(multiply);  // Instanciando hilo
                        hilos[k].Start(new object[] {temp,matrix2OneDim,i+counterBlock}); // Iniciando hilo
                        hilos[k].Join();
                        tiempo+= sw.Elapsed;
                        //aqui finaliza
                    }
                    counterBlock += n_block;
                    counterSeg += n_block;
                }
                vlsGrafica.Add(new HilosTiempo(var_hilos[q],tiempo.TotalMilliseconds));
                matrix3 = new double[newRows][];
                for(int i = 0; i < newRows; i++) { // Definiendo matriz resultado como arreglos anidados
                    matrix3[i] = new double[newCols];
                }
            }
            
            graficar(vlsGrafica,newRows,newCols);
            foreach(var i in vlsGrafica){
                Console.WriteLine($"Hilos {i.hilos} Tiempo {i.tiempo}");
            }
        }

        static void graficar(List<HilosTiempo> valores, int rows, int cols){
            var plt = new ScottPlot.Plot(600,400);
            double[] tiempos = new double[valores.Count]; // y
            double[] hilos = new double[valores.Count]; // x
            int j =0;
            foreach(var i in valores){
                tiempos[j] = i.tiempo;
                hilos[j] = i.hilos;
                j++;
            }

            plt.PlotScatter(hilos,tiempos);
            plt.Legend();

            plt.Title("Hilos vs Tiempo");
            plt.YLabel("Tiempo (ms)");
            plt.XLabel("Hilos");
            string nombreImagen = string.Format("matriz_resultado_{0}x{1}_Grafica.png",rows,cols);
            try{
                plt.SaveFig(nombreImagen);
                Console.WriteLine("Se ha guardado la imágen");
            }catch(Exception){
                Console.WriteLine("No se ha podido guardar la imágen");
            }
        }

        static void imprimir(double[][] matr) { // Imprimir matrices
            Console.WriteLine("MATRIZ ------");
            for(int i = 0; i < matr.Length; i++){
                for(int j= 0; j < matr[0].Length; j++){
                    Console.Write($"{matr[i][j]}, ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("FIN  MATRIZ ------");
        }
         //static void multiply(double[] array1, double[] array2, int row) {
        static void multiply(object data) {
            // Desempacar parametros
            object[] data_objects = (object[]) data;
            double[] array1 = (double[]) data_objects[0];
            double[] array2 = (double[]) data_objects[1];
            int row = (int) data_objects[2];

            int interval = array1.Length; // Intervalo de reccorido
            int counter = 0;
            double sum = 0;
            for(int i=0; i <(array2.Length / interval); i ++) { // Recorrer segundo arreglo en intervalos
                for(int j = 0; j < interval; j++) {
                    // Console.Write(array2[counter] + ", ");
                    sum += array1[j] * array2[counter]; // Acumulacion
                    counter++;
                }
                matrix3[row][i]= sum; // Asignacion de valor calculado a matriz global
                sum = 0;
                // Console.Write("\n");
            }
        }

    }
}
