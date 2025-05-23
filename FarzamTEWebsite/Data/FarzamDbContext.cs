﻿using FarzamTEWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FarzamTEWebsite.Data
{
    public class FarzamDbContext : DbContext
    {
        public FarzamDbContext(DbContextOptions<FarzamDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HappyCall> HappyCalls { get; set; }
        public DbSet<InComingCall> InComingCalls { get; set; }
        public DbSet<Brokerage> Brokerages { get; set; }
        public DbSet<Transaction_Statistics_M> Transaction_Statistics_M { get; set; }
        public DbSet<CaseReport> CaseReports { get; set; }
        public DbSet<TransportToSmart> TransportsToSmart { get; set; }
        public DbSet<TTS_Reason> TTS_Reasons { get; set; }
        public DbSet<Notice_Call> Notice_Call { get; set; }
        public DbSet<Notice_SMS> Notice_SMS { get; set; }
        public DbSet<InComingCall_Stat> inComingCall_Stats { get; set; }
    }
}
