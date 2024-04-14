using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mnozenie_macierzy
{
    internal class Macierz
    {
        public int[,] macierz = new int[GlobalVariables.L_WIERSZY, GlobalVariables.L_KOLUMN];

        public Macierz()
        {
        }

        public Macierz(int seed) 
        {
            Random random = new Random(seed);
            for(int i = 0; i < GlobalVariables.L_WIERSZY; i++)
            {
                for(int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                {
                    macierz[i, j] = random.Next(0, 10);
                }
            }
        }

        public Macierz Mnozenie(Macierz macierz1, Macierz macierz2)
        {
            Macierz wynikowa = new Macierz();

            for (int i = 0; i < GlobalVariables.L_WIERSZY; i++)
            {
                for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                {
                    for (int k = 0; k < GlobalVariables.L_KOLUMN; k++)
                    {
                        wynikowa.macierz[i, j] += (macierz1.macierz[i, k] * macierz2.macierz[k, j]);
                    }
                }
            }
            return wynikowa;
        }

        public Macierz MnozenieWatki(Macierz macierz1, Macierz macierz2, int StartIndex, int StopIndex)
        {
            Macierz wynikowa = new Macierz();

            for (int i = StartIndex; i < StopIndex; i++)
            {
                for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                {
                    for (int k = 0; k < GlobalVariables.L_KOLUMN; k++)
                    {
                        wynikowa.macierz[i, j] += (macierz1.macierz[i, k] * macierz2.macierz[k, j]);
                    }
                }
            }
            return wynikowa;
        }

        public int dlugosc_liczby()
        {
            int najd_liczba = 0;
            for (int i = 0; i < GlobalVariables.L_WIERSZY; i++)
            {
                for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                {
                    if (najd_liczba < macierz[i, j].ToString().Length)
                    {
                        najd_liczba = macierz[i, j].ToString().Length;
                    }
                }
            }
            return najd_liczba;
        }

        public void Wyświetl(int dlugosc_liczby)
        {
            for (int i = 0; i < GlobalVariables.L_WIERSZY; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                {
                    Console.Write(macierz[i, j].ToString().PadLeft(dlugosc_liczby));
                    Console.Write("  ");
                }
                Console.Write("|\n");
            }
            Console.WriteLine();
        }
    }
}
