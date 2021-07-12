using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Misc.Interfaces;
namespace Misc
{
	public class ConMenu : IMenu
	{
		private List<MenuItem> menuitems;
		private ConIO ui = ConIO.PInstance;
		private int x;
		private int y;
		private int width;
		public ConMenu()
		{
			this.menuitems = new List<MenuItem>();
			this.MenuName = "Menu";
			this.x = 0;
			this.y = 0;
			this.width = 40;
		}
		public ConMenu(string menuname) : this()
		{
			if (menuname != null) this.MenuName = menuname;
		}
		public ConMenu(int x, int y) : this()
		{
			this.x = x;
			this.y = y;
		}
		public ConMenu(int x, int y, string menuname) : this()
		{
			if (menuname != null) this.MenuName = menuname;
			this.x = x;
			this.y = y;
		}

		public string MenuName { get; set; }
		public void Clear() { this.menuitems.Clear(); }

		public void AddMenuItem(int menunselection, string menutext)
		{
			MenuItem newiem = new MenuItem();
			newiem.MenuSelection = menunselection;
			newiem.MenuText = menutext;
			this.menuitems.Add(newiem);
		}

		public int GetMenuSelection(string prompt = null)
		{
			int ret = 0;
			this.ShowMenu();
			while (true)
			{
				if ((this.x == 0) && (this.y == 0))
				{
					ui.Show("\n".PadLeft(this.width));
					if ((prompt != null) && (prompt.Length > 0)) ui.Show(prompt);
					else ui.Show("Select option: ");
					ret = ui.GetNumber();

					foreach (MenuItem menuitem in this.menuitems)
					{
						if ((menuitem.MenuSelection == ret) && (menuitem.MenuText.Length > 0)) return ret;
					}
					ui.Show("Invalid selection - Press Any Key \n".PadLeft(this.width));
					ui.GetKey();
				}
				else {
					ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "".PadRight(this.width));
					if ((prompt != null) && (prompt.Length > 0)) ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, prompt);
					else ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "Select option: ");
					ret = ui.GetNumber();

					foreach (MenuItem menuitem in this.menuitems)
					{
						if ((menuitem.MenuSelection == ret) && (menuitem.MenuText.Length > 0)) return ret;
					}
					ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "Invalid selection - Press Any Key ".PadRight(this.width));
					ui.GetKey();
				}
			}
		}

		// return an array of selected options OR null if non selected
		public int[] GetMenuMultiSelection(string prompt = null, int itemwidth = 10)
		{
			int[] ret = null;
			int i1;
			bool found;
			if ((itemwidth > 0) && (itemwidth < 140)) this.width = itemwidth;
			while (true)
			{
				this.ShowHorizontalMenu();
				if ((this.x == 0) && (this.y == 0))
				{
					if ((prompt != null) && (prompt.Length > 0)) ui.Show(prompt);
					else ui.Show("\nSelect option(s): ");
					ret = ui.GetMultiNumber();
					found = true;
					if (ret != null) {
						// check each option to see if a valid options
						i1 = 0;
						while (i1 < ret.Length) {
							found = false;
							foreach (MenuItem menuitem in this.menuitems) {
								if ((menuitem.MenuSelection == ret[i1]) && (menuitem.MenuText.Length > 0)) { found = true; break; }
							}
							if (!found) {
								ui.Show(" - Invalid selection - Press Any Key - ");
								ui.GetKey();
								break;
							}
							i1++;
						}
					}
					if (found) return ret;
				}
				else
				{
					ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "".PadRight(this.width, ' '));
					if ((prompt != null) && (prompt.Length > 0)) ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, prompt);
					else ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "Select option: ");
					ret = ui.GetMultiNumber();
					found = true;
					if (ret != null)
					{
						// check each option to see if a valid options
						i1 = 0;
						while (i1 < ret.Length)
						{
							found = false;
							foreach (MenuItem menuitem in this.menuitems)
							{
								if ((menuitem.MenuSelection == ret[i1]) && (menuitem.MenuText.Length > 0)) { found = true; break; }
							}
							if (!found)
							{
								ui.ShowXY(this.x, this.y + this.menuitems.Count + 2, "Invalid selection - Press Any Key ".PadRight(this.width, ' '));
								ui.GetKey();
								break;
							}
							i1++;
						}
					}
					if (found) return ret;
				}
			}
		}

		private void ShowMenu()
		{
			int x1 = this.x; int y1 = this.y;
			if ((this.x == 0) && (this.y == 0)) { 
				ui.Show($"{this.MenuName}\n");
			}
			else { 
				ui.ShowXY(x1, y1++, $"{this.MenuName.PadRight(this.width, ' ')}");
			}
			foreach (MenuItem menuitem in this.menuitems)
			{
				if ((this.y == 0) && (this.y == 0)) {
					if (menuitem.MenuText.Length > 0)
						ui.Show($"  {menuitem.MenuSelection,2}.  {menuitem.MenuText}\n");
				}
				else {
					if (menuitem.MenuText.Length > 0)
						ui.ShowXY(x1, y1, $"  {menuitem.MenuSelection,2}.  {menuitem.MenuText}".PadRight(this.width, ' '));
					y1++;
				}
			}
		}

		private void ShowHorizontalMenu()
		{
			int x1 = this.x; int y1 = this.y;
			ui.Show($"\r".PadRight(Console.WindowWidth, ' ')); ui.Show("\r");
			if ((this.x == 0) && (this.y == 0))
			{
				ui.Show($"\r{this.MenuName}  ".PadLeft(Console.WindowWidth, ' '));
			}
			else
			{
				ui.ShowXY(x1, y1, $"{this.MenuName.PadRight(this.width, ' ')}");
			}
			foreach (MenuItem menuitem in this.menuitems)
			{
				if (menuitem.MenuText.Length > 0)
					ui.Show($"  {menuitem.MenuText} ({menuitem.MenuSelection})");
			}
		}

	}




}
