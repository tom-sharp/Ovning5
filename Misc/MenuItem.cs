namespace Misc
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




}
