using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Misc;
using GarageApp;
using GarageApp.Interfaces;
namespace GarageApp.Vehicles
{
	public class Vehicle : IVehicle
	{
		public string Regnumber { get; }
		public Factory.VType VehicleType { get; }
		public Factory.VColor Color { get; set; }
		public string MakeModel { get; set; }
		public virtual string Name => Factory.GetVTypeName(this.VehicleType);

		public virtual string GetProperties() => $"{Factory.GetVColorName(this.Color),-10} {this.MakeModel,-15} ";

		public Vehicle(string regnumber, Factory.VType vt )
		{
			this.Regnumber = regnumber;
			this.VehicleType = vt;
			this.Color = Factory.VColor.Unknown;
		}

		public virtual bool IsValid() {
			if (this.Regnumber.IsEmpty()) return true;
			return true;
		}

	}
}
