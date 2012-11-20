using Raven.Client;
using Raven.Client.Embedded;
using System.Linq;
using GestUAB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace GestUAB
{
	public static partial class PopulateDatabaseExtensions
	{
		public static void PopulateUsers (this IDocumentStore ds)
		{
			using (var session = ds.OpenSession()) {

// Operations against session
//                var firstNames = new string[] {"Kristina", "Paige", "Sherri", "Gretchen", "Karen", "Patrick", "Elsie", "Hazel",
//                                       "Malcolm", "Dolores", "Francis", "Sandy", "Marion", "Beth", "Julia", "Jerome",
//                                       "Neal", "Jean", "Kristine", "Crystal", "Alex", "Eric", "Wesley", "Franklin",
//                                       "Claire", "Marian", "Marcia", "Dwight", "Wayne", "Stephanie", "Neal",
//                                       "Gretchen", "Tim", "Jerome", "Shelley", "Priscilla", "Elsie", "Beth", "Erica",
//                                       "Douglas", "Donald", "Katherine", "Paul", "Patricia", "Lois", "Louis", "Christina",
//                                       "Darlene", "Harvey", "William", "Frederick", "Shirley", "Jason", "Judith", "Gretchen",
//                                       "Don", "Glenda", "Scott", "Pat", "Michelle", "Jessica", "Evan", "Melinda", "Calvin",
//                                       "Eugene", "Vickie", "Luis", "Allan", "Melanie", "Marianne", "Natalie", "Caroline",
//                                       "Arlene", "Kyle", "Calvin", "Gary", "Samantha", "Sara", "Stacy", "Gladys", "Mike",
//                                       "Lynne", "Faye", "Diana", "Leon", "Ethel", "Steve", "Alison", "Sherri", "Patsy",
//                                       "Kelly", "Stacy", "Curtis", "Dana", "Jennifer", "Brett", "Brandon", "Keith", "Joann"};
//
//                var lastNames = new string[]{"Chung", "Chen", "Melton", "Hill", "Puckett", "Song", "Hamilton", "Bender", "Wagner",
//                                           "McLaughlin", "McNamara", "Raynor", "Moon", "Woodard", "Desai", "Wallace",
//                                           "Lawrence", "Griffin", "Dougherty", "Powers", "May", "Steele", "Teague", "Vick",
//                                           "Gallagher", "Solomon", "Walsh", "Monroe", "Connolly", "Hawkins", "Middleton",
//                                           "Goldstein", "Watts", "Johnston", "Weeks", "Wilkerson", "Barton", "Walton", "Hall",
//                                           "Ross", "Chung", "Bender", "Woods", "Mangum", "Joseph", "Rosenthal", "Bowden",
//                                           "Barton", "Underwood", "Jones", "Baker", "Merritt", "Cross", "Cooper", "Holmes",
//                                           "Sharpe", "Morgan", "Hoyle", "Allen", "Rich", "Rich", "Grant", "Proctor", "Diaz",
//                                           "Graham", "Watkins", "Hinton", "Marsh", "Hewitt", "Branch", "Walton", "O'Brien",
//                                           "Case", "Watts", "Christensen", "Parks", "Hardin", "Lucas", "Eason", "Davidson",
//                                           "Whitehead", "Rose", "Sparks", "Moore", "Pearson", "Rodgers", "Graves",
//                                           "Scarborough", "Sutton", "Sinclair", "Bowman", "Olsen", "Love", "McLean",
//                                           "Christian", "Lamb", "James", "Chandler", "Stout"};

				var firstNames = new string[] {
					"Kristina",
					"Paige",
					"Sherri"/*,
					"Gretchen",
					"Karen",
					"Patrick",
					"Elsie",
					"Hazel",
					"Malcolm",
					"Dolores"*/
				};

				var lastNames = new string[]{"Chung", "Chen", "Melton"/*, "Hill", "Puckett", "Song", "Hamilton", "Bender", "Wagner",
                                             "McLaughlin"*/};

				var combs =
                (from first in firstNames
                from last in lastNames
                select new System.Tuple<string, string> (first, last)).ToList ();

				combs.ForEach (x => {
					session.Store (
                        new User{Username = x.Item1.ToLower() + "-" + x.Item2.ToLower(), 
                                 FirstName = x.Item1, 
                                 LastName = x.Item2, 
                                 Email = x.Item1.ToLower() + "-" +  x.Item2.ToLower() + "@gestuab.com" 
                                }
					);
				}
				);
                session.SaveChanges ();

//                Random rand = new Random();

//                for (int i = combs.Count - 1; i > 0; i--)
//                {
//                    int n = rand.Next(i + 1);
//                    var mem = combs[i];
//                    combs[i] = combs[n];
//                    combs[n] = mem;
//                    session.Store (
////                        new User{Username = "username", 
////                                 FirstName = "firstname", 
////                                 LastName = "lastname", 
////                                 Email = "email" 
////                                });
//                        new User{Username = mem.Item1[0].ToString() + mem.Item2, 
//                                 FirstName = mem.Item1, 
//                                 LastName = mem.Item2, 
//                                 Email = mem.Item1.ToLower() + "." +  mem.Item1.ToLower() + "@gestuab.com" 
//                                });
//                }


				ds.DatabaseCommands.PutIndex ("UsersByUsername", new IndexDefinitionBuilder<User>
	            {
	                Map = users => from user in users
	                               select new { user.Username },
	                Indexes =
	                    {
	                        { x => x.Username, FieldIndexing.Analyzed}
	                    }
	            });
			}
		}
	}
}

