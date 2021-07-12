using System.Collections.Generic;

using GarageApp.Interfaces;
namespace GarageApp
{
	public interface IGarageManager
	{
		void Start(IUI userinterface);

		IEnumerable<IGarageStats> GetGarages();

		IEnumerable<IVehicle> LinqQuery(GQuery query);

		IGarageStats Garage { get; }

		bool AddNewGarage(string name, int capacity);

		bool RemoveGarage(int id);

		bool SelectGarage(int id);

		bool CheckIn(IVehicle newVehicle);

		bool CheckOut(string regno);

		void QuickSetup();
	}
}