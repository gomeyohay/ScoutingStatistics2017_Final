﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApplication5
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ScoutingPropsEntities : DbContext
    {
        public ScoutingPropsEntities()
            : base("name=ScoutingPropsEntities")
        {
        }

        public ScoutingPropsEntities(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AllianceScouting> AllianceScoutings { get; set; }
        public virtual DbSet<GameScouting> GameScoutings { get; set; }
        public virtual DbSet<PhoneList> PhoneLists { get; set; }
        public virtual DbSet<PitScouting> PitScoutings { get; set; }
        public virtual DbSet<PitScoutingProps> PitScoutingProps { get; set; }
        public virtual DbSet<regional_table> regional_table { get; set; }
        public virtual DbSet<TeamScouting> TeamScoutings { get; set; }
        public virtual DbSet<TeamScoutingProps> TeamScoutingProps { get; set; }
    }
}