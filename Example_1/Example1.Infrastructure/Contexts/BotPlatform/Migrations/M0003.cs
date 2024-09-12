using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Example1.Infrastructure.Contexts.BotPlatform.Migrations;

[DbContext(typeof(BotPlatformDbContext))]
[Migration("0003")]
public class M0003 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        const string sql = @"
ALTER TABLE `FilesBox` ADD CONSTRAINT `FK_FilesBox_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
ALTER TABLE `Verifications` ADD CONSTRAINT `FK_Verifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
";

        migrationBuilder.Sql(sql);
    }
}