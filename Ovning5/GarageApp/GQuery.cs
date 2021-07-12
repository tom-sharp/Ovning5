using System;
using GarageApp.Interfaces;
using Misc;

namespace GarageApp
{
	public class GQuery : IQGarage
	{
		public bool FilterRegistrationNumberActive { get; set; }
		public bool FilterVehicleTypeActive { get; set; }
		public bool FilterColorActive { get; set; }


		public string SelectRegistrationNumber { get; set; }

		public void Clear() {
			int i1;
			this.FilterRegistrationNumberActive = false;
			this.FilterVehicleTypeActive = false;
			this.FilterColorActive = false;
			this.SelectRegistrationNumber = "";
			i1 = 0;
			while (i1 < this.SelectedVehicleTypes.Length) { this.SelectedVehicleTypes[i1++] = 0; }
			i1 = 0;
			while (i1 < this.SelectedVehicleColors.Length) { this.SelectedVehicleColors[i1++] = 0; }
		}

		public void SelectVehicleType(Factory.VType vehicletype) {
			switch (vehicletype) {
				case Factory.VType.Unknown: this.SelectedVehicleTypes[1] = vehicletype; break;
				case Factory.VType.Car: this.SelectedVehicleTypes[2] = vehicletype; break;
				case Factory.VType.Motorcycle: this.SelectedVehicleTypes[3] = vehicletype; break;
				case Factory.VType.Bus: this.SelectedVehicleTypes[4] = vehicletype; break;
				case Factory.VType.Boat: this.SelectedVehicleTypes[5] = vehicletype; break;
				case Factory.VType.Airplane: this.SelectedVehicleTypes[6] = vehicletype; break;
				default: BugCheck.Log(this, "SelectVehicleType:: Unhandled Vehicletype"); break;
			}
		}

		public void SelectVehicleColor(Factory.VColor vehiclecolor) {
			switch (vehiclecolor)
			{
				case Factory.VColor.Unknown: this.SelectedVehicleColors[1] = vehiclecolor; break;
				case Factory.VColor.Red: this.SelectedVehicleColors[2] = vehiclecolor; break;
				case Factory.VColor.Blue: this.SelectedVehicleColors[3] = vehiclecolor; break;
				case Factory.VColor.Green: this.SelectedVehicleColors[4] = vehiclecolor; break;
				case Factory.VColor.White: this.SelectedVehicleColors[5] = vehiclecolor; break;
				default: BugCheck.Log(this, "SelectVehicleColor:: Unhandled Vehiclecolor"); break;
			}
		}

		public GQuery()
		{
			this.FilterRegistrationNumberActive = false;
			this.FilterVehicleTypeActive = false;
			this.FilterColorActive = false;
			this.SelectedVehicleTypes = new Factory.VType[Factory.VTypeName.Length];
			this.SelectedVehicleColors = new Factory.VColor[Factory.VColorsName.Length];
		}

		public GQuery(string registrationnumber) : this()
		{
			if (registrationnumber != null) {
				this.FilterRegistrationNumberActive = true;
				this.SelectRegistrationNumber = registrationnumber;
			}
		}

		public bool IsSelected(IVehicle v)
		{
			if ((this.FilterOnVehicleRegNo(v)) &&
				(this.FilterOnVehicleType(v)) &&
				(this.FilterOnVehicleColor(v))) return true;
			return false;

		}


		private Factory.VType[] SelectedVehicleTypes;

		private Factory.VColor[] SelectedVehicleColors;

		private bool FilterOnVehicleRegNo(IVehicle v)
		{
			if (this.FilterRegistrationNumberActive)
			{
				return v.Regnumber.Contains(this.SelectRegistrationNumber, StringComparison.CurrentCultureIgnoreCase);
			}
			return true;
		}

		private bool FilterOnVehicleType(IVehicle v)
		{
			if (this.FilterVehicleTypeActive)
			{
				foreach (var vt in this.SelectedVehicleTypes)
					if (vt == v.VehicleType) return true;
				return false;
			}
			return true;
		}
		private bool FilterOnVehicleColor(IVehicle v)
		{
			if (this.FilterColorActive)
			{
				foreach (var vc in this.SelectedVehicleColors)
					if (vc == v.Color) return true;
				return false;
			}
			return true;
		}
	}
}
