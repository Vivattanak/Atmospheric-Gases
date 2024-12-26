using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2_H2G7BJ
{
    //class of abstract atmospheric variable
    public interface IAtmosphericVar
    {
        double changeOzone(Ozone t, List<Gases> Layers, int index);
        double changeOxygen(Oxygen t, List<Gases> Layers, int index);
        double changeCarbonD(CarbonD t, List<Gases> Layers, int index);
    }
    //class of thunderstorm
    public class Thunderstorm : IAtmosphericVar
    {
        public double changeOzone(Ozone t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(0.0, out double new_t);
            return t.GetThickness();
        }
        public double changeOxygen(Oxygen t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(50.0, out double new_t);
            t.AddThickness(ref Layers, index, new_t, Gases.Type.Ozone);
            return t.GetThickness();
        }
        public double changeCarbonD(CarbonD t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(0.0, out double new_t);
            return t.GetThickness();
        }
        private Thunderstorm() { }

        private static Thunderstorm? instance = null;
        public static Thunderstorm Instance()
        {
            if (instance == null)
            {
                instance = new Thunderstorm();
            }
            return instance;
        }
    }
    //class of sunshine
    public class Sunshine : IAtmosphericVar
    {
        public double changeOzone(Ozone t, List<Gases> Layers,  int index)
        {
            t.ModifyThickness(0.0, out double new_t);
            return t.GetThickness();
        }
        public double changeOxygen(Oxygen t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(5.0, out double new_t);
            t.AddThickness(ref Layers, index, new_t, Gases.Type.Ozone);
            return t.GetThickness();
        }
        public double changeCarbonD(CarbonD t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(5.0, out double new_t);
            t.AddThickness(ref Layers, index, new_t, Gases.Type.Oxygen);
            return t.GetThickness();
        }
        private Sunshine() { }
        private static Sunshine? instance = null;

        public static Sunshine Instance()
        {
            if (instance == null)
            {
                instance = new Sunshine();
            }
            return instance;
        }
    }
    //class of other effects
    public class Other : IAtmosphericVar
    {
        public double changeOzone(Ozone t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(5.0, out double new_t); 
            t.AddThickness(ref Layers, index, new_t, Gases.Type.Oxygen);
            return t.GetThickness();
        }
        public double changeOxygen(Oxygen t, List<Gases> Layers, int index)
        {   
            t.ModifyThickness(10.0, out double new_t);
            t.AddThickness(ref Layers, index, new_t, Gases.Type.CarbonD);
            return t.GetThickness();
        }
        public double changeCarbonD(CarbonD t, List<Gases> Layers, int index)
        {
            t.ModifyThickness(0.0, out double new_t);
            return t.GetThickness();
        }
        private Other() { }
        private static Other? instance = null;

        public static Other Instance()
        {
            if (instance == null)
            {
                instance = new Other();
            }
            return instance;
        }
    }
}
