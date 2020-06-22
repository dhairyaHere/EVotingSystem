namespace PollingSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ConductPoll : DbContext
    {
        // Your context has been configured to use a 'ConductPoll' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PollingSystem.Models.ConductPoll' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ConductPoll' 
        // connection string in the application configuration file.
        public ConductPoll()
            : base("name=ConductPoll")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConductPoll,PollingSystem.Migrations.Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Election> Elections { get; set; }
        public virtual DbSet<CastVote> CastVotes { get; set; }
        public virtual DbSet<VoterDetail> VoterDetails { get; set; }
        public virtual DbSet<Contestant> Contestants { get; set; }

    }

    
}