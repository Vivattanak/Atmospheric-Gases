using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2_H2G7BJ
{
    public abstract class Gases //im making the visibilty public in order to do testing
    {
        public enum Type { Ozone, Oxygen, CarbonD }
        public Type type { get; }
        protected double thickness;
        public void ModifyThickness(double t, out double new_t) 
        {
            new_t = (t * thickness) / 100.0;
            thickness -= new_t;
        }
        public void AddThickness(ref List<Gases> Layers, int index, double new_t, Type typeNeeded) 
        {
            for (int j = index; j != -1; j--)
            {
                if (Layers[j].type == typeNeeded)
                {
                    Layers[j].thickness += new_t;
                    return;
                }

            }
            if (new_t >= 0.5)
            {
                Gases? gas = null;
                switch (typeNeeded)
                {
                    case Type.Ozone: gas = new Ozone(Gases.Type.Ozone, new_t); break;
                    case Type.Oxygen: gas = new Oxygen(Gases.Type.Oxygen, new_t); break;
                    case Type.CarbonD: gas = new CarbonD(Gases.Type.CarbonD, new_t); break;
                }
                Layers.Insert(0, gas);
            }
        }
        public double GetThickness() { return thickness; }
        public bool Exist() { return thickness >= 0.5; }
        protected Gases(Type t, double e) { type = t; thickness = e; }
        public void Simulate(IAtmosphericVar weather, ref List<Gases> Layers, int index, out double new_t)
        {
            thickness = AffectedBy(weather, Layers, index);
            new_t = thickness;
            
        }
        protected abstract double AffectedBy(IAtmosphericVar a_Var, List<Gases> Layers, int index);
    }
        //class of ozone
    public class Ozone : Gases
    {
        public Ozone(Type type, double e = 0.0) : base(type , e) { }
        protected override double AffectedBy(IAtmosphericVar a_Var, List<Gases> Layers, int index)
        {
            return a_Var.changeOzone(this, Layers, index);
        }
    }
    //class of oxygen
    public class Oxygen : Gases
    {
         public Oxygen(Type type, double e = 0.0) : base(type, e) { }
         protected override double AffectedBy(IAtmosphericVar a_Var, List<Gases> Layers, int index)
         {
             return a_Var.changeOxygen(this, Layers, index);
         }
    }
    //class of carbon dioxide
    public class CarbonD : Gases
    {
         public CarbonD(Type type, double e = 0.0) : base(type, e) { }
         protected override double AffectedBy(IAtmosphericVar a_Var, List<Gases> Layers, int index)
         {
             return a_Var.changeCarbonD(this, Layers, index);
         }
    }

}
