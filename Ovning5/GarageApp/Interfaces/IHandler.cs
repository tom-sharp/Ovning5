

using System.Collections.Generic;

namespace GarageApp.Interfaces
{

	// garahe handler
	public interface IHandler
	{

		bool AddNewGarage(string name, int capacity);
		bool RemoveGarage(int id);
		bool SelectGarage(int id);
		bool CheckIn(IVehicle vehicle);
		bool CheckOut(string regno);

		IGarageStats Garage { get; }

		int GaragesCount { get; }

		IEnumerable<IGarageStats> GetGarages();

		IEnumerable<IVehicle> LinqQuery(GQuery query);
	}
}
