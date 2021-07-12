using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp.Interfaces;
namespace GarageApp
{
	class GarageStats : IGarageStats
	{
		public string Name { get; }
		public int Capacity { get; }
		public int Free { get; }
		public int Id { get; }

		public GarageStats(int id, string name, int capacity, int free)
		{
			this.Id = id;
			this.Name = name;
			this.Capacity = capacity;
			this.Free = free;
		}
	}
}
