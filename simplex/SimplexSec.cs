using System;
using System.Collections.Generic;
using System.Linq;

namespace simplex{
    class SimplexSec{
        static List<Constraint> constraintsList = new List<Constraint>();
        static int[] objectiveFunction;
        static double[][] tableu;
        static int numVariables;
        static int cols;
        static int rows;
        static int s;
        static int a;
        static int e;
        static double z;
        static void printTableu(){
            for(int i = 0; i < tableu.Length; i++){
                for(int j = 0; j < cols; j++){
                    if(tableu[i][j] < 0 || tableu[i][j] > 9)
                        Console.Write($"{ tableu[i][j]:#.###} |");
                    else
                        Console.Write($"{tableu[i][j]:#.###}  |");
                }
                Console.WriteLine();
            }
        }
        static void PrepareTableu(){
            s = constraintsList.FindAll(x => x.typeR == Constraint.Type.LessOrEqual).Count;
            e = constraintsList.FindAll(x => x.typeR == Constraint.Type.GreaterOrEqual).Count;
            a = constraintsList.FindAll(x => x.typeR == Constraint.Type.Equal).Count + e;
            
            tableu = new double[s+1][];
            cols = s + numVariables + 1;
            Console.WriteLine($"a : {a} e: {e} s : {s} cols : {cols}");
            for(int i = 0; i < tableu.Length; i++){
                tableu[i] = new double[cols];
                if(i == tableu.Length - 1){
                    Console.WriteLine(i);
                    for(int j =0; j < cols; j++){
                        if(j < numVariables)
                            tableu[i][j] = objectiveFunction[j] * -1;
                        else
                            tableu[i][j] = 0;
                    }
                }
                else{
                    int count =numVariables;
                    for(int j = 0; j < cols; j++){
                        //Console.WriteLine($"a : {constraintsList[i].constants[j]} i {i} j {j} ");
                        if(j < numVariables){
                            tableu[i][j] = constraintsList[i].constants[j];
                        }

                        else if(j < cols-2){
                            if(i == 0)
                                tableu[i][numVariables] = 1;
                            else if(i==1)
                                tableu[i][numVariables+1] = 1;
                            else if(i==2)
                                tableu[i][numVariables+2] = 1;
                            else{
                                tableu[i][j] = 0;
                            }
                        }
                        else{
                            tableu[i][cols-1] = constraintsList[i].result;
                        }
                    }
                }
            }
            printTableu();
        }
        static void Solve(){
            rows = tableu.Length;
            //Console.WriteLine($"tiene negativos {}");
            //while there's a negative in the z row, the process continues
            while(tableu[rows-1].Any(x => x < 0)){
                
                //get the variable that is out and the pivote row
                double min = double.MaxValue;
                int numColumn = 0;
                for(int j = 0; j < cols; j++){
                    if( tableu[rows-1][j] < min){
                        min = tableu[rows-1][j];
                        numColumn = j;
                    }
                }
                Console.WriteLine($"num min {min} column {numColumn}");
                //looks for the minimun in the rhs
                min = double.MaxValue;
                double divider =0;
                int numRow=0;
                for(int i = 0; i < tableu.Length-1; i++){
                    try{
                        divider = tableu[i][cols-1]/tableu[i][numColumn];
                    }catch(Exception){
                        //in case the divider is zero exception
                        continue;
                    }
                    Console.WriteLine($"divider row{i} {divider}");
                    if(divider < min && divider > 0){
                        min = divider;
                        numRow = i;
                    }
                }
                Console.WriteLine($"num min {min} row {numRow}");
                //change pivot row
                double valueToOne = tableu[numRow][numColumn];
                for(int k = 0; k < cols; k++){
                    tableu[numRow][k] /= valueToOne;
                    Console.WriteLine($" {tableu[numRow][k]}");
                }
                //multiply the rest of rows to update them using the pivot row
                for(int m = 0; m < rows; m++){
                    if(m == numRow) continue; //don't change the pivot row
                    double pivote = tableu[m][numColumn]*-1;
                    double[] temp = tableu[m].Zip(tableu[numRow], (s,x) => ( (x*pivote) + s ) ).ToArray();
                    tableu[m] =temp;
                }
                printTableu();
            }
        }
        public static void printResults(){
            z = tableu[rows-1][cols-1];
            double x1 = tableu[0][cols-1];
            double x2 = tableu[1][cols-1];
            double x3 = tableu[2][cols-1];
            Console.WriteLine($"valor óptimo {z:#.###}, var 1 {x1:#.###} var 2 {x2:#.###} var 3 {x3:#.###}");
        }
        public static void Init(){
            
            constraintsList.Add(new Constraint(new int[]{2,1}, 18 , Constraint.Type.LessOrEqual));
            constraintsList.Add(new Constraint(new int[]{2,3},42 , Constraint.Type.LessOrEqual));
            constraintsList.Add(new Constraint(new int[]{3,1}, 24, Constraint.Type.LessOrEqual));
            int numConstraints = constraintsList.Count;
            
            objectiveFunction = new int[]{3,2};
            numVariables = objectiveFunction.Length;
            PrepareTableu();
            Solve();
            printResults();
        }
    }
}