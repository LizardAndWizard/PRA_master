using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace app.Models;

public partial class PraDrivingSchoolContext : DbContext
{
    public PraDrivingSchoolContext()
    {
    }

    public PraDrivingSchoolContext(DbContextOptions<PraDrivingSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Colour> Colours { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Rezervation> Rezervations { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:AppConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Idbrand).HasName("PK__Brand__A8EBD9B7803ECB0D");

            entity.ToTable("Brand");

            entity.Property(e => e.Idbrand).HasColumnName("IDBrand");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PK__Category__1AA1EC66C01EC25B");

            entity.ToTable("Category");

            entity.Property(e => e.Idcategory).HasColumnName("IDCategory");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Colour>(entity =>
        {
            entity.HasKey(e => e.Idcolour).HasName("PK__Colour__004935CD858DC786");

            entity.ToTable("Colour");

            entity.Property(e => e.Idcolour).HasColumnName("IDColour");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Idinstructor).HasName("PK__Instruct__928E86C4125A0A2B");

            entity.ToTable("Instructor");

            entity.Property(e => e.Idinstructor).HasColumnName("IDInstructor");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Person).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__Perso__4222D4EF");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Idmodel).HasName("PK__Model__9C90CDAC9D0A09F7");

            entity.ToTable("Model");

            entity.Property(e => e.Idmodel).HasColumnName("IDModel");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.Models)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Model__BrandID__3B75D760");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Idperson).HasName("PK__Person__78E1C524C69E6035");

            entity.ToTable("Person");

            entity.Property(e => e.Idperson).HasColumnName("IDPerson");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PswSalt).HasMaxLength(255);
            entity.Property(e => e.PswdHash).HasMaxLength(255);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Idrequest).HasName("PK__Request__387A59D8CB131BB3");

            entity.ToTable("Request");

            entity.Property(e => e.Idrequest).HasColumnName("IDRequest");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Requests)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__Instruc__5629CD9C");

            entity.HasOne(d => d.State).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__StateID__571DF1D5");

            entity.HasOne(d => d.Student).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__Student__5535A963");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Idreview).HasName("PK__Review__E5003B6F380B00D0");

            entity.ToTable("Review");

            entity.Property(e => e.Idreview).HasColumnName("IDReview");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__Instruct__5070F446");

            entity.HasOne(d => d.Student).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__StudentI__4F7CD00D");
        });

        modelBuilder.Entity<Rezervation>(entity =>
        {
            entity.HasKey(e => e.Idrezervation).HasName("PK__Rezervat__1074A5E318667AB0");

            entity.ToTable("Rezervation");

            entity.Property(e => e.Idrezervation).HasColumnName("IDRezervation");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StateId)
                .HasDefaultValueSql("((3))")
                .HasColumnName("StateID");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Rezervations)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rezervati__Instr__5AEE82B9");

            entity.HasOne(d => d.State).WithMany(p => p.Rezervations)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rezervati__State__5BE2A6F2");

            entity.HasOne(d => d.Student).WithMany(p => p.Rezervations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rezervati__Stude__59FA5E80");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Idstate).HasName("PK__State__860530348B82C0D5");

            entity.ToTable("State");

            entity.Property(e => e.Idstate).HasColumnName("IDState");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Oib).HasName("PK__Student__CB394B3FAAB3E146");

            entity.ToTable("Student");

            entity.HasIndex(e => e.PersonId, "UQ__Student__AA2FFB847A6EFA04").IsUnique();

            entity.Property(e => e.Oib)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OIB");
            entity.Property(e => e.HoursDriven).HasDefaultValueSql("((0))");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Person).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__PersonI__45F365D3");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.IdtimeSlot).HasName("PK__TimeSlot__3D1FABF113FA1F30");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.IdtimeSlot).HasColumnName("IDTimeSlot");
            entity.Property(e => e.Done).HasDefaultValueSql("((0))");
            entity.Property(e => e.RezervationId).HasColumnName("RezervationID");

            entity.HasOne(d => d.Rezervation).WithMany(p => p.TimeSlots)
                .HasForeignKey(d => d.RezervationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TimeSlot__Rezerv__5FB337D6");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Idvehicle).HasName("PK__Vehicle__964C37C0DC0571AF");

            entity.ToTable("Vehicle");

            entity.Property(e => e.Idvehicle).HasColumnName("IDVehicle");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ColourId).HasColumnName("ColourID");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");

            entity.HasOne(d => d.Category).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__Categor__4AB81AF0");

            entity.HasOne(d => d.Colour).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ColourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__ColourI__49C3F6B7");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__Instruc__4CA06362");

            entity.HasOne(d => d.Model).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicle__ModelID__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
