using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GarageApp;
using Misc;
using GarageApp.Vehicles;
using GarageApp.Interfaces;
namespace GarageApp
{
	public class QuickSetup
	{

		public void Setup(IHandler ghandler) {
			Random random = new Random();
			int i1 = 100;
			string regno = "abc";
			if (ghandler == null) return;
			ghandler.AddNewGarage("Narvavägen 10", 1000);
			ghandler.AddNewGarage("Norrmalmstorg 120", 300);
			ghandler.AddNewGarage("Mälardalen", 600);
			ghandler.AddNewGarage("Kvarteret Turkosen", 80);
			ghandler.AddNewGarage("Norra Lidingö", 4);
			ghandler.SelectGarage(1);
			while (i1++ < 999) {
				switch (random.Next(1, 6)) {
					case 1: ghandler.CheckIn(Factory.GetNewVehicle(Factory.VType.Car, regno + i1.ToString())); break;
					case 2: ghandler.CheckIn(Factory.GetNewVehicle(Factory.VType.Bus, regno + i1.ToString())); break;
					case 3: ghandler.CheckIn(Factory.GetNewVehicle(Factory.VType.Motorcycle, regno + i1.ToString())); break;
					case 4: ghandler.CheckIn(Factory.GetNewVehicle(Factory.VType.Boat, regno + i1.ToString())); break;
					case 5: ghandler.CheckIn(Factory.GetNewVehicle(Factory.VType.Airplane, regno + i1.ToString())); break;
				}

			}
			this.SetRandomProperties(ghandler);
		}

		private void SetRandomProperties(IHandler ghandler)
		{
			Random random = new Random();
			GQuery query = new GQuery();
			var result = ghandler.LinqQuery(query);
			foreach (var v in result) {
				//set random color
				switch (random.Next(1, Factory.VColorsName.Length)) {
					case 1: v.Color = Factory.VColor.Red;	break;
					case 2: v.Color = Factory.VColor.Green;	break;
					case 3: v.Color = Factory.VColor.Blue;	break;
					case 4: v.Color = Factory.VColor.White;	break;
					case 5: v.Color = Factory.VColor.Unknown;	break;
				}
				// set ranodom make
				switch (random.Next(1, 11))
				{
					case 1: v.MakeModel = "Volvo V70"; break;
					case 2: v.MakeModel = "Audi X3"; break;
					case 3: v.MakeModel = "BMW 300i"; break;
					case 4: v.MakeModel = "Saab 900"; break;
					case 5: v.MakeModel = "Chevrolet"; break;
					case 6: v.MakeModel = "Ford"; break;
					case 7: v.MakeModel = "Toyota"; break;
					case 8: v.MakeModel = "Nissan"; break;
					case 9: v.MakeModel = "Tesla S"; break;
					case 10: v.MakeModel = "Mazda"; break;
				}
				if (v is Car) ((Car)v).IsElectric = (random.Next(0, 2) == 0);
				if (v is Motorcycle) ((Motorcycle)v).IsOffroad = (random.Next(0, 2) == 0);
				if (v is Bus) ((Bus)v).Passengers = random.Next(0, 81);
				if (v is Boat) ((Boat)v).Length = random.Next(10, 101);	// feet
				if (v is Airplane) ((Airplane)v).Wingspan = random.Next(10, 101); // meter
			}
		}
	}
}
