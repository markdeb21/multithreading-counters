using MultithreadingCounters;

var input = 0;
var counters = new List<Counter>();

while (input != 4)
{
    Console.Clear();
    Console.WriteLine(@"1. Iniciar un contador.
2. Detener un contador.
3. Mostrar el estado actual de los contadores.
4. Salir del programa, deteniendo todos los hilos activos.");
    var stringInput = Console.ReadLine() ?? "";
    var converted = StringToInt(stringInput, ref input);

    if (!converted)
    {
        Console.Clear();
        Console.WriteLine($"'{stringInput}' is not a valid option.");
        Thread.Sleep(1000);
    }

    Console.Clear();

    switch (input)
    {
        case 1:
            Console.Write($"Introduce un intervalo para el contador #{counters.Count + 1}: ");
            var intervalString = Console.ReadLine() ?? "";
            int interval = -1;

            if (StringToInt(intervalString, ref interval))
            {
                var counter = new Counter(Math.Abs(interval));
                counters.Add(counter);

                var indexOfCounter = counters.IndexOf(counter);

                counters[indexOfCounter].Start();
                Console.Write($"El contador #{indexOfCounter} ha sido iniciado con éxito");
            }

            break;

        case 2:

            if (counters.Count > 0)
            {

                for (int i = 0; i < counters.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Contador #{i + 1}");
                }

                Console.Write($"Introduce el contador a detener: ");
                var counterToStopString = Console.ReadLine() ?? "";

                int counterToStop = -1;

                if (StringToInt(counterToStopString, ref counterToStop))
                {
                    if (counterToStop >= 0 && counterToStop < counters.Count)
                    {
                        counters[counterToStop - 1].Stop();
                        counters.Remove(counters[counterToStop - 1]);
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write($"El contador #{counterToStop} no es válido.");

                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                PrintNoCounters();
            }

            break;

        case 3:
            if (counters.Count > 0)
            {
                Console.Clear();

                bool monitoring = true;

                Thread monitorThread = new Thread(() =>
                {

                    Console.WriteLine("Para salir del modo monitor, presiona ESC\n");
                    for (int i = 0; i < counters.Count; i++)
                    {
                        Console.WriteLine($"Contador #{i + 1} = {counters[i].GetValue()}");
                    }

                    while (monitoring)
                    {
                        for (int i = 0; i < counters.Count; i++)
                        {
                            Console.SetCursorPosition(0, i + 2);
                            Console.Write($"Contador #{i + 1} = {counters[i].GetValue()}    ");
                        }

                        Thread.Sleep(100);
                    }
                });

                monitorThread.Start();

                while (monitoring)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true).Key;
                        if (key == ConsoleKey.Escape)
                        {
                            monitoring = false;
                            monitorThread.Join();
                            break;
                        }
                    }

                    Thread.Sleep(50);
                }
            }
            else
            {
                PrintNoCounters();
            }
            break;

        case 4:

            if (counters.Count > 0)
            {
                for (int i = 0; i < counters.Count; i++)
                {
                    Console.WriteLine($"Deteniendo el contador #{i + 1} de manera segura...");
                    counters[i].Stop();
                }

                counters.Clear();
                Console.Clear();
            }
            else
            {
                PrintNoCounters();
            }

            break;

        default:
            Console.WriteLine($"'{input}' is not a valid option.");
            break;
    }
}

bool StringToInt(string input, ref int target)
{
    return Int32.TryParse(input, out target);
}

void PrintNoCounters()
{
    Console.Clear();
    Console.WriteLine($"No hay contadores.");
    Thread.Sleep(1000);
}
