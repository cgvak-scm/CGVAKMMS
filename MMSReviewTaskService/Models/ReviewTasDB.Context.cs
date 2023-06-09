﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMSService.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ReviewTaskEntities : DbContext
    {
        public ReviewTaskEntities()
            : base("name=ReviewTaskEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Employee_Master> Employee_Master { get; set; }
        public DbSet<MMS_Meeting_Category> MMS_Meeting_Category { get; set; }
        public DbSet<MMS_Meeting_Details> MMS_Meeting_Details { get; set; }
        public DbSet<MMS_Meeting_Master> MMS_Meeting_Master { get; set; }
        public DbSet<MMS_Review_Frequency> MMS_Review_Frequency { get; set; }
        public DbSet<MMS_Track_ReviewTasks> MMS_Track_ReviewTasks { get; set; }
    
        public virtual ObjectResult<Proc_Get_Review_Tasks_Result> Proc_Get_Review_Tasks(Nullable<int> chairperson, Nullable<int> category_id, Nullable<int> participant, string priorityType)
        {
            var chairpersonParameter = chairperson.HasValue ?
                new ObjectParameter("Chairperson", chairperson) :
                new ObjectParameter("Chairperson", typeof(int));
    
            var category_idParameter = category_id.HasValue ?
                new ObjectParameter("Category_id", category_id) :
                new ObjectParameter("Category_id", typeof(int));
    
            var participantParameter = participant.HasValue ?
                new ObjectParameter("Participant", participant) :
                new ObjectParameter("Participant", typeof(int));
    
            var priorityTypeParameter = priorityType != null ?
                new ObjectParameter("PriorityType", priorityType) :
                new ObjectParameter("PriorityType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_Get_Review_Tasks_Result>("Proc_Get_Review_Tasks", chairpersonParameter, category_idParameter, participantParameter, priorityTypeParameter);
        }
    }
}
