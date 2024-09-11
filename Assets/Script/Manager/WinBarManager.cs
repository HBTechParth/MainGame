using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class WinBarManager : MonoBehaviour
{
    public TextMeshProUGUI[] winBarTexts; // Array of 3 UI Text elements
    private List<string> playerNames;
    private string[] gameNames = { "Joker", "Deal Rummy", "Poker", "Teen Patti", "Ludo", "Jhandi Munda", "Andar Bahar", "Roulette", "CarRoulette", "AK47 TeenPatti", "7Up Down", "Point Rummy", "Pool Rummy", "Dragon Tiger", "Avaitor" };
    private List<string> remainingNames;

    private bool isInitialRun = true; // To track the first run and apply -625X wait logic only once


    /*public Transform object1;
    public Transform object2;
    public Transform object3;
    public float shakeDuration = 1f;
    public float shakeStrength = 1f;
    public int shakeVibrato = 10;
    public float delayBetweenShakes = 2f;*/

    public Transform spinImg;
    public Transform settingImg;
    void Start()
    {
        playerNames = GenerateNamesList();
        remainingNames = new List<string>(playerNames);

        // Initialize the positions of the texts to be off-screen (-1200X)
        for (int i = 0; i < winBarTexts.Length; i++)
        {
            winBarTexts[i].transform.localPosition = new Vector3(-1300f, winBarTexts[i].transform.localPosition.y, 0f);
        }

        StartCoroutine(UpdateWinBar());
        spinImg.transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360)
           .SetLoops(-1, LoopType.Restart) // -1 for infinite loops
           .SetEase(Ease.Linear);
        settingImg.transform.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360)
           .SetLoops(-1, LoopType.Restart) // -1 for infinite loops
           .SetEase(Ease.Linear);

        //  StartCoroutine(ShakeSequence());
    }
    /*  IEnumerator ShakeSequence()
      {
          while (true) // Infinite loop for repeating the shakes
          {
              // Shake both objects simultaneously
              object1.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
              object2.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
              object3.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);

              // Wait for the duration of the shake
              yield return new WaitForSeconds(shakeDuration);

              // Add delay before the next shake
              yield return new WaitForSeconds(delayBetweenShakes);
          }
      }*/
    List<string> GenerateNamesList()
    {
        List<string> names = new List<string>
        {
            // Boys' Names
            "Aarav", "Vivaan", "Aditya", "Vihaan", "Arjun", "Sai", "Krishna", "Ishaan", "Shaurya", "Ayaan",
            "Dhruv", "Ritvik", "Kabir", "Rishi", "Arnav", "Ansh", "Samar", "Atharva", "Advait", "Om",
    "Dev", "Ved", "Shaan", "Armaan", "Raunak", "Laksh", "Yuvaan", "Aryan", "Nirvaan", "Rohan",
    "Reyansh", "Aakash", "Kartik", "Siddharth", "Gaurav", "Pranav", "Yash", "Ishan", "Kunal", "Nikhil",
    "Mihir", "Raghav", "Sarthak", "Vikram", "Aayush", "Harsh", "Rajat", "Ajay", "Varun", "Vikrant",
    "Aman", "Ravindra", "Rakesh", "Sandeep", "Vishal", "Naveen", "Jay", "Tarun", "Rajesh", "Ashish",
    "Chetan", "Bhavesh", "Neeraj", "Anuj", "Manish", "Parth", "Sanjeev", "Ravinder", "Saurabh", "Puneet",
    "Rahul", "Amit", "Ravi", "Arpit", "Prateek", "Vikas", "Akhil", "Rohit", "Ankit", "Piyush",
    "Rohan", "Anurag", "Siddhesh", "Rajat", "Harshil", "Uday", "Lokesh", "Mahesh", "Sunil", "Vivek",
    "Deepak", "Keshav", "Sumit", "Jatin", "Ashwin", "Karan", "Rishabh", "Arvind", "Mohan", "Yuvraj",
    "Raj", "Hemant", "Balraj", "Jai", "Bharat", "Raghuraj", "Vinay", "Ritesh", "Keshav", "Bhargav",
    "Kiran", "Sameer", "Shravan", "Sachin", "Anil", "Vinod", "Shyam", "Gopal", "Abhinav", "Atul",
    "Satyam", "Shivam", "Ravish", "Jayesh", "Raghavendra", "Kishore", "Harshvardhan", "Tejas", "Suraj", "Dharmendra",
    "Bhupesh", "Lalit", "Sanjeet", "Hitesh", "Harpreet", "Vikash", "Ganesh", "Prabhakar", "Rohini", "Vimal",
    "Narendra", "Bhupinder", "Jaspreet", "Jatin", "Rajeev", "Kailash", "Ajit", "Akhilesh", "Rajinder", "Mandeep",
    "Yashwant", "Prakash", "Mukesh", "Mahendra", "Raman", "Sudarshan", "Sushant", "Dinesh", "Rajan", "Bhagwan",
    "Kamal", "Anirudh", "Sushil", "Tarachand", "Girish", "Shankar", "Vishnu", "Jagmohan", "Shivendra", "Raghvendra",
    "Ramesh", "Madhukar", "Ravikant", "Vikramaditya", "Suryakant", "Avinash", "Tapan", "Prabhu", "Nilesh", "Sachindra",
    "Pradip", "Jayant", "Deepesh", "Vasudev", "Yogesh", "Manoj", "Surendra", "Vikrant", "Madhav", "Sumit",
    "Gautam", "Sushil", "Harsh", "Praveen", "Sanjay", "Narayan", "Devendra", "Nirmal", "Keshav", "Ravindra",
    "Rajat", "Brijesh", "Srinivas", "Ashok", "Vivekanand", "Navin", "Maheshwar", "Subhash", "Bhuvan", "Ajinkya",
    "Bharat", "Shantanu", "Vasudev", "Dilip", "Vijay", "Gopal", "Rajendra", "Kalyan", "Kailash", "Rajaram",
    "Shantanu", "Praveer", "Vinit", "Jeevan", "Karanveer", "Ravi", "Virendra", "Keshav", "Manik", "Krishnakant",
    "Dharmesh", "Vikas", "Ramendra", "Jayant", "Girish", "Ravindra", "Sudhir", "Dev", "Nandan", "Niranjan",
    "Vinod", "Himanshu", "Gaurang", "Harinder", "Siddheshwar", "Raghav", "Tarun", "Rameshwar", "Sushant", "Upendra",
    "Trilok", "Naresh", "Prem", "Vijayendra", "Saurabh", "Vikramjeet", "Nitin", "Rajkumar", "Mithilesh", "Manish",
    "Bhaskar", "Shrikant", "Raghu", "Narender", "Prashant", "Satyendra", "Abhishek", "Umesh", "Kailash", "Rajan",
    "Vikash", "Rajiv", "Deepak", "Sandeep", "Sanjay", "Vikas", "Virat", "Siddharth", "Kiran", "Madhusudan",
    "Sumit", "Rajeev", "Kartikeya", "Lalit", "Rahul", "Mahesh", "Sanjit", "Manoj", "Ajay", "Chetan",
    "Rajnish", "Alok", "Gaurav", "Hemant", "Mukesh", "Amit", "Rakesh", "Raj", "Jitendra", "Virendra",
    "Rajesh", "Anand", "Rajat", "Kunal", "Nishant", "Vikas", "Anuj", "Rajeev", "Yogendra", "Ravindra",
    "Sanjay", "Narendra", "Anil", "Suresh", "Satish", "Vikram", "Suhas", "Kamal", "Ajit", "Mahavir",
    "Chetan", "Rohit", "Vinod", "Vimal", "Yash", "Sudhanshu", "Sunil", "Ashok", "Sandeep", "Sumit",
    "Vijay", "Gaurav", "Bhupesh", "Vishal", "Naveen", "Karan", "Mahendra", "Deepak", "Vijayendra", "Harish",
    "Manish", "Ankit", "Prashant", "Vikramaditya", "Kiran", "Suresh", "Madhav", "Mahesh", "Pravin", "Pramod",
    "Vinay", "Rajeev", "Amrit", "Vishnu", "Anand", "Virendra", "Sudhir", "Saurabh", "Kunal", "Rajkumar",
    "Ravi", "Jitendra", "Sandeep", "Ravikant", "Narendra", "Subhash", "Mahesh", "Alok", "Chetan", "Rajendra",
    "Ashutosh", "Rajnish", "Piyush", "Yogesh", "Kailash", "Arun", "Sumit", "Ajay", "Maheshwar", "Sanjit",
    "Naresh", "Virendra", "Harsh", "Pramod", "Nitin", "Sushant", "Rakesh", "Keshav", "Tarun", "Rajan",
    "Karanveer", "Rohit", "Hemant", "Ajinkya", "Anirudh", "Bhavesh", "Shrikant", "Sudhanshu", "Ashwin", "Yogendra"
            ,
            // Girls' Names
            "Aaradhya", "Aarushi", "Aarya", "Aashvi", "Aayushi", "Abha", "Aditi", "Advika", "Aishwarya", "Akanksha",
            "Akshara", "Alia", "Alisha", "Amara", "Amaya", "Amrita", "Ananya", "Anika", "Anisha", "Anjali",
    "Ankita", "Anoushka", "Anya", "Aparna", "Apoorva", "Aradhya", "Arohi", "Arpita", "Arundhati", "Arya",
    "Asha", "Ashima", "Asmita", "Aswini", "Athira", "Avani", "Avika", "Avni", "Ayushi", "Bhavya",
    "Bhumika", "Bindiya", "Brija", "Chaitali", "Chandani", "Charu", "Chavi", "Chhavi", "Chitra", "Damini",
    "Darshana", "Deepa", "Deepika", "Devika", "Devyani", "Dhriti", "Dhvani", "Diya", "Divya", "Eesha",
    "Eshani", "Esha", "Eshika", "Gauri", "Gayatri", "Geetanjali", "Geeta", "Gita", "Gitanjali", "Gul",
    "Gunjan", "Harini", "Harsha", "Hema", "Hemangi", "Himani", "Indira", "Isha", "Ishani", "Ishika",
    "Ishita", "Jahnavi", "Jasleen", "Jaya", "Jayashree", "Jeevika", "Jhanvi", "Jigna", "Juhi", "Jyoti",
    "Kajal", "Kalpana", "Kamakshi", "Kamini", "Kanchan", "Kangana", "Kanika", "Karisma", "Kashish", "Kavya",
    "Keerthi", "Khushbu", "Khushi", "Kirti", "Kritika", "Krishna", "Kumud", "Kusum", "Laila", "Lakshmi",
    "Lalita", "Lavanya", "Leela", "Lina", "Lipika", "Lisha", "Liya", "Madhavi", "Madhuri", "Mahima",
    "Maithili", "Malini", "Manasi", "Manisha", "Manju", "Mansi", "Megha", "Meghna", "Meher", "Mihika",
    "Mira", "Mohini", "Moksha", "Mona", "Mridula", "Mukta", "Myra", "Naina", "Namita", "Nandini",
    "Neelam", "Neha", "Nidhi", "Nikita", "Nimisha", "Nina", "Niranjana", "Nirmala", "Nisha", "Nishtha",
    "Nitika", "Nitya", "Ojasvi", "Omisha", "Padma", "Pallavi", "Pari", "Parineeti", "Parul", "Pihu",
    "Pooja", "Poorva", "Prachi", "Pragya", "Pranavi", "Pratibha", "Preeti", "Preksha", "Priya", "Priyanka",
    "Puja", "Purnima", "Rachana", "Radha", "Ragini", "Raksha", "Rama", "Ramya", "Ranjana", "Rashi",
    "Rati", "Raveena", "Reena", "Renu", "Reva", "Rhea", "Richa", "Riddhi", "Rina", "Rishika",
    "Rita", "Ritu", "Riya", "Rohini", "Roma", "Roshni", "Rubina", "Ruchi", "Rudra", "Rupa",
    "Rupali", "Saanvi", "Sabrina", "Sadhna", "Saheli", "Sai", "Sakshi", "Saloni", "Samaira", "Sana",
    "Sandhya", "Sangeeta", "Sanjana", "Santosh", "Sarika", "Sarita", "Sarla", "Saroj", "Sarvesh", "Sasha",
    "Sashmita", "Seema", "Shaila", "Shakshi", "Shalini", "Shama", "Shambhavi", "Shanaya", "Sharanya", "Sharmila",
    "Sharika", "Shashi", "Shefali", "Sheetal", "Shikha", "Shilpa", "Shivani", "Shobha", "Shreya", "Shruti",
    "Shubha", "Shubhangi", "Shweta", "Simran", "Sindhu", "Sneha", "Soniya", "Sparsha", "Srishti", "Stuti",
    "Sudha", "Sujata", "Sukanya", "Suman", "Sundari", "Sunita", "Supriya", "Surbhi", "Surekha", "Suruchi",
    "Sushma", "Swara", "Swati", "Tanisha", "Tanvi", "Tara", "Tejaswini", "Tripti", "Trishna", "Tulika",
    "Uma", "Urmila", "Urmi", "Usha", "Vaidehi", "Vaishali", "Vandana", "Varsha", "Vasudha", "Vidya",
    "Vineeta", "Vinita", "Vrinda", "Yamini", "Yashasvi", "Yashika", "Yashodhara", "Yogita", "Yukti", "Zara",
    "Zarina", "Zoya", "Akshita", "Aradhya", "Ashika", "Asmita", "Ayesha", "Chhaya", "Damini", "Ganga",
    "Gayatri", "Hemlata", "Indira", "Janaki", "Kajal", "Kamala", "Kanchan", "Kavita", "Kiran", "Kripa",
    "Kriti", "Leena", "Lila", "Lina", "Mala", "Malati", "Mallika", "Meena", "Menaka", "Minakshi",
    "Minu", "Mira", "Nalini", "Neena", "Nila", "Nilima", "Padmaja", "Padmini", "Parvati", "Poornima",
    "Prerna", "Priyal", "Prithika", "Radha", "Rajani", "Rakhi", "Rama", "Ramita", "Ranjita", "Rasika",
    "Ratna", "Rekha", "Renuka", "Rina", "Ritika", "Roma", "Roopa", "Roshan", "Rupa", "Sagarika",
    "Sahira", "Sakina", "Salma", "Sampa", "Sanchita", "Sandhya", "Sangeeta", "Santosh", "Sapna", "Sarika",
    "Sarla", "Saroj", "Sarojini", "Satyabhama", "Savita", "Shaila", "Shakti", "Shanta", "Sharmila", "Sheela",
    "Sheetal", "Shilpa", "Shobha", "Shraddha", "Shreya", "Shruti", "Sita", "Soniya", "Sulekha", "Sunanda",
    "Sunayna", "Sundari", "Sunita", "Sushila", "Swarnlata", "Swati", "Tanisha", "Tanuja", "Tejaswini", "Tilottama",
    "Triveni", "Uma", "Urvashi", "Usha", "Vaidehi", "Vandana", "Vanita", "Varsha", "Vasundhara", "Vasudha",
    "Veena", "Vidya", "Vimala", "Vinita", "Vishakha", "Yamuna", "Yashodhara", "Yashodhra", "Yashoda", "Zainab",
    "Zarina", "Zeenat", "Zoya", "Aakruti", "Aamani", "Aanchal", "Aarohi", "Aashna", "Aashita", "Abhilasha",
    "Aditi", "Advaita", "Agamya", "Ahana", "Aishani", "Aishwarya", "Akira", "Alaknanda", "Amala", "Ambika",
    "Amita", "Amrapali", "Amrita", "Anamika", "Anandi", "Ananya", "Anila", "Ankita", "Anshika", "Anshu",
    "Anshula", "Anuja", "Anupama", "Anuradha", "Anusha", "Anushree", "Apeksha", "Apurva", "Aruna", "Arundhati",
    "Ashwini", "Asita", "Asmita", "Atasi", "Avni", "Ayanna", "Ayesha", "Bela", "Bhagyashree", "Bhavana",
    "Bindiya", "Brahmani", "Brinda", "Charulata", "Chaya", "Chhavi", "Damayanti", "Debjani", "Deepali", "Devika",
    "Dhanashree", "Dharani", "Dhatri", "Dhruti", "Divija", "Drishti", "Durga", "Esha", "Gargi", "Geeta",
    "Geetika", "Ginni", "Gomati", "Hamsa", "Hansini", "Harini", "Hema", "Hemangi", "Himani", "Ishana",
    "Jagrati", "Jalpa", "Janhavi", "Jayanti", "Jeevitha", "Jyotsna", "Kalavati", "Kalpana", "Kamini", "Kanaka",
    "Kanika", "Kanti", "Karishma", "Karuna", "Kaveri", "Kavya", "Khyati", "Komal", "Krishna", "Krithika",
    "Krupali", "Kshirja", "Kumari", "Kusum", "Lalita", "Latika", "Lavanya", "Lekha", "Lina", "Madhu",
    "Madhuri", "Mahima", "Mala", "Malati", "Malini", "Manasa", "Manasi", "Manju", "Manjusha", "Manorama",
    "Maya", "Meenal", "Meera", "Meghna", "Menaka", "Minakshi", "Mohana", "Moksha", "Monisha", "Mridula",
    "Mukta", "Nalini", "Nandini", "Navya", "Neha", "Nikita", "Nila", "Nilima", "Nirmala", "Nisha",
    "Nishtha", "Nitya", "Pallavi", "Parvati", "Pavitra", "Payal", "Poornima", "Prachi", "Pragati", "Pragya",
    "Pranavi", "Prarthana", "Preeti", "Priya", "Priyanka", "Purnima", "Radha", "Ragini", "Rajani", "Rama",
    "Ramita", "Ranjana", "Rasika", "Rati", "Rekha", "Renuka", "Riddhi", "Ritika", "Roshni", "Rubina",
    "Rupali", "Sadhana", "Saheli", "Sakshi", "Saloni", "Samaira", "Sanchita", "Sandhya", "Sangita", "Sanjana",
    "Santosh", "Sarika", "Sarita", "Saroj", "Savita", "Seema", "Shaila", "Shalini", "Sharada", "Shashi",
    "Shikha", "Shilpa", "Shobha", "Shraddha", "Shreya", "Shruti", "Sita", "Sneha", "Soniya", "Sulekha",
    "Suman", "Sundari", "Sunita", "Supriya", "Surekha", "Swati", "Tanisha", "Tanvi", "Tara", "Tejaswini",
    "Trishna", "Triveni", "Udaya", "Uma", "Urmila", "Usha", "Vaidehi", "Vaishali", "Vandana", "Varsha",
    "Vasundhara", "Vasudha", "Veena", "Vidya", "Vimala", "Vinita", "Vrinda", "Yamini", "Yashasvi", "Yashodhara"

        };

        for (int i = 0; i < 480; i++)
        {
            names.Add($"Name{i + 1}");
        }

        return names;
    }

    IEnumerator UpdateWinBar()
    {
        // Initial run logic, waits for each text to reach -625X
        if (isInitialRun)
        {
            yield return StartCoroutine(AssignAndMoveText(0));
            yield return StartCoroutine(WaitForTextToReachPosition(0, -500f));
            yield return StartCoroutine(AssignAndMoveText(1));
            yield return StartCoroutine(WaitForTextToReachPosition(1, -500f));
            yield return StartCoroutine(AssignAndMoveText(2));
            /* yield return StartCoroutine(WaitForTextToReachPosition(2, -450f));
             yield return StartCoroutine(AssignAndMoveText(3));*/

            isInitialRun = false; // Set this to false after the initial wait logic
        }

        // Continuous loop where texts are reset after reaching 1200X
        while (true)
        {
            for (int i = 0; i < winBarTexts.Length; i++)
            {
                if (winBarTexts[i].transform.localPosition.x >= 1300f)
                {
                    // Reset position to -1200X and assign a new message
                    winBarTexts[i].transform.localPosition = new Vector3(-1300f, winBarTexts[i].transform.localPosition.y, 0f);
                    yield return StartCoroutine(AssignAndMoveText(i));
                }
            }

            yield return null; // Continue the loop
        }
    }

    IEnumerator AssignAndMoveText(int index)
    {
        // Check if remaining names list is empty, reset it
        if (remainingNames.Count == 0)
        {
            remainingNames = new List<string>(playerNames);
        }

        // Select random player, game, and amount
        int randomIndex = Random.Range(0, remainingNames.Count);
        string randomPlayer = remainingNames[randomIndex];
        remainingNames.RemoveAt(randomIndex);

        if (Random.Range(0, 2) == 1)
        {
            randomPlayer = GenerateUsername(randomPlayer);
        }

        string randomGame = gameNames[Random.Range(0, gameNames.Length)];
        int randomAmount = GenerateRandomAmount();

        // Assign text to the specific text element in the win bar
        winBarTexts[index].text = $"<color=yellow>[ {randomPlayer} ]</color> won [ {randomAmount} ]  <sprite=0> in <color=yellow>[ {randomGame} ]</color>";

        // Move the text to the target position
        float moveDuration = 10f; // Duration for the text to move across the screen
        Vector3 targetPosition = new Vector3(1300f, winBarTexts[index].transform.localPosition.y, 0f);

        // Start movement
        winBarTexts[index].transform.DOLocalMoveX(targetPosition.x, moveDuration).SetEase(Ease.Linear);

        yield return null;
    }

    IEnumerator WaitForTextToReachPosition(int index, float targetX)
    {
        while (winBarTexts[index].transform.localPosition.x < targetX)
        {
            yield return null; // Wait until the text reaches the target X position
        }
    }

    public int GenerateRandomAmount()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < 0.8f)
        {
            return UnityEngine.Random.Range(100, 2001);
        }
        else if (randomValue < 0.95f)
        {
            return UnityEngine.Random.Range(2000, 5001);
        }
        else if (randomValue < 0.99f)
        {
            return UnityEngine.Random.Range(5000, 10001);
        }
        else
        {
            return UnityEngine.Random.Range(5, 101);
        }
    }

    string GenerateUsername(string name)
    {
        int randomNumber = Random.Range(100, 10000);
        return $"{name}{randomNumber}";
    }
}
