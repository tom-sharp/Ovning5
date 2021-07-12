

/*
 
 övning5 - Garage

 Application design

Garage: (maintain vehicles)
	<- Checkin(vehicle) -> success/fail
	<- Checkout(vehicle) ->success/fail
	-> Enum content

Handler: (maintain garages)
	-> Checkin(vehicle) -> garage 
	-> Checkout(vehicle) -> garage
	-> enum content -> garage
	<- create (garage) -> success/fail
	<- delete (garage) -> success/fail
	<- update (garage) -> success/fail
	<- select (garage) -> success/fail
	<- enum content (garages) 
	<- enum content (selected) 
	<- Checkin (selected)
	<- Checkout (selected)

Manager: (dispatcher)
	-> create garage -> Handler
	-> delete garage -> Handler
	-> update garage -> Handler
	-> select garage -> Handler
	-> enum content (garages) 
	-> enum content (selected) 
	-> checkin (selected) -> Handler
	-> checkout (selected) -> Handler
	<- create garage
	<- delete garage
	<- update garage
	<- select garage
	<- checkin (selected)
	<- checkout (selected)
	<- queries
 
userinterface: (requestor)
	-> create garage -> Manager
	-> delete garage -> Manager
	-> update garage -> Manager
	-> select garage -> Manager
	-> checkin (current) -> Manager
	-> checkout (current) -> Manager
	-> queries -> Manager


 */



using System;
using GarageApp;
using GarageApp.Interfaces;

namespace Ovning5
{
	class Program
	{
		static void Main(string[] args)
		{
			IUI ui = new ConUI();
			GarageMain  GarageManager = new GarageMain();
			GarageManager.Start(ui);

		}
	}
}
