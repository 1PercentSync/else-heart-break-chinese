using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
[Serializable]
public class CameraFocusPoint : MonoBehaviour
{
	// Token: 0x060002F4 RID: 756 RVA: 0x00016BB8 File Offset: 0x00014DB8
	private Color FromNum(int pN)
	{
		switch (pN)
		{
		case 0:
			return Color.red;
		case 1:
			return Color.green;
		case 2:
			return Color.blue;
		default:
			return Color.cyan;
		}
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00016BF8 File Offset: 0x00014DF8
	private void Start()
	{
		this.CreateBorders();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00016C00 File Offset: 0x00014E00
	private void CreateBorders()
	{
		this._borders.Clear();
		List<CameraFocusPoint> list = new List<CameraFocusPoint>((CameraFocusPoint[])global::UnityEngine.Object.FindObjectsOfType(typeof(CameraFocusPoint)));
		if (list != null && list.Count > 1)
		{
			this.RemoveSelf(list);
			while (list.Count > 0)
			{
				CameraFocusPoint cameraFocusPoint = this.PopClosestPoint(list);
				Line2D[] array = this.CreateLinesFromPoint(cameraFocusPoint);
				CameraFocusPoint.LineAndPoint lineAndPoint = default(CameraFocusPoint.LineAndPoint);
				lineAndPoint.line = array[0];
				lineAndPoint.lineTwo = array[1];
				lineAndPoint.point = cameraFocusPoint;
				this._borders.Add(lineAndPoint);
				this.RemoveAllPointsOutsideLine(list, lineAndPoint);
			}
		}
		this._polygonPoints = this.GetPolygonPoints();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00016CB0 File Offset: 0x00014EB0
	private Line2D[] CreateLinesFromPoint(CameraFocusPoint p)
	{
		Vector3 vector = this.DeltaPos(p);
		Vector2 vector2 = new Vector2(vector.x, vector.z);
		Vector2 vector3 = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 vector4 = vector2;
		vector4.Normalize();
		Line2D line2D = new Line2D(vector3 + vector2, vector4);
		float num = 0.5f;
		if (UnityHelper.Contains(this._weightRelations, p.__camname))
		{
			num = UnityHelper.Get(this._weightRelations, p.__camname);
		}
		else if (UnityHelper.Contains(p._weightRelations, this.__camname))
		{
			num = 1f - UnityHelper.Get(p._weightRelations, this.__camname);
		}
		num += 0.01f;
		Line2D line2D2 = new Line2D(vector3 + vector2 * num, vector4);
		return new Line2D[] { line2D, line2D2 };
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00016DB8 File Offset: 0x00014FB8
	private void RemoveSelf(List<CameraFocusPoint> pPoints)
	{
		for (int i = pPoints.Count - 1; i >= 0; i--)
		{
			if (pPoints[i] == this)
			{
				pPoints.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00016DF8 File Offset: 0x00014FF8
	private void RemoveAllPointsOutsideLine(List<CameraFocusPoint> pPoints, CameraFocusPoint.LineAndPoint pLineAndPoint)
	{
		for (int i = pPoints.Count - 1; i >= 0; i--)
		{
			if (this.isOutsideLine(pLineAndPoint, pPoints[i]))
			{
				pPoints.RemoveAt(i);
			}
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00016E38 File Offset: 0x00015038
	private bool isOutsideLine(CameraFocusPoint.LineAndPoint pLineAndPoint, CameraFocusPoint pPoint)
	{
		return pLineAndPoint.line.DistanceTo(new Vector2(pPoint.transform.position.x, pPoint.transform.position.z)) > 0f;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00016E84 File Offset: 0x00015084
	private CameraFocusPoint PopClosestPoint(List<CameraFocusPoint> pPoints)
	{
		if (pPoints == null || pPoints.Count <= 0)
		{
			return null;
		}
		float num = float.MaxValue;
		int num2 = -1;
		for (int i = 0; i < pPoints.Count; i++)
		{
			if (this.DistTo(pPoints[i]) < num)
			{
				num2 = i;
				num = this.DistTo(pPoints[i]);
			}
		}
		if (num2 == -1)
		{
			return null;
		}
		CameraFocusPoint cameraFocusPoint = pPoints[num2];
		pPoints.RemoveAt(num2);
		return cameraFocusPoint;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00016F00 File Offset: 0x00015100
	private Vector3 DeltaPos(CameraFocusPoint pPoint)
	{
		if (pPoint == null)
		{
			return Vector3.zero;
		}
		return pPoint.transform.position - base.transform.position;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00016F3C File Offset: 0x0001513C
	private float DistTo(CameraFocusPoint pPoint)
	{
		return Vector3.Magnitude(pPoint.transform.position - base.transform.position);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00016F6C File Offset: 0x0001516C
	public CameraFocusPoint TestAgainstBorders(Vector3 pPosition)
	{
		Vector2 vector = new Vector2(pPosition.x, pPosition.z);
		foreach (CameraFocusPoint.LineAndPoint lineAndPoint in this._borders)
		{
			if (lineAndPoint.lineTwo.DistanceTo(vector) > 0f)
			{
				return lineAndPoint.point;
			}
		}
		return null;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00017008 File Offset: 0x00015208
	private Vector3[] GetPolygonPoints()
	{
		List<Vector3> list = new List<Vector3>();
		foreach (CameraFocusPoint.LineAndPoint lineAndPoint in this._borders)
		{
			Vector2[] closestIntersectionPoint = this.GetClosestIntersectionPoint(lineAndPoint);
			list.Add(new Vector3(closestIntersectionPoint[0].x, 0f, closestIntersectionPoint[0].y));
			list.Add(new Vector3(closestIntersectionPoint[1].x, 0f, closestIntersectionPoint[1].y));
		}
		return list.ToArray();
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000170CC File Offset: 0x000152CC
	private Vector2[] GetClosestIntersectionPoint(CameraFocusPoint.LineAndPoint tester)
	{
		Vector2 vector = tester.lineTwo.Point + tester.lineTwo.BiNormal * 1000f;
		Vector2 vector2 = tester.lineTwo.Point + tester.lineTwo.BiNormal * -1000f;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		foreach (CameraFocusPoint.LineAndPoint lineAndPoint in this._borders)
		{
			if (!(lineAndPoint.point == tester.point))
			{
				if (tester.lineTwo.DotProduct(tester.lineTwo.Normal, lineAndPoint.lineTwo.Normal) > 0f)
				{
					Vector2 intersectionPoint = lineAndPoint.lineTwo.GetIntersectionPoint(tester.lineTwo.Point, tester.lineTwo.BiNormal);
					float num3 = Vector2.Distance(intersectionPoint, tester.lineTwo.Point);
					if (num3 < num)
					{
						num = num3;
						vector = intersectionPoint;
					}
				}
				else if (tester.lineTwo.DotProduct(tester.lineTwo.Normal, lineAndPoint.lineTwo.Normal) < 0f)
				{
					Vector2 intersectionPoint2 = lineAndPoint.lineTwo.GetIntersectionPoint(tester.lineTwo.Point, tester.lineTwo.BiNormal);
					float num4 = Vector2.Distance(intersectionPoint2, tester.lineTwo.Point);
					if (num4 < num2)
					{
						num2 = num4;
						vector2 = intersectionPoint2;
					}
				}
			}
		}
		return new Vector2[] { vector, vector2 };
	}

	// Token: 0x040001DF RID: 479
	private const float ZONE_OVERLAP = 0.01f;

	// Token: 0x040001E0 RID: 480
	private static string[] _names = new string[]
	{
		"Kelly", "Lonnie", "Terence", "Emily", "Madison", "Hannah", "Emma", "Ashley", "Abigail", "Alexis",
		"Olivia", "Samantha", "Sarah", "Elizabeth", "Alyssa", "Grace", "Isabella", "Lauren", "Jessica", "Taylor",
		"Brianna", "Kayla", "Anna", "Victoria", "Sophia", "Natalie", "Sydney", "Chloe", "Megan", "Jasmine",
		"Rachel", "Hailey", "Morgan", "Destiny", "Julia", "Jennifer", "Kaitlyn", "Katherine", "Haley", "Alexandra",
		"Nicole", "Mia", "Savannah", "Maria", "Ava", "Mackenzie", "Allison", "Amanda", "Stephanie", "Brooke",
		"Makayla", "Jenna", "Faith", "Jordan", "Mary", "Rebecca", "Katelyn", "Andrea", "Kaylee", "Paige",
		"Gabrielle", "Madeline", "Ella", "Michelle", "Trinity", "Kimberly", "Sara", "Zoe", "Caroline", "Kylie",
		"Amber", "Vanessa", "Sierra", "Alexa", "Lily", "Danielle", "Erin", "Angelina", "Gabriella", "Riley",
		"Autumn", "Jada", "Leah", "Lillian", "Jacqueline", "Bailey", "Melissa", "Marissa", "Shelby", "Ariana",
		"Isabel", "Maya", "Courtney", "Audrey", "Leslie", "Claire", "Angela", "Sofia", "Jocelyn", "Evelyn",
		"Catherine", "Aaliyah", "Mariah", "Melanie", "Molly", "Arianna", "Christina", "Katie", "Breanna", "Diana",
		"Jade", "Avery", "Briana", "Kathryn", "Amy", "Laura", "Alexandria", "Isabelle", "Gabriela", "Madelyn",
		"Angel", "Kelsey", "Caitlin", "Brooklyn", "Adriana", "Margaret", "Lindsey", "Amelia", "Mikayla", "Kelly",
		"Kennedy", "Daniela", "Alicia", "Cheyenne", "Sabrina", "Mya", "Lydia", "Jillian", "Miranda", "Cassandra",
		"Cassidy", "Ana", "Gracie", "Tiffany", "Daisy", "Brittany", "Alexia", "Skylar", "Erica", "Mckenzie",
		"Summer", "Caitlyn", "Sophie", "Ashlyn", "Karen", "Gianna", "Angelica", "Crystal", "Nevaeh", "Kiara",
		"Natalia", "Alondra", "Hope", "Naomi", "Erika", "Peyton", "Kylee", "Bianca", "Valerie", "Kendall",
		"Payton", "Veronica", "Chelsea", "Jordyn", "Juliana", "Karina", "Kate", "Abby", "Jamie", "Heather",
		"Rylee", "Meghan", "Carly", "Valeria", "Bethany", "Jasmin", "Delaney", "Reagan", "Cynthia", "Ruby",
		"Aubrey", "Addison", "Karla", "Charlotte", "Mckenna", "Kyla", "Hayley", "Alejandra", "Giselle", "Maggie",
		"Monica", "Brenda", "Jazmin", "Julianna", "Makenzie", "Esmeralda", "Sadie", "Kristen", "Genesis", "Hanna",
		"Shannon", "Ariel", "Kyra", "Alana", "Rebekah", "Michaela", "Adrianna", "Eva", "Diamond", "Jayla",
		"Nadia", "Lizbeth", "Desiree", "Mallory", "Ellie", "Layla", "Elena", "Kaitlin", "Amaya", "Mariana",
		"Camryn", "Lindsay", "Aliyah", "Macy", "Kara", "Elise", "Julie", "Kendra", "Lucy", "Makenna",
		"Alison", "Selena", "Haylee", "Jazmine", "Liliana", "Claudia", "Zoey", "Savanna", "Nina", "Britney",
		"Fatima", "Joanna", "Vivian", "Holly", "Allyson", "Guadalupe", "Cameron", "Asia", "Raven", "Serenity",
		"Katelynn", "Josephine", "Nancy", "Katrina", "April", "Serena", "Camille", "Kailey", "Carmen", "Kathleen",
		"Celeste", "Ciara", "Carolina", "Lilly", "Jaden", "Cecilia", "Tatiana", "Sandra", "Kira", "Cindy",
		"Kirsten", "Tessa", "Heaven", "Skyler", "Cierra", "Kristina", "Paris", "Miriam", "Anastasia", "Jayden",
		"Alaina", "Brittney", "Patricia", "Dakota", "Wendy", "Bridget", "Casey", "Yesenia", "Tara", "Rachael",
		"Clara", "Esther", "Natasha", "Priscilla", "Christine", "Kayleigh", "Madeleine", "Josie", "Kassandra", "Alissa",
		"Tori", "Mercedes", "Paola", "Lauryn", "Melody", "Ashlee", "Annabelle", "Eleanor", "Sidney", "Laila",
		"Kamryn", "Nayeli", "Brenna", "Kiana", "Shayla", "Emilee", "Denise", "Rose", "Marisa", "Ashlynn",
		"Callie", "Allie", "Julissa", "Nia", "Imani", "Heidi", "Meredith", "Annie", "Logan", "Daniella",
		"Jadyn", "Alexus", "Ruth", "Eliza", "Annika", "Bryanna", "Camila", "Kaylie", "Anne", "Alayna",
		"Helen", "Bella", "Piper", "Aniya", "Clarissa", "Harley", "Dana", "Georgia", "Ashleigh", "Ashanti",
		"Anahi", "Kassidy", "Rosa", "Talia", "Yasmin", "Ivy", "Emely", "Aniyah", "Marina", "Lisa",
		"Whitney", "Dominique", "Jayda", "Shania", "Hallie", "Eden", "Cristina", "Fiona", "Nora", "Madalyn",
		"Reese", "Carolyn", "Deanna", "Elisabeth", "Eliana", "Hailee", "India", "Alivia", "Tabitha", "Halle",
		"Iris", "Kristin", "Marisol", "Tiana", "Lexi", "Tatum", "Linda", "Aurora", "Lesly", "Teresa",
		"Raquel", "Perla", "Krystal", "Stella", "Sasha", "Leilani", "Brooklynn", "Kiley", "Angie", "Jenny",
		"Virginia", "Gloria", "Leila", "Jaqueline", "Kaitlynn", "Sage", "Baylee", "Lacey", "Rylie", "Paulina",
		"Francesca", "Kiera", "Madisyn", "Phoebe", "Monique", "Itzel", "Alina", "Viviana", "Gillian", "Genevieve",
		"Noelle", "Alice", "Haleigh", "Tamia", "Regan", "Destinee", "Alisha", "Ayanna", "Renee", "Cadence",
		"Kaleigh", "Katlyn", "Skye", "Kyleigh", "Jane", "Carla", "Kaylin", "Martha", "Tania", "Brynn",
		"Malia", "Marie", "Taryn", "Madyson", "Gina", "Amari", "Izabella", "Deja", "Carissa", "Ashton",
		"Cora", "Alyson", "Lucia", "Johanna", "Cheyanne", "Janet", "Emilie", "Jessie", "Tia", "Kiersten",
		"Dulce", "Macie", "Gisselle", "Meagan", "Yasmine", "Mckayla", "Precious", "Tamara", "Justice", "Carley",
		"Krista", "Ellen", "Keira", "Elaina", "Maddison", "Cara", "Kierra", "Abbey", "Joy", "Jacquelyn",
		"Mikaela", "Alanna", "Karissa", "Janelle", "Fernanda", "Maritza", "Ryleigh", "America", "Anya", "Kasey",
		"Joselyn", "Susan", "Amya", "Larissa", "Ryan", "Sarai", "Athena", "Miracle", "Jaiden", "Pamela",
		"Ainsley", "Litzy", "Sharon", "Kali", "Cassie", "Abbigail", "Elisa", "Aileen", "Tiara", "Anika",
		"Marlene", "Emilia", "Kaley", "Kailee", "Lena", "Janiya", "Charity", "Irene", "Theresa", "Mayra",
		"Raegan", "Estrella", "Carlie", "Angelique", "Sylvia", "Nyla", "Brandy", "Sonia", "Jaelyn", "Simone",
		"Marilyn", "Nataly", "Bailee", "Samara", "Sienna", "Tyler", "Valentina", "Madelynn", "Brandi", "Janae",
		"Madilyn", "Macey", "Araceli", "Isis", "Jaclyn", "Maia", "Abbie", "Lorena", "Aspen", "Rhiannon",
		"Lexie", "Lesley", "Melina", "Alma", "Kelsie", "Daphne", "Colleen", "Ximena", "Ann", "Kaila",
		"Violet", "Kailyn", "Felicity", "Lila", "Luz", "Marley", "Haylie", "Kenya", "Hayden", "Liberty",
		"Isabela", "Eve", "Willow", "Lilian", "Nathalie", "Laney", "Alessandra", "Harmony", "Helena", "Tatyana",
		"Brielle", "Judith", "Deborah", "Jimena", "Cristal", "Arielle", "Julianne", "Zaria", "Reyna", "Justine",
		"Kaya", "Aimee", "Noemi", "Zoie", "Regina", "Anaya", "Tianna", "Tess", "Kaelyn", "Barbara",
		"Juliet", "Tyra", "Clare", "Tanya", "Teagan", "Skyla", "Sydnee", "Jaida", "Janessa", "Yadira",
		"Gwendolyn", "Edith", "Shyanne", "Elaine", "Chelsey", "Brisa", "Thalia", "Chasity", "Karlee", "Maci",
		"Ally", "Lana", "Jazlyn", "Lillie", "Frances", "Adrienne", "Mara", "Hunter", "Esperanza", "Kaylyn",
		"Delilah", "Stacy", "Paula", "Savanah", "Juliette", "Celia", "Presley", "Carlee", "Lea", "Mariela",
		"Haven", "Felicia", "Hana", "Carina", "Hailie", "Leticia", "Adeline", "Jazmyn", "Mollie", "Mariam",
		"Corinne", "Kenzie", "Nichole", "Scarlett", "Breana", "Jolie", "Laurel", "Aisha", "Kayley", "Lia",
		"Alisa", "Karlie", "Eileen", "Rosemary", "Toni", "Arely", "Amani", "Tina", "Tayler", "Karli",
		"Kennedi", "Aubree", "Halie", "Hazel", "Carrie", "Keely", "Leanna", "Aracely", "Lizeth", "Cayla",
		"Ansley", "Shaniya", "Alena", "Aleah", "Amara", "Stephany", "Antonia", "Giovanna", "Ayana", "Micah",
		"Jewel", "Dayana", "Ingrid", "Ciera", "Kaia", "Devin", "Maribel", "Annette", "Maeve", "Quinn",
		"Lola", "Amira", "Damaris", "Melany", "Yazmin", "Ebony", "Nya", "Kaylynn", "Blanca", "Desirae",
		"Elle", "Joyce", "Shayna", "Salma", "Yvette", "Monserrat", "Cecelia", "Destiney", "London", "Kianna",
		"Aria", "Greta", "Jaylin", "Donna", "Micaela", "Lyric", "Aliya", "Abigayle", "Annalise", "Lara",
		"Emmalee", "Lilliana", "Destini", "Nicolette", "Parker", "Candace", "Elyse", "Catalina", "Aiyana", "Rebeca",
		"Dayanara", "Hadley", "Maura", "Katharine", "Kadence", "Marianna", "Myah", "Jenifer", "Chaya", "Essence",
		"Kathy", "Ayla", "Liana", "Mattie", "Nikki", "Sandy", "Kaydence", "Shea", "Karly", "Stacey",
		"Cali", "Katarina", "Gia", "Shyann", "Dylan", "Bria", "Katelin", "Princess", "Margarita", "Kelli",
		"Jalyn", "Penelope", "Lizette", "Aryanna", "Iliana", "Kayli", "Gretchen", "Laci", "Jacey", "Celine",
		"Moriah", "Shaylee", "Devon", "Kasandra", "Ryann", "Alize", "Alysa", "Jamya", "Susana", "Jakayla",
		"Patience", "Carol", "Elsa", "Taliyah", "Sydni", "Kaylah", "Christian", "Joana", "Estefania", "Aliza",
		"Savana", "Devyn", "Elissa", "Ashly", "Arlene", "Annabella", "Raina", "Taniya", "Trista", "Jaycee",
		"Saige", "Frida", "Sheila", "Annabel", "Alia", "Rayna", "Jasmyn", "Shirley", "Karley", "Kenna",
		"Kenia", "Kendal", "Graciela", "Paloma", "Jaidyn", "Abigale", "Magdalena", "Kallie", "Kaiya", "Calista",
		"Marlee", "Abril", "Iyana", "Christiana", "Madisen", "Mireya", "Selina", "Ericka", "Nyah", "Robin",
		"Fabiola", "Danna", "Emerson", "Abagail", "Carson", "Jaylynn", "Nyasia", "Belen", "Kaliyah", "Aylin",
		"Elyssa", "Meadow", "Anissa", "Lyndsey", "Lexus", "Brianne", "Tierra", "Mandy", "Janiyah", "Kya",
		"Montana", "Dalia", "Jailyn", "Beatriz", "Noelia", "Tracy", "Reilly", "Yuliana", "Dorothy", "Amiya",
		"Kourtney", "Janice", "Robyn", "Sydnie", "Aubrie", "Jaime", "Carli", "Anita", "Evelin", "Diane",
		"Shakira", "Rocio", "Miah", "Armani", "Reina", "Madalynn", "Alexandrea", "Dasia", "Christa", "Makaila",
		"Norah", "Sarahi", "Journey", "Kacie", "Casandra", "Jackeline", "Joslyn", "Amina", "Makena", "Ashtyn",
		"Joelle", "Kellie", "Citlali", "Lina", "Sky", "Jana", "Taya", "Jaliyah", "Giana", "Christy",
		"Anjali", "Kaci", "Jaylene", "Yareli", "Johana", "Rubi", "Saniya", "Sally", "Shawna", "Katy",
		"Berenice", "Galilea", "Candice", "Sherlyn", "Shreya", "Luna", "Natalee", "Libby", "Hillary", "Yoselin",
		"Maliyah", "Rachelle", "Roselyn", "Loren", "Areli", "Keyla", "Celina", "Isabell", "Camilla", "Kayden",
		"Samira", "Chanel", "Drew", "Aryana", "Darlene", "Unique", "Alexys", "Sanaa", "Jaylyn", "Roxana",
		"Jalynn", "Silvia", "Kinsey", "Dianna", "Bryana", "Mira", "Baby", "Reanna", "Iyanna", "Maleah",
		"Kalyn", "Lainey", "Delia", "Campbell", "Miya", "Rowan", "Natalya", "Myra", "Yessenia", "Amiyah",
		"Jazmyne", "Brionna", "Alex", "Taniyah", "Chyna", "Meaghan", "Melinda", "Lacie", "Amelie", "Lucille",
		"Kelsi", "Lacy", "Maegan", "Sarina", "Kristine", "Sheridan", "Mina", "Phoenix", "Chandler", "Jeanette",
		"Allyssa", "Kimora", "Jacklyn"
	};

	// Token: 0x040001E1 RID: 481
	private static int nameIterator = 0;

	// Token: 0x040001E2 RID: 482
	public CameraFocusPoint.ZoomControl zoomControl = CameraFocusPoint.ZoomControl.NO;

	// Token: 0x040001E3 RID: 483
	public Vector2 angleControl = default(Vector2);

	// Token: 0x040001E4 RID: 484
	public bool controlX_Pitch;

	// Token: 0x040001E5 RID: 485
	public bool controlY_Yaw;

	// Token: 0x040001E6 RID: 486
	public bool lockX_Pitch;

	// Token: 0x040001E7 RID: 487
	public bool lockY_Yaw;

	// Token: 0x040001E8 RID: 488
	public bool lockZoom = true;

	// Token: 0x040001E9 RID: 489
	public bool followCharacter;

	// Token: 0x040001EA RID: 490
	public List<SerializableKeyValuePair> _weightRelations = new List<SerializableKeyValuePair>();

	// Token: 0x040001EB RID: 491
	public string __camname;

	// Token: 0x040001EC RID: 492
	public List<CameraFocusPoint.LineAndPoint> _borders = new List<CameraFocusPoint.LineAndPoint>();

	// Token: 0x040001ED RID: 493
	private Vector3[] _polygonPoints;

	// Token: 0x02000052 RID: 82
	public enum ZoomControl
	{
		// Token: 0x040001EF RID: 495
		NO = -1,
		// Token: 0x040001F0 RID: 496
		EXTRA_CLOSE,
		// Token: 0x040001F1 RID: 497
		CLOSE,
		// Token: 0x040001F2 RID: 498
		MEDIUM,
		// Token: 0x040001F3 RID: 499
		FAR,
		// Token: 0x040001F4 RID: 500
		EXTRA_FAR
	}

	// Token: 0x02000053 RID: 83
	public struct LineAndPoint
	{
		// Token: 0x040001F5 RID: 501
		public Line2D line;

		// Token: 0x040001F6 RID: 502
		public Line2D lineTwo;

		// Token: 0x040001F7 RID: 503
		public CameraFocusPoint point;
	}
}
