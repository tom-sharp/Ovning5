using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
namespace GarageApp.Vehicles
{
	public class Bus : Vehicle
	{
		public override string Name => "Bus";
		public override string GetProperties() { return $"{base.GetProperties()} Passengers: {this.Passengers}"; }
		public int Passengers { get; set; }

		public Bus(string regnumber, int Passengers) : base(regnumber, Factory.VType.Bus)
		{

		}
	}
}
