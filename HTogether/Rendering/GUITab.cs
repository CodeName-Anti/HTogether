namespace HTogether.Rendering;

public class GUITab
{
	public string Name { get; set; }
	public int Id { get; set; }
	public bool Enabled { get; set; } = true;

	public GUITab(string name, int id)
	{
		Name = name;
		Id = id;
	}
}
