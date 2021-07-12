using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApp.Vehicles
{
	public class Airplane : Vehicle
	{
		public override string Name => "Airplane";
		public override string GetProperties() { return $"{base.GetProperties()} Wingspan: {this.Wingspan}"; }
		public int Wingspan { get; set; }

		public Airplane(string regnumber, int wingspan) : base(regnumber, Factory.VType.Airplane)
		{
			this.Wingspan = wingspan;
		}
	}
}
