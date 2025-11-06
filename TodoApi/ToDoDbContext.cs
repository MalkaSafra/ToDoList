using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsComplete).HasColumnName("isComplete");
            entity.Property(e => e.NameI)
                .HasMaxLength(100)
                .HasColumnName("Name_i");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace TodoApi;

//public partial class ToDoDbContext : DbContext
//{
//    public ToDoDbContext()
//    {
//    }

//    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Item> Items { get; set; }

//    // הסרתי את OnConfiguring כי מגדירים את החיבור ב-Program.cs
//    // אם יש OnConfiguring, הוא עוקף את ההגדרה מ-Program.cs

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder
//            .UseCollation("utf8mb4_0900_ai_ci")
//            .HasCharSet("utf8mb4");

//        modelBuilder.Entity<Item>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PRIMARY");

//            entity.ToTable("items");

//            entity.Property(e => e.Id)
//                .HasColumnName("ID")
//                .ValueGeneratedOnAdd(); // חשוב! אומר ל-EF שה-ID נוצר אוטומטית

//            entity.Property(e => e.IsComplete)
//                .HasColumnName("isComplete")
//                .HasDefaultValue(false); // ברירת מחדל במסד נתונים

//            entity.Property(e => e.NameI)
//                .HasMaxLength(100)
//                .HasColumnName("Name_i");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}