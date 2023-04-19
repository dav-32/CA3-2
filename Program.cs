/*#################################################################################################################################
|              /             /           ***      Author: Damhan Mac Fadden        ***            \               \                |
|             /             /             ***          Date : 130423              ***              \               \               |
|            /             /               ***         Title: CA3                ***                \               \              |
##################################################################################################################################*/


using System.Globalization;// Culture information
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

// #1. For file validation, I was getting stuck in catch loop and couldn't see why. I tried with a bool for fileOpen but still was getting
//     the same, so only resolved this by using Environment.Exit(1);

// #2. I only have the seperate classes created but didn't actually use them for the program as I was able to get it working without them, and
//     I don't have a good full understanding of the over and back passing using classes yet.

//#3.  At the minute the validation for 10 csv and date is working. The date is working fine, but the csv one isn't perfect as if you input
//     a line with less than 10 csv values it includes 10 for that one as it sees the other lines have 10, so doesn't catch an error. If you
//     enter a line with 12 csv values, it prints out all lines as errors as it adds 2 extra blank spaces to all lines.
//
//#4.  I have a line at the end of the csv with an age input, occupation input and incorrect date format. The age or occupation do not get added
//     and the incorrect date is picked up by the check. However, when running on the college computer the last ship with the date format of
//     06/26/1851 picks up an error for all lines and doesn't get added to any of the reports. The date is set to US-EN and doesn't give this
//     error on my computer though.


namespace CA3
{
    internal class Program
    {

        //-----------------------------------------------Class Level Variables----------------------------------------------------------//
        static string menuChoice;                                                      // User input for menu choice
        static string[] fields = null;                                                 // Unknown length array     

        static string filePath;                                                        // CSV file string
        static int lineNumber = 1;

        static string shipChoice;
        static string fileChoice;




        //--------------------------------------------------- Main Method --------------------------------------------------------------//
        static void Main(string[] args)
        {
            ChooseFile();                                                               // Choose the file
            MenuInput();                                                                // Gives user menu selection
            while (menuChoice != "4")                                                     // Enter switch
            { MenuChoiceSwitch(); }
            ExitProgram();                                                              // Exit Program method


        }
        //---------------------------------------------------   Methods ------------------------------------------------------------------//
        static void ChooseFile()
        {
           

            
            DoubleBreakerLine();
            SpaceLine();
            Console.WriteLine("Welcome to our Famine Records Inspection program!");
            SpaceLine();
            DoubleBreakerLine();
            

            bool isValid = false;
            do
            {
            SpaceLine();
            Console.WriteLine("We have two files to select from:");
            SpaceLine();
            Console.WriteLine("1. Famine File");
            Console.WriteLine("2. Famine File Sligo");
            SpaceLine();
            Console.Write("Please choose which file you would like to inspect:");
            fileChoice = Console.ReadLine();
            if (IsPresent(fileChoice, "file choice") == true && IsInteger(fileChoice, "File choice") == true && IsWithInRangeInt(fileChoice, "File choice input", 1, 2) == true)
            {
                isValid = true;

            }

            else
                isValid = false;
        }
            while (isValid == false);

            if (fileChoice == "1")
            { filePath = "../../../faminefile.csv"; }
            else if (fileChoice == "2")
            { filePath = "../../../faminefile2.csv"; }

            SpaceLine();

        }
        
        
        
        
        static string MenuInput()                                                          //  Method for menu selection
        {
            bool isValid = false;

            do
            {
                SpaceLine();
                BreakerLine();
                SpaceLine();
                Console.WriteLine("MAIN MENU");
                SpaceLine();
                BreakerLine();
                Console.WriteLine("1. Ship Reports");
                Console.WriteLine("2. Occupation Report");
                Console.WriteLine("3. Age Reports");
                Console.WriteLine("4. Exit");
                SpaceLine();
                Console.Write("Enter choice : ");
                menuChoice = Console.ReadLine();


                if (IsPresent(menuChoice, "menu choice") == true && IsInteger(menuChoice, "Menu choice") == true && IsWithInRangeInt(menuChoice, "Menu choice input", 1, 4) == true)
                {
                    isValid = true;

                }

                else
                    isValid = false;
            }
            while (isValid == false);
            SpaceLine();
            BreakerLine();
            return menuChoice;
        }

        static void ExitProgram()                                                       // Method to exit program
        {
            SpaceLine();
            BreakerLine();
            SpaceLine();
            Console.WriteLine("Program terminated. Thank you!");
            SpaceLine();
            BreakerLine();
        }

        static void MenuChoiceSwitch()                                                  // Switch for user selection in menu
        {

            switch (menuChoice)
            {
                case "1":
                    Menu1();                                                            // Individual Ship Information
                    break;
                case "2":
                    Menu2();                                                            // Occupation Report
                    break;
                case "3":
                    Menu3();                                                            // Age Report
                    break;

                default:                                                                // If anything apart from 1,2,3 is entered
                    Console.WriteLine("Invalid entry, please try again.");
                    MenuInput();
                    break;


            }
        }

        static void Menu1()                                                                     // Ship Reports
        {

            try
            {

                List<string> ships = new List<string>();
                using (StreamReader reader = new StreamReader(filePath))                        // Read CSV file


                {

                    reader.ReadLine();                                                          // Skip first line

                    while (!reader.EndOfStream)                                                 // while not end of file and file has opened
                    {

                        string line = reader.ReadLine();                                        //read next line
                        lineNumber++;
                        fields = line.Split(',');

                        if (LengthCheck() && DateCheck())                                         // Only going to this code if csv and date check are OK
                        {

                            string shipName = fields[8];
                            if (!ships.Contains(shipName))
                            {
                                ships.Add(shipName);
                            }
                        }
                    }



                    bool isValid = false;

                    do
                    {
                        Console.WriteLine("List of ships:");                                            // List ships + ask user to choose one
                        for (int i = 0; i < ships.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {ships[i]}");
                        }
                        SpaceLine();
                        Console.Write("Please enter which number ship you would like to choose: ");
                        shipChoice = Console.ReadLine();

                                                                                                        //checking for valid inputs
                        if (IsPresent(shipChoice, "menu choice") == true && IsWithInRangeInt(shipChoice, "Menu choice input", 1, ships.Count) == true)
                        {
                            isValid = true;

                        }

                        else
                            isValid = false;
                    }
                    while (isValid == false);


                    int number = int.Parse(shipChoice);

                    SpaceLine();
                    BreakerLine();
                    SpaceLine();


                    // Select the chosen ship's data
                    string chosenShip = ships[number - 1];

                    List<string[]> chosenShipData = new List<string[]>();

                    using (StreamReader reader1 = new StreamReader(filePath))
                    {
                        reader1.ReadLine();
                        while (!reader1.EndOfStream)
                        {
                            string line = reader1.ReadLine();
                            fields = line.Split(',');

                            if (fields[8] == chosenShip && LengthCheck() && DateCheck())
                            {
                                chosenShipData.Add(fields);
                            }
                        }
                    }


                    Console.WriteLine($"Ship Name: {chosenShip} : Departure Point: {chosenShipData[0][7]} : Arrival Date: {chosenShipData[0][9]} : Total Passenger Number: {chosenShipData.Count}");
                    BreakerLine();
                    SpaceLine();


                    foreach (string[] passengerData in chosenShipData)                                      // print pax
                    {
                        Console.WriteLine($"First Name: {passengerData[1]} \t Last Name: {passengerData[0]}");
                    }


                }
                MenuInput();                                                                // looping back to menu input
                lineNumber = 1;                                                             // reset line number count
            }
            catch (Exception MyError)
            {
                Console.WriteLine($"Error reading CSV file: {MyError.Message}");
                ExitProgram();
                Environment.Exit(1);


            }
            //**********************************************************************************************************************************//
            //        Had it working the below way first, but I couldn't get the data validation working so swapped to start on above way
            //**********************************************************************************************************************************//

            //try
            //{
            //    var lines = File.ReadAllLines(filePath);                                        // Read the file into memory


            //    fields = lines.Skip(1).ToArray();                                             // Skip the header row

            //                                                                         //            {
            //        var shipNames = fields.Select(line => line.Split(',')[8]).Distinct().ToArray();   // Get a list of ships


            //    Console.WriteLine("List of ships:");                                            // List ships + ask user to choose one
            //    for (int i = 0; i < shipNames.Length; i++)
            //    {
            //        Console.WriteLine($"{i + 1}: {shipNames[i]}");
            //    }
            //    SpaceLine();
            //    Console.Write("Please enter which number ship you would like to choose: ");
            //    int choice = int.Parse(Console.ReadLine()) - 1;
            //    SpaceLine();
            //    BreakerLine();
            //    SpaceLine();


            //    var selectedShipData = fields.Where(line => line.Split(',')[8] == shipNames[choice]).ToArray(); // Only pax from selected ship

            //    // display the ship details and passenger list
            //    DoubleBreakerLine();
            //    Console.WriteLine($"{shipNames[choice]} : Leaving from {selectedShipData[0].Split(',')[7]} Arrived : {selectedShipData[0].Split(',')[9]} with {selectedShipData.Length} passengers");
            //    DoubleBreakerLine();
            //    SpaceLine();

            //    foreach (var line in selectedShipData)                                                      // Print out every passenger
            //    {
            //        Console.WriteLine($"First Name {line.Split(',')[0]} : Last Name {line.Split(',')[1]}");
            //    }
            //    MenuInput();
            //}

            ////------------------------------------------------/// Catch Errors \\\----------------------------------------------------------\\

            //catch (Exception MyError)
            //{
            //    Console.WriteLine($"Error reading CSV file: {MyError.Message}");
            //    ExitProgram();
            //    Environment.Exit(1);


            //}



        }//******************************************************* END OF METHOD ***************************************************************//

        static void Menu2()                                                                     // Occupation Report Method//
        {
            //------------------------------------------------/// Code \\\--------------------------------------------------------------\\

            Dictionary<string, int> occupationCounts = new Dictionary<string, int>();           // Dictionary function to store occupation
            try
            {


                using (StreamReader reader = new StreamReader(filePath))                        // Read CSV file


                {

                    reader.ReadLine();                                                          // Skip first line

                    while (!reader.EndOfStream)                                                 // while not end of file and file has opened
                    {
                        string line = reader.ReadLine();                                        //read next line
                        lineNumber++;

                        fields = line.Split(',');                                               //split fields by comma

                        //-------------------------------/// Data Validation Checks \\\-----------------------------------------------------\\

                        //LengthCheck();                                                          // Check if field has 10 csv
                        //DateCheck();                                                            // Check date is correct US style

                        // Originally had the checks in here, but it was double printing the error line when I added it as below

                        //--------------------------------------/// Code #2 \\\-------------------------------------------------------------\\

                        if (LengthCheck() && DateCheck())                                         // Only going to this code if csv and date check are OK
                        {
                            string occupation = fields[4].Trim();                                   // Trim occupation from fifth column


                            if (occupationCounts.ContainsKey(occupation))                           // Add to occupation count for this occupation
                            {
                                occupationCounts[occupation]++;
                            }
                            else                                                                    // New occupation if hasn't been seen before
                            {
                                occupationCounts.Add(occupation, 1);
                            }
                        }


                    }
                }

                //------------------------------------------------/// Print Report \\\----------------------------------------------------------\\
                // Print the occupation counts to the console
                SpaceLine();
                Console.WriteLine("********************OCCUPATION REPORT********************");
                SpaceLine();
                foreach (KeyValuePair<string, int> kvp in occupationCounts)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
                MenuInput();                                                                                   // loop back to menu
                lineNumber = 1;                                                                                // reset line number count
            }
            //------------------------------------------------/// Catch Errors \\\----------------------------------------------------------\\

            catch (Exception MyError)
            {
                Console.WriteLine($"Error reading CSV file: {MyError.Message}");
                ExitProgram();
                Environment.Exit(1);

            }





            //******************************************************* END OF METHOD **********************************************************//

        }



        static void Menu3()                                                                     // Age Report Method
        {

            //------------------------------------------------/// Try \\\--------------------------------------------------------------\\
            try
            {
                //---------------------------------------------/// Variables \\\-----------------------------------------------------------\\
                int infants = 0;
                int children = 0;
                int teenage = 0;
                int youngAdult = 0;
                int adult = 0;
                int olderAdult = 0;
                int unknown = 0;

                //------------------------------------------------/// Code \\\--------------------------------------------------------------\\

                using (StreamReader reader = new StreamReader(filePath))                        // Read CSV file
                {
                    reader.ReadLine();                                                          // Skip first line

                    while (!reader.EndOfStream)                                                 // while not end of file
                    {
                        string line = reader.ReadLine();                                        //read next line
                        lineNumber++;


                        fields = line.Split(',');                                               // Split the line into columns

                        //-------------------------------/// Data Validation Checks \\\-----------------------------------------------------\\

                        //LengthCheck();                                                          // Check if field has 10 csv
                        //DateCheck();                                                            // Check date is correct US style

                        // Originally had the checks in here, but it was double printing the error line when I added it as below


                        //--------------------------------------/// Code #2 \\\--------------------------------------------------------------\\

                        if (LengthCheck() && DateCheck())                                           // Only going to this code if csv and date check are OK
                        {
                            string ageString = fields[2].Trim().ToLower();                          // Get age information from the third column

                            double age;

                            if (ageString.StartsWith("age "))
                            {
                                age = double.Parse(ageString.Substring(4));
                            }

                            else if (ageString.StartsWith("infant in months: "))
                            {
                                double months = double.Parse(ageString.Substring(18));
                                age = months / 12;                                                  // convert months to years
                            }

                            else                                                                     // Age is unknown
                            {

                                age = -1;
                            }

                            //--------------------------------------/// Break into age groups  \\\------------------------------------------------\\                                                                
                            if (age >= 0 && age < 1)
                            {
                                infants++;
                            }
                            else if (age >= 1 && age <= 12)
                            {
                                children++;
                            }
                            else if (age >= 13 && age <= 19)
                            {
                                teenage++;
                            }
                            else if (age >= 20 && age <= 29)
                            {
                                youngAdult++;
                            }
                            else if (age >= 30 && age <= 49)
                            {
                                adult++;
                            }
                            else if (age >= 50)
                            {
                                olderAdult++;
                            }

                            else if (age == -1)
                            { unknown++; }
                            else
                            {
                                unknown++;
                            }
                        }
                    }
                    //------------------------------------------------/// Print Age Report \\\----------------------------------------------\\
                    SpaceLine();
                    Console.WriteLine("********************AGE REPORT********************");
                    SpaceLine();
                    Console.WriteLine($"Infants (<1 year): {infants}");
                    Console.WriteLine($"Children (1 - 12): {children}");
                    Console.WriteLine($"Teenage (13 - 19): {teenage}");
                    Console.WriteLine($"Young adult (20 - 29): {youngAdult}");
                    Console.WriteLine($"Adult (30 - 49): {adult}");
                    Console.WriteLine($"Older adult (50+): {olderAdult}");
                    Console.WriteLine($"Unknown: {unknown}");

                    MenuInput();                                                                        // loop back to menu
                    lineNumber = 1;                                                                     // reset line number count
                }
            }
            //------------------------------------------------/// Catch Errors \\\----------------------------------------------------------\\
            catch (Exception MyError)
            {
                Console.WriteLine($"Error reading CSV file: {MyError.Message}");
                ExitProgram();
                Environment.Exit(1);
            }
            //******************************************************* END OF METHOD ********************************************************//
        }

        //---------------------------------------------/// Data Validation Methods \\\------------------------------------------------------\\
        static bool LengthCheck()
        {
            if (fields.Length != 10)
            {
                SpaceLine();
                Stars();
                SpaceLine();
                Console.WriteLine($"WARNING! Line {lineNumber} does not have 10 comma-separated values.");
                Console.WriteLine($"Line {lineNumber} information has not been added to report.");
                SpaceLine();
                Stars();
                SpaceLine();

                return false;                                                                         // Skip to next line
            }
            else
            { return true; }
        }


        static bool DateCheck()
        {
            string dateStr = fields[9];

            DateOnly date;


            if (DateOnly.TryParse(dateStr, CultureInfo.GetCultureInfo("us-EN"), DateTimeStyles.None, out date))
            {
                return true;
            }
            else
            {
                SpaceLine();
                Stars();
                SpaceLine();
                Console.WriteLine($"WARNING !! Invalid date format on line number {lineNumber}.");
                Console.WriteLine($"Line {lineNumber} information has not been added to report.");
                SpaceLine();
                Stars();
                SpaceLine();
                return false;
            }
        }


        static bool IsInteger(string textIn, string itemName)
        {

            bool isOK;
            int num;

            isOK = int.TryParse(textIn, out num);

            if (isOK == true)
                return true;
            else
            {
                SpaceLine();
                Stars();
                SpaceLine();
                Console.WriteLine($"WARNING! {itemName} must be of type integer and from selection of 1,2,3 or 4.");
                Console.WriteLine("Please enter again below.");
                SpaceLine();
                Stars();
                SpaceLine();
                return false;
            }

        }

        static bool IsPresent(string textIn, string itemName)
        {

            if (textIn != "")
                return true;
            else
            {
                SpaceLine();
                Stars();
                SpaceLine();
                Console.WriteLine($"WARNING! You must enter a value for {itemName}");
                Console.WriteLine("Please enter again below.");
                SpaceLine();
                Stars();
                SpaceLine();
                return false;
            }


        }


        static bool IsWithInRangeInt(string textIn, string itemName, int min, int max)
        {
            double num = double.Parse(textIn);

            if (num >= min && num <= max)
                return true;
            else
            {
                SpaceLine();
                Stars();
                SpaceLine();
                Console.WriteLine($"WARNING! {itemName} must be within the range {min} - {max}");
                Console.WriteLine("Please enter again below.");
                SpaceLine();
                Stars();
                SpaceLine();
                return false;
            }

        }


        //---------------------------------------------/// Formatting Methods \\\-----------------------------------------------------------\\
        static void BreakerLine()
        {
            Console.WriteLine("---------------------------------------------------------");
        }
        static void SpaceLine()
        {
            Console.WriteLine("");
        }

        static void DoubleBreakerLine()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------------");
        }

        static void Stars()
        {
            Console.WriteLine("***************************************************************");
        }


    }
}
/*#################################################################################################################################
|              /             /           ***                                       ***            \               \                |
|             /             /             ***                END                  ***              \               \               |
|            /             /               ***                                   ***                \               \              |
##################################################################################################################################*/