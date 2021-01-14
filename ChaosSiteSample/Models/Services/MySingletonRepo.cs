namespace ChaosSiteSample.Models.Services
{
	public class MySingletonRepo : ISingletonRepo
	{
		public string GetSingletonName()
		{
			return "Singleton Flash";
		}
	}
}