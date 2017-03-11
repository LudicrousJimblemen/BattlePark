using System;

public abstract class Item {
	public int Id { get; private set; }
	
	protected string languageId;
	
	public string ProperString { get { return String.Format("vendorItems.{0}.proper", languageId); } }
	public string SingularString { get { return String.Format("vendorItems.{0}.singular", languageId); } }
	public string PluralString { get { return String.Format("vendorItems.{0}.plural", languageId); } }
	public string IndefiniteString { get { return String.Format("vendorItems.{0}.indefinite", languageId); } }
	
	public Money DefaultPrice { get; private set; }
	
	public float Explosivosity { get; set; }
	public Money Price { get; set; }
}