using System.Security.Cryptography.X509Certificates;
using TextFile;
namespace Assignment2_H2G7BJ
{
    public class Program
    {
        public class UniqueGas
        {
            public HashSet<Gases.Type> UniqueGasCount(ref List<Gases> Layers)
            {
                HashSet<Gases.Type> uniqueTypes = new HashSet<Gases.Type>();

                foreach (var layer in Layers)
                {
                    uniqueTypes.Add(layer.type);
                }

                return uniqueTypes;
            }
        }
        public class GasSimulationForTesting
        {
            public List<Gases> SimulateGasLayers(List<Gases> Layers, List<IAtmosphericVar> weather)
            {
                UniqueGas Unique = new UniqueGas();
                HashSet<Gases.Type> initialUniqueGasCount = new();
                initialUniqueGasCount = Unique.UniqueGasCount(ref Layers);
                List<Gases.Type> LayersType = new();
                for (int j = 0; j < Layers.Count; j++)
                { LayersType.Add(Layers[j].type); }

                for (int i = 0, round = 0; (initialUniqueGasCount.ToList()).All(gas => LayersType.Contains(gas)) && weather.Any() && Layers.Any(); ++i, ++round)
                {
                    if (i == weather.Count)
                    {
                        i = 0;
                    }
                    List<int> tempind = new();
                    bool exist = false;
                    bool end = false;
                    if (end) break;
                    int initialLayerCount = Layers.Count;
                    int ind = 0;
                    while (ind < Layers.Count)
                    {
                        Layers[ind].Simulate(weather[i], ref Layers, ind, out double new_t);

                        if (initialLayerCount != Layers.Count)
                        {
                            initialLayerCount = Layers.Count;
                            ind += 2;
                        }
                        else
                        {
                            ind++;
                        }
                    }
                    for (int j = 0; j < Layers.Count; ++j)
                    {
                        if (!Layers[j].Exist())
                        {
                            tempind.Add(j);
                            exist = tempind.Any();
                        }
                    }
                    int x = 1;
                    for (; tempind.Any();)
                    {
                        if (x > 1)
                        {
                            tempind[0]--;
                        }
                        Layers.RemoveAt(tempind[0]);
                        x++;
                        tempind.RemoveAt(0);

                    }
                    exist = tempind.Any();
                    LayersType.Clear();
                    for (int j = 0; j < Layers.Count; j++)
                    { LayersType.Add(Layers[j].type); }
                }
                return Layers;
            }

        }
 
        static void Main(string[] args)
        {
            
            TextFileReader reader = new("input.txt");

            //filling the atmosphere
            reader.ReadLine(out string line); int n = int.Parse(line);
            List<Gases> Layers = new();
            for (int i = 0; i < n; i++)
            {
                char seperator = ' ' ;
                Gases? gas = null;
                if (reader.ReadLine(out line))
                {
                    string[] tokens = line.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                    char ch = char.Parse(tokens[0]);
                    double thickness = double.Parse(tokens[1]);

                    switch (ch)
                    {
                        case 'Z': gas = new Ozone(Gases.Type.Ozone,thickness); break;
                        case 'X': gas = new Oxygen(Gases.Type.Oxygen,thickness); break;
                        case 'C': gas = new CarbonD(Gases.Type.CarbonD,thickness); break;
                    }
                }
                Layers.Add(gas);
            }

            //adding atmospheric variables
            List<IAtmosphericVar> weather = new();
            while (reader.ReadChar(out char ch))
            {
                switch (ch)
                {
                    case 'T': weather.Add(Thunderstorm.Instance()); break;
                    case 'S': weather.Add(Sunshine.Instance()); break;
                    case 'O': weather.Add(Other.Instance()); break;
                }
            }

            //simulating the rounds
            try
            {
                UniqueGas Unique = new UniqueGas();
                HashSet<Gases.Type> initialUniqueGasCount = new();
                initialUniqueGasCount = Unique.UniqueGasCount(ref Layers);
                List<Gases.Type> LayersType = new();
                int round = 0;
                for (int j = 0; j < Layers.Count; j++)
                { LayersType.Add(Layers[j].type); }
                Console.WriteLine($"\nRound 0(before simulation):");
                for (int i = 0; i < Layers.Count; i++)
                {
                    Console.WriteLine($"\t{Layers[i].type}: {Layers[i].GetThickness()} \t");
                }
                for (int i = 0; (initialUniqueGasCount.ToList()).All(gas => LayersType.Contains(gas)) && weather.Any() && Layers.Any(); ++i, ++round)
                {
                    if (i == weather.Count)
                    {
                        i = 0;
                    }
                    List<int> tempind = new();
                    bool exist = false;
                    bool end = false;
                    if (end) break;
                    Console.WriteLine($"\nRound {round+1}:");
                    int initialLayerCount = Layers.Count;
                    int ind = 0;
                    while (ind < Layers.Count)
                    {
                        Layers[ind].Simulate(weather[i], ref Layers, ind, out double new_t);

                        if (initialLayerCount != Layers.Count)
                        {
                            initialLayerCount = Layers.Count;
                            ind+=2;
                        }
                        else
                        {
                            ind++; 
                        }
                    }
                    for (int j = 0; j < Layers.Count; ++j)
                    {
                        Console.WriteLine($"\t{Layers[j].type}: {Layers[j].GetThickness():F5} \t");
                        if (!Layers[j].Exist())
                        {
                            tempind.Add(j);
                            exist = tempind.Any();
                        }
                    }
                    int x = 1;
                    for (; tempind.Any();)
                    {
                        if (x > 1)
                        {
                            tempind[0]--;
                        }
                            Console.Write($"\t{Layers[tempind[0]].type} at Layer {tempind[0] + x} has been eliminated");
                            Layers.RemoveAt(tempind[0]);
                            x++;
                            tempind.RemoveAt(0);

                    }
                    exist = tempind.Any();
                    LayersType.Clear();
                    for (int j = 0; j < Layers.Count; j++)
                    { LayersType.Add(Layers[j].type);  }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.ToString());
            }

        }
    }
}