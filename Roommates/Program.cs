using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        try
                        {
                            Room room = roomRepo.GetById(id);
                            Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("search for chore"):
                        Console.WriteLine("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        try
                        {
                            Chore chore = choreRepo.GetById(choreId);
                            Console.WriteLine($"{chore.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("search for roommate"):
                        Console.WriteLine("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        try
                        {
                            Roommate roommate = roommateRepo.GetById(roommateId);
                            Console.WriteLine($"{roommate.FirstName} - {roommate.RentPortion} - {roommate.Room.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.InnerException);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("get unassigned chores"):
                        List<Chore> unnasignedChores = new List<Chore>();
                        unnasignedChores = choreRepo.GetUnassignedChores();
                        foreach (var c in unnasignedChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("assign chore"):
                        List<Chore> unnasignedChoresDisplay = null;
                        unnasignedChoresDisplay = choreRepo.GetUnassignedChores();
                        foreach (var c in unnasignedChoresDisplay)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Chore Id: ");
                        int assignChoreId = int.Parse(Console.ReadLine());
                        
                        List<Roommate> eligibleRoommates = new List<Roommate>();
                        eligibleRoommates = roommateRepo.GetAll();
                        foreach (var r in eligibleRoommates)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName} - {r.Room.Name}");
                        }
                        Console.WriteLine("Roommate Id: ");
                        int assignRoommateId = int.Parse(Console.ReadLine());

                        int insertedRow = choreRepo.AssignChore(assignChoreId, assignRoommateId);
                        Console.WriteLine($"Row {insertedRow} has been inserted into RoommateChore Table.");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (var r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.WriteLine("Which room would you like to update?");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("delete a room"):
                        List<Room> listOfRooms = roomRepo.GetAll();
                        foreach (var r in listOfRooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.WriteLine("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        roomRepo.Delete(roomId);
                        Console.WriteLine("Room has successfully been deleted.");
                        Console.WriteLine("Press any button to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "search for chore",
                "search for roommate",
                "get unassigned chores",
                "assign chore",
                "update a room",
                "delete a room",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}