using System;
using SimpleJSON;


public class RoomEvent
{
	static void Main(string[] args)
	{
		// Display the number of command line arguments:
		System.Console.WriteLine(args.Length);
	}

	private static string timeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

	private string location;
	private string summary;
	private string description;
	private DateTime fromDate;
	private DateTime toDate;

	public RoomEvent (string aLocation, string aSummary, string aDescription, DateTime aFromDate, DateTime aToDate)
	{
		this.location = aLocation;
		this.summary = aSummary;
		this.description = aDescription;
		this.fromDate = aFromDate;
		this.toDate = aToDate;
	}

	public string Location {
		get {
			return this.location;
		}
	}

	public string Summary {
		get {
			return this.summary;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public DateTime FromDate {
		get {
			return this.fromDate;
		}
	}

	public DateTime ToDate {
		get {
			return this.toDate;
		}
	}

	public static RoomEvent fromJson(JSONNode parsedJson)
	{
		string location = parsedJson ["location"];
		string summary = parsedJson ["summary"];
		string description = parsedJson ["description"];
		DateTime fromDate = DateTime.ParseExact (parsedJson ["start"], timeFormat, System.Globalization.CultureInfo.InvariantCulture);
		DateTime toDate = DateTime.ParseExact (parsedJson ["end"], timeFormat, System.Globalization.CultureInfo.InvariantCulture);
	
		return new RoomEvent (location, summary, description, fromDate, toDate);
	}

	public override string ToString ()
	{
		return string.Format ("[RoomEvent: location={0}, summary={1}, description={2}, fromDate={3}, toDate={4}]", location, summary, description, fromDate, toDate);
	}
	
}

