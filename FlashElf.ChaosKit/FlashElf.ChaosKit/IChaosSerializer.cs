namespace FlashElf.ChaosKit
{
	public interface IChaosSerializer
	{
		byte[] Serialize<T>(T obj);
		object Deserialize(System.Type type, byte[] data);
	}
}