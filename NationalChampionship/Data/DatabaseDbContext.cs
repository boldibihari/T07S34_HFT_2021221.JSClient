using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NationalChampionship.Data.Models;
using System;

namespace NationalChampionship.Data
{
    public class DatabaseDbContext : IdentityDbContext<User>
    {
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Stadium> Stadiums { get; set; }

        public DbSet<UserClub> UserClub { get; set; }

        public DatabaseDbContext()
        {
            Database.EnsureCreated();
        }

        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> opt)
            : base(opt)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.
                    UseLazyLoadingProxies().
                     UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NB1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            IdentityRole admin = new IdentityRole { Id = "341743f0-asd2–42de-afbf-59kmkkmk72cf6", Name = "Admin", NormalizedName = "ADMIN" };
            IdentityRole user = new IdentityRole { Id = "3341743f0-dee2–42de-bbbb-59kmkkmk72cf6", Name = "User", NormalizedName = "USER" };

            modelBuilder.Entity<IdentityRole>().HasData(admin, user);

            var boldi = new User
            {
                Id = "e2174cf0-9999-4cfe-afbf-59f706d72cf6",
                Email = "boldibihari@nik.uni-obuda.hu",
                NormalizedEmail = "BOLDIBIHARI@NIK.UNI-OBUDA.HU",
                EmailConfirmed = true,
                UserName = "boldibihari",
                NormalizedUserName = "BOLDIBIHARI",
                SecurityStamp = string.Empty,
                LastName = "Bihari",
                FirstName = "Boldi",
                FavouriteClubs = null,
                RoleId = admin.Id,
                Role = admin.Name
            };

            var andris = new User
            {
                Id = "e2174cf0-9412-4cfe-afbf-59f706d72cf7",
                Email = "kovacs.andras@nik.uni-obuda.hu",
                NormalizedEmail = "KOVACS.ANDRAS@NIK.UNI-OBUDA.HU",
                EmailConfirmed = true,
                UserName = "kovacs.andras",
                NormalizedUserName = "KOVACS.ANDRAS",
                SecurityStamp = string.Empty,
                LastName = "Kovács",
                FirstName = "András",
                FavouriteClubs = null,
                RoleId = user.Id,
                Role = user.Name
            };

            boldi.PasswordHash = new PasswordHasher<User>().HashPassword(null, "boldi");
            andris.PasswordHash = new PasswordHasher<User>().HashPassword(null, "andris");

            modelBuilder.Entity<User>().HasData(boldi);
            modelBuilder.Entity<User>().HasData(andris);

            // Admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = admin.Id,
                UserId = boldi.Id,
            });

            // User
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = user.Id,
                UserId = andris.Id,
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity
                .HasOne(player => player.Club)
                .WithMany(club => club.Players)
                .HasForeignKey(player => player.ClubId);
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity
                .HasOne(manager => manager.Club)
                .WithOne(club => club.Manager)
                .HasForeignKey<Manager>(manager => manager.ClubId);
            });

            modelBuilder.Entity<Stadium>(entity =>
            {
                entity
                .HasOne(stadium => stadium.Club)
                .WithOne(club => club.Stadium)
                .HasForeignKey<Stadium>(stadium => stadium.ClubId);
            });

            modelBuilder.Entity<UserClub>(entity =>
            {
                entity
                .HasKey(userClub => new { userClub.UserId, userClub.ClubId });
            });

            modelBuilder.Entity<UserClub>(entity =>
            {
                entity
                .HasOne(userClub => userClub.User)
                .WithMany(user => user.FavouriteClubs)
                .HasForeignKey(userClub => userClub.UserId);
            });

            modelBuilder.Entity<UserClub>(entity =>
            {
                entity
                .HasOne(userClub => userClub.Club)
                .WithMany(club => club.Users)
                .HasForeignKey(userClub => userClub.ClubId);
            });

            #region FillDatabase

            UserClub uc1 = new()
            {
                UserClubId = 1,
                UserId = "e2174cf0-9999-4cfe-afbf-59f706d72cf6",
                ClubId = 1925
            };

            UserClub uc2 = new()
            {
                UserClubId = 2,
                UserId = "e2174cf0-9999-4cfe-afbf-59f706d72cf6",
                ClubId = 1926
            };

            UserClub uc3 = new()
            {
                UserClubId = 3,
                UserId = "e2174cf0-9412-4cfe-afbf-59f706d72cf7",
                ClubId = 25596
            };

            UserClub uc4 = new()
            {
                UserClubId = 4,
                UserId = "e2174cf0-9412-4cfe-afbf-59f706d72cf7",
                ClubId = 25608
            };

            Club c1 = new()
            {
                ClubId = 1925,
                ClubName = "Ferencvárosi TC",
                ClubCity = "Budapest",
                ClubColour = "green-white",
                ClubFounded = 1899,
            };
            Club c2 = new()
            {
                ClubId = 1926,
                ClubName = "MOL Fehérvár FC",
                ClubCity = "Székesfehérvár",
                ClubColour = "red-blue",
                ClubFounded = 1914,
            };
            Club c3 = new()
            {
                ClubId = 25596,
                ClubName = "Puskás Akadémia FC",
                ClubCity = "Felcsút",
                ClubColour = "blue-yellow",
                ClubFounded = 2012,
            };
            Club c4 = new()
            {
                ClubId = 25608,
                ClubName = "Mezőkövesd Zsóry FC",
                ClubCity = "Mezőkövesd",
                ClubColour = "blue-yellow",
                ClubFounded = 1975,
            };
            Club c5 = new()
            {
                ClubId = 1916,
                ClubName = "Budapest Honvéd FC",
                ClubCity = "Budapest",
                ClubColour = "red-black",
                ClubFounded = 1909,
            };
            Club c6 = new()
            {
                ClubId = 1924,
                ClubName = "Újpest FC",
                ClubCity = "Budapest",
                ClubColour = "purple-white",
                ClubFounded = 1885,
            };
            Club c7 = new()
            {
                ClubId = 1921,
                ClubName = "Debreceni VSC",
                ClubCity = "Debrecen",
                ClubColour = "red-white",
                ClubFounded = 1902,
            };
            Club c8 = new()
            {
                ClubId = 6059,
                ClubName = "Paksi FC",
                ClubCity = "Paks",
                ClubColour = "green-white",
                ClubFounded = 1952,
            };
            Club c9 = new()
            {
                ClubId = 25397,
                ClubName = "Kisvárda FC",
                ClubCity = "Kisvárda",
                ClubColour = "red-white",
                ClubFounded = 1911,
            };
            Club c10 = new()
            {
                ClubId = 1922,
                ClubName = "Zalaegerszegi TE FC",
                ClubCity = "Zalaegerszeg",
                ClubColour = "blue-white",
                ClubFounded = 1920,
            };
            Club c11 = new()
            {
                ClubId = 1915,
                ClubName = "MTK Budapest FC",
                ClubCity = "Budapest",
                ClubColour = "blue-white",
                ClubFounded = 1888,
            };
            Club c12 = new()
            {
                ClubId = 8057,
                ClubName = "Gyirmót FC Győr",
                ClubCity = "Győr",
                ClubColour = "yellow-blue",
                ClubFounded = 1993,
            };

            Manager m1 = new()
            {
                ManagerId = 53308,
                ManagerName = "Stanislav Cherchesov",
                CountryCode = "ru",
                ManagerCountry = "Russia",
                ManagerBirthdate = new DateTime(1963, 09, 02),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c1.ClubId,
            };
            Manager m2 = new()
            {
                ManagerId = 70361,
                ManagerName = "Michael Boris",
                CountryCode = "de",
                ManagerCountry = "Germany",
                ManagerBirthdate = new DateTime(1975, 06, 03),
                ManagerStartYear = 2022,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c2.ClubId,
            };
            Manager m3 = new()
            {
                ManagerId = 23741,
                ManagerName = "Zsolt Hornyák",
                CountryCode = "sk",
                ManagerCountry = "Slovakia",
                ManagerBirthdate = new DateTime(1973, 05, 01),
                ManagerStartYear = 2019,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c3.ClubId,
            };
            Manager m4 = new()
            {
                ManagerId = 789805,
                ManagerName = "Attila Supka",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1962, 09, 19),
                ManagerStartYear = 2021,
                PreferredFormation = "4-4-2",
                WonChampionship = false,
                ClubId = c4.ClubId,
            };
            Manager m5 = new()
            {
                ManagerId = 788195,
                ManagerName = "Tamás Bódog",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1970, 09, 27),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c5.ClubId,
            };
            Manager m6 = new()
            {
                ManagerId = 788807,
                ManagerName = "Miloš Kruščić",
                CountryCode = "rs",
                ManagerCountry = "Serbia",
                ManagerBirthdate = new DateTime(1976, 10, 03),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c6.ClubId,
            };
            Manager m7 = new()
            {
                ManagerId = 796020,
                ManagerName = "Szabolcs Huszti",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1983, 04, 18),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c7.ClubId,
            };
            Manager m8 = new()
            {
                ManagerId = 792587,
                ManagerName = "György Bognár",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1961, 11, 05),
                ManagerStartYear = 2020,
                PreferredFormation = "3-5-2",
                WonChampionship = false,
                ClubId = c8.ClubId,
            };
            Manager m9 = new()
            {
                ManagerId = 796591,
                ManagerName = "Gábor Erős",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1980, 07, 01),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c9.ClubId,
            };
            Manager m10 = new()
            {
                ManagerId = 790483,
                ManagerName = "Róbert Waltner",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1977, 09, 20),
                ManagerStartYear = 2021,
                PreferredFormation = "4-2-3-1",
                WonChampionship = false,
                ClubId = c10.ClubId,
            };
            Manager m11 = new()
            {
                ManagerId = 787304,
                ManagerName = "Gábor Márton",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1966, 10, 15),
                ManagerStartYear = 2021,
                PreferredFormation = "4-1-4-1",
                WonChampionship = false,
                ClubId = c11.ClubId,
            };
            Manager m12 = new()
            {
                ManagerId = 65570,
                ManagerName = "Aurél Csertői",
                CountryCode = "hu",
                ManagerCountry = "Hungary",
                ManagerBirthdate = new DateTime(1965, 09, 25),
                ManagerStartYear = 2019,
                PreferredFormation = "4-3-1-2",
                WonChampionship = false,
                ClubId = c12.ClubId,
            };

            Stadium s1 = new()
            {
                StadiumId = 1,
                StadiumName = "Groupama Aréna",
                Completed = 2014,
                Capacity = 22000,
                Location = "Budapest, Üllői út 129, 1091",
                ClubId = c1.ClubId,
            };
            Stadium s2 = new()
            {
                StadiumId = 2,
                StadiumName = "MOL Aréna Sóstó",
                Completed = 2018,
                Capacity = 14201,
                Location = "Székesfehérvár, Csikvári út 10, 8000",
                ClubId = c2.ClubId,
            };
            Stadium s3 = new()
            {
                StadiumId = 3,
                StadiumName = "Pancho Aréna",
                Completed = 2014,
                Capacity = 3500,
                Location = "Felcsút, Fő út 176, 8086",
                ClubId = c3.ClubId,
            };
            Stadium s4 = new()
            {
                StadiumId = 4,
                StadiumName = "Mezőkövesdi Stadion",
                Completed = 2016,
                Capacity = 5020,
                Location = "Mezőkövesd, Széchenyi István út 56, 3400",
                ClubId = c4.ClubId,
            };
            Stadium s5 = new()
            {
                StadiumId = 5,
                StadiumName = "Bozsik Aréna",
                Completed = 2021,
                Capacity = 8200,
                Location = "Budapest, Puskás Ferenc út 1-3, 1194",
                ClubId = c5.ClubId,
            };
            Stadium s6 = new()
            {
                StadiumId = 6,
                StadiumName = "Szusza Ferenc Stadion",
                Completed = 1922,
                Capacity = 13501,
                Location = "Budapest, Megyeri út 13, 1044",
                ClubId = c6.ClubId,
            };
            Stadium s7 = new()
            {
                StadiumId = 7,
                StadiumName = "Nagyerdei Stadion",
                Completed = 2014,
                Capacity = 20340,
                Location = "Debrecen, Nagyerdei körút 12, 4032",
                ClubId = c7.ClubId,
            };
            Stadium s8 = new()
            {
                StadiumId = 8,
                StadiumName = "Fehérvári úti Stadon",
                Completed = 1966,
                Capacity = 4500,
                Location = " Paks, Fehérvári út 29, 7030",
                ClubId = c8.ClubId,
            };
            Stadium s9 = new()
            {
                StadiumId = 9,
                StadiumName = "Várkerti Stadion",
                Completed = 2018,
                Capacity = 3000,
                Location = "Kisvárda, Városmajor út, 4600",
                ClubId = c9.ClubId,
            };
            Stadium s10 = new()
            {
                StadiumId = 10,
                StadiumName = "ZTE-Aréna",
                Completed = 2002,
                Capacity = 11200,
                Location = "Zalaegerszeg, Október 6. tér 16, 8900",
                ClubId = c10.ClubId,
            };
            Stadium s11 = new()
            {
                StadiumId = 11,
                StadiumName = "Új Hidegkuti Nándor Stadion",
                Completed = 2016,
                Capacity = 5014,
                Location = "Budapest, Salgótarjáni út 12-14, 1087",
                ClubId = c11.ClubId,
            };
            Stadium s12 = new()
            {
                StadiumId = 12,
                StadiumName = "Alcufer Stadion",
                Completed = 2005,
                Capacity = 4500,
                Location = "Győr, Ménfői út 61, 9019",
                ClubId = c12.ClubId,
            };

            //Player p1 = new()
            //{
            //    PlayerId = 1,
            //    PlayerName = "Dénes Dibusz",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1990, 11, 16),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 2.20,
            //    ClubId = c1.ClubId,
            //};
            //Player p2 = new()
            //{
            //    PlayerId = 2,
            //    PlayerName = "Ádám Bogdán",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1987, 09, 27),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.350,
            //    ClubId = c1.ClubId,
            //};
            //Player p3 = new()
            //{
            //    PlayerId = 3,
            //    PlayerName = "Miha Blazic",
            //    PlayerNationality = "Slovenian",
            //    PlayerBirthDate = new DateTime(1993, 05, 08),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 2.00,
            //    ClubId = c1.ClubId,
            //};
            //Player p4 = new()
            //{
            //    PlayerId = 4,
            //    PlayerName = "Adnan Kovacevic",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1993, 09, 09),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 1.20,
            //    ClubId = c1.ClubId,
            //};
            //Player p5 = new()
            //{
            //    PlayerId = 5,
            //    PlayerName = "Eldar Civic",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1996, 05, 28),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 1.50,
            //    ClubId = c1.ClubId,
            //};
            //Player p6 = new()
            //{
            //    PlayerId = 6,
            //    PlayerName = "Endre Botka",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1994, 08, 25),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 1.00,
            //    ClubId = c1.ClubId,
            //};
            //Player p7 = new()
            //{
            //    PlayerId = 7,
            //    PlayerName = "Gergő Lovrencsics C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1988, 11, 01),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.800,
            //    ClubId = c1.ClubId,
            //};
            //Player p8 = new()
            //{
            //    PlayerId = 8,
            //    PlayerName = "Igor Kharatin",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1995, 02, 02),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 2.50,
            //    ClubId = c1.ClubId,
            //};
            //Player p9 = new()
            //{
            //    PlayerId = 9,
            //    PlayerName = "Somália",
            //    PlayerNationality = "Brazilian",
            //    PlayerBirthDate = new DateTime(1988, 09, 28),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 1.30,
            //    ClubId = c1.ClubId,
            //};
            //Player p10 = new()
            //{
            //    PlayerId = 10,
            //    PlayerName = "Dávid Sigér",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1990, 11, 30),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 1.00,
            //    ClubId = c1.ClubId,
            //};
            //Player p11 = new()
            //{
            //    PlayerId = 11,
            //    PlayerName = "Isael",
            //    PlayerNationality = "Brazilian",
            //    PlayerBirthDate = new DateTime(1988, 05, 13),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.50,
            //    ClubId = c1.ClubId,
            //};
            //Player p12 = new()
            //{
            //    PlayerId = 12,
            //    PlayerName = "Tokmac Nguen",
            //    PlayerNationality = "Norvegian",
            //    PlayerBirthDate = new DateTime(1993, 10, 20),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 2.80,
            //    ClubId = c1.ClubId,
            //};
            //Player p13 = new()
            //{
            //    PlayerId = 13,
            //    PlayerName = "Myrto Uzuni",
            //    PlayerNationality = "Albanian",
            //    PlayerBirthDate = new DateTime(1995, 05, 31),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 2.20,
            //    ClubId = c1.ClubId,
            //};
            //Player p14 = new()
            //{
            //    PlayerId = 14,
            //    PlayerName = "Oleksandr Zubkov",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1996, 08, 03),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 2.00,
            //    ClubId = c1.ClubId,
            //};
            //Player p15 = new()
            //{
            //    PlayerId = 15,
            //    PlayerName = "Franck Boli",
            //    PlayerNationality = "Ivorian",
            //    PlayerBirthDate = new DateTime(1993, 12, 07),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.00,
            //    ClubId = c1.ClubId,
            //};
            //Player p16 = new()
            //{
            //    PlayerId = 16,
            //    PlayerName = "Ádám Kovácsik",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1991, 04, 04),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.900,
            //    ClubId = c2.ClubId,
            //};
            //Player p17 = new()
            //{
            //    PlayerId = 17,
            //    PlayerName = "Emil Rockov",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1995, 01, 27),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.900,
            //    ClubId = c2.ClubId,
            //};
            //Player p18 = new()
            //{
            //    PlayerId = 18,
            //    PlayerName = "Visar Musliu",
            //    PlayerNationality = "Northern Macedonia",
            //    PlayerBirthDate = new DateTime(1994, 11, 13),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 1.00,
            //    ClubId = c2.ClubId,
            //};
            //Player p19 = new()
            //{
            //    PlayerId = 19,
            //    PlayerName = "Stopira",
            //    PlayerNationality = "Cape Verde",
            //    PlayerBirthDate = new DateTime(1988, 05, 20),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.500,
            //    ClubId = c2.ClubId,
            //};
            //Player p20 = new()
            //{
            //    PlayerId = 20,
            //    PlayerName = "Hangya Szilveszter",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1994, 01, 02),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.450,
            //    ClubId = c2.ClubId,
            //};
            //Player p21 = new()
            //{
            //    PlayerId = 21,
            //    PlayerName = "Loic Nego",
            //    PlayerNationality = "French",
            //    PlayerBirthDate = new DateTime(1991, 01, 15),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 2.20,
            //    ClubId = c2.ClubId,
            //};
            //Player p22 = new()
            //{
            //    PlayerId = 22,
            //    PlayerName = "Bendegúz Bolla",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1999, 11, 22),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c2.ClubId,
            //};
            //Player p23 = new()
            //{
            //    PlayerId = 23,
            //    PlayerName = "Rúben Pinto",
            //    PlayerNationality = "Portuguese",
            //    PlayerBirthDate = new DateTime(1992, 04, 24),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 1.00,
            //    ClubId = c2.ClubId,
            //};
            //Player p24 = new()
            //{
            //    PlayerId = 24,
            //    PlayerName = "Lyes Houri",
            //    PlayerNationality = "French",
            //    PlayerBirthDate = new DateTime(1996, 01, 19),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 1.10,
            //    ClubId = c2.ClubId,
            //};
            //Player p25 = new()
            //{
            //    PlayerId = 25,
            //    PlayerName = "Boban Nikolov",
            //    PlayerNationality = "Northern Macedonia",
            //    PlayerBirthDate = new DateTime(1994, 07, 28),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.750,
            //    ClubId = c2.ClubId,
            //};
            //Player p26 = new()
            //{
            //    PlayerId = 26,
            //    PlayerName = "István Kovács",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1992, 03, 27),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.550,
            //    ClubId = c2.ClubId,
            //};
            //Player p27 = new()
            //{
            //    PlayerId = 27,
            //    PlayerName = "Ivan Petryak",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1994, 03, 13),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.60,
            //    ClubId = c2.ClubId,
            //};
            //Player p28 = new()
            //{
            //    PlayerId = 28,
            //    PlayerName = "Evandro",
            //    PlayerNationality = "Brazilian",
            //    PlayerBirthDate = new DateTime(1997, 01, 14),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.20,
            //    ClubId = c2.ClubId,
            //};
            //Player p29 = new()
            //{
            //    PlayerId = 29,
            //    PlayerName = "Nemanja Nikolics C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1987, 12, 31),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.50,
            //    ClubId = c2.ClubId,
            //};
            //Player p30 = new()
            //{
            //    PlayerId = 30,
            //    PlayerName = "Armin Hodzic",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1994, 11, 17),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.00,
            //    ClubId = c2.ClubId,
            //};
            //Player p31 = new()
            //{
            //    PlayerId = 31,
            //    PlayerName = "Balázs Tóth",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1997, 09, 04),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.250,
            //    ClubId = c3.ClubId,
            //};
            //Player p32 = new()
            //{
            //    PlayerId = 32,
            //    PlayerName = "Ágoston Kiss",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(2001, 03, 14),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.100,
            //    ClubId = c3.ClubId,
            //};
            //Player p33 = new()
            //{
            //    PlayerId = 33,
            //    PlayerName = "Thomas Meißner",
            //    PlayerNationality = "German",
            //    PlayerBirthDate = new DateTime(1991, 03, 26),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.600,
            //    ClubId = c3.ClubId,
            //};
            //Player p34 = new()
            //{
            //    PlayerId = 34,
            //    PlayerName = "João Nunes",
            //    PlayerNationality = "Portuguese",
            //    PlayerBirthDate = new DateTime(1995, 11, 19),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c3.ClubId,
            //};
            //Player p35 = new()
            //{
            //    PlayerId = 35,
            //    PlayerName = "Kamen Hadzhiev",
            //    PlayerNationality = "Bulgarian",
            //    PlayerBirthDate = new DateTime(1991, 09, 22),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c3.ClubId,
            //};
            //Player p36 = new()
            //{
            //    PlayerId = 36,
            //    PlayerName = "Zsolt Nagy",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1993, 05, 25),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.300,
            //    ClubId = c3.ClubId,
            //};
            //Player p37 = new()
            //{
            //    PlayerId = 37,
            //    PlayerName = "Roland Szolnoki C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1992, 01, 21),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.300,
            //    ClubId = c3.ClubId,
            //};
            //Player p38 = new()
            //{
            //    PlayerId = 38,
            //    PlayerName = "Yoëll van Nieff",
            //    PlayerNationality = "Dutch",
            //    PlayerBirthDate = new DateTime(1993, 06, 17),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.600,
            //    ClubId = c3.ClubId,
            //};
            //Player p39 = new()
            //{
            //    PlayerId = 39,
            //    PlayerName = "Jakub Plsek",
            //    PlayerNationality = "Czech",
            //    PlayerBirthDate = new DateTime(1993, 12, 13),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.500,
            //    ClubId = c3.ClubId,
            //};
            //Player p40 = new()
            //{
            //    PlayerId = 40,
            //    PlayerName = "Jozef Urblik",
            //    PlayerNationality = "Slovakian",
            //    PlayerBirthDate = new DateTime(1996, 08, 22),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.400,
            //    ClubId = c3.ClubId,
            //};
            //Player p41 = new()
            //{
            //    PlayerId = 41,
            //    PlayerName = "Josip Knezevic",
            //    PlayerNationality = "Croatian",
            //    PlayerBirthDate = new DateTime(1988, 10, 03),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.500,
            //    ClubId = c3.ClubId,
            //};
            //Player p42 = new()
            //{
            //    PlayerId = 42,
            //    PlayerName = "Tamás Kiss",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(2000, 11, 24),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.400,
            //    ClubId = c3.ClubId,
            //};
            //Player p43 = new()
            //{
            //    PlayerId = 43,
            //    PlayerName = "Alexandru Baluta",
            //    PlayerNationality = "Romanian",
            //    PlayerBirthDate = new DateTime(1993, 09, 13),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 2.00,
            //    ClubId = c3.ClubId,
            //};
            //Player p44 = new()
            //{
            //    PlayerId = 44,
            //    PlayerName = "Antonio Mance",
            //    PlayerNationality = "Croatian",
            //    PlayerBirthDate = new DateTime(1995, 08, 07),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.30,
            //    ClubId = c3.ClubId,
            //};
            //Player p45 = new()
            //{
            //    PlayerId = 45,
            //    PlayerName = "David Vanecek",
            //    PlayerNationality = "Czech",
            //    PlayerBirthDate = new DateTime(1991, 03, 09),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.650,
            //    ClubId = c3.ClubId,
            //};
            //Player p46 = new()
            //{
            //    PlayerId = 46,
            //    PlayerName = "Péter Szappanos",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1990, 11, 14),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.350,
            //    ClubId = c4.ClubId,
            //};
            //Player p47 = new()
            //{
            //    PlayerId = 47,
            //    PlayerName = "Danylo Ryabenko",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1998, 10, 09),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.100,
            //    ClubId = c4.ClubId,
            //};
            //Player p48 = new()
            //{
            //    PlayerId = 48,
            //    PlayerName = "Andriy Nesterov",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1990, 07, 02),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.350,
            //    ClubId = c4.ClubId,
            //};
            //Player p49 = new()
            //{
            //    PlayerId = 49,
            //    PlayerName = "Róbert Pillár",
            //    PlayerNationality = "Slovakian",
            //    PlayerBirthDate = new DateTime(1991, 05, 27),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.300,
            //    ClubId = c4.ClubId,
            //};
            //Player p50 = new()
            //{
            //    PlayerId = 50,
            //    PlayerName = "Richárd Guzmics",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1987, 04, 16),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.200,
            //    ClubId = c4.ClubId,
            //};
            //Player p51 = new()
            //{
            //    PlayerId = 51,
            //    PlayerName = "Luka Lakvekheliani",
            //    PlayerNationality = "Georgian",
            //    PlayerBirthDate = new DateTime(1998, 10, 20),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.450,
            //    ClubId = c4.ClubId,
            //};
            //Player p52 = new()
            //{
            //    PlayerId = 52,
            //    PlayerName = "Dániel Farkas",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1993, 01, 13),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.300,
            //    ClubId = c4.ClubId,
            //};
            //Player p53 = new()
            //{
            //    PlayerId = 53,
            //    PlayerName = "Aleksandr Karnitskiy",
            //    PlayerNationality = "Belarusian",
            //    PlayerBirthDate = new DateTime(1989, 02, 14),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.400,
            //    ClubId = c4.ClubId,
            //};
            //Player p54 = new()
            //{
            //    PlayerId = 54,
            //    PlayerName = "Zsombor Berecz",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1995, 12, 13),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.550,
            //    ClubId = c4.ClubId,
            //};
            //Player p55 = new()
            //{
            //    PlayerId = 55,
            //    PlayerName = "Dino Berisovic",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1994, 01, 31),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.400,
            //    ClubId = c4.ClubId,
            //};
            //Player p56 = new()
            //{
            //    PlayerId = 56,
            //    PlayerName = "Antonio Vutov",
            //    PlayerNationality = "Bulgarian",
            //    PlayerBirthDate = new DateTime(1996, 06, 06),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.600,
            //    ClubId = c4.ClubId,
            //};
            //Player p57 = new()
            //{
            //    PlayerId = 57,
            //    PlayerName = "Tamás Cseri C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1988, 01, 15),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.400,
            //    ClubId = c4.ClubId,
            //};
            //Player p58 = new()
            //{
            //    PlayerId = 58,
            //    PlayerName = "Serder Serderov",
            //    PlayerNationality = "Russian",
            //    PlayerBirthDate = new DateTime(1994, 03, 10),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.600,
            //    ClubId = c4.ClubId,
            //};
            //Player p59 = new()
            //{
            //    PlayerId = 59,
            //    PlayerName = "Andriy Boryachuk",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1996, 04, 23),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.450,
            //    ClubId = c4.ClubId,
            //};
            //Player p60 = new()
            //{
            //    PlayerId = 60,
            //    PlayerName = "Marin Jurina",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1993, 11, 26),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.300,
            //    ClubId = c4.ClubId,
            //};
            //Player p61 = new()
            //{
            //    PlayerId = 61,
            //    PlayerName = "Tomas Tujvel",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1983, 09, 19),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.150,
            //    ClubId = c5.ClubId,
            //};
            //Player p62 = new()
            //{
            //    PlayerId = 62,
            //    PlayerName = "Oleksandr Nad",
            //    PlayerNationality = "Ukrainian",
            //    PlayerBirthDate = new DateTime(1985, 09, 02),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.150,
            //    ClubId = c5.ClubId,
            //};
            //Player p63 = new()
            //{
            //    PlayerId = 63,
            //    PlayerName = "Botond Baráth",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1992, 04, 21),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.500,
            //    ClubId = c5.ClubId,
            //};
            //Player p64 = new()
            //{
            //    PlayerId = 64,
            //    PlayerName = "Bence Batik",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1993, 11, 08),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c5.ClubId,
            //};
            //Player p65 = new()
            //{
            //    PlayerId = 65,
            //    PlayerName = "Djordje Kamber",
            //    PlayerNationality = "Bosniaks",
            //    PlayerBirthDate = new DateTime(1983, 11, 20),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.100,
            //    ClubId = c5.ClubId,
            //};
            //Player p66 = new()
            //{
            //    PlayerId = 66,
            //    PlayerName = "Krisztián Tamás",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1995, 04, 18),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c5.ClubId,
            //};
            //Player p67 = new()
            //{
            //    PlayerId = 67,
            //    PlayerName = "Eke Uzoma",
            //    PlayerNationality = "Nigerian",
            //    PlayerBirthDate = new DateTime(1989, 08, 11),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.300,
            //    ClubId = c5.ClubId,
            //};
            //Player p68 = new()
            //{
            //    PlayerId = 68,
            //    PlayerName = "Mohamed Mezghrani",
            //    PlayerNationality = "Algerian",
            //    PlayerBirthDate = new DateTime(1994, 06, 02),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.350,
            //    ClubId = c5.ClubId,
            //};
            //Player p69 = new()
            //{
            //    PlayerId = 69,
            //    PlayerName = "Patrik Hidi C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1990, 11, 27),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.500,
            //    ClubId = c5.ClubId,
            //};
            //Player p70 = new()
            //{
            //    PlayerId = 70,
            //    PlayerName = "Dániel Gazdag",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1996, 03, 02),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 1.00,
            //    ClubId = c5.ClubId,
            //};
            //Player p71 = new()
            //{
            //    PlayerId = 71,
            //    PlayerName = "Gergő Nagy",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1993, 01, 07),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.400,
            //    ClubId = c5.ClubId,
            //};
            //Player p72 = new()
            //{
            //    PlayerId = 72,
            //    PlayerName = "Donát Zsótér",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1996, 01, 06),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.450,
            //    ClubId = c5.ClubId,
            //};
            //Player p73 = new()
            //{
            //    PlayerId = 73,
            //    PlayerName = "Márton Eppel",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1991, 10, 26),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 1.40,
            //    ClubId = c5.ClubId,
            //};
            //Player p74 = new()
            //{
            //    PlayerId = 74,
            //    PlayerName = "Norbert Balogh",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1996, 02, 21),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.450,
            //    ClubId = c5.ClubId,
            //};
            //Player p75 = new()
            //{
            //    PlayerId = 75,
            //    PlayerName = "Roland Ugrai",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1992, 11, 13),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.450,
            //    ClubId = c5.ClubId,
            //};
            //Player p76 = new()
            //{
            //    PlayerId = 76,
            //    PlayerName = "Filip Pajovic",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1993, 07, 30),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.200,
            //    ClubId = c6.ClubId,
            //};
            //Player p77 = new()
            //{
            //    PlayerId = 77,
            //    PlayerName = "Dávid Banai",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1994, 05, 09),
            //    PlayerPosition = PlayerPosition.Goalkeeper,
            //    PlayerValue = 0.250,
            //    ClubId = c6.ClubId,
            //};
            //Player p78 = new()
            //{
            //    PlayerId = 78,
            //    PlayerName = "Kire Ristevski",
            //    PlayerNationality = "Northern Macedonia",
            //    PlayerBirthDate = new DateTime(1990, 10, 22),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c6.ClubId,
            //};
            //Player p79 = new()
            //{
            //    PlayerId = 79,
            //    PlayerName = "Georgios Koutroumpis",
            //    PlayerNationality = "Greek",
            //    PlayerBirthDate = new DateTime(1991, 02, 10),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.400,
            //    ClubId = c6.ClubId,
            //};
            //Player p80 = new()
            //{
            //    PlayerId = 80,
            //    PlayerName = "Zsolt Máté",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1997, 09, 14),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.250,
            //    ClubId = c6.ClubId,
            //};
            //Player p81 = new()
            //{
            //    PlayerId = 81,
            //    PlayerName = "Nemanja Antonov",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1995, 05, 06),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.550,
            //    ClubId = c6.ClubId,
            //};
            //Player p82 = new()
            //{
            //    PlayerId = 82,
            //    PlayerName = "Branko Pauljevic",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1989, 06, 12),
            //    PlayerPosition = PlayerPosition.Defender,
            //    PlayerValue = 0.200,
            //    ClubId = c6.ClubId,
            //};
            //Player p83 = new()
            //{
            //    PlayerId = 83,
            //    PlayerName = "Miroslav Bjelos",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1990, 10, 29),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.350,
            //    ClubId = c6.ClubId,
            //};
            //Player p84 = new()
            //{
            //    PlayerId = 84,
            //    PlayerName = "Vincent Onovo",
            //    PlayerNationality = "Nigerian",
            //    PlayerBirthDate = new DateTime(1995, 12, 10),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.350,
            //    ClubId = c6.ClubId,
            //};
            //Player p85 = new()
            //{
            //    PlayerId = 85,
            //    PlayerName = "Nikola Mitrovic",
            //    PlayerNationality = "Serbian",
            //    PlayerBirthDate = new DateTime(1987, 01, 02),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.250,
            //    ClubId = c6.ClubId,
            //};
            //Player p86 = new()
            //{
            //    PlayerId = 86,
            //    PlayerName = "Barnabás Rázc",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1996, 04, 26),
            //    PlayerPosition = PlayerPosition.Midfielder,
            //    PlayerValue = 0.200,
            //    ClubId = c6.ClubId,
            //};
            //Player p87 = new()
            //{
            //    PlayerId = 87,
            //    PlayerName = "Giorgi Beridze",
            //    PlayerNationality = "Georgian",
            //    PlayerBirthDate = new DateTime(1997, 05, 12),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.400,
            //    ClubId = c6.ClubId,
            //};
            //Player p88 = new()
            //{
            //    PlayerId = 88,
            //    PlayerName = "Zoltán Stieber",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1988, 10, 16),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.400,
            //    ClubId = c6.ClubId,
            //};
            //Player p89 = new()
            //{
            //    PlayerId = 89,
            //    PlayerName = "Krisztián Simon C",
            //    PlayerNationality = "Hungarian",
            //    PlayerBirthDate = new DateTime(1991, 06, 10),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.400,
            //    ClubId = c6.ClubId,
            //};
            //Player p90 = new()
            //{
            //    PlayerId = 90,
            //    PlayerName = "Junior Tallo",
            //    PlayerNationality = "Ivorian",
            //    PlayerBirthDate = new DateTime(1992, 12, 21),
            //    PlayerPosition = PlayerPosition.Forward,
            //    PlayerValue = 0.300,
            //    ClubId = c6.ClubId,
            //};

            modelBuilder.Entity<Club>().HasData(c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12);
            modelBuilder.Entity<Manager>().HasData(m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12);
            modelBuilder.Entity<Stadium>().HasData(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12);
            modelBuilder.Entity<UserClub>().HasData(uc1, uc2, uc3, uc4);
            //modelBuilder.Entity<Player>().HasData(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32, p33, p34, p35, p36, p37, p38, p39, p40, p41, p42, p43, p44, p45, p46, p47, p48, p49, p50, p51, p52, p53, p54, p55, p56, p57, p58, p59, p60, p61, p62, p63, p64, p65, p66, p67, p68, p69, p70, p71, p72, p73, p74, p75, p76, p77, p78, p79, p80, p81, p82, p83, p84, p85, p86, p87, p88, p89, p90);
            #endregion
        }
    }
}
