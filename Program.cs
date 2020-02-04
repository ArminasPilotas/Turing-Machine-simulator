using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ND1_Turingo_Masina
{
    class Program
    {
        public static void Menu()
        {
            string start;
            Console.WriteLine("TURING MACHINE SIMULATOR");
            Console.WriteLine();
            Console.WriteLine("1. Start Turing Machine SIMULATOR");
            Console.WriteLine("2. Exit Program");
            Console.Write("Enter your choice: ");
            start = Console.ReadLine();
            if (start == "2")
            {
                Environment.Exit(0);
            }
            Console.Clear();



        }
        public static ConcurrentDictionary<int, string> duomenys = new ConcurrentDictionary<int, string>(); //mapas skirtas duomenims saugoti pagal ID

        private static int currentID = 0;
        public static int GetID {
            get
            {
                return currentID++;
            }
        }
        class Turing_Machine
        {
            public int id;
            public int head;
            public static int countfiles = -1;
            public int steps;
            public string tape;
            public string[] c_state;
            public char[] c_symbol;
            public char[] n_symbol;
            public char[] directionarray;
            public string[] n_state;
            public string[] failas;
            public string[] temp1;
            public char[] headpositioncursor;
            public char headicon;

            public void ReadAndSetData()
            {
                string text= Directory.GetCurrentDirectory();
                id = GetID;
                countfiles++;
                string[] files = {@"\1.txt",@"\2.txt",@"\3.txt",@"\4.txt" };
                Console.Clear();
                string temp = text + files[countfiles];

                while (!File.Exists(temp))
                {
                    Console.Clear();
                    Console.WriteLine("File does not found, exiting program");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                    failas = File.ReadAllLines(temp);
                    Console.Clear();


                
                Console.Clear();

                tape = failas[1];
                head = Int32.Parse(failas[0])-1;
                int count = 0;
                int empty = 0;
                for (int i = 0; i < failas.Length; i++)
                {
                    if (failas[i].Length == 0)
                        empty++;
                }
                c_state = new string[failas.Length - 2 - empty];
                c_symbol = new char[failas.Length - 2 - empty];
                n_symbol = new char[failas.Length - 2 - empty];
                directionarray = new char[failas.Length - 2 - empty];
                n_state = new string[failas.Length - 2 - empty];

                for (int i = 2; i < failas.Length; i++)
                {

                    temp1 = failas[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (temp1.Length != 0)
                    {
                        c_state[count] = temp1[0];
                        c_symbol[count] = char.Parse(temp1[1]);
                        n_symbol[count] = char.Parse(temp1[2]);
                        directionarray[count] = char.Parse(temp1[3]);
                        n_state[count] = temp1[4];
                        count++;
                    }
                    headpositioncursor = new char[tape.Length];
                    headicon = 'V';
                    headpositioncursor[head] = headicon; //nustato pradine galvos padeti nuo kur pradeti tiuringo masina

                }

            }
            public void Cout(string halted="")
            {
                string duomuo = $"\nSteps: {steps}\n{new string(headpositioncursor)}\n{tape}\n{halted}\n";
                duomenys[id] = duomuo;
            }

            public void MainTuring()
            {
        
                
                bool outofbonds = false;
                bool nomoves = false;
                int okpoz;
                string nstatenow = "0";
                int tapelength = 0;
                while (outofbonds != true && nomoves != true)
                {
                    Thread.Sleep(70);
                    okpoz = 0;
                    for (int i = 0; i < c_state.Length; i++)
                    {
                        if (c_state[i] == nstatenow && tape[head] == c_symbol[i])
                        {
                            okpoz = i;
                            nstatenow = n_state[i];
                            break;
                        }
                        else okpoz = -1;
                    }
                    if (okpoz == -1)
                    {
                        nomoves = true;
                        break;
                    }
                    if (head+1 == tape.Length && directionarray[okpoz] == 'R')
                    {
                        outofbonds = true;
                        tape = tape.Remove(head, 1);
                        tapelength = tape.Length;
                        tape = tape.Substring(0, head) + Char.ToString(n_symbol[okpoz]) + tape.Substring(head, tapelength - head);
                        steps++;
                        break;
                        
                    }
                    else if (head == 0 && directionarray[okpoz] == 'L')
                    {
                        outofbonds = true;
                        tape = tape.Remove(head, 1);
                        tapelength = tape.Length;
                        tape = tape.Substring(0, head) + Char.ToString(n_symbol[okpoz]) + tape.Substring(head, tapelength - head);
                        steps++;
                        break;    
                    }
                    tape = tape.Remove(head, 1);
                    tapelength = tape.Length;
                    tape = tape.Substring(0, head) + Char.ToString(n_symbol[okpoz]) + tape.Substring(head, tapelength - head);
                    if (directionarray[okpoz] == 'R')
                    {
                        headpositioncursor[head] = ' ';
                        head++;
                        headpositioncursor[head] = headicon;

                    }
                    else if (directionarray[okpoz] == 'L')
                    {
                        headpositioncursor[head] = ' ';
                        head--;
                        headpositioncursor[head] = headicon;
                    }
                    steps++;
                    Cout();
                }
                
                Cout("Halted!!");
                
            }
            



        }
        static void Main(string[] args)
        {
            Menu();
            Turing_Machine tm = new Turing_Machine();
            Turing_Machine tm1 = new Turing_Machine();
            Turing_Machine tm2 = new Turing_Machine();
            Turing_Machine tm3 = new Turing_Machine();

            tm.ReadAndSetData();
            tm1.ReadAndSetData();
            tm2.ReadAndSetData();
            tm3.ReadAndSetData();
         
            Thread thr = new Thread(new ThreadStart(tm.MainTuring));
            Thread thr1 = new Thread(new ThreadStart(tm1.MainTuring));
            Thread thr2 = new Thread(new ThreadStart(tm2.MainTuring));
            Thread thr3 = new Thread(new ThreadStart(tm3.MainTuring));


            thr.Start();
            thr1.Start();
            thr2.Start();
            thr3.Start();
      
            while (true)
            {
                Console.Clear();
                foreach(int ID in duomenys.Keys)
                {
                    Console.WriteLine(duomenys[ID]);
                    
                }
                Thread.Sleep(30);
            }
            

           
        }
    }
}
