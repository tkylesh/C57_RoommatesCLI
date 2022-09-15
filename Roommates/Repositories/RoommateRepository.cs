using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    internal class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString)
        {
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.FirstName, rm.RentPortion,r.Id as 'RoomId', r.MaxOccupancy as 'MaxOccupancy', r.Name as 'RoomName'
                                        FROM Roommate rm
                                        JOIN Room r ON r.Id = rm.RoomId
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        Roommate roommate = null;
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    Name = reader.GetString(reader.GetOrdinal("RoomName")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                                }
                            };
                        }
                        return roommate;
                    }

                }

            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * 
                                        FROM Roommate rm
                                        Join
                                        Room r ON r.Id = rm.RoomId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> allRoommates = new List<Roommate>();
                        Roommate roommate = null;
                        while (reader.Read())
                        {
                            roommate = new Roommate()
                            {

                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                                }
                            };

                            allRoommates.Add(roommate);
                        }
                        return allRoommates;
                    }

                }
            }
        }

        
            }
        }
    }
}

