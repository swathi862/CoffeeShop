using CoffeeShop.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CoffeeShopTest
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;";
        public Coffee TestCoffee { get; set; }
        public DatabaseFixture()
        {
            Coffee newCoffee = new Coffee
            {
                Title = "Test Coffee",
                BeanType = "Espresso"
            };
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Coffee (Title, BeanType)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newCoffee.Title}', '{newCoffee.BeanType}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newCoffee.Id = newId;
                    TestCoffee = newCoffee;
                }
            }
        }
        public void Dispose()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM Coffee WHERE Title='Test Coffee'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}