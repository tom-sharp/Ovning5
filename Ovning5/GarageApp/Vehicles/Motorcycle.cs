using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
namespace GarageApp.Vehicles
{
	public class Motorcycle : Vehicle
	{
		public override string GetProperties() { 
			if (this.IsOffroad) return $"{base.GetProperties()} Offroad    ";
			return $"{base.GetProperties()} Citybike   ";
		}

		public bool IsOffroad { get; set; }

		public Motorcycle(string regnumber, bool offroad) : base(regnumber, Factory.VType.Motorcycle)
		{
			this.IsOffroad = offroad;
		}
	}
}
