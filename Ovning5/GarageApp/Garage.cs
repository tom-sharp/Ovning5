using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
using Misc;

namespace GarageApp
{
	public class Garage<T> : IEnumerable<T> where T : class, IVehicle
	{
		public string Name { get; set; }
		public int Id { get; }
		private T[] vehicles = null;

		public Garage(int id, string name, int capacity) 
		{
			this.Id = id;
			if (capacity > 0) this.vehicles = new T[capacity];
			if (name != null) this.Name = name;
			else this.Name = "";
		}

		public int Capacity() { if (this.vehicles != null) return vehicles.Length; return 0; }

		public int Free() {
			if (this.vehicles == null) return 0;
			int i1 = 0;
			foreach (var v in vehicles) {
				if (v == null) i1++;
			}
			return i1;
		}


		public bool Checkin(T vehicle) {
			if (this.vehicles == null) throw new ApplicationException($"Has no Capacity");
			if (vehicle == null) throw new ArgumentNullException();
			if (vehicle.Regnumber == null) throw new ApplicationException($"Registration number is a null reference");
			if (vehicle.Regnumber.Length == 0) return false;
			if (this.IsExist(vehicle)) return false;
			int i1 = 0;
			while (i1 < vehicles.Length) {
				if (vehicles[i1] == null) { vehicles[i1] = vehicle; return true; }
				i1++;
			}
			return false;
		}

		public bool Checkout(string regno) {
			if (this.vehicles == null) throw new ApplicationException($"This garage is empty and has no vehicles");
			if (regno == null)  throw new ArgumentNullException("Can not check out an invalid registration number");
			if (regno.Length == 0) return false;

			int i1 = 0;
			string rno = regno.ToLower();

			while (i1 < vehicles.Length)
			{
				if (vehicles[i1] != null) { if (vehicles[i1].Regnumber.ToLower() == rno) {vehicles[i1] = null; return true; } }
				i1++;
			}
			return false;
		}

		// Test LINQ
		// using linq - functions or expressions MUST always return a bool

		// UI -> req format?


		public IEnumerable<T> LinqQuery(IQGarage query) {
			if (query == null) throw new ArgumentNullException();
			var result = this.vehicles.Where(v => v != null)
				.Where(v => query.IsSelected(v));
			return result;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (this.vehicles != null)
			{
				foreach (var v in this.vehicles)
				{
					if (v != null) yield return v;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		bool IsExist(T obj)
		{
			if (this.vehicles == null) return false;
			string rno = obj.Regnumber.ToLower();
			foreach (var v in vehicles)
			{
				if (v != null)
				{
					if (v == obj) { BugCheck.Log(this, "IsExist :: try to add same reference twice"); return true; }
					if (v.Regnumber.ToLower() == rno) return true;
				}
			}
			return false;
		}




	}
}
