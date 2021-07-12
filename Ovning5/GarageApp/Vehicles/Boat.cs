using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
namespace GarageApp.Vehicles
{
	public class Boat : Vehicle
	{
		public override string Name => "Boat";
		public override string GetProperties() { return $"{base.GetProperties()} Length: {this.Length}"; }
		public int Length { get; set; }

		public Boat(string regnumber, int length) : base(regnumber, Factory.VType.Boat)
		{
			this.Length = length;
		}
	}
}
