using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Secuencial {
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matriz1={
                {1,5,4},
                {1,7,8},
                {5,2,3},
                {5,2,3}
            };
            int[,] matriz2={
                {1,7,9},
                {2,7,3},
                {2,4,5}
            };

            // n_filas = matriz1.GetLength(0); Obtener numero de filas
            // matriz1.GetLength(1); Obtener numero de columnas


            int[,] resultado = new int[3,3];
            int c, suma;

            // Console.Write("La primera matriz: [[1,5,4],[1,7,8],[5,2,3]]\n");
            // Console.Write("La segunda matriz: [[1,7,9],[2,7,3],[2,4,5]]\n");

            for(int i=0;i<3;i++){ /* for(int i =0; i < n_filas; i++) */
                c=0;
                while(c<3){
                    suma=0;
                    for(int j=0;j<2;j++)
                        suma=suma+(matriz1[i,j]*matriz2[i,c]);
                        resultado[i,c]=suma;
                        c=c+1;
                }
            }
            // Imprimir
            for(int i=0;i<3;i++) {
                for(int j=0;j<3;j++)
                    Console.Write(resultado[i,j]+"\t");
                    Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
