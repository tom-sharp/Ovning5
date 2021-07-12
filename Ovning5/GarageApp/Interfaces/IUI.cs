

namespace GarageApp.Interfaces
{
	public interface IUI
	{
		void ErrMsg(string msg);
		void Msg(string msg);
		void Run(IGarageManager manager);
	}
}
