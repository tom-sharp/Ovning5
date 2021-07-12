using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp;
namespace GarageApp.Interfaces
{
	public interface IVehicle
	{
		public string Name { get; }
		public string Regnumber { get; }
		public Factory.VType VehicleType { get; }
		public Factory.VColor Color { get; set; }
		public string MakeModel { get; set; }
		public bool IsValid();
		public string GetProperties();
	}

}
