using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Misc;
using GarageApp.Interfaces;
using GarageApp.Vehicles;
namespace GarageApp
{

	class ConUI : IUI
	{
		private ConIO ui =  ConIO.PInstance;
		private ConMenu menu = new ConMenu(x: 20, y: 5);
		IGarageManager gmanager;
		IGarageStats current = null;
		private bool cheat = false;

		public void Run(IGarageManager manager)
		{
			this.gmanager = manager;
			bool quit = false;
			int ret;
			while (!quit)
			{
				this.BuildMainMenu();
				this.ui.ClearScrn();
				ret = this.menu.GetMenuSelection();
				switch (ret)
				{
					case 0: quit = true; break;
					case 10: ManageGaragesMenu(); break;
					case 11: QuickSetUp(); break;
					default:
						// as menu is dynamic have to catch all dynamic here
						if (this.gmanager.SelectGarage(ret)) {
							this.current = this.gmanager.Garage;
							if (this.current != null) this.OperateGarageMainMenu();
							else BugCheck.Log(this, "Current Garage is nullptr");
						}
						break;
				}
			}
		}

		public void Msg(string msg) { this.ui.ShowStatus(msg); }
		public void ErrMsg(string msg) { this.ui.ShowErr(msg); }


		// Operate selected garage
		private void OperateGarageMainMenu()
		{
			bool quit = false;
			int ret;
			while (!quit)
			{
				this.BuildOperateGarageMenu();
				this.ui.ClearScrn();
				ret = this.menu.GetMenuSelection();
				switch (ret)
				{
					case 0: quit = true; break;
					case 1: OperateCheckin(); break;
					case 2: OperateCheckOut(); break;
					case 3: OperateFindRegNo(); break;
					case 4: OperateFindAdvanced(); break;
					case 5: OperateStatistics(); break;
					default:
							BugCheck.Log(this, "OperateGarage Unhandled menu option");
						break;
				}
			}
		}

		private void OperateStatistics() {
			var query = new GQuery();
			var result = this.gmanager.LinqQuery(query);
			int[] counter = new int[30];
			ui.ClearScrn();
			ui.Show($"\nStatistics for garage {this.gmanager.Garage.Name}: \n\n");
			foreach (var v in result)
			{
				switch (v.VehicleType) {
					case Factory.VType.Unknown: counter[1]++; break;
					case Factory.VType.Car: counter[2]++; break;
					case Factory.VType.Motorcycle: counter[3]++; break;
					case Factory.VType.Bus: counter[4]++; break;
					case Factory.VType.Boat: counter[5]++; break;
					case Factory.VType.Airplane: counter[6]++; break;
					default:
						counter[0]++;
						break;
				}
			}
			ui.Show($" {"Vehicle type",-20} Number of vehicles\n");

			ui.Show($" {Factory.GetVTypeName(Factory.VType.Car),-20} {counter[2],10}\n");
			ui.Show($" {Factory.GetVTypeName(Factory.VType.Motorcycle),-20} {counter[3],10} \n");
			ui.Show($" {Factory.GetVTypeName(Factory.VType.Bus),-20} {counter[4],10} \n");
			ui.Show($" {Factory.GetVTypeName(Factory.VType.Boat),-20} {counter[5],10} \n");
			ui.Show($" {Factory.GetVTypeName(Factory.VType.Airplane),-20} {counter[6],10} \n");

			ui.ShowStatus($" Summary: Total {result.Count()} vehicles in the garage");


		}

		private void OperateFindAllVehicles() {
			this.OperateFind(new GQuery());
		}

		private void OperateFindRegNo() {
			var query = new GQuery();
			query.FilterRegistrationNumberActive = true;
			while (true)
			{
				ui.ClearScrn();
				ui.Show($"\nSearch for vehicles in garage {this.gmanager.Garage.Name}\n");
				query.SelectRegistrationNumber = this.InputRegistrationNumber();
				this.OperateFind(query);
				if (!ui.GetYN("Do you want to make another search (y/n) ?")) break;
			}
		}

		private void OperateFindAdvanced() {
			var query = new GQuery();

			while (true)
			{
				ui.ClearScrn();
				ui.Show($"\nAdvanced Search in garage {this.gmanager.Garage.Name}\n\n");

				this.FindSelectRegistrationNumber(query);
				ui.Show("\n");
				this.FindSelectVTypes(query);
				ui.Show("\n");
				this.FindSelectVColors(query);
				this.OperateFind(query);

				if (!ui.GetYN("Do you want to make another search (y/n) ?")) break;
				query.Clear();
			}
		}

		private void FindSelectRegistrationNumber(GQuery query) {
			query.FilterRegistrationNumberActive = false;
			query.SelectRegistrationNumber = ui.GetString("Search for registration number (leave emtpy for all): ");
			if (query.SelectRegistrationNumber.Length > 0) query.FilterRegistrationNumberActive = true;
		}

		private void FindSelectVTypes(GQuery query) {
			var menu = new ConMenu();
			int i1;
			int[] ret = null;

			menu.Clear();
			menu.MenuName = "Vehicle type: ";
			menu.AddMenuItem(1, Factory.GetVTypeName(Factory.VType.Unknown));
			menu.AddMenuItem(2, Factory.GetVTypeName(Factory.VType.Car));
			menu.AddMenuItem(3, Factory.GetVTypeName(Factory.VType.Motorcycle));
			menu.AddMenuItem(4, Factory.GetVTypeName(Factory.VType.Bus));
			menu.AddMenuItem(5, Factory.GetVTypeName(Factory.VType.Boat));
			menu.AddMenuItem(6, Factory.GetVTypeName(Factory.VType.Airplane));
			ret = menu.GetMenuMultiSelection("\nSearch for (leave empty for all): ", itemwidth: 12);
			if (ret != null)
			{
				query.FilterVehicleTypeActive = true;
				i1 = 0;
				while (i1 < ret.Length) {
					switch (ret[i1]) {
						case 1: query.SelectVehicleType(Factory.VType.Unknown); break;
						case 2: query.SelectVehicleType(Factory.VType.Car); break;
						case 3: query.SelectVehicleType(Factory.VType.Motorcycle); break;
						case 4: query.SelectVehicleType(Factory.VType.Bus); break;
						case 5: query.SelectVehicleType(Factory.VType.Boat); break;
						case 6: query.SelectVehicleType(Factory.VType.Airplane); break;
						default: BugCheck.Log(this, "FindSelectVehicleTypes:: Unhandled menu option"); break;
					}
					i1++;
				}
			}
			else query.FilterVehicleTypeActive = false;
		}

		private void FindSelectVColors(GQuery query)
		{
			var menu = new ConMenu();
			int i1;
			int[] ret = null;

			menu.Clear();
			menu.MenuName = "Vehicle Color: ";
			menu.AddMenuItem(1, Factory.GetVColorName(Factory.VColor.Unknown));
			menu.AddMenuItem(2, Factory.GetVColorName(Factory.VColor.Red));
			menu.AddMenuItem(3, Factory.GetVColorName(Factory.VColor.Green));
			menu.AddMenuItem(4, Factory.GetVColorName(Factory.VColor.Blue));
			menu.AddMenuItem(5, Factory.GetVColorName(Factory.VColor.White));

			ret = menu.GetMenuMultiSelection("\nSearch for (leave empty for all): ",itemwidth: 12);
			if (ret != null)
			{
				query.FilterColorActive = true;
				i1 = 0;
				while (i1 < ret.Length)
				{
					switch (ret[i1])
					{
						case 1: query.SelectVehicleColor(Factory.VColor.Unknown); break;
						case 2: query.SelectVehicleColor(Factory.VColor.Red); break;
						case 3: query.SelectVehicleColor(Factory.VColor.Green); break;
						case 4: query.SelectVehicleColor(Factory.VColor.Blue); break;
						case 5: query.SelectVehicleColor(Factory.VColor.White); break;
						default: BugCheck.Log(this, "FindSelectVColors:: Unhandled menu option"); break;
					}
					i1++;
				}
			}
			else query.FilterColorActive = false;
		}

		private void OperateFind(GQuery query) {
			if (query == null) { BugCheck.Log(this, "Unexpected nullptr argument"); return; }
			var result = this.gmanager.LinqQuery(query);
			ui.ClearScrn();
			ShowVehicleHeader();
			foreach (var v in result) {
				this.ShowVehicle(v);
			}
			ui.ShowStatus($" Search Resulted in {result.Count()} hits. Press any key to continue");
		}

		private void OperateCheckOut()
		{
			ui.ClearScrn();
			ui.Show($"\nCheck out vehicles from garage {this.gmanager.Garage.Name}   Free {this.gmanager.Garage.Free}\n");
			while (true)
			{
				if (this.gmanager.CheckOut(this.InputRegistrationNumber()))
					ui.ShowStatus("Successfully Checked out vehicle");
				else ui.ShowErr("Could not checkout this vehicle, it was not found");

				if (!ui.GetYN("Do you want to Check out another vehicle (y/n) ?")) break;
			}
		}


		private void OperateCheckin() {
			Factory.VType vt;
			string regno;
			IVehicle newVehicle = null;

			while (true) {
				ui.ClearScrn();
				ui.Show($"\nCheck in vehicles to garage {this.gmanager.Garage.Name}   Free {this.gmanager.Garage.Free}\n");
				vt = this.SelectVehicleType();
				regno = this.InputRegistrationNumber();
				newVehicle = Factory.GetNewVehicle(vt, regno);

				if (newVehicle.IsValid())
				{
					this.SetVehicleInfo(newVehicle);
					if (this.gmanager.CheckIn(newVehicle))
					{
						ui.ShowStatus($"Successfully Checked in Vehicle");
					}
					else 
					{ 
						ui.ShowErr("Checkin rejected vehicle. Garage is full, or there is an identical registration number parked here already");
					}
				}
				else
				{
					ui.ShowErr("Registration number can not be empty");
				}

				if (!ui.GetYN("Do you want to CheckIn another vehicle (y/n) ?")) break;
			}
		}

		private void SetVehicleInfo(IVehicle vehicle) {

			vehicle.MakeModel = ui.GetString("Enter Make and Model: ");
			vehicle.Color = SelectVehicleColor();

			switch (vehicle.VehicleType) {
				case Factory.VType.Car: this.SetCarInfo(vehicle as Car); break;
				case Factory.VType.Bus: this.SetBusInfo(vehicle as Bus); break;
				case Factory.VType.Motorcycle: this.SetMotorcycleInfo(vehicle as Motorcycle); break;
				case Factory.VType.Boat: this.SetBoatInfo(vehicle as Boat); break;
				case Factory.VType.Airplane: this.SetAirplaneInfo(vehicle as Airplane); break;
				default:
					BugCheck.Log(this, "Unhandled Vehicle type");
					break; ;
			}
		}

		private void SetCarInfo(Car car)
		{
			car.IsElectric = ui.GetYN("Is this an electric car (y/n) ? ");
		}
		private void SetBusInfo(Bus bus)
		{
			bus.Passengers = ui.GetNumber("Enter Passanger capacity: ", required: true);
		}
		private void SetMotorcycleInfo(Motorcycle motorcycle)
		{
			motorcycle.IsOffroad = ui.GetYN("Is this an offroad bike (y/n) ? ");
		}
		private void SetBoatInfo(Boat boat)
		{
			boat.Length = ui.GetNumber("Enter Boat length (feet): ", required: true);
		}
		private void SetAirplaneInfo(Airplane airplane)
		{
			airplane.Wingspan = ui.GetNumber("Enter Wing span (m): ", required: true);
		}

		private void QuickSetUp() {
			this.cheat = true;
			try
			{
				this.gmanager.QuickSetup();
				ui.ShowStatus(" Quick Setup: Successfully created five garages and checked in 899 vehicles into garage 1");
			}
			catch { 
				ui.ShowErr(" Quick Setup: tried to created five garages and check in 899 vehicles into garage 1, but something went wrong.");
			}
		}

		// Manage Garages
		private void ManageGaragesMenu()
		{
			bool quit = false;
			int ret;
			while (!quit)
			{
				this.BuildManageGaragesMenu();
				this.ui.ClearScrn();
				ret = this.menu.GetMenuSelection();
				switch (ret)
				{
					case 0: quit = true; break;
					case 10: this.AddNewGarage(); break;
					default:
						ui.ShowErr("Sorry this is not implemented yet");
						BugCheck.Log(this, "ManageGarage Unhandled menu option");
						break;
				}
			}
		}

		// Add a new garage
		private void AddNewGarage()
		{
			string name = "";
			int capacity = 0;
			ui.ClearScrn();
			ui.ShowXY(20, 5, "ADD A NEW GARAGE");
			ui.ShowXY(20, 7, "Enter name: "); name = ui.GetString();
			if (name.Length == 0)
			{
				ui.ShowXY(20, 8, $"A garage must have a name");
				ui.GetString();
				return;
			}
			ui.ShowXY(20, 8, "Enter Capacity: "); capacity = ui.GetNumber();
			if (capacity <= 0)
			{
				ui.ShowXY(20, 9, $"Please return and create a new garage when it has some");
				ui.GetString();
				return;
			}
			this.gmanager.AddNewGarage(name, capacity);
		}

		private void BuildMainMenu()
		{
			this.menu.Clear();
			this.menu.MenuName = "GARAGE MAIN MENU";
			this.menu.AddMenuItem(99, "");
			foreach (var g in this.gmanager.GetGarages())
			{
				this.menu.AddMenuItem(g.Id, String.Format($"Operate: {g.Name} ({g.Capacity})"));
			}
			this.menu.AddMenuItem(99, "");
			this.menu.AddMenuItem(10, "Manage garages");
			if (!this.cheat)this.menu.AddMenuItem(11, "QuickSetUp (use only once)");
			this.menu.AddMenuItem(0, "Exit");
		}

		private void BuildManageGaragesMenu()
		{
			this.menu.Clear();
			this.menu.MenuName = "MANAGE GARAGES";
			this.menu.AddMenuItem(99, "");
			foreach (var g in this.gmanager.GetGarages())
			{
				this.menu.AddMenuItem(g.Id, String.Format($"Manage: {g.Name} ({g.Capacity})"));
			}
			this.menu.AddMenuItem(99, "");
			this.menu.AddMenuItem(10, "Add a new Garage");
			this.menu.AddMenuItem(0, "Back to main meny");
		}

		private void BuildOperateGarageMenu()
		{
			this.menu.Clear();
			this.menu.MenuName = $"Operate Garage: {this.current.Name}";
			this.menu.AddMenuItem(99, "");
			this.menu.AddMenuItem(1, "Check in vehicle");
			this.menu.AddMenuItem(2, "Check out vehicle");
			this.menu.AddMenuItem(3, "Find Vehicle(s)");
			this.menu.AddMenuItem(4, "Advanced Search");
			this.menu.AddMenuItem(5, "Show garage statistics");
			this.menu.AddMenuItem(99, "");
			this.menu.AddMenuItem(0, "Back to main menu");
		}


		private Factory.VType SelectVehicleType() {
			ConMenu vtmenu = new ConMenu();
			int ret;
			// build menu
			vtmenu.MenuName = "";
			vtmenu.AddMenuItem(1, Factory.GetVTypeName(Factory.VType.Car));
			vtmenu.AddMenuItem(2, Factory.GetVTypeName(Factory.VType.Bus));
			vtmenu.AddMenuItem(3, Factory.GetVTypeName(Factory.VType.Motorcycle));
			vtmenu.AddMenuItem(4, Factory.GetVTypeName(Factory.VType.Boat));
			vtmenu.AddMenuItem(5, Factory.GetVTypeName(Factory.VType.Airplane));
			while (true) {
				ret = vtmenu.GetMenuSelection("Select Vehicle type: ");
				switch (ret) {
					case 1: return Factory.VType.Car;
					case 2: return Factory.VType.Bus;
					case 3: return Factory.VType.Motorcycle;
					case 4: return Factory.VType.Boat;
					case 5: return Factory.VType.Airplane;
					default: ui.Show("Invalid selection\n"); break;
				}
			}
		}
		private Factory.VColor SelectVehicleColor()
		{
			ConMenu vtmenu = new ConMenu();
			int ret;
			// build menu
			vtmenu.MenuName = "";
			vtmenu.AddMenuItem(1, Factory.GetVColorName(Factory.VColor.Unknown));
			vtmenu.AddMenuItem(2, Factory.GetVColorName(Factory.VColor.Red));
			vtmenu.AddMenuItem(3, Factory.GetVColorName(Factory.VColor.Green));
			vtmenu.AddMenuItem(4, Factory.GetVColorName(Factory.VColor.Blue));
			vtmenu.AddMenuItem(5, Factory.GetVColorName(Factory.VColor.White));
			while (true)
			{
				ret = vtmenu.GetMenuSelection("Select vehicle color: ");
				switch (ret)
				{
					case 1: return Factory.VColor.Unknown;
					case 2: return Factory.VColor.Red;
					case 3: return Factory.VColor.Green;
					case 4: return Factory.VColor.Blue;
					case 5: return Factory.VColor.White;
					default: ui.Show("Invalid selection\n"); break;
				}
			}
		}


		private string InputRegistrationNumber() => ui.GetString("Enter Registration number: ");


		private void ShowVehicleHeader()
		{
			ui.Show($"\nGarage {this.gmanager.Garage.Id}  {this.gmanager.Garage.Name}   Capacity:  {this.gmanager.Garage.Capacity} (Free {this.gmanager.Garage.Free})\n\n");
			ui.Show(" RegNo".PadRight(16) + "Vehicle type".PadRight(20) + "Color".PadRight(10) + "Make/Model".PadRight(15) + "Properties\n");
		}
		private void ShowVehicle(IVehicle vehicle)
		{
			ui.Show($" {vehicle.Regnumber,-15}{vehicle.Name,-20}{vehicle.GetProperties()} \n");
		}

		private void ShowErrVehicleNotFound()
		{
			ui.ShowErr("Could not find any vehicle with that registration number");
		}


	}

}
