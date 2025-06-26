using System;
using GymPlusAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Spreadsheet> Spreadsheets { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<CustomMuscleGroup> CustomMuscleGroups { get; set; }
    public DbSet<RecurrentTraining> RecurrentTrainings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(u => u.Password)
                .IsRequired();
            
            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(u => u.Role)
                .IsRequired()
                .HasDefaultValue("User") // Default role
                .HasMaxLength(50);
            
            entity.HasMany(u => u.Spreadsheets)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
        });

        // Spreadsheet
        modelBuilder.Entity<Spreadsheet>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasMany(s => s.Workouts)
                .WithOne(w => w.Spreadsheet)
                .HasForeignKey(w => w.SpreadsheetId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            entity.Property(s => s.Description)
                .HasMaxLength(100);
            
            entity.Property(s => s.IsRecurring)
                .IsRequired();
            
            entity.Property(s => s.CreatedAt)
                .IsRequired()
                .HasColumnType("date");
            
            entity.Property(s => s.DaysOfWeek)
                .IsRequired();
        });

        // Workout
        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(w => w.Reps)
                .IsRequired();
            
            entity.Property(w => w.Series)
                .IsRequired();

            entity.Property(w => w.Weight)
                .IsRequired();

            // Restrição para impedir nome de treino duplicado dentro da mesma planilha
            entity.HasIndex(w => new { w.Name, w.SpreadsheetId })
                .IsUnique();
        });

        modelBuilder.Entity<CustomMuscleGroup>(entity =>
        {
            entity.HasKey(mg => mg.Id);

            entity.Property(mg => mg.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(mg => mg.BitValue)
                .IsRequired();
            
            entity.HasOne(mg => mg.User)
                .WithMany(u => u.CustomMuscleGroups)
                .HasForeignKey(mg => mg.UserId)
                .IsRequired();
            
            entity.HasOne(s => s.Spreadsheet)
                .WithMany(g => g.CustomMuscleGroups)
                .HasForeignKey(g => g.SpreadsheetId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); // Não remove grupos de musculos
        });

        modelBuilder.Entity<RecurrentTraining>(entity =>
        {
            entity.HasKey(rt => rt.Id);

            entity.Property(rt => rt.Date)
                .IsRequired()
                .HasColumnType("date");
            
            entity.HasIndex(rt => new { rt.Date, rt.SpreadsheetId })
                .IsUnique();

            entity.Property(rt => rt.IsCompleted);

            entity.Property(rt => rt.Observations);
            
            entity.HasOne(s => s.Spreadsheet)
                .WithMany(rt => rt.RecurrentTrainings)
                .HasForeignKey(tc => tc.SpreadsheetId)
                .IsRequired();
            
            entity.HasOne(u => u.User)
                .WithMany(rt => rt.RecurrentTrainings)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();
        });
    }
}
