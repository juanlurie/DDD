﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Iris.EntityFramework.KeyValueStore
{
    public class KeyValueEntityConfiguration : EntityTypeConfiguration<KeyValueEntity>
    {
        public KeyValueEntityConfiguration()
        {
            ToTable("KeyValueStore");
            HasKey(entity => entity.Hash);
            
            Property(entity => entity.Hash)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasMaxLength(40);
            
            Property(entity => entity.Key)
                .IsRequired()
                .HasMaxLength(1000);
            
            Property(entity => entity.Value)
                .IsRequired();
            
            this.HasTimestamp(entity => entity.TimeStamp);
        }
    }
}
