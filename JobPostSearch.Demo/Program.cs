
using CsvHelper;
using JobPostSearch.Engine.Engines;
using System.Globalization;

var engine = new LinkedinJobSearchEngine();

//approx 1 min
var results = await engine.ExhaustiveSearchAsync("business analyst");
var jobs = results.SelectMany(x => x.Jobs).ToList();

using var writer = new StreamWriter("linkedin_business-analyst_results.csv");
using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
csv.WriteRecords(jobs);
