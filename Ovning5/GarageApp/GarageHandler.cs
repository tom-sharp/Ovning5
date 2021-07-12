using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Misc;
using GarageApp;
using GarageApp.Interfaces;

namespace GarageApp
{

	class GarageHandler : IHandler
	{
		Garage<IVehicle> current = null;
		Dictionary<int, Garage<IVehicle>> garages = new Dictionary<int, Garage<IVehicle>>();

		public GarageHandler() { }


		///
		///          Maintain Garages  Below
		/// 
		public int GaragesCount { get { return this.garages.Count; } }

		public bool AddNewGarage(string name, int capacity)
		{
			int i1 = 1;
			while (garages.ContainsKey(i1)) i1++;
			if (this.garages.TryAdd(i1, new Garage<IVehicle>(i1, name, capacity)))
			{
				// set the newly created garage to current
				if (!this.garages.TryGetValue(i1, out this.current)) BugCheck.Log(this, "Was unable to retrieve newly created garage");
				return true;
			}
			return false;
		}

		public IEnumerable<IGarageStats> GetGarages()
		{
			foreach (var g in this.garages)
			{
				yield return new GarageStats(g.Key, g.Value.Name, g.Value.Capacity(), g.Value.Free());
			}
		}

		public IGarageStats Garage
		{
			get
			{
				if (this.current == null) return null;
				return new GarageStats(current.Id, current.Name, current.Capacity(), current.Free());
			}
		}

		public bool SelectGarage(int id)
		{
			if (this.garages.TryGetValue(id, out this.current)) return true;
			return false;
		}

		public bool RemoveGarage(int id)
		{
			return this.garages.Remove(id);
		}

		///
		///          Manage current Garage   Below  (IHandler)
		/// 

		// Check  into current Garage
		public bool CheckIn(IVehicle vehicle)
		{         
			if (vehicle == null) throw new ArgumentNullException();
			if (this.current == null) throw new ApplicationException(" No default garage to check in to is selected");
			try
			{
				return this.current.Checkin(vehicle);
			}
			catch (ApplicationException ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}

		// Check out from current Garage
		public bool CheckOut(string regno)
		{        
			if (regno == null) throw new ArgumentNullException();
			if (this.current == null) throw new ApplicationException(" No default garage to check in to is selected");
			try
			{
				return this.current.Checkout(regno);
			}
			catch (ApplicationException ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}


		public IEnumerable<IVehicle> LinqQuery(GQuery query) {
			return current.LinqQuery(query);
		}
	}

}


