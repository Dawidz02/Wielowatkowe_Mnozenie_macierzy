using System.Text.RegularExpressions;
using System.Threading;

public class GlobalVariables
{
    public static int L_WIERSZY = 7;
    public static int L_KOLUMN = 7;
    public static int L_WATKOW = 2;
}

namespace Mnozenie_macierzy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int dlugosc = 0;

            // Tworzenie i generowanie wartości macierzy
            Console.Write("Wprowadź ziarno: ");
            int seed = int.Parse(Console.ReadLine());
            Macierz macierz_1 = new Macierz(seed);
            Macierz macierz_2 = new Macierz(seed+1);
            Macierz macierz_pom = new Macierz();
            Macierz macierz_pom_2 = new Macierz();
            Macierz macierz_wynik = new Macierz();
            Macierz macierz_wynik_watki = new Macierz();
            Macierz macierz_wynik_watki_2 = new Macierz();

            // Mnożenie jednowątkowe
            var watch = System.Diagnostics.Stopwatch.StartNew();
            macierz_wynik = macierz_1.Mnozenie(macierz_1, macierz_2);
            watch.Stop();

            // Wyświetlanie macierzy
            dlugosc = macierz_wynik.dlugosc_liczby();
            macierz_1.Wyświetl(dlugosc);
            macierz_2.Wyświetl(dlugosc);
            macierz_wynik.Wyświetl(dlugosc);
         

            // Przydzialy dla watków
            int[] przydzial = new int[GlobalVariables.L_WATKOW + 1];
            for(int i = 0; i < GlobalVariables.L_WATKOW + 1; i++)
            {
                przydzial[i] = GlobalVariables.L_WIERSZY / GlobalVariables.L_WATKOW;
            }
            przydzial[0] = 0;
            int reszta = GlobalVariables.L_WIERSZY % GlobalVariables.L_WATKOW;
            if (reszta != 0)
            {
                int pom = 0;
                while(pom < reszta)
                {
                    for(int i = 1; i < GlobalVariables.L_WATKOW; i++)
                    {
                        if(pom < reszta)
                        {
                            ++przydzial[i];
                            ++pom;
                        }
                    }
                }
            }
            
            for (int i = 1; i < GlobalVariables.L_WATKOW + 1; i++)
            {
                przydzial[i] = przydzial[i - 1] + przydzial[i];
            }


            // Tworzenie wątków przy pomocy funkcji "Thread"
            var watch_2 = System.Diagnostics.Stopwatch.StartNew();
            
            Thread[] threads = new Thread[GlobalVariables.L_WATKOW];
            for (int i = 0; i < GlobalVariables.L_WATKOW; i++)
            {
                int a = i;
                int przydzial_start = przydzial[i];
                int przydzial_koniec = przydzial[i + 1];

                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"Wątek {a} został uruchomiony, pracuje na przedziale {przydzial_start} - {przydzial_koniec}");
                    macierz_pom = macierz_1.MnozenieWatki(macierz_1, macierz_2, przydzial_start, przydzial_koniec);
                    for (int l = przydzial_start; l < przydzial_koniec; l++)
                    {
                        for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                        {
                            macierz_wynik_watki.macierz[l, j] = macierz_pom.macierz[l, j];
                        }
                    }
                });
                threads[i].Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            watch_2.Stop();
            macierz_wynik_watki.Wyświetl(dlugosc);

            // Tworzenie wątków przy pomocy funkcji "Parallel"
            var watch_3 = System.Diagnostics.Stopwatch.StartNew();

            Parallel.For(0, GlobalVariables.L_WATKOW, new ParallelOptions { MaxDegreeOfParallelism = GlobalVariables.L_WATKOW }, i =>
            {
                int przydzial_start_2 = przydzial[i];
                int przydzial_koniec_2 = przydzial[i + 1];
                macierz_pom_2 = macierz_1.MnozenieWatki(macierz_1, macierz_2, przydzial_start_2, przydzial_koniec_2);
                for (int l = przydzial_start_2; l < przydzial_koniec_2; l++)
                {
                    for (int j = 0; j < GlobalVariables.L_KOLUMN; j++)
                    {
                        macierz_wynik_watki_2.macierz[l, j] = macierz_pom_2.macierz[l, j];
                    }
                }
            });
            watch_3.Stop();
            macierz_wynik_watki_2.Wyświetl(dlugosc);


            Console.WriteLine($"Pomiar czasu dla 1 wątku: {watch} \nPomiar czasu dla wielowątkowości Thread {watch_2} \nPomiar czasu dla wielowątkowości Parallel {watch_3}");
          
            // Zapis pomiarów do pliku
            string[] dane = { watch.Elapsed.TotalSeconds.ToString(), watch_2.Elapsed.TotalSeconds.ToString(), watch_3.Elapsed.TotalSeconds.ToString() };
            string sciezkaPliku = "C:\\Users\\Admin\\OneDrive\\Pulpit\\.net-java\\Laby_3\\Mnozenie_macierzy\\Mnozenie_macierzy\\Pomiary.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(sciezkaPliku, true))
                {
                    writer.WriteLine();
                    foreach (string linia in dane)
                    {
                        writer.Write(linia);
                        writer.Write("   ");
                    }
                }
                Console.WriteLine("Dane zostały zapisane do pliku: " + sciezkaPliku);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas zapisywania danych: " + ex.Message);
            }
        }
    }
}
