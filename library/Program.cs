//Project;Library manaement system
// Group 5
//authors:
//SAndesh Karki...984431
//Roshani Sunar...984431
//Prakriti Khadka..984247
//KanchanThapa...983728

//Latest update added on 10/05/2023 @7:30 A.M
//!!Anyone testing please refer to the readme file attached along the solution file for login details.!!

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml;

namespace Alib
{
    class Book  //Class for storing vairable used in array Book
    {
        public int code;
        public string name;
        public int value;
        public Book(int code, string name, int value)
        {
            this.code = code;
            this.name = name;
            this.value = value;

        }
    }

    class Login  //Class for storing variable used in array of Login
    {
        public int uid;
        public string password;
        public string Logname;

        public Login(int uid, string password, string Logname)
        {
            this.uid = uid;
            this.password = password;
            this.Logname = Logname;
        }
    } 

    class LendInfo  //Class for storing vairable ussed in array of Lendinf information
    {
        public int stcode;
        public int bkcode;

        public LendInfo(int stcode, int bkcode)
        {
            this.stcode = stcode;
            this.bkcode = bkcode;
        }
    }

    class Library //Class for main programm and methods
    {
        static Book[]? books; //declaration of array book
        static Login[]? logins; //declaration of array for login
        static LendInfo[]? lendinfo; //declaration of array for the lending information 
        
        static void Main(string[] args) //man execution of programme
        {
            
            LoadData(); // Load data from files

            if (books == null || logins == null || lendinfo == null)//Checks if the data is loaded correctly
            {
                Console.WriteLine(new string(' ', 10) + "Error loading data. Please check your data files.");//shows error if data is not correctly loaded into the programme
                Console.WriteLine(new string(' ', 10) + "Closing The Programme.........");
                Environment.Exit(0);//exist the console
                return;
            }

            int InId; //integer variable set to record the user input as userid
            string InPW; //string variable set to record the input as the password 
            Console.WriteLine(new string(' ', 10) + "Welcome To the Library");//shows to the user when data is loaded sucessfully 
            Console.WriteLine(new string(' ', 10) + "===============================================================================");

            while (true)//enter the loop for login 
            {
                Console.WriteLine(new string(' ', 10) + "Enter Your library card number: ");//programme accept the user id fro user
                InId = Convert.ToInt32(Console.ReadLine());//programme saves the user id to the int variable

                Console.WriteLine(new string(' ', 10) + "Enter Your Password: ");//program accept the password from the user
                InPW = Console.ReadLine();//pprogram saves the password into the string variable

                // Implement login logic here
                int authRes=Auth(InId, InPW);
                if (authRes ==1)
                {
                    Console.WriteLine(new string(' ', 20) + "Welcome, Admin! ");
                    Console.WriteLine(new string(' ', 10) + "===============================================================================");
                    Admin();
                    LogOut();
                }
                
                else if (authRes==0)
                {
                    Student(InId);
                    LogOut();
                }
                else
                {
                    Console.WriteLine(new string(' ', 20) + "Invalid credentials. Please try again.");//error to the unsucessfull login attempt
                    Console.WriteLine(new string(' ', 10) + "===============================================================================");
                }
                LogOut();
                
            }
        }
        public static void LoadData() //method for loading data from the text file into the programme'S ARRAY
        {
                string[] bookLines = File.ReadAllLines("text\\book.txt");  //program readfile from the subdirectory located with the peograam exe file
                books = new Book[bookLines.Length]; 
                for (int i = 0; i < bookLines.Length; i++)  //populating book
            {
                    string[] fields = bookLines[i].Split(',');
                    int code = Convert.ToInt32(fields[0]);
                    string name = fields[1];
                    int value = Convert.ToInt32(fields[2]);
                  
                    books[i] = new Book(code, name, value);
                }

                string[] loginLines = File.ReadAllLines("text\\logincred.txt");  //program readfile from the subdirectory located with the peograam exe file
            logins = new Login[loginLines.Length];
                for (int i = 0; i < loginLines.Length; i++) //populating logininfo from text file
                {
                    string[] fields = loginLines[i].Split(",");
                    int uid = Convert.ToInt32(fields[0]);
                    string password = fields[1];
                    string Logname = fields[2];
                    logins[i] = new Login(uid, password, Logname);

                }

                string[] lendinfolines = File.ReadAllLines("text\\lend.txt");  //program readfile from the subdirectory located with the peograam exe file
            lendinfo = new LendInfo[lendinfolines.Length];  //popultaing lending information from text file
                for (int i = 0; i < lendinfolines.Length; i++)
                {
                    string[] fields = lendinfolines[i].Split(",");
                    int stcode = Convert.ToInt32(fields[0]);
                    int bkcode = Convert.ToInt32(fields[1]);
                    lendinfo[i] = new LendInfo(stcode, bkcode);

                }
        }

        public static void SaveData()  //method to save all the changes of data into the txt file
        {
          
            
            using (StreamWriter sw = new StreamWriter("text//book.txt"))  //program overite the text document to load book data from the programme
            {
                foreach (Book book in books)
                {
                    sw.WriteLine($"{book.code},{book.name},{book.value}");
                }
            }

            using (StreamWriter sw = new StreamWriter("text//logincred.txt"))  //program overwrite the text document to load login data from the programme
            {
                foreach (Login login in logins)
                {
                    sw.WriteLine($"{login.uid},{login.password},{login.Logname}");
                }
            }

            using (StreamWriter sw = new StreamWriter("text//lend.txt")) //program oveerwrite the text document to load the leding data from the programe
            {
                foreach (LendInfo lendInfo in lendinfo)
                {
                    sw.WriteLine($"{lendInfo.stcode},{lendInfo.bkcode}");
                }
            }
        }

        public static void ShowBook() //method to show all the available book to the user
        {
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 20) + "{0,-10} {1,-50} {2,-50}", "Book-Code", "Book-Name", "Availability");

            foreach (Book book in books)
            {
                string availability = (book.value == 0) ? "Not Available" : "Available";
                Console.WriteLine(new string(' ', 20) + "{0,-10} {1,-50} {2,-50}", book.code, book.name, availability);
            }

            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
        }

        public static int Auth(int userId, string password) //method for login authentication logic
        {
            int adminId = 101;
            string adminpass = "admin2023";

            if (userId ==adminId && password==adminpass) //condititon for login to be by admin
            {
                return 1; //return value for admin login
            }
            foreach (Login login in logins)
            {
                if (login != null && login.uid == userId && login.password == password) //logic for sucessful login
                {
                    Delay();
                    Console.WriteLine(new string(' ', 10) + $"Welcome, {login.Logname}!");
                    return 0;
                }
            }

            Console.WriteLine(new string(' ', 10) + "Invalid credentials. Please try again."); //logic for unsucessfulll login attempt
            return -1;
        }

        public static void Student(int inId)
        {
            int choice;
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 20) + "Press '1' to lend the book ");
            Console.WriteLine(new string(' ', 20) + "Press '2' to return the book");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            while (true)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to lending portal.....");
                            Thread.Sleep(1000);
                            Booklend();

                            break;
                        case 2:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to return portal.....");
                            Thread.Sleep(1000);
                            BookReturn(inId);

                            break;
                        default:
                            Console.WriteLine(new string(' ', 10) + "Invalid choice. Please press 1 or 2.");
                            Thread.Sleep(1000);
                            break;
                    }
                    break; // Exit the loop after a valid choice is made
                }
                else
                {
                    Console.WriteLine(new string(' ', 10) + "Invalid input. Please enter a number.");
                }
            }
        }
        public static void LendDetail()
        {
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            Console.WriteLine(new string(' ', 10) + "{0,-15} {1,-20}", "Student-Code", "Book-Code");

            foreach (LendInfo lendInfo in lendinfo)
            {
                Console.WriteLine(new string(' ', 10) + "{0,-15} {1,-20}", lendInfo.stcode, lendInfo.bkcode);
            }

            Console.WriteLine(new string(' ', 10) + "===============================================================================");
        } 
        public static void BookReturn(int inId) //method for user to return the borrowed book
        {
            Delay();
            Console.WriteLine(new string(' ' ,20)+"-------Book Return------");
            int returnChoice;

            ShowBook(); //shows availability of book to the student 

            Console.WriteLine(new string(' ', 10) + "Enter the book code of the book you want to return");

            try //for detection of any incoreect formating entrty by user
            {
                returnChoice = Convert.ToInt32(Console.ReadLine());

                bool bookFound = false;

                foreach (Book book in books) //viewing in book's array
                {
                    if (book.code == returnChoice) //logic to find the correct book on book's array
                    {
                        if (book.value >= 0) 
                        {
                            bool borrowed = false;
                            foreach (LendInfo lendInfo in lendinfo) //searching through the list of Lending info recorded
                            {
                                if (lendInfo.stcode == inId && lendInfo.bkcode == returnChoice) //argument for finding the lending history
                                {
                                    borrowed = true;
                                    break;
                                }
                            }

                            if (borrowed) //if condition when the book is borroweed
                            {
                                Console.Clear();
                                Console.WriteLine(new string(' ', 10) + "===============================================================================");
                                Console.WriteLine(new string(' ', 20) + "Requested book found. Processing the return.");
                                book.value = book.value + 1; //adding the quantitiy of book in the inventory after login
                                Console.WriteLine(new string(' ', 20) + "You have successfully returned the {0}", book.name);
                                Console.WriteLine(new string(' ', 10) + "===============================================================================");

                                // Remove lending information from array that keeps lennding information
                                int indexToRemove = -1;

                                for (int i = 0; i < lendinfo.Length; i++)
                                {
                                    if (lendinfo[i].stcode == inId && lendinfo[i].bkcode == returnChoice)  //searching for the required lending information in the arrray of lendinfo
                                    {
                                        indexToRemove = i;
                                        break;
                                    }
                                }

                                if (indexToRemove >= 0) //checkinf if the lending info still exist in the array if nor code block is skkiped
                                {
                                    for (int i = indexToRemove; i < lendinfo.Length - 1; i++)
                                    {
                                        lendinfo[i] = lendinfo[i + 1];
                                    }
                                    Array.Resize(ref lendinfo, lendinfo.Length - 1); 

                                    SaveData();
                                }
                                else //alternative response if the value of book couldn't be added on 
                                {
                                    Console.WriteLine(new string(' ', 10) + "Error processing return. Please try again.");
                                }
                            }
                            else //response if there is no prior lending detected 
                            {
                                Console.WriteLine(new string(' ', 10) + "Error: You have not borrowed this book.");
                            }
                        }

                        bookFound = true;
                        break; // Exit the loop after finding the book
                    }
                }

                if (!bookFound)
                {
                    Console.WriteLine(new string(' ', 10) + "Book Not found. Please try again.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(new string(' ', 10) + "Invalid input. Please enter a valid book code.");
            }

            LogOut(); 
        }
        public static void Booklend() //method for lending the book 
        {
            Delay(); //adding a pre defined delay method 
           
            Console.WriteLine(new string(' ', 50) + "-------Book Lending------");
            int lendChoice;
                ShowBook();
                Console.WriteLine(new string(' ', 10) + "Enter the book code of the book you want to lend");

                try
                {
                    lendChoice = Convert.ToInt32(Console.ReadLine());

                    bool bookFound = false;

                    foreach (Book book in books) 
                    {
                        if (book.code == lendChoice)
                        {
                            if (book.value > 0)
                            {
                                Console.WriteLine(new string(' ', 10) + "Requested book found. Processing the lending.");
                                book.value = book.value - 1;
                                Console.WriteLine(new string(' ', 10) + "You have successfully lended the {0}", book.name);
                                SaveData();
                                Console.ReadLine();
                            }
                            else if(book.value <= 0)
                            {
                                Console.WriteLine(new string(' ', 10) + "Sorry, this book is currently not available for lending.");
                            }

                            bookFound = true;
                            break; // Exit the loop after finding the book
                        }
                    }

                    if (!bookFound)
                    {
                        Console.WriteLine(new string(' ', 10) + "Book Not found. Please try again.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(new string(' ', 10) + "Invalid input. Please enter a valid book code.");
                }
        }
        public static void Admin() //method for admin portal 
        {
            Delay();
            Console.WriteLine(new string(' ', 20) + "Admin Pannel");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");
            int choice;
            Console.WriteLine(new string(' ', 20) + "Choose any options: ");
            Console.WriteLine(new string(' ', 20) + "Press 1 for Add student");
            Console.WriteLine(new string(' ', 20) + "Press 2 to Remove student");
            Console.WriteLine(new string(' ', 20) + "Press 3 for Add book");
            Console.WriteLine(new string(' ', 20) + "Press 4 for Remove book");
            Console.WriteLine(new string(' ', 20) + "Press 5 to see lending detail");
            Console.WriteLine(new string(' ', 20) + "Press 6 to see Book Inventory");
            Console.WriteLine(new string(' ', 10) + "===============================================================================");


            while (true)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out choice))
                {
                    switch (choice) //swtich case for admin choice
                    {
                        case 1: 
                            Console.WriteLine(new string(' ', 10) + "Redirecting to Add student portal.....");
                            Thread.Sleep(1000);
                            Delay();
                            AddStudent(); //calling method for adding student
                            break;
                        case 2:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to Remove student portal.....");
                            Thread.Sleep(1000);
                            Delay();
                            RemoveStudent(); //calling method to remove student 
                            break;
                        case 3:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to Add book portal.....");
                            Thread.Sleep(1000);
                            Delay();
                            AddBook(); //calling method to add a book 
                            break;
                        case 4:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to Remove book portal.....");
                            Thread.Sleep(1000);
                            Delay();
                            RemoveBook(); //calling method to remove a book 
                            break;
                        case 5:
                            Console.WriteLine(new string(' ', 10) + "Redirecting to Lending details.....");
                            Thread.Sleep(1000);
                            Delay();
                            LendDetail(); //calling method to view lending info 
                            break;
                        case 6:
                            //switch to see inventory 
                            Console.WriteLine(new string(' ', 10) + "Redirecting to inventory portal.....");
                            Thread.Sleep(1000);
                            Delay();
                            
                            Console.WriteLine(new string(' ', 10) + "===============================================================================");
                            Console.WriteLine(new string(' ', 20) + "{0,-10} {1,-50} {2,-50}", "Book-Code", "Book-Name", "Quantity");

                            foreach (Book book in books)
                            {
                                
                                Console.WriteLine(new string(' ', 20) + "{0,-10} {1,-50} {2,-50}", book.code, book.name, book.value);
                            }

                            Console.WriteLine(new string(' ', 10) + "===============================================================================");
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                            break;

                        default:
                            Console.WriteLine(new string(' ', 10) + "Invalid choice. Please press a valid number.");
                            break;
                    }
                    break; // Exit the loop after a valid choice is made
                }
                else
                {
                    Console.WriteLine(new string(' ', 10) + "Invalid input. Please enter a number.");
                }
            }
        }
        private static void RemoveBook() //method to remove a book 
        {
            Delay();
            Console.WriteLine(new string(' ', 10) + "Enter Book Code of the book to be removed: ");
            int bookCodeToRemove = Convert.ToInt32(Console.ReadLine());

            int indexToRemove = -1;

            for (int i = 0; i < books.Length; i++)
            {
                if (books[i].code == bookCodeToRemove) 
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                // Remove book from books array
                for (int i = indexToRemove; i < books.Length - 1; i++)
                {
                    books[i] = books[i + 1];
                }
                Array.Resize(ref books, books.Length - 1);

                Console.WriteLine(new string(' ', 10) + "{0}Book removed successfully!", bookCodeToRemove);
                SaveData(); //saving data after change in array 
            }
            else
            {
                Console.WriteLine(new string(' ', 10) + "Book not found.");
            }
        }
        private static void AddBook()   //method to add book
        {
            Delay();
            Console.WriteLine(new string(' ', 10) + "Enter Book Code for the new book: ");
            int newBookCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(new string(' ', 10) + "Enter Name for the new book: ");
            string newBookName = Console.ReadLine();

            Console.WriteLine("Enter Value for the new book: ");
            int newBookValue = Convert.ToInt32(Console.ReadLine());

            // Add new book to books array
            Array.Resize(ref books, books.Length + 1);
            books[books.Length - 1] = new Book(newBookCode, newBookName, newBookValue);

            Console.WriteLine(new string(' ', 10) + "New book added successfully!");
            SaveData(); //saving data after change made in array 
        }
        private static void RemoveStudent() //method to remove student 
        {
            Delay();
            Console.WriteLine(new string(' ', 10) + "Enter User ID of the student to be removed: ");
            int userIdToRemove = Convert.ToInt32(Console.ReadLine());

            int indexToRemove = -1;

            for (int i = 0; i < logins.Length; i++)
            {
                if (logins[i].uid == userIdToRemove)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                // Remove student from logins array
                for (int i = indexToRemove; i < logins.Length - 1; i++)
                {
                    logins[i] = logins[i + 1];
                }
                Array.Resize(ref logins, logins.Length - 1);

                Console.WriteLine(new string(' ', 10) + "Student removed successfully!");
                SaveData(); //saving data after an change made in array
            }
            else
            {
                Console.WriteLine(new string(' ', 10) + "Student not found.");
            }
        }
        private static void AddStudent()  // method to add new student 
        {
            Delay();
            Console.WriteLine(new string(' ', 10) + "Enter User ID for the new student: ");
            int newUserId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(new string(' ', 10) + "Enter Password for the new student: ");
            string newPassword = Console.ReadLine();

            Console.WriteLine(new string(' ', 10) + "Enter Name for the new student: ");
            string newUserName = Console.ReadLine();

            // Add new student to logins array
            Array.Resize(ref logins, logins.Length + 1);
            logins[logins.Length - 1] = new Login(newUserId, newPassword, newUserName);

            Console.WriteLine(new string(' ', 10) + "New student with following detail added successfully!");
            Console.WriteLine(new string(' ', 10) + "library Id: {0}  Password: {1} UserName: {2}",newUserId, newPassword, newUserName);
            SaveData();
        }
        public static void LogOut() //method for insitalizing logout from session 
        {
            while (true)
            {
                Console.WriteLine(new string(' ', 10) + "Do You wish to End the session..?");
                Console.WriteLine(new string(' ', 10) + "Press 'Y' for yes and 'N' for no ");
                string LogoutChoice = Console.ReadLine();

                if (LogoutChoice== "y"||LogoutChoice=="Y") //statement for positive input 
                {
                    Console.WriteLine(new string(' ', 10) + "You chose to exit the program ");
                    Console.WriteLine(new string(' ', 10) + "Closing the Programme");
                    Environment.Exit(0); //exit from console
                }
                else if (LogoutChoice== "n"||LogoutChoice=="N") //statement for negative input
                {
                    Console.WriteLine(new string(' ', 10) + "You have chosen not to exit");
                    Console.WriteLine(new string(' ', 10) + "Redirecting you to the Login Page");
                    Thread.Sleep(1000);
                    Delay();
                    return;
                }
                else
                {
                    Console.WriteLine(new string(' ', 10) + "Invalid input. Please enter 'Y' or 'N'.");
                }
            }
        }
        public static void Delay() //method to add a 1 second delay for ux and clear console for clarity
        {
            Console.Clear(); //command clear the console
            Console.WriteLine(new string(' ', 10) + "-----------------------------------------------------------------");
            Console.WriteLine(new string(' ', 30) + "Loading...............!!");
            Console.WriteLine(new string(' ', 10) + "-----------------------------------------------------------------");
            Thread.Sleep(1000);
            Console.Clear() ;
        }
    }
}