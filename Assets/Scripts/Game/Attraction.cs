using System;

public class Attraction {
	public int Id { get; protected set; }
	
	protected string languageId;
	
	public string ProperString { get { return String.Format("attractions.{0}.proper", languageId); } }
	public string SingularString { get { return String.Format("attractions.{0}.singular", languageId); } }
	public string PluralString { get { return String.Format("attractions.{0}.plural", languageId); } }
	
	public Money DefaultPrice { get; protected set; }
	
	public Money Price { get; set; }
	
	public Attraction(int id, string languageId, Money defaultPrice) {
		this.Id = id;
		this.languageId = languageId;
		this.DefaultPrice = defaultPrice;
	}
	
	public static readonly Attraction FunRide = new Attraction(1, "funRide", new Money(600, 00));
}