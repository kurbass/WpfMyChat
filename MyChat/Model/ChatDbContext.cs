using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.Model
{
    public class ChatDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().
                AddJsonFile("conf.json").Build();

            optionsBuilder.UseSqlServer(configuration["ConnString"]);
        }

        public DbSet<User> Users { get; set; }
    }
}
