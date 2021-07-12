using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApp.Vehicles
{
	public class Car : Vehicle
	{
		public override string Name => "Car";
		public bool IsElectric { get; set; }
		public override string GetProperties() { 
			if (this.IsElectric) return $"{base.GetProperties()} Electric    ";
			return $"{base.GetProperties()} Gasoline   ";
		}
		public Car(string regnumber, bool electric ) : base(regnumber, Factory.VType.Car)
		{
			this.IsElectric = electric;
		}

	}
}
