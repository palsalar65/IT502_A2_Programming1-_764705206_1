/* 
 * Project Name: LANGHAM Hotel Management System
 * Author Name:  Kushal Palsalar
 * Date: 21-07-2024
 * Application Purpose: features of C# that are implemented on the Hotel Management Application System for the client “LANGHAM Hotels”
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assessment2Task2
{
    // Custom Class - Room
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    // Custom Class - Customer
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string Name { get; set; }
    }

    // Custom Class - RoomAllocation
    public class RoomlAllocaltion
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    // Custom Main Class - Program
    class Program
    {
        // Variables declaration and initialization
        public static Room[] listofRooms = new Room[0];
        public static int[] listOfRoomlAllocaltions = new int[0];
        public static string filePath;
        public static int[] listOfCustomerNumbers = new int[0];
        public static string[] listOfNames = new string[0];

        // Main function
        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, "HotelManagement.txt");

            char ans = 'Y';

            do
            {
                Console.Clear();
                DisplayMenu();

                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddRooms();
                        break;
                    case 2:
                        DisplayRooms();
                        break;
                    case 3:
                        AllocateRoomToCustomer();
                        break;
                    case 4:
                        DeallocateRoom();
                        break;
                    case 5:
                        DisplayRoomAllocations();
                        break;
                    case 6:
                        DisplayBillingMessage();
                        break;
                    case 7:
                        SaveRoomAllocationsToFile();
                        break;
                    case 8:
                        ShowRoomAllocationsFromFile();
                        break;
                    case 9:
                        Console.WriteLine("Exiting application...\nThank You!!");
                        break;
                    case 0:
                        BackupRoomAllocationFile();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.Write("\nWould You Like To Continue(Y/N):");
                ans = Convert.ToChar(Console.ReadLine());
            } while (ans == 'y' || ans == 'Y');
        }

        static void DisplayMenu()
        {
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("                 LANGHAM HOTEL MANAGEMENT SYSTEM                  ");
            Console.WriteLine("                            MENU                                 ");
            Console.WriteLine("***********************************************************************************");
            Console.WriteLine("1. Add Rooms");
            Console.WriteLine("2. Display Rooms");
            Console.WriteLine("3. Allocate Rooms");
            Console.WriteLine("4. De-Allocate Rooms");
            Console.WriteLine("5. Display Room Allocation Details");
            Console.WriteLine("6. Billing");
            Console.WriteLine("7. Save the Room Allocations To a File");
            Console.WriteLine("8. Show the Room Allocations From a File");
            Console.WriteLine("9. Exit");
            Console.WriteLine("0. Backup Room Allocation File");
            Console.WriteLine("***********************************************************************************");
            Console.Write("Enter Your Choice Number Here:");
        }

        static void AddRooms()
        {
            try
            {
                Console.Write("Please Enter the Total Number of Rooms in the Hotel: ");
                int totalRooms = Convert.ToInt32(Console.ReadLine());

                Room[] newRooms = new Room[listofRooms.Length + totalRooms];
                Array.Copy(listofRooms, newRooms, listofRooms.Length);

                for (int i = listofRooms.Length; i < newRooms.Length; i++)
                {
                    Console.Write("Please enter the Room Number: ");
                    int roomNo = Convert.ToInt32(Console.ReadLine());
                    newRooms[i] = new Room { RoomNo = roomNo };
                }

                listofRooms = newRooms;
                Console.WriteLine("Rooms added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Unhandled Exception: System.FormatException: Input string was not in a correct format.");
            }
        }

        static void DisplayRooms()
        {
            if (listofRooms.Length == 0)
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            Console.WriteLine("List of Rooms:");
            foreach (var room in listofRooms)
            {
                Console.WriteLine($"Room Number: {room.RoomNo}, Is Allocated: {room.IsAllocated}");
            }
        }

        static void AllocateRoomToCustomer()
        {
            try
            {
                Console.Write("Please Enter the Room Number to Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());

                var room = listofRooms.FirstOrDefault(r => r.RoomNo == roomNo);
                if (room == null || room.IsAllocated)
                {
                    throw new InvalidOperationException("Room is either not available or already allocated.");
                }

                Console.Write("Please Enter the Customer Number: ");
                int customerNo = Convert.ToInt32(Console.ReadLine());
                Console.Write("Please Enter the Customer Name: ");
                string Name = Console.ReadLine();

                Array.Resize(ref listOfRoomlAllocaltions, listOfRoomlAllocaltions.Length + 1);
                Array.Resize(ref listOfCustomerNumbers, listOfCustomerNumbers.Length + 1);
                Array.Resize(ref listOfNames, listOfNames.Length + 1);

                listOfRoomlAllocaltions[listOfRoomlAllocaltions.Length - 1] = roomNo;
                listOfCustomerNumbers[listOfCustomerNumbers.Length - 1] = customerNo;
                listOfNames[listOfNames.Length - 1] = Name;

                room.IsAllocated = true;
                Console.WriteLine("Room allocated successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Unhandled Exception: System.FormatException: Input string was not in a correct format.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Unhandled Exception: System.InvalidOperationException: Sequence contains no matching element.");
            }
        }

        static void DeallocateRoom()
        {
            try
            {
                Console.Write("Please Enter the Room Number to De-Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());

                int index = Array.FindIndex(listOfRoomlAllocaltions, ra => ra == roomNo);
                if (index == -1)
                {
                    throw new InvalidOperationException("Room is not allocated.");
                }

                listOfRoomlAllocaltions = listOfRoomlAllocaltions.Where((val, idx) => idx != index).ToArray();
                listOfCustomerNumbers = listOfCustomerNumbers.Where((val, idx) => idx != index).ToArray();
                listOfNames = listOfNames.Where((val, idx) => idx != index).ToArray();

                var room = listofRooms.FirstOrDefault(r => r.RoomNo == roomNo);
                room.IsAllocated = false;

                Console.WriteLine("Room de-allocated successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Unhandled Exception: System.FormatException: Input string was not in a correct format.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Unhandled Exception: System.InvalidOperationException: Sequence contains no matching element.");
            }
        }

        static void DisplayRoomAllocations()
        {
            if (listOfRoomlAllocaltions.Length == 0)
            {
                Console.WriteLine("No room allocations available.");
                return;
            }

            Console.WriteLine("Room Allocations:");
            for (int i = 0; i < listOfRoomlAllocaltions.Length; i++)
            {
                Console.WriteLine($"Room Number: {listOfRoomlAllocaltions[i]}, Customer Name: {listOfNames[i]}");
            }
        }

        static void DisplayBillingMessage()
        {
            Console.WriteLine("Billing Feature is Under Construction and will be added soon…!!!");
        }

        static void SaveRoomAllocationsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Room Allocations as of " + DateTime.Now);
                    for (int i = 0; i < listOfRoomlAllocaltions.Length; i++)
                    {
                        writer.WriteLine($"Room Number: {listOfRoomlAllocaltions[i]}, Customer Name: {listOfNames[i]}");
                    }
                }

                Console.WriteLine("Room allocations saved to file successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Unhandled Exception: System.ArgumentException: Stream was not writable.");
            }
        }

        static void ShowRoomAllocationsFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Unhandled Exception: System.FileNotFoundException: Could not find file.");
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine("Room Allocations from File:");
                    Console.WriteLine(content);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Unhandled Exception: System.FileNotFoundException: Could not find file.");
            }
        }

        static void BackupRoomAllocationFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("\"Unhandled Exception: System.FileNotFoundException: Could not find file.");
                }

                string backupFilePath = Path.Combine(Path.GetDirectoryName(filePath), "lhms_studentid_backup.txt");

                File.Copy(filePath, backupFilePath, true);

                Console.WriteLine("Room allocation file backed up successfully.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Unhandled Exception: System.FileNotFoundException: Could not find file.");
            }
        }
    }
}
