using System;


namespace Secuencial {
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matriz1= {
                {1,5,4},
                {1,7,8},
                {5,2,3},
                {5,2,3},
            };
            int[,] matriz2={
                {1,7,9,1},
                {2,7,3,2},
                {2,4,5,3},
                {5,4,2,5},
                {1,2,3,4}
            };

            int n_filas = matriz1.GetLength(0); 
            int n_columnas= matriz2.GetLength(1);
            int[,] resultado = new int[n_filas,n_columnas];
            int c, suma;
            DateTime startTime, endTime;

            startTime = DateTime.Now;
            for(int i=0;i< n_filas;i++){ 
                c=0;
                while(c<n_columnas){
                    suma=0;
                    for(int j=0;j<n_filas-1;j++)
                        suma=suma+(matriz1[i,j]*matriz2[i,c]);
                        resultado[i,c]=suma;
                        c=c+1;
                }
            }
            for(int i=0;i<n_filas;i++) {
                for(int j=0;j<n_columnas;j++)
                    Console.Write(resultado[i,j]+"\t");
                    Console.WriteLine();
            }
            endTime = DateTime.Now;
            Double elapsedMillisecs = ((TimeSpan)(endTime - startTime)).TotalMilliseconds;
            Console.WriteLine(elapsedMillisecs+" milisecs");
            
        }
    }
}
