using Microsoft.VisualStudio.TestTools.UnitTesting;



using GarageApp;
using GarageApp.Interfaces;
using GarageApp.Vehicles;
using System;

namespace GarageTest
{
	[TestClass]
	public class GarageUnitTest
	{

		[TestInitialize]
		public void Init()
		{
		}


		[TestCleanup]
		public void CleanUp()
		{
		}



		// Garage test
		// 1. test the construcort - ok
		// 2. capacity -ok 
		// 3. test free - ok
		// 4. checkin - ok
		// 5. checkout - ok
		// 6. enumrator - ok
		// 7. linqquery


		Garage<IVehicle> TestGarage;
		void InitGarage(int capacity) {
			TestGarage = new Garage<IVehicle>(id: 0, "", capacity);
		}

		void CheckInVehicles(int count)
		{
			Car newCar = null;
			int i1 = 0;
			string regno;
			while (i1++ < count) {
				regno = i1.ToString();
				newCar = new Car(regno, electric:false);
				this.TestGarage.Checkin(newCar);
			}
		}


		[TestMethod]
		public void Constructor_InitWithNegative_ZeroCapacity()
		{

			// Arrage
			const int expected = 0;

			// Act
			InitGarage(-1);

			// Assert
			Assert.AreEqual(expected, TestGarage.Capacity());
			Assert.AreEqual(expected, TestGarage.Free());

		}

		[TestMethod]
		public void Constructor_InitWithZeroCapacity_ZeroCapacity()
		{
			// Arrage
			const int expected = 0;

			// Act
			InitGarage(expected);

			// Assert
			Assert.AreEqual(expected, TestGarage.Capacity());
			Assert.AreEqual(expected, TestGarage.Free());
		}

		[TestMethod]
		public void Constructor_InitWithPositiveCapacity_PositiveCapacity()
		{
			// Arrage
			const int expected = 200;

			// Act
			InitGarage(expected);

			// Assert
			Assert.AreEqual(expected, TestGarage.Capacity());
			Assert.AreEqual(expected, TestGarage.Free());
		}

		[TestMethod]
		public void CheckIn_VehicleWhenGarageInitInvalid_ExceptionThrown()
		{
			// Arrage
			const int expected_capacity = 0;
			const int expected_free = 0;
			const string expected_msg = "fail";

			string catched_msg;
			InitGarage(-1);

			// Act
			try
			{
				CheckInVehicles(1);
				catched_msg = "success";
			}
			catch (ApplicationException ex) { 
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void CheckIn_VehicleWhithNullReferens_ExceptionThrown()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const string expected_msg = "fail";

			InitGarage(expected_capacity);
			Car newCar = null;
			string catched_msg;

			// Act
			try
			{
				this.TestGarage.Checkin(newCar);
				catched_msg = "success";
			}
			catch (ArgumentNullException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void CheckIn_VehicleWithValidGarage_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 199;
			const bool expected_ret = true;
			bool actual_ret;

			// Act
			InitGarage(expected_capacity);
			actual_ret = TestGarage.Checkin(new Car(regnumber: "non-existing", electric: false));

			// Assert
			Assert.AreEqual(expected_ret, actual_ret);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void CheckIn_VehicleWhenGarageFull_Fail()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 0;
			const string expected_msg = "success";
			const bool expected_ret = false;

			string catched_msg;
			bool actual_ret = true;
			InitGarage(expected_capacity);
			CheckInVehicles(200);
			// Act
			try
			{
				actual_ret = TestGarage.Checkin(new Car(regnumber: "non-existing", electric: false));
				catched_msg = "success";
			}
			catch (ApplicationException ex){
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_ret, actual_ret);
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void CheckIn_VehicleWhithNullRefRegno_ExcteptionThrown()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const string expected_msg = "fail";

			InitGarage(expected_capacity);
			Car newCar = new Car(regnumber: null, electric: false); 
			string catched_msg;

			// Act
			try
			{
				this.TestGarage.Checkin(newCar);
				catched_msg = "success";
			}
			catch (ApplicationException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void CheckIn_VehicleWhithDublicateRegno_Fail()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 199;
			const string expected_msg = "success";
			const bool expected_ret = false;

			InitGarage(expected_capacity);
			Car newCar = new Car(regnumber: "abc123", electric: false);
			this.TestGarage.Checkin(newCar);
			string catched_msg;
			bool actual_ret = true;
			// Act
			try
			{
				actual_ret = this.TestGarage.Checkin(newCar);
				catched_msg = "success";
			}
			catch (ApplicationException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_ret, actual_ret);
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void Free_ReportCorrectFreeAvilability_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 150;

			InitGarage(expected_capacity);

			// Act
			CheckInVehicles(50);

			// Assert
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void Free_Report_Correct_free_avilability_when_garage_full()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 0;

			InitGarage(expected_capacity);

			// Act
			CheckInVehicles(expected_capacity);

			// Assert
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void CheckOut_VehicleWhithNullRefArgument_ExceptionThrown()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const string expected_msg = "fail";

			InitGarage(expected_capacity);
			string catched_msg;

			// Act
			try
			{
				this.TestGarage.Checkout(regno: null);
				catched_msg = "success";
			}
			catch (ArgumentNullException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void CheckOut_VehicleThatDoNotExist_Fail()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 199;
			const string expected_msg = "success";
			const bool expected_ret = false;

			InitGarage(expected_capacity);
			Car newCar = new Car(regnumber: "abc123", electric: false);
			this.TestGarage.Checkin(newCar);
			string catched_msg;
			bool actual_ret = true;

			// Act
			try
			{
				actual_ret = this.TestGarage.Checkout(regno: "abcxxx");
				catched_msg = "success";
			}
			catch (ApplicationException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_ret, actual_ret);
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void CheckOut_VehicleThatDoExist_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const string expected_msg = "success";
			const bool expected_ret = true;
			bool actual_ret = false;

			InitGarage(expected_capacity);
			Car newCar = new Car(regnumber: "abc123", electric: false);
			TestGarage.Checkin(newCar);
			string catched_msg;

			// Act
			try
			{
				actual_ret = this.TestGarage.Checkout(regno: "abc123");
				catched_msg = "success";
			}
			catch (ApplicationException ex)
			{
				catched_msg = "fail";
			}

			// Assert
			Assert.AreEqual(expected_ret, actual_ret);
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void EnumGarage_whenempty_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const int expected_count = 0;

			InitGarage(expected_capacity);
			int actual_count = 0;


			// Act
			foreach (var v in TestGarage)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void EnumGarage_whenfull_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 0;
			const int expected_count = 200;

			InitGarage(expected_capacity);
			CheckInVehicles(200);
			int actual_count = 0;


			// Act
			foreach (var v in TestGarage)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void LinqQuery_NullRefArgument_exceptionthrown()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 200;
			const string expected_msg = "fail";

			string catched_msg;

			InitGarage(expected_capacity);


			// Act
			try
			{
				var result = TestGarage.LinqQuery(null);
				catched_msg = "success";
			}
			catch (ArgumentNullException ex)
			{
				catched_msg = "fail";
			}


			// Assert
			Assert.AreEqual(expected_msg, catched_msg);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}




		[TestMethod]
		public void LinqQuery_FindAllRegnumbers_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 10;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc121", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc122", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc123", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc124", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc131", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc132", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc133", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc134", electric: false));

			GQuery query = new GQuery(registrationnumber: "");
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void LinqQuery_FindASpecificRegnumber_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 1;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc121", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc122", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc123", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc124", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc131", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc132", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc133", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc134", electric: false));

			GQuery query = new GQuery(registrationnumber: "abc130");
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void LinqQuery_FindASelectionofRegnumbers_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 5;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc121", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc122", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc123", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc124", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc131", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc132", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc133", electric: false));
			TestGarage.Checkin(new Car(regnumber: "abc134", electric: false));

			GQuery query = new GQuery(registrationnumber: "12");
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void LinqQuery_FindASelectionOfVehicletype_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 2;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false));
			TestGarage.Checkin(new Motorcycle(regnumber: "abc121", offroad: false));
			TestGarage.Checkin(new Bus(regnumber: "abc122", Passengers: 10));
			TestGarage.Checkin(new Boat(regnumber: "abc123", length: 10));
			TestGarage.Checkin(new Airplane(regnumber: "abc124", wingspan: 10));
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false));
			TestGarage.Checkin(new Motorcycle(regnumber: "abc131", offroad: false));
			TestGarage.Checkin(new Bus(regnumber: "abc132", Passengers: 10));
			TestGarage.Checkin(new Boat(regnumber: "abc133", length: 10));
			TestGarage.Checkin(new Airplane(regnumber: "abc134", wingspan: 10));

			GQuery query = new GQuery();
			query.FilterVehicleTypeActive = true;
			query.SelectVehicleType(Factory.VType.Car);
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void LinqQuery_FindASelectionOfMultipleVehicletype_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 4;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false));
			TestGarage.Checkin(new Motorcycle(regnumber: "abc121", offroad: false));
			TestGarage.Checkin(new Bus(regnumber: "abc122", Passengers: 10));
			TestGarage.Checkin(new Boat(regnumber: "abc123", length: 10));
			TestGarage.Checkin(new Airplane(regnumber: "abc124", wingspan: 10));
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false));
			TestGarage.Checkin(new Motorcycle(regnumber: "abc131", offroad: false));
			TestGarage.Checkin(new Bus(regnumber: "abc132", Passengers: 10));
			TestGarage.Checkin(new Boat(regnumber: "abc133", length: 10));
			TestGarage.Checkin(new Airplane(regnumber: "abc134", wingspan: 10));

			GQuery query = new GQuery();
			query.FilterVehicleTypeActive = true;
			query.SelectVehicleType(Factory.VType.Car);
			query.SelectVehicleType(Factory.VType.Bus);
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void LinqQuery_FindASelectionOfVehicleColor_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 3;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false) { Color = Factory.VColor.Red});
			TestGarage.Checkin(new Motorcycle(regnumber: "abc121", offroad: false) { Color = Factory.VColor.Green});
			TestGarage.Checkin(new Bus(regnumber: "abc122", Passengers: 10) { Color = Factory.VColor.Blue});
			TestGarage.Checkin(new Boat(regnumber: "abc123", length: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Airplane(regnumber: "abc124", wingspan: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false) { Color = Factory.VColor.Green });
			TestGarage.Checkin(new Motorcycle(regnumber: "abc131", offroad: false) { Color = Factory.VColor.Blue });
			TestGarage.Checkin(new Bus(regnumber: "abc132", Passengers: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Boat(regnumber: "abc133", length: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Airplane(regnumber: "abc134", wingspan: 10) { Color = Factory.VColor.Green });

			GQuery query = new GQuery();
			query.FilterColorActive = true;
			query.SelectVehicleColor(Factory.VColor.Red);
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


		[TestMethod]
		public void LinqQuery_FindASelectionOfMultipleVehicleColors_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 5;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Motorcycle(regnumber: "abc121", offroad: false) { Color = Factory.VColor.Green });
			TestGarage.Checkin(new Bus(regnumber: "abc122", Passengers: 10) { Color = Factory.VColor.Blue });
			TestGarage.Checkin(new Boat(regnumber: "abc123", length: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Airplane(regnumber: "abc124", wingspan: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false) { Color = Factory.VColor.Green });
			TestGarage.Checkin(new Motorcycle(regnumber: "abc131", offroad: false) { Color = Factory.VColor.Blue });
			TestGarage.Checkin(new Bus(regnumber: "abc132", Passengers: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Boat(regnumber: "abc133", length: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Airplane(regnumber: "abc134", wingspan: 10) { Color = Factory.VColor.Green });

			GQuery query = new GQuery();
			query.FilterColorActive = true;
			query.SelectVehicleColor(Factory.VColor.Red);
			query.SelectVehicleColor(Factory.VColor.Blue);
			int actual_count = 0;


			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}

		[TestMethod]
		public void LinqQuery_FindASelectionOfvehiclesOnRegnumerTypeAndColor_Success()
		{
			// Arrage
			const int expected_capacity = 200;
			const int expected_free = 190;
			const int expected_count = 2;

			InitGarage(expected_capacity);

			TestGarage.Checkin(new Car(regnumber: "abc120", electric: false) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Motorcycle(regnumber: "abc121", offroad: false) { Color = Factory.VColor.Green });
			TestGarage.Checkin(new Bus(regnumber: "abc122", Passengers: 10) { Color = Factory.VColor.Blue });
			TestGarage.Checkin(new Boat(regnumber: "abc123", length: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Airplane(regnumber: "abc124", wingspan: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Car(regnumber: "abc130", electric: false) { Color = Factory.VColor.Green });
			TestGarage.Checkin(new Motorcycle(regnumber: "abc131", offroad: false) { Color = Factory.VColor.Blue });
			TestGarage.Checkin(new Bus(regnumber: "abc132", Passengers: 10) { Color = Factory.VColor.White });
			TestGarage.Checkin(new Boat(regnumber: "abc133", length: 10) { Color = Factory.VColor.Red });
			TestGarage.Checkin(new Airplane(regnumber: "abc134", wingspan: 10) { Color = Factory.VColor.Green });

			GQuery query = new GQuery();

			query.FilterRegistrationNumberActive = true;
			query.SelectRegistrationNumber = "12";

			query.FilterVehicleTypeActive = true;
			query.SelectVehicleType(Factory.VType.Car);
			query.SelectVehicleType(Factory.VType.Bus);

			query.FilterColorActive = true;
			query.SelectVehicleColor(Factory.VColor.Red);
			query.SelectVehicleColor(Factory.VColor.Blue);
			query.SelectVehicleColor(Factory.VColor.Green);

			int actual_count = 0;



			// Act
			var result = TestGarage.LinqQuery(query);
			foreach (var v in result)
			{
				actual_count++;
			}


			// Assert
			Assert.AreEqual(expected_count, actual_count);
			Assert.AreEqual(expected_capacity, TestGarage.Capacity());
			Assert.AreEqual(expected_free, TestGarage.Free());
		}


	}
}
