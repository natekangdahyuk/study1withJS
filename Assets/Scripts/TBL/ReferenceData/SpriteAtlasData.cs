
public class SpriteAtlasData : IReferenceDataByKey
{
	public object GetKey()
	{
		return AtlasName;
	}

	public string AtlasName = "";
	public bool Preload = false;
}