namespace ChaosSiteSample.Models.Services
{
	public class MyFirstDecorateRepo : IDecorateRepo
	{
		private readonly IDecorateRepo _next;

		public MyFirstDecorateRepo(IDecorateRepo next)
		{
			_next = next;
		}

		public string GetName()
		{
			var name = _next.GetName();
			name = $"({name})";
			return name;
		}
	}
}