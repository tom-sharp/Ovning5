using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
using GarageApp.Vehicles;
using Misc;

namespace GarageApp
{
	delegate void mydelegate(int x,int y);
	public static class Factory
	{
		public enum VColor { Invalid, Unknown, Red, Green, Blue, White };
		public static string[] VColorsName = { "Invalid", "Unknown", "Red", "Green", "Blue", "White" };

		public static string GetVColorName(VColor color)
		{
			switch (color)
			{
				case VColor.Unknown: return VColorsName[1];
				case VColor.Red: return VColorsName[2];
				case VColor.Green: return VColorsName[3];
				case VColor.Blue: return VColorsName[4];
				case VColor.White: return VColorsName[5];
			}
			return VTypeName[0];
		}


		public enum VType { Invalid, Unknown, Car, Bus, Boat, Airplane, Motorcycle };
		public static string[] VTypeName = { "Invalid", "Unknown", "Car", "Bus", "Boat", "Airplane", "Motorcycle" };


		public static string GetVTypeName(VType vehicleType)
		{
			switch (vehicleType)
			{
				case VType.Unknown: return VTypeName[1];
				case VType.Car: return VTypeName[2];
				case VType.Bus: return VTypeName[3];
				case VType.Boat: return VTypeName[4];
				case VType.Airplane: return VTypeName[5];
				case VType.Motorcycle: return VTypeName[6];
			}
			return VTypeName[0];
		}



		public static IVehicle GetNewVehicle(VType vehicletype, string regno) {
			switch (vehicletype)
			{
				case VType.Car: return new Car(regno, false);
				case VType.Bus:	return new Bus(regno, 0);
				case VType.Boat: return new Boat(regno, 0);
				case VType.Airplane: return new Airplane(regno, 0);
				case VType.Motorcycle: return new Motorcycle(regno, false);
				default:
					BugCheck.Log(null," Factory :: Unhandeld Vehicle type");
					return null;
			}
		}
	}
}
