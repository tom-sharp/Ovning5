using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovning5.Misc
{
	internal class MenuItem
	{
		public MenuItem() { }
		public MenuItem(int menunumber, string menutext)
		{
			this.MenuSelection = menunumber;
			this.MenuText = menutext;
		}

		public int MenuSelection { get; set; }
		public string MenuText { get; set; }
	}

	public class ConsoleMenu
	{
		List<MenuItem> menuitems;
		ConIO ui = new ConIO();	// Convert to Sinleton

		public ConsoleMenu()
		{
			menuitems = new List<MenuItem>();
		}
		public ConsoleMenu(string menuname) : this()
		{
			if (menuname != null) this.MenuName = menuname;
			else this.MenuName = "Menu";
		}

		public string MenuName { get; set; }

		public void AddMenuItem(int menunselection, string menutext)
		{
			MenuItem newiem = new MenuItem();
			newiem.MenuSelection = menunselection;
			newiem.MenuText = menutext;
			this.menuitems.Add(newiem);
		}

		private void ShowMenu()
		{
			Console.WriteLine($"{this.MenuName}");
			foreach (MenuItem menuitem in this.menuitems)
			{
				if (menuitem.MenuText.Length > 0)
					ui.Show($"  {menuitem.MenuSelection,2}.	{menuitem.MenuText}\n");
				else
					ui.Show($"\n");   // put space in menu
			}
		}

		public int GetMenuSelection()
		{
			this.ShowMenu();
			while (true) {
				int ret = ui.GetNumber("Select option: ");
				foreach (MenuItem menuitem in this.menuitems) {
					if ((menuitem.MenuSelection == ret) && (menuitem.MenuText.Length > 0)) return ret;
				}
				ui.Show("Invalid selection\n");
			}


		}
	}




}
