using System;
using System.Collections.Generic;
using GarageApp.Interfaces;

namespace GarageApp
{
	public class GarageMain : IGarageManager
	{

		private IUI ui;
		private IHandler goperator = new GarageHandler();
		private int maxgarages;

		public GarageMain()
		{
			this.maxgarages = 8;
		}

		public void Start(IUI userinterface)
		{
			if (userinterface != null) {
				this.ui = userinterface;
				LoadGarageState();
				ui.Run(this);
				SaveGarageState();
			}
		}

		void LoadGarageState()
		{
			// not implemented yet
		}
		void SaveGarageState()
		{
			// not implemented yet
		}

		public void QuickSetup() {
			QuickSetup cheat = new QuickSetup();
			cheat.Setup(this.goperator);
		}

		public IGarageStats Garage { get { return this.goperator.Garage; } }

		public IEnumerable<IGarageStats> GetGarages() { return goperator.GetGarages(); }

		public bool AddNewGarage(string name, int capacity)
		{
			if (this.goperator.GaragesCount < this.maxgarages) {
				try
				{
					this.goperator.AddNewGarage(name, capacity);
					return true;
				}
				catch (ApplicationException ex) {
					ui.ErrMsg(ex.Message);
				}
			}
			else ui.ErrMsg($"Reached limit of garages Max {this.maxgarages}, please upgrade license");
			return false;
		}

		public bool SelectGarage(int id)
		{
			if (!this.goperator.SelectGarage(id)) { 
				ui.ErrMsg($"Sorry - Could not select Garage {id} - was not found");
				return false;
			}
			return true;
		}

		public bool RemoveGarage(int id)
		{
			if (this.goperator.RemoveGarage(id)) {
				ui.ErrMsg($"Sorry - Could not Delete Garage {id} - was not found");
				return false;
			}
			return true;
		}

		public bool CheckIn(IVehicle newVehicle)
		{
			try
			{
				return this.goperator.CheckIn(newVehicle);
			}
			catch (ApplicationException ex) {
				ui.ErrMsg(ex.Message);
				return false;
			}
		}
		public bool CheckOut(string regno)
		{
			if ((regno == null) || (regno.Length == 0)) {
				ui.ErrMsg("Can not checkout an empty registration number");
				return false;
			}

			try
			{
				return this.goperator.CheckOut(regno);
			}
			catch (ApplicationException ex)
			{
				ui.ErrMsg(ex.Message);
				return false;
			}
		}


		public IEnumerable<IVehicle> LinqQuery(GQuery query)
		{
			return this.goperator.LinqQuery(query);
		}

	}
}
