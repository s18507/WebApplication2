using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Request;

namespace WebApplication2.Services
{
    public class SqlDbService :  IAnimalsDbService
    {

        private const string connection = "Data Source=db-mssql;" +
                                          "Initial Catalog=s18507;Integrated Security=True";
        public IEnumerable<Animal> GetAnimals(string sortBy)
        {
            string sortOrder = "ASC";
            if (sortBy == null)
            {
                sortBy = "AdmissionDate";
                sortOrder = "DESC";
            }

            if (sortBy.ToUpper().Contains("DESC") || sortBy.ToUpper().Contains("ASC"))
            {
                sortOrder = sortBy.Split(" ")[1];
                sortBy = sortBy.Split(" ")[0];
            }

            if (!sortBy.ToLower().Equals("idanimal")
                && !sortBy.ToLower().Equals("name")
                && !sortBy.ToLower().Equals("type")
                && !sortBy.ToLower().Equals("admissiondate")
                && !sortBy.ToLower().Equals("firstname")
                && !sortBy.ToLower().Equals("lastname"))
                return null;

            var _animals = new List<Animal>();
            using (var con = new SqlConnection(connection))
            using (var commands = new SqlCommand())
            {
                commands.Connection = con;
                commands.CommandText = "SELECT a.Name, a.Type, a.AdmissionDate, o.LastName FROM Animal a " +
                                             "INNER JOIN Owner o ON a.IdOwner = o.IdOwner " +
                                             "ORDER BY CASE @sortBy " +
                                                            "WHEN 'IdAnimal' THEN CAST([IdAnimal] AS VARCHAR(10)) " +
                                                            "WHEN 'Name' THEN [Name] " +
                                                            "WHEN 'Type' THEN [Type] " +
                                                            "WHEN 'AdmissionDate' THEN CONVERT(varchar, [AdmissionDate], 23) " +
                                                            "WHEN 'FirstName' THEN [FirstName] " +
                                                            "WHEN 'LastName' THEN [LastName]" +
                                                            "END " + sortOrder + ";";

                commands.Parameters.Add(new SqlParameter("sortBy", sortBy));
                con.Open();
                var dr = commands.ExecuteReader();

                while (dr.Read())
                {
                    _animals.Add(
                        new Animal
                        {
                            Name = dr["Name"].ToString(),
                            Type = dr["Type"].ToString(),
                            AdmissionDate = dr["AdmissionDate"].ToString(),
                            lastName = dr["lastName"].ToString()
                        });
                }
                dr.Close();
            }
            return _animals;
        }
    
        public AnimalRequest CreateAnimal(AnimalRequest request)
        {
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                com.Transaction = transaction;

                com.CommandText = "Insert Into Animal (Name, Type, AdmissionDate, IdOwner) Values (@Name, @Type, @AdmissionDate, @IdOwner)";
                com.Parameters.AddWithValue("Name", request.name);
                com.Parameters.AddWithValue("Type", request.type);
                com.Parameters.AddWithValue("AdmissionDate", request.admissionDate);
                com.Parameters.AddWithValue("IdOwner", request.idOwner);

                com.ExecuteNonQuery();
                transaction.Commit();
            }
            return request;
        }
    }
}

