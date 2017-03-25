using System;

public class Item {
	public int Id { get; protected set; }
	
	protected string languageId;
	
	public string ProperString { get { return String.Format("vendorItems.{0}.proper", languageId); } }
	public string SingularString { get { return String.Format("vendorItems.{0}.singular", languageId); } }
	public string PluralString { get { return String.Format("vendorItems.{0}.plural", languageId); } }
	public string IndefiniteString { get { return String.Format("vendorItems.{0}.indefinite", languageId); } }
	
	public Money DefaultPrice { get; protected set; }
	
	public float Explosivosity { get; set; }
	public Money Price { get; set; }
	
	public Item() { }
	
	public Item(int id, string languageId, Money defaultPrice) {
		this.Id = id;
		this.languageId = languageId;
		this.DefaultPrice = defaultPrice;
	}
	
	public static readonly Item ParkMap = new Item(1, "parkMap", new Money(1, 00));
}