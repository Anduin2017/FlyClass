using Anduin.FlyClass.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.MySql;

public class MySqlContext(DbContextOptions<MySqlContext> options) : FlyClassDbContext(options);
