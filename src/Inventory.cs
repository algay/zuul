class Inventory
{
	private int maxWeight;
	private Dictionary<string, Item> items;
	public Inventory(int maxWeight){
		if(maxWeight<1){
			maxWeight=1;
		}
		this.maxWeight = maxWeight;
		this.items = new Dictionary<string, Item>();
	}
	public int GetWeight(){
		int weight = 0;
		foreach(KeyValuePair<string, Item> i in this.items){
			weight += items[i.Key].Weight;
		}
		return weight;
	}
	public int GetMaxWeight(){
		return this.maxWeight;
	}
	public void Add(string key, Item value){
		this.items.Add(key, value);
	}
	public bool Contains(string item){
		return this.items.ContainsKey(item);
	}
	public void Remove(string item){
		this.items.Remove(item);
	}
	public Dictionary<string, Item> GetItems(){
		return this.items;
	}
	public Item GetItem(string key){
		return this.items[key];
	}
}
